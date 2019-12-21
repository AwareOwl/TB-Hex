using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : GOUI {

    MainMenu instance;

    private void Start () {
        instance = this;
        CreateMainMenu ();
        CurrentGOUI = this;
    }

    static public void ShowMainMenu () {
        ClientLogic.MyInterface.CmdCurrentGameConcede ();
        DestroyMenu ();
        CurrentCanvas.AddComponent<MainMenu> ();
    }

    static public void CreateMainMenu () {
        GameObject Clone;
        GameObject Button;
        int maxX = 5;
        Clone = CreateSprite ("UI/Panel_Window_01_Sliced", 720, 540, 10, 450, 105 + maxX * 105, false);
        DestroyImmediate (Clone.GetComponent<UIController> ());
        for (int x = 0; x < maxX; x++) {
            if (x == 3) {
                continue;
            }
            Button = CreateSpriteWithText ("UI/Butt_M_EmptySquare", Language.PlayAgainstAI, 720, 540 + (int) (105 * (x - maxX / 2f + 0.5f)), 11, 330, 90);
            switch (x) {
                case 0:
                    Button.name = UIString.MainMenuStartGameVsAI;
                    break;
                case 1:
                    Button.name = UIString.MainMenuStartQuickMatch;
                    Button.transform.Find ("Text").GetComponent<TextMesh> ().text = Language.QuickOnlineGame;
                    break;
                case 2:
                    Button.name = UIString.ShowCustomGameLobby;
                    Button.transform.Find ("Text").GetComponent<TextMesh> ().text = Language.CustomGame;
                    break;
                case 4:
                    Button.name = UIString.ShowSetList;
                    Button.transform.Find ("Text").GetComponent<TextMesh> ().text = Language.SelectCardSetTooltip;
                    break;
            }
        }

        maxX = 6;
        Clone = CreateSprite ("UI/Panel_Window_01_Sliced", 1200, 990 - 30 * maxX, 10, 360, 120 + 60 * maxX, false);
        DestroyImmediate (Clone.GetComponent<UIController> ());
        for (int x = 0; x < maxX; x++) {
            Button = CreateSpriteWithText ("UI/Butt_M_EmptySquare", Language.PlayAgainstAI, 1200, 960 - 60 * x, 11, 240, 60);
            switch (x) {
                case 0:
                    Button.name = UIString.ShowGameModeMenu;
                    Button.transform.Find ("Text").GetComponent<TextMesh> ().text = Language.GameVersion;
                    break;
                case 1:
                    Button.name = UIString.LibraryMenu;
                    Button.transform.Find ("Text").GetComponent<TextMesh> ().text = Language.Library;
                    break;
                case 2:
                    Button.name = UIString.ShowProfileMenu;
                    Button.transform.Find ("Text").GetComponent<TextMesh> ().text = Language.Profile;
                    break;
                case 3:
                    Button.name = UIString.ShowBossMenu;
                    Button.transform.Find ("Text").GetComponent<TextMesh> ().text = Language.Bosses;
                    break;
                case 4:
                    Button.name = UIString.ShowPuzzleMenu;
                    Button.transform.Find ("Text").GetComponent<TextMesh> ().text = Language.Puzzles;
                    break;
                case 5:
                    Button.name = UIString.ShowTutorialMenu;
                    Button.transform.Find ("Text").GetComponent<TextMesh> ().text = Language.Tutorial;
                    break;

            }
        }
    }
}
