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

    static public bool editMode;

    static public int [] matchTypes;

    static int [] NumberOfButtons = new int [] { 5, 3, 5, 9, 8 };
    static int [] Selected = new int [] {0, 1, 1, 1, 0 };
    static GameObject [] [] Buttons;

    override public void DestroyThis () {
        EditedBoard.DestroyAllVisuals ();
    }

    private void Awake () {
        instance = this;
        CurrentGOUI = this;
    }

    public void Update () {
        if (Input.GetKey ("p")) {
            for (int x = 1; x <= 4; x++) {
                if (Input.GetKeyDown (x.ToString ())) {
                    TestPuzzle (x);
                }
            }
        }
        
    }

    static public void TestPuzzle (int numberOfTurns) {
        ClientInterface client = ClientLogic.MyInterface;
        int id = GameModeEditor.gameModeId;
        CardPoolClass cardPool = new CardPoolClass ();
        cardPool.LoadFromString (ServerData.GetCardPool (id));

        HandClass hand1 = new HandClass ();
        for (int x = 0; x < 4; x++) {
            hand1.stack [x].Add (cardPool.Card [x]);
        }
        string accountName = client.AccountName;
        int gameMode = id;
        HandClass hand2 = new HandClass ();
        AIClass AI2 = new AIClass ();
        hand2.GenerateRandomHand (gameMode, AI2);
        MatchClass match = MatchMakingClass.CreateGame (gameMode, 1, new PlayerPropertiesClass [] {
            new PlayerPropertiesClass (1, null, client.AccountName, client.UserName, hand1, client),
            new PlayerPropertiesClass (2, AI2, "AI opponent", ServerData.GetGameModeName (id), hand2, null) });
        PlayerClass player = match.Player [2];
        PlayerPropertiesClass properties = player.properties;
        match.properties.turnLimit = numberOfTurns;

        match.Board = new BoardClass (match);
        match.Board.LoadFromFile (currentId);
        player.enabled = false;
        properties.AI.puzzle = true;
        ServerLogic.StartMatch (match);
    }

    static public void LoadDataToEditor (int id, bool isClientOwner, string boardName, string [] board, int [] matchTypes) {
        DestroyMenu ();
        currentId = id;
        editMode = isClientOwner;
        CurrentCanvas.AddComponent<BoardEditorMenu> ();

        EnvironmentScript.CreateNewBackground (1);
        CreateBoardEditorMenu ();
        CameraScript.SetBoardEditorCamera ();

        EditedBoard = new BoardClass ();
        EditedBoard.EnableVisualisation ();
        EditedBoard.CreateNewBoard ();

        BoardEditorMenu.boardName = boardName;
        EditedBoard.LoadFromString (board);
        BoardEditorMenu.matchTypes = matchTypes;
    }

    static public void SaveBoard () {
        ClientLogic.MyInterface.CmdSaveBoard (currentId, EditedBoard.BoardToString ());
    }

    static public void SaveMatchTypes (int [] newMatchTypes) {
        matchTypes = newMatchTypes;
        ClientLogic.MyInterface.CmdSaveBoardMatchTypes (currentId, matchTypes);
    }

    static public void ShowBoardEditorMenu () {
        ShowBoardEditorMenu (currentId);
    }

    static public void ShowBoardEditorMenu (int id) {
        currentId = id;
        ClientLogic.MyInterface.CmdDownloadBoard (currentId);
        //EnvironmentScript.CreateRandomBoard ();
    }

    static public void TileAction (int x, int y) {
        if (!editMode) {
            return;
        }
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
            EditedBoard.DestroyRemains (x, y);
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

        NumberOfButtons [4] = AppDefaults.availableTokens;

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

        if (editMode) {
            py += 120 + dy;

            AddButtons (px, py, maxX, maxY);

            py += 120 + dy;

            AddButtons (px, py, maxX, maxY);

            maxY += 1;

            py += 150 + dy;

            AddButtons (px, py, maxX, maxY);

            maxX = (AppDefaults.availableTokens - 1) / 3 + 1;
            maxY += 1;

            py += 210 + dy;

            AddButtons (px + 30 * maxX - 150, py, maxX, maxY);
        }

        for (int x = 1; x < 5; x++) {
            SelectButton (x, Selected [x]);
        }


    }

    static public void SelectButton (int type, int number) {
        Selected [type] = number;
        if (Buttons [type] != null) {
            foreach (GameObject button in Buttons [type]) {
                if (button != null) {
                    button.GetComponent<UIController> ().FreeAndUnlcok ();
                }
            }
            if (Buttons [type] [number]) {
                    Buttons [type] [number].GetComponent<UIController> ().PressAndLock ();
            }
        }
    }

    static int type = 0;

    static void AddButtons (int px, int py, int maxX, int maxY) {

        GameObject Clone;
        VisualToken VT;

        int sx = 90 + 60 * maxX;
        int sy = 140 + 60 * maxY;
        int dy = 70;

        BackgroundObject = CreateBackgroundSprite ("UI/Panel_Window_01_Sliced", px, py, 10, sx, sy);
        switch (type) {

            case 0:
                Clone = CreateText (Language.File, px, py - sy / 2 + dy, 12, 0.02f);
                break;
            case 1:
                Clone = CreateText (Language.TileFilling, px, py - sy / 2 + dy, 12, 0.02f);
                break;
            case 2:
                Clone = CreateText (Language.TokenOwner, px, py - sy / 2 + dy, 12, 0.02f);
                break;
            case 3:
                Clone = CreateText (Language.TokenValue, px, py - sy / 2 + dy, 12, 0.02f);
                break;
            case 4:
                Clone = CreateText (Language.TokenType, px, py - sy / 2 + dy, 12, 0.02f);
                break;
        }

        for (int y = 0; y < maxY; y++) {
            for (int x = 0; x < maxX; x++) {
                int number = x + y * maxX;
                if (!editMode && number < 2) {
                    continue;
                }
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
                            BackgroundObject = CreateSprite ("UI/Butt_S_Settings", npx, npy, 11, 60, 60, true);
                            BackgroundObject.name = UIString.BoardEditorSettings;
                            break;
                        case 3:
                            BackgroundObject = CreateSprite ("UI/Butt_S_Help", npx, npy, 11, 60, 60, true);
                            BackgroundObject.name = UIString.BoardEditorAbout;
                            break;
                        case 4:
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
