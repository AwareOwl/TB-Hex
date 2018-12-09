using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeMenu : GOUI {

    static GameObject Background;

    static string [,] lists;
    static int [,] ids;

    // Use this for initialization
    void Start () {
        CreateGameModeMenu ();
        CurrentGUI = this;
        ClientLogic.MyInterface.CmdDownloadGameModeLists ();
    }

    static public void UpdateLists (string [] officialNames, string [] publicNames, string [] yourNames,
        int [] officialIds, int [] publicIds, int [] yourIds) {

    }

    static public void ShowGameModeMenu () {
        DestroyMenu ();
        CurrentCanvas.AddComponent<GameModeMenu> ();
    }

    static public void CreateGameModeMenu () {
        GameObject Button;
        GameObject Clone;

        Background = CreateSprite ("UI/Panel_Window_01_Sliced", 720, 540, 10, 690, 780, false);

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
        }
    }
}
