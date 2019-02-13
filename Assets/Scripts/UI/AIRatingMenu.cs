using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRatingMenu : GOUI {

    static int count;
    static int gameModeId;
    static GameObject [] Card;
    static GameObject [] Text;


    void Awake () {
        CreateAIRatingMenu ();
        CurrentGUI = this;
    }

    private void Update () {
        if (Input.GetKeyDown ("r")) {
            ShowAIRatingMenuMenu (gameModeId);
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

        for (int z = 0; z < count; z++) {
            card = new VisualCard (cardPool.Card [z]);
            Card [z] = card.Anchor;
            Card [z].transform.localPosition = new Vector3 (1000, 1000, 1000);
            
            card.Anchor.transform.SetParent (GOUI.CurrentCanvas.transform);
            card.Anchor.transform.localScale = Vector3.one * 0.14f;
            card.Anchor.transform.localEulerAngles = new Vector3 (-90, 0, 0);

            GameObject Clone = CreateText (z.ToString ());
            Clone.transform.localScale = Vector3.one * 0.03f;
            Clone.transform.SetParent (Card [z].transform);
            Clone.transform.localPosition = new Vector3 (0, 0, -1.1f);

        }

    }

    static public void RefreshPage () {
        List<int> Popularity = new List<int> ();
        for (int x = 0; x < count; x++) {
            Popularity.Add (x);
        }
        Popularity.Sort ((t1, t2) => RatingClass.cardPopularity [t1].CompareTo (RatingClass.cardPopularity [t2]));

        for (int y = 0; y < 8; y++) {
            for (int x = 0; x < 14; x++) {
                int number = x + y * 12;
                if (number >= count) {
                    break;
                }

                SetInPixPosition (Card [Popularity [number]], 50 + 110 * x, 80 + 145 * y, 12);
            }
        }
    }

}
