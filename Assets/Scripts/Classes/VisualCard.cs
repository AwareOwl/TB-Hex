using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualCard {

    public GameObject Anchor;
    public GameObject Background;
    GameObject AbilityTile;
    GameObject AbilityIcon;
    VisualToken Token;
    VisualArea Area;

    public VisualCard () {
        NewCard ();
    }

    public VisualCard (CardClass card) {
        NewCard ();
        SetState (card);
    }

    static public void Make4Cards () {
        for (int x = 0; x < 4; x++) {
            for (int y = 0; y < 4; y++) {
                VisualCard card = new VisualCard ();
                card.Background.transform.localPosition = new Vector3 (-1.95f + x * 1.3f, 2 - 0.15f * y, -5.75f - 0.025f * y);
                card.Background.transform.localEulerAngles = new Vector3 (-25, 0, 0);
                card.Area.SetAbilityArea (x);
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

    public void SetState (CardClass card) {
        SetState (card.tokenType, card.value, card.abilityArea, card.abilityType);
    }

    public void SetState (int tokenType, int value, int abilityArea, int abilityType) {
        Token.SetType (tokenType);
        Token.SetValue (value);
        Area.SetAbilityArea (abilityArea);
        SetAbilityIcon (abilityType);
    }

    public void SetAbilityIcon (int type) {
        if (type == 0) {
            Area.DisableAbilityArea ();
            EnableAbilityTile (false);
        } else {
            EnableAbilityTile (true);
        }
        SetAbilityIconInObject (AbilityIcon, type);
        AbilityTile.GetComponent<VisualEffectScript> ().Init (AppDefaults.GetAbilityColor (type), false, true);
    }

    public void EnableAbilityTile (bool enable) {
        Renderer [] renderers = AbilityTile.GetComponentsInChildren<Renderer> ();
        foreach (Renderer renderer in renderers) {
            renderer.enabled = enable;
        }
        if (AbilityIcon.GetComponent<Renderer> ()) {
            AbilityIcon.GetComponent<Renderer> ().enabled = enable;
        }
    }

    static public string GetIconPath (int type) {
        if (type < 10) {
            return "Textures/Ability/Ability0" + type.ToString ();
        } else {
            return "Textures/Ability/Ability" + type.ToString ();
        }
    }

    static public void SetAbilityIconInObject (GameObject icon, int type) {
        Texture2D abilityTex = Resources.Load (GetIconPath (type)) as Texture2D;
        if (abilityTex == null) {
            icon.GetComponent<Renderer> ().enabled = false;
            return;
        }
        icon.GetComponent<Renderer> ().enabled = true;
        icon.GetComponent<Renderer> ().material.mainTexture = abilityTex;
        icon.GetComponent<Renderer> ().material.color = new Color (0, 0, 0);
    }

    GameObject NewCard () {
        Anchor = new GameObject ();

        Background = GameObject.CreatePrimitive (PrimitiveType.Cube);
        Background.transform.SetParent (Anchor.transform);
        Background.transform.localScale = new Vector3 (1.2f, 0.05f, 1.4f);
        Background.transform.localPosition = new Vector3 (0, 0, -0.2f);
        Background.GetComponent<Renderer> ().material.color = Color.black;
        Background.name = "Card";

        Area = new VisualArea ();
        Area.Anchor.transform.SetParent (Anchor.transform);
        Area.Anchor.transform.localPosition = new Vector3 (0, 0.05f, -0.3f);


        /*ManaTile = GameObject.Instantiate (Resources.Load ("Prefabs/Hex")) as GameObject;
        ManaTile.transform.localScale = new Vector3 (0.5f, 0.35f, 0.5f);
        ManaTile.transform.localPosition = new Vector3 (-0.375f, 0.1f, 0.6f);
        ManaTile.transform.localEulerAngles = new Vector3 (0, 30, 0);
        ManaTile.transform.parent = Background.transform;*/

        AbilityTile = GameObject.Instantiate (AppDefaults.Tile) as GameObject;
        AbilityTile.AddComponent<VisualEffectScript> ().Init (new Color (0, 0, 0), false, true);
        AbilityTile.transform.localScale = new Vector3 (0.5f, 0.1f, 0.5f);
        AbilityTile.transform.localPosition = new Vector3 (0.35f, 0.05f, 0.5f);
        AbilityTile.transform.localEulerAngles = new Vector3 (0, 90, 0);
        AbilityTile.transform.parent = Anchor.transform;


        AbilityIcon = GameObject.CreatePrimitive (PrimitiveType.Quad);
        GameObject.Destroy (AbilityIcon.GetComponent<Collider> ());
        AbilityIcon.GetComponent<Renderer> ().material.shader = Shader.Find ("Sprites/Default");
        AbilityIcon.transform.localPosition = AbilityTile.transform.position + new Vector3 (0, 0.03f, 0);
        AbilityIcon.transform.SetParent (Anchor.transform, true);
        AbilityIcon.transform.localEulerAngles = new Vector3 (90, 0, 0);
        AbilityIcon.transform.localScale = new Vector3 (0.45f, 0.45f, 0.45f);

        Token = new VisualToken ();
        Token.Anchor.transform.localPosition = new Vector3 (0, 0.05f, -0.3f);
        Token.Anchor.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
        Token.Anchor.transform.parent = Anchor.transform;

        Anchor.transform.localScale *= 0.8f;

        return Anchor;
    }
}
