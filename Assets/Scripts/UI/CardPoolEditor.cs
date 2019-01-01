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

    static int [] NumberOfButtons = new int [] { 3, 8, 6, 2, 13 };
    static int [] Selected = new int [] { 0, 1, 0, 0, 1 };
    static GameObject [] [] Buttons;

    static int currentId;

    static PageUI pageUI;

    static GameObject EmptyCardSlot;
    static VisualCard [] CardSlot;

    private void Start () {
        instance = this;
        NumberOfButtons [2] = AppDefaults.AvailableTokens;
        NumberOfButtons [4] = AppDefaults.AvailableAbilities;
        CreateCardPoolEditorMenu ();
        CurrentGUI = this;

        ClientLogic.MyInterface.CmdDownloadCardPoolToEditor (currentId);
        //EditedCardPool.EnableVisualisation ();
        //EditedCardPool.LoadFromFile (1);
    }

    static public void LoadDataToEditor (int gameModeId, string [] cardPool) {
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

    static public int CardCountOnPage () {
        return maxX * maxY;
    }

    static public int DeltaNumber () {
        return CardCountOnPage () * page;
    }

    static public void CardAction (int number) {
        EditedCardPool.SetCard (number + DeltaNumber (), Selected [1] + 1, Selected [2], Selected [3] * 3 + 1, Selected [4]);
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
    }

    static public void RefreshPage () {
        SetEmptySlot (1000);
        int count = CardCountOnPage ();
        for (int x = 0; x < count; x++) {
            UpdateCard (x);
        }
        int pageLimit = EditedCardPool.Card.Count / count + 1;
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
        if (cardNumber < EditedCardPool.Card.Count) {
            if (CardSlot [number] == null) {
                CreateCard (number);
            }
            Debug.Log (number);
            Debug.Log (CardSlot.Length);
            Debug.Log (EditedCardPool.Card.Count);
            Debug.Log (cardNumber);
            CardSlot [number].SetState (EditedCardPool.Card [cardNumber]);
        } else {
            if (CardSlot [number] != null) {
                CardSlot [number].DestroyVisual ();
                CardSlot [number] = null;
            }
            if (cardNumber == EditedCardPool.Card.Count) {
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

        DestroyImmediate (ExitButton);

        Buttons = new GameObject [NumberOfButtons.Length] [];
        for (int x = 0; x < NumberOfButtons.Length; x++) {
            Buttons [x] = new GameObject [NumberOfButtons [x]];
        }

        GameObject Clone =CreateSprite ("UI/Panel_Window_01_Sliced", 870, 425, 10, 1110, 840, false);
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

        for (int x = 1; x < 5; x++) {
            SelectButton (x, Selected [x]);
        }

        CreateCardSlots ();


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
        Selected [type] = number;
        foreach (GameObject button in Buttons [type]) {
            if (button != null) {
                button.GetComponent<UIController> ().FreeAndUnlcok ();
            }
        }
        Buttons [type] [number].GetComponent<UIController> ().PressAndLock ();
    }

    static int type = 0;

    static void AddButtons (int px, int py, int maxX, int maxY) {

        GameObject Clone;
        VisualToken VT;

        int sx = 90 + 60 * maxX;
        int sy = 140 + 60 * maxY;
        int dy = 70;

        BackgroundObject = CreateSprite ("UI/Panel_Window_01_Sliced", px, py, 10, sx, sy, false);

        switch (type) {

            case 0:
                Clone = CreateText ("File", px, py - sy / 2 + dy, 12, 0.02f);
                break;
            case 1:
                Clone = CreateText ("Token Value", px, py - sy / 2 + dy, 12, 0.02f);
                break;
            case 2:
                Clone = CreateText ("Token Type", px, py - sy / 2 + dy, 12, 0.02f);
                break;
            case 3:
                Clone = CreateText ("Ability Area", px, py - sy / 2 + dy, 12, 0.02f);
                break;
            case 4:
                Clone = CreateText ("Ability Type", px, py - sy / 2 + dy, 12, 0.02f);
                break;
        }

        for (int y = 0; y < maxY; y++) {
            for (int x = 0; x < maxX; x++) {
                int number = x + y * maxX;
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
                            BackgroundObject.name = UIString.CardPoolEditorTokenType;
                            VT = new VisualToken ();
                            Clone = VT.Anchor;
                            Clone.transform.SetParent (BackgroundObject.transform);
                            Clone.transform.localEulerAngles = new Vector3 (-90, 0, 0);
                            Clone.transform.localPosition = Vector3.zero;
                            Clone.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
                            VT.SetType (number);
                            DestroyImmediate (VT.Text);
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
                            BackgroundObject.GetComponent<UIController> ().abilityType = number;
                            BackgroundObject.name = UIString.CardPoolEditorAbilityType;
                            Clone = CreateSprite (VisualCard.GetIconPath (number), npx, npy, 12, 45, 45, false);
                            Clone.GetComponent<SpriteRenderer> ().color = AppDefaults.GetAbilityColor (number);
                            Destroy (Clone.GetComponent<Collider> ());
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
