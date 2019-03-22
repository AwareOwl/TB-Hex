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
            Tile [x].transform.localScale = new Vector3 (0.35f, 0.05f, 0.35f);
            Tile [x].transform.Find ("Tile").GetComponent<Renderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            //Tiles [x].transform.parent = Background.transform;
        }

        float a = 0.38f;
        float b = a / 2;
        float c = b * Mathf.Sqrt (3);

        Tile [0].transform.localPosition = new Vector3 (-a, 0, 0);
        Tile [1].transform.localPosition = new Vector3 (-b, 0, c);
        Tile [2].transform.localPosition = new Vector3 (b, 0, c);
        Tile [3].transform.localPosition = new Vector3 (a, 0, 0);
        Tile [4].transform.localPosition = new Vector3 (b, 0, -c);
        Tile [5].transform.localPosition = new Vector3 (-b, 0, -c);
        for (int x = 0; x < 6; x++) {

            Tile [x].transform.parent = Anchor.transform;
        }
    }

    public void SetAbilityArea (int x) {
        DisableAbilityArea ();
        if (x > 0  && x <= 3) {
            Tile [x - 1].GetComponent<VisualEffectScript> ().SetColor (new Color (1, 1, 1));
            Tile [x + 3 - 1].GetComponent<VisualEffectScript> ().SetColor (new Color (1, 1, 1));
        } else if (x == 4) {
            for (int y = 0; y < 6; y++) {
                Tile [y].GetComponent<VisualEffectScript> ().SetColor (new Color (1, 1, 1));
            }
        }
    }
    public void DisableAbilityArea () {
        for (int y = 0; y < 6; y++) {
            if (Tile [y].GetComponent<VisualEffectScript> () == null) {
                Tile [y].AddComponent<VisualEffectScript> ().SetColor (new Color (0.3f, 0.3f, 0.3f));
            } else {
                Tile [y].GetComponent<VisualEffectScript> ().SetColor (new Color (0.3f, 0.3f, 0.3f));
            }
        }
    }
}
