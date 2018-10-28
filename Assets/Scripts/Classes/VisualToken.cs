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
        Anchor.transform.parent = tokenClass.tile.visualTile.Anchor.transform;
        Anchor.transform.localPosition = new Vector3 (0, 0.4f, 0);
        SetState ();
    }

    public void DestroyToken () {
        GameObject.Destroy (Anchor);
    }

    public void SetState () {
        SetOwner ();
        SetValue ();
        SetType ();
    }

    public void SetOwner () {
        Base.GetComponent<VisualEffectScript> ().Color = AppDefaults.PlayerColor [tokenClass.owner];
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

    public void CreateToken () {
        Anchor = new GameObject ();

        Base = GameObject.Instantiate (Resources.Load ("Prefabs/TokenBase")) as GameObject;
        Base.AddComponent<VisualEffectScript> ().Init (new Color (0.8f, 0.8f, 0.8f), false, true);
        Base.transform.localScale = new Vector3 (0.72f, 0.05f, 0.72f);
        Base.transform.parent = Anchor.transform;

        Border = GameObject.Instantiate (Resources.Load ("Prefabs/TokenBorder")) as GameObject;
        Border.AddComponent<VisualEffectScript> ().Init (new Color (0.1f, 0.1f, 0.1f), false, true);
        Border.transform.localScale = new Vector3 (0.8f, 0.1f, 0.8f);
        Border.transform.parent = Anchor.transform;

        Text = GameObject.Instantiate (Resources.Load ("Prefabs/PreText")) as GameObject;
        Text.GetComponent<TextMesh> ().text = Random.Range (1, 4).ToString ();
        //Text.GetComponent<Renderer> ().material.color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f));
        Text.transform.localScale = new Vector3 (0.4f, 0.4f, 0.4f);
        Text.transform.parent = Anchor.transform;
        Text.transform.localPosition = new Vector3 (0, 0.05f, 0);
        Text.transform.localEulerAngles = new Vector3 (90, 0, 0);

        Anchor.AddComponent<VisualEffectScript> ();
        Anchor.GetComponent<VisualEffectScript> ().Init (Color.white, false, true);
        Anchor.GetComponent<VisualEffectScript> ().Colored = false;
        Anchor.GetComponent<VisualEffectScript> ().GrowAppear [0] = true;
    }

    public void SetType (int type) {
        Border.GetComponent<VisualEffectScript> ().Color = AppDefaults.GetBorderColorMain (type);
        Type = type;
    }

}
