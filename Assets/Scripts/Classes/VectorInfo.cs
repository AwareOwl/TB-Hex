using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorInfo {

    public List <AbilityVector> vectors = new List<AbilityVector> ();

    public int strongestValue = 0;
    public List<TileClass> StrongestTargets = new List<TileClass> ();

    public int weakestValue = 999;
    public List<TileClass> WeakestTargets = new List<TileClass> ();

    public TileClass PlayedTokenTile;

    public List<AbilityVector> TriggeredVector = new List<AbilityVector> ();
    public List<AbilityVector> NotTriggeredVector = new List<AbilityVector> ();

    public List<TileClass> Triggered1 = new List<TileClass> ();
    public List<TileClass> Triggered2 = new List<TileClass> ();
    public List<TileClass> NotTriggered = new List<TileClass> ();

    public VectorInfo () {

    }

    public VectorInfo (AbilityVector [] vectors) {
        foreach (AbilityVector vector in vectors) {
            if (vector.target != null){
                this.vectors.Add (vector);
                if (vector.target.IsFilledTile ()) {
                    int value = vector.target.token.value;
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

    public void CheckTriggers (MatchClass match, TileClass playToken, int abilityType) {
        this.PlayedTokenTile = playToken;
        foreach (AbilityVector vector in vectors) {
            switch (abilityType) {
                case 1:
                case 4:
                case 10:
                case 13:
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
            }
        }
        switch (abilityType) {
            case 8:
            case 11:
                if (match.LastPlayedToken () != null) {
                    Triggered2.Add (match.LastPlayedTile ());
                }
                break;

        }
    }
}
