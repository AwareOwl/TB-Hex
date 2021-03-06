﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GOUI : MonoBehaviour {

    static public GameObject CurrentCanvas;
    static public GameObject UICanvas;
    static public GOUI CurrentGOUI;
    static public GOUI CurrentSubGOUI;

    static public GameObject OptionsButton;
    static public GameObject ChatButton;

    static float globalScale = 2;


    static public void DestroyMenu () {
        TutorialManager.Reset ();
        DestroySubMenu ();
        if (CurrentGOUI != null) {
            CurrentGOUI.DestroyThis ();
            CreateNewCanvas ();
        }
        Transform trans = UICanvas.transform;
        int count = trans.childCount;
        Transform [] t = new Transform [count];
        for (int x = 0; x < count; x++) {
            t [x] = trans.GetChild (x);
        }
        for (int x = 0; x < count; x++) {
            GameObject.DestroyImmediate (t [x].gameObject);
        }
    }

    static public void DestroySubMenu () {
        if (CurrentSubGOUI != null) {
            CurrentSubGOUI.DestroyThis ();
        }
    }

    virtual public void DestroyThis () {
    }

    static public void DestroyTemplateButtons () {
        DestroyImmediate (OptionsButton);
        DestroyImmediate (ChatButton);
    }

    virtual public void ShowPropertiesMenu () {
    }

    static public void CreateNewCanvas () {
        if (CurrentCanvas != null) {
            DestroyImmediate (CurrentCanvas);
        }
        CameraScript.SetStandardCamera ();
        CurrentCanvas = new GameObject ();
        CurrentCanvas.transform.parent = CameraScript.CameraObject.transform;
        CurrentCanvas.transform.localPosition = new Vector3 (0, 0, 0.8655f * globalScale);
        CurrentCanvas.transform.localEulerAngles = new Vector3 (0, 0, 0);
        CurrentCanvas.name = "CurrentCanvas";

        GameObject Clone;
        Clone = CreateSprite ("UI/Butt_S_Settings", 1395, 45, 11, 60, 60, true);
        Clone.name = UIString.ShowOptionsMenu;
        OptionsButton = Clone;
        Clone = CreateSprite ("UI/Butt_S_Name", 1335, 45, 11, 60, 60, true);
        Clone.name = UIString.ShowChat;
        ChatButton = Clone;
    }

    public void SetExitButton () {
        SetSprite (OptionsButton, "UI/Butt_S_Value", true);
        OptionsButton.name = UIString.ExitApp;
    }

    static public GameObject CreateText (string s, int px, int py, int layer, float scale) {
        GameObject Clone = CreateText (s);
        Clone.transform.localScale = Vector3.one * scale * globalScale;
        SetInPixPosition (Clone, px, py, layer);
        return Clone;
    }

    static public GameObject CreateText (string s) {
        GameObject Clone = CreateText ();
        SetText (Clone, s);
        return Clone;
    }

    static public GameObject SetText (GameObject obj, string s) {
        TextMesh textMesh = obj.GetComponent<TextMesh> ();
        if (textMesh != null) {
            textMesh.text = s;
            return obj;
        }
        Text text = obj.GetComponent<Text> ();
        if (text != null) {
            text.text = s;
        }
        return obj;
    }

    static public void AttachToCanvas (GameObject obj) {
        if (CurrentCanvas == null) {
            CreateNewCanvas ();
        }
        obj.transform.SetParent (CurrentCanvas.transform);
        obj.transform.localEulerAngles = new Vector3 (0, 0, 0);
    }

    static public GameObject CreateText () {
        GameObject Clone;
        Clone = Instantiate (Resources.Load ("Prefabs/PreText")) as GameObject;
        AttachToCanvas (Clone);
        return Clone;
    }

    static public GameObject CreateUISlider (int px, int py, int sx, int sy) {
        GameObject Clone;
        Clone = Instantiate (Resources.Load ("Prefabs/PreUISlider")) as GameObject;
        AttachToUICanvas (Clone, px, py);
        Clone.GetComponent<RectTransform> ().localScale = new Vector3 (2, 2, 2);
        SetInPixScale (Clone, sx, sy);
        return Clone;
    }

    static public GameObject CreateSprite (bool addUIController = true) {
		GameObject Clone;
		Clone = new GameObject ();
        AttachToCanvas (Clone);
        Clone.AddComponent<SpriteRenderer> ();
        if (addUIController) {
            Clone.AddComponent<UIController> ();
            AddColider (Clone);
        }
		Clone.GetComponent<SpriteRenderer> ().drawMode = SpriteDrawMode.Sliced;
		return Clone;
    }

    static public GameObject CreateBackgroundSprite (string assetName) {
        GameObject Clone = CreateSprite (false);
        SetSprite (Clone, assetName);
        return Clone;
    }

	static public GameObject CreateSprite (string assetName, bool onMouseOver = false) {
		GameObject Clone = CreateSprite ();
		SetSprite (Clone, assetName, onMouseOver);
		return Clone;
    }

    static public GameObject CreateSpriteWithText (
        string assetName, string text, int px, int py, int layer, int sx, int sy) {
        return CreateSpriteWithText (assetName, text, px, py, layer, sx, sy, 0.03f);
    }


    static public GameObject CreateSpriteWithText (
        string assetName, string text, int px, int py, int layer, int sx, int sy, float st) {
        GameObject sprite = CreateSprite (assetName, px, py, layer, sx, sy, true);
        GameObject textObject = CreateText (text, px, py, layer + 1, st);
        AddTextToGameObject (sprite, textObject);
        textObject.transform.SetParent (sprite.transform);
        textObject.name = "Text";
        return sprite;
    }

    static public GameObject CreateBackgroundSprite (string assetName, int px, int py, int layer, int sx, int sy) {
        GameObject Clone = CreateBackgroundSprite (assetName);
        SetInPixPosition (Clone, px, py, layer);
        SetInPixScale (Clone, sx, sy);
        return Clone;
    }

    static public GameObject CreateSprite (string assetName, int px, int py, int layer, int sx, int sy, bool onMouseOver) {
        GameObject Clone = CreateSprite (assetName, onMouseOver);
        SetInPixPosition (Clone, px, py, layer);
        SetInPixScale (Clone, sx, sy);
        return Clone;
    }

    static public void SetInPixPosition (GameObject Clone, int x, int y, int z) {
        SetInPixPosition (Clone, x, y, z, true);
    }

    static public void SetInPixPosition (GameObject Clone, int x, int y, int z, bool updateCollider) {
        RectTransform rTransform = Clone.GetComponent<RectTransform> ();
        if (rTransform != null) {
            rTransform.anchoredPosition = new Vector2 (x /*- 540 * Screen.width / Screen.height,*/ - 720, -y + 540);
        } else {

            Clone.transform.localPosition = new Vector3 (globalScale * (x - 720f) / 1080, globalScale * (540f - y) / 1080, 0);
        }
        if (Clone.GetComponent<Renderer> () != null) {
            Clone.GetComponent<Renderer> ().sortingOrder = z;
        }
        if (updateCollider) {
            AddColider (Clone);
            if (Clone.GetComponent<BoxCollider> () != null) {
                Clone.GetComponent<BoxCollider> ().center = new Vector3 (0, 0, -z * 0.01f);
            }
        }
    }

	static public void AddColider (GameObject Clone) {
		if (Clone.GetComponent <SpriteRenderer> () != null) {
			bool enabled = true;
            Vector3 center = Vector3.zero;
            if (Clone.GetComponent<BoxCollider> () != null) {
                center = Clone.GetComponent<BoxCollider> ().center;
                enabled = Clone.GetComponent<BoxCollider> ().enabled;
                DestroyImmediate (Clone.GetComponent<BoxCollider> ());
            }
            Clone.AddComponent<BoxCollider> ().size = (Vector3) Clone.GetComponent<SpriteRenderer> ().size + new Vector3 (0, 0, 0.05f);
            Clone.GetComponent<BoxCollider> ().center = center;
            Clone.GetComponent<BoxCollider> ().enabled = enabled;
        }
	}

	static public Vector3 GetInPixPosition (int x, int y, float z) {
		return new Vector3 (GetInPixPositionX (x), (540f - y) / 1080, z);
	}

	static public float GetInPixPositionX (int x) {
		return 10f * (x - 720) / 1080;
	}

	static public float GetInPixPositionY (int y) {
		return 10f * (y - 540) / 1080;
    }

    static public void SetInPixScale (GameObject Clone, int x, int y) {
        SetInPixScale (Clone, x, y, true);
    }

    static public void SetInPixScale (GameObject Clone, int x, int y, bool updateCollider) {
        SpriteRenderer sRenderer = Clone.GetComponent<SpriteRenderer> ();
        if (sRenderer != null) {
            Clone.GetComponent<SpriteRenderer> ().size = new Vector2 (10f * x / 1080f, 10f * y / 1080f);
            Clone.transform.localScale = new Vector3 (globalScale * 0.1f, globalScale * 0.1f, 0.1f); //new Vector3 (Mathf.Min (x, y) / 1080f, Mathf.Min (x, y) / 1080f, 1);
            if (updateCollider) {
                AddColider (Clone);
            }
        }
        RectTransform rTransform = Clone.GetComponent<RectTransform> ();
        if (rTransform != null) {
            rTransform.sizeDelta = new Vector2 (x, y);
            Transform temp = rTransform.Find ("Template");
            if (temp != null) {
                rTransform.Find ("Label").GetComponent<Text> ().fontSize = y / 2;
                Transform content = temp.Find ("Viewport").Find ("Content");
                content.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, y * 0.9f);
                Transform item = content.Find ("Item");
                item.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, y * 0.8f);
                item.Find ("Item Checkmark").GetComponent<RectTransform> ().anchoredPosition = new Vector2 (y * 0.4f, 0);
                item.Find ("Item Checkmark").GetComponent<RectTransform> ().sizeDelta = new Vector2  (y * 0.6f, y * 0.6f);
                item.Find ("Item Label").GetComponent<Text> ().fontSize = y / 3;
                item.Find ("Item Label").GetComponent<RectTransform> ().anchoredPosition = new Vector2 (y * 0.8f, 0);
            }
        }
    }

    static public void SetSpriteScale (GameObject Clone, int x, int y) {
		Clone.GetComponent<SpriteRenderer> ().size = new Vector2 (x / 180f, y / 180f);
		AddColider (Clone);
	}

	static public void SetSprite (GameObject obj, string assetName) {
        SetSprite (obj, GetSprite (assetName));
	}

    static public void SetSprite (GameObject obj, Sprite sprite) {
        if (obj.GetComponent<SpriteRenderer> () != null) {
            obj.GetComponent<SpriteRenderer> ().sprite = sprite;
        }
        if (obj.GetComponent<UIController> () != null) {
            obj.GetComponent<UIController> ().NormalSprite = sprite;
        }
    }

    static public void SetSpriteColor (GameObject obj, Color col) {
        obj.GetComponent<SpriteRenderer> ().color = col;
    }

	static public void SetSprite (GameObject obj, string assetName, bool onMouseOver) {
		UIController CS = obj.GetComponent<UIController> ();
		SetSprite (obj, assetName);
		if (assetName.EndsWith ("_D")) {
			assetName = assetName.Remove (assetName.Length - 2);
		}
		if (assetName.EndsWith ("_P")) {
			if (CS != null) {
				CS.OnMouseOverSprite = CS.NormalSprite;
				CS.OnMouseClickSprite = CS.NormalSprite;
			}
		}
		if (CS != null && onMouseOver) {
			CS.OnMouseOverSprite = GetSprite (assetName + "_H");
			CS.OnMouseClickSprite = GetSprite (assetName + "_P");
		}
    }

    static public void SetInteractible (GameObject obj, bool interactible) {
        UIController CS = obj.GetComponent<UIController> ();
        CS.interactible = interactible;
    }

    static public void RemoveOnMouseOver (GameObject obj) {
        UIController CS = obj.GetComponent<UIController> ();
        CS.OnMouseOverSprite = null;
        CS.OnMouseClickSprite = null;
        CS.Text = null;
    }

	static public Sprite GetSprite (string assetName){
		return Resources.Load (assetName, typeof (Sprite)) as Sprite;
    }

    static public Texture GetTexture (string assetName) {
        return Resources.Load (assetName, typeof (Texture)) as Texture;
    }

    static public GameObject CreateUIImage (string assetName, bool onMouseOver) {
        GameObject clone;

        clone = Instantiate (Resources.Load ("Prefabs/PreUIImage")) as GameObject;
        clone.transform.SetParent (UICanvas.transform);
        clone.GetComponent<Image> ().sprite = GetSprite (assetName);
        clone.GetComponent<Image> ().type = Image.Type.Tiled;
        clone.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
        clone.AddComponent<UIController> ();

        SetSprite (clone, assetName, onMouseOver);

        return clone;
    }

    static public GameObject CreateUIImage (string assetName, int px, int py, int sx, int sy, bool onMouseOver) {
        GameObject clone;

        clone = Instantiate (Resources.Load ("Prefabs/PreUIImage")) as GameObject;
        clone.transform.SetParent (UICanvas.transform);
        clone.GetComponent<Image> ().sprite = GetSprite (assetName);
        SetAnchoredPosition (clone, px, py);
        clone.GetComponent<RectTransform> ().sizeDelta = new Vector2 (sx, sy);
        clone.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
        clone.AddComponent<UIController> ();

        SetSprite (clone, assetName, onMouseOver);

        return clone;
    }

    static public GameObject CreateUIButton (string assetName, int px, int py, int sx, int sy, bool onMouseOver) {
        GameObject clone = CreateUIButton (assetName, onMouseOver);
        SetAnchoredPosition (clone, px, py);
        clone.GetComponent<RectTransform> ().sizeDelta = new Vector2 (sx, sy);
        clone.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);

        return clone;

    }

    static public GameObject CreateUIButton (string assetName, bool onMouseOver) {
        GameObject clone;

        clone = Instantiate (Resources.Load ("Prefabs/PreUIButton")) as GameObject;
        clone.transform.SetParent (UICanvas.transform);
        clone.GetComponent<Image> ().sprite = GetSprite (assetName);
        clone.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
        clone.AddComponent<UIController> ();
        clone.GetComponent<Button> ().onClick.AddListener (delegate {
            clone.GetComponent<UIController> ().OnClickAction ();
        });

        SetSprite (clone, assetName, onMouseOver);

        return clone;

    }
    
    static public GameObject CreateInputField (string placeholderText, int px, int py, int sx, int sy) {
        GameObject clone;

        clone = Instantiate (Resources.Load ("Prefabs/PreInputField")) as GameObject;
        clone.transform.SetParent (UICanvas.transform);
        SetAnchoredPosition (clone, px, py);
        clone.GetComponent<RectTransform> ().sizeDelta = new Vector2 (sx, sy);
        clone.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
        clone.transform.Find ("Placeholder").GetComponent<Text> ().fontSize = sy / 2;
        clone.transform.Find ("Placeholder").GetComponent<Text> ().text = placeholderText;
        clone.transform.Find ("Placeholder").GetComponent<RectTransform> ().offsetMax = new Vector2 (10, -sy / 4);
        clone.transform.Find ("Text").GetComponent<Text> ().fontSize = sy /2;
        clone.transform.Find ("Text").GetComponent<RectTransform> ().offsetMax = new Vector2 (10, - sy / 4);

        return clone;
    }

    static public GameObject CreateUIToggle (string text, int px, int py, int sx, int sy) {
        GameObject clone;

        clone = Instantiate (Resources.Load ("Prefabs/PreUIToggle")) as GameObject;
        clone.transform.SetParent (UICanvas.transform);
        SetAnchoredPosition (clone, px, py);
        clone.GetComponent<RectTransform> ().sizeDelta = new Vector2 (sx, sy);
        clone.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
        //clone.transform.Find ("Label").GetComponent<Text> ().fontSize = sy / 2;
        clone.transform.Find ("Label").GetComponent<Text> ().fontSize = sy - 6;
        clone.transform.Find ("Label").GetComponent<Text> ().text = text;
        //clone.transform.Find ("Label").GetComponent<RectTransform> ().offsetMax = new Vector2 (10, -sy / 4);
        //clone.transform.Find ("Text").GetComponent<Text> ().fontSize = sy / 2;
        //clone.transform.Find ("Text").GetComponent<RectTransform> ().offsetMax = new Vector2 (10, -sy / 4);

        return clone;
    }

    static public GameObject CreateUIScrollView (int px, int py, int sx, int sy) {
        GameObject clone;
        clone = Instantiate (Resources.Load ("Prefabs/PreUIScrollView")) as GameObject;
        AttachToUICanvas (clone, px, py);
        clone.GetComponent<RectTransform> ().sizeDelta = new Vector2 (sx, sy);

        return clone;
    }

    static public GameObject CreateUIText (string text, int px, int py, int sx, int fontSize) {
        GameObject Clone = CreateUIText (text, px, py);
        RectTransform rTrans = Clone.GetComponent<RectTransform> ();
        rTrans.sizeDelta = new Vector2 (sx, rTrans.sizeDelta.y);
        Clone.GetComponent<Text> ().fontSize = fontSize;
        return Clone;
    }

    static public void AttachToUICanvas (GameObject Clone, int px, int py) {
        Clone.transform.SetParent (UICanvas.transform);
        Clone.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
        SetAnchoredPosition (Clone, px, py);
    }

    static public GameObject CreateUIDropdown (int px, int py, int sx, int sy) {
        GameObject Clone;
        Clone = Instantiate (Resources.Load ("Prefabs/PreUIDropdown")) as GameObject;
        AttachToUICanvas (Clone, px, py);
        SetInPixScale (Clone, sx, sy);
        return Clone;
    }

    static public GameObject CreateUIText (string text, int px, int py) {
        GameObject Clone;
        Clone = Instantiate (Resources.Load ("Prefabs/PreUIText")) as GameObject;
        Clone.GetComponent<Text> ().text = text;
        AttachToUICanvas (Clone, px, py);
        return Clone;
    }

    static public void SetAnchoredPosition (GameObject obj, int px, int py) {
        obj.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (px - 720, -py + 540);
    }

    static public void AddTextToGameObject (GameObject obj, GameObject text) {
        obj.GetComponent<UIController> ().Text = text;
    }

    static public void AddReferences (GameObject obj, GameObject [] References) {
        UIController uic = obj.GetComponent<UIController> ();
        foreach (GameObject Ref in References) {
            uic.references.Add (Ref);
        }
    }

    static public GameObject ShowMessageWithProgressionBar (string s, string optionName, int level, int currentBarLevel, int maxBarLevel, int gainedBarLevel) {
        GameObject Background;
        GameObject Collider;
        GameObject Button;
        GameObject Text;
        GameObject Clone;

        Text = CreateUIText (s, 720, 540 - 115);
        int textWidth = (int) Mathf.Min (Mathf.Max (Text.GetComponent<Text> ().preferredWidth, 125), 430);
        int textHeight = (int) Text.GetComponent<Text> ().preferredHeight;

        Background = CreateUIButton ("UI/Panel_Window_01_Sliced", 720, 540, textWidth + 150, textHeight + 390, false);
        Text.transform.SetParent (Background.transform);

        Collider = CreateUIImage ("UI/Transparent", 720, 540, 10000, 10000, false);
        Collider.transform.SetParent (Background.transform);

        //
        Clone = CreateUIText ("Level " + level, 720, 465 + textHeight / 2 + 15);
        Clone.GetComponent<Text> ().fontSize = 24;
        Collider.transform.SetParent (Background.transform);

        float percentage = 1f * currentBarLevel / maxBarLevel;

        Clone = CreateUIImage ("UI/White", 720, 510 + textHeight / 2 + 15, 400, 30, true);
        Clone.transform.SetParent (Background.transform);

        Clone = CreateUIImage ("UI/White", 720 - 200 + (int) (200 * percentage), 510 + textHeight / 2 + 15, (int) (400 * percentage), 30, true);
        Clone.GetComponent<Image> ().color = new Color (0.5f, 0.5f, 1f);
        Clone.transform.SetParent (Background.transform);

        Clone = CreateUIText (currentBarLevel + "/" + maxBarLevel + " (+" + gainedBarLevel + ")", 720, 510 + textHeight / 2 + 15);
        Clone.GetComponent<Text> ().fontSize = 24;
        Clone.transform.SetParent (Background.transform);

        if (level > 1 && currentBarLevel < gainedBarLevel) {
            Clone = CreateUIText ("Level up!", 720, 540 + textHeight / 2 + 15);
            Clone.GetComponent<Text> ().fontSize = 21;
            Clone.GetComponent<Text> ().color = new Color (0, 0.5f, 0);
            Clone.transform.SetParent (Background.transform);
        }
        //

        Button = CreateUIButton ("UI/Butt_M_EmptySquare", 720, 630 + textHeight / 2 + 15, 90, 60, true);
        Button.transform.SetParent (Background.transform);
        

        Text = CreateUIText ("Ok", 720, 630 + textHeight / 2 + 15);
        Text.transform.SetParent (Background.transform);
        AddTextToGameObject (Button, Text);
        
        switch (optionName) {
            case "MatchResults":
                Button.GetComponent<Button> ().onClick.AddListener (delegate {
                    if (UnlockedContent.cached) {
                        UnlockedContent.LoadUnlockedContentMenu (0);
                    } else {
                        MainMenu.ShowMainMenu ();
                    }
                    DestroyImmediate (Background);
                });
                break;
            case "PuzzleResults":
                Button.GetComponent<Button> ().onClick.AddListener (delegate {
                    if (UnlockedContent.cached) {
                        UnlockedContent.LoadUnlockedContentMenu (1);
                    } else {
                        PuzzleMenu.ShowPuzzleMenu ();
                    }
                    DestroyImmediate (Background);
                });
                break;
            case "BossResults":
                Button.GetComponent<Button> ().onClick.AddListener (delegate {
                    if (UnlockedContent.cached) {
                        UnlockedContent.LoadUnlockedContentMenu (3);
                    } else {
                        BossMenu.ShowBossMenu ();
                    }
                    DestroyImmediate (Background);
                });
                break;
            case "TutorialResults":
                Button.GetComponent<Button> ().onClick.AddListener (delegate {
                    if (UnlockedContent.cached) {
                        UnlockedContent.LoadUnlockedContentMenu (4);
                    } else {
                        TutorialMenu.ShowTutorialMenu ();
                    }
                    DestroyImmediate (Background);
                });
                break;
        }

        return Background;
    }

    static public GameObject ShowMessage (string s, string optionName) {
        GameObject Background;
        GameObject Collider;
        GameObject Button;
        GameObject Text;

        Text = CreateUIText (s, 720, 540 - 40);
        int textWidth = (int) Mathf.Min (Mathf.Max (Text.GetComponent<Text> ().preferredWidth, 125), 430);
        int textHeight = (int) Text.GetComponent<Text> ().preferredHeight;

        Background = CreateUIButton ("UI/Panel_Window_01_Sliced", 720, 540, textWidth + 150, textHeight + 210, false);
        DestroyImmediate (Background.GetComponent<UIController> ());
        Text.transform.SetParent (Background.transform);

        Collider = CreateUIImage ("UI/Transparent", 720, 540, 10000, 10000, false);
        Collider.transform.SetParent (Background.transform);
        
        Button = CreateUIButton ("UI/Butt_M_EmptySquare", 720, 540 + textHeight / 2 + 15, 90, 60, true);
        Button.transform.SetParent (Background.transform);

        Text = CreateUIText ("Ok", 720, 540 + textHeight / 2 + 15);
        Text.transform.SetParent (Background.transform);
        AddTextToGameObject (Button, Text);

        switch (optionName) {
            case "Destroy":
                Button.GetComponent<Button> ().onClick.AddListener (delegate {
                    DestroyImmediate (Background);
                });
                break;
            case "MainMenu":
                Button.GetComponent<Button> ().onClick.AddListener (delegate {
                    MainMenu.ShowMainMenu ();
                    DestroyImmediate (Background);
                });
                break;
            case UIString.Cancel:
                SetInPixScale (Button, 120, 60);
                Text.GetComponent<Text> ().text = Language.Cancel;
                Button.GetComponent<Button> ().onClick.AddListener (delegate {
                    ClientLogic.MyInterface.CmdLeaveQuickQueue ();
                    MainMenu.ShowMainMenu ();
                    DestroyImmediate (Background);
                });
                break;
            case "StartGameVsAI":
                Button.GetComponent<Button> ().onClick.AddListener (delegate {
                    ClientLogic.MyInterface.CmdJoinGameAgainstAI ();
                    DestroyImmediate (Background);
                });
                break;
            case "ExitGame":
                Button.GetComponent<Button> ().onClick.AddListener (delegate {
                    Application.Quit ();
                });
                break;
            case UIString.ShowCustomGameLobby:
                Button.GetComponent<Button> ().onClick.AddListener (delegate {
                    CustomGameLobby.ShowCustomGameLobby ();
                    DestroyImmediate (Background);
                });
                break;
        }

        return Background;
    }

    static public GameObject ShowMessage (string s) {
         return ShowMessage (s, "Destroy");
    }
}
