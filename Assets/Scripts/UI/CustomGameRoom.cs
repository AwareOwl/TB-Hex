using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGameRoom : GOUI {

    static bool isHost;

    //static int id;
    static GameObject gameNameObject;
    static GameObject progress;

    static RowClass [] row;

    public override void DestroyThis () {
        row = null;
    }

    // Use this for initialization
    void Awake () {
        CurrentGUI = this;
        CreateCustomGameRoom ();
    }

    static public void StartMatch () {
        ClientLogic.MyInterface.CmdCustomGameRoomStartMatch ();
    }

    static public void ShowCustomGameRoom () {
        DestroyMenu ();
        CurrentCanvas.AddComponent<CustomGameRoom> ();
    }

    static public void LoadData (bool isHost, string gameName, int matchType, int [] avatars, string [] names, bool [] AIs) {
        if (row == null) {
            row = new RowClass [CustomGameClass.GetNumberOfSlots (matchType)];
            for (int x = 0; x < row.Length; x++) {
                row [x] = CurrentGUI.gameObject.AddComponent<RowClass> ();
                row [x].Init (x, RowClass.RoomUsers);
            }
        }
        CustomGameRoom.isHost = isHost;
        gameNameObject.GetComponent<TextMesh> ().text = gameName;
        progress.GetComponent<TextMesh> ().text = Language.ReadyToStart;
        for (int x = 0; x < row.Length; x++) {
            string name = names [x];
            if (AIs [x]) {
                name = Language.AIOpponent;
            }
            row [x].SetState (names [x], x, avatars [x], true);
            row [x].FreeRow ();
            if (names [x] == null || names [x] == "") {
                if (isHost) {
                    row [x].SetState (1);
                } else {
                    row [x].SetState (2);
                }
                progress.GetComponent<TextMesh> ().text = "";
                continue;
            }
            if (names [x] == ClientLogic.MyInterface.UserName) {
                row [x].SelectRow ();
            }
            if (isHost) {
                row [x].SetState (4);
            } else {
                row [x].SetState (5);
            }
        }
    }

    static public void AddAIOpponent (int rowNumber) {
        ClientLogic.MyInterface.CmdCustomGameAddAI (rowNumber);
    }

    static public void KickPlayer (int rowNumber) {
        ClientLogic.MyInterface.CmdCustomGameKickPlayer (rowNumber);
    }

    static public void CreateCustomGameRoom () {
        GameObject Clone;
        CreateSprite ("UI/Panel_Window_01_Sliced", 720, 540, 10, 690, 760, false);
        Clone = CreateText ("Custom game #124", 720, 245, 11, 0.03f);
        gameNameObject = Clone;

        Clone = CreateText ("Ready to start!", 720, 690, 11, 0.03f);
        progress = Clone;

        /*
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
        row [3].SetState (4);*/

        Clone = CreateSprite ("UI/Butt_M_Apply", 495, 810, 11, 90, 90, true);
        Clone.name = UIString.CustomGameRoomStartMatch;

        Clone = CreateSprite ("UI/Butt_M_Discard", 945, 810, 11, 90, 90, true);
        Clone.name = UIString.ShowCustomGameLobby;
    }

}
