using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeMenu : GOUI {

    static GameObject Background;

    static GameObject DropdownObject;
    static Dropdown DropdownComponent;

    static string [][] lists;
    static int [][] ids;
    static int group;

    // Use this for initialization
    void Start () {
        CreateGameModeMenu ();
        CurrentGUI = this;
        ClientLogic.MyInterface.CmdDownloadGameModeLists ();
    }

    static public void ApplyGameMode () {
        ClientLogic.MyInterface.CmdChangeGameMode (ids [group] [DropdownComponent.value]);
        MainMenu.ShowMainMenu ();
    }

    static public void UpdateLists (int selectedGameMode,
        string [] officialNames, string [] publicNames, string [] yourNames,
        int [] officialIds, int [] publicIds, int [] yourIds) {

        ids = new int [3][];

        ids [0] = officialIds;

        int count = officialNames.Length;
        int selectedIndex = 0;
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData> ();
        for (int x = 0; x < count; x++) {
            Dropdown.OptionData option = new Dropdown.OptionData (officialNames [x]);
            options.Add (option);
            if (ids [0][x] == selectedGameMode) {
                selectedIndex = x;
            }
        }
        DropdownComponent.options = options;
        DropdownComponent.value = selectedIndex;
    }

    static public void ShowGameModeMenu () {
        DestroyMenu ();
        CurrentCanvas.AddComponent<GameModeMenu> ();
    }

    static public void CreateGameModeMenu () {
        GameObject Button;
        GameObject Clone;

        Background = CreateSprite ("UI/Panel_Window_01_Sliced", 720, 540, 10, 540, 360, false);

        Clone = CreateUIText (Language.OfficialGameVersions + ":", 720, 450, 520, 36);

        Clone = CreateUIDropdown (720, 510, 390, 60);
        DropdownObject = Clone;
        DropdownComponent = DropdownObject.GetComponent<Dropdown> ();


        Clone = CreateSprite ("UI/Butt_M_Apply", 555, 615, 11, 90, 90, true);
        Clone.name = UIString.GameModeMenuApply;

        Clone = CreateSprite ("UI/Butt_M_Discard", 885, 615, 11, 90, 90, true);
        Clone.name = UIString.ShowMainMenu;

        /*
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
            Button = CreateSpriteWithText ("UI/Butt_M_EmptySquare", text, 720 + (int) (330 * (x - maxX / 2f)), 480, 11, 330, 90);

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
        }*/
    }
}
