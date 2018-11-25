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
        CreateSprite ("UI/Panel_Window_01_Sliced", 700, 540, 10, 450, 330, false);
        Button = CreateSprite ("UI/Butt_M_EmptySquare", 700, 480, 11, 330, 90, true);
        Button.name = UIString.MainMenuStartGameVsAI;
        Clone = CreateText (Language.PlayAgainstAI, 700, 480, 12, 0.03f);
        AddTextToGameObject (Button, Clone);
        Button = CreateSprite ("UI/Butt_M_EmptySquare", 700, 600, 11, 330, 90, true);
        Button.name = UIString.MainMenuShowSetEditor;
        Clone = CreateText (Language.EditSet, 700, 600, 12, 0.03f);
        AddTextToGameObject (Button, Clone);

    }
}
