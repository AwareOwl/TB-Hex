using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeEditor : GOUI {

    static public bool editMode = false;

    static GameObject Background;

    static public string gameModeName;
    static public int gameModeId;
    static public RowClass [] row;
    static public int selectedRow;

    static public PageUI pageUI;

    static string [] boardNames;
    static int [] boardIds;
    static bool [] boardIsLegal;

    static int currentPage;

    // Use this for initialization
    void Awake () {
        CurrentGUI = this;
    }

    static public void ShowGameModeEditor () {
        ClientLogic.MyInterface.CmdDownloadGameModeToEditor (gameModeId);
    }

    static public void ShowGameModeEditor (int gameModeId) {
        GameModeEditor.gameModeId = gameModeId;
        ShowGameModeEditor ();
    }

    static public void DeleteBoard (int boardId) {
        ClientLogic.MyInterface.CmdDeleteBoard (gameModeId, boardId);
    }
    static public void CreateNewBoard () {
        ClientLogic.MyInterface.CmdCreateNewBoard (gameModeId, Language.NewBoard);
    }

    static public void ClickOnRow (int id) {
        selectedRow = id;
        ShowPage ();
    }

    static public void ClickOnPageButton (int number) {
        currentPage = number;
        ShowPage ();
    }

    static public void ShowCardPoolEditor () {
        CardPoolEditor.ShowCardPoolEditorMenu (gameModeId);
    }

    override public void ShowPropertiesMenu () {
        PropertiesMenu.ShowSetPropertiesMenu (PropertiesMenu.gameModePropertiesKey, gameModeName, 0);
    }

    static public void ApplyProperties (string name, int iconNumber) {
        ClientLogic.MyInterface.CmdSaveGameModeProperties (gameModeId, name, iconNumber);
    }

    static public int PageLimit () {
        int count = boardIds.Length;
        if (editMode) {
            return count / 5 + 1;
        } else {
            return (count - 1) / 5 + 1;
        }
    }

    static public void RefreshPageButtons () {
        int pageLimit = PageLimit ();
        Debug.Log (pageLimit);
        pageUI.Init (9, pageLimit, new Vector2Int (480, 780), UIString.GameModeEditorPageButton);
    }

    static public void ShowPage () {
        for (int x = 0; x < 5; x++) {
            int number = currentPage * 5 + x;
            int count = boardIds.Length;
            row [x].FreeRow ();
            if (number < count) {
                bool legal = boardIsLegal [number];
                row [x].SetState (boardNames [number], boardIds [number], 0, legal);
                if (boardIds [number] == selectedRow) {
                    row [x].SelectRow ();
                }
            } else if (number == count && editMode) {
                row [x].SetState (1);
            } else {
                row [x].SetState (2);
            }
        }
        pageUI.SelectPage (currentPage);
    }

    static public void LoadDataToEditor (bool editMode, int gameModeId, string gameModeName, string [] boardNames, int [] boardIds, bool [] boardIsLegal) {
        DestroyMenu ();
        CurrentCanvas.AddComponent<GameModeEditor> ();
        GameModeEditor.editMode = editMode;
        CreateGameModeEditor ();
        GameModeEditor.gameModeId = gameModeId;
        GameModeEditor.gameModeName = gameModeName;
        GameModeEditor.boardNames = boardNames;
        GameModeEditor.boardIds = boardIds;
        GameModeEditor.boardIsLegal = boardIsLegal;
        currentPage = Mathf.Min (PageLimit (), currentPage);
        Debug.Log (currentPage);
        RefreshPageButtons ();
        ShowPage ();
    }

    static public void CreateGameModeEditor () {

        GameObject Button;
        GameObject Clone;

        Background = CreateSprite ("UI/Panel_Window_01_Sliced", 720, 540, 10, 720, 810, false);

        Clone = CreateUIText (Language.AvailableBoards + ":", 720, 230, 520, 36);

        row = new RowClass [5];
        for (int x = 0; x < 5; x++) {
            row [x] = CurrentGUI.gameObject.AddComponent<RowClass> ();
            row [x].Init (x, RowClass.BoardList);
        }

        GameObject pageUIObject = new GameObject ();
        pageUI = pageUIObject.AddComponent<PageUI> ();
        

        Background = CreateSprite ("UI/Panel_Window_01_Sliced", 195, 330, 10, 390, 390, false);
        Clone = CreateUIText (Language.Tools + ":", 195, 225, 520, 36);

        if (editMode) {
            Clone = CreateSprite ("UI/Butt_S_Name", 90, 315, 11, 60, 60, true);
            Clone.name = UIString.GameModeEditorChangeName;
        }
        Clone = CreateSprite ("UI/Butt_S_DeckPreview", 150, 315, 11, 60, 60, true);
        Clone.name = UIString.GameModeEditorEditCardPool;
        Clone = CreateSprite ("UI/Butt_S_Settings", 210, 315, 11, 60, 60, true);
        Clone.name = UIString.GameModeEditorSettings;
        Clone = CreateSprite ("UI/Butt_S_Help", 270, 315, 11, 60, 60, true);
        Clone.name = UIString.GameModeEditorAbout;

        /*Clone = CreateSprite ("UI/Butt_M_Apply", 105, 420, 11, 90, 90, true);
        Clone.name = UIString.GameModeEditorApply;*/

        Clone = CreateSprite ("UI/Butt_M_Discard", 285, 420, 11, 90, 90, true);
        Clone.name = UIString.GoBackToGameModeSelection;

    }
}
