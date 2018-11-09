using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatingClass {

    static int [] winner = new int [5];
    static int [] turn = new int [42];

    static int [] winnerScore = new int [1000];
    static int [] loserScore = new int [1000];

    static public int [,] AbilityOnStack;

    RatingClass () {

    }

    static public void AnalyzeStatistics (MatchClass match) { // anal...
        int winnerNumber = match.winner.playerNumber;
        winner [winnerNumber] ++;
        winnerScore [winnerNumber]++;
        for (int x = 1; x <= match.numberOfPlayers; x++) {
            PlayerClass player = match.Player [x];
            if (winnerNumber != x) {
                loserScore [player.score]++;
            }
        }
        turn [match.turn]++;
        SavePlayerWinRatio ();
        SaveTurn ();
    }

    static public void SavePlayerWinRatio () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < winner.Length; x++) {
            lines.Add (x.ToString () + ": "  + winner [x].ToString ());
        }
        ServerData.SaveRatingPlayerWinRatio (lines.ToArray ());
    }

    static public void SaveTurn () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < turn.Length; x++) {
            lines.Add (x.ToString () + ": " + turn [x].ToString ());
        }
        ServerData.SaveRatingTurn (lines.ToArray ());
    }

}
