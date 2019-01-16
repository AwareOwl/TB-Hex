using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIClass {

    int difficulty = 100;

    public int edgeDanger = 5;
    public int surroundDanger = 5;
    public int multiTargetDanger = 5;

    public float MaxEmptyTileCount;

    public AIClass () {
        edgeDanger = Random.Range (0, 10);
        surroundDanger = Random.Range (0, 10);
        multiTargetDanger = Random.Range (0, 10);
    }

    public Vector3Int FindBestMove (MatchClass match) {
        int playerNumber = match.turnOfPlayer;
        PlayerClass player = match.Player [playerNumber];
        TileClass [] tiles = match.Board.GetEmptyTiles ().ToArray ();
        TileClass bestTile = null;
        float bestValue = -999999f;
        int bestStack = 0;
        int numberOfBests = 0;
        foreach (TileClass tile in tiles) {
            if (tile == null || !tile.enabled || tile.token != null) {
                //Debug.Log ("ło kurwa");
            }
            for (int x = 0; x < player.topCardNumber.Length; x++) {
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
        float myScoreIncome = match.Player [playerNumber].scoreIncome;
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
                            if (tVE.WeakestTargets.Count == 1 && tVE.WeakestTargets [0] == tile) {
                                tokenValue = valueOverTime (tokenValue, 1, 1, turnsLeft);
                            }
                            break;
                        case 4:
                            if (tVE.StrongestTargets.Count == 1 && tVE.StrongestTargets [0] == tile) {
                                tokenValue = valueOverTime (tokenValue, -1, 1, turnsLeft);
                            }
                            break;
                        case 5:
                            tokenValue = valueOverTime (tokenValue, -3, targetValue, turnsLeft);
                            break;
                        case 8:
                            if (tile.token.value < vector.target.token.value) {
                                tokenValue = 0;
                            }
                            break;
                    }

                }
                tokenValue = Mathf.Max (tokenValue, 0);
                switch (tokenType) {
                    case 1:
                        tokenValue *= 1.9f;
                        break;
                    case 2:
                        tokenValue *= -0.9f;
                        break;
                    case 5:
                        tokenValue = (tokenValue * (tokenValue + 1) - Mathf.Max (tokenValue - turnsLeft, 0) * (tokenValue - turnsLeft + 1)) / 2 / turnsLeft;
                        break;
                    case 9:
                        tokenValue += 1;
                        break;
                }
                riskValue = tokenValue + 1 - Mathf.Sqrt (tokenValue + 1);
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
