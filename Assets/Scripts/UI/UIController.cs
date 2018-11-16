using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {

    public int number;
    public int x;
    public int y;

    public Sprite NormalSprite;
    public Sprite OnMouseOverSprite;
    public Sprite OnMouseClickSprite;
    public GameObject Text;
    public GameObject HoverObject;

    public List<GameObject> references = new List<GameObject> ();

    public bool Over = false;
    public bool Pressed;
    public bool PressedAndLocked;

    // Use this for initialization
    void Start () {
		
	}

    public void PressAndLock () {
        PressedAndLocked = true;
        SetOnMouseClickSprite ();
    }
    public void FreeAndUnlcok () {
        PressedAndLocked = false;
        if (!Over) {
            SetNormalSprite ();
        } else {
            SetOnMouseOverSprite ();
        }
    }

    public void OnPointerEnter (PointerEventData eventData) {
        Over = true;
        if (!Pressed) {
            SetOnMouseOverSprite ();
        }
    }

    public void OnPointerExit (PointerEventData eventData) {
        if (!Pressed) {
            SetNormalSprite ();
        }
        Over = false;
    }

    public void OnPointerDown (PointerEventData eventData) {
        SetOnMouseClickSprite ();
        Pressed = true;
    }


    private void OnMouseEnter () {
        Over = true;
        if (!Pressed) {
            SetOnMouseOverSprite ();
        }
        if (!EventSystem.current.IsPointerOverGameObject ()) {
            switch (name) {
                case "Tile":
                    OnMouseOverTileAction ();
                    break;
            }
        }
    }

    private void OnMouseExit () {
        switch (name) {
            case "Tile":
                OnMouseLeaveTileAction ();
                break;
        }
        if (!Pressed) {
            SetNormalSprite ();
        }
        Over = false;
    }

    public void SetNormalSprite () {
        if (!PressedAndLocked) {
            SetSprite (NormalSprite);
            SetTextColor (Color.black);
            SetHoverObjectColor (new Color (1, 1, 1, 0f));
        }
    }

    public void SetOnMouseOverSprite () {
        if (!PressedAndLocked) {
            SetSprite (OnMouseOverSprite);
            SetTextColor (Color.white);
            SetHoverObjectColor (new Color (1, 1, 1, 0.25f));
        }
    }

    public void SetOnMouseClickSprite () {
        SetSprite (OnMouseClickSprite);
        SetTextColor (Color.white);
        SetHoverObjectColor (new Color (1, 1, 1, 0.25f));
    }

    public void SetHoverObjectColor (Color color) {
        if (HoverObject != null){
            HoverObject.GetComponent<VisualEffectScript> ().SetColor (color);
        }
    }

    public void SetTextColor (Color color) {
        if (Text != null) {
            if (Text.GetComponent<Renderer> ()) {
                Text.GetComponent<Renderer> ().material.color = color;
            } else if (Text.GetComponent <Text> () != null) {
                Text.GetComponent<Text> ().color = color;
            }
        }
    }

    private void OnMouseOver () {
        if (!EventSystem.current.IsPointerOverGameObject ()) {
            if (Input.GetMouseButtonDown (0)) {
                SetOnMouseClickSprite ();
                Pressed = true;
            }
            if (Input.GetMouseButtonUp (0)) {
                if (Pressed) {
                    switch (name) {
                        case "SelectPL":
                            Language.SetLanguage (Language.Polish);
                            break;
                        case "SelectENG":
                            Language.SetLanguage (Language.English);
                            break;

                        case "StartHost":
                            MyNetworkManager.StartNewHost ();
                            break;
                        case "StartClient":
                            MyNetworkManager.StartNewClient ();
                            break;
                        case "Tile":
                            TileAction ();
                            //transform.parent.GetComponent<VisualEffectScript> ().PushItDown (-1);
                            break;
                        case "LogUserIn":
                            LoginMenu.LogIn ();
                            break;
                        case "RegisterMenu":
                            RegisterMenu.ShowRegisterMenu ();
                            break;
                        case "RegisterUser":
                            RegisterMenu.Register ();
                            break;
                        case "LoginMenu":
                            LoginMenu.ShowLoginMenu ();
                            break;
                        case "CloseMessage":
                            DestroyReferences ();
                            break;

                        // BoardEditor
                        case "SaveBoard":
                            BoardEditorMenu.SaveBoard ();
                            break;
                        case "LoadBoard":
                            BoardEditorMenu.LoadBoard (2);
                            break;
                        case "BoardEditorTileType":
                            BoardEditorMenu.SelectButton (1, number);
                            break;
                        case "BoardEditorOwner":
                            BoardEditorMenu.SelectButton (2, number);
                            break;
                        case "BoardEditorValue":
                            BoardEditorMenu.SelectButton (3, number);
                            break;
                        case "BoardEditorTokenType":
                            BoardEditorMenu.SelectButton (4, number);
                            break;

                        // CardPoolEditor
                        case UIString.CardPoolEditorValue:
                            CardPoolEditor.SelectButton (1, number);
                            break;
                        case UIString.CardPoolEditorTokenType:
                            CardPoolEditor.SelectButton (2, number);
                            break;
                        case UIString.CardPoolEditorAbilityArea:
                            CardPoolEditor.SelectButton (3, number);
                            break;
                        case UIString.CardPoolEditorAbilityType:
                            CardPoolEditor.SelectButton (4, number);
                            break;
                        case UIString.CardPoolEditorCard:
                            CardPoolEditor.CardAction (number);
                            break;

                        case UIString.CardPoolEditorSaveCardPool:
                            CardPoolEditor.SaveCardPool (1);
                            break;



                        // Other
                        default:
                            Debug.Log (name);
                            break;
                    }
                }
                Pressed = false;
            }
        } else {
            //Debug.Log (name);
        }
    }

    public void OnMouseOverTileAction () {
        if (BoardEditorMenu.instance != null) {
        }
        if (InGameUI.instance != null) {
            InGameUI.SetAreaHovers (x, y);
        }
    }

    public void TileAction () {
        if (BoardEditorMenu.instance != null) {
            BoardEditorMenu.TileAction (x, y);
        }
        if (InGameUI.instance != null) {
            InGameUI.TileAction (x, y);
        }
    }

    public void OnMouseLeaveTileAction () {
        if (InGameUI.instance != null) {
            InGameUI.HideAreaHovers ();
        }
    }

    void DestroyReferences () {
        foreach (GameObject Ref in references) {
            DestroyImmediate (Ref);
        }
        DestroyImmediate (gameObject);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonUp (0)) {
            FreeButton ();
        }
    }

    public void FreeButton () {
        Pressed = false;
        if (Over) {
            SetOnMouseOverSprite ();
        } else {
            SetNormalSprite ();
        }
    }

    void SetSprite (Sprite sprite) {
        if (sprite != null){
            if (GetComponent<SpriteRenderer> () != null) {
                GetComponent<SpriteRenderer> ().sprite = sprite;
            } else if (GetComponent<Image> () != null) {
                GetComponent<Image> ().sprite = sprite;
            }
        }
    }
}
