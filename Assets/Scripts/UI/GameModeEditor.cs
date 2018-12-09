using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeEditor : GOUI {

    int gameModeId;

    // Use this for initialization
    void Start () {
        CreateGameModeEditor ();
        CurrentGUI = this;
    }

    static public void ShowGameModeEditor () {
        DestroyMenu ();
        CurrentCanvas.AddComponent<GameModeEditor> ();
    }

    static public void CreateGameModeEditor () {

    }
}
