using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRatingMenu : GOUI {

    static int sortMode;
    static int count;
    static int gameModeId;
    static GameObject [] Card;
    static GameObject [] Text;


    void Awake () {
        CreateAIRatingMenu ();
        CurrentGOUI = this;
    }

    private void Update () {
        if (Input.GetKeyDown ("r")) {
            ShowAIRatingMenuMenu (gameModeId);
            RefreshPage ();
        }
        if (Input.GetKeyDown ("s")) {
            sortMode = (sortMode + 1) % 2;
            RefreshPage ();
        }
    }

    static public void ShowAIRatingMenuMenu (int gameModeId) {
        AIRatingMenu.gameModeId = gameModeId;
        DestroyMenu ();
        CurrentCanvas.AddComponent<AIRatingMenu> ();
    }

    static public void CreateAIRatingMenu () {
        VisualCard card;
        CardPoolClass cardPool = new CardPoolClass ();
        cardPool.LoadFromFile (gameModeId);

        count = cardPool.Card.Count;
        Card = new GameObject [count];
        Text = new GameObject [count];

        for (int z = 0; z < count; z++) {
            card = new VisualCard (cardPool.Card [z]);
            Card [z] = card.Anchor;
            Card [z].transform.localPosition = new Vector3 (1000, 1000, 1000);
            
            card.Anchor.transform.SetParent (GOUI.CurrentCanvas.transform);
            card.Anchor.transform.localScale = Vector3.one * 0.12f;
            card.Anchor.transform.localEulerAngles = new Vector3 (-90, 0, 0);

            GameObject Clone = CreateText (z.ToString ());
            Clone.transform.localScale = Vector3.one * 0.022f;
            Clone.transform.SetParent (Card [z].transform);
            Clone.transform.localPosition = new Vector3 (0, 0, -1.1f);
            Text [z] = Clone;

        }

    }

    static int rowCount = 22;

    static public void RefreshPage () {
        switch (sortMode) {
            case 0:
                break;
        }
        if (sortMode == 0) {
            List<int> Popularity = new List<int> ();
            for (int x = 0; x < count; x++) {
                Popularity.Add (x);
            }
            Popularity.Sort ((t1, t2) => RatingClass.cardPopularity [t1].CompareTo (RatingClass.cardPopularity [t2]));

            for (int y = 0; y < 9; y++) {
                for (int x = 0; x < rowCount; x++) {
                    int number = x + y * rowCount;
                    if (number >= count) {
                        break;
                    }

                    SetInPixPosition (Card [Popularity [number]], -150 + 85 * x, 80 + 120 * y, 12);
                    Text [number].GetComponent<TextMesh> ().text = number.ToString () + " (" + RatingClass.cardPopularity [number].ToString () + ")";
                }
            }

        } else {
            List<int> winRatio = new List<int> ();
            for (int x = 0; x < count; x++) {
                winRatio.Add (x);
            }
            winRatio.Sort ((t1, t2) => RatingClass.cardNumberWinRatio [t1].CompareTo (RatingClass.cardNumberWinRatio [t2]));

            for (int y = 0; y < 9; y++) {
                for (int x = 0; x < rowCount; x++) {
                    int number = x + y * rowCount;
                    if (number >= count) {
                        break;
                    }

                    SetInPixPosition (Card [winRatio [number]], -150 + 85 * x, 80 + 120 * y, 12);
                    Text [number].GetComponent<TextMesh> ().text = number.ToString () + " (" + RatingClass.cardNumberWinRatio [number].ToString () + ")";
                }
            }
        }
    }

}
