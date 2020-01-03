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
                    TokenType tokenType = vector.target.token.type;
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

                    if (token.type != tokenType) {
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
        TokenType tokenType = token.type;
        switch (tokenType) {
            case TokenType.T3:
            case TokenType.T4:
            case TokenType.T5:
            case TokenType.T22:
            case TokenType.T23:
                break;
            default:
                return;
        }
        switch (tokenType) {
            case TokenType.T3:
                if (weakestTargets.Count == 1) {
                    Triggered1.Add (weakestTargets [0]);
                }
                break;
            case TokenType.T4:
                if (strongestTargets.Count == 1) {
                    Triggered1.Add (strongestTargets [0]);
                }
                break;
            case TokenType.T5:
                break;
        }
        foreach (AbilityVector vector in vectors) {
            switch (tokenType) {
                case TokenType.T5:
                    if (IsFilledTile (vector.target)) {
                        Triggered1.Add (vector.target);
                    }
                    break;
                case TokenType.T22:
                    if (IsFilledTile (vector.target) && IsAllyTeam (match.GetTeam (vector.target), match.GetTeam (token.owner))){

                        Triggered1.Add (vector.target);
                    }
                    break;
                case TokenType.T23:
                    if (IsFilledTile (vector.target) && IsEnemyTeam (match.GetTeam (vector.target), match.GetTeam (token.owner)) && vector.target.token.value == token.value) {

                        Triggered1.Add (vector.target);
                    }
                    break;

            }
        }

    }

    public void CheckAbilityTriggers (MatchClass match, int playerNumber, TileClass playTile, AbilityType abilityType, TokenClass token) {
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
                case AbilityType.T1:
                case AbilityType.T4:
                case AbilityType.T10:
                case AbilityType.T13:
                case AbilityType.T20:
                case AbilityType.T24:
                case AbilityType.T46:
                case AbilityType.T51:
                case AbilityType.T58:
                case AbilityType.T59:
                case AbilityType.T71:
                    if (IsFilledTile (vector.target)) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T2:
                    if (IsFilledTile (vector.target)) {
                        Triggered1.Add (vector.target);
                    }
                    if (IsEmptyTile (vector.target)) {
                        Triggered2.Add (vector.target);
                    }
                    break;
                case AbilityType.T3:
                case AbilityType.T6:
                case AbilityType.T53:
                    if (IsEmptyTile (vector.target)) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T5:
                    if (!vector.directional) {
                        break;
                    }
                    if (IsPushable (vector.target, vector.pushTarget)) {
                        TriggeredVector.Add (vector);
                    } else {
                        NotTriggeredVector.Add (vector);
                    }
                    break;
                case AbilityType.T8:
                    if (IsFilledTile (vector.target) && isEnemy && match.LastPlayedToken () != null) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T9:
                    if (IsFilledTile (vector.target) && weakestTargets.Count == 1 && vector.target.token.value == weakestValue) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T12:
                    if (IsFilledTile (vector.target) && strongestTargets.Count == 1 && vector.target.token.value == strongestValue) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T14:
                case AbilityType.T16:
                case AbilityType.T17:
                    if (IsFilledTile (vector.target) && weakerCount == 1 && vector.target.token.value < token.value) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T15:
                case AbilityType.T47:
                case AbilityType.T52:
                case AbilityType.T55:
                    if (IsFilledTile (vector.target) && isAlly) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T74:
                    if (IsFilledTile (vector.target) && isAlly) {
                        Triggered1.Add (vector.target);
                    } else if (HasRemains (vector.target)){
                        Triggered2.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T21:
                    if (IsFilledTile (vector.target) && isAlly && match.LastPlayedToken () != null) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T23:
                    if (IsFilledTile (vector.target) && isEnemy && match.LastPlayedToken () != null) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T18:
                    if (IsEmptyTile (vector.target) && emptyTileCount == 1 && match.LastPlayedToken () != null) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T19:
                    if (IsFilledTile (vector.target) && isAlly && allyCount == 1) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T25:
                    if (IsEmptyTile (vector.target) && emptyTileCount <= 3) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T26:
                    if (IsFilledTile (vector.target) && strongestValue == vector.target.token.value && weakestValue != vector.target.token.value) {
                        Triggered1.Add (vector.target);
                    } else if (IsFilledTile (vector.target) && weakestValue == vector.target.token.value && strongestValue != vector.target.token.value) {
                        Triggered2.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T27:
                    if (IsFilledTile (vector.target) && vector.target.token.type != token.type) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T28:
                    if (IsFilledTile (vector.target) && isAlly && allyCount == 1 && match.LastPlayedToken () != null) {
                        Triggered1.Add (vector.target);
                        Triggered2.Add (match.LastPlayedTile ());
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T75:
                    if (IsFilledTile (vector.target) && tokenCount <= 2) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T29:
                    if (IsFilledTile (vector.target) && enemyCount <= 2 && isEnemy) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T30:
                case AbilityType.T33:
                    if (IsFilledTile (vector.target) && match.Player [token.owner].score >= 20) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T31:
                case AbilityType.T32:
                case AbilityType.T60:
                    if (HasRemains (vector.target)) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T34:
                    if (!vector.directional) {
                        break;
                    }
                    if (IsPushableToEmptyTile (vector.target, vector.pushTarget)) {
                        TriggeredVector.Add (vector);
                    } else {
                        NotTriggeredVector.Add (vector);
                    }
                    break;
                case AbilityType.T35:
                    if (IsFilledTile (vector.target) && vector.target.token.value < token.value) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T36:
                case AbilityType.T37:
                    if (targets.Count == 2 && IsFilledTile (vector.target)) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T39:
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
                case AbilityType.T40:
                    if (HasRemains (vector.target)) {
                        Triggered2.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T41:
                    if (IsFilledTile (vector.target) && strongerCount == 1 && vector.target.token.value > token.value) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T43:
                    if (IsFilledTile (vector.target) && strongestValue == vector.target.token.value && weakestValue != vector.target.token.value) {
                        Triggered2.Add (vector.target);
                    } else if (IsFilledTile (vector.target) && weakestValue == vector.target.token.value && strongestValue != vector.target.token.value) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T44:
                    if (IsFilledTile (vector.target) && vector.target.token.value > token.value) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T48:
                    if (IsEmptyTile (vector.target) && emptyTileCount * 20 <= match.Player [playerNumber].score) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T50:
                    if (IsFilledTile (vector.target) && isEnemy && enemyCount == 1) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T54:
                    if (IsFilledTile (vector.target) && isEnemy) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T56:
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
                case AbilityType.T57:
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
                case AbilityType.T63:
                case AbilityType.T65:
                case AbilityType.T70:
                    if (IsFilledTile (vector.target) && 
                        match.GetPlayer (targetToken.owner) != null && match.GetPlayer (targetToken.owner).score >= match.Player [token.owner].score + 20) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T64:
                    if (IsFilledTile (vector.target) && weakestTargets.Count == 1 && weakestValue == vector.target.token.value &&
                        match.GetPlayer (targetToken.owner) != null && match.GetPlayer (targetToken.owner).score >= match.Player [token.owner].score + 20) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T66:
                    if (IsFilledTile (vector.target) && isEnemy && enemyCount == 1) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T67:
                case AbilityType.T68:
                    if (IsFilledTile (vector.target) && targetToken.value == emptyTileCount) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T69:
                    if (IsFilledTile (vector.target) &&
                        match.GetPlayer (targetToken.owner) != null && match.GetPlayer (targetToken.owner).scoreIncome >= 20) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
                case AbilityType.T73:
                    if (IsFilledTile (vector.target) && tokenCount == 1 && match.LastPlayedToken () != null) {
                        Triggered1.Add (vector.target);
                    } else {
                        NotTriggered.Add (vector.target);
                    }
                    break;
            }
        }
        switch (abilityType) {
            case AbilityType.T22:
                if (match.LastPlayedToken () != null) {
                    Triggered1.Add (playTile);
                }
                break;
            case AbilityType.T30:
            case AbilityType.T33:
            case AbilityType.T48:
                if (match.Player [token.owner].score >= 20) {
                    Triggered2.Add (playTile);
                }
                break;
            case AbilityType.T40:
                if (remainsCount < 2) {
                    Triggered1.Add (playTile);
                }
                break;
            case AbilityType.T54:
                Triggered2.Add (playTile);
                break;

        }
        for (int x = 0; x < triggeredPlayers.Count; x++) {
            int player = triggeredPlayers [x];
            switch (abilityType) {
            }
        }
        switch (abilityType) {
            case AbilityType.T8:
            case AbilityType.T11:
            case AbilityType.T18:
            case AbilityType.T21:
            case AbilityType.T22:
            case AbilityType.T23:
                if (match.LastPlayedToken () != null) {
                    Triggered2.Add (match.LastPlayedTile ());
                }
                break;
            case AbilityType.T73:
                if (match.LastPlayedToken () != null && tokenCount == 1) {
                    Triggered2.Add (match.LastPlayedTile ());
                }
                break;

        }
        switch (abilityType) {
            case AbilityType.T44:
            case AbilityType.T53:
                Triggered2.Add (playTile);
                break;
        }
    }
}
