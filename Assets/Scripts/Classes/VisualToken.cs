using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualToken {

    public GameObject Anchor;
    public GameObject Base;
    public GameObject Border;
    public GameObject Text;

    int Type;

    public VisualToken () {
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
    }

    public void SetTokenType (int type) {
        switch (type) {
            case 1:
                Border.GetComponent<VisualEffectScript> ().Color = new Color (0.8f, 0.6f, 0.1f);
                break;
            case 2:
                Border.GetComponent<VisualEffectScript> ().Color = new Color (0.0f, 0.2f, 0.0f);
                break;
            case 3:
                Border.GetComponent<VisualEffectScript> ().Color = new Color (0.0f, 0.2f, 0.6f);
                break;
            case 4:
                Border.GetComponent<VisualEffectScript> ().Color = new Color (0.0f, 0.2f, 0.6f);
                break;
            case 5:
                Border.GetComponent<VisualEffectScript> ().Color = new Color (0.0f, 0.6f, 0.2f);
                break;
            default:
                Border.GetComponent<VisualEffectScript> ().Color = Color.black;
                break;
        }
        Type = type;
    }

}
