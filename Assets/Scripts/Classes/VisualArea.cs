using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualArea {

    public GameObject Anchor;
    GameObject [] Tile = new GameObject [6];

    public VisualArea () {
        Anchor = new GameObject ();

        for (int x = 0; x < 6; x++) {
            Tile [x] = GameObject.Instantiate (AppDefaults.Tile) as GameObject;
            Tile [x].transform.localScale = new Vector3 (0.35f, 0.1f, 0.35f);
            //Tiles [x].transform.parent = Background.transform;
        }

        Tile [0].transform.localPosition = new Vector3 (-0.4f, 0, 0);
        Tile [1].transform.localPosition = new Vector3 (-0.2f, 0, 0.35f);
        Tile [2].transform.localPosition = new Vector3 (0.2f, 0, 0.35f);
        Tile [3].transform.localPosition = new Vector3 (0.4f, 0, 0);
        Tile [4].transform.localPosition = new Vector3 (0.2f, 0, -0.35f);
        Tile [5].transform.localPosition = new Vector3 (-0.2f, 0, -0.35f);
        for (int x = 0; x < 6; x++) {

            Tile [x].transform.parent = Anchor.transform;
        }
    }

    public void SetAbilityArea (int x) {
        DisableAbilityArea ();
        if (x > 0  && x <= 3) {
            Tile [x - 1].GetComponent<VisualEffectScript> ().Init (new Color (1, 1, 1), false, true);
            Tile [x + 3 - 1].GetComponent<VisualEffectScript> ().Init (new Color (1, 1, 1), false, true);
        } else if (x == 4) {
            for (int y = 0; y < 6; y++) {
                Tile [y].GetComponent<VisualEffectScript> ().Init (new Color (1, 1, 1), false, true);
            }
        }
    }
    public void DisableAbilityArea () {
        for (int y = 0; y < 6; y++) {
            if (Tile [y].GetComponent<VisualEffectScript> () == null) {
                Tile [y].AddComponent<VisualEffectScript> ().Init (new Color (0.3f, 0.3f, 0.3f), false, true);
            } else {
                Tile [y].GetComponent<VisualEffectScript> ().Color = new Color (0.3f, 0.3f, 0.3f);
            }
        }
    }
}
