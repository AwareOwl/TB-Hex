﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SetEditor : GOUI {

    static public SetEditor instance;

    static public HandClass hand;

    static CardPoolClass cardPool;
    static bool [] available;

    static VisualCard [,] Collection;
    static VisualCard [,] Set;

    static GameObject [,] CollectionCollider;
    static GameObject [,] SetCollider;

    static GameObject [] PageButton;

    static public int SelectedCollectionX = -1;
    static public int SelectedCollectionY;

    static public int Page;
    static public int PageCount = 20;

    static public int setId;

    static public string setName;
    static public int iconNumber;

    static public void ApplySetProperties () {
        ClientLogic.MyInterface.CmdSaveSetProperties (setId, setName, iconNumber);
    }

    private void Start () {
        instance = this;
        Collection = new VisualCard [4, 5];
        CollectionCollider = new GameObject [4, 5];
        Set = new VisualCard [4, 5];
        SetCollider = new GameObject [4, 5];
        CreateCardPoolEditorMenu ();
        CurrentGUI = this;
        ClientLogic.MyInterface.CmdDownloadCardPoolToEditor ();
        ClientLogic.MyInterface.CmdDownloadSetToEditor (setId);
        DestroyImmediate (ExitButton);
    }

    static public void ShowSetEditorMenu (int setId) {
        SetEditor.setId = setId;
        DestroyMenu ();
        CurrentCanvas.AddComponent<SetEditor> ();
    }

    static public void LoadCardPool (string [] s) {
        cardPool = new CardPoolClass ();
        cardPool.LoadFromString (s);
        available = new bool [cardPool.Card.Count];
        SetAllAvailable ();
        LoadPageUI ();
        LoadPage (0);
    }

    static public void SetAllAvailable () {
        for (int x = 0; x < available.Length; x++) {
            available [x] = true;
        }
    }

    static public void LoadPage (int page) {
        SelectedCollectionX = -1;
        Page = page;
        int MaxX = 4;
        for (int x = 0; x < MaxX; x++) {
            for (int y = 0; y < 5; y++) {
                if (Collection [x, y] != null) {
                    Collection [x, y].DestroyVisual ();
                    Collection [x, y] = null;
                }
                int number = y * MaxX + x + page * PageCount;
                if (number < cardPool.Card.Count && available [number]) {
                    LoadCardInCollection (x, y, number);
                }
            }
        }
        foreach (GameObject button in PageButton) {
            if (button != null) {
                button.GetComponent<UIController> ().FreeAndUnlcok ();
            }
        }
        if (PageButton [page] != null) {
            PageButton [page].GetComponent<UIController> ().PressAndLock ();
        }
    }

    static public void SelectCardInCollection (int x, int y) {
        if (SelectedCollectionX != -1) {
            if (Collection [SelectedCollectionX, SelectedCollectionY] != null) {
                Collection [SelectedCollectionX, SelectedCollectionY].DisableHighlight ();
            }
        }
        SelectedCollectionX = x;
        SelectedCollectionY = y;
        if (Collection [x, y] != null) {
            Collection [x, y].EnableHighlight ();
        }
    }

    static public void LoadCardInCollection (int number) {
        int MaxX = 4;
        for (int x = 0; x < MaxX; x++) {
            for (int y = 0; y < 5; y++) {
                if (number == y * MaxX + x + Page * PageCount) {
                    LoadCardInCollection (x, y, number);
                }
            }
        }
    }

    static public void LoadCardInCollection (int x, int y, int number) {
        CardClass card = cardPool.Card [number];
        VisualCard vCard = LoadCard (card);
        Collection [x, y] = vCard;
        SetInPixPosition (vCard.Anchor, 120 + 120 * x, 220 + 156 * y, 12);
        CollectionCollider [x, y].GetComponent<UIController> ().card = card;
        vCard.Anchor.name = "CardInCollection";
    }

    static public void LoadRandomSet () {
        hand = new HandClass ();
        hand.GenerateRandomHand (cardPool);
        LoadSet (hand);
        LoadPage (Page);
    }

    static public void LoadSet (string [] lines, string name, int iconNumber) {
        hand = new HandClass ();
        hand.LoadFromString (lines);
        LoadSet (hand);
        LoadPage (Page);
        SetEditor.setName = name;
        SetEditor.iconNumber = iconNumber;
    }

    static public void RemoveCardFromCollection (int number) {
        int MaxX = 4;
        for (int x = 0; x < MaxX; x++) {
            for (int y = 0; y < 5; y++) {
                int number2 = y * MaxX + x + Page * PageCount;
                if (number2 == number) {
                    if (Collection [x, y] != null) {
                        Collection [x, y].DestroyVisual ();
                        Collection [x, y] = null;
                    }
                    CollectionCollider [x, y].GetComponent<UIController> ().card = null;
                }
            }
        }
        available [number] = false;
    }

    static public void RotateCardInCollection (int x, int y) {
        CardClass card = cardPool.Card [x + y * 4];
        card.RotateArea ();
        Collection [x, y].SetState (card);
    }

    static public void RotateCardInSet (int x, int y) {
        CardClass card = hand.GetCard (x, y);
        CardClass card2 = cardPool.Card [card.cardNumber];
        card.RotateArea ();
        card2.RotateArea ();
        Set [x, y].SetState (card);
    }


    static public void RemoveCardFromSet (int x, int y) {
        if (Set [x, y] != null) {
            Set [x, y].DestroyVisual ();
        }
        CardClass card = hand.GetCard (x, y);
        if (card == null) {
            return;
        }
        int number = card.cardNumber;
        hand.RemoveCard (x, y);
        available [number] = true;
        LoadCardInCollection (number);
        SetCollider [x, y].GetComponent<UIController> ().card = null;
    }

    static public void LoadSet (HandClass hand) {
        SetAllAvailable ();
        int MaxX = 4;
        for (int x = 0; x < MaxX; x++) {
            StackClass stack = hand.stack [x];
            for (int y = 0; y < 5; y++) {
                if (Set [x, y] != null) {
                    Set [x, y].DestroyVisual ();
                }
                if (y < stack.card.Count) {
                    CardClass card = stack.card [y];
                    if (card != null) {
                        LoadCardInSet (x, y, card);
                    }
                }
            }
        }
    }

    static public CardClass GetSelectedCard () {
        if (SelectedCollectionX != -1) {
            return cardPool.Card [SelectedCollectionX + SelectedCollectionY * 4 + Page * PageCount];
        } else {
            return null;
        }
    }

    static public void LoadCardInSet (int x, int y) {
        CardClass selected = GetSelectedCard ();
        RemoveCardFromSet (x, y);
        if (selected != null) {
            LoadCardInSet (x, y, GetSelectedCard ());
            SelectedCollectionX = -1;
        } else {
            //RemoveCardFromSet (x, y);
        }
    }

    static public void LoadCardInSet (int x, int y, CardClass card) {
        if (!available [card.cardNumber]) {
            return;
        }
        hand.SetCard (x, y, card);
        VisualCard vCard = LoadCard (card);
        Set [x, y] = vCard;
        SetInPixPosition (vCard.Anchor, 960 + 120 * x, 220 + 156 * y, 12);
        SetCollider [x, y].GetComponent<UIController> ().card = card;
        RemoveCardFromCollection (card.cardNumber);
    }

    static public VisualCard LoadCard (CardClass card) {
        VisualCard vCard = new VisualCard (card);
        vCard.Anchor.transform.SetParent (CurrentCanvas.transform);
        vCard.Anchor.transform.localEulerAngles = new Vector3 (-90, 0, 0);
        vCard.Anchor.transform.localScale = Vector3.one * 0.14f;
        DestroyImmediate (vCard.Background.GetComponent<Collider> ());
        return vCard;
    }
    
    static public void SaveSet () {
        ClientLogic.MyInterface.CmdSavePlayerModeSet (hand.HandToString (), setId);
    }


    // Use this for initialization
    void CreateCardPoolEditorMenu () {
        GameObject Clone;

        Clone = CreateSprite ("UI/Panel_Window_01_Sliced", 300, 540, 10, 600, 1080, false);

        Clone = CreateUIText (Language.AvailableCardPool, 165, 100);
        Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;
        Clone.GetComponent<Text> ().fontSize = 36;
        Clone.GetComponent<RectTransform> ().sizeDelta = new Vector2 (200, 200);

        Clone = CreateSprite ("UI/Panel_Window_01_Sliced", 1140, 540, 10, 600, 1080, false);

        Clone = CreateUIText (Language.YourCardSet, 1005, 100);
        Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;
        Clone.GetComponent<Text> ().fontSize = 36;
        Clone.GetComponent<RectTransform> ().sizeDelta = new Vector2 (200, 200);


        Clone = CreateSprite ("UI/Butt_S_Help", 990 + 60 * 4, 90, 11, 64, 64, true);
        Clone.name = UIString.SetEditorAbout;
        Clone = CreateSprite ("UI/Butt_S_Name", 990 + 60 * 5, 90, 11, 64, 64, true);
        Clone.name = UIString.SetEditorChangeSetProperties;
        Clone = CreateSprite ("UI/Butt_S_SetRandomize", 990 + 60 * 6, 90, 11, 64, 64, true);
        Clone.name = UIString.SetEditorGenerateRandomSet;
        /*Clone = CreateSprite ("UI/Butt_S_SetList", 990 + 60 * 6, 90, 11, 64, 64, true);
        Clone.name = UIString.SetEditorShowSetList;*/

        Clone = CreateSprite ("UI/Shadow_Butt_M_Rectangle_Sliced", 660, 960, 10, 120, 120, false);
        Clone = CreateSprite ("UI/Shadow_Butt_M_Rectangle_Sliced", 660 + 120, 960, 10, 120, 120, false);

        Clone = CreateSprite ("UI/Butt_M_Apply", 660, 960, 11, 90, 90, true);
        Clone.name = UIString.SetEditorSaveSet;
        Clone = CreateSprite ("UI/Butt_M_Discard", 660 + 120, 960, 11, 90, 90, true);
        Clone.name = UIString.ShowMainMenu;

        for (int x = 0; x < 4; x++) {

            Clone = CreateSprite ("UI/Panel_Slot_01_SetRow", 960 + 120 * x, 535, 11, 122, 780, false);

            Clone = CreateText (x.ToString (), 960 + 120 * x, 990, 11, 0.03f);

            for (int y = 0; y < 5; y++) {
                UIController UIC;
                Clone = CreateSprite ("UI/Panel_Slot_01_CollectionCard", 120 + 120 * x, 225 + 156 * y, 11, 120, 150, false);

                Clone =
                    CreateSprite ("UI/Panel_Slot_01_CollectionCard", 120 + 120 * x, 225 + 156 * y, 12, 120, 150, false);
                DestroyImmediate (Clone.GetComponent<SpriteRenderer> ());
                Clone.name = UIString.SetEditorCollectionCard;
                UIC = Clone.GetComponent<UIController> ();
                UIC.x = x;
                UIC.y = y;
                CollectionCollider [x, y] = Clone;



                Clone =
                    CreateSprite ("UI/Panel_Slot_01_CollectionCard", 960 + 120 * x, 225 + 156 * y, 12, 120, 150, false);
                DestroyImmediate (Clone.GetComponent<SpriteRenderer> ());
                Clone.name = UIString.SetEditorSetCard;
                UIC = Clone.GetComponent<UIController> ();
                UIC.x = x;
                UIC.y = y;
                SetCollider [x, y] = Clone;
            }

        }
    }

    static public void LoadPageUI () {
        GameObject Clone;
        GameObject Background;
        int pageLimit = (cardPool.Card.Count - 1) / PageCount + 1;
        PageButton = new GameObject [pageLimit];
        if (pageLimit > 1) {
            for (int x = 0; x < pageLimit; x++) {
                Background = CreateSprite ("UI/Butt_M_EmptyRect_Sliced", 90 + 60 * x, 990, 11, 60, 60, true);
                Background.GetComponent<UIController> ().number = x;
                Background.name = "SetEditorPageButton";
                Clone = CreateText ((x + 1).ToString (), 90 + 60 * x, 990, 12, 0.03f);
                AddTextToGameObject (Background, Clone);
                PageButton [x] = Background;
            }
        }
    }
        
}
