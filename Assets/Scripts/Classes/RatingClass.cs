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
    static float [] AIabilityTokenStackSize = new float [AICount];


    static float [] AIabilityAfterAbility = new float [AICount];
    static float [] AIabilityAfterToken = new float [AICount];
    static float [] AItokenAfterAbility = new float [AICount];
    static float [] AItokenAfterToken = new float [AICount];

    static float [] AIability_AbilitySynergy = new float [AICount];
    static float [] AIability_TokenSynergy = new float [AICount];
    static float [] AItoken_TokenSynergy = new float [AICount];

    static int [] winnerScore = new int [1000];
    static int [] loserScore = new int [1000];

    static float [] numberOfCards = new float [40]; // CardsInHand

    static public float [] cardPopularity = new float [100];
    static float [] cardNumberWinRatio = new float [100];

    static public float [,,] abilityOnStack; // AbilityType, AbilityArea (0, 2, 6 fields), stackNumber;
    static public float [,,] abilityOnRow;
    static public float [,,] tokenOnRow;
    static public float [,,] abilityTokenOnRow; // AbilityType, TokenType, rowNumber

    static public float [,,] abilityStackSize;
    static public float [,,] tokenStackSize;
    static public float [,,] abilityTokenStackSize;

    static public float [,,,] ability_AbilitySynergy;// AbilityType, AbilityArea (0, 2, 6 fields), AbilityType, AbilityArea (0, 2, 6 fields);
    static public float [,,,] ability_TokenSynergy;
    static public float [,,,] token_TokenSynergy;

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
        tokenOnRow = new float [availableTokens, 9, 10];
        abilityTokenOnRow = new float [availableAbilities, availableTokens, 10];

        abilityStackSize = new float [availableAbilities, 3, 10];
        tokenStackSize = new float [availableTokens, 9, 10];
        abilityTokenStackSize = new float [availableAbilities, availableTokens, 10];

        ability_AbilitySynergy = new float [availableAbilities, 3, availableAbilities, 3];
        ability_TokenSynergy = new float [availableAbilities, 3, availableTokens, 9];
        token_TokenSynergy = new float [availableTokens, 9, availableTokens, 9];

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

        LoadAbilityOnRow ();
        LoadTokenOnRow ();
        LoadAbilityTokenOnRow ();

        LoadAbilityStackSize ();
        LoadTokenStackSize ();
        LoadAbilityTokenStackSize ();

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
            AIabilityTokenStackSize [x] = 0.5f;

            AIabilityAfterAbility [x] = 0.5f;
            AIabilityAfterToken [x] = 0.5f;
            AItokenAfterAbility [x] = 0.5f;
            AItokenAfterToken [x] = 0.5f;

            AIability_AbilitySynergy [x] = 0.5f;
            AIability_TokenSynergy [x] = 0.5f;
            AItoken_TokenSynergy [x] = 0.5f;
        }

        for (int x = 0; x < mapPlayer.GetLength (0); x++) {
            for (int y = 0; y < mapPlayer.GetLength (1); y++) {
                mapPlayer [x, y] = 0.5f;
            }
        }

        LoadAbility_AbilitySynergy ();
        LoadAbility_TokenSynergy ();
        LoadToken_TokenSynergy ();

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
            int tokenValue1 = card [x].tokenValue;


            for (int y = 0; y < count; y++) {
                int abilityType2 = card [y].abilityType;
                int abilityArea2 = card [y].AreaSize ();
                int tokenType2 = card [y].tokenType;
                int tokenValue2 = card [y].tokenValue;

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
            HandClass hand = player.properties.startingHand;
            for (int y = 0; y < hand.stack.Length; y++) {
                StackClass stack = hand.stack [y];
                int cardCount = stack.card.Count;
                numberOfCards += cardCount;
                for (int z = 0; z < cardCount; z++) {
                    CardClass card = stack.card [z];
                    int abilityArea = card.AreaSize ();
                    int abilityType = card.abilityType;
                    int tokenType = card.tokenType;
                    int tokenValue = card.tokenValue;
                    foreach (CardClass usedCard in usedCards [x]) {
                        if (abilityType < usedCard.abilityType) {
                            ability_AbilitySynergy [abilityType, abilityArea, usedCard.abilityType, usedCard.AreaSize ()] *= 0.999f;
                        } else {
                            ability_AbilitySynergy [usedCard.abilityType, usedCard.AreaSize (), abilityType, abilityArea] *= 0.999f;
                        }
                        if (tokenType < usedCard.tokenType) {
                            token_TokenSynergy [tokenType, tokenValue, usedCard.tokenType, usedCard.tokenValue] *= 0.999f;
                        } else {
                            token_TokenSynergy [usedCard.tokenType, usedCard.tokenValue, tokenType, tokenValue] *= 0.999f;
                        }
                        ability_TokenSynergy [abilityType, abilityArea, usedCard.tokenType, usedCard.tokenValue] *= 0.999f;
                        ability_TokenSynergy [usedCard.abilityType, usedCard.AreaSize (), tokenType, tokenValue] *= 0.999f;
                        if (winnerNumber == x) {
                            if (abilityType < usedCard.abilityType) {
                                ability_AbilitySynergy [abilityType, abilityArea, usedCard.abilityType, usedCard.AreaSize ()] += 0.001f;
                            } else {
                                ability_AbilitySynergy [usedCard.abilityType, usedCard.AreaSize (), abilityType, abilityArea] += 0.001f;
                            }
                            if (tokenType < usedCard.tokenType) {
                                token_TokenSynergy [tokenType, tokenValue, usedCard.tokenType, usedCard.tokenValue] += 0.001f;
                            } else {
                                token_TokenSynergy [usedCard.tokenType, usedCard.tokenValue, tokenType, tokenValue] += 0.001f;
                            }
                            ability_TokenSynergy [abilityType, abilityArea, usedCard.tokenType, usedCard.tokenValue] += 0.001f;
                            ability_TokenSynergy [usedCard.abilityType, usedCard.AreaSize (), tokenType, tokenValue] += 0.001f;
                        }
                    }
                    if (z > 0) {
                        CardClass prevCard = stack.card [z - 1];
                        abilityAfterAbility [abilityType, abilityArea, prevCard.abilityType, prevCard.AreaSize ()] *= 0.999f;
                        abilityAfterToken [abilityType, abilityArea, prevCard.tokenType, prevCard.tokenValue] *= 0.999f;
                        tokenAfterAbility [tokenType, tokenValue, prevCard.abilityType, prevCard.AreaSize ()] *= 0.999f;
                        tokenAfterToken [tokenType, tokenValue, prevCard.tokenType, prevCard.tokenValue] *= 0.999f;
                        if (winnerNumber == x) {
                            abilityAfterAbility [abilityType, abilityArea, prevCard.abilityType, prevCard.AreaSize ()] += 0.001f;
                            abilityAfterToken [abilityType, abilityArea, prevCard.tokenType, prevCard.tokenValue] += 0.001f;
                            tokenAfterAbility [tokenType, tokenValue, prevCard.abilityType, prevCard.AreaSize ()] += 0.001f;
                            tokenAfterToken [tokenType, tokenValue, prevCard.tokenType, prevCard.tokenValue] += 0.001f;
                        }
                    }
                    usedCards [x].Add (card);
                    abilityOnStack [abilityType, abilityArea, y] *= 0.999f;

                    abilityOnRow [abilityType, abilityArea, z] *= 0.999f;
                    tokenOnRow [tokenType, tokenValue, z] *= 0.999f;
                    abilityTokenOnRow [abilityType, tokenType, z] *= 0.999f;

                    abilityStackSize [abilityType, abilityArea, cardCount] *= 0.999f;
                    tokenStackSize [tokenType, tokenValue, cardCount] *= 0.999f;
                    abilityTokenStackSize [abilityType, tokenType, cardCount] *= 0.999f;

                    cardNumberWinRatio [card.cardNumber] *= 0.999f;
                    cardPopularity [card.cardNumber] += 0.005f;

                    if (winnerNumber == x) {
                        abilityOnStack [abilityType, abilityArea, y] += 0.001f;

                        abilityOnRow [abilityType, abilityArea, z] += 0.001f;
                        tokenOnRow [tokenType, tokenValue, z] += 0.001f;
                        abilityTokenOnRow [abilityType, tokenType, z] += 0.001f;

                        abilityStackSize [abilityType, abilityArea, cardCount] += 0.001f;
                        tokenStackSize [tokenType, tokenValue, cardCount] += 0.001f;
                        abilityTokenStackSize [abilityType, tokenType, cardCount] += 0.001f;

                        cardNumberWinRatio [card.cardNumber] += 0.001f;
                    }
                }
            }

            //usedCards [x] = new List<CardClass> ();

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
            AIabilityTokenStackSize [AI.tokenStackSize] *= 0.999f;

            AIabilityAfterAbility [AI.abilityAfterAbility] *= 0.999f;
            AIabilityAfterToken [AI.abilityAfterToken] *= 0.999f;
            AItokenAfterAbility [AI.tokenAfterAbility] *= 0.999f;
            AItokenAfterToken [AI.tokenAfterToken] *= 0.999f;

            AIability_AbilitySynergy [AI.ability_AbilitySynergy] *= 0.999f;
            AIability_TokenSynergy [AI.ability_TokenSynergy] *= 0.999f;
            AItoken_TokenSynergy [AI.ability_TokenSynergy] *= 0.999f;

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
                AIabilityTokenStackSize [AI.tokenStackSize] += 0.001f;

                AIabilityAfterAbility [AI.abilityAfterAbility] += 0.001f;
                AIabilityAfterToken [AI.abilityAfterToken] += 0.001f;
                AItokenAfterAbility [AI.tokenAfterAbility] += 0.001f;
                AItokenAfterToken [AI.tokenAfterToken] += 0.001f;

                AIability_AbilitySynergy [AI.ability_AbilitySynergy] += 0.001f;
                AIability_TokenSynergy [AI.ability_TokenSynergy] += 0.001f;
                AItoken_TokenSynergy [AI.ability_TokenSynergy] += 0.001f;
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
                    int tokenValue1 = loserCard.tokenValue;
                    foreach (CardClass winnerCard in usedCards [winnerNumber]) {
                        int abilityType2 = winnerCard.abilityType;
                        int abilityArea2 = winnerCard.AreaSize ();
                        int tokenType2 = winnerCard.tokenType;
                        int tokenValue2 = winnerCard.tokenValue;
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
        SaveTokenOnRow ();
        SaveAbilityTokenOnRow ();

        SaveAbilityStackSize ();
        SaveTokenStackSize ();
        SaveAbilityTokenStackSize ();

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

        SaveAbility_AbilitySynergy ();
        SaveAbility_TokenSynergy ();
        SaveToken_TokenSynergy ();

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
        RatingData.SaveRatingAbilityOnStack (lines.ToArray ());
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
        RatingData.SaveRatingAbilityOnRow (lines.ToArray ());
    }

    static public void LoadAbilityOnRow () {
        string [] lines = RatingData.GetRatingAbilityOnRow ();
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
        RatingData.SaveRatingTokenOnRow (lines.ToArray ());
    }

    static public void LoadTokenOnRow () {
        string [] lines = RatingData.GetRatingTokenOnRow ();
        Load (lines, tokenOnRow);
    }

    static public void SaveAbilityTokenOnRow () {
        RatingData.SaveRatingAbilityTokenOnRow (Save (abilityTokenOnRow));
    }
    
    static public void LoadAbilityTokenOnRow () {
        string [] lines = RatingData.GetRatingAbilityTokenOnRow ();
        Load (lines, abilityTokenOnRow);
    }


    static public void SaveWinnerScore () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < winnerScore.Length; x++) {
            lines.Add ("[" + x.ToString () + "] " + winnerScore [x].ToString ());
        }
        RatingData.SaveRatingWinnerScore (lines.ToArray ());
    }

    static public void SaveLoserScore () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < loserScore.Length; x++) {
            lines.Add ("[" + x.ToString () + "] " + loserScore [x].ToString ());
        }
        RatingData.SaveRatingLoserScore (lines.ToArray ());
    }

    static public void SavePlayerWinRatio () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < winner.Length; x++) {
            lines.Add ("[" + x.ToString () + "] " + winner [x].ToString ());
        }
        RatingData.SaveRatingPlayerWinRatio (lines.ToArray ());
    }

    static public void SaveCardNumberWinRatio () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < cardNumberWinRatio.Length; x++) {
            lines.Add ("[" + x.ToString () + "] " + cardNumberWinRatio [x].ToString ());
        }
        RatingData.SaveRatingCardNumberWinRatio (lines.ToArray ());
    }

    static public void SaveCardPopularity () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < cardPopularity.Length; x++) {
            lines.Add ("[" + x.ToString () + "] " + cardPopularity [x].ToString ());
        }
        RatingData.SaveRatingCardPopularity (lines.ToArray ());
    }

    static public void LoadCardPopularity () {
        string [] lines = RatingData.GetRatingPopularity ();
        Load (lines, cardPopularity);
    }

    static public void SaveTurn () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < turn.Length; x++) {
            lines.Add ("[" + x.ToString () + "] " + turn [x].ToString ());
        }
        RatingData.SaveRatingTurn (lines.ToArray ());
    }

    static public void SaveEdgeDanger () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < turn.Length; x++) {
            lines.Add ("[" + x.ToString () + "] " + edgeDanger [x].ToString ());
        }
        RatingData.SaveRatingEdgeDanger (lines.ToArray ());
    }

    static public void SaveMultiTargetDanger () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < turn.Length; x++) {
            lines.Add ("[" + x.ToString () + "] " + multiTargetDanger [x].ToString ());
        }
        RatingData.SaveRatingMultiTargetDanger (lines.ToArray ());
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
        s = "AIabilityTokenStackSize: ";
        for (int x = 0; x < AICount; x++) {
            s += " [" + x.ToString () + "] " + AIabilityTokenStackSize [x].ToString ();
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

        s = "AIability_AbilitySynergy: ";
        for (int x = 0; x < AICount; x++) {
            s += " [" + x.ToString () + "] " + AIability_AbilitySynergy [x].ToString ();
        }
        lines.Add (s);
        s = "AIability_TokenSynergy: ";
        for (int x = 0; x < AICount; x++) {
            s += " [" + x.ToString () + "] " + AIability_TokenSynergy [x].ToString ();
        }
        lines.Add (s);
        s = "AItoken_TokenSynergy: ";
        for (int x = 0; x < AICount; x++) {
            s += " [" + x.ToString () + "] " + AItoken_TokenSynergy [x].ToString ();
        }
        lines.Add (s);
        RatingData.SaveRatingAISettings (lines.ToArray ());
    }

    static public void SaveSurroundDanger () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < turn.Length; x++) {
            lines.Add ("[" + x.ToString () + "] " + surroundDanger [x].ToString ());
        }
        RatingData.SaveRatingSurroundDanger (lines.ToArray ());
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
        RatingData.SaveRatingMapPlayer (lines.ToArray ());
    }

    static public void SaveNumberOfCards () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < numberOfCards.Length; x++) {
            lines.Add ("[" + x.ToString () + "] " + numberOfCards [x].ToString ());
        }
        RatingData.SaveRatingNumberOfCards (lines.ToArray ());
    }

    static public void SaveAbility_AbilitySynergy () {
        RatingData.SaveRatingAbility_AbilitySynergy (Save (ability_AbilitySynergy));
    }

    static public void LoadAbility_AbilitySynergy () {
        string [] lines = RatingData.GetRatingAbility_AbilitySynergy ();
        Load (lines, ability_AbilitySynergy);
    }


    static public void LoadAbility_TokenSynergy () {
        string [] lines = RatingData.GetRatingAbility_TokenSynergy ();
        Load (lines, ability_TokenSynergy);
    }


    static public void SaveAbility_TokenSynergy () {
        RatingData.SaveRatingAbility_TokenSynergy (Save (ability_TokenSynergy));
    }

    static public void LoadToken_TokenSynergy () {
        string [] lines = RatingData.GetRatingToken_TokenSynergy ();
        Load (lines, token_TokenSynergy);
    }


    static public void SaveToken_TokenSynergy () {
        RatingData.SaveRatingToken_TokenSynergy (Save (token_TokenSynergy));
    }

    static public void SaveAbilityAfterAbility () {
        RatingData.SaveRatingAbilityAfterAbility (Save (abilityAfterAbility));
    }

    static public void LoadAbilityAfterAbility () {
        string [] lines = RatingData.GetRatingAbilityAfterAbility ();
        Load (lines, abilityAfterAbility);
    }

    static public void SaveAbilityAfterToken () {
        RatingData.SaveRatingAbilityAfterToken (Save (abilityAfterToken));
    }


    static public void LoadAbilityAfterToken () {
        string [] lines = RatingData.GetRatingAbilityAfterToken ();
        Load (lines, abilityAfterToken);
    }

    static public void SaveTokenAfterAbility () {
        RatingData.SaveRatingTokenAfterAbility (Save (tokenAfterAbility));
    }


    static public void LoadTokenAfterAbility () {
        string [] lines = RatingData.GetRatingTokenAfterAbility ();
        Load (lines, tokenAfterAbility);
    }

    static public void SaveTokenAfterToken () {
        RatingData.SaveRatingTokenAfterToken (Save (tokenAfterToken));
    }


    static public void LoadTokenAfterToken () {
        string [] lines = RatingData.GetRatingTokenAfterToken ();
        Load (lines, tokenAfterToken);
    }
    
    static public void LoadAbilityStackSize () {
        string [] lines = RatingData.GetRatingAbilityStackSize ();
        Load (lines, abilityStackSize);
    }

    static public void SaveAbilityStackSize () {
        RatingData.SaveRatingAbilityStackSize (Save (abilityStackSize));
    }

    static public void LoadTokenStackSize () {
        string [] lines = RatingData.GetRatingTokenStackSize ();
        Load (lines, tokenStackSize);
    }

    static public void SaveTokenStackSize () {
        RatingData.SaveRatingTokenStackSize (Save (tokenStackSize));
    }

    static public void LoadAbilityTokenStackSize () {
        string [] lines = RatingData.GetRatingAbilityTokenStackSize ();
        Load (lines, abilityTokenStackSize);
    }

    static public void SaveAbilityTokenStackSize () {
        RatingData.SaveRatingTokenStackSize (Save (abilityTokenStackSize));
    }

    static public void SaveAbilityAgainstAbility () {
        RatingData.SaveRatingAbilityAgainstAbility (Save (abilityAgainstAbility));
    }

    static public void LoadAbilityAgainstAbility () {
        string [] lines = RatingData.GetRatingAbilityAgainstAbility ();
        Load (lines, abilityAgainstAbility);
    }

    static public void SaveAbilityAgainstToken () {
        RatingData.SaveRatingAbilityAgainstToken (Save (abilityAgainstToken));
    }

    static public void LoadAbilityAgainstToken () {
        string [] lines = RatingData.GetRatingAbilityAgainstToken ();
        Load (lines, abilityAgainstToken);
    }

    static public void SaveTokenAgainstAbility () {
        RatingData.SaveRatingTokenAgainstAbility (Save (tokenAgainstAbility));
    }

    static public void LoadTokenAgainstAbility () {
        string [] lines = RatingData.GetRatingTokenAgainstAbility ();
        Load (lines, tokenAgainstAbility);
    }

    static public void SaveTokenAgainstToken () {
        RatingData.SaveRatingTokenAgainstToken (Save (tokenAgainstToken));
    }

    static public void LoadTokenAgainstToken () {
        string [] lines = RatingData.GetRatingTokenAgainstToken ();
        Load (lines, tokenAgainstToken);
    }

    static float minValue = 0.4f;

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
                array [x] = Mathf.Max (array [x], minValue);
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
                    array [x, y, y2] = Mathf.Max (array [x, y, y2], minValue);
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
                        array [x, x2, y, y2] = Mathf.Max (array [x, x2, y, y2], minValue);
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
