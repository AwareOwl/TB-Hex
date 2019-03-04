using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIClass {

    static public float maxCardValue = 0;

    int difficulty = 100;

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
        edgeDanger = Random.Range (0, 12);
        surroundDanger = Random.Range (3, 8);
        multiTargetDanger = Random.Range (0, 8);

        abilityRow = Random.Range (7, 14);
        tokenRow = Random.Range (8, 16);
        abilityTokenRow = Random.Range (12, 28);

        abilityStackSize = Random.Range (3, 7);
        tokenStackSize = Random.Range (0, 6);
        abilityTokenStackSize = Random.Range (3, 8);

        abilityAfterAbility = Random.Range (4, 9);
        abilityAfterToken = Random.Range (1, 14);
        tokenAfterAbility = Random.Range (1, 14);
        tokenAfterToken = Random.Range (0, 12);

        ability_AbilitySynergy = Random.Range (4, 10);
        ability_TokenSynergy = Random.Range (6, 19);
        token_TokenSynergy = Random.Range (6, 9);

        abilityAgainstAbility = Random.Range (4, 7);
        abilityAgainstToken = Random.Range (4, 7);
        tokenAgainstAbility = Random.Range (4, 7);
        tokenAgainstToken = Random.Range (4, 7);
    }

    public Vector3Int FindBestMove (MatchClass match) {
        int playerNumber = match.turnOfPlayer;
        PlayerClass player = match.Player [playerNumber];
        TileClass [] tiles = match.Board.GetEmptyTiles ().ToArray ();
        TileClass bestTile = null;
        float bestValue = -999999f;
        int bestStack = 0;
        int numberOfBests = 0;
        if (tiles.Length == 0){
            Debug.Log ("Board error, turn: " + match.turn.ToString() + ", board: " + match.Board.boardTemplateId.ToString() + ", all tiles count: " + match.Board.tileList.Count);
            Debug.Log (match.finished);
        }
        foreach (TileClass tile in tiles) {
            if (tile == null || !tile.enabled || tile.token != null) {
                Debug.Log ("Tile error");
            }
            for (int x = 0; x < player.hand.stack.Length; x++) {
                MatchClass tempMatch = new MatchClass (match);
                tempMatch.real = false;
                tempMatch.PlayCard (tile.x, tile.y, playerNumber, x);
                float tempValue = CalculateMatchValue (tempMatch, playerNumber);
                if (tempMatch.finished) {
                    if (tempMatch.winner != null) {
                        if (tempMatch.winner.properties.playerNumber == playerNumber) {
                            tempValue += 100000;
                        } else {
                            tempValue -= 100000;
                        }
                    }
                }
                if (bestTile == null || bestValue < tempValue) {
                    bestTile = tile;
                    bestStack = x;
                    bestValue = tempValue;
                    numberOfBests = 1;
                } else if (bestValue == tempValue) {
                    numberOfBests++;
                    if (Random.Range (0, numberOfBests) == 0) {
                        bestTile = tile;
                        bestStack = x;
                        bestValue = tempValue;
                    }
                }
            }
        }
        return new Vector3Int (bestTile.x, bestTile.y, bestStack);
    }

    public float CalculateMatchValue (MatchClass match, int playerNumber) {
        float value = 0;
        float playerValue = 4f;
        float myTurnsToWin = TurnToWinPredict (match, playerNumber);
        for (int x = 1; x <= match.numberOfPlayers; x++) {
            if (x != playerNumber) {
                playerValue *= TurnToWinPredict (match, x)/ myTurnsToWin;
                //value += myScoreIncome - match.Player [x].scoreIncome;
            }
        }
        //value += playerValue;
        value += CalculateBoardValue (match, playerNumber, myTurnsToWin);
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
                            if (vector.flipTarget != null && vector.flipTarget.IsEmptyTile ()) {
                                edgeCount++;
                            }
                        } else if (vector.target.IsEmptyTile ()) {
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
                int tokenType = tile.token.type;
                int tokenOwner = tile.token.owner;
                foreach (AbilityVector vector in oVE.vectors) {
                    TileClass targetTile = vector.target;
                    if (!targetTile.IsFilledTile ()) {
                        continue;
                    }
                    VectorInfo tVE = VE [targetTile.x, targetTile.y];
                    TokenClass targetToken = targetTile.token;
                    int targetType = targetToken.type;
                    int targetValue = targetToken.value;
                    switch (targetType) {
                        case 3:
                            if (tVE.weakestTargets.Count == 1 && tVE.weakestTargets [0] == tile) {
                                tokenValue = valueOverTime (tokenValue, 1, 1, turnsLeft);
                            }
                            break;
                        case 4:
                            if (tVE.strongestTargets.Count == 1 && tVE.strongestTargets [0] == tile) {
                                tokenValue = valueOverTime (tokenValue, -1, 1, turnsLeft);
                            }
                            break;
                        case 5:
                            tokenValue = valueOverTime (tokenValue, -3, targetValue, turnsLeft);
                            break;
                        case 8:
                            if (tokenValue < targetValue) {
                                tokenValue = 0;
                            }
                            break;
                    }

                }
                tokenValue = Mathf.Max (tokenValue, 0);
                switch (tokenType) {
                    case 1:
                    case 12:
                        tokenValue *= 1.9f;
                        break;
                    case 2:
                        tokenValue *= -0.9f;
                        break;
                    case 5:
                        tokenValue = (tokenValue * (tokenValue + 1) - Mathf.Max (tokenValue - turnsLeft, 0) * (tokenValue - turnsLeft + 1)) / 2 / turnsLeft;
                        break;
                    case 6:
                        tokenValue *= 1.06f / match.numberOfPlayers;
                        break;
                    case 9:
                    case 11:
                        tokenValue += 1;
                        break;
                    case 10:
                        tokenValue = valueOverTime (tokenValue + oVE.emptyTileCount, - oVE.emptyTileCount, oVE.emptyTileCount, turnsLeft);
                        break;
                    case 13:
                        tokenValue += 0.3f / tokenValue;
                        break;
                }
                switch (tokenType) {
                    case 12:
                        riskValue = Mathf.Sqrt (tokenValue) - 0.2f;
                        break;
                    default:
                        riskValue = tokenValue + 1 - Mathf.Sqrt (tokenValue + 1);
                        break;
                }
                riskValue *= Mathf.Min (1f, (
                    dangerCount * surroundDanger +
                    edgeCount * edgeDanger) / 100f);
                if (tile.token.owner != playerNumber) {
                    tokenValue *= -1;
                }
                tokenValue -= riskValue;
                value += tokenValue;
            } else {
                riskValue *= multiDangerCount * multiTargetDanger / 200f;
                value -= riskValue;
            }
        }
        return value;
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
        MatchPropertiesClass properties = match.Properties;
        float value = properties.turnLimit - match.turn + 1;
        BoardClass board = match.Board;
        int emptyTiles = board.GetEmptyTilesCount ();
        value = Mathf.Min (value, 1f * (properties.scoreLimit - player.score) / Mathf.Max (player.scoreIncome, 0.1f));
        if (MaxEmptyTileCount > emptyTiles) {
            value = Mathf.Min (value, emptyTiles / ((MaxEmptyTileCount - emptyTiles) / match.turn));
        }
        value = Mathf.Max (value, 0.001f);
        return value;

    }

}
