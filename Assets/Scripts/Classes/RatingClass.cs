using System;
using System.Collections.Generic;
using UnityEngine;

public class RatingClass {

    static bool CreateBackUp = true;

    static int [] winner = new int [5];
    static int [,] turn = new int [42,10];

    static float [,] mapPlayer = new float [600, 5];

    static int [,] mapTurn = new int [600, 50];

    const int AICount = 100;
    const int AIStats = 180;

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

    static public float [] cardPopularity = new float [AIStats];
    static public float [] cardNumberWinRatio = new float [AIStats];
    static public float [] buggedCard = new float [AIStats];
    static public float [] buggedAbility = new float [AIStats];
    static public float [] buggedToken = new float [AIStats];

    static public float [,,] abilityOnStack; // AbilityType, AbilityArea (0, 2, 6 fields), stackNumber;
    static public float [,,] tokenOnStack; // AbilityType, AbilityArea (0, 2, 6 fields), stackNumber;
    static public float [,,] abilityTokenOnStack; // AbilityType, AbilityArea (0, 2, 6 fields), stackNumber;


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
        int availableAbilities = AppDefaults.availableAbilities;
        int availableTokens = AppDefaults.availableTokens;
        abilityOnStack = new float [availableAbilities, 3, 10];
        tokenOnStack = new float [availableTokens, 9, 10];
        abilityTokenOnStack = new float [availableAbilities, availableTokens, 10];

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
            int abilityType1 = (int) card [x].abilityType;
            int abilityArea1 = card [x].AreaSize ();
            int tokenType1 = (int) card [x].tokenType;
            int tokenValue1 = card [x].tokenValue;


            for (int y = 0; y < count; y++) {
                int abilityType2 = (int) card [y].abilityType;
                int abilityArea2 = card [y].AreaSize ();
                int tokenType2 = (int) card [y].tokenType;
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
        if (match.winner == null || match.winner.Count == 0) {
            return;
        }
        int winnerNumber = match.winner[0].properties.playerNumber;
        AnalyzeStatisticsPart1 (match, winnerNumber);
    }

    static public void AnalyzeStatisticsPart1 (MatchClass match, int winnerNumber) {
        winner [winnerNumber]++;
        winnerScore [Mathf.Max (0, match.winner [0].score)]++;
        for (int x = 0; x < cardPopularity.Length; x++) {
            cardPopularity [x] *= 0.9999f;
        }
        List<CardClass> [] usedCards = new List<CardClass> [match.numberOfPlayers + 1];

        AnalyzeStatisticsPart2 (match, winnerNumber, usedCards);
        AnalyzeStatisticsPart3 (match, winnerNumber, usedCards);
    }



    static public void AnalyzeStatisticsPart2 (MatchClass match, int winnerNumber, List<CardClass> [] usedCards) {
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
                    int abilityType = (int) card.abilityType;
                    int tokenType = (int) card.tokenType;
                    int tokenValue = card.tokenValue;
                    foreach (CardClass usedCard in usedCards [x]) {
                        int usedAbilityArea = usedCard.AreaSize ();
                        int usedAbilityType = (int) usedCard.abilityType;
                        int usedTokenType = (int) usedCard.tokenType;
                        int usedTokenValue = usedCard.tokenValue;
                        if (abilityType < usedAbilityType) {
                            ability_AbilitySynergy [abilityType, abilityArea, usedAbilityType, usedAbilityArea] *= 0.999f;
                        } else {
                            ability_AbilitySynergy [usedAbilityType, usedAbilityArea, abilityType, abilityArea] *= 0.999f;
                        }
                        if (tokenType < usedTokenType) {
                            token_TokenSynergy [tokenType, tokenValue, usedTokenType, usedTokenValue] *= 0.999f;
                        } else {
                            token_TokenSynergy [usedTokenType, usedTokenValue, tokenType, tokenValue] *= 0.999f;
                        }
                        ability_TokenSynergy [abilityType, abilityArea, usedTokenType, usedTokenValue] *= 0.999f;
                        ability_TokenSynergy [usedAbilityType, usedAbilityArea, tokenType, tokenValue] *= 0.999f;
                        if (winnerNumber == x) {
                            if (abilityType < usedAbilityType) {
                                ability_AbilitySynergy [abilityType, abilityArea, usedAbilityType, usedAbilityArea] += 0.001f;
                            } else {
                                ability_AbilitySynergy [usedAbilityType, usedAbilityArea, abilityType, abilityArea] += 0.001f;
                            }
                            if (tokenType < usedTokenType) {
                                token_TokenSynergy [tokenType, tokenValue, usedTokenType, usedTokenValue] += 0.001f;
                            } else {
                                token_TokenSynergy [usedTokenType, usedTokenValue, tokenType, tokenValue] += 0.001f;
                            }
                            ability_TokenSynergy [abilityType, abilityArea, usedTokenType, usedTokenValue] += 0.001f;
                            ability_TokenSynergy [usedAbilityType, usedAbilityArea, tokenType, tokenValue] += 0.001f;
                        }
                    }
                    if (z > 0) {
                        CardClass prevCard = stack.card [z - 1];
                        int prevAbilityArea = prevCard.AreaSize ();
                        int prevAbilityType = (int) prevCard.abilityType;
                        int prevTokenType = (int) prevCard.tokenType;
                        int prevTokenValue = prevCard.tokenValue;
                        abilityAfterAbility [abilityType, abilityArea, prevAbilityType, prevAbilityArea] *= 0.999f;
                        abilityAfterToken [abilityType, abilityArea, prevTokenType, prevTokenValue] *= 0.999f;
                        tokenAfterAbility [tokenType, tokenValue, prevAbilityType, prevAbilityArea] *= 0.999f;
                        tokenAfterToken [tokenType, tokenValue, prevTokenType, prevTokenValue] *= 0.999f;
                        if (winnerNumber == x) {
                            abilityAfterAbility [abilityType, abilityArea, prevAbilityType, prevAbilityArea] += 0.001f;
                            abilityAfterToken [abilityType, abilityArea, prevTokenType, prevTokenValue] += 0.001f;
                            tokenAfterAbility [tokenType, tokenValue, prevAbilityType, prevAbilityArea] += 0.001f;
                            tokenAfterToken [tokenType, tokenValue, prevTokenType, prevTokenValue] += 0.001f;
                        }
                    }
                    usedCards [x].Add (card);
                    abilityOnStack [abilityType, abilityArea, y] *= 0.999f;
                    tokenOnStack [tokenType, tokenValue, z] *= 0.999f;
                    abilityTokenOnStack [abilityType, tokenType, z] *= 0.999f;

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
                        tokenOnStack [tokenType, tokenValue, z] += 0.001f;
                        abilityTokenOnStack [abilityType, tokenType, z] += 0.001f;

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
    }

    static public void AnalyzeStatisticsPart3 (MatchClass match, int winnerNumber, List<CardClass> [] usedCards) {
        turn [match.turn, match.winCondition]++;
        mapTurn [match.Board.boardTemplateId, match.turn]++;
        int loserNumber = 0;
        for (int x = 1; x <= match.numberOfPlayers; x++) {
            for (int y = 1; y <= match.numberOfPlayers; y++) {
                if (x == y || y == winnerNumber) {
                    continue;
                }
                loserNumber = y;
                foreach (CardClass loserCard in usedCards [loserNumber]) {
                    int abilityType1 = (int) loserCard.abilityType;
                    int abilityArea1 = loserCard.AreaSize ();
                    int tokenType1 = (int) loserCard.tokenType;
                    int tokenValue1 = loserCard.tokenValue;
                    foreach (CardClass winnerCard in usedCards [winnerNumber]) {
                        int abilityType2 = (int) winnerCard.abilityType;
                        int abilityArea2 = winnerCard.AreaSize ();
                        int tokenType2 = (int) winnerCard.tokenType;
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

        SaveAbilityOnStack ();
        SaveTokenOnStack ();
        SaveAbilityTokenOnStack ();

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
        SaveMapPlayer ();
        SaveMapTurn ();
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

        SaveBuggedCard ();
        SaveBuggedAbility ();
        SaveBuggedToken ();
    }

    static public void LoadAbilityOnStack () {
        string [] lines = RatingData.GetRatingAbilityOnStack ();
        Load (lines, abilityOnStack);
    }

    static public void SaveAbilityOnStack () {
        RatingData.SaveRatingAbilityOnStack (Save (abilityOnStack));
    }

    static public void LoadTokenOnStack () {
        string [] lines = RatingData.GetRatingTokenOnStack ();
        Load (lines, tokenOnStack);
    }

    static public void SaveTokenOnStack () {
        RatingData.SaveRatingTokenOnStack (Save (tokenOnStack));
    }

    static public void LoadAbilityTokenOnStack () {
        string [] lines = RatingData.GetRatingAbilityTokenOnStack ();
        Load (lines, abilityTokenOnStack);
    }

    static public void SaveAbilityTokenOnStack () {
        RatingData.SaveRatingAbilityTokenOnStack (Save (abilityTokenOnStack));
    }

    static public void SaveAbilityOnRow () {
        RatingData.SaveRatingAbilityOnRow (Save (abilityOnRow));
    }

    static public void LoadAbilityOnRow () {
        string [] lines = RatingData.GetRatingAbilityOnRow ();
        Load (lines, abilityOnRow);
    }

    static public void SaveTokenOnRow () {
        RatingData.SaveRatingTokenOnRow (Save (tokenOnRow));
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
        RatingData.SaveRatingWinnerScore (Save (winnerScore));
    }

    static public void SaveLoserScore () {
        RatingData.SaveRatingLoserScore (Save (loserScore));
    }

    static public void SavePlayerWinRatio () {
        RatingData.SaveRatingPlayerWinRatio (Save (winner));
    }

    static public void SaveCardNumberWinRatio () {
        RatingData.SaveRatingCardNumberWinRatio (Save (cardNumberWinRatio));
    }

    static public void SaveCardPopularity () {
        RatingData.SaveRatingCardPopularity (Save (cardPopularity));
    }

    static public void LoadCardPopularity () {
        string [] lines = RatingData.GetRatingPopularity ();
        Load (lines, cardPopularity);
    }

    static public void SaveTurn () {
        List<string> lines = new List<string> ();
        for (int x = 0; x < turn.GetLength (0); x++) {
            string s = "[" + x.ToString () + "] ";
            for (int y = 0; y < turn.GetLength (1); y++) {
                s += "[" + y.ToString () + "] " + turn [x, y].ToString () + " ";
            }
            lines.Add (s);
        }
        RatingData.SaveRatingTurn (lines.ToArray ());
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

    static public void SaveMapPlayer () {
        RatingData.SaveRatingMapPlayer (Save (mapPlayer));
    }

    static public void SaveMapTurn () {
        RatingData.SaveRatingMapTurn (Save (mapTurn));
    }

    static public void SaveNumberOfCards () {
        RatingData.SaveRatingNumberOfCards (Save (numberOfCards));
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
        RatingData.SaveRatingAbilityTokenStackSize (Save (abilityTokenStackSize));
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

    static public void SaveBuggedCard () {
        RatingData.SaveRatingBuggedCard (Save (buggedCard));
    }

    static public void SaveBuggedAbility () {
        RatingData.SaveRatingBuggedAbility (Save (buggedAbility));
    }

    static public void SaveBuggedToken () {
        RatingData.SaveRatingBuggedToken (Save (buggedToken));
    }

    static float minValue = 0.46f;

    static public void Load (string [] lines, float [] array) {
        for (int x = 0; x < array.GetLength (0); x++) {
            string [] word = null;
            float value = 0.5f;
            if (lines != null && x < lines.Length) {
                word = lines [x].Split (new char [] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (x < lines.Length && word [1] != null) {
                    //value = float.Parse (word [1], CultureInfo.InvariantCulture);
                    float.TryParse (word [1], out value);
                }
            }
            array [x] = value;
            if (array != cardPopularity) {
                array [x] = Mathf.Max (array [x], minValue);
            }
        }
    }

    static public void Load (string [] lines, float [,,] array) {
        int count1 = lines.Length;
        int aCount1 = array.GetLength (0);

        for (int x1 = 0; x1 < aCount1; x1++) {
            string [] s2 = null;
            int count2 = 0;
            if (x1 < count1) {
                s2 = lines [x1].Split (new char [1] { ';' });
                count2 = s2.Length - 1;
            }
            int aCount2 = array.GetLength (1);

            for (int x2 = 0; x2 < aCount2; x2++) {
                string [] s3 = null;
                int count3 = 0;
                if (s2 != null && x2 < count2) {
                    s3 = s2 [x2].Split (new char [1] { ' ' });
                    count3 = s3.Length - 1;
                }
                int aCount3 = array.GetLength (2);
                
                for (int x3 = 0; x3 < aCount3; x3++) {
                    float value = 0.5f;
                    if (s3 != null && x3 < count3 && s3 [x3] != null && s3 [x3] != "") {
                        //value = float.Parse (s3 [x3], CultureInfo.InvariantCulture);
                        float.TryParse (s3 [x3], out value);
                    }
                    array [x1, x2, x3] = Mathf.Max (value, minValue);
                }
            }
        }
    }

    static public string [] Save (float [] array) {
        List<string> lines = new List<string> ();
        for (int x = 0; x < array.GetLength (0); x++) {
            string s = "[" + x.ToString () + "] ";
            s += array [x].ToString () + " ";
            lines.Add (s);
        }
        return lines.ToArray ();
    }

    static public string [] Save (int [] array) {
        List<string> lines = new List<string> ();
        for (int x = 0; x < array.GetLength (0); x++) {
            string s = "[" + x.ToString () + "] ";
            s += array [x].ToString () + " ";
            lines.Add (s);
        }
        return lines.ToArray ();
    }

    static public string [] Save (float [,] array) {
        List<string> lines = new List<string> ();
        for (int x1 = 0; x1 < array.GetLength (0); x1++) {
            string s = "";
            for (int x2 = 0; x2 < array.GetLength (1); x2++) {
                s += array [x1, x2] + " ";
            }
            s += ";";
        }
        return lines.ToArray ();
    }

    static public string [] Save (int [,] array) {
        List<string> lines = new List<string> ();
        for (int x1 = 0; x1 < array.GetLength (0); x1++) {
            string s = "";
            for (int x2 = 0; x2 < array.GetLength (1); x2++) {
                s += array [x1, x2] + " ";
            }
            s += ";";
        }
        return lines.ToArray ();
    }

    static public string [] Save (float [,,] array) {
        List<string> lines = new List<string> ();
        for (int x1 = 0; x1 < array.GetLength (0); x1++) {
            string s = "";
            for (int x2 = 0; x2 < array.GetLength (1); x2++) {
                for (int x3 = 0; x3 < array.GetLength (2); x3++) {
                    s += array [x1, x2, x3] + " ";
                }
                s += ";";
            }
            lines.Add (s);
        }
        return lines.ToArray ();
    }

    static public string [] Save (int [,,] array) {
        List<string> lines = new List<string> ();
        for (int x1 = 0; x1 < array.GetLength (0); x1++) {
            string s = "";
            for (int x2 = 0; x2 < array.GetLength (1); x2++) {
                for (int x3 = 0; x3 < array.GetLength (2); x3++) {
                    s += array [x1, x2, x3] + " ";
                }
                s += ";";
            }
            lines.Add (s);
        }
        return lines.ToArray ();
    }


    static public void Load (string [] lines, float [,,,] array) {
        int count1 = lines.Length;
        int aCount1 = array.GetLength (0);

        for (int x1 = 0; x1 < aCount1; x1++) {
            string [] s2 = null;
            int count2 = 0;
            if (x1 < count1) {
                s2 = lines [x1].Split (new char [1] { ':' });
                count2 = s2.Length - 1;
            }
            int aCount2 = array.GetLength (1);

            for (int x2 = 0; x2 < aCount2; x2++) {
                string [] s3 = null;
                int count3 = 0;
                if (s2 != null && x2 < count2) {
                    s3 = s2 [x2].Split (new char [1] { ';' });
                    count3 = s3.Length - 1;
                }
                int aCount3 = array.GetLength (2);

                for (int x3 = 0; x3 < aCount3; x3++) {
                    string [] s4 = null;
                    int count4 = 0;
                    if (s3 != null && x3 < count3) {
                        s4 = s3 [x3].Split (new char [1] { ' ' });
                        count4 = s4.Length - 1;
                    }
                    int aCount4 = array.GetLength (3);

                    for (int x4 = 0; x4 < aCount4; x4++) {
                        float value = 0.5f;
                        if (s4 != null && x4 < count4 && s4 [x4] != null && s4 [x4] != "") {
                            //value = float.Parse (s4 [x4], CultureInfo.InvariantCulture);
                            float.TryParse (s4 [x4], out value);
                        }
                        array [x1, x2, x3, x4] = Mathf.Max (value, minValue);
                    }
                }
            }
        }
    }

    static public string [] Save (float [,,,] array) {
        List<string> lines = new List<string> ();
        for (int x1 = 0; x1 < array.GetLength (0); x1++) {
            string s = "";
            for (int x2 = 0; x2 < array.GetLength (1); x2++) {
                for (int x3 = 0; x3 < array.GetLength (2); x3++) {
                    for (int x4 = 0; x4 < array.GetLength (3); x4++) {
                        s += array [x1, x2, x3, x4] + " ";
                    }
                    s += ";";
                }
                s += ":";
            }
            lines.Add (s);
        }
        return lines.ToArray ();
    }


}
