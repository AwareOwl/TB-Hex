﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualCard {

    public Collider collider;
    CardClass card;
    public GameObject Anchor;
    public GameObject Background;
    GameObject AbilityTile;
    GameObject AbilityIcon;
    GameObject Highlight;
    VisualToken Token;
    VisualArea Area;

    public VisualCard () {
        NewCard ();
    }

    public VisualCard (CardClass card) {
        this.card = card;
        NewCard ();
        SetState (card);
    }

    public void DestroyCardVisual () {
        VisualEffectScript VES = Anchor.AddComponent<VisualEffectScript> ();
        VES.destroyOnEnd = true;
        VES.SetLastScale (Anchor.transform.localScale);
        VES.AddPhase ();
        VES.SetLastScale (Vector3.zero);
        VES.SetLastPhaseTimer (0.25f);
    }

    public void DelayedSetState (int tokenValue, TokenType tokenType, int abilityArea, AbilityType abilityType) {
        VisualMatch.instance.SetState (this, tokenValue, tokenType, abilityArea, abilityType);
    }


    public void SetState (CardClass card) {
        SetState (card.tokenValue, card.tokenType, card.abilityArea, card.abilityType);
    }

    public void SetState (int value, TokenType tokenType, int abilityArea, AbilityType abilityType) {
        Token.SetType (tokenType);
        Token.SetValue (value);
        Area.SetAbilityArea (abilityArea);
        SetAbilityIcon (abilityType);
        Renderer [] renderes = Anchor.GetComponentsInChildren<Renderer> ();
        foreach (Renderer renderer in renderes) {
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
    }

    public void DestroyVisual () {
        GameObject.DestroyImmediate (Anchor);
    }

    public void SetAbilityIcon (AbilityType abilityType) {
        switch (abilityType) {
            case AbilityType.T0:
            case AbilityType.T11:
            case AbilityType.T22:
            case AbilityType.T38:
            case AbilityType.T42:
                Area.DisableAbilityArea ();
                break;
        }
        if (abilityType == 0) {
            EnableAbilityTile (false);
        } else {
            EnableAbilityTile (true);
        }
        SetAbilityIconInObject (AbilityIcon, abilityType);
        AbilityTile.GetComponent<VisualEffectScript> ().SetColor (AppDefaults.GetAbilityColor (abilityType));
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

    static public string GetIconPath (AbilityType abilityType) {
        return GetIconPath ((int) abilityType);
    }

    static public string GetIconPath (int type) {
        if (type < 10) {
            return "Textures/Ability/Ability0" + type.ToString ();
        } else {
            return "Textures/Ability/Ability" + type.ToString ();
        }
    }

    static public void SetAbilityIconInObject (GameObject icon, AbilityType type) {
        Texture2D abilityTex = Resources.Load (GetIconPath (type)) as Texture2D;
        if (abilityTex == null) {
            icon.GetComponent<Renderer> ().enabled = false;
            return;
        }
        icon.GetComponent<Renderer> ().enabled = true;
        icon.GetComponent<Renderer> ().material.mainTexture = abilityTex;
        switch (type) {
           
            default:
                icon.GetComponent<Renderer> ().material.color = new Color (0, 0, 0);
                break;
        }
    }

    public void EnableHighlight () {
        if (Highlight == null) {
            GameObject Clone = GOUI.CreateBackgroundSprite ("Textures/Other/Selection");
            Clone.transform.SetParent (Background.transform, true);
            Clone.transform.localPosition = new Vector3 (0, 0, 0);
            Clone.transform.localEulerAngles = new Vector3 (-90, 0, 0);
            Clone.GetComponent<Renderer> ().sortingOrder = 20;
            Clone.GetComponent<SpriteRenderer> ().color = Color.green;
            Clone.GetComponent<SpriteRenderer> ().size = new Vector2 (1, 1);
            Clone.transform.localScale = new Vector3 (1.4f, 1.4f, 1.4f);
            Highlight = Clone;

            /*GameObject Clone = GameObject.CreatePrimitive (PrimitiveType.Quad);
            GameObject.Destroy (AbilityIcon.GetComponent<Collider> ());
            Renderer renderer = Clone.GetComponent<Renderer> ();
            renderer.material.shader = Shader.Find ("Sprites/Default");
            renderer.material.mainTexture = Resources.Load ("Textures/Other/Selection") as Texture;
            renderer.material.color = Color.green;
            Clone.transform.SetParent (Background.transform, true);
            Clone.transform.localPosition = new Vector3 (0, 0, 0);
            Clone.transform.localEulerAngles = new Vector3 (-90, 0, 0);
            Clone.transform.localScale = new Vector3 (1.4f, 1.4f, 1.4f);
            Highlight = Clone;*/
        }
    }

    public void DisableHighlight () {
        GameObject.DestroyImmediate (Highlight);
    }

    GameObject NewCard () {
        Anchor = new GameObject ();

        Background = GameObject.CreatePrimitive (PrimitiveType.Cube);
        Background.transform.SetParent (Anchor.transform);
        Background.transform.localScale = new Vector3 (1.2f, 0.05f, 1.4f);
        Background.transform.localPosition = new Vector3 (0, 0, -0.2f);
        Background.GetComponent<Renderer> ().material.color = Color.black;
        Background.AddComponent<UIController> ().card = card;
        collider = Background.GetComponent<Collider> ();
        Background.name = "Card";

        Area = new VisualArea ();
        Area.Anchor.transform.SetParent (Anchor.transform);
        Area.Anchor.transform.localPosition = new Vector3 (0, 0.05f, -0.3f);

        AbilityTile = GameObject.Instantiate (AppDefaults.Tile) as GameObject;
        AbilityTile.AddComponent<VisualEffectScript> ().SetColor (new Color (0, 0, 0));
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

        Renderer [] renderes = Anchor.GetComponentsInChildren<Renderer> ();
        foreach (Renderer renderer in renderes) {
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }

        Anchor.transform.localScale *= 0.8f;

        return Anchor;
    }
}
