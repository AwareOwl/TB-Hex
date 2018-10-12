using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualCard {

    GameObject Background;
    GameObject [] Tile = new GameObject [6];
    GameObject ManaTile;
    GameObject AbilityTile;
    GameObject AbilityIcon;
    VisualToken Token;

    public VisualCard () {
        GameObject Clone = NewCard ();
    }

    static public void Make4Cards () {
        for (int x = 0; x < 4; x++) {
            for (int y = 0; y < 4; y++) {
                VisualCard card = new VisualCard ();
                card.Background.transform.localPosition = new Vector3 (-1.95f + x * 1.3f, 2 - 0.15f * y, -5.75f - 0.025f * y);
                card.Background.transform.localEulerAngles = new Vector3 (-25, 0, 0);
                card.SetAbilityArea (x);
                if (y == 0) {
                    card.SetAbilityIcon (x % 4 + 1);
                } else {
                    card.SetAbilityIcon (Random.Range (1, 5));
                }
                if (Random.Range (0, 3) == 0) {
                    //break;
                }
            }
        }
    }

    public void SetAbilityArea (int x) {
        for (int y = 0; y < 6; y++) {
            if (Tile [y].GetComponent<VisualEffectScript> () == null) {
                Tile [y].AddComponent<VisualEffectScript> ().Init (new Color (0.3f, 0.3f, 0.3f), false, true);
            }
        }
        if (x < 3) {
            Tile [x].GetComponent<VisualEffectScript> ().Init (new Color (1, 1, 1), false, true);
            Tile [x + 3].GetComponent<VisualEffectScript> ().Init (new Color (1, 1, 1), false, true);
        } else {
            for (int y = 0; y < 6; y++) {
                Tile [y].GetComponent<VisualEffectScript> ().Init (new Color (1, 1, 1), false, true);
            }
        }
    }

    public void SetAbilityIcon (int x) {
        AbilityIcon.GetComponent<Renderer> ().material.mainTexture = Resources.Load ("Textures/Ability/Ability0" + x.ToString ()) as Texture;
        /*switch (x) {
            case 1:
                AbilityIcon.GetComponent<Renderer> ().material.color = Color.red;
                break;
            case 2:
                AbilityIcon.GetComponent<Renderer> ().material.color = new Color (0, 0.95f, 0);
                break;
            case 3:
                AbilityIcon.GetComponent<Renderer> ().material.color = new Color (1, 0.5f, 0);
                break;
            case 4:
                AbilityIcon.GetComponent<Renderer> ().material.color = new Color (1, 1, 0);
                break;
        }*/
        AbilityIcon.GetComponent<Renderer> ().material.color = new Color (0, 0, 0);
        switch (x) {
            case 1:
                ManaTile.GetComponent<VisualEffectScript> ().Init (Color.red, false, true);
                break;
            case 2:
                ManaTile.GetComponent<VisualEffectScript> ().Init (new Color (0, 0.95f, 0), false, true);
                break;
            case 3:
                ManaTile.GetComponent<VisualEffectScript> ().Init (new Color (1, 0.5f, 0), false, true);
                break;
            case 4:
                ManaTile.GetComponent<VisualEffectScript> ().Init (new Color (1, 1, 0), false, true);
                break;
        }
    }

    GameObject NewCard () {
        Background = GameObject.CreatePrimitive (PrimitiveType.Cube);
        Background.transform.localScale = new Vector3 (1.2f, 0.05f, 1.4f);
        Background.transform.localPosition = new Vector3 (0, 0, -0.2f);
        Background.GetComponent<Renderer> ().material.color = Color.black;
        Background.name = "Card";

        for (int x = 0; x < 6; x++) {
            Tile [x] = GameObject.Instantiate (AppDefaults.Tile) as GameObject;
            Tile [x].transform.localScale = new Vector3 (0.35f, 0.1f, 0.35f);
            //Tiles [x].transform.parent = Background.transform;
        }

        Tile [0].transform.localPosition = new Vector3 (-0.4f, 0.05f, -0.3f);
        Tile [1].transform.localPosition = new Vector3 (-0.2f, 0.05f, 0.05f);
        Tile [2].transform.localPosition = new Vector3 (0.2f, 0.05f, 0.05f);
        Tile [3].transform.localPosition = new Vector3 (0.4f, 0.05f, -0.3f);
        Tile [4].transform.localPosition = new Vector3 (0.2f, 0.05f, -0.65f);
        Tile [5].transform.localPosition = new Vector3 (-0.2f, 0.05f, -0.65f);
        for (int x = 0; x < 6; x++) {

            /*Token = new VisualToken ();
            Token.Base.transform.localPosition = Tile [x].transform.localPosition + new Vector3 (0, 0.05f, 0);
            Token.Base.transform.parent = Background.transform;*/

            Tile [x].transform.parent = Background.transform;
        }


        /*ManaTile = GameObject.Instantiate (Resources.Load ("Prefabs/Hex")) as GameObject;
        ManaTile.transform.localScale = new Vector3 (0.5f, 0.35f, 0.5f);
        ManaTile.transform.localPosition = new Vector3 (-0.375f, 0.1f, 0.6f);
        ManaTile.transform.localEulerAngles = new Vector3 (0, 30, 0);
        ManaTile.transform.parent = Background.transform;*/

        ManaTile = GameObject.Instantiate (AppDefaults.Tile) as GameObject;
        ManaTile.AddComponent<VisualEffectScript> ().Init (new Color (0, 0, 0), false, true);
        ManaTile.transform.localScale = new Vector3 (0.5f, 0.1f, 0.5f);
        ManaTile.transform.localPosition = new Vector3 (0.35f, 0.05f, 0.5f);
        ManaTile.transform.localEulerAngles = new Vector3 (0, 90, 0);
        ManaTile.transform.parent = Background.transform;


        AbilityIcon = GameObject.CreatePrimitive (PrimitiveType.Quad);
        AbilityIcon.GetComponent<Renderer> ().material.shader = Shader.Find ("Sprites/Default");
        AbilityIcon.transform.localEulerAngles = new Vector3 (60, 0, 0);
        AbilityIcon.transform.localPosition = ManaTile.transform.position + new Vector3 (0, 0.05f, 0);
        AbilityIcon.transform.SetParent (Background.transform, true);
        AbilityIcon.transform.localScale = new Vector3 (0.4f, 0.4f, 0.4f);

        Token = new VisualToken ();
        Token.Anchor.transform.localPosition = new Vector3 (0, 0.05f, -0.3f);
        Token.Anchor.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
        Token.Anchor.transform.parent = Background.transform;

        Background.transform.localScale *= 0.8f;

        return Background;
    }
}
