using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIClass {

    static public float maxCardValue = 0;

    public float turnsLeft;

    int difficulty = 100;

    public bool puzzle = false;
    public bool boss = false;
    public bool tutorial = false;

    public int edgeDanger = 8;
    public int surroundDanger = 5;
    public int multiTargetDanger = 5;

    public int abilityRow = 9;
    public int tokenRow = 9;
    public int abilityTokenRow = 18;

    public int abilityStackSize = 5;
    public int tokenStackSize = 5;
    public int abilityTokenStackSize = 5;

    public int abilityAfterAbility = 8;
    public int abilityAfterToken = 8;
    public int tokenAfterAbility = 8;
    public int tokenAfterToken = 8;

    public int ability_AbilitySynergy = 8;
    public int ability_TokenSynergy = 8;
    public int token_TokenSynergy = 8;

    public int abilityAgainstAbility = 8;
    public int abilityAgainstToken = 8;
    public int tokenAgainstAbility = 8;
    public int tokenAgainstToken = 8;

    public float MaxEmptyTileCount;

    public AIClass () {
        edgeDanger = new MyRandom ().Range (0, 10);
        surroundDanger = new MyRandom ().Range (1, 6);
        multiTargetDanger = new MyRandom ().Range (0, 7);

        abilityRow = new MyRandom ().Range (11, 21);
        tokenRow = new MyRandom ().Range (16, 25);
        abilityTokenRow = new MyRandom ().Range (16, 35);

        abilityStackSize = new MyRandom ().Range (2, 9);
        tokenStackSize = new MyRandom ().Range (1, 8);
        abilityTokenStackSize = new MyRandom ().Range (3, 10);

        abilityAfterAbility = new MyRandom ().Range (3, 11);
        abilityAfterToken = new MyRandom ().Range (1, 17);
        tokenAfterAbility = new MyRandom ().Range (2, 17);
        tokenAfterToken = new MyRandom ().Range (2, 16);

        ability_AbilitySynergy = new MyRandom ().Range (5, 14);
        ability_TokenSynergy = new MyRandom ().Range (11, 25);
        token_TokenSynergy = new MyRandom ().Range (11, 15);

        abilityAgainstAbility = new MyRandom ().Range (4, 7);
        abilityAgainstToken = new MyRandom ().Range (4, 7);
        tokenAgainstAbility = new MyRandom ().Range (4, 7);
        tokenAgainstToken = new MyRandom ().Range (4, 8);
    }

    public Vector3Int FindBestMove (MatchClass match) {
        int playerNumber = match.turnOfPlayer;
        PlayerClass player = match.Player [playerNumber];
        TileClass [] tiles = match.Board.GetPlayableTiles (playerNumber).ToArray ();
        TileClass bestTile = null;
        float bestValue = -999999f;
        int bestStack = 0;
        int numberOfBests = 0;
        if (tiles.Length == 0){
            Debug.Log ("Board error, turn: " + match.turn.ToString() + ", board: " + match.Board.boardTemplateId.ToString() + ", all tiles count: " + match.Board.tileList.Count);
            Debug.Log (match.finished);
            for (int x = 1; x <= 2; x++) {
                PlayerClass tempPlayer = match.Player [x];
                foreach (StackClass stack in tempPlayer.hand.stack) {
                    foreach (CardClass card in stack.card) {
                        RatingClass.buggedCard [card.cardNumber]++;
                        RatingClass.buggedAbility [(int) card.abilityType]++;
                        RatingClass.buggedToken [(int) card.tokenType]++;
                    }
                }
            }
        }
        int count = tiles.Length;
        TileClass [] RandomizedTiles = new TileClass [count];
        List <int> tempRNGNumbers = new List<int > ();
        for (int x = 0; x < count; x++) {
            tempRNGNumbers.Add (x);
        }

        for (int x = 0; x < count; x++) {
            int randomNumber = new MyRandom ().Range (0, count - x);
            RandomizedTiles [x] = tiles [ tempRNGNumbers [randomNumber]];
            tempRNGNumbers.RemoveAt (randomNumber);
        }

        int brk = 0;
        foreach (TileClass tile in RandomizedTiles) {
            if (tile == null || !tile.enabled || !tile.IsPlayable (playerNumber)) {
                Debug.Log ("Tile error");
            }
            brk++;
            if (brk > 40) {
                break;
            }
            for (int x = 0; x < player.hand.stack.Length; x++) {
                //Debug.Log ("Currently testing x: " + tile.x + ", y: " + tile.y + ", stack: " + x + ", top card: " + player.hand.GetStack (x).topCardNumber);
                //Debug.Log ("Start");
                MatchClass tempMatch = new MatchClass (match);
               // Debug.Log ("Currently testing x: " + tile.x + ", y: " + tile.y + ", stack: " + x + ", top card: " + tempMatch.Player [playerNumber].hand.GetStack (x).topCardNumber);
                tempMatch.real = false;
                tempMatch.PlayCard (tile.x, tile.y, playerNumber, x);
                float tempValue = CalculateMatchValue (tempMatch, playerNumber, x);
                if (tempMatch.finished) {
                    if (tempMatch.winner.Count > 0) {
                        if (tempMatch.winner [0].properties.playerNumber == playerNumber) {
                            tempValue += 100000;
                        } else {
                            tempValue -= 100000;
                        }
                    }
                }
                if (!tempMatch.Player [playerNumber].enabled) {
                    tempValue -= 10000;
                }
                //Debug.Log ("TempValue: " + tempValue);
                if (bestTile == null || bestValue < tempValue) {
                    bestTile = tile;
                    bestStack = x;
                    bestValue = tempValue;
                    numberOfBests = 1;
                } else if (bestValue == tempValue) {
                    numberOfBests++;
                    if (new MyRandom ().Range (0, numberOfBests) == 0) {
                        bestTile = tile;
                        bestStack = x;
                        bestValue = tempValue;
                    }
                }
            }
        }
        //Debug.Log ("BestValue: " + bestValue);
        return new Vector3Int (bestTile.x, bestTile.y, bestStack);
    }

    public float CalculateMatchValue (MatchClass match, int playerNumber, int stack) {
        string s = "";
        float value = 0;
        PlayerClass player = match.Player [playerNumber];
        value += player.AIValue;
        float advantageValue = match.properties.scoreLimit;

        s += "Player value " + player.AIValue + System.Environment.NewLine;

        s += "PlayerScore: ";

        PlayerPropertiesClass playerProperties = match.GetPlayerProperties (playerNumber);
        int teamNumber = playerProperties.team;
        int [] teamScore = new int [5];
        for (int x = 1; x < match.Player.Length; x++) {
            PlayerPropertiesClass playerProperties2 = match.GetPlayerProperties (x);
            if (playerProperties2 != null) {
                PlayerClass player2 = match.GetPlayer (x);
                teamScore [playerProperties2.team] += player2.score;
                s += x + ", " + player2.score + "; ";
            }
        }

        s += System.Environment.NewLine;

        s += "TeamScore: ";

        for (int x = 1; x < match.Player.Length; x++) {
            s += teamScore [x] + " ";
        }
        s += System.Environment.NewLine;

        turnsLeft = match.properties.turnLimit;
        for (int x = 1; x <= match.numberOfPlayers; x++) {
            PlayerPropertiesClass playerProperties2 = match.GetPlayerProperties (x);
            if (playerProperties2 == null) {
                continue;
            }
            float hisTurnToWin = TurnToWinPredict (match, x);
            turnsLeft = Mathf.Min (turnsLeft, hisTurnToWin);
            if (x != playerNumber) {
                s += "PlayerNumber: " + playerProperties2.playerNumber + ", Team score: " + teamScore [teamNumber] + ", Compare to score: " + teamScore [playerProperties2.team] + 
                    ", Difference of score: " + (teamScore [teamNumber] - teamScore [playerProperties2.team]).ToString() +
                    ", Turns left: " + turnsLeft + System.Environment.NewLine;
                advantageValue = Mathf.Min (advantageValue, teamScore [teamNumber] - teamScore [playerProperties2.team]);
                value -= match.GetPlayer (x).AIValue;
            }
        }

        s += System.Environment.NewLine;
        

        advantageValue /= turnsLeft;
        
        value += advantageValue;
        s += "advantageValue " + advantageValue + System.Environment.NewLine;

        float boardValue = CalculateBoardValue (match, playerNumber, turnsLeft);
        value += boardValue;
        s += "CalculateBoardValue " + boardValue + System.Environment.NewLine;

        float turnValue = CalculateTurnValue (match, playerNumber, turnsLeft, stack);
        value += turnValue;
        s += "CalculateBoardValue " + turnValue + System.Environment.NewLine;

        s += "Total value: " + value + System.Environment.NewLine;
        //Debug.Log (s);
        return value;
    }

    public float CalculateTurnValue (MatchClass match, int playerNumber, float turnsLeft, int stack) {
        float value = 0;
        float prefix = 0;
        PlayerPropertiesClass properties = match.GetPlayerProperties (playerNumber);
        if (properties == null) {
            return 0;
        }
        string s = "";
        int myTeam = properties.team;
        int numberOfAllies = 0;
        int numberOfEnemies = 0;
        for (int x = 1; x <= 4; x++) {
            PlayerPropertiesClass properties2 = match.GetPlayerProperties (x);
            if (properties2 == null) {
                continue;
            }
            s += "Player " + x + ", Team" + properties2.team + " ";
            if (properties2.team == myTeam) {
                s += "ally";
                numberOfAllies++;
            } else {
                s += "enemy";
                numberOfEnemies++;
            }
            s += System.Environment.NewLine;
        }
        for (int x = 0; x < turnsLeft; x++) {
            PlayerPropertiesClass properties2 = match.GetPlayerProperties (match.turnOfPlayer);
            if (properties2 != null && properties2.team == myTeam) {
                prefix += 4 / Mathf.Max (1, numberOfAllies);
            } else {
                prefix -= 4 / Mathf.Max (1, numberOfEnemies);
            }
            s += prefix + " (" + match.turnOfPlayer + ") ";
            match.IncrementTurnOfPlayer ();
            value += prefix;
        }
        //Debug.Log (match.turn + ", PlayerNumber: " + playerNumber + System.Environment.NewLine + 
            //"Value: " +  value + ", Turns left" + turnsLeft + ", value / turnsLeft:" + (value / turnsLeft).ToString() + ", Stack: " + stack + " " + match.Player[playerNumber].GetStack (stack).topCardNumber + System.Environment.NewLine + s);
        value /= turnsLeft;
        return value;
    }

    public float CalculateBoardValue (MatchClass match, int playerNumber, float turnsLeft) {
        float value = 0;
        TileClass [] tiles = match.Board.tileList.ToArray ();
        VectorInfo [,] VE = new VectorInfo [10, 10];
        foreach (TileClass tile in tiles) {
            if (tile.IsFilledTile ()) {
                VE [tile.x, tile.y] = match.GetTokenVectorInfo (tile, tile.token);
            }
        }
        foreach (TileClass tile in tiles) {
            float dangerCount = 0;
            float edgeCount = 0;
            float multiDangerCount = 0;
            for (int x = 1; x <= 4; x++) {
                if (tile.IsFilledTile ()) {
                    x = 4;
                }
                AbilityVector [] vectors = match.Board.GetAbilityVectors (tile.x, tile.y, x);
                float enemies = 0;
                float allies = 0;
                foreach (AbilityVector vector in vectors) {
                    if (tile.IsFilledTile ()) {
                        if (vector.target == null || !vector.target.enabled) {
                            if (vector.flipTarget != null && vector.flipTarget.IsPlayable (playerNumber)) {
                                edgeCount++;
                            }
                        } else if (vector.target.IsPlayable (playerNumber)) {
                            dangerCount++;
                        }
                    } else if (vector.target != null && vector.target.IsFilledTile ()) {
                        if (vector.target.token.owner == playerNumber) {
                            allies++;
                        } else{
                            enemies++;
                        }
                    }
                }
                multiDangerCount += Mathf.Max (0, enemies - allies - 0.5f);

            }
            float riskValue = 2;
            if (tile.IsFilledTile ()) {
                VectorInfo oVE = VE [tile.x, tile.y];
                float tokenValue = tile.token.value;
                TokenType tokenType = tile.token.type;
                int tokenOwner = tile.token.owner;
                foreach (AbilityVector vector in oVE.vectors) {
                    TileClass targetTile = vector.target;
                    if (!targetTile.IsFilledTile ()) {
                        continue;
                    }
                    VectorInfo tVE = VE [targetTile.x, targetTile.y];
                    TokenClass targetToken = targetTile.token;
                    TokenType targetType = targetToken.type;
                    int targetValue = targetToken.value;
                    switch (targetType) {
                        case TokenType.T3:
                            if (tVE.weakestTargets.Count == 1 && tVE.weakestTargets [0] == tile) {
                                tokenValue = valueOverTime (tokenValue, 1, 1, turnsLeft);
                            }
                            break;
                        case TokenType.T4:
                            if (tVE.strongestTargets.Count == 1 && tVE.strongestTargets [0] == tile) {
                                tokenValue = valueOverTime (tokenValue, -1, 1, turnsLeft);
                            }
                            break;
                        case TokenType.T5:
                            tokenValue = valueOverTime (tokenValue, -3, targetValue, turnsLeft);
                            break;
                        case TokenType.T8:
                            if (tokenValue < targetValue) {
                                tokenValue = 0;
                            }
                            break;
                        case TokenType.T19:
                            tokenValue++;
                            break;
                    }

                }
                tokenValue = Mathf.Max (tokenValue, 0);
                if (tokenValue > 0) {
                    switch (tokenType) {
                        case TokenType.T1:
                        case TokenType.T12:
                            tokenValue *= 1.9f;
                            break;
                        case TokenType.T2:
                            tokenValue *= -0.9f;
                            break;
                        case TokenType.T5:
                            tokenValue = (tokenValue * (tokenValue + 1) - Mathf.Max (tokenValue - turnsLeft, 0) * (tokenValue - turnsLeft + 1)) / 2 / turnsLeft;
                            break;
                        case TokenType.T6:
                            tokenValue *= 1.06f / match.numberOfPlayers;
                            break;
                        case TokenType.T9:
                            if (match.turnOfPlayer == playerNumber) {
                                tokenValue += 1f;
                            } else {
                                tokenValue += 2.1f;
                            }
                            break;
                        case TokenType.T11:
                            tokenValue += 1;
                            break;
                        case TokenType.T10:
                            tokenValue = valueOverTime (tokenValue + oVE.emptyTileCount, -oVE.emptyTileCount, oVE.emptyTileCount, turnsLeft);
                            break;
                        case TokenType.T13:
                            tokenValue += 4 * (turnsLeft - 1) / turnsLeft;
                            break;
                        case TokenType.T14:
                            tokenValue = valueOverTime (tokenValue, Mathf.Max (4f - tokenValue, tokenValue), 4, turnsLeft);
                            break;
                        case TokenType.T15:
                            if (match.turnOfPlayer == playerNumber) {
                                tokenValue += 0.9f;
                            } else {
                                tokenValue -= 0.8f;
                            }
                            break;
                        case TokenType.T17:
                            tokenValue += 0.1f;
                            break;
                        case TokenType.T16:
                            tokenValue = (tokenValue - 1) * 1.4f + 1;
                            break;
                    }
                }
                switch (tokenType) {
                    case TokenType.T12:
                    case TokenType.T21:
                        riskValue = Mathf.Sqrt (Mathf.Abs (tokenValue)) - 0.2f;
                        break;
                    default:
                        riskValue = tokenValue + 1 - Mathf.Sqrt (Mathf.Abs (tokenValue) + 1);
                        break;
                }

                riskValue *= Mathf.Min (1f, (
                    dangerCount * surroundDanger +
                    edgeCount * edgeDanger) / 100f);
                PlayerPropertiesClass properties = match.GetPlayerProperties (tokenOwner);
                PlayerPropertiesClass properties2 = match.GetPlayerProperties (playerNumber);
                if (properties == null) {
                    tokenValue = 0;
                } else {
                    if (properties.team != properties2.team) {
                        tokenValue *= -1;
                    }
                    tokenValue -= riskValue;
                }
                value += tokenValue;
                //Debug.Log (tokenValue);
            } else {
                
                riskValue *= multiDangerCount * multiTargetDanger / 200f;
                value -= riskValue;
                if (match.Player [playerNumber].properties.specialStatus == 1) {
                    value++;
                }
                //Debug.Log (riskValue);
            }
        }
        //Debug.Log (playerNumber + " " + value);
        return value;
    }

    static public float AproxTokenValue (TokenType tokenType, int value) {
        float output = value;
        if (value > 0) {
            switch (tokenType) {
                case TokenType.T1:
                case TokenType.T12:
                    output *= 1.9f;
                    break;
                case TokenType.T2:
                    output *= -0.9f;
                    break;
                case TokenType.T3:
                case TokenType.T4:
                case TokenType.T9:
                case TokenType.T11:
                    output += 1.05f;
                    break;
                case TokenType.T6:
                    output *= 0.53f;
                    break;
                case TokenType.T8:
                    output = output + (output - 1) / 2;
                    break;
                case TokenType.T10:
                    output += 2.2f;
                    break;
                case TokenType.T13:
                    output += 3.8f;
                    break;
                case TokenType.T14:
                    output = (4 - value) * 0.75f + value * 0.25f;
                    break;
                case TokenType.T15:
                    output += 0.8f;
                    break;
            }
        }
        return output;
    }

    public float valueOverTime (float value, float valueDifference, float turnDelay, float turnsLeft) {
        float perc = Mathf.Max ((turnsLeft - turnDelay), 0) / turnsLeft;
        return value * (1 - perc) + Mathf.Max (value + valueDifference, 0) * perc;
    }

    public float TurnToWinPredict (MatchClass match, int playerNumber) {
        PlayerClass player = match.Player [playerNumber];
        if (player == null) {
            return 10000;
        }
        PlayerPropertiesClass playerProperties = match.GetPlayerProperties (playerNumber);


        int teamNumber = playerProperties.team;
        int teamScore = 0;
        int teamIncome = 0;
        for (int x = 1; x < match.Player.Length; x++) {
            PlayerPropertiesClass playerProperties2 = match.GetPlayerProperties (x);
            if (playerProperties2 != null && playerProperties2.team == teamNumber) {
                PlayerClass player2 = match.GetPlayer (playerNumber);
                teamScore += player2.score;
                teamIncome += player2.scoreIncome;
            }
        }

        MatchPropertiesClass properties = match.properties;
        float value = properties.turnLimit - match.turn + 1;
        BoardClass board = match.Board;
        int playableTiles = board.GetPlayableTilesCount (playerNumber);
        value = Mathf.Min (value, 1f * (properties.scoreLimit - teamScore) / Mathf.Max (teamIncome, 0.1f));
        if (MaxEmptyTileCount > playableTiles) {
            value = Mathf.Min (value, playableTiles / ((MaxEmptyTileCount - playableTiles) / match.turn));
        }
        value = Mathf.Max (value, 0.001f);
        return value;

    }

}
