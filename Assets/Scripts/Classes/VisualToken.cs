using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualToken {

    public TokenClass tokenClass;

    public GameObject Anchor;
    public GameObject Base;
    public GameObject Border;
    public GameObject Text;

    int Type;

    public VisualToken () {
        CreateToken ();
    }

    public VisualToken (TokenClass tokenClass) {
        this.tokenClass = tokenClass;
        CreateToken ();
        SetParent (tokenClass.tile.visualTile.Anchor);
        SetState ();
    }

    public void SetParent (GameObject parent) {
        Anchor.transform.parent = parent.transform;
        Anchor.transform.localPosition = new Vector3 (0, 0.4f, 0);
    }

    public void DestroyToken () {
        GameObject.Destroy (Anchor);
    }

    public void SetState (int owner, int type, int value) {
        SetOwner (owner);
        SetValue (value);
        SetType (type);
    }

    public void SetState () {
        SetOwner ();
        SetValue ();
        SetType ();
    }

    public void SetOwner () {
        SetOwner (tokenClass.owner);
    }

    public void SetOwner (int owner) {
        Base.GetComponent<VisualEffectScript> ().SetColor (AppDefaults.PlayerColor [owner]);
    }

    public void SetValue () {
        SetValue (tokenClass.value);
    }

    public void SetValue (int value) {
        Text.GetComponent<TextMesh> ().text = value.ToString ();
    }

    public void SetType () {
        SetType (tokenClass.type);
    }
    public void SetType (int type) {
        Border.GetComponent<VisualEffectScript> ().SetColor (AppDefaults.GetBorderColorMain (type));
        Type = type;
    }

    public void CreateToken () {
        Anchor = new GameObject ();

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

    public void AddCreateAnimation () {
        Anchor.GetComponent<VisualEffectScript> ().SetScale (new Vector3 [2] {
        new Vector3 (0, 0, 0), new Vector3 (1, 1, 1) });
    }

    public void AddPlayAnimation () {
        VisualEffectScript VEScript = Anchor.GetComponent<VisualEffectScript> ();

        VEScript.AddPhase ();
        float height = 2;
        VEScript.SetDeltaPosition (new Vector3 [3] {
        new Vector3 (0, height, 0), new Vector3 (0, height, 0), new Vector3 (0, 0, 0) });
    }

    public void AddLerpAnimation () {
        Anchor.GetComponent<VisualEffectScript> ().lerpPosition = true;
    }


}
