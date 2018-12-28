using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatingClass {

    static bool CreateBackUp = true;

    static int [] winner = new int [5];
    static int [] turn = new int [42];

    static float [,] mapPlayer = new float [10, 5];

    static float [] edgeDanger = new float [100];
    static float [] multiTargetDanger = new float [100];
    static float [] surroundDanger = new float [100];

    static int [] winnerScore = new int [1000];
    static int [] loserScore = new int [1000];

    static float [] numberOfCards = new float [40]; // CardsInHand

    static float [] cardNumberWinRatio = new float [60];

    static public float [,,] abilityOnStack; // AbilityType, AbilityArea (0, 2, 6 fields), stackNumber;
    static public float [,,] abilityOnRow;
    static public float [,,] tokenOnRow;

    static public float [,,,] abilityAbilitySynergy;// AbilityType, AbilityArea (0, 2, 6 fields), AbilityType, AbilityArea (0, 2, 6 fields);
    static public float [,,,] abilityTokenSynergy;
    static public float [,,,] abilityAfterAbility;
    static public float [,,,] abilityAfterToken;
    static public float [,,,] tokenAfterAbility;
    static public float [,,,] tokenAfterToken;

    static RatingClass () {
        int availableAbilities = AppDefaults.AvailableAbilities;
        int availableTokens = AppDefaults.AvailableTokens;
        abilityOnStack = new float [availableAbilities, 3, 10];
        abilityOnRow = new float [availableAbilities, 3, 10];
        tokenOnRow = new float [availableTokens, 9, 10];
        abilityAbilitySynergy = new float [availableAbilities, 3, availableAbilities, 3];
        abilityAfterAbility = new float [availableAbilities, 3, availableAbilities, 3];
        abilityAfterToken = new float [availableAbilities, 3, availableTokens, 9];
        tokenAfterAbility = new float [availableTokens, 9, availableAbilities, 3];
        tokenAfterToken = new float [availableTokens, 9, availableTokens, 9];

        for (int x = 0; x < abilityOnStack.GetLength (0); x++) {
            for (int y = 0; y < abilityOnStack.GetLength (1); y++) {
                for (int z = 0; z < abilityOnStack.GetLength (2); z++) {
                    abilityOnStack [x, y, z] = 0.5f;
                    //AbilityOnRow [x, y, z] = 0.5f;
                }
            }
        }
        LoadAbilityOnRow ();
        LoadTokenOnRow ();
        for (int x = 0; x < cardNumberWinRatio.Length; x++) {
            cardNumberWinRatio [x] = 0.5f;
        }
        for (int x = 0; x < numberOfCards.Length; x++) {
            numberOfCards [x] = 0.5f;
        }
        for (int x = 0; x < edgeDanger.Length; x++) {
            edgeDanger [x] = 0.5f;
            multiTargetDanger [x] = 0.5f;
            surroundDanger [x] = 0.5f;
        }
        for (int x = 0; x < mapPlayer.GetLength (0); x++) {
            for (int y = 0; y < mapPlayer.GetLength (1); y++) {
                mapPlayer [x, y] = 0.5f;
            }
        }
        LoadAbilityAbilitySynergy ();
        LoadAbilityAfterAbility ();
        LoadAbilityAfterToken ();
        LoadTokenAfterAbility ();
        LoadTokenAfterToken ();
    }

    static public void AnalyzeStatistics (MatchClass match) { // anal...
        if (match.winner == null) {
            return;
        }
        int winnerNumber = match.winner.properties.playerNumber;
        winner [winnerNumber] ++;
        winnerScore [match.winner.score]++;
        for (int x = 1; x <= match.numberOfPlayers; x++) {
            List<CardClass> usedCards = new List<CardClass> ();
            PlayerClass player = match.Player [x];
            int numberOfCards = 0;
            HandClass hand = player.properties.hand;
            for (int y = 0; y < hand.stack.Length; y++) {
                StackClass stack = hand.stack [y];
                int cardCount = stack.card.Count;
                numberOfCards += cardCount;
                for (int z = 0; z < cardCount; z++) {
                    CardClass card = stack.card [z];
                    int abilityArea = card.AreaSize ();
                    int abilityType = card.abilityType;
                    int tokenType = card.tokenType;
                    int tokenValue = card.value;
                    foreach (CardClass usedCard in usedCards) {
                        abilityAbilitySynergy [
                            Mathf.Min (abilityType, usedCard.abilityType),
                            Mathf.Min (abilityArea, usedCard.AreaSize ()), 
                            Mathf.Max (abilityType, usedCard.abilityType),
                            Mathf.Min (abilityArea, usedCard.AreaSize ())] *= 0.999f;
                        if (winnerNumber == x) {
                            abilityAbilitySynergy [
                            Mathf.Min (abilityType, usedCard.abilityType),
                            Mathf.Min (abilityArea, usedCard.AreaSize ()),
                            Mathf.Max (abilityType, usedCard.abilityType),
                            Mathf.Min (abilityArea, usedCard.AreaSize ())] += 0.001f;
                        }
                    }
                    if (z > 0) {
                        CardClass prevCard = stack.card [z - 1];
                        abilityAfterAbility [abilityType, abilityArea, prevCard.abilityType, prevCard.AreaSize ()] *= 0.999f;
                        abilityAfterToken [abilityType, abilityArea, prevCard.tokenType, prevCard.value] *= 0.999f;
                        tokenAfterAbility [tokenType, tokenValue, prevCard.abilityType, prevCard.AreaSize ()] *= 0.999f;
                        tokenAfterToken [tokenType, tokenValue, prevCard.tokenType, prevCard.value] *= 0.999f;
                        if (winnerNumber == x) {
                            abilityAfterAbility [abilityType, abilityArea, prevCard.abilityType, prevCard.AreaSize ()] += 0.001f;
                            abilityAfterToken [abilityType, abilityArea, prevCard.tokenType, prevCard.value] += 0.001f;
                            tokenAfterAbility [tokenType, tokenValue, prevCard.abilityType, prevCard.AreaSize ()] += 0.001f;
                            tokenAfterToken [tokenType, tokenValue, prevCard.tokenType, prevCard.value] += 0.001f;
                        }
                    }
                    usedCards.Add (card);
                    abilityOnStack [abilityType, abilityArea, y] *= 0.999f;
                    abilityOnRow [abilityType, abilityArea, z] *= 0.999f;
                    tokenOnRow [tokenType, tokenValue, z] *= 0.999f;
                    cardNumberWinRatio [card.cardNumber] *= 0.999f;
                    if (winnerNumber == x) {
                        abilityOnStack [abilityType, abilityArea, y] += 0.001f;
                        abilityOnRow [abilityType, abilityArea, z] += 0.001f;
                        tokenOnRow [tokenType, tokenValue, z] += 0.001f;
                        cardNumberWinRatio [card.cardNumber] += 0.001f;
                    }
                }
            }
            AIClass AI = player.properties.AI;
            mapPlayer [match.Board.boardTemplateId, x] *= 0.999f;
            RatingClass.numberOfCards [numberOfCards] *= 0.999f;
            edgeDanger [AI.edgeDanger] *= 0.999f;
            multiTargetDanger [AI.multiTargetDanger] *= 0.999f;
            surroundDanger [AI.surroundDanger] *= 0.999f;
            if (winnerNumber == x) {
                mapPlayer [match.Board.boardTemplateId, x] += 0.001f;
                RatingClass.numberOfCards [numberOfCards] += 0.001f;
                edgeDanger [AI.edgeDanger] += 0.001f;
                multiTargetDanger [AI.multiTargetDanger] += 0.001f;
                surroundDanger [AI.surroundDanger] += 0.001f;
            } else {
                loserScore [player.score]++;
            }

        }
        turn [match.turn]++;
    }

    static public void SaveEverything () {
        Debug.Log ("Test");
        SaveAbilityOnStack ();
        SaveAbilityOnRow ();
        SaveTokenOnRow ();
        SaveWinnerScore ();
        SaveLoserScore ();
        SavePlayerWinRatio ();
        SaveCardNumberWinRatio ();
        SaveTurn ();
        SaveEdgeDanger ();
        SaveMultiTargetDanger ();
        SaveSurroundDanger ();
        SaveMapPlayer ();
        SaveNumberOfCards ();
        SaveAbilityAbilitySynergy ();
        SaveAbilityAfterAbility ();
        SaveAbilityAfterToken ();
        SaveTokenAfterAbility ();
        SaveTokenAfterToken ();
    }

    static public void SaveAbilityOnStack () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < abilityOnStack.GetLength (0); x++) {
            string s = "[" + x.ToString () + "] ";
            for (int y = 0; y < abilityOnStack.GetLength (1); y++) {
                s += "[" + y.ToString () + "] ";
                for (int z = 0; z < abilityOnStack.GetLength (2); z++) {
                    s += abilityOnStack [x, y, z].ToString () + " ";
                }
            }
            lines.Add (s);
        }
        ServerData.SaveRatingAbilityOnStack (lines.ToArray ());
    }

    static public void SaveAbilityOnRow () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < abilityOnRow.GetLength (0); x++) {
            string s = "[" + x.ToString () + "] ";
            for (int y = 0; y < abilityOnRow.GetLength (1); y++) {
                s += "[" + y.ToString () + "] ";
                for (int z = 0; z < abilityOnRow.GetLength (2); z++) {
                    s += abilityOnRow [x, y, z].ToString () + " ";
                }
            }
            lines.Add (s);
        }
        ServerData.SaveRatingAbilityOnRow (lines.ToArray ());
    }

    static public void LoadAbilityOnRow () {
        string [] lines = ServerData.GetRatingAbilityOnRow ();
        for (int x = 0; x < abilityOnRow.GetLength (0); x++) {
            string [] word = null;
            if (lines != null && x < lines.Length) {
                word = lines [x].Split (new char [] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }
            for (int y = 0; y < abilityOnRow.GetLength (1); y++) {
                for (int z = 0; z < abilityOnRow.GetLength (2); z++) {
                    int number = 2 + y * (abilityOnRow.GetLength (2) + 1) + z;
                    if (word != null && number < word.Length) {
                        abilityOnRow [x, y, z] = float.Parse (word [number]);
                    } else {
                        abilityOnRow [x, y, z] = 0.5f;
                    }
                }
            }
        }
    }

    static public void SaveTokenOnRow () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < tokenOnRow.GetLength (0); x++) {
            string s = "[" + x.ToString () + "] ";
            for (int y = 0; y < tokenOnRow.GetLength (1); y++) {
                s += "[" + y.ToString () + "] ";
                for (int z = 0; z < tokenOnRow.GetLength (2); z++) {
                    s += tokenOnRow [x, y, z].ToString () + " ";
                }
            }
            lines.Add (s);
        }
        ServerData.SaveRatingTokenOnRow (lines.ToArray ());
    }

    static public void LoadTokenOnRow () {
        string [] lines = ServerData.GetRatingTokenOnRow ();
        for (int x = 0; x < tokenOnRow.GetLength (0); x++) {
            string [] word = null;
            if (lines != null && x < lines.Length) {
                word = lines [x].Split (new char [] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }
            for (int y = 0; y < tokenOnRow.GetLength (1); y++) {
                for (int z = 0; z < tokenOnRow.GetLength (2); z++) {
                    int number = 2 + y * (tokenOnRow.GetLength (2) + 1) + z;
                    if (word != null && number < word.Length) {
                        tokenOnRow [x, y, z] = float.Parse (word [number]);
                    } else {
                        tokenOnRow [x, y, z] = 0.5f;
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

    static public void SaveCardNumberWinRatio () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < cardNumberWinRatio.Length; x++) {
            lines.Add ("[" + x.ToString () + "] " + cardNumberWinRatio [x].ToString ());
        }
        ServerData.SaveRatingCardNumberWinRatio (lines.ToArray ());
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

    static public void SaveMapPlayer () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < mapPlayer.GetLength (0); x++) {
            string s = "[" + x.ToString () + "] ";
            for (int y = 0; y < mapPlayer.GetLength (1); y++) {
                s += mapPlayer [x, y].ToString () + " ";
            }
            lines.Add (s);
        }
        ServerData.SaveRatingMapPlayer (lines.ToArray ());
    }

    static public void SaveNumberOfCards () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < numberOfCards.Length; x++) {
            lines.Add ("[" + x.ToString () + "] " + numberOfCards [x].ToString ());
        }
        ServerData.SaveRatingNumberOfCards (lines.ToArray ());
    }

    static public void SaveAbilityAbilitySynergy () {
        ServerData.SaveRatingAbilityAbilitySynergy (Save (abilityAbilitySynergy));
    }

    static public void LoadAbilityAbilitySynergy () {
        string [] lines = ServerData.GetRatingAbilityAbilitySynergy ();
        for (int x = 0; x < abilityAbilitySynergy.GetLength (0); x++) {
            string [] word = null;
            int l1 = abilityAbilitySynergy.GetLength (1);
            for (int x2 = 0; x2 < l1; x2++) {
                int lineNumber = 1 + x * (l1 + 1) + x2;
                if (lines != null && lineNumber < lines.Length) {
                    word = lines [lineNumber].Split (new char [] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                }
                for (int y = 0; y < abilityAbilitySynergy.GetLength (2); y++) {
                    int l2 = abilityAbilitySynergy.GetLength (3);
                    for (int y2 = 0; y2 < l2; y2++) {
                        int number = 2 + y * (l2 + 1) + y2;
                        if (word != null && number < word.Length) {
                            abilityAbilitySynergy [x, x2, y, y2] = float.Parse (word [number]);
                        } else {
                            abilityAbilitySynergy [x, x2, y, y2] = 0.5f;
                        }
                    }
                }
            }
        }
    }

    static public void SaveAbilityAfterAbility () {
        ServerData.SaveRatingAbilityAfterAbility (Save (abilityAfterAbility));
    }

    static public void LoadAbilityAfterAbility () {
        string [] lines = ServerData.GetRatingAbilityAfterAbility ();
        Load (lines, abilityAfterAbility);
    }

    static public void SaveAbilityAfterToken () {
        ServerData.SaveRatingAbilityAfterToken (Save (abilityAfterToken));
    }


    static public void LoadAbilityAfterToken () {
        string [] lines = ServerData.GetRatingAbilityAfterToken ();
        Load (lines, abilityAfterToken);
    }

    static public void SaveTokenAfterAbility () {
        ServerData.SaveRatingTokenAfterAbility (Save (tokenAfterAbility));
    }


    static public void LoadTokenAfterAbility () {
        string [] lines = ServerData.GetRatingTokenAfterAbility ();
        Load (lines, tokenAfterAbility);
    }

    static public void SaveTokenAfterToken () {
        ServerData.SaveRatingTokenAfterToken (Save (tokenAfterToken));
    }


    static public void LoadTokenAfterToken () {
        string [] lines = ServerData.GetRatingTokenAfterToken ();
        Load (lines, tokenAfterToken);
    }

    static public void Load (string [] lines, float [,,,] array) {
        for (int x = 0; x < array.GetLength (0); x++) {
            string [] word = null;
            int l1 = array.GetLength (1);
            for (int x2 = 0; x2 < l1; x2++) {
                int lineNumber = 1 + x * (l1 + 1) + x2;
                if (lines != null && lineNumber < lines.Length) {
                    word = lines [lineNumber].Split (new char [] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                }
                for (int y = 0; y < array.GetLength (2); y++) {
                    int l2 = array.GetLength (3);
                    for (int y2 = 0; y2 < l2; y2++) {
                        int number = 2 + y * (l2 + 1) + y2;
                        if (word != null && number < word.Length) {
                            array [x, x2, y, y2] = float.Parse (word [number]);
                        } else {
                            array [x, x2, y, y2] = 0.5f;
                        }
                    }
                }
            }
        }
    }
    static public string [] Save (float [,,,] array) {
        List<string> lines = new List<string> ();
        for (int x = 0; x < array.GetLength (0); x++) {
            string s = "[" + x.ToString () + "] ";
            lines.Add (s);
            for (int x2 = 0; x2 < array.GetLength (1); x2++) {
                s = "[" + x2.ToString () + "] ";
                for (int y = 0; y < array.GetLength (2); y++) {
                    s += "[" + y.ToString () + "] ";
                    for (int y2 = 0; y2 < array.GetLength (3); y2++) {
                        s += array [x, x2, y, y2].ToString () + " ";
                    }
                }
                lines.Add (s);
            }
        }
        return lines.ToArray ();
    }


}
