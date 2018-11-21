﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : GOUI {

    static public List<bool> Bolds;
    static public List<Texture> Textures;
    static public List<string> Texts;


    static GameObject background;
    static GameObject pointer;

    static Vector3 anchor;
    static float height = 10;
    static float width = 10;

    public float timer = 0;
    static public float timerScale = 0.2f;


    static public List<GameObject> garbage = new List<GameObject> ();

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime;
        timer = Mathf.Min (timer, 1 * timerScale);
        UpdateTransparency ();
    }

    void UpdateTransparency () {
        foreach (GameObject obj in garbage) {
            SpriteRenderer sRenderer = obj.GetComponent<SpriteRenderer> ();
            if (sRenderer != null) {
                Color col = sRenderer.color;
                sRenderer.color = new Color (col.r, col.g, col.b, timer / timerScale);
            }
        }
    }

    static public void NewTooltip () {
        DestroyTooltip ();
        Bolds = new List<bool> ();
        Textures = new List<Texture> ();
        Texts = new List<string> ();
        garbage = new List<GameObject> ();
    }

    static public void NewTooltip (Transform transform, string s) {
        NewTooltip ();
        AddFragment (null, s, false);
        FinalizeTooltip (transform);
    }

    static public void NewTooltip (Transform transform, CardClass card) {
        NewTooltip ();
        AddFragment (null, Language.AbilityName [card.abilityType], true);
        AddFragment (null, Language.GetAbilityDescription (card.abilityType), false);
        AddFragment (null, "", false);
        AddFragment (null, Language.TokenName [card.tokenType], true);
        AddFragment (null, Language.TokenDescription [card.tokenType], false);
        FinalizeTooltip (transform);
    }

    static public void NewTooltip (Transform transform, TokenClass token) {
        NewTooltip ();
        AddFragment (null, Language.TokenName [token.type], true);
        AddFragment (null, Language.TokenDescription [token.type], false);
        FinalizeTooltip (transform);
    }

    static public void AddFragment (Texture texture, string text, bool bold) {
        Textures.Add (texture);
        Texts.Add (text);
        Bolds.Add (bold);
        //Title.GetComponent<Text> ().preferredHeight / 2;
    }

    static public void DestroyTooltip () {
        foreach (GameObject garbage in garbage) {
            if (garbage != null) {
                GameObject.DestroyImmediate (garbage);
            }
        }
        garbage = new List<GameObject> ();
    }


    static public void FinalizeTooltip (Transform transform) {
        FinalizeUpperTooltip (transform);
        if (garbage.Count > 0) {
            garbage [0].AddComponent<Tooltip> ().UpdateTransparency ();
        }
    }

    static public void FinalizeUpperTooltip (Transform transform) {
        background = CreateSprite ("UI/Panel_PopUp_01_Sliced", false);
        GameObject.DestroyImmediate (background.GetComponent<BoxCollider> ());
        GameObject.DestroyImmediate (background.GetComponent<UIController> ());
        pointer = CreateSprite ("UI/Panel_PopUp_01_Sliced_Tip", false);
        GameObject.DestroyImmediate (pointer.GetComponent<BoxCollider> ());
        GameObject.DestroyImmediate (pointer.GetComponent<UIController> ());
        garbage.Add (background);
        garbage.Add (pointer);

        GameObject someAnchor = new GameObject ();
        someAnchor.transform.SetParent (transform);
        someAnchor.transform.localPosition = new Vector3 (0, 0, 0.5f);

        SpriteRenderer SR = transform.GetComponent<SpriteRenderer> ();
        if (SR != null) {
            someAnchor.transform.localPosition = new Vector3 (0, 0.5f, 0);
            someAnchor.transform.localPosition *= SR.size.y;
        }

        anchor = new Vector3 ((int) Input.mousePosition.x,
            (int) (Camera.main.WorldToScreenPoint (someAnchor.transform.position)).y);
        DestroyImmediate (someAnchor);

        //anchor = Input.mousePosition;
        anchor *= 1080f / Screen.height;
        anchor.y = 1080f - anchor.y;

        height = 30;
        width = 30;

        for (int x = Texts.Count - 1; x >= 0; x--) {
            string text = Texts [x];
            GameObject textObject = CreateUIText (text, (int) anchor.x, (int) anchor.y);
            Text textC = textObject.GetComponent<Text> ();
            if (Bolds [x]) {
                textC.fontStyle = FontStyle.Bold;
            }
            textC.color = Color.white;
            textC.fontSize = 24;
            float thisHeight = textC.preferredHeight;
            float thisWidth = Mathf.Min (textC.preferredWidth, 300);
            height += thisHeight;
            width = Mathf.Max (width, thisWidth + 30);
            SetAnchoredPosition (textObject, (int) anchor.x, (int) (anchor.y - height + thisHeight / 2));
            garbage.Add (textObject);
        }

        if (anchor.y - height < 0) {
            DestroyTooltip ();
            FinalizeSideTooltip (transform);
            return;
        }

        SetInPixPosition (background, (int) anchor.x, (int) (anchor.y - 15 - height / 2), 30, false);
        SetInPixPosition (pointer, (int) anchor.x, (int) (anchor.y - 15 - anchor.y % 2), 31, false);
        SetInPixScale (background, (int) width, (int) height, false);
        SetInPixScale (pointer, 30, 30, false);
    }

    static public void FinalizeSideTooltip (Transform transform) {
        background = CreateSprite ("UI/Panel_PopUp_01_Sliced", false);
        GameObject.DestroyImmediate (background.GetComponent<BoxCollider> ());
        GameObject.DestroyImmediate (background.GetComponent<UIController> ());
        pointer = CreateSprite ("UI/Panel_PopUp_01_Sliced_Tip", false);
        GameObject.DestroyImmediate (pointer.GetComponent<BoxCollider> ());
        GameObject.DestroyImmediate (pointer.GetComponent<UIController> ());
        garbage.Add (background);
        garbage.Add (pointer);

        int side = 1;
        if (Input.mousePosition.x > Screen.width / 2) {
            side = -1;
        }

        GameObject someAnchor = new GameObject ();
        someAnchor.transform.SetParent (transform);
        someAnchor.transform.localPosition = new Vector3 (0.5f * side, 0, 0);

        SpriteRenderer SR = transform.GetComponent<SpriteRenderer> ();
        if (SR != null) {
            someAnchor.transform.localPosition = new Vector3 (0.5f * side, 0, 0);
            someAnchor.transform.localPosition *= SR.size.x;
        }

        anchor = new Vector3 (
            (int) (Camera.main.WorldToScreenPoint (someAnchor.transform.position)).x + 10 * side,
            (int) Input.mousePosition.y);
        DestroyImmediate (someAnchor);

        //anchor = Input.mousePosition;
        anchor *= 1080f / Screen.height;
        anchor.y = 1080f - anchor.y;

        height = 30;
        width = 30;

        GameObject [] textObjects = new GameObject [Texts.Count];

        for (int x = Texts.Count - 1; x >= 0; x--) {
            string text = Texts [x];
            textObjects [x] = CreateUIText (text, (int) anchor.x, (int) anchor.y);
            Text textC = textObjects [x].GetComponent<Text> ();
            if (Bolds [x]) {
                textC.fontStyle = FontStyle.Bold;
            }
            textC.color = Color.white;
            textC.fontSize = 24;
            float thisWidth = Mathf.Min (textC.preferredWidth, 300);
            float thisHeight = textC.preferredHeight;
            height += thisHeight;
            width = Mathf.Max (width, thisWidth + 30);
            textObjects [x] = textObjects [x];
            garbage.Add (textObjects [x]);
        }

        float tempHeight = anchor.y + height / 2 - 30;

        for (int x = Texts.Count - 1; x >= 0; x--) {
            Text textC = textObjects [x].GetComponent<Text> ();
            float thisHeight = textC.preferredHeight;
            tempHeight -= thisHeight;
            SetAnchoredPosition (textObjects [x], (int) (anchor.x + side * width / 2), (int) (tempHeight + thisHeight / 2));
        }

        SetInPixPosition (background, (int) (anchor.x + side * width / 2), (int) (anchor.y - 15), 30, false);
        SetInPixPosition (pointer, (int) anchor.x, (int) (anchor.y - 15 - anchor.y % 2), 31, false);
        SetInPixScale (background, (int) width, (int) height, false);
        SetInPixScale (pointer, 30, 30, false);
        pointer.transform.localEulerAngles = new Vector3 (0, 0, -90 * side);
    }
}