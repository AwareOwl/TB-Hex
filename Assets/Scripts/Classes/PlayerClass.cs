using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass {

    public HandClass Hand;

    public int playerNumber;
    public int score;
    public int scoreIncome;

    public PlayerPropertiesClass Properties;

    public VisualPlayer visualPlayer;

    public PlayerClass () {

    }

    public PlayerClass (int playerNumber) {
        this.playerNumber = playerNumber;
        Properties = new PlayerPropertiesClass ();
        Hand = new HandClass ();
    }

    public PlayerClass (PlayerClass player) {
        playerNumber = player.playerNumber;
        Properties = player.Properties;
        Hand = player.Hand;
    }

    public void MoveTopCard (int stackNumber) {
        Hand.MoveTopCard (stackNumber);
    }

    public void SetScoreIncome (int scoreIncome) {
        this.scoreIncome = scoreIncome;
    }

    public void AddScore (int scoreToAdd) {
        this.score += scoreToAdd;
    }

    public void UpdateVisuals (MatchClass match) {
        if (visualPlayer != null) {
            visualPlayer.UpdateVisuals (match);
        }
    }

    public void EnableVisuals () {
        if (visualPlayer == null) {
            visualPlayer = new VisualPlayer ();
            if (playerNumber == InGameUI.MyPlayerNumber) {
                Hand.EnableVisual ();
            }
            //visualPlayer = new VisualPlayer ();
        }
    }


}
