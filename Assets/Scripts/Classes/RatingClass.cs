using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatingClass {

    static bool CreateBackUp = true;

    static int [] winner = new int [5];
    static int [] turn = new int [42];

    static float [] edgeDanger = new float [100];
    static float [] multiTargetDanger = new float [100];
    static float [] surroundDanger = new float [100];

    static int [] winnerScore = new int [1000];
    static int [] loserScore = new int [1000];

    static float [] NumberOfCards = new float [40]; // CardsInHand

    static public float [,,] AbilityOnStack; // AbilityType, AbilityArea (0, 2, 6 fields), stackNumber;
    static public float [,,] AbilityOnRow;

    static RatingClass () {
        AbilityOnStack = new float [AppDefaults.AvailableAbilities, 3, 10];
        AbilityOnRow = new float [AppDefaults.AvailableAbilities, 3, 10];

        for (int x = 0; x < AbilityOnStack.GetLength (0); x++) {
            for (int y = 0; y < AbilityOnStack.GetLength (1); y++) {
                for (int z = 0; z < AbilityOnStack.GetLength (2); z++) {
                    AbilityOnStack [x, y, z] = 0.5f;
                    //AbilityOnRow [x, y, z] = 0.5f;
                }
            }
        }
        LoadAbilityOnRow ();
        for (int x = 0; x < edgeDanger.Length; x++) {
            edgeDanger [x] = 0.5f;
            multiTargetDanger [x] = 0.5f;
            surroundDanger [x] = 0.5f;
        }
    }

    static public void AnalyzeStatistics (MatchClass match) { // anal...
        int winnerNumber = match.winner.playerNumber;
        winner [winnerNumber] ++;
        winnerScore [match.winner.score]++;
        for (int x = 1; x <= match.numberOfPlayers; x++) {
            PlayerClass player = match.Player [x];
            int numberOfCards = 0;
            HandClass hand = player.properties.hand;
            for (int y = 0; y < hand.stack.Length; y++) {
                StackClass stack = hand.stack [y];
                int cardCount = stack.card.Count;
                numberOfCards += cardCount;
                for (int z = 0; z < cardCount; z++) {
                    CardClass card = stack.card [z];
                    int abilityArea = card.abilityArea;
                    switch (card.abilityArea) {
                        case 0:
                            abilityArea = 0;
                            break;
                        case 1:
                        case 2:
                        case 3:
                            abilityArea = 1;
                            break;
                        case 4:
                            abilityArea = 2;
                            break;
                    }
                    AbilityOnStack [card.abilityType, abilityArea, y] *= 0.999f;
                    AbilityOnRow [card.abilityType, abilityArea, z] *= 0.999f;
                    if (winnerNumber == x) {
                        AbilityOnStack [card.abilityType, abilityArea, y] += 0.001f;
                        AbilityOnRow [card.abilityType, abilityArea, z] += 0.001f;
                    }
                }
            }
            AIClass AI = player.properties.AI;
            NumberOfCards [numberOfCards] *= 0.999f;
            edgeDanger [AI.edgeDanger] *= 0.999f;
            multiTargetDanger [AI.multiTargetDanger] *= 0.999f;
            surroundDanger [AI.surroundDanger] *= 0.999f;
            if (winnerNumber == x) {
                NumberOfCards [numberOfCards] += 0.001f;
                edgeDanger [AI.edgeDanger] += 0.001f;
                multiTargetDanger [AI.multiTargetDanger] += 0.001f;
                surroundDanger [AI.surroundDanger] += 0.001f;
            } else {
                loserScore [player.score]++;
            }

        }
        turn [match.turn]++;
        SaveAbilityOnStack ();
        SaveAbilityOnRow ();
        SaveWinnerScore ();
        SaveLoserScore ();
        SavePlayerWinRatio ();
        SaveTurn ();
        SaveEdgeDanger ();
        SaveMultiTargetDanger ();
        SaveSurroundDanger ();
    }

    static public void SaveAbilityOnStack () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < AbilityOnStack.GetLength (0); x++) {
            string s = "[" + x.ToString () + "] ";
            for (int y = 0; y < AbilityOnStack.GetLength (1); y++) {
                s += "[" + y.ToString () + "] ";
                for (int z = 0; z < AbilityOnStack.GetLength (2); z++) {
                    s += AbilityOnStack [x, y, z].ToString () + " ";
                }
            }
            lines.Add (s);
        }
        ServerData.SaveRatingAbilityOnStack (lines.ToArray ());
    }

    static public void SaveAbilityOnRow () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < AbilityOnRow.GetLength (0); x++) {
            string s = "[" + x.ToString () + "] ";
            for (int y = 0; y < AbilityOnRow.GetLength (1); y++) {
                s += "[" + y.ToString () + "] ";
                for (int z = 0; z < AbilityOnRow.GetLength (2); z++) {
                    s += AbilityOnRow [x, y, z].ToString () + " ";
                }
            }
            lines.Add (s);
        }
        ServerData.SaveRatingAbilityOnRow (lines.ToArray ());
    }

    static public void LoadAbilityOnRow () {
        string [] lines = ServerData.GetRatingAbilityOnRow ();
        for (int x = 0; x < AbilityOnRow.GetLength (0); x++) {
            string [] word = null;
            if (x < lines.Length) {
                word = lines [x].Split (' ');
            }
            for (int y = 0; y < AbilityOnRow.GetLength (1); y++) {
                for (int z = 0; z < AbilityOnRow.GetLength (2); z++) {
                    int number = 2 + y * (AbilityOnRow.GetLength (2) + 1) + z;
                    if (word != null && number < word.Length) {
                        AbilityOnRow [x, y, z] = float.Parse (word [number]);
                    } else {
                        AbilityOnRow [x, y, z] = 0.5f;
                    }
                }
            }
        }
    }

    static public void SaveWinnerScore () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < winnerScore.Length; x++) {
            lines.Add ("[" + x.ToString () + "] " + winnerScore [x].ToString ());
        }
        ServerData.SaveRatingWinnerScore (lines.ToArray ());
    }

    static public void SaveLoserScore () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < loserScore.Length; x++) {
            lines.Add ("[" + x.ToString () + "] " + loserScore [x].ToString ());
        }
        ServerData.SaveRatingLoserScore (lines.ToArray ());
    }

    static public void SavePlayerWinRatio () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < winner.Length; x++) {
            lines.Add ("[" + x.ToString () + "] " + winner [x].ToString ());
        }
        ServerData.SaveRatingPlayerWinRatio (lines.ToArray ());
    }

    static public void SaveTurn () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < turn.Length; x++) {
            lines.Add ("[" + x.ToString () + "] " + turn [x].ToString ());
        }
        ServerData.SaveRatingTurn (lines.ToArray ());
    }

    static public void SaveEdgeDanger () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < turn.Length; x++) {
            lines.Add ("[" + x.ToString () + "] " + edgeDanger [x].ToString ());
        }
        ServerData.SaveRatingEdgeDanger (lines.ToArray ());
    }

    static public void SaveMultiTargetDanger () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < turn.Length; x++) {
            lines.Add ("[" + x.ToString () + "] " + multiTargetDanger [x].ToString ());
        }
        ServerData.SaveRatingMultiTargetDanger (lines.ToArray ());
    }

    static public void SaveSurroundDanger () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < turn.Length; x++) {
            lines.Add ("[" + x.ToString () + "] " + surroundDanger [x].ToString ());
        }
        ServerData.SaveRatingSurroundDanger (lines.ToArray ());
    }

}
