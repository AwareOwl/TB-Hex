﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGameLobby : GOUI {

    static RowClass [] row;

    static int currentPage = 0;
    static PageUI pageUI;

    static int [] ids;
    static string [] names;
    static int [] matchTypes;

    // Use this for initialization
    void Start () {
        CurrentGUI = this;
        CreateCustomGameLobby ();
        ClientLogic.MyInterface.CmdDownloadListOfCustomGames ();
    }

    static public void ShowCustomGameLobby () {
        DestroyMenu ();
        CurrentCanvas.AddComponent<CustomGameLobby> ();
    }

    static public void LoadData (int [] ids, string [] names, int [] matchTypes) {
        currentPage = 0;
        CustomGameLobby.ids = ids;
        CustomGameLobby.names = names;
        CustomGameLobby.matchTypes = matchTypes;
    }

    static public void ShowPage () {
        ShowPage (currentPage);
    }

    static public void ShowPage (int pageNumber) {
        currentPage = pageNumber;
        for (int x = 0; x < 5; x++) {
            int number = currentPage * 5 + x;
            int count = ids.Length;
            row [x].FreeRow ();
            if (number < count) {
                bool legal = boardIsLegal [number];
                row [x].SetState (boardNames [number], boardIds [number], 0, legal);
                if (boardIds [number] == selectedRow) {
                    row [x].SelectRow ();
                }
            } else if (number == count) {
                row [x].SetState (1);
            } else {
                row [x].SetState (2);
            }
        }
        pageUI.SelectPage (currentPage);
    }
    
    
    static public void CreateCustomGameLobby () {
        GameObject Clone;
        CreateSprite ("UI/Panel_Window_01_Sliced", 720, 540, 10, 690, 960, false);

        Clone = CreateUIText ("Available games: ", 720, 155, 520, 36);

        Clone = CreateSpriteWithText ("UI/Butt_M_EmptySquare", "Create new game", 720, 240, 11, 530, 90);

        row = new RowClass [5];
        for (int x = 0; x < 5; x++) {
            row [x] = CurrentGUI.gameObject.AddComponent<RowClass> ();
            row [x].Init (x, RowClass.BoardList);
        }

        row [0].SetState ("Custom game 1 (2FFA, 1/2)", 0, 0, true);
        row [1].SetState ("Custom game 3 (3FFA, 3/3)", 0, 0, false);
        row [2].SetState ("Hehehe (2FFA, 1/2)", 0, 0, true);
        row [3].SetState ("Custom game 15 (2v2, 1/4)", 0, 0, true);
        row [4].SetState ("Custom game 16 (1v2, 2/3)", 0, 0, true);
        row [0].SetState (3);
        row [1].SetState (3);
        row [2].SetState (3);
        row [3].SetState (3);
        row [4].SetState (3);

        pageUI = new PageUI ();
        pageUI.Init (9, 12, new Vector2Int (480, 780), "meh");
        pageUI.SelectPage (0);


        Clone = CreateSprite ("UI/Butt_M_Apply", 495, 900, 11, 90, 90, true);
        Clone.name = UIString.GameModeEditorApply;

        Clone = CreateSprite ("UI/Butt_M_Discard", 945, 900, 11, 90, 90, true);
        Clone.name = UIString.ShowMainMenu;
    }

}