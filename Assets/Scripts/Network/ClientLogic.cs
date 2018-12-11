using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientLogic : MonoBehaviour {

    static public ClientInterface MyInterface;

    static public void LogIn (string accountName, string userName) {
        MyInterface.AccountName = accountName;
        MyInterface.UserName = userName;
        
        if (!InputController.autoRunAI) {
            //BoardEditorMenu.ShowBoardEditorMenu ();
            //CardPoolEditor.ShowCardPoolEditorMenu ();
            //ClientLogic.MyInterface.CmdJoinGameAgainstAI ();
            //SetEditor.ShowSetEditorMenu ();
            MainMenu.ShowMainMenu ();
        }
    }

    static public void LoadCurrentGameMatch (string [] lines) {
        InGameUI.PlayedMatch = new MatchClass ();
        InGameUI.PlayedMatch.LoadFromString (lines);
    }

    static public void LoadCurrentGameMatchProperties (string [] lines) {
        MatchClass match = InGameUI.PlayedMatch;
        match.Properties = new MatchPropertiesClass ();
        match.Properties.LoadFromString (lines);
    }

    static public void LoadCurrentGameBoard (string [] lines) {
        MatchClass match = InGameUI.PlayedMatch;
        match.Board = new BoardClass ();
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
        PlayerPropertiesClass playerProperties = player.properties;
        playerProperties.hand = new HandClass ();
        playerProperties.hand.LoadFromString (lines);
    }
}
