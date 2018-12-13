using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : GOUI {

    MainMenu instance;

    private void Start () {
        instance = this;
        CreateMainMenu ();
        CurrentGUI = this;
    }

    static public void ShowMainMenu () {
        DestroyMenu ();
        CurrentCanvas.AddComponent<MainMenu> ();
    }

    static public void CreateMainMenu () {
        GameObject Clone;
        GameObject Button;
        int maxX = 1;
        CreateSprite ("UI/Panel_Window_01_Sliced", 720, 540, 10, 105 + maxX * 345, 330, false);
        for (int x = 0; x < maxX; x++) {
            Button = CreateSpriteWithText ("UI/Butt_M_EmptySquare", Language.PlayAgainstAI, 720 + (int) (345 * (x - maxX / 2f + 0.5f)), 480, 11, 330, 90);
            switch (x) {
                case 0:
                    Button.name = UIString.MainMenuStartGameVsAI;
                    break;
                case 1:
                    Button.name = UIString.MainMenuStartQuickMatch;
                    Button.transform.Find ("Text").GetComponent<TextMesh> ().text = Language.QuickOnlineGame;
                    break;
            }
        }
        Button = CreateSpriteWithText ("UI/Butt_M_EmptySquare", Language.SelectCardSetTooltip, 720, 600, 11, 330, 90);
        Button.name = UIString.ShowSetList;

        CreateSprite ("UI/Panel_Window_01_Sliced", 1200, 960, 10, 450, 210, false);
        Button = CreateSpriteWithText ("UI/Butt_M_EmptySquare", Language.GameVersion, 1200, 960, 11, 330, 90);
        Button.name = UIString.ShowGameModeMenu;

    }
}
