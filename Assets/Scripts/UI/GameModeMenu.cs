using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeMenu : GOUI {

    static GameObject Background;

    static GameObject DropdownObject;

    static string [][] names;
    static int [][] ids;
    static int currentGroup;

    static int currentPage;

    static int [] selectedRow;

    static public RowClass [] row;

    // Use this for initialization
    void Start () {
        CurrentGUI = this;
        CreateGameModeMenu ();
        ClientLogic.MyInterface.CmdDownloadGameModeLists ();
    }

    static public void ApplyGameMode () {
        ClientLogic.MyInterface.CmdChangeGameMode (ids [currentGroup] [selectedRow [currentGroup]]);
        MainMenu.ShowMainMenu ();
    }

    static public void UpdateLists (int selectedGameMode,
        string [] officialNames, string [] publicNames, string [] yourNames,
        int [] officialIds, int [] publicIds, int [] yourIds) {

        names = new string [3] [];
        ids = new int [3] [];

        names [0] = officialNames;
        names [1] = publicNames;
        names [2] = yourNames;
        ids [0] = officialIds;
        ids [1] = publicIds;
        ids [2] = yourIds;
        ShowPage ();
        /*
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData> ();
        for (int x = 0; x < count; x++) {
            Dropdown.OptionData option = new Dropdown.OptionData (officialNames [x]);
            options.Add (option);
            if (ids [0][x] == selectedGameMode) {
                selectedIndex = x;
            }
        }
        DropdownComponent.options = options;
        DropdownComponent.value = selectedIndex;*/
    }

    static public void SelectGroup (int number) {
        currentGroup = number;
        ShowPage ();
    }

    static public void SelectPage (int number) {
        currentPage = number;
        ShowPage ();
    }

    static public void ShowPage () {
        for (int x = 0; x < 5; x++) {
            int number = currentPage * 5 + x;
            int count = ids [currentGroup].Length;
            if (number < count) {
                row [x].SetState (names [currentGroup] [number], ids [currentGroup] [number], 0, true);
            } else if (number == count) {
                row [x].SetState (1);
            } else {
                row [x].SetState (2);
            }
            if (currentGroup != 2) {
                if (number >= count) {
                    row [x].SetState (2);
                }
                row [x].SetState (3);
            }
        }
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
        for (int x = 0; x < 3; x++) {
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
                    Button.GetComponent<UIController> ().PressAndLock ();
                    break;
                case 1:
                    Button.name = UIString.GameModeMenuPublicGameModes;
                    break;
                case 2:
                    Button.name = UIString.GameModeMenuYourGameModes;
                    break;
            }
        }
        row = new RowClass [5];
        for (int x = 0; x < 5; x++) {
            row [x] = CurrentGUI.gameObject.AddComponent <RowClass>();
            row [x].Init (x, RowClass.GameModeList);
        }

        GameObject pageUIObject = new GameObject ();
        PageUI pageUI = pageUIObject.AddComponent<PageUI> ();
        pageUI.Init (9, 13, new Vector2Int (480, 780), UIString.GameModeListPageButton);
    }
}
