using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIClass {

    float difficulty = 1;

    public float MaxEmptyTileCount;

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
                        tempValue += 10000000;
                    } else {
                        tempValue -= 10000000;
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
                value += myScoreIncome - match.Player [x].scoreIncome;
            }
        }
        value += playerValue;
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
