using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : GOUI {

    static public bool permanent = false;

    static public List<bool> Bolds;
    static public List<Texture> Textures;
    static public List<string> Texts;
    static public GameObject [] textObjects;
    static public float [] textHeights;


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
            if (obj != null) {
                SpriteRenderer sRenderer = obj.GetComponent<SpriteRenderer> ();
                if (sRenderer != null) {
                    Color col = sRenderer.color;
                    sRenderer.color = new Color (col.r, col.g, col.b, timer / timerScale);
                }
            } 
        }
    }

    static public void NewTooltip () {
        if (permanent) {
            return;
        }
        DestroyTooltip ();
        Bolds = new List<bool> ();
        Textures = new List<Texture> ();
        Texts = new List<string> ();
        garbage = new List<GameObject> ();
    }

    static public void NewTooltip (int px, int py, int side, string s) {
        DestroyPermanentTooltip ();
        NewTooltip ();
        permanent = true;
        AddFragment (null, s, false);
        FinalizeTooltip (px, py, side);
    }

    static public void NewTooltip (Transform transform, string s) {
        if (permanent) {
            return;
        }
        NewTooltip ();
        AddFragment (null, s, false);
        FinalizeTooltip (transform);
    }

    static public void NewTooltip (Transform transform, CardClass card) {
        if (permanent) {
            return;
        }
        NewTooltip ();
        AddFragment (null, Language.AbilityName [(int) card.abilityType], true);
        AddFragment (null, Language.GetAbilityDescription (card.abilityType), false);
        AddFragment (null, "", false);
        AddFragment (null, Language.TokenName [(int) card.tokenType], true);
        AddFragment (null, Language.GetTokenDescription (card.tokenType), false);
        FinalizeTooltip (transform);
    }

    static public void NewAvatarTooltip (Transform transform, int avatarNumber) {
        if (permanent) {
            return;
        }
        NewTooltip ();
        AddFragment (null, Language.AvatarName [avatarNumber], true);
        FinalizeTooltip (transform);
    }

    static public void NewAbilityTypeTooltip (Transform transform, AbilityType abilityType) {
        if (permanent) {
            return;
        }
        NewTooltip ();
        AddFragment (null, Language.AbilityName [(int) abilityType], true);
        AddFragment (null, Language.GetAbilityDescription (abilityType), false);
        FinalizeTooltip (transform);
    }

    static public void NewTokenTypeTooltip (Transform transform, TokenType tokenType) {
        if (permanent) {
            return;
        }
        NewTooltip ();
        AddFragment (null, Language.TokenName [(int) tokenType], true);
        AddFragment (null, Language.GetTokenDescription (tokenType), false);
        FinalizeTooltip (transform);
    }

    static public void NewTooltip (Transform transform, TokenClass token) {
        if (permanent) {
            return;
        }
        NewTooltip ();
        AddFragment (null, Language.TokenName [(int) token.type], true);
        AddFragment (null, Language.GetTokenDescription (token.type), false);
        FinalizeTooltip (transform);
    }

    static public void AddFragment (Texture texture, string text, bool bold) {
        Textures.Add (texture);
        Texts.Add (text);
        Bolds.Add (bold);
        //Title.GetComponent<Text> ().preferredHeight / 2;
    }

    static public void DestroyPermanentTooltip () {
        foreach (GameObject garbage in garbage) {
            if (garbage != null) {
                GameObject.DestroyImmediate (garbage);
            }
        }
        garbage = new List<GameObject> ();
        permanent = false;
    }

    static public void DestroyTooltip () {
        if (permanent) {
            return;
        }
        DestroyPermanentTooltip ();
    }


    static public void FinalizeTooltip (Transform transform) {
        if (permanent) {
            return;
        }
        if (!FinalizeUpperTooltip (transform)) {
            DestroyTooltip ();
            FinalizeSideTooltip (transform);
        }
        if (garbage.Count > 0) {
            garbage [0].AddComponent<Tooltip> ().UpdateTransparency ();
        }
    }

    static public void FinalizeTooltip (int px, int py, int side) {
        px = px * Screen.height / 1080;
        py = (1080 - py) * Screen.height / 1080;
        px += Screen.width / 2 - 720;
        FinalizeUpperTooltip (px, py, side);
        if (garbage.Count > 0) {
            garbage [0].AddComponent<Tooltip> ().UpdateTransparency ();
        }
    }

    static public bool FinalizeUpperTooltip (Transform transform) {
        GameObject someAnchor = new GameObject ();
        someAnchor.transform.SetParent (transform);
        someAnchor.transform.localPosition = new Vector3 (0, 0, 0.5f);

        SpriteRenderer SR = transform.GetComponent<SpriteRenderer> ();
        if (SR != null) {
            someAnchor.transform.localPosition = new Vector3 (0, 0.5f, 0);
            someAnchor.transform.localPosition *= SR.size.y;
        }
        int nx = (int) (Input.mousePosition.x);
        int ny = (int) (Camera.main.WorldToScreenPoint (someAnchor.transform.position)).y;
        DestroyImmediate (someAnchor);
        return FinalizeUpperTooltip (nx, ny, 1);
    }

    static public bool FinalizeUpperTooltip (int px, int py, int side) {
        background = CreateUIImage ("UI/Panel_PopUp_01_Sliced", false);
        DestroyImmediate (background.GetComponent<BoxCollider> ());
        DestroyImmediate (background.GetComponent<UIController> ());
        background.GetComponent<Image> ().raycastTarget = false;
        pointer = CreateUIImage ("UI/Panel_PopUp_01_Sliced_Tip", false);
        DestroyImmediate (pointer.GetComponent<BoxCollider> ());
        DestroyImmediate (pointer.GetComponent<UIController> ());
        pointer.GetComponent<Image> ().raycastTarget = false;
        garbage.Add (background);
        garbage.Add (pointer);

        anchor = new Vector3 (px, py);

        anchor *= 1080f / Screen.height;
        anchor.x = (int) (anchor.x - 540 * Screen.width / Screen.height + 720);
        anchor.y = 1080f - anchor.y;

        height = 30;
        width = 30;

        /*GameObject button = null;
        
        if (permanent) {
            button = CreateUIButton ("UI/Butt_M_Apply",  true);
            button.name = UIString.TooltipDestroy;
            button.GetComponent<Image> ().type = Image.Type.Simple;
            SetInPixScale (button, 60, 60);
            garbage.Add (button);
            height += 90;
        }*/


        int count = Texts.Count;
        textObjects = new GameObject [count];
        textHeights = new float [count];

        for (int x = count - 1; x >= 0; x--) {
            string text = Texts [x];
            GameObject textObject = CreateUIText (text, (int) anchor.x, (int) anchor.y);
            Text textC = textObject.GetComponent<Text> ();
            if (Bolds [x]) {
                textC.fontStyle = FontStyle.Bold;
            }
            textC.color = Color.white;
            textC.fontSize = 24;
            float thisHeight = textC.preferredHeight;
            float thisWidth = Mathf.Min (textC.preferredWidth, 400);
            height += thisHeight;
            width = Mathf.Max (width, thisWidth + 30);
            textObjects [x] = textObject;
            textHeights [x] = height - thisHeight / 2;
            garbage.Add (textObject);
        }

        if (anchor.y < height) {
            if (permanent) {
                DestroyPermanentTooltip ();
                permanent = true;
                FinalizeSideTooltip (px, py, side);
            }
            return false;
        }
        
        float posX2 = Mathf.Clamp (anchor.x, width / 2, 1440 - width / 2);

        /*if (permanent) {
            SetAnchoredPosition (button, (int) posX2, (int) (anchor.y - 75));
        }*/

        for (int x = Texts.Count - 1; x >= 0; x--) {
            SetAnchoredPosition (textObjects [x], (int) posX2, (int) (anchor.y - textHeights [x]));
        }

        SetInPixPosition (background, (int) (posX2), (int) (anchor.y - 15 - height / 2), 30, false);
        SetInPixPosition (pointer, (int) (anchor.x), (int) (anchor.y - 15 - anchor.y % 2), 31, false);
        SetInPixScale (background, (int) width, (int) height, false);
        SetInPixScale (pointer, 30, 30, false);
        return true;
    }

    static public bool FinalizeSideTooltip (Transform transform) {
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

        int px = (int) (Camera.main.WorldToScreenPoint (someAnchor.transform.position)).x + 10 * side;
        int py = (int) Input.mousePosition.y;

        DestroyImmediate (someAnchor);

        return FinalizeSideTooltip (px, py, side);
    }

    static public bool FinalizeSideTooltip (int px, int py, int side) {
        background = CreateUIImage ("UI/Panel_PopUp_01_Sliced", false);
        GameObject.DestroyImmediate (background.GetComponent<BoxCollider> ());
        GameObject.DestroyImmediate (background.GetComponent<UIController> ());
        pointer = CreateUIImage ("UI/Panel_PopUp_01_Sliced_Tip", false);
        GameObject.DestroyImmediate (pointer.GetComponent<BoxCollider> ());
        GameObject.DestroyImmediate (pointer.GetComponent<UIController> ());
        garbage.Add (background);
        garbage.Add (pointer);

        /*int side = 1;
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
        DestroyImmediate (someAnchor);*/
        anchor = new Vector3 (px, py);

        //anchor = Input.mousePosition;
        anchor *= 1080f / Screen.height;
        anchor.x = (int) (anchor.x - 540 * Screen.width / Screen.height + 720);
        anchor.y = 1080f - anchor.y;

        height = 30;
        width = 30;

        /*GameObject button = null;

        if (permanent) {
            button = CreateUIButton ("UI/Butt_M_Apply", true);
            button.name = UIString.TooltipDestroy;
            button.GetComponent<Image> ().type = Image.Type.Simple;
            SetInPixScale (button, 60, 60);
            garbage.Add (button);
            height += 90;
        }*/

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
            float thisWidth = Mathf.Min (textC.preferredWidth, 400);
            float thisHeight = textC.preferredHeight;
            height += thisHeight;
            width = Mathf.Max (width, thisWidth + 30);
            textObjects [x] = textObjects [x];
            garbage.Add (textObjects [x]);
        }

        float tempHeight = anchor.y + height / 2 - 15;
        if (anchor.y < height / 2) {
            tempHeight += (height / 2) - anchor.y;
        }
            /*
            if (permanent) {
                SetAnchoredPosition (button, (int) (anchor.x + side * width / 2), (int) (tempHeight - 75));
            }*/

            for (int x = Texts.Count - 1; x >= 0; x--) {
            Text textC = textObjects [x].GetComponent<Text> ();
            float thisHeight = textC.preferredHeight;
            tempHeight -= thisHeight;
            SetAnchoredPosition (textObjects [x], (int) (anchor.x + side * width / 2), (int) (tempHeight + thisHeight / 2));
        }
        
        SetInPixPosition (background, (int) (anchor.x + side * width / 2), Mathf.Max ((int) (anchor.y), (int) (height / 2)), 30, false);
        SetInPixPosition (pointer, (int) anchor.x, (int) (anchor.y - anchor.y % 2), 31, false);
        SetInPixScale (background, (int) width, (int) height, false);
        SetInPixScale (pointer, 30, 30, false);
        pointer.transform.localEulerAngles = new Vector3 (0, 0, -90 * side);
        return true;
    }
}
