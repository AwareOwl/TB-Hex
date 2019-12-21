using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorInfo {

    public List <AbilityVector> vectors = new List<AbilityVector> ();

    public List<TileClass> targets = new List<TileClass> ();

    public int strongestValue = 0;
    public List<TileClass> strongestTargets = new List<TileClass> ();


    public int weakestValue = 999;
    public List<TileClass> weakestTargets = new List<TileClass> ();

    public int strongerCount = 0;
    public int weakerCount = 0;

    public int sumOfAlliesValues = 0;

    public int emptyTileCount = 0;
    public int tokenCount = 0;
    public int allyCount = 0;
    public int enemyCount = 0;
    public int differentTypesCount = 0;
    public int remainsCount = 0;

    public List<int> triggeredPlayers = new List<int> ();

    public TileClass PlayedTokenTile;

    public List<AbilityVector> TriggeredVector = new List<AbilityVector> ();
    public List<AbilityVector> NotTriggeredVector = new List<AbilityVector> ();

    public List<TileClass> Triggered1 = new List<TileClass> ();
    public List<TileClass> Triggered2 = new List<TileClass> ();
    public List<TileClass> NotTriggered = new List<TileClass> ();

    public VectorInfo () {

    }

    public VectorInfo (AbilityVector [] vectors, TokenClass token, int team) {
        Init (vectors, token, team);
    }

    public void Init (AbilityVector [] vectors, TokenClass token, int team) {
        foreach (AbilityVector vector in vectors) {
            if (vector.target != null) {
                this.vectors.Add (vector);
                if (vector.target.IsFilledTile ()) {
                    targets.Add (vector.target);
                    int value = vector.target.token.value;
                    int type = vector.target.token.type;
                    int owner = vector.target.token.owner;
                    if (strongestValue < value) {
                        strongestTargets = new List<TileClass> ();
                        strongestValue = value;
                    }
                    if (strongestValue == value) {
                        strongestTargets.Add (vector.target);
                    }

                    if (weakestValue > value) {
                        weakestTargets = new List<TileClass> ();
                        weakestValue = value;
                    }
                    if (weakestValue == value) {
                        weakestTargets.Add (vector.target);
                    }

                    tokenCount++;
                    if (token.value > value) {
                        weakerCount++;
                    }
                    if (token.value < value) {
                        strongerCount++;
                    }

                    if (token.type != type) {
                        differentTypesCount++;
                    }

                    if (IsAllyTeam (vector.target, team)) {
                        allyCount++;
                        sumOfAlliesValues += value;
                    } else if (IsEnemyTeam (vector.target, team)) {
                        enemyCount++;
                    }
                    if (!triggeredPlayers.Contains (owner)) {
                        triggeredPlayers.Add (owner);
                    }
                }
                if (vector.target.IsEmptyTile ()) {
                    emptyTileCount++;
                    if (vector.target.HasRemains ()) {
                        remainsCount++;
                    }
                }
            }

        }
    }

    public bool IsFilledTile (TileClass tile) {
        return RelationLogic.IsFilledTile (tile);
    }

    public bool HasRemains (TileClass tile) {
        return tile != null && tile.HasRemains ();
    }

    public bool IsEnemyTeam (TileClass tile, int teamNumber) {
        return IsEnemyTeam (tile.GetTeam (), teamNumber);
    }

    public bool IsEnemyTeam (int teamNumber1, int teamNumber2) {
        return RelationLogic.IsEnemyTeam (teamNumber1, teamNumber2);
    }

    public bool IsAllyTeam (TileClass tile, int teamNumber) {
        return IsAllyTeam (tile.GetTeam (), teamNumber);
    }

    public bool IsAllyTeam (int teamNumber1, int teamNumber2) {
        return RelationLogic.IsAllyTeam (teamNumber1, teamNumber2);
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

    public bool IsPushableToEmptyTile (TileClass sourceTile, TileClass destinationTile) {
        if (IsFilledTile (sourceTile) && IsEmptyTile (destinationTile)) {
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
            case 22:
            case 23:
                break;
            default:
                return;
        }
        switch (tokenType) {
            case 3:
                if (weakestTargets.Count == 1) {
                    Triggered1.Add (weakestTargets [0]);
                }
                break;
            case 4:
                if (strongestTargets.Count == 1) {
                    Triggered1.Add (strongestTargets [0]);
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
                case 22:
                    if (IsFilledTile (vector.target) && IsAllyTeam (match.GetTeam (vector.target), match.GetTeam (token.owner))){

                        Triggered1.Add (vector.target);
                    }
                    break;
                case 23:
                    if (IsFilledTile (vector.target) && IsEnemyTeam (match.GetTeam (vector.target), match.GetTeam (token.owner)) && vector.target.token.value == token.value) {

                        Triggered1.Add (vector.target);
                    }
                    break;

            }
        }

    }

    public void CheckAbilityTriggers (MatchClass match, int playerNumber, TileClass playTile, int abilityType, TokenClass token) {
        this.PlayedTokenTile = playTile;
        foreach (AbilityVector vector in vectors) {
            TokenClass targetToken = vector.target.token;
            bool isEnemy = false;
            bool isAlly = false;
            if (IsFilledTile (vector.target)) {
                isEnemy = IsEnemyTeam (match.GetTeam (vector.target), match.GetTeam (token.owner));
                isAlly = IsAllyTeam (match.GetTeam (vector.target), match.GetTeam (token.owner));
            }
            switch (abilityType) {
                case 1:
                case 4:
                case 10:
                case 13:
                case 20:
                case 24:
                case 46:
                case 51:
                case 58:
                case 59:
                case 71:
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
                case 53:
                    if (IsEmptyTile (vector.target)) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 5:
                    if (!vector.directional) {
                        break;
                    }
                    if (IsPushable (vector.target, vector.pushTarget)) {
                        TriggeredVector.Add (vector);
                    } else {
                        NotTriggeredVector.Add (vector);
                    }
                    break;
                case 8:
                    if (IsFilledTile (vector.target) && isEnemy && match.LastPlayedToken () != null) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 9:
                    if (IsFilledTile (vector.target) && weakestTargets.Count == 1 && vector.target.token.value == weakestValue) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 12:
                    if (IsFilledTile (vector.target) && strongestTargets.Count == 1 && vector.target.token.value == strongestValue) {
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
                case 47:
                case 52:
                case 55:
                    if (IsFilledTile (vector.target) && isAlly) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 21:
                    if (IsFilledTile (vector.target) && isAlly && match.LastPlayedToken () != null) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 23:
                    if (IsFilledTile (vector.target) && isEnemy && match.LastPlayedToken () != null) {
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
                    if (IsFilledTile (vector.target) && isAlly && allyCount == 1) {
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
                    if (IsFilledTile (vector.target) && isAlly && allyCount == 1 && match.LastPlayedToken () != null) {
                        Triggered1.Add (vector.target);
                        Triggered2.Add (match.LastPlayedTile ());
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 29:
                    if (IsFilledTile (vector.target) && enemyCount <= 2 && isEnemy) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 30:
                case 33:
                    if (IsFilledTile (vector.target) && match.Player [token.owner].score >= 20) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 31:
                case 32:
                case 60:
                    if (HasRemains (vector.target)) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 34:
                    if (!vector.directional) {
                        break;
                    }
                    if (IsPushableToEmptyTile (vector.target, vector.pushTarget)) {
                        TriggeredVector.Add (vector);
                    } else {
                        NotTriggeredVector.Add (vector);
                    }
                    break;
                case 35:
                    if (IsFilledTile (vector.target) && vector.target.token.value < token.value) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 36:
                case 37:
                    if (targets.Count == 2 && IsFilledTile (vector.target)) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 39:
                    if (emptyTileCount >= 1 && enemyCount == 1) {
                        if (IsEmptyTile (vector.target)) {
                            Triggered1.Add (vector.target);
                        } else if (isEnemy) {
                            Triggered2.Add (vector.target);
                        }
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 40:
                    if (HasRemains (vector.target)) {
                        Triggered2.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 41:
                    if (IsFilledTile (vector.target) && strongerCount == 1 && vector.target.token.value > token.value) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 43:
                    if (IsFilledTile (vector.target) && strongestValue == vector.target.token.value && weakestValue != vector.target.token.value) {
                        Triggered2.Add (vector.target);
                    } else if (IsFilledTile (vector.target) && weakestValue == vector.target.token.value && strongestValue != vector.target.token.value) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 44:
                    if (IsFilledTile (vector.target) && vector.target.token.value > token.value) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 48:
                    if (IsEmptyTile (vector.target) && emptyTileCount * 20 <= match.Player [playerNumber].score) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 50:
                    if (IsFilledTile (vector.target) && isEnemy && enemyCount == 1) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 54:
                    if (IsFilledTile (vector.target) && isEnemy) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 56:
                    if (IsFilledTile (vector.target) && weakestTargets.Count == 1) {
                        if (vector.target.token.value == weakestValue) {
                            Triggered1.Add (vector.target);
                        } else if (vector.target.token.owner == weakestTargets [0].token.owner) {
                            Triggered2.Add (vector.target);
                        }
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 57:
                    if (IsFilledTile (vector.target)) {
                        if (weakestValue == vector.target.token.value) {
                            Triggered2.Add (vector.target);
                        } else {
                            Triggered1.Add (vector.target);
                        }
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 63:
                case 65:
                case 70:
                    if (IsFilledTile (vector.target) && 
                        match.GetPlayer (targetToken.owner) != null && match.GetPlayer (targetToken.owner).score >= match.Player [token.owner].score + 20) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 64:
                    if (IsFilledTile (vector.target) && weakestTargets.Count == 1 && weakestValue == vector.target.token.value &&
                        match.GetPlayer (targetToken.owner) != null && match.GetPlayer (targetToken.owner).score >= match.Player [token.owner].score + 20) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 66:
                    if (IsFilledTile (vector.target) && isEnemy && enemyCount == 1) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 67:
                case 68:
                    if (IsFilledTile (vector.target) && targetToken.value == emptyTileCount) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case 69:
                    if (IsFilledTile (vector.target) &&
                        match.GetPlayer (targetToken.owner) != null && match.GetPlayer (targetToken.owner).scoreIncome >= 20) {
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
            case 30:
            case 33:
            case 48:
                if (match.Player [token.owner].score >= 20) {
                    Triggered2.Add (playTile);
                }
                break;
            case 40:
                if (remainsCount < 2) {
                    Triggered1.Add (playTile);
                }
                break;
            case 54:
                Triggered2.Add (playTile);
                break;

        }
        for (int x = 0; x < triggeredPlayers.Count; x++) {
            int player = triggeredPlayers [x];
            switch (abilityType) {
            }
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
        switch (abilityType) {
            case 44:
            case 53:
                Triggered2.Add (playTile);
                break;
        }
    }
}
