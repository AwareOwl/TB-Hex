using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchPropertiesClass {

    public int gameMode;
    public bool special;
    public bool scoreWinCondition = true;
    public int scoreLimit = 500;
    public bool turnWinCondition = true;
    public int turnLimit = 40;
    public bool allowToRotateAbilityAreas;
    public bool usedCardsArePutOnBottomOfStack;

    public MatchPropertiesClass () {

    }

    public MatchPropertiesClass (int gameMode) {
        this.gameMode = gameMode;
        scoreWinCondition = ServerData.GetGameModeHasScoreWinCondition (gameMode);
        scoreLimit = ServerData.GetGameModeScoreWinConditionValue (gameMode);
        turnWinCondition = ServerData.GetGameModeHasTurnWinCondition (gameMode);
        turnLimit = ServerData.GetGameModeTurnWinConditionValue (gameMode);
        allowToRotateAbilityAreas = ServerData.GetGameModeIsAllowedToRotateCardsDuringMatch (gameMode);
        usedCardsArePutOnBottomOfStack = ServerData.GetGameModeUsedCardsArePutOnBottomOfStack (gameMode);
    }

    public string [] MatchPropertiesToString () {
        List<string> s = new List<string> ();
        s.Add (gameMode.ToString ());
        s.Add (special.ToString ());
        s.Add (scoreWinCondition.ToString ());
        s.Add (scoreLimit.ToString ());
        s.Add (turnWinCondition.ToString ());
        s.Add (turnLimit.ToString ());
        s.Add (allowToRotateAbilityAreas.ToString ());
        s.Add (usedCardsArePutOnBottomOfStack.ToString ());
        return s.ToArray ();
    }

    public void LoadFromString (string [] lines) {
        gameMode = int.Parse (lines [0]);
        special = Convert.ToBoolean (lines [1]);
        scoreWinCondition = Convert.ToBoolean (lines [2]);
        scoreLimit = int.Parse (lines [3]);
        turnWinCondition = Convert.ToBoolean (lines [4]);
        turnLimit = int.Parse (lines [5]);
        allowToRotateAbilityAreas = Convert.ToBoolean (lines [6]);
        usedCardsArePutOnBottomOfStack = Convert.ToBoolean (lines [7]);
    }
}
