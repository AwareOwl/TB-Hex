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
        CreateSprite ("UI/Panel_Window_01_Sliced", 700, 540, 10, 120 + 2 * 330, 330, false);
        for (int x = 0; x < 2; x++) {
            Button = CreateSprite ("UI/Butt_M_EmptySquare", 700 + (int) (330 * (x - 0.5f)), 480, 11, 330, 90, true);
            Button.name = UIString.MainMenuStartGameVsAI;
            Clone = CreateText (Language.PlayAgainstAI, 700 + (int) (330 * (x - 0.5f)), 480, 12, 0.03f);
            AddTextToGameObject (Button, Clone);
        }
        Button = CreateSprite ("UI/Butt_M_EmptySquare", 700, 600, 11, 330, 90, true);
        Button.name = UIString.ShowSetList;
        Clone = CreateText (Language.EditSet, 700, 600, 12, 0.03f);
        AddTextToGameObject (Button, Clone);

    }
}
