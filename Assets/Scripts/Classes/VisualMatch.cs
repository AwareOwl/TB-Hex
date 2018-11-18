using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualMatch {

    MatchClass match;

    public VisualMatch () {

    }

    public void EnableVisual () {
        //usedCardPreview = GOUI.CurrentCanvas.AddComponent<UsedCardPreview> ();
    }

    public void PlayCard (int playerNumber, CardClass card) {
        new GameObject().AddComponent <UsedCardPreview> ().Init (playerNumber, card);
    }
}
