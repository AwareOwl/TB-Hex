using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientLogic : MonoBehaviour {

    static public ClientInterface MyInterface;

    static public void LogIn (string accountName, string userName) {
        MyInterface.AccountName = accountName;
        MyInterface.UserName = userName;

        InputController.autoRunAI = false;

        if (!InputController.autoRunAI) {
            //BoardEditorMenu.ShowBoardEditorMenu ();
            //CardPoolEditor.ShowCardPoolEditorMenu ();
            //ClientLogic.MyInterface.CmdJoinGameAgainstAI ();
            //SetEditor.ShowSetEditorMenu ();

            //AIRatingMenu.ShowAIRatingMenuMenu (MyInterface.GameMode);
            //AIRatingMenu.RefreshPage ();
            if (UnlockedContent.cached) {
                UnlockedContent.LoadUnlockedContentMenu (0);
            } else {
                MainMenu.ShowMainMenu ();
            }
        } else {
            //ServerLogic.JoinGameAgainstAI (ClientLogic.MyInterface).RunAI ();
            AIRatingMenu.ShowAIRatingMenuMenu (MyInterface.GameMode);
            AIRatingMenu.RefreshPage ();
            //MainMenu.ShowMainMenu ();
        }
    }

    static public void LoadCurrentGameMatch (string [] lines) {
        GOUI.DestroyMenu ();
        InGameUI.PlayedMatch = new MatchClass ();
        InGameUI.PlayedMatch.LoadFromString (lines);
    }

    static public void LoadCurrentGameMatchProperties (string [] lines) {
        MatchClass match = InGameUI.PlayedMatch;
        match.properties = new MatchPropertiesClass ();
        match.properties.LoadFromString (lines);
    }

    static public void LoadCurrentGameBoard (string [] lines) {
        MatchClass match = InGameUI.PlayedMatch;
        match.Board = new BoardClass (match);
        match.Board.LoadBoard (lines, 8, 8);
    }

    static public void LoadCurrentGamePlayer (int playerNumber, string [] lines) {
        MatchClass match = InGameUI.PlayedMatch;
        match.Player [playerNumber] = new PlayerClass ();
        match.Player [playerNumber].LoadFromString (lines);
    }

    static public void LoadCurrentGamePlayerProperties (int playerNumber, string [] lines) {
        MatchClass match = InGameUI.PlayedMatch;
        PlayerClass player = match.Player [playerNumber];
        player.properties = new PlayerPropertiesClass ();
        player.properties.LoadFromString (lines);
    }

    static public void LoadCurrentGameHand (int playerNumber, string [] lines) {
        MatchClass match = InGameUI.PlayedMatch;
        PlayerClass player = match.Player [playerNumber];
        player.hand = new HandClass (lines.Length / 2);
        player.hand.LoadFromModeString (lines);
    }

    static public void LoadCurrentGamePlayedMove (string [] lines) {
        if (lines == null || lines.Length == 0) {
            return;
        }
        MatchClass match = InGameUI.PlayedMatch;
        match.LastMove = new MoveHistoryClass ();
        match.LastMove.PlayedLoadFromString (match, lines);
    }

    static public void LoadCustomGameRoom (bool isHost, string gameName, int matchType, int [] avatars, string [] names, bool [] AIs, int [] teams) {
        if (GOUI.CurrentGOUI.GetType () != typeof (CustomGameRoom)) {
            CustomGameRoom.ShowCustomGameRoom ();
        }
        CustomGameRoom.LoadData (isHost, gameName, matchType, avatars, names, AIs, teams);
    }

    static public void RefreshProfileSettings (ClientInterface client, string userName, int avatar) {
        if (userName != null && userName != "") {
            client.UserName = userName;
        }
        ProfileMenu.ShowProfileMenu (userName, avatar);
    }
}
