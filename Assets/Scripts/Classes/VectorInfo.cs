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

    public int sumOfAlliesValues = 0;

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
                        sumOfAlliesValues += value;
                    } else if (IsEnemy (vector.target, token.owner)) {
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

    public bool HasRemains (TileClass tile) {
        return tile != null && tile.HasRemains ();
    }

    public bool IsEnemy (TileClass tile, int playerNumber) {
        return IsFilledTile (tile) && tile.token.owner != playerNumber && tile.token.owner != 0;
    }

    public bool IsAlly (TileClass tile, int playerNumber) {
        return IsFilledTile (tile) && tile.token.owner == playerNumber && tile.token.owner != 0;
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

    public void CheckTokenTriggers (MatchClass match, TileClass tokenTile, TokenClass token) {
        if (token == null) {
            return;
        }
        int tokenType = token.type;
        switch (tokenType) {
            case 3:
            case 4:
            case 5:
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
            case 5:
                break;
        }
        foreach (AbilityVector vector in vectors) {
            switch (tokenType) {
                case 5:
                    if (IsFilledTile (vector.target)) {
                        Triggered1.Add (vector.target);
                    }
                    break;

            }
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
                case 24:
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
                    if (IsFilledTile (vector.target) && IsAlly (vector.target, token.owner)) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 21:
                    if (IsFilledTile (vector.target) && IsAlly (vector.target, token.owner) && match.LastPlayedToken () != null) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 23:
                    if (IsFilledTile (vector.target) && IsEnemy (vector.target, token.owner) && match.LastPlayedToken () != null) {
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
                    if (IsFilledTile (vector.target) && IsAlly (vector.target, token.owner) && allyCount == 1) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 25:
                    if (IsEmptyTile (vector.target) && emptyTileCount <= 3) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 26:
                    if (IsFilledTile (vector.target) && strongestValue == vector.target.token.value && weakestValue != vector.target.token.value) {
                        Triggered1.Add (vector.target);
                    } else if (IsFilledTile (vector.target) && weakestValue == vector.target.token.value && strongestValue != vector.target.token.value) {
                        Triggered2.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 27:
                    if (IsFilledTile (vector.target) && vector.target.token.type != token.type) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 28:
                    if (IsFilledTile (vector.target) && IsAlly (vector.target, token.owner) && allyCount == 1 && match.LastPlayedToken () != null) {
                        Triggered1.Add (vector.target);
                        Triggered2.Add (match.LastPlayedTile ());
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 29:
                    if (IsFilledTile (vector.target) && enemyCount <= 2 && IsEnemy (vector.target, token.owner)) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 30:
                    if (IsFilledTile (vector.target) && match.Player [token.owner].score >= 20) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 31:
                case 32:
                    if (HasRemains (vector.target)) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
            }
        }
        switch (abilityType) {
            case 22:
                if (match.LastPlayedToken () != null) {
                    Triggered1.Add (playTile);
                }
                break;

        }
        switch (abilityType) {
            case 8:
            case 11:
            case 18:
            case 21:
            case 22:
            case 23:
                if (match.LastPlayedToken () != null) {
                    Triggered2.Add (match.LastPlayedTile ());
                }
                break;

        }
    }
}
