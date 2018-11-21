using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualToken {

    public TokenClass token;

    public GameObject Anchor;
    public GameObject Base;
    public GameObject Border;
    public GameObject Text;

    int Type;

    public VisualToken () {
        CreateToken ();
    }

    public VisualToken (TokenClass tokenClass) {
        this.token = tokenClass;
        DelayedInit ();
    }

    public void DelayedInit () {
        Anchor = new GameObject ();
        if (VisualMatch.instance != null) {
            VisualMatch.instance.Init (this, token.tile.visualTile.Anchor,
                token.owner,
                token.type,
                token.value);
        } else {
            Init (token.tile.visualTile.Anchor,
                token.owner,
                token.type,
                token.value);
        }
    }

    public void Init (GameObject parent, int owner, int type, int value) {
        CreateToken ();
        SetParent (parent);
        SetState (owner, type, value);
    }

    public void SetParent (GameObject parent) {
        Anchor.transform.parent = parent.transform;
        Anchor.transform.localPosition = new Vector3 (0, 0.4f, 0);
    }

    public void DestroyToken (GameObject anchor) {
        //if (anchor != null) {
            VisualEffectScript VEScript = anchor.AddComponent<VisualEffectScript> ();
            VEScript.SetScale (new Vector3 [] { new Vector3 (1, 1, 1), new Vector3 (0, 0, 0) });
            VEScript.SetPhaseTimer (0.5f);
            VEScript.destroyOnEnd = true;
        //}
        //GameObject.Destroy (anchor);
    }

    public void DelayedDestroyToken () {
        if (VisualMatch.instance != null) {
            VisualMatch.instance.DestroyToken (this, Anchor);
        } else {
            DestroyToken (Anchor);
        }
    }

    public void SetState (int owner, int type, int value) {
        SetOwner (owner);
        SetValue (value);
        SetType (type);
    }
    /*
    public void SetState () {
        SetOwner ();
        SetValue ();
        SetType ();
    }*/

    public void DelayedSetState () {
        if (VisualMatch.instance != null) {
            VisualMatch.instance.SetState (this, token.owner, token.type, token.value);
        } else {
            SetState (token.owner, token.type, token.value);
        }
    }

    public void SetOwner () {
        SetOwner (token.owner);
    }

    public void SetOwner (int owner) {
        Base.GetComponent<VisualEffectScript> ().SetColor (AppDefaults.PlayerColor [owner]);
    }

    public void SetValue () {
        SetValue (token.value);
    }

    public void SetValue (int value) {
        Text.GetComponent<TextMesh> ().text = value.ToString ();
    }

    public void SetType () {
        SetType (token.type);
    }
    public void SetType (int type) {
        Border.GetComponent<VisualEffectScript> ().SetColor (AppDefaults.GetBorderColorMain (type));
        Type = type;
    }

    public void CreateToken () {
        if (Anchor == null) {
            Anchor = new GameObject ();
        }

        Base = GameObject.Instantiate (Resources.Load ("Prefabs/TokenBase")) as GameObject;
        Base.AddComponent<VisualEffectScript> ().SetColor (new Color (0.8f, 0.8f, 0.8f));
        Base.transform.localScale = new Vector3 (0.72f, 0.05f, 0.72f);
        Base.transform.parent = Anchor.transform;

        Border = GameObject.Instantiate (Resources.Load ("Prefabs/TokenBorder")) as GameObject;
        Border.AddComponent<VisualEffectScript> ().SetColor (new Color (0.1f, 0.1f, 0.1f));
        Border.transform.localScale = new Vector3 (0.8f, 0.1f, 0.8f);
        Border.transform.parent = Anchor.transform;

        Text = GameObject.Instantiate (Resources.Load ("Prefabs/PreText")) as GameObject;
        Text.GetComponent<TextMesh> ().text = Random.Range (1, 4).ToString ();
        //Text.GetComponent<Renderer> ().material.color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f));
        Text.transform.localScale = new Vector3 (0.4f, 0.4f, 0.4f);
        Text.transform.parent = Anchor.transform;
        Text.transform.localPosition = new Vector3 (0, 0.05f, 0);
        Text.transform.localEulerAngles = new Vector3 (90, 0, 0);

        Anchor.name = "AToken";
        Anchor.AddComponent<VisualEffectScript> ().SetPhaseTimer (0.5f);
    }

    public void DelayedCreateToken () {
        VisualMatch.instance.CreateToken (this);
    }

    public void AddCreateAnimation () {
        Anchor.GetComponent<VisualEffectScript> ().SetScale (new Vector3 [2] {
        new Vector3 (0, 0, 0), new Vector3 (1, 1, 1) });
    }

    public void DelayedAddCreateAnimation () {
        if (VisualMatch.instance != null) {
            VisualMatch.instance.AddCreateAnimation (this);
        } else {
            AddCreateAnimation ();
        }
    }

    public void AddPlayAnimation () {
        VisualEffectScript VEScript = Anchor.GetComponent<VisualEffectScript> ();

        VEScript.AddPhase ();
        float height = 2;
        VEScript.SetDeltaPosition (new Vector3 [3] {
        new Vector3 (0, height, 0), new Vector3 (0, height, 0), new Vector3 (0, 0, 0) });
    }

    public void DelayedAddPlayAnimation () {
        VisualMatch.instance.AddPlayAnimation (this);
    }

    public void SetTile (GameObject tile) {
        Anchor.transform.SetParent (tile.transform);
        Anchor.GetComponent<VisualEffectScript> ().SetPosition (new Vector3 (0, 0.4f, 0));
        Anchor.GetComponent<VisualEffectScript> ().SetLerpPosition (true);
    }

    public void DelayedSetTile (GameObject tile) {
        VisualMatch.instance.SetTile (this, tile);
    }

    public void MoveToDisabledTile (int x, int y) {
        VisualEffectScript VEScript = Anchor.GetComponent<VisualEffectScript> ();
        GameObject tile = EnvironmentScript.BackgroundTiles [x + 1, y + 1];
        VEScript.AddPhase ();
        Anchor.transform.SetParent (tile.transform);
        float deltaPosition = Mathf.Abs (tile.transform.position.y / tile.transform.lossyScale.y);
        VEScript.SetPosition (new Vector3 (0, 0.4f + deltaPosition, 0));
        VEScript.SetLerpPosition (true);
        //VEScript.basicPosition [VEScript.endPhase] = VisualTile.TilePosition (x, 0.4f, y) - token.tile.visualTile.Anchor.transform.position;
        VEScript.AddPhase ();
        VEScript.basicPosition [VEScript.endPhase] = new Vector3 (0, 0.4f, 0);
        VEScript.lerpPosition [VEScript.endPhase - 1] = false;
        VEScript.SetPhaseTimer (VEScript.endPhase, deltaPosition * 2);
    }

    public void DelayedMoveToDisabledTile (int x, int y) {
        VisualMatch.instance.MoveToDisabledTile (this, x, y);
    }


}
