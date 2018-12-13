using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorInfo {

    public List <AbilityVector> vectors = new List<AbilityVector> ();

    public int strongestValue = 0;
    public List<TileClass> StrongestTargets = new List<TileClass> ();


    public int weakestValue = 999;
    public List<TileClass> WeakestTargets = new List<TileClass> ();

    public int strongerCount = 0;
    public int weakerCount = 0;

    public int emptyTileCount = 0;

    public int allyCount = 0;
    public int enemyCount = 0;

    public List<int> TargetPlayers = new List<int> ();

    public TileClass PlayedTokenTile;

    public List<AbilityVector> TriggeredVector = new List<AbilityVector> ();
    public List<AbilityVector> NotTriggeredVector = new List<AbilityVector> ();

    public List<TileClass> Triggered1 = new List<TileClass> ();
    public List<TileClass> Triggered2 = new List<TileClass> ();
    public List<TileClass> NotTriggered = new List<TileClass> ();

    public VectorInfo () {

    }

    public VectorInfo (BoardClass board, TileClass tile) {
        Init (board.GetAbilityVectors (tile.x, tile.y, 4), tile.token);
    }

    public VectorInfo (AbilityVector [] vectors, TokenClass token) {
        Init (vectors, token);
    }

    public void Init (AbilityVector [] vectors, TokenClass token) {
        foreach (AbilityVector vector in vectors) {
            if (vector.target != null) {
                this.vectors.Add (vector);
                if (vector.target.IsFilledTile ()) {
                    int value = vector.target.token.value;
                    int owner = vector.target.token.owner;
                    if (strongestValue < value) {
                        StrongestTargets = new List<TileClass> ();
                        strongestValue = value;
                    }
                    if (strongestValue == value) {
                        StrongestTargets.Add (vector.target);
                    }

                    if (weakestValue > value) {
                        WeakestTargets = new List<TileClass> ();
                        weakestValue = value;
                    }
                    if (weakestValue == value) {
                        WeakestTargets.Add (vector.target);
                    }

                    if (token.value > value) {
                        weakerCount++;
                    }
                    if (token.value < value) {
                        strongerCount++;
                    }


                    if (token.owner == owner) {
                        allyCount++;
                    } else {
                        enemyCount++;
                    }
                    if (!TargetPlayers.Contains (owner)) {
                        TargetPlayers.Add (owner);
                    }
                }
                if (vector.target.IsEmptyTile ()) {
                    emptyTileCount++;
                }
            }

        }
    }

    public bool IsFilledTile (TileClass tile) {
        return tile != null && tile.IsFilledTile ();
    }

    public bool IsEnemy (TileClass tile, int playerNumber) {
        return IsFilledTile (tile) && tile.token.owner != playerNumber;
    }

    public bool IsEmptyTile (TileClass tile) {
        return tile != null && tile.IsEmptyTile ();
    }

    public bool IsPushable (TileClass sourceTile, TileClass destinationTile) {
        if (IsFilledTile (sourceTile) && !IsFilledTile (destinationTile)) {
            return true;
        } else {
            return false;
        }
    }

    public void CheckTokenAfterTurnTriggers (MatchClass match, TileClass tokenTile, TokenClass token) {
        if (token == null) {
            return;
        }
        int tokenType = token.type;
        switch (tokenType) {
            case 3:
            case 4:
                break;
            default:
                return;
        }
        switch (tokenType) {
            case 3:
                if (WeakestTargets.Count == 1) {
                    Triggered1.Add (WeakestTargets [0]);
                }
                break;
            case 4:
                if (StrongestTargets.Count == 1) {
                    Triggered1.Add (StrongestTargets [0]);
                }
                break;
            default:
                return;
        }

    }

    public void CheckAbilityTriggers (MatchClass match, TileClass playTile, int abilityType, TokenClass token) {
        this.PlayedTokenTile = playTile;
        foreach (AbilityVector vector in vectors) {
            switch (abilityType) {
                case 1:
                case 4:
                case 10:
                case 13:
                case 20:
                    if (IsFilledTile (vector.target)) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 2:
                    if (IsFilledTile (vector.target)) {
                        Triggered1.Add (vector.target);
                    }
                    if (IsEmptyTile (vector.target)) {
                        Triggered2.Add (vector.target);
                    }
                    break;
                case 3:
                case 6:
                    if (IsEmptyTile (vector.target)) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 5:
                    if (IsPushable (vector.target, vector.pushTarget)) {
                        TriggeredVector.Add (vector);
                    } else {
                        NotTriggeredVector.Add (vector);
                    }
                    break;
                case 8:
                    if (IsEnemy (vector.target, match.turnOfPlayer) && match.LastPlayedToken () != null) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 9:
                    if (IsFilledTile (vector.target) && WeakestTargets.Count == 1 && vector.target.token.value == weakestValue) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 12:
                    if (IsFilledTile (vector.target) && StrongestTargets.Count == 1 && vector.target.token.value == strongestValue) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 14:
                case 16:
                case 17:
                    if (IsFilledTile (vector.target) && weakerCount == 1 && vector.target.token.value < token.value) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 15:
                    if (IsFilledTile (vector.target) && vector.target.token.owner == token.owner) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 18:
                    if (IsEmptyTile (vector.target) && emptyTileCount == 1 && match.LastPlayedToken () != null) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 19:
                    if (IsFilledTile (vector.target) && vector.target.token.owner == token.owner && allyCount == 1) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
            }
        }
        switch (abilityType) {
            case 8:
            case 11:
            case 18:
                if (match.LastPlayedToken () != null) {
                    Triggered2.Add (match.LastPlayedTile ());
                }
                break;

        }
    }
}
