using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeMenu : GOUI {

    static GameObject Background;

    static GameObject DropdownObject;

    static public GameObject [] groupButton;

    static string [][] names;
    static int [][] ids;
    static bool [] yourIsLegal;
    static public int currentGroup;

    static int [] currentPage = new int [3];

    static int selectedId;

    static public RowClass [] row;

    static PageUI pageUI;

    // Use this for initialization
    void Start () {
        CurrentGUI = this;
        CreateGameModeMenu ();
        ClientLogic.MyInterface.CmdDownloadGameModeLists ();
    }

    static public void ApplyGameMode () {
        ClientLogic.MyInterface.CmdChangeGameMode (selectedId);
        MainMenu.ShowMainMenu ();
    }

    static public void UpdateLists (int selectedGameMode,
        string [] officialNames, string [] publicNames, string [] yourNames,
        int [] officialIds, int [] publicIds, int [] yourIds, bool [] yourIsLegal) {

        selectedId = selectedGameMode;

        names = new string [3] [];
        ids = new int [3] [];

        names [0] = officialNames;
        names [1] = publicNames;
        names [2] = yourNames;
        ids [0] = officialIds;
        ids [1] = publicIds;
        ids [2] = yourIds;
        GameModeMenu.yourIsLegal = yourIsLegal;
        SelectGroup (currentGroup);
        ShowPage ();
    }

    static public void SelectGroup (int number) {
        currentGroup = number;
        currentPage [currentGroup] = Mathf.Min (currentPage [currentGroup], PageLimit ());
        for (int x = 0; x < groupButton.Length; x++) {
            groupButton [x].GetComponent<UIController> ().FreeAndUnlcok ();
        }
        groupButton [number].GetComponent<UIController> ().PressAndLock ();
        CreatePageButtons ();
        ShowPage ();
    }

    static public int PageLimit () {
        int count = ids [currentGroup].Length;
        if (currentGroup == 2) {
            count++;
        }
        return (count - 1) / 5 + 1;
    }

    static public void CreatePageButtons () {
        int pageLimit = PageLimit ();
        GameObject pageUIObject = new GameObject ();
        if (pageUI == null) {
            pageUI = pageUIObject.AddComponent<PageUI> ();
        }
        pageUI.Init (9, pageLimit, new Vector2Int (480, 780), UIString.GameModeListPageButton);
    }

    static public void SelectPage (int number) {
        currentPage [currentGroup] = number;
        ShowPage ();
    }

    static public void ClickOnGameMode (int row) {
        selectedId = row;
        ShowPage ();
    }

    static public void ShowGameModeEditor (int row) {
        ClickOnGameMode (row);

    }

    static public void ShowPage () {
        for (int x = 0; x < 5; x++) {
            int number = currentPage [currentGroup] * 5 + x;
            int count = ids [currentGroup].Length;
            row [x].FreeRow ();
            if (number < count) {
                bool legal = true;
                if (currentGroup == 2) {
                    legal = yourIsLegal [number];
                }
                row [x].SetState (names [currentGroup] [number], ids [currentGroup] [number], 0, legal);
                if (ids [currentGroup] [number] == selectedId) {
                    row [x].SelectRow ();
                }
            } else if (number == count) {
                row [x].SetState (1);
            } else {
                row [x].SetState (2);
            }
            if (currentGroup != 2) {
                if (number < count) {
                    row [x].SetState (6);
                } else {
                    row [x].SetState (2);
                }
            }
        }
        pageUI.SelectPage (currentPage [currentGroup]);
    }

    static public void ShowGameModeMenu () {
        DestroyMenu ();
        CurrentCanvas.AddComponent<GameModeMenu> ();
    }

    static public void CreateGameModeMenu () {
        GameObject Button;
        GameObject Clone;

        Background = CreateSprite ("UI/Panel_Window_01_Sliced", 720, 540, 10, 1230, 990, false);

        //Clone = CreateUIText (Language.OfficialGameVersions + ":", 720, 230, 520, 36);


        Clone = CreateSprite ("UI/Butt_M_Apply", 495, 915, 11, 90, 90, true);
        Clone.name = UIString.GameModeMenuApply;

        Clone = CreateSprite ("UI/Butt_M_Discard", 945, 915, 11, 90, 90, true);
        Clone.name = UIString.ShowMainMenu;
        
        int maxX = 3;
        groupButton = new GameObject [maxX];
        for (int x = 0; x < maxX; x++) {
            string text = "";
            switch (x) {
                case 0:
                    text = Language.OfficialGameVersions;
                    break;
                case 1:
                    text = Language.PublicGameVersions;
                    break;
                case 2:
                    text = Language.YourGameVersions;
                    break;
            }
            Button = CreateSpriteWithText ("UI/Butt_M_EmptySquare", text, 900 + (int) (360 * (x - maxX / 2f)), 165, 11, 360, 90);

            switch (x) {
                case 0:
                    Button.name = UIString.GameModeMenuOfficialGameModes;
                    break;
                case 1:
                    Button.name = UIString.GameModeMenuPublicGameModes;
                    break;
                case 2:
                    Button.name = UIString.GameModeMenuYourGameModes;
                    break;
            }
            groupButton [x] = Button;
        }
        //SelectGroup (0);
        row = new RowClass [5];
        for (int x = 0; x < 5; x++) {
            row [x] = CurrentGUI.gameObject.AddComponent <RowClass>();
            row [x].Init (x, RowClass.GameModeList);
        }
    }
}
