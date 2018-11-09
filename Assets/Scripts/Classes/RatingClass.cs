using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatingClass {

    static int [] winner = new int [5];
    static int [] turn = new int [42];

    static int [] winnerScore = new int [1000];
    static int [] loserScore = new int [1000];

    static float [] NumberOfCards = new float [40]; // CardsInHand

    static public float [,,] AbilityOnStack; // AbilityType, AbilityArea (0, 2, 6 fields), stackNumber;
    static public float [,,] AbilityOnRow;

    RatingClass () {
        AbilityOnStack = new float [AppDefaults.AvailableAbilities, 3, 10];
        AbilityOnRow = new float [AppDefaults.AvailableAbilities, 3, 10];

        for (int x = 0; x < AbilityOnStack.GetLength (0); x++) {
            for (int y = 0; y < AbilityOnStack.GetLength (0); y++) {
                for (int z = 0; z < AbilityOnStack.GetLength (0); z++) {
                    AbilityOnStack [x, y, z] = 0.5f;
                    AbilityOnRow [x, y, z] = 0.5f;
                }
            }
        }
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
