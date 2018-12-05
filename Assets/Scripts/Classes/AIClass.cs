﻿using System.Collections;
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
                    if (tempMatch.winner.playerNumber == playerNumber) {
                        tempValue += 100000;
                    } else {
                        tempValue -= 100000;
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
        value += CalculateBoardValue (match, playerNumber);
        return value;
    }

    public float CalculateBoardValue (MatchClass match, int playerNumber) {
        float value = 0;
        TileClass [] tiles = match.Board.tileList.ToArray ();
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
                VectorInfo info = new VectorInfo (match.Board, tile);
                info.CheckTokenAfterTurnTriggers (match, tile);
                float tokenValue = tile.token.value;
                int tokenType = tile.token.type;
                int tokenOwner = tile.token.owner;
                switch (tokenType) {
                    case 1:
                        tokenValue *= 1.9f;
                        break;
                    case 2:
                        tokenValue *= -0.9f;
                        break;
                    case 3:
                        if (info.WeakestTargets.Count == 1) {
                            if (info.WeakestTargets [0].token.owner == tokenOwner) {
                                tokenValue += 1f;
                            } else {
                                tokenValue -= 1f;
                            }
                        }
                        break;
                    case 4:
                        if (info.StrongestTargets.Count == 1) {
                            if (info.StrongestTargets [0].token.owner == tokenOwner) {
                                tokenValue -= 1f;
                            } else {
                                tokenValue += 1f;
                            }
                        }
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

    public float TurnToWinPredict (MatchClass match, int playerNumber) {
        MatchPropertiesClass properties = match.Properties;
        float value = properties.turnLimit - match.turn + 1;
        PlayerClass player = match.Player [playerNumber];
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
