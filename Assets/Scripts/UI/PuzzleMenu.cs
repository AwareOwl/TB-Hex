using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleMenu : GOUI {

    static PageUI pageUI;

    static string [] name;
    static int [] id;
    static bool [] finished;
    static List <int> finishedIndex;
    static List<int> unfinishedIndex;

    static GameObject [] puzzleRow;
    static GameObject [] puzzleRowText;
    static VisualCard [] VC = new VisualCard [4];
    static GameObject [,] visualTile = new GameObject [8, 8];
    static VisualToken [,] visualToken = new VisualToken [8, 8];

    static Text PuzzleNameText;

    static int selectedRow = -1;
    static int selectedId = -1;
    static int pageType = 0;
    static int currentPage;


	// Use this for initialization
	void Start () {
        DestroyTemplateButtons ();
        CreatePuzzleMenu ();
        CurrentGOUI = this;
    }

    static public void ShowPuzzleMenu () {
        ClientLogic.MyInterface.CmdDownloadDataToPuzzleMenu ();
    }

    static public void LoadPuzzleMenu (string [] name, int [] id, bool [] finished) {
        DestroyMenu ();
        PuzzleMenu.name = name;
        PuzzleMenu.id = id;
        PuzzleMenu.finished = finished;
        SortFinished ();
        CurrentCanvas.AddComponent<PuzzleMenu> ();
    }

    static public void LoadPuzzlePreview (string puzzleName, string [] puzzleBoard, string [] puzzleCardPool) {
        CreatePuzzlePreview (puzzleName, puzzleBoard, puzzleCardPool);
    }

    static public void ApplySelection () {
        if (selectedId == -1) {
            return;
        }
        ClientLogic.MyInterface.CmdStartPuzzle (selectedId);
    }

    static public void SortFinished () {
        finishedIndex = new List<int> ();
        unfinishedIndex = new List<int> ();
        int count = id.Length;
        for (int x = 0; x < count; x++) {
            if (finished [x]) {
                finishedIndex.Add (x);
            } else {
                unfinishedIndex.Add (x);
            }
        }
    }

    static public void SelectPage (int number) {
        currentPage = number;
        RefreshPage ();
    }

    static public void RefreshPage () {
        List<int> list = SelectedList ();
        int count = puzzleRow.GetLength (0);
        int puzzleCount = list.Count;
        for (int x = 0; x < count; x++) {
            GameObject Clone = puzzleRow [x];
            UIController UIC = Clone.GetComponent<UIController> ();
            UIC.FreeAndUnlcok ();
            GameObject Text = puzzleRowText [x];
            int number = x + currentPage * count;

           
            if (number < puzzleCount) {
                SetSprite (Clone, "UI/Panel_Slot_01_Sliced", true);
                if (RowNumberToId (x) == selectedId) {
                    UIC.PressAndLock ();
                }
                
                Text.GetComponent<TextMesh> ().text = name [list [number]];
            } else {
                SetSprite (Clone, "UI/Panel_Slot_01_Sliced_D", true);
                Text.GetComponent<TextMesh> ().text = "";
            }
        }
    }

    static public int RowNumberToNumber (int rowNumber) {
        int count = puzzleRow.GetLength (0);
        return rowNumber + currentPage * count;
    }

    static public int RowNumberToId (int rowNumber) {
        return id [SelectedList ()[RowNumberToNumber (rowNumber)]];
    }

    static public List<int> SelectedList () {
        switch (pageType) {
            case 0:
                return unfinishedIndex;
            case 1:
                return finishedIndex;
            default:
                return null;
        }
    }

    static public void SelectRow (int rowNumber) {
        selectedRow = rowNumber;
        if (SelectedList ().Count > RowNumberToNumber (rowNumber)) {
            int id = RowNumberToId (rowNumber);
            selectedId = id;
            ClientLogic.MyInterface.CmdDownloadPreviewToPuzzleMenu (id);
            RefreshPage ();
        }
    }

    static UIController [] typeButton = new UIController [2];
    static public void SelectType () {
        SelectType (pageType);
    }

    static public void SelectType (int type) {
        pageType = type;
        for (int x = 0; x < 2; x++) {
            typeButton [x].FreeAndUnlcok ();
        }
        typeButton [type].PressAndLock ();

        List<int> list = SelectedList ();
        int count = puzzleRow.GetLength (0);
        int puzzleCount = list.Count;
        int pageCount = Mathf.Max (1, (puzzleCount - 1) / count + 1);
        pageUI.Init (10, pageCount, new Vector2Int (90, 870), UIString.PuzzleMenuPageButton);

        RefreshPage ();
    }

    static public void CreatePuzzleMenu () {

        GameObject Clone;
        UIController UIC;

        Clone = CreateSprite ("UI/Panel_Window_01_Sliced", 720, 540, 10, 1440, 1080, false);
        //Clone = CreateSprite ("UI/White", 720, 540, 11, 5, 1080, false);

        Clone = CreateUIText (Language.ListOfPuzzles, 325, 120, 500, 36);
        Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;

        Clone = CreateUIText ("", 1015, 120, 500, 36);
        Text text = Clone.GetComponent<Text> ();
        text.alignment = TextAnchor.MiddleLeft;
        PuzzleNameText = text;

        /*Clone = CreateUIText (Language.PuzzleAbout, 360, 210, 600, 24);
        Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;*/

        Clone = CreateSprite ("UI/Butt_S_Help", 1320, 90, 11, 64, 64, true);
        Clone.name = UIString.PuzzleMenuAbout;


        GameObject pageUIObject = new GameObject ();
        pageUI = pageUIObject.AddComponent<PageUI> ();

        puzzleRow = new GameObject [9];
        puzzleRowText = new GameObject [9];

        for (int x = 0; x < 9; x++) {
            Clone = CreateSpriteWithText ("UI/Panel_Slot_01_Sliced", "", 360, 300 + 60 * x, 11, 600, 60);
            UIC = Clone.GetComponent<UIController> ();
            GameObject Text;
            Text = UIC.Text;
            Text.transform.parent = CurrentCanvas.transform;
            Text.GetComponent<TextMesh> ().anchor = TextAnchor.MiddleLeft;
            puzzleRowText [x] = Text;
            SetInPixPosition (Text, 90, 300 + 60 * x, 12);
            Clone.name = UIString.PuzzleMenuRow;
            UIC.number = x;
            UIC.Text = null;
            puzzleRow [x] = Clone;

            /*
            int number = Random.Range (1, 20);
            Clone = CreateSprite (VisualCard.GetIconPath (number), 600, 300 + 60 * x, 12, 45, 45, true);
            Clone.GetComponent<Renderer> ().material.color = AppDefaults.GetAbilityColor (number);*/
        }

        for (int x = 0; x < 2; x++) {
            string name = "";
            switch (x) {
                case 0:
                    name = Language.Unfinished;
                    break;
                case 1:
                    name = Language.Finished;
                    break;
            }
            Clone = CreateSpriteWithText ("UI/Butt_M_EmptySquare", name, 210 + 300 * x, 240, 11, 300, 60);
            Clone.name = UIString.PuzzleMenuType;
            UIC = Clone.GetComponent<UIController> ();
            UIC.number = x;
            typeButton [x] = UIC;
        }

        SelectType (0);


        Clone = CreateSprite ("UI/Butt_M_Apply", 105, 975, 11, 90, 90, true);
        Clone.name = UIString.PuzzleMenuApply;

        Clone = CreateSprite ("UI/Butt_M_Discard", 615, 975, 11, 90, 90, true);
        Clone.name = UIString.ShowMainMenu;

    }
    static public void CreatePuzzlePreview (string puzzleName, string [] puzzleBoard, string [] puzzleCardPool) {
        GameObject Clone;

        PuzzleNameText.text = puzzleName;

        BoardClass board = new BoardClass ();
        board.LoadFromString (puzzleBoard);
        CardPoolClass cardPool = new CardPoolClass ();
        cardPool.LoadFromString (puzzleCardPool);

        for (int x = 0; x < 8; x++) {
            for (int y = 0; y < 8; y++) {
                if (visualTile [x, y] != null) {
                    DestroyImmediate (visualTile [x, y]);
                }
                TileClass tile = board.tile [x, y];
                if (!tile.enabled) {
                    continue;
                }
                GameObject tileAnchor;
                GameObject Tile;
                tileAnchor = Instantiate (AppDefaults.Tile) as GameObject;
                tileAnchor.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
                Tile = tileAnchor.transform.Find ("Tile").gameObject;
                Tile.transform.localScale = new Vector3 (0.5f, 0.5f, 0.15f);
                tileAnchor.transform.parent = CurrentCanvas.transform;
                tileAnchor.transform.localEulerAngles = new Vector3 (-90, 0, 0);
                VisualEffectScript TEffect = Tile.gameObject.AddComponent<VisualEffectScript> ();
                TEffect.SetColor (EnvironmentScript.TileColorMain (true, 1) * 0.75f);
                visualTile [x, y] = tileAnchor;

                Vector3 V3 = VisualTile.TilePosition (x, 0, 8 - y);
                SetInPixPosition (tileAnchor, 1095 + (int) (V3.x * 60), 480 + (int) (V3.z * 60), 12);

                TokenClass token = tile.token;
                if (token == null) {
                    continue;
                }

                visualToken [x, y] = new VisualToken ();
                visualToken [x, y].SetState (token);
                GameObject tokenAnchor = visualToken [x, y].Anchor;
                tokenAnchor.transform.parent = tileAnchor.transform;
                tokenAnchor.transform.localPosition = new Vector3 (0, 0.1f, 0);
                tokenAnchor.transform.localScale = new Vector3 (1, 1, 1);
                tokenAnchor.transform.localEulerAngles = new Vector3 (0, 0, 0);


            }
        }


        for (int x = 0; x < 4; x++) {
            if (VC [x] != null) {
                DestroyImmediate (VC [x].Anchor);
            }
            Clone = CreateSprite ("UI/Panel_Slot_01_CollectionCard", 915 + 120 * x, 930, 11, 120, 150, false);
            VC [x] = SetEditor.LoadCard (cardPool.Card [x]);

            SetInPixPosition (VC [x].Anchor, 915 + 120 * x, 925, 12);
        }

        SelectType ();
    }
}
