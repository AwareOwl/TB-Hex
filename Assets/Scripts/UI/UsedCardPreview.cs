using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsedCardPreview : MonoBehaviour {

    static List<GameObject> [] ExistingPreviews = new List<GameObject> [5];

    int PlayerNumber;
    CardClass card;
    GameObject Anchor;
    GameObject Card;

    public bool destroy;

    public float timer = 0;
    float timerScale;
    public float destroyTime;

    static public void ClearEverything () {
        for (int x = 0; x < ExistingPreviews.Length; x++) {
            if (ExistingPreviews [x] != null) {
                for (int y = 0; y < ExistingPreviews [x].Count; y++) {
                    if (ExistingPreviews [x] [y] != null) {
                        DestroyImmediate (ExistingPreviews [x] [y]);
                    }
                }
            }
        }
    }


    public void Init (int playerNumber, CardClass card) {
        timerScale = AppSettings.AnimationDuration;
        destroyTime = timerScale + 10f * AppSettings.CardPreviewDuration;
        if (AppSettings.CardPreviewDuration >= 1) {
            destroyTime += 10000f;
        }
        PlayerNumber = playerNumber;
        this.card = card;
        if (ExistingPreviews [playerNumber] == null) {
            ExistingPreviews [playerNumber] = new List<GameObject> ();
        }
        ExistingPreviews [playerNumber].Add (gameObject);

        GameObject Clone = gameObject;

        Clone.transform.parent = GOUI.CurrentCanvas.transform;
        Clone.transform.localScale = new Vector3 (1, 1, 1);
        Clone.transform.localPosition = new Vector3 (
            (3.85f + Random.Range (-0.1f, 0.1f)) * (playerNumber * 2 - 3),
            2 - 0.15f * ExistingPreviews [playerNumber].Count,
            5 + 0.1f * ExistingPreviews [playerNumber].Count);
        Clone.transform.localEulerAngles = new Vector3 (-90f, 0, Random.Range (-2f, 2f));
        Anchor = Clone;

        VisualEffectScript VEScript = Clone.AddComponent<VisualEffectScript> ();
        VEScript.SetScale (new Vector3 [] { new Vector3 (0, 0, 0), new Vector3 (1, 1, 1) });
        VEScript.destroyThisOnEnd = true;

        RefreshPosition ();
    }

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime * ExistingPreviews [PlayerNumber].Count * ExistingPreviews [PlayerNumber].Count;
        RefreshPosition ();
        /*if (PlayerScript.CScript () == null && gameObject != null) {
            DestroyImmediate (gameObject);
        }*/
    }

    public void RefreshPosition () {
        for (int x = 0; x < ExistingPreviews [PlayerNumber].Count; x++) {
            if (ExistingPreviews [PlayerNumber] [x] == null) {
                ExistingPreviews [PlayerNumber].RemoveAt (x);
                x--;
            }
        }

        int pos = 0;
        for (int x = 0; x < ExistingPreviews [PlayerNumber].Count; x++) {
            if (gameObject == ExistingPreviews [PlayerNumber] [x]) {
                pos = x;
            }
        }

        Anchor.transform.localPosition = new Vector3 (
            Anchor.transform.localPosition.x,
            0.8f * Anchor.transform.localPosition.y + 0.2f * (2f - 0.15f * pos),
            5 + 0.1f * pos);



        if (destroy) {
            if (timer < destroyTime) {
                timer = destroyTime;
            }
        } else if (ExistingPreviews [PlayerNumber] [0] != gameObject) {
            if (timer > timerScale) {
                timer = timerScale;
            }
        }
        float gray = 0.6f;
        if (timer * 2 > timerScale && timer - destroyTime < 0) {
            if (Card == null) {
                GameObject Clone;
                Clone = new VisualCard (card).Anchor;
                Clone.transform.parent = Anchor.transform;
                Clone.transform.localScale = new Vector3 (1, 1, 1);
                Clone.transform.localPosition = new Vector3 (0, 0, 0);
                Clone.transform.localEulerAngles = new Vector3 (0, 0, 0);
                Card = Clone;
            }
        } else if (ExistingPreviews [PlayerNumber] [0] == gameObject || destroy) {
            if (timer - destroyTime >= 0 && timer - destroyTime < timerScale) {
                gray = 0.4f;
                if ((timer - destroyTime) * 2 >= timerScale) {
                    if (Card != null) {
                        if (Anchor.GetComponent<VisualEffectScript> () == null) {
                            VisualEffectScript VEScript = Anchor.AddComponent<VisualEffectScript> ();
                            VEScript.SetScale (new Vector3 [] { new Vector3 (1, 1, 1), new Vector3 (0, 0, 0) });
                            VEScript.destroyOnEnd = true;
                        }
                    }
                }
            } else if (timer >= destroyTime + timerScale) {
                ExistingPreviews [PlayerNumber].RemoveAt (pos);
                DestroyImmediate (gameObject);
            }
        }
    }
}
