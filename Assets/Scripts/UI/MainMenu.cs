﻿using System.Collections;
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
        ClientLogic.MyInterface.CmdCurrentGameConcede ();
        DestroyMenu ();
        CurrentCanvas.AddComponent<MainMenu> ();
    }

    static public void CreateMainMenu () {
        GameObject Clone;
        GameObject Button;
        int maxX = 3;
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
                case 2:
                    Button.name = UIString.ShowCustomGameLobby;
                    //Button.name = UIString.ShowCustomGameLobby;
                    Button.transform.Find ("Text").GetComponent<TextMesh> ().text = Language.CustomGame;
                    break;
            }
        }
        Button = CreateSpriteWithText ("UI/Butt_M_EmptySquare", Language.SelectCardSetTooltip, 720, 600, 11, 330, 90);
        Button.name = UIString.ShowSetList;

        CreateSprite ("UI/Panel_Window_01_Sliced", 1200, 930, 10, 360, 240, false);
        Button = CreateSpriteWithText ("UI/Butt_M_EmptySquare", Language.Profile, 1200, 900, 11, 240, 60, 0.025f);
        Button.name = UIString.ShowProfileMenu;
        Button = CreateSpriteWithText ("UI/Butt_M_EmptySquare", Language.GameVersion, 1200, 960, 11, 240, 60, 0.025f);
        Button.name = UIString.ShowGameModeMenu;

    }
}
