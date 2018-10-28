using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : GOUI {

    static public int NumberOfPlayers = 2;

    static public void ShowInGameUI () {
        CreatePlayersUI ();
    }
    
    static public void CreatePlayersUI () {
        for (int x = 0; x < NumberOfPlayers; x++) {
            CreatePlayerUI (x, x < 1);
        }
    }

    static public void CreatePlayerUI (int playerNumber, bool ally) {
        GameObject Clone;
        int margin = 60;
        int avatarSize = 90;
        int shiftLength = (1440 - margin * 2) / NumberOfPlayers;
        int barLength = shiftLength - avatarSize - 30;
        int avatarPosition = margin + avatarSize / 2 + shiftLength * playerNumber;
        int barPosition = margin + barLength / 2 + shiftLength * playerNumber;
        if (ally) {
            barPosition += avatarSize;
        } else {
            avatarPosition += barLength;
        }
        Clone = CreateSprite ("Textures/Avatars/Avatar0" + ((playerNumber%4) + 2).ToString ());
        SetInPixScale (Clone, avatarSize, avatarSize);
        SetInPixPosition (Clone, avatarPosition, 45, 14);

        Clone = CreateSprite ("UI/White");
        SetInPixScale (Clone, barLength, 30);
        SetInPixPosition (Clone, barPosition, 15, 11);

        SetSpriteColor (Clone, AppDefaults.PlayerColor [playerNumber] * 0.5f);
        float percentage = Random.Range (0f, 1f);
        Clone = CreateSprite ("UI/White");
        SetInPixScale (Clone, (int) (percentage * barLength), 30);
        if (ally) {
            SetInPixPosition (Clone, barPosition - (int) (barLength / 2 * (1 - percentage)), 15, 12);
        } else {
            SetInPixPosition (Clone, barPosition + (int) ((barLength * (1 - percentage) + 1) / 2), 15, 12);
        }
        SetSpriteColor (Clone, AppDefaults.PlayerColor [playerNumber]);

        Clone = CreateText ();
        Clone.GetComponent<TextMesh> ().color = Color.black;
        Clone.transform.localScale = Vector3.one * 0.02f;
        SetInPixPosition (Clone, barPosition, 15, 13);
        Clone.GetComponent<TextMesh> ().text = ((int) (500 * percentage)).ToString () + " (+" + (Random.Range (0, 30).ToString ()) + ")";
    }
}
