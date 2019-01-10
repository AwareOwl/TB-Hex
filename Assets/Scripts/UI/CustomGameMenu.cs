using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGameMenu : GOUI {

    static RowClass [] row;

    // Use this for initialization
    void Start () {
        CurrentGUI = this;
        CreateCustomGameMenu ();
    }

    static public void ShowCustomGameMenu () {
        DestroyMenu ();
        CurrentCanvas.AddComponent<CustomGameMenu> ();
    }

    static public void CreateCustomGameMenu () {
        GameObject Clone;
        CreateSprite ("UI/Panel_Window_01_Sliced", 720, 540, 10, 690, 760, false);
        Clone = CreateUIText ("Custom game #124", 720, 245, 520, 36);

        Clone = CreateUIText ("Ready to start!", 720, 690, 520, 36);

        row = new RowClass [4];
        for (int x = 0; x < row.Length; x++) {
            row [x] = CurrentGUI.gameObject.AddComponent<RowClass> ();
            row [x].Init (x, RowClass.BoardList);
        }

        row [0].SetState ("Some player", 0, 1, true);
        row [1].SetState ("Doge", 0, 2, false);
        row [1].SelectRow ();
        row [2].SetState ("AI opponent", 0, 3, true);
        row [3].SetState ("AI opponent", 0, 3, true);
        row [0].SetState (4);
        row [1].SetState (4);
        row [2].SetState (4);
        row [3].SetState (4);

        Clone = CreateSprite ("UI/Butt_M_Apply", 495, 810, 11, 90, 90, true);
        Clone.name = UIString.GameModeEditorApply;

        Clone = CreateSprite ("UI/Butt_M_Discard", 945, 810, 11, 90, 90, true);
        Clone.name = UIString.ShowMainMenu;
    }

}
