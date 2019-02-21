using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatingClass {

    static bool CreateBackUp = true;

    static int [] winner = new int [5];
    static int [] turn = new int [42];

    static float [,] mapPlayer = new float [400, 5];

    const int AICount = 100;

    static float [] edgeDanger = new float [AICount];
    static float [] multiTargetDanger = new float [AICount];
    static float [] surroundDanger = new float [AICount];

    static float [] AIabilityRow = new float [AICount];
    static float [] AItokenRow = new float [AICount];
    static float [] AIabilityTokenRow = new float [AICount];
    static float [] AIabilityStackSize = new float [AICount];
    static float [] AItokenStackSize = new float [AICount];
    static float [] AIabilityAfterAbility = new float [AICount];
    static float [] AIabilityAfterToken = new float [AICount];
    static float [] AItokenAfterAbility = new float [AICount];
    static float [] AItokenAfterToken = new float [AICount];
    static float [] AIabilityAbilitySynergy = new float [AICount];
    static float [] AIabilityTokenSynergy = new float [AICount];

    static int [] winnerScore = new int [1000];
    static int [] loserScore = new int [1000];

    static float [] numberOfCards = new float [40]; // CardsInHand

    static public float [] cardPopularity = new float [100];
    static float [] cardNumberWinRatio = new float [100];

    static public float [,,] abilityOnStack; // AbilityType, AbilityArea (0, 2, 6 fields), stackNumber;
    static public float [,,] abilityOnRow;
    static public float [,,] abilityStackSize;
    static public float [,,] tokenOnRow;
    static public float [,,] tokenStackSize;
    static public float [,,] abilityTokenOnRow; // AbilityType, TokenType, rowNumber

    static public float [,,,] abilityAbilitySynergy;// AbilityType, AbilityArea (0, 2, 6 fields), AbilityType, AbilityArea (0, 2, 6 fields);
    static public float [,,,] abilityTokenSynergy;
    static public float [,,,] abilityAfterAbility;
    static public float [,,,] abilityAfterToken;
    static public float [,,,] tokenAfterAbility;
    static public float [,,,] tokenAfterToken;

    static public float [,,,] abilityAgainstAbility;
    static public float [,,,] abilityAgainstToken;
    static public float [,,,] tokenAgainstAbility;
    static public float [,,,] tokenAgainstToken;


    static RatingClass () {
        int availableAbilities = AppDefaults.AvailableAbilities;
        int availableTokens = AppDefaults.AvailableTokens;
        abilityOnStack = new float [availableAbilities, 3, 10];
        abilityOnRow = new float [availableAbilities, 3, 10];
        abilityStackSize = new float [availableAbilities, 3, 10];
        tokenOnRow = new float [availableTokens, 9, 10];
        tokenStackSize = new float [availableTokens, 9, 10];
        abilityTokenOnRow = new float [availableAbilities, availableTokens, 10];

        abilityAbilitySynergy = new float [availableAbilities, 3, availableAbilities, 3];
        abilityTokenSynergy = new float [availableAbilities, 3, availableTokens, 9];
        abilityAfterAbility = new float [availableAbilities, 3, availableAbilities, 3];
        abilityAfterToken = new float [availableAbilities, 3, availableTokens, 9];
        tokenAfterAbility = new float [availableTokens, 9, availableAbilities, 3];
        tokenAfterToken = new float [availableTokens, 9, availableTokens, 9];

        abilityAgainstAbility = new float [availableAbilities, 3, availableAbilities, 3];
        abilityAgainstToken = new float [availableAbilities, 3, availableTokens, 9];
        tokenAgainstAbility = new float [availableTokens, 9, availableAbilities, 3];
        tokenAgainstToken = new float [availableTokens, 9, availableTokens, 9];

        for (int x = 0; x < abilityOnStack.GetLength (0); x++) {
            for (int y = 0; y < abilityOnStack.GetLength (1); y++) {
                for (int z = 0; z < abilityOnStack.GetLength (2); z++) {
                    abilityOnStack [x, y, z] = 0.5f;
                    //AbilityOnRow [x, y, z] = 0.5f;
                }
            }
        }
        for (int x = 0; x < abilityTokenOnRow.GetLength (0); x++) {
            for (int y = 0; y < abilityTokenOnRow.GetLength (1); y++) {
                for (int z = 0; z < abilityTokenOnRow.GetLength (2); z++) {
                    abilityTokenOnRow [x, y, z] = 0.5f;
                    //AbilityOnRow [x, y, z] = 0.5f;
                }
            }
        }
        LoadAbilityOnRow ();
        LoadAbilityStackSize ();
        LoadTokenOnRow ();
        LoadTokenStackSize ();
        LoadAbilityTokenOnRow ();
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

            AIabilityRow [x] = 0.5f;
            AItokenRow [x] = 0.5f;
            AIabilityTokenRow [x] = 0.5f;
            AIabilityStackSize [x] = 0.5f;
            AItokenStackSize [x] = 0.5f;
            AIabilityAfterAbility [x] = 0.5f;
            AIabilityAfterToken [x] = 0.5f;
            AItokenAfterAbility [x] = 0.5f;
            AItokenAfterToken [x] = 0.5f;
            AIabilityAbilitySynergy [x] = 0.5f;
            AIabilityTokenSynergy [x] = 0.5f;
        }

        for (int x = 0; x < mapPlayer.GetLength (0); x++) {
            for (int y = 0; y < mapPlayer.GetLength (1); y++) {
                mapPlayer [x, y] = 0.5f;
            }
        }
        LoadAbilityAbilitySynergy ();
        LoadAbilityTokenSynergy ();
        LoadAbilityAfterAbility ();
        LoadAbilityAfterToken ();
        LoadTokenAfterAbility ();
        LoadTokenAfterToken ();

        LoadAbilityAgainstAbility ();
        LoadAbilityAgainstToken ();
        LoadTokenAgainstAbility ();
        LoadTokenAgainstToken ();

        LoadCardPopularity ();
    }

    static public float Normalize (float oldRating, float strength, float enemyPopularity) {
        strength *= 2;
        return oldRating * strength * enemyPopularity / 100f + oldRating * (1 - enemyPopularity / 100f);
    }

    static public float [] PopularityRating (int gameModeId) {
        CardPoolClass cardPool = new CardPoolClass ();
        cardPool.LoadFromFile (gameModeId);

        List<CardClass> card = cardPool.Card;

        int count = card.Count;
        float [] rating = new float [count];
        for (int x = 0; x < count; x++) {
            rating [x] = 1f;
        }

        for (int x = 0; x < count; x++) {
            int abilityType1 = card [x].abilityType;
            int abilityArea1 = card [x].AreaSize ();
            int tokenType1 = card [x].tokenType;
            int tokenValue1 = card [x].value;


            for (int y = 0; y < count; y++) {
                int abilityType2 = card [y].abilityType;
                int abilityArea2 = card [y].AreaSize ();
                int tokenType2 = card [y].tokenType;
                int tokenValue2 = card [y].value;

                float popularity = cardPopularity [y];
                float rat = rating [y];
                rat = Normalize (rat, abilityAgainstAbility [abilityType2, abilityArea2, abilityType1, abilityArea1], popularity);
                rat = Normalize (rat, abilityAgainstToken [abilityType2, abilityArea2, tokenType1, tokenValue1], popularity);
                rat = Normalize (rat, tokenAgainstAbility [tokenType2, tokenValue2, abilityType1, abilityArea1], popularity);
                rat = Normalize (rat, tokenAgainstToken [tokenType2, tokenValue2, tokenType1, tokenValue1], popularity);
                rating [y] = rat;
            }
        }
        return rating;
    }

    static public void AnalyzeStatistics (MatchClass match) { // anal...
        if (match.winner == null) {
            return;
        }
        int winnerNumber = match.winner.properties.playerNumber;
        winner [winnerNumber] ++;
        winnerScore [Mathf.Max (0, match.winner.score)]++;
        for (int x = 0; x < cardPopularity.Length; x++) {
           cardPopularity [x] *= 0.9999f;
        }
        List<CardClass> [] usedCards = new List <CardClass> [match.numberOfPlayers + 1];
        for (int x = 1; x <= match.numberOfPlayers; x++) {
            usedCards [x] = new List<CardClass> ();
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
                    foreach (CardClass usedCard in usedCards [x]) {
                        if (abilityType < usedCard.abilityType) {
                            abilityAbilitySynergy [abilityType, abilityArea, usedCard.abilityType, usedCard.AreaSize ()] *= 0.999f;
                        } else {
                            abilityAbilitySynergy [usedCard.abilityType, usedCard.AreaSize (), abilityType, abilityArea] *= 0.999f;
                        }
                        abilityTokenSynergy [abilityType, abilityArea, usedCard.tokenType, usedCard.value] *= 0.999f;
                        abilityTokenSynergy [usedCard.abilityType, usedCard.AreaSize (), tokenType, tokenValue] *= 0.999f;
                        if (winnerNumber == x) {
                            if (abilityType < usedCard.abilityType) {
                                abilityAbilitySynergy [abilityType, abilityArea, usedCard.abilityType, usedCard.AreaSize ()] += 0.001f;
                            } else {
                                abilityAbilitySynergy [usedCard.abilityType, usedCard.AreaSize (), abilityType, abilityArea] += 0.001f;
                            }
                            abilityTokenSynergy [abilityType, abilityArea, usedCard.tokenType, usedCard.value] += 0.001f;
                            abilityTokenSynergy [usedCard.abilityType, usedCard.AreaSize (), tokenType, tokenValue] += 0.001f;
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
                    usedCards [x].Add (card);
                    abilityOnStack [abilityType, abilityArea, y] *= 0.999f;
                    abilityOnRow [abilityType, abilityArea, z] *= 0.999f;
                    abilityStackSize [abilityType, abilityArea, cardCount] *= 0.999f;
                    tokenOnRow [tokenType, tokenValue, z] *= 0.999f;
                    tokenStackSize [tokenType, tokenValue, cardCount] *= 0.999f;
                    abilityTokenOnRow [abilityType, tokenType, z] *= 0.999f;
                    cardNumberWinRatio [card.cardNumber] *= 0.999f;
                    cardPopularity [card.cardNumber] += 0.005f;
                    if (winnerNumber == x) {
                        abilityOnStack [abilityType, abilityArea, y] += 0.001f;
                        abilityOnRow [abilityType, abilityArea, z] += 0.001f;
                        abilityStackSize [abilityType, abilityArea, cardCount] += 0.001f;
                        tokenOnRow [tokenType, tokenValue, z] += 0.001f;
                        tokenStackSize [tokenType, tokenValue, cardCount] += 0.001f;
                        abilityTokenOnRow [abilityType, tokenType, z] += 0.001f;
                        cardNumberWinRatio [card.cardNumber] += 0.001f;
                    }
                }
            }

            usedCards [x] = new List<CardClass> ();

            AIClass AI = player.properties.AI;
            mapPlayer [match.Board.boardTemplateId, x] *= 0.999f;
            RatingClass.numberOfCards [numberOfCards] *= 0.999f;

            edgeDanger [AI.edgeDanger] *= 0.999f;
            multiTargetDanger [AI.multiTargetDanger] *= 0.999f;
            surroundDanger [AI.surroundDanger] *= 0.999f;

            AIabilityRow [AI.abilityRow] *= 0.999f;
            AItokenRow [AI.tokenRow] *= 0.999f;
            AIabilityTokenRow [AI.abilityTokenRow] *= 0.999f;
            AIabilityStackSize [AI.abilityStackSize] *= 0.999f;
            AItokenStackSize [AI.tokenStackSize] *= 0.999f;
            AIabilityAfterAbility [AI.abilityAfterAbility] *= 0.999f;
            AIabilityAfterToken [AI.abilityAfterToken] *= 0.999f;
            AItokenAfterAbility [AI.tokenAfterAbility] *= 0.999f;
            AItokenAfterToken [AI.tokenAfterToken] *= 0.999f;
            AIabilityAbilitySynergy [AI.abilityAbilitySynergy] *= 0.999f;
            AIabilityTokenSynergy [AI.abilityTokenSynergy] *= 0.999f;

            if (winnerNumber == x) {
                mapPlayer [match.Board.boardTemplateId, x] += 0.001f;
                RatingClass.numberOfCards [numberOfCards] += 0.001f;

                edgeDanger [AI.edgeDanger] += 0.001f;
                multiTargetDanger [AI.multiTargetDanger] += 0.001f;
                surroundDanger [AI.surroundDanger] += 0.001f;

                AIabilityRow [AI.abilityRow] += 0.001f;
                AItokenRow [AI.tokenRow] += 0.001f;
                AIabilityTokenRow [AI.abilityTokenRow] += 0.001f;
                AIabilityStackSize [AI.abilityStackSize] += 0.001f;
                AItokenStackSize [AI.tokenStackSize] += 0.001f;
                AIabilityAfterAbility [AI.abilityAfterAbility] += 0.001f;
                AIabilityAfterToken [AI.abilityAfterToken] += 0.001f;
                AItokenAfterAbility [AI.tokenAfterAbility] += 0.001f;
                AItokenAfterToken [AI.tokenAfterToken] += 0.001f;
                AIabilityAbilitySynergy [AI.abilityAbilitySynergy] += 0.001f;
                AIabilityTokenSynergy [AI.abilityTokenSynergy] += 0.001f;
            } else {
                loserScore [Mathf.Max (0, player.score)]++;
            }

        }
        turn [match.turn]++;
        int loserNumber = 0;
        for (int x = 1; x <= match.numberOfPlayers; x++) {
            for (int y = 1; y <= match.numberOfPlayers; y++) {
                if (x == y || y == winnerNumber) {
                    continue;
                }
                loserNumber = y;
                foreach (CardClass loserCard in usedCards [loserNumber]) {
                    int abilityType1 = loserCard.abilityType;
                    int abilityArea1 = loserCard.AreaSize ();
                    int tokenType1 = loserCard.tokenType;
                    int tokenValue1 = loserCard.value;
                    foreach (CardClass winnerCard in usedCards [winnerNumber]) {
                        int abilityType2 = winnerCard.abilityType;
                        int abilityArea2 = winnerCard.AreaSize ();
                        int tokenType2 = winnerCard.tokenType;
                        int tokenValue2 = winnerCard.value;
                        abilityAgainstAbility [abilityType2, abilityArea2, abilityType1, abilityArea1] *= 0.999f;
                        abilityAgainstToken [abilityType2, abilityArea2, tokenType1, tokenValue1] *= 0.999f;
                        tokenAgainstAbility [tokenType2, tokenValue2, abilityType1, abilityArea1] *= 0.999f;
                        tokenAgainstToken [tokenType2, tokenValue2, tokenType1, tokenValue1] *= 0.999f;
                        abilityAgainstAbility [abilityType1, abilityArea1, abilityType2, abilityArea2] *= 0.999f;
                        abilityAgainstToken [abilityType1, abilityArea1, tokenType2, tokenValue2] *= 0.999f;
                        tokenAgainstAbility [tokenType1, tokenValue1, abilityType2, abilityArea2] *= 0.999f;
                        tokenAgainstToken [tokenType1, tokenValue1, tokenType2, tokenValue2] *= 0.999f;
                        abilityAgainstAbility [abilityType2, abilityArea2, abilityType1, abilityArea1] += 0.001f;
                        abilityAgainstToken [abilityType2, abilityArea2, tokenType1, tokenValue1] += 0.001f;
                        tokenAgainstAbility [tokenType2, tokenValue2, abilityType1, abilityArea1] += 0.001f;
                        tokenAgainstToken [tokenType2, tokenValue2, tokenType1, tokenValue1] += 0.001f;
                    }
                }
            }
        }
    }

    static public void SaveEverything () {
        Debug.Log ("Test");
        SaveAbilityOnStack ();
        SaveAbilityOnRow ();
        SaveAbilityStackSize ();
        SaveTokenOnRow ();
        SaveTokenStackSize ();
        SaveAbilityTokenOnRow ();
        SaveWinnerScore ();
        SaveLoserScore ();
        SavePlayerWinRatio ();
        SaveCardNumberWinRatio ();
        SaveCardPopularity ();
        SaveTurn ();
        SaveAISettings ();
        SaveEdgeDanger ();
        SaveMultiTargetDanger ();
        SaveSurroundDanger ();
        SaveMapPlayer ();
        SaveNumberOfCards ();
        SaveAbilityAbilitySynergy ();
        SaveAbilityTokenSynergy ();
        SaveAbilityAfterAbility ();
        SaveAbilityAfterToken ();
        SaveTokenAfterAbility ();
        SaveTokenAfterToken ();

        SaveAbilityAgainstAbility ();
        SaveAbilityAgainstToken ();
        SaveTokenAgainstAbility ();
        SaveTokenAgainstToken ();
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
        Load (lines, abilityOnRow);
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
        Load (lines, tokenOnRow);
    }

    static public void SaveAbilityTokenOnRow () {
        ServerData.SaveRatingAbilityTokenOnRow (Save (abilityTokenOnRow));
    }
    
    static public void LoadAbilityTokenOnRow () {
        string [] lines = ServerData.GetRatingAbilityTokenOnRow ();
        Load (lines, abilityTokenOnRow);
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

    static public void SaveCardPopularity () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < cardPopularity.Length; x++) {
            lines.Add ("[" + x.ToString () + "] " + cardPopularity [x].ToString ());
        }
        ServerData.SaveRatingCardPopularity (lines.ToArray ());
    }

    static public void LoadCardPopularity () {
        string [] lines = ServerData.GetRatingPopularity ();
        Load (lines, cardPopularity);
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

    static public void SaveAISettings () {
        List<string> lines = new List<string> ();
        string s;
        s = "edgeDanger: ";
        for (int x = 0; x < AICount; x++) {
            s += " [" + x.ToString () + "] " + edgeDanger [x].ToString ();
        }
        lines.Add (s);
        s = "multiTargetDanger: ";
        for (int x = 0; x < AICount; x++) {
            s += " [" + x.ToString () + "] " + multiTargetDanger [x].ToString ();
        }
        lines.Add (s);
        s = "surroundDanger: ";
        for (int x = 0; x < AICount; x++) {
            s += " [" + x.ToString () + "] " + surroundDanger [x].ToString ();
        }
        lines.Add (s);
        s = "AIabilityRow: ";
        for (int x = 0; x < AICount; x++) {
            s += " [" + x.ToString () + "] " + AIabilityRow [x].ToString ();
        }
        lines.Add (s);
        s = "AItokenRow: ";
        for (int x = 0; x < AICount; x++) {
            s += " [" + x.ToString () + "] " + AItokenRow [x].ToString ();
        }
        lines.Add (s);
        s = "AIabilityTokenRow: ";
        for (int x = 0; x < AICount; x++) {
            s += " [" + x.ToString () + "] " + AIabilityTokenRow [x].ToString ();
        }
        lines.Add (s);
        s = "AIabilityStackSize: ";
        for (int x = 0; x < AICount; x++) {
            s += " [" + x.ToString () + "] " + AIabilityStackSize [x].ToString ();
        }
        lines.Add (s);
        s = "AItokenStackSize: ";
        for (int x = 0; x < AICount; x++) {
            s += " [" + x.ToString () + "] " + AItokenStackSize [x].ToString ();
        }
        lines.Add (s);
        s = "AIabilityAfterAbility: ";
        for (int x = 0; x < AICount; x++) {
            s += " [" + x.ToString () + "] " + AIabilityAfterAbility [x].ToString ();
        }
        lines.Add (s);
        s = "AIabilityAfterToken: ";
        for (int x = 0; x < AICount; x++) {
            s += " [" + x.ToString () + "] " + AIabilityAfterToken [x].ToString ();
        }
        lines.Add (s);
        s = "AItokenAfterAbility: ";
        for (int x = 0; x < AICount; x++) {
            s += " [" + x.ToString () + "] " + AItokenAfterAbility [x].ToString ();
        }
        lines.Add (s);
        s = "AItokenAfterToken: ";
        for (int x = 0; x < AICount; x++) {
            s += " [" + x.ToString () + "] " + AItokenAfterToken [x].ToString ();
        }
        lines.Add (s);
        s = "AIabilityAbilitySynergy: ";
        for (int x = 0; x < AICount; x++) {
            s += " [" + x.ToString () + "] " + AIabilityAbilitySynergy [x].ToString ();
        }
        lines.Add (s);
        s = "AIabilityTokenSynergy: ";
        for (int x = 0; x < AICount; x++) {
            s += " [" + x.ToString () + "] " + AIabilityTokenSynergy [x].ToString ();
        }
        lines.Add (s);
        ServerData.SaveRatingAISettings (lines.ToArray ());
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
        Load (lines, abilityAbilitySynergy);
    }


    static public void LoadAbilityTokenSynergy () {
        string [] lines = ServerData.GetRatingAbilityTokenSynergy ();
        Load (lines, abilityTokenSynergy);
    }


    static public void SaveAbilityTokenSynergy () {
        ServerData.SaveRatingAbilityTokenSynergy (Save (abilityTokenSynergy));
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
    
    static public void LoadAbilityStackSize () {
        string [] lines = ServerData.GetRatingAbilityStackSize ();
        Load (lines, abilityStackSize);
    }

    static public void SaveAbilityStackSize () {
        ServerData.SaveRatingAbilityStackSize (Save (abilityStackSize));
    }

    static public void LoadTokenStackSize () {
        string [] lines = ServerData.GetRatingTokenStackSize ();
        Load (lines, tokenStackSize);
    }

    static public void SaveTokenStackSize () {
        ServerData.SaveRatingTokenStackSize (Save (tokenStackSize));
    }

    static public void SaveAbilityAgainstAbility () {
        ServerData.SaveRatingAbilityAgainstAbility (Save (abilityAgainstAbility));
    }

    static public void LoadAbilityAgainstAbility () {
        string [] lines = ServerData.GetRatingAbilityAgainstAbility ();
        Load (lines, abilityAgainstAbility);
    }

    static public void SaveAbilityAgainstToken () {
        ServerData.SaveRatingAbilityAgainstToken (Save (abilityAgainstToken));
    }

    static public void LoadAbilityAgainstToken () {
        string [] lines = ServerData.GetRatingAbilityAgainstToken ();
        Load (lines, abilityAgainstToken);
    }

    static public void SaveTokenAgainstAbility () {
        ServerData.SaveRatingTokenAgainstAbility (Save (tokenAgainstAbility));
    }

    static public void LoadTokenAgainstAbility () {
        string [] lines = ServerData.GetRatingTokenAgainstAbility ();
        Load (lines, tokenAgainstAbility);
    }

    static public void SaveTokenAgainstToken () {
        ServerData.SaveRatingTokenAgainstToken (Save (tokenAgainstToken));
    }

    static public void LoadTokenAgainstToken () {
        string [] lines = ServerData.GetRatingTokenAgainstToken ();
        Load (lines, tokenAgainstToken);
    }

    static float minValue = 0.42f;

    static public void Load (string [] lines, float [] array) {
        for (int x = 0; x < array.GetLength (0); x++) {
            string [] word = null;
            if (lines != null && x < lines.Length) {
                word = lines [x].Split (new char [] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }
            if (word != null) {
                array [x] = float.Parse (word [1]);
            } else {
                array [x] = 0.5f;
            }
            if (array != cardPopularity) {
                array [x] = Mathf.Min (array [x], minValue);
            }
        }
    }

    static public void Load (string [] lines, float [,,] array) {
        for (int x = 0; x < array.GetLength (0); x++) {
            string [] word = null;
            int l1 = array.GetLength (1);
            if (lines != null && x < lines.Length) {
                word = lines [x].Split (new char [] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }
            for (int y = 0; y < l1; y++) {
                int l2 = array.GetLength (2);
                for (int y2 = 0; y2 < l2; y2++) {
                    int number = 2 + y * (l2 + 1) + y2;
                    if (word != null && number < word.Length) {
                        array [x, y, y2] = float.Parse (word [number]);
                    } else {
                        array [x, y, y2] = 0.5f;
                    }
                    array [x, y, y2] = Mathf.Min (array [x, y, y2], minValue);
                }
            }
        }
    }

    static public string [] Save (float [,,] array) {
        List<string> lines = new List<string> ();
        for (int x = 0; x < array.GetLength (0); x++) {
            string s = "[" + x.ToString () + "] ";
            for (int y = 0; y < array.GetLength (1); y++) {
                s += "[" + y.ToString () + "] ";
                for (int y2 = 0; y2 < array.GetLength (2); y2++) {
                    s += array [x, y, y2].ToString () + " ";
                }
            }
            lines.Add (s);
        }
        return lines.ToArray ();
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
                        array [x, x2, y, y2] = Mathf.Min (array [x, x2, y, y2], minValue);
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
