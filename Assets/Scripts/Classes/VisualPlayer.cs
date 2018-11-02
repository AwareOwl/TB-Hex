using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualPlayer {

    PlayerClass player;
    int playerNumber;
    bool ally;
    int numberOfPlayers;

    int margin;
    int avatarSize;
    int shiftLength;
    int barLength;
    int avatarPosition;
    int barPosition;

    GameObject HealthBar;
    GameObject ScoreText;

    public VisualPlayer () {

    }

    public void CreatePlayerUI (PlayerClass player, bool ally, int numberOfPlayers) {
        GameObject Clone;

        this.player = player;
        playerNumber = player.playerNumber - 1;
        this.ally = ally;
        this.numberOfPlayers = numberOfPlayers;

        int margin = 60;
        int avatarSize = 90;
        int shiftLength = (1440 - margin * 2) / numberOfPlayers;
        int barLength = shiftLength - avatarSize - 30;
        int avatarPosition = margin + avatarSize / 2 + shiftLength * playerNumber;
        int barPosition = margin + barLength / 2 + shiftLength * playerNumber;

        if (ally) {
            barPosition += avatarSize;
        } else {
            avatarPosition += barLength;
        }

        Clone = GOUI.CreateSprite ("Textures/Avatars/Avatar0" + ((playerNumber % 4) + 2).ToString ());
        GOUI.SetInPixScale (Clone, avatarSize, avatarSize);
        GOUI.SetInPixPosition (Clone, avatarPosition, 45, 14);

        Clone = GOUI.CreateSprite ("UI/White");
        GOUI.SetInPixScale (Clone, barLength, 40);
        GOUI.SetInPixPosition (Clone, barPosition, 20, 11);
        GOUI.SetSpriteColor (Clone, AppDefaults.PlayerColor [playerNumber + 1] * 0.5f);

        Clone = GOUI.CreateSprite ("UI/White");
        GOUI.SetSpriteColor (Clone, AppDefaults.PlayerColor [playerNumber + 1]);
        HealthBar = Clone;

        Clone = GOUI.CreateText ("", barPosition, 20, 13, 0.03f);
        Clone.GetComponent<TextMesh> ().color = Color.black;
        ScoreText = Clone;
    }

    public void UpdateVisuals (MatchClass match) {
        SetPlayerHealthBar (match, player);
    }

    public void SetPlayerHealthBar (MatchClass match, PlayerClass player) {
        SetPlayerHealthBar (player.score, player.scoreIncome, match.Properties.scoreLimit);
    }

    public void SetPlayerHealthBar (int score, int scoreIncome, int scoreLimit) {
        int avatarPosition = margin + avatarSize / 2 + shiftLength * player.playerNumber;
        int barPosition = margin + barLength / 2 + shiftLength * player.playerNumber;
        if (ally) {
            barPosition += avatarSize;
        } else {
            avatarPosition += barLength;
        }

        float percentage = score / scoreLimit;
        GOUI.SetInPixScale (HealthBar, (int) (percentage * barLength), 40);
        if (ally) {
            GOUI.SetInPixPosition (HealthBar, barPosition - (int) (barLength / 2 * (1 - percentage)), 20, 12);
        } else {
            GOUI.SetInPixPosition (HealthBar, barPosition + (int) ((barLength * (1 - percentage) + 1) / 2), 20, 12);
        }
        ScoreText.GetComponent<TextMesh> ().text = score.ToString () + " (+" + scoreIncome.ToString () + ")";
    }

}
