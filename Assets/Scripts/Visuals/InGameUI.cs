using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : GOUI {

    static public MatchClass PlayedMatch;

    static public int MyPlayerNumber = 1;

    static public int NumberOfPlayers = 2;

    static public PlayerClass GetPlayer (int number) {
        return PlayedMatch.Player [number];
    }

    static public void ShowInGameUI (MatchClass playedMatch) {
        PlayedMatch = playedMatch;
        CreatePlayersUI ();
    }
    
    static public void CreatePlayersUI () {
        for (int x = 0; x < NumberOfPlayers; x++) {
            bool ally = GetPlayer (x).Properties.team == GetPlayer (MyPlayerNumber).Properties.team;
            VisualPlayer player = new VisualPlayer ();
            player.CreatePlayerUI (x, ally, NumberOfPlayers);
        }
    }

    public void SetPlayerHealthBar (int playerNumber) {
        PlayerClass player = GetPlayer (playerNumber);

        SetPlayerHealthBar (playerNumber, player.score, player.scoreIncome, PlayedMatch.Properties.scoreLimit);
    }
}
