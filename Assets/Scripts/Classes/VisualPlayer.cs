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
    GameObject UserNameText;
    GameObject Highlight;

    public VisualPlayer () {

    }

    public void CreatePlayerUI (PlayerClass player, bool ally, int numberOfPlayers) {
        GameObject Clone;

        this.player = player;
        playerNumber = player.properties.playerNumber - 1;
        this.ally = ally;
        this.numberOfPlayers = numberOfPlayers;

        margin = 120;
        avatarSize = 70;
        shiftLength = (1440 - margin * 2) / numberOfPlayers;
        barLength = shiftLength - avatarSize - 30;
        avatarPosition = margin + avatarSize / 2 + shiftLength * playerNumber;
        barPosition = margin + barLength / 2 + shiftLength * playerNumber;

        if (ally) {
            barPosition += avatarSize;
        } else {
            avatarPosition += barLength;
        }

        Clone = GOUI.CreateSprite ("Textures/Avatars/Avatar0" + ((playerNumber % 4) + 2).ToString ());
        GOUI.SetInPixScale (Clone, avatarSize, avatarSize);
        GOUI.SetInPixPosition (Clone, avatarPosition, 45, 14);

        Clone = GOUI.CreateSprite ("Textures/Other/Selection".ToString ());
        Clone.GetComponent<SpriteRenderer> ().color = new Color (0, 1, 0);
        GOUI.SetInPixScale (Clone, avatarSize + 30, avatarSize + 30);
        GOUI.SetInPixPosition (Clone, avatarPosition, 45, 12);
        Highlight = Clone;
        SetPlayerActive (false);

        Clone = GOUI.CreateSprite ("UI/White");
        GOUI.SetInPixScale (Clone, barLength, 30);
        GOUI.SetInPixPosition (Clone, barPosition, 25, 11);
        GOUI.SetSpriteColor (Clone, AppDefaults.PlayerColor [playerNumber + 1] * 0.5f);

        Clone = GOUI.CreateSprite ("UI/White");
        GOUI.SetSpriteColor (Clone, AppDefaults.PlayerColor [playerNumber + 1]);
        HealthBar = Clone;

        Clone = GOUI.CreateText ("", barPosition, 25, 13, 0.025f);
        Clone.GetComponent<TextMesh> ().color = Color.black;
        ScoreText = Clone;

        Clone = GOUI.CreateText ("", barPosition, 55, 13, 0.025f);
        Clone.GetComponent<TextMesh> ().color = Color.black;
        Clone.GetComponent<TextMesh> ().text = player.properties.displayName;
        UserNameText = Clone;
    }

    public void DelayedSetActivePlayer (bool active) {
        VisualMatch.instance.SetPlayerActive (this, active);
    }

    public void SetPlayerActive (bool active) {
        Highlight.GetComponent<SpriteRenderer> ().enabled = active;
    }

    public void UpdateVisuals (MatchClass match) {
        DelayedSetPlayerHealthBar (match, player);
    }

    public void SetPlayerHealthBar (MatchClass match, PlayerClass player) {
        SetPlayerHealthBar (player.score, player.scoreIncome, match.Properties.scoreLimit);
    }

    public void DelayedSetPlayerHealthBar (MatchClass match, PlayerClass player) {
        DelayedSetPlayerHealthBar (player.score, player.scoreIncome, match.Properties.scoreLimit);
    }

    public void SetPlayerHealthBar (int score, int scoreIncome, int scoreLimit) {
        /*int avatarPosition = margin + avatarSize / 2 + shiftLength * player.playerNumber;
        int barPosition = margin + barLength / 2 + shiftLength * player.playerNumber;
        if (ally) {
            barPosition += avatarSize;
        } else {
            avatarPosition += barLength;
        }*/

        float percentage = Mathf.Min (1f * score / scoreLimit, 1);
        GOUI.SetInPixScale (HealthBar, (int) (percentage * barLength), 30);
        if (ally) {
            GOUI.SetInPixPosition (HealthBar, barPosition - (int) (barLength / 2 * (1 - percentage)), 25, 12);
        } else {
            GOUI.SetInPixPosition (HealthBar, barPosition + (int) ((barLength * (1 - percentage) + 1) / 2), 25, 12);
        }
        ScoreText.GetComponent<TextMesh> ().text = score.ToString () + " (+" + scoreIncome.ToString () + ")";
    }

    public void DelayedSetPlayerHealthBar (int score, int scoreIncome, int scoreLimit) {
        VisualMatch.instance.SetPlayerHealthBar (this, score, scoreIncome, scoreLimit);
    }

}
