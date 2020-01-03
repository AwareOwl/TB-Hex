using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPoolEditor : GOUI {

    static public CardPoolEditor instance;

    static public CardPoolClass EditedCardPool;

    static public GameObject BackgroundObject;

    static public int TileType;
    static public int Owner;
    static public int Value;
    static public int TokenType;


    static int maxX = 8;
    static int maxY = 4;

    static int page;
    static int abilityPage;
    static int tokenPage;

    static int [] NumberOfButtons = new int [] { 3, 8, 6, 2, 13 };
    static int [] Selected = new int [] { 0, 1, 0, 0, 1 };
    static GameObject [] [] Buttons;
    static GameObject [] [] ButtonImage;
    static GameObject [] [] ButtonText;

    static int currentId;

    static PageUI pageUI;

    static GameObject EmptyCardSlot;
    static VisualCard [] CardSlot;

    static public bool editMode;

    private void Start () {
        instance = this;
        NumberOfButtons [2] = AppDefaults.availableTokens;
        NumberOfButtons [4] = AppDefaults.availableAbilities;
        editMode = GameModeEditor.editMode;
        CreateCardPoolEditorMenu ();
        CurrentGOUI = this;

        DestroyTemplateButtons ();
        ClientLogic.MyInterface.CmdDownloadCardPoolToEditor (currentId);
        //EditedCardPool.EnableVisualisation ();
        //EditedCardPool.LoadFromFile (1);
    }

    static public void LoadDataToEditor (int gameModeId, string [] cardPool) {
        page = 0;
        EditedCardPool = new CardPoolClass ();
        EditedCardPool.LoadFromString (cardPool);

        CreateSampleCards ();
    }

    private void Update () {
        for (int x = 1; x <= 9; x++) {
            if (Input.GetKeyDown (x.ToString ())) {
                SelectPage (x - 1);
            }
        }
    }

    static public void SelectPage (int pageNumber) {
        page = Mathf.Min (pageNumber, EditedCardPool.Card.Count / CardCountOnPage ());
        RefreshPage ();
        pageUI.SelectPage (page);
    }

    static int AbilityPageLimit () {
        if (AppDefaults.availableAbilities <= 34) {
            return 1;
        } else if (AppDefaults.availableAbilities <= 65) {
            return 2;
        } else {
            return 3;
        }
    }

    static int TokenPageLimit () {
        if (AppDefaults.availableTokens <= 20) {
            return 1;
        } else {
            return 2;
        }
    }

    static public void PrevAbilityPage () {
        SelectAbilityPage (abilityPage - 1);
    }

    static public void NextAbilityPage () {
        SelectAbilityPage (abilityPage + 1);
    }

    static public void PrevTokenPage () {
        SelectTokenPage (tokenPage - 1);
    }

    static public void NextTokenPage () {
        SelectTokenPage (tokenPage + 1);
    }

    static public void SelectAbilityPage (int pageNumber) {
        abilityPage = Mathf.Min (pageNumber, AbilityPageLimit ());
        RefreshAbilityPage ();
    }

    static public void SelectTokenPage (int pageNumber) {
        tokenPage = Mathf.Min (pageNumber, TokenPageLimit ());
        RefreshTokenPage ();
    }

    static public void RefreshAbilityPage () {
        RefreshAbilityPage (abilityPage);
    }

    static public void RefreshAbilityPage (int pageNumber) {
        type = 4;
        int count = Buttons [type].Length;
        for (int x = 0; x < count; x++) {
            int number = AbilityButtonToAbilityNumber (x);
            GameObject button = Buttons [type] [x];
            GameObject buttonText = ButtonText [type] [x];
            GameObject buttonImage = ButtonImage [type] [x];
            if (button != null) {
                button.GetComponent<Renderer> ().enabled = true;
                button.GetComponent<Collider> ().enabled = true;
                buttonImage.GetComponent<Renderer> ().enabled = false;
                UIController UIC = button.GetComponent<UIController> ();
                UIC.FreeAndUnlcok ();
                if (abilityPage < AbilityPageLimit () - 1 && x == 33) {
                    button.name = UIString.CardPoolEditorNextAbilityPage;
                    SetText (buttonText, ">");
                } else if (abilityPage > 0 && x == 0) {
                    button.name = UIString.CardPoolEditorPrevAbilityPage;
                    SetText (buttonText, "<");
                } else {
                    button.name = UIString.CardPoolEditorAbilityType;
                    if (number < AppDefaults.availableAbilities) {
                        SetSprite (buttonImage, VisualCard.GetIconPath (number));
                        buttonImage.GetComponent<SpriteRenderer> ().color = AppDefaults.GetAbilityColor (number);
                        UIC.abilityType = (AbilityType) number;
                        buttonImage.GetComponent<Renderer> ().enabled = true;
                        if (Selected [type] == number) {
                            UIC.PressAndLock ();
                        }
                    } else {
                        button.GetComponent<Renderer> ().enabled = false;
                        button.GetComponent<Collider> ().enabled = false;
                    }
                    SetText (buttonText, "");
                }
            }
        }
    }

    static public void RefreshTokenPage () {
        RefreshTokenPage (tokenPage);
    }

    static public void RefreshTokenPage (int tokenPage) {
        VisualToken VT;
        GameObject Clone;
        type = 2;
        int count = Buttons [type].Length;
        for (int x = 0; x < count; x++) {
            int number = TokenButtonToTokenNumber (x);
            TokenType tokenType = (TokenType) number;
            GameObject button = Buttons [type] [x];
            GameObject buttonText = ButtonText [type] [x];
            if (button != null) {
                button.GetComponent<Renderer> ().enabled = true;
                button.GetComponent<Collider> ().enabled = true;
                if (button.transform.childCount > 0) {
                    DestroyImmediate (button.transform.GetChild (0).gameObject);
                }
                UIController UIC = button.GetComponent<UIController> ();
                UIC.FreeAndUnlcok ();
                if (tokenPage < TokenPageLimit () - 1 && x == 19) {
                    button.name = UIString.CardPoolEditorNextTokenPage;
                    SetText (buttonText, ">");
                } else if (tokenPage > 0 && x == 0) {
                    button.name = UIString.CardPoolEditorPrevTokenPage;
                    SetText (buttonText, "<");
                } else {
                    button.name = UIString.CardPoolEditorTokenType;
                    if (number < AppDefaults.availableTokens) {
                        VT = new VisualToken ();
                        Clone = VT.Anchor;
                        Clone.transform.SetParent (button.transform);
                        Clone.transform.localEulerAngles = new Vector3 (-90, 0, 0);
                        Clone.transform.localPosition = Vector3.zero;
                        Clone.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
                        VT.SetType (tokenType);
                        DestroyImmediate (VT.Text);
                        if (Selected [type] == number) {
                            UIC.PressAndLock ();
                        }
                        UIC.tokenType = tokenType;
                    } else {
                        button.GetComponent<Renderer> ().enabled = false;
                        button.GetComponent<Collider> ().enabled = false;
                    }
                    SetText (buttonText, "");
                }
            }
        }
    }

    static public int AbilityButtonToAbilityNumber (int buttonNumber) {
        int output = buttonNumber + abilityPage * 32;
        return output;
    }
    static public int TokenButtonToTokenNumber (int buttonNumber) {
        int output = buttonNumber + tokenPage * 18;
        return output;
    }

    static public int CardCountOnPage () {
        return maxX * maxY;
    }

    static public int DeltaNumber () {
        return CardCountOnPage () * page;
    }

    static public void CardAction (int number) {
        if (!editMode) {
            return;
        }
        EditedCardPool.SetCard (number + DeltaNumber (), Selected [1] + 1, (TokenType) Selected [2], Selected [3] * 3 + 1, (AbilityType) Selected [4]);
        RefreshPage ();
        /*UpdateCard (number);
        if (number == EditedCardPool.Card.Count - 1) {
            SetEmptySlot (number + 1);
        }*/
    }


    static public void CreateSampleCards () {
        CardSlot = new VisualCard [CardCountOnPage ()];
        EmptyCardSlot = CreateEmptySlot ();
        //SetEmptySlot (0);
        RefreshPage ();
        RefreshAbilityPage ();
        RefreshTokenPage ();
    }

    static public void RefreshPage () {
        SetEmptySlot (1000);
        int count = CardCountOnPage ();
        for (int x = 0; x < count; x++) {
            UpdateCard (x);
        }
        int cardPoolCount = EditedCardPool.Card.Count;
        int pageLimit;
        if (editMode) {
            pageLimit = cardPoolCount / count + 1;
        } else {
            pageLimit = (cardPoolCount - 1) / count + 1;
        }
        pageUI.Init (17, pageLimit, new Vector2Int (390, 770), UIString.CardPoolEditorPageButton);
    }


    static public void SaveCardPool () {
        ClientLogic.MyInterface.CmdSaveCardPool (currentId, EditedCardPool.CardPoolToString ());
    }

    static void CreateCard (int number) {
        int x = number % maxX;
        int y = number / maxX;
        VisualCard card = new VisualCard ();
        card.Anchor.transform.SetParent (GOUI.CurrentCanvas.transform);
        card.Anchor.transform.localScale = Vector3.one * 0.14f;
        SetInPixPosition (card.Anchor, 450 + 120 * x, 145 + 156 * y, 12);
        card.Anchor.transform.localEulerAngles = new Vector3 (-90, 0, 0);
        card.Background.name = UIString.CardPoolEditorCard;
        card.Background.GetComponent<UIController> ().number = number;
        CardSlot [number] = card;
    }

    static void UpdateCard (int number) {
        int cardNumber = number + DeltaNumber ();
        //Debug.Log (number + " " + cardNumber + " " + EditedCardPool.Card.Count);
        if (cardNumber < EditedCardPool.Card.Count) {
            if (CardSlot [number] == null) {
                CreateCard (number);
            }
            CardClass card = EditedCardPool.Card [cardNumber];
            CardSlot [number].SetState (card);
            CardSlot [number].Background.GetComponent<UIController> ().card = card;
        } else {
            if (CardSlot [number] != null) {
                CardSlot [number].DestroyVisual ();
                CardSlot [number] = null;
            }
            if (cardNumber == EditedCardPool.Card.Count && editMode) {
                SetEmptySlot (number);
            }
        }
    }

    static GameObject CreateEmptySlot () {
        GameObject Clone = GameObject.CreatePrimitive (PrimitiveType.Quad);
        Clone.transform.SetParent (GOUI.CurrentCanvas.transform);
        Clone.transform.localScale = Vector3.one * 0.15f;
        Clone.transform.localEulerAngles = new Vector3 (0, 0, 0);
        Clone.GetComponent<Renderer> ().material.shader = Shader.Find ("Sprites/Default");
        Clone.GetComponent<Renderer> ().material.color = new Color (0.3f, 0.3f, 0.3f);
        Clone.GetComponent<Renderer> ().material.mainTexture = Resources.Load ("Textures/Other/LittlePlus") as Texture2D;
        Clone.name = UIString.CardPoolEditorCard;
        Clone.AddComponent<UIController> ();
        return Clone;
    }

    static void SetEmptySlot (int number) {
        int x = number % maxX;
        int y = number / maxX;
        SetInPixPosition (EmptyCardSlot, 450 + 120 * x, 160 + 156 * y, 12);
        EmptyCardSlot.GetComponent<UIController> ().number = number;
    }

    static public void ShowCardPoolEditorMenu () {
        DestroyMenu ();
        CurrentCanvas.AddComponent<CardPoolEditor> ();
    }

    static public void ShowCardPoolEditorMenu (int id) {
        currentId = id;
        ShowCardPoolEditorMenu ();
    }

    static public void CreateCardPoolEditorMenu () {


        Buttons = new GameObject [NumberOfButtons.Length] [];
        ButtonText = new GameObject [NumberOfButtons.Length] [];
        ButtonImage = new GameObject [NumberOfButtons.Length] [];
        for (int x = 0; x < NumberOfButtons.Length; x++) {
            Buttons [x] = new GameObject [NumberOfButtons [x]];
            ButtonText [x] = new GameObject [NumberOfButtons [x]];
            ButtonImage [x] = new GameObject [NumberOfButtons [x]];
        }

        GameObject Clone = CreateSprite ("UI/Panel_Window_01_Sliced", 870, 425, 10, 1110, 840, false);
        GameObject.DestroyImmediate (Clone.GetComponent<Collider> ());

        int sx = 345;
        int sy = 150;

        int px = sx / 2;
        int py = sy / 2 + 30;

        int maxX = 4;
        int maxY = 1;

        int dy = 50;

        type = 0;

        AddButtons (px, py, maxX, maxY);

        if (editMode) {
            maxY += 1;

            py += 150 + dy;

            AddButtons (px, py, maxX, maxY);

            maxY += 3;

            py += 270 + dy;

            AddButtons (px, py, maxX, maxY);

            py += 270 + dy;

            maxY = 2;

            AddButtons (px, py, maxX, maxY);

            maxX = 17;

            sx = 90 + 60 * maxX;
            px += sx / 2 + px - 30;
            //py += 150 + dy;

            AddButtons (px, py, maxX, maxY);

        }
        int tempAbilityPage = abilityPage;
        abilityPage = 0;
        for (int x = 1; x < 5; x++) {
            SelectButton (x, Selected [x]);
        }
        abilityPage = tempAbilityPage;

        CreateCardSlots ();

        RefreshAbilityPage (abilityPage);

        pageUI = new PageUI ();

    }

    static public void CreateCardSlots () {
        GameObject Clone;
        for (int y = 0; y < maxY; y++) {
            for (int x = 0; x < maxX; x++) {
                Clone = CreateSprite ("UI/Panel_Slot_01_CollectionCard", 450 + 120 * x, 150 + 156 * y, 11, 120, 150, false);
                GameObject.DestroyImmediate (Clone.GetComponent<Collider> ());
            }
        }
    }

    static public void SelectButton (int type, int number) {
        if (type == 2) {
            if (number != 0 && number != 19) {
                Selected [type] = TokenButtonToTokenNumber (number);
            } else if (number == 0 && tokenPage == 0) {
                Selected [type] = 0;
            }
            RefreshTokenPage ();
        } else if (type == 4) {
            if (number != 0 && number != 33) {
                Selected [type] = AbilityButtonToAbilityNumber (number);
            } else if (number == 0 && abilityPage == 0) {
                Selected [type] = 0;
            }
            RefreshAbilityPage ();
        } else {
            Selected [type] = number;
            foreach (GameObject button in Buttons [type]) {
                if (button != null) {
                    button.GetComponent<UIController> ().FreeAndUnlcok ();
                }
            }
            if (Buttons [type] != null) {
                if (Buttons [type] [number] != null) {
                    Buttons [type] [number].GetComponent<UIController> ().PressAndLock ();
                }
            }
        }
    }

    static int type = 0;

    static void AddButtons (int px, int py, int maxX, int maxY) {

        GameObject Clone;

        int sx = 90 + 60 * maxX;
        int sy = 140 + 60 * maxY;
        int dy = 70;

        BackgroundObject = CreateBackgroundSprite ("UI/Panel_Window_01_Sliced", px, py, 10, sx, sy);

        switch (type) {

            case 0:
                Clone = CreateText (Language.File, px, py - sy / 2 + dy, 12, 0.02f);
                break;
            case 1:
                Clone = CreateText (Language.TokenValue, px, py - sy / 2 + dy, 12, 0.02f);
                break;
            case 2:
                Clone = CreateText (Language.TokenType, px, py - sy / 2 + dy, 12, 0.02f);
                break;
            case 3:
                Clone = CreateText (Language.AbilityArea, px, py - sy / 2 + dy, 12, 0.02f);
                break;
            case 4:
                Clone = CreateText (Language.AbilityType, px, py - sy / 2 + dy, 12, 0.02f);
                break;
        }

        for (int y = 0; y < maxY; y++) {
            for (int x = 0; x < maxX; x++) {
                int number = x + y * maxX;
                TokenType tokenType = (TokenType) number;
                if (!editMode && type == 0 && number == 0) {
                    continue;
                }
                if (number >= NumberOfButtons [type]) {
                    continue;
                }
                int npx = px + x * 60 + 75 - sx / 2;
                int npy = py + y * 60 + 125 - sy / 2;
                if (type == 0) {
                    switch (x) {
                        case 0:
                            BackgroundObject = CreateSprite ("UI/Butt_M_Apply", npx, npy, 11, 60, 60, true);
                            BackgroundObject.name = UIString.CardPoolEditorSaveCardPool;
                            break;
                        case 1:
                            BackgroundObject = CreateSprite ("UI/Butt_S_Help", npx, npy, 11, 60, 60, true);
                            BackgroundObject.name = UIString.CardPoolEditorAbout;
                            break;
                        case 2:
                            BackgroundObject = CreateSprite ("UI/Butt_M_Discard", npx, npy, 11, 60, 60, true);
                            BackgroundObject.name = UIString.GoBackToGameModeEditor;
                            break;
                    }
                }
                if (type > 0) {
                    if (type == 3) {
                        BackgroundObject = CreateSprite ("UI/Butt_M_EmptySquare", npx + (x * 2 + 1) * 30, npy + (y * 2 + 1) * 30, 11, 120, 120, true);
                    } else {
                        BackgroundObject = CreateSprite ("UI/Butt_M_EmptySquare", npx, npy, 11, 60, 60, true);
                    }
                    switch (type) {
                        case 1:
                            BackgroundObject.name = UIString.CardPoolEditorValue;
                            Clone = CreateText ((number + 1).ToString (), npx, npy, 12, 0.03f);
                            AddTextToGameObject (BackgroundObject, Clone);
                            break;
                        case 2:
                            BackgroundObject.GetComponent<UIController> ().tokenType = tokenType;
                            BackgroundObject.name = UIString.CardPoolEditorTokenType;
                            ButtonText [type] [number] = CreateText ("", npx, npy, 13, 0.03f);
                            break;
                        case 3:
                            BackgroundObject.name = UIString.CardPoolEditorAbilityArea;
                            VisualArea area = new VisualArea ();
                            area.Anchor.transform.SetParent (BackgroundObject.transform);
                            area.Anchor.transform.localPosition = new Vector3 (0, 0, 0);
                            area.Anchor.transform.localScale = new Vector3 (0.7f, 0.7f, 0.7f);
                            area.Anchor.transform.localEulerAngles = new Vector3 (-90, 0, 0);
                            if (number == 0) {
                                area.SetAbilityArea (1);
                            } else {
                                area.SetAbilityArea (4);
                            }
                            break;
                        case 4:
                            UIController UIC = BackgroundObject.GetComponent<UIController> ();
                            BackgroundObject.name = UIString.CardPoolEditorAbilityType;
                            Clone = CreateSprite (VisualCard.GetIconPath (number), npx, npy, 12, 45, 45, false);
                            Destroy (Clone.GetComponent<Collider> ());
                            ButtonText [type] [number] = CreateText ("", npx, npy, 13, 0.03f);
                            ButtonImage [type] [number] = Clone;
                            UIC.Text = ButtonText [type] [number];
                            break;
                    }

                }
                BackgroundObject.GetComponent<UIController> ().number = number;
                Buttons [type] [number] = BackgroundObject;

            }
        }
        type++;
    }

}
