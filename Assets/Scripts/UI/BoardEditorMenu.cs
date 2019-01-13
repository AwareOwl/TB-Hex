using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardEditorMenu : GOUI {

    static public BoardEditorMenu instance;

    static public BoardClass EditedBoard;

    static public GameObject BackgroundObject;

    static int currentId = -1;

    static string boardName;

    static public int TileType;
    static public int Owner;
    static public int Value;
    static public int TokenType;

    static int [] NumberOfButtons = new int [] { 4, 3, 5, 9, 8 };
    static int [] Selected = new int [] {0, 1, 1, 1, 0 };
    static GameObject [] [] Buttons;

    override public void DestroyThis () {
        EditedBoard.DestroyAllVisuals ();
    }

    private void Start () {
        instance = this;
        EnvironmentScript.CreateNewBackground (1);
        CreateBoardEditorMenu ();
        CameraScript.SetBoardEditorCamera ();
        CurrentGUI = this;

        EditedBoard = new BoardClass ();
        EditedBoard.EnableVisualisation ();
        EditedBoard.CreateNewBoard ();
        ClientLogic.MyInterface.CmdDownloadBoard (currentId);
    }

    static public void LoadDataToEditor (int id, string boardName, string [] board) {
        currentId = id;
        BoardEditorMenu.boardName = boardName;
        EditedBoard.LoadFromString (board);
    }

    static public void SaveBoard () {
        ClientLogic.MyInterface.CmdSaveBoard (currentId, EditedBoard.BoardToString ());
    }

    static public void ShowBoardEditorMenu (int id) {
        DestroyMenu ();
        currentId = id;
        CurrentCanvas.AddComponent<BoardEditorMenu> ();
        //EnvironmentScript.CreateRandomBoard ();
    }

    static public void TileAction (int x, int y) {
        EnableTile (x, y);
        SetToken (x, y);
    }

    static public void EnableTile (int x, int y) {
        if (Selected [1] > 0) {
            EditedBoard.EnableTile (x, y, true);
        } else if (Selected [1] == 0) {
            EditedBoard.EnableTile (x, y, false);
        }
    }

    static public void SetToken (int x, int y) {
        if (Selected [1] == 2) {
            EditedBoard.SetToken (x, y, Selected [4], Selected [3] + 1, Selected [2]);
        } else {
            EditedBoard.DestroyToken (x, y);
        }
    }


    static public void ApplyProperties (string name, int iconNumber) {
        ClientLogic.MyInterface.CmdSaveBoardProperties (currentId, name, iconNumber);
    }
    override public void ShowPropertiesMenu () {
        PropertiesMenu.ShowSetPropertiesMenu (PropertiesMenu.boardPropertiesKey, boardName, 0);
    }

    static public void CreateBoardEditorMenu () {
        type = 0;

        NumberOfButtons [4] = AppDefaults.AvailableTokens;

        Buttons = new GameObject [NumberOfButtons.Length] [];
        for (int x = 0; x < NumberOfButtons.Length; x++) {
            Buttons [x] = new GameObject [NumberOfButtons [x]];
        }

        int sx = 390;
        int sy = 150;

        int px = sx / 2;
        int py = sy / 2 + 30;

        int maxX = 5;
        int maxY = 1;

        int dy = 50;
     

        AddButtons (px, py, maxX, maxY);

        py += 120 + dy;

        AddButtons (px, py, maxX, maxY);

        py += 120 + dy;

        AddButtons (px, py, maxX, maxY);

        maxY += 1;

        py += 150 + dy;

        AddButtons (px, py, maxX, maxY);

        maxY += 1;

        py += 210 + dy;

        AddButtons (px, py, maxX, maxY);

        for (int x = 1; x < 5; x++) {
            SelectButton (x, Selected [x]);
        }


    }

    static public void SelectButton (int type, int number) {
        Selected [type] = number;
        foreach (GameObject button in Buttons [type]) {
            button.GetComponent<UIController> ().FreeAndUnlcok ();
        }
        Buttons [type] [number].GetComponent<UIController> ().PressAndLock ();
    }

    static int type = 0;

    static void AddButtons (int px, int py, int maxX, int maxY) {

        GameObject Clone;
        VisualToken VT;

        int sx = 90 + 60 * maxX;
        int sy = 140 + 60 * maxY;
        int dy = 70;

        BackgroundObject = CreateSprite ("UI/Panel_Window_01_Sliced", px, py, 10, sx, sy, false);
        switch (type) {

            case 0:
                Clone = CreateText ("File", px, py - sy / 2 + dy, 12, 0.02f);
                break;
            case 1:
                Clone = CreateText ("Tile filling", px, py - sy / 2 + dy, 12, 0.02f);
                break;
            case 2:
                Clone = CreateText ("Owner", px, py - sy / 2 + dy, 12, 0.02f);
                break;
            case 3:
                Clone = CreateText ("Token value", px, py - sy / 2 + dy, 12, 0.02f);
                break;
            case 4:
                Clone = CreateText ("Token type", px, py - sy / 2 + dy, 12, 0.02f);
                break;
        }

        for (int y = 0; y < maxY; y++) {
            for (int x = 0; x < maxX; x++) {
                int number = x + y * maxX;
                if (number >= NumberOfButtons [type]) {
                    continue;
                }
                int npx = px + x * 60 + 75 - sx / 2;
                int npy = py + y * 60 + 125 - sy / 2;
                if (type == 0) {
                    switch (x) {
                        case 0:
                            BackgroundObject = CreateSprite ("UI/Butt_M_Apply", npx, npy, 11, 60, 60, true);
                            BackgroundObject.name = UIString.SaveBoard;
                            break;
                        case 1:
                            BackgroundObject = CreateSprite ("UI/Butt_S_Name", npx, npy, 11, 60, 60, true);
                            BackgroundObject.name = UIString.ChangeBoardName;
                            //BackgroundObject.name = "LoadBoard";
                            break;
                        case 2:
                            BackgroundObject = CreateSprite ("UI/Butt_S_Help", npx, npy, 11, 60, 60, true);
                            BackgroundObject.name = UIString.BoardEditorAbout;
                            break;
                        case 3:
                            BackgroundObject = CreateSprite ("UI/Butt_M_Discard", npx, npy, 11, 60, 60, true);
                            BackgroundObject.name = UIString.GoBackToGameModeEditor;
                            break;
                    }
                }
                if (type > 0) {
                    BackgroundObject = CreateSprite ("UI/Butt_M_EmptySquare", npx, npy, 11, 60, 60, true);
                    switch (type) {
                        case 1:
                            BackgroundObject.name = "BoardEditorTileType";
                            GameObject tile = Instantiate (AppDefaults.Tile) as GameObject;
                            tile.transform.SetParent (BackgroundObject.transform);
                            tile.transform.localEulerAngles = new Vector3 (-90, 0, 0);
                            tile.transform.localPosition = Vector3.zero;
                            tile.transform.localScale = new Vector3 (0.4f, 0.2f, 0.4f);
                            switch (x) {
                                case 0:
                                    tile.transform.Find ("Tile").gameObject.AddComponent<VisualEffectScript> ().SetColor (new Color (0.2f, 0.2f, 0.2f));
                                    break;
                                case 1:
                                    tile.transform.Find ("Tile").gameObject.AddComponent<VisualEffectScript> ().SetColor (new Color (0.6f, 0.6f, 0.6f));
                                    break;
                                case 2:
                                    tile.transform.Find ("Tile").gameObject.AddComponent<VisualEffectScript> ().SetColor (new Color (0.6f, 0.6f, 0.6f));
                                    VT = new VisualToken ();
                                    Clone = VT.Anchor;
                                    Clone.transform.SetParent (tile.transform);
                                    Clone.transform.localEulerAngles = new Vector3 (0, 0, 0);
                                    Clone.transform.localPosition = /*Vector3.zero;*/ new Vector3 (0, 0.3f, 0);
                                    Clone.transform.localScale = Vector3.one;
                                    DestroyImmediate (VT.Text);
                                    break;
                            }
                            break;
                        case 2:
                            BackgroundObject.name = "BoardEditorOwner";
                            Clone = CreateSprite ("UI/White", npx, npy, 12, 30, 30, false);
                            Clone.GetComponent<SpriteRenderer> ().color = AppDefaults.PlayerColor [x];
                            Destroy (Clone.GetComponent<Collider> ());
                            break;
                        case 3:
                            BackgroundObject.name = "BoardEditorValue";
                            Clone = CreateText ((number + 1).ToString(), npx, npy, 12, 0.03f);
                            AddTextToGameObject (BackgroundObject, Clone);
                            break;
                        case 4:
                            BackgroundObject.GetComponent<UIController> ().tokenType = number;
                            BackgroundObject.name = "BoardEditorTokenType";
                            VT = new VisualToken ();
                            Clone = VT.Anchor;
                            Clone.transform.SetParent (BackgroundObject.transform);
                            Clone.transform.localEulerAngles = new Vector3 (-90, 0, 0);
                            Clone.transform.localPosition = Vector3.zero;
                            Clone.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
                            VT.SetType (number);
                            DestroyImmediate (VT.Text);
                            break;
                    }

                }
                BackgroundObject.GetComponent<UIController> ().number = number;
                Buttons [type] [number] = BackgroundObject;

            }
        }
        type++;
    }
}
