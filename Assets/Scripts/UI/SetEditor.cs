using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SetEditor : GOUI {
    
    static public HandClass hand;

    static CardPoolClass cardPool;
    static bool [] available;

    static bool [] unlockedAbilities;
    static bool [] unlockedTokens;

    static public List<int> filteredCard = new List<int> ();

    static VisualCard [,] Collection;
    static VisualCard [,] Set;

    static GameObject [,] CollectionCollider;
    static GameObject [,] SetCollider;

    static public GameObject pageUIObject;
    static public PageUI pageUI;

    static public int SelectedCollectionX = -1;
    static public int SelectedCollectionY;

    static public int numberOfPages;
    static public int currentPage;
    static public int PageCount = 20;

    static public int setId;

    static public string setName;
    static public int iconNumber;

    static public bool usedCardsArePutOnBottomOfStack;
    static public int numberOfStacks;
    static public int minimumNumberOfCardsOnStack;

    static public void ApplySetProperties (string setName, int iconNumber) {
        SetEditor.setName = setName;
        SetEditor.iconNumber = iconNumber;
        ClientLogic.MyInterface.CmdSaveSetProperties (setId, setName, iconNumber);
    }

    private void Start () {
        CurrentGOUI = this;
        ClientLogic.MyInterface.CmdDownloadDataToSetEditor (setId);
        DestroyTemplateButtons ();
    }

    static public void ShowSetEditorMenu (int setId) {
        SetEditor.setId = setId;
        DestroyMenu ();
        CurrentCanvas.AddComponent<SetEditor> ();
    }

    static public void LoadData (string [] cardPool, bool [] unlockedAbilities, bool [] unlockedTokens, string [] set, string name, int iconNumber, bool usedCardsArePutOnBottomOfStack, int numberOfStacks, int minimalNumberOfCardsOnStack) {


        for (int x = 0; x < 4; x++) {
            int MaxNumber = 0;
            switch (x) {
                case 0:
                    MaxNumber = 9;
                    break;
                case 1:
                    MaxNumber = AppDefaults.AvailableTokens + 1;
                    break;
                case 2:
                    MaxNumber = AppDefaults.AvailableAbilities + 1;
                    break;
                case 3:
                    MaxNumber = 4;
                    break;
            }
            filter [x] = new bool [MaxNumber];
            filter [x] [0] = true;
        }

        SetEditor.unlockedAbilities = unlockedAbilities;
        SetEditor.unlockedTokens = unlockedTokens;

        SetEditor.numberOfStacks = numberOfStacks;
        SetEditor.minimumNumberOfCardsOnStack = minimalNumberOfCardsOnStack;
        SetEditor.usedCardsArePutOnBottomOfStack = usedCardsArePutOnBottomOfStack;
        LoadCardPool (cardPool);
        CreateCardPoolEditorMenu ();
        FilterCards ();
        LoadSet (set, name, iconNumber);

    }

    static public void LoadCardPool (string [] s) {
        cardPool = new CardPoolClass ();
        cardPool.LoadFromString (s);
        available = new bool [cardPool.Card.Count];
        SetAllAvailable ();
    }

    static public void SetAllAvailable () {
        for (int x = 0; x < available.Length; x++) {
            available [x] = true;
        }
    }
    static public void LoadPage () {
        LoadPage (Mathf.Min (currentPage, numberOfPages - 1));
    }

    static public void LoadPage (int page) {
        SelectedCollectionX = -1;
        currentPage = pageUI.SelectPage (page);
        int MaxX = 4;
        for (int x = 0; x < MaxX; x++) {
            for (int y = 0; y < 5; y++) {
                if (Collection [x, y] != null) {
                    Collection [x, y].DestroyVisual ();
                    Collection [x, y] = null;
                    CollectionCollider [x, y].GetComponent<Collider> ().enabled = false;
                }
                int number = y * MaxX + x + page * PageCount;
                if (number < filteredCard.Count) {
                    int cardNumber = filteredCard [number];
                    if (available [cardNumber]) {
                        LoadCardInCollection (x, y, cardNumber);
                    }
                }
            }
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
        bool cardExists = false;
        int filteredPosition = 0;
        for (int x = 0; x < filteredCard.Count; x++) {
            if (filteredCard [x] == number) {
                cardExists = true;
                filteredPosition = x;
                break;
            }
        }
        if (!cardExists) {
            return;
        }
        int MaxX = 4;
        for (int x = 0; x < MaxX; x++) {
            for (int y = 0; y < 5; y++) {
                if (filteredPosition == y * MaxX + x + currentPage * PageCount) {
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
        CollectionCollider [x, y].GetComponent<Collider> ().enabled = true;
        vCard.Anchor.name = "CardInCollection";
    }

    static public void LoadRandomSet () {
        hand = new HandClass ();
        hand.GenerateRandomHand (cardPool, null, null, usedCardsArePutOnBottomOfStack, numberOfStacks, minimumNumberOfCardsOnStack);
        LoadSet (hand);
        LoadPage (currentPage);
    }

    static public void LoadSet (string [] lines, string name, int iconNumber) {
        hand = new HandClass ();
        hand.LoadFromFileString (cardPool, lines, numberOfStacks);
        LoadSet (hand);
        LoadPage (currentPage);
        SetEditor.setName = name;
        SetEditor.iconNumber = iconNumber;
    }

    override public void ShowPropertiesMenu () {
        PropertiesMenu.ShowSetPropertiesMenu (PropertiesMenu.setPropertiesKey, setName, iconNumber);
    }

    static public void RemoveCardFromCollection (int number) {
        bool cardExists = false;
        int filteredPosition = 0;
        for (int x = 0; x < filteredCard.Count; x++) {
            if (filteredCard [x] == number) {
                cardExists = true;
                filteredPosition = x;
                break;
            }
        }
        if (!cardExists) {
            return;
        }
        int MaxX = 4;
        for (int x = 0; x < MaxX; x++) {
            for (int y = 0; y < 5; y++) {
                int number2 = y * MaxX + x + currentPage * PageCount;
                if (number2 == filteredPosition) {
                    if (Collection [x, y] != null) {
                        Collection [x, y].DestroyVisual ();
                        Collection [x, y] = null;
                    }
                    CollectionCollider [x, y].GetComponent<UIController> ().card = null;
                    CollectionCollider [x, y].GetComponent<Collider> ().enabled = false;
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
        //LoadPage ();
        SetCollider [x, y].GetComponent<UIController> ().card = null;
    }

    static public void LoadSet (HandClass hand) {
        SetAllAvailable ();
        for (int x = 0; x < numberOfStacks; x++) {
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
            return cardPool.Card [filteredCard [SelectedCollectionX + SelectedCollectionY * 4 + currentPage * PageCount]];
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
        Transform transform = vCard.Anchor.transform;
        transform.SetParent (CurrentCanvas.transform);
        transform.localEulerAngles = new Vector3 (-90, 0, 0);
        transform.localScale = Vector3.one * 0.14f;
        DestroyImmediate (vCard.Background.GetComponent<Collider> ());
        /*
        GameObject Clone = CreateSprite ("Textures/Other/Lock", 0, 0, 12, 40, 40, false);
        Clone.transform.SetParent (transform);
        Clone.transform.localPosition = new Vector3 (0, 0, -1);
        Destroy (Clone.GetComponent<Collider> ());*/
        return vCard;
    }
    
    static public void SaveSet () {
        ClientLogic.MyInterface.CmdSavePlayerModeSet (hand.HandToString (), setId);
    }

    static GameObject FilterMenuBackground;
    static UIController [] FilterMenuButton = new UIController [4];
    static int currentFilterMenu = -1;
    static FilterButtonClass [] FilterButton;
    static bool [] [] filter = new bool [4] [];

    static public void ShowFilterMenu (int number) {
        for (int x = 0; x < 4; x++) {
            FilterMenuButton [x].FreeAndUnlcok ();
        }
        if (currentFilterMenu == number) {
            if (FilterMenuBackground != null) {
                DestroyImmediate (FilterMenuBackground);
            }
            currentFilterMenu = -1;
        } else {
            FilterMenuButton [number].PressAndLock ();
            currentFilterMenu = number;
            ShowFilterMenu ();
        }
    }

    public class FilterButtonClass {

        GameObject Background;
        GameObject Text;
        GameObject Icon;
        int number;

        public FilterButtonClass () {

        }

        public FilterButtonClass (Transform parent, int px, int py, int number, bool selected, float scale) {
            GameObject Clone;
            GameObject Text;
            UIController UIC;

            this.number = number;
            int alteredNumber = number - 1;
            Clone = CreateSpriteWithText ("UI/Butt_M_EmptySquare", "", px, py, 11, (int) (45 * scale), (int) (45 * scale), 0.025f);
            Text = Clone.transform.Find ("Text").gameObject;
            Clone.transform.parent = parent;
            Background = Clone;
            //Clone = GameObject.CreatePrimitive (PrimitiveType.Quad);
            if (number == 0) {
                Clone.GetComponent<UIController> ().number = 0;
                Text.GetComponent<TextMesh> ().text = "All";
                Text.GetComponent<Renderer> ().material.color = Color.black;
            } else if (currentFilterMenu == 0) {
                Text.GetComponent<TextMesh> ().text = number.ToString ();
                Text.GetComponent<Renderer> ().material.color = Color.black;
            }
            SelectFilter (selected);
            Clone.GetComponent<Renderer> ().material.shader = Shader.Find ("Sprites/Default");
            Background.name = UIString.SetEditorFilterButton;
            UIC = Background.GetComponent<UIController> ();
            UIC.number = number;
            if (number > 0 && currentFilterMenu != 0) {
                switch (currentFilterMenu) {
                    case 1:
                        UIC.tokenType = alteredNumber;
                        if (unlockedTokens [alteredNumber]) {
                            VisualToken VT = new VisualToken ();
                            Clone = VT.Anchor;
                            Clone.transform.SetParent (Background.transform);
                            Clone.transform.localEulerAngles = new Vector3 (-90, 0, 0);
                            Clone.transform.localPosition = Vector3.zero;
                            Clone.transform.localScale = new Vector3 (0.4f, 0.4f, 0.4f);
                            VT.SetType (alteredNumber);
                            DestroyImmediate (VT.Text);
                        } else {
                            Clone = CreateSprite ("Textures/Other/Lock", px, py, 12, 40, 40, false);
                            Clone.transform.SetParent (Background.transform);
                            Destroy (Clone.GetComponent<Collider> ());
                        }
                        break;
                    case 2:
                        UIC.abilityType = alteredNumber;
                        if (unlockedAbilities [alteredNumber]) {
                            Clone = CreateSprite (VisualCard.GetIconPath (alteredNumber), px, py, 12, 40, 40, false);
                            Clone.GetComponent<Renderer> ().material.color = AppDefaults.GetAbilityColor (alteredNumber);
                            Clone.transform.SetParent (Background.transform);
                            Destroy (Clone.GetComponent<Collider> ());
                        } else {
                            Clone = CreateSprite ("Textures/Other/Lock", px, py, 12, 40, 40, false);
                            Clone.transform.SetParent (Background.transform);
                            Destroy (Clone.GetComponent<Collider> ());
                        }
                        break;
                    case 3:
                        VisualArea area = new VisualArea ();
                        area.Anchor.transform.SetParent (Background.transform);
                        area.Anchor.transform.localPosition = new Vector3 (0, 0, 0);
                        area.Anchor.transform.localScale = new Vector3 (0.6f, 0.6f, 0.6f);
                        area.Anchor.transform.localEulerAngles = new Vector3 (-90, 0, 0);
                        switch (number) {
                            case 1:
                                area.SetAbilityArea (0);
                                break;
                            case 2:
                                area.SetAbilityArea (1);
                                break;
                            case 3:
                                area.SetAbilityArea (4);
                                break;
                        }
                        break;
                }
            } else {
            }
            Icon = Clone;
        }

        public void SelectFilter (bool selected) {
            if (selected) {
                Background.GetComponent<UIController> ().PressAndLock ();
            } else {
                Background.GetComponent<UIController> ().FreeAndUnlcok ();
            }
            /*
            if (selected) {
                Background.GetComponent<UIController> ().PressAndLock ();
                Background.GetComponent<Renderer> ().material.color = Color.white;
                if (Text != null) {
                    Text.GetComponent<Renderer> ().material.color = Color.black;
                }
            } else {
                Background.GetComponent<Renderer> ().material.color = new Color (0.3f, 0.3f, 0.3f);
                if (Text != null) {
                    Text.GetComponent<Renderer> ().material.color = Color.white;
                }
            }*/
            //Background.GetComponent<Renderer> ().enabled = selected;
        }
    }

    static public void SelectFilterButton (int number) {
        bool value = !filter [currentFilterMenu] [number];
        int count = filter [currentFilterMenu].Length;
        if (number == 0) {
            filter [currentFilterMenu] [0] = value;
            if (value) {
                for (int x = 1; x < count; x++) {
                    filter [currentFilterMenu] [x] = false;
                    FilterButton [x].SelectFilter (false);
                }
            }
        } else {
            filter [currentFilterMenu] [0] = false;
            FilterButton [0].SelectFilter (false);
            filter [currentFilterMenu] [number] = value;
            bool anySelected = false;
            for (int x = 1; x < count; x++) {
                if (filter [currentFilterMenu] [x]) {
                    anySelected = true;
                }
            }
            if (!anySelected) {
                filter [currentFilterMenu] [0] = true;
                FilterButton [0].SelectFilter (true);
            }
        }
        FilterButton [number].SelectFilter (value);
        FilterCards ();
    }

    static public void FilterCards () {
        filteredCard = new List<int> ();
        List<CardClass> cards = cardPool.Card;
        int count = cards.Count;
        for (int x = 0; x < count; x++) {
            CardClass card = cards [x];
            if ((filter [0] [0] || filter [0] [card.tokenValue]) &&
                (filter [1] [0] || filter [1] [card.tokenType + 1]) && unlockedTokens [card.tokenType] &&
                (filter [2] [0] || filter [2] [card.abilityType + 1]) && unlockedAbilities [card.abilityType] &&
                (filter [3] [0] || filter [3] [card.AreaSize () + 1])) {
                filteredCard.Add (x);
            }
        }
        LoadPageUI ();
        LoadPage ();
    }

    static public void ShowFilterMenu () {
        GameObject Clone;

        if (FilterMenuBackground != null) {
            DestroyImmediate (FilterMenuBackground);
        }

        int scale = 1;
        if (currentFilterMenu == 3) {
            scale = 2;
        }

        int MaxNumber = filter [currentFilterMenu].Length;

        float XCount = 4 / scale;
        float YCount = (int) ((MaxNumber - 1) / XCount + 1);

        Clone = CreateSprite ("UI/Panel_Window_01_Sliced", 720, 60 + (int) (22.5f * YCount * scale), 10, 300, 120 + (int) (45 * YCount * scale), false);

        GameObject BG = Clone;
        FilterMenuBackground = BG;

        FilterButton = new FilterButtonClass [(int) (YCount * XCount)];
        for (int y = 0; y < YCount; y++) {
            for (int x = 0; x < XCount; x++) {
                int number = y * (int) XCount + x;
                if (number < MaxNumber) {
                    FilterButton [number] = new FilterButtonClass (BG.transform,
                        (int) (630 + (x + 0.5f) * 45 * scale),
                        (int) (60 + (y + 0.5f) * 45 * scale),
                        number,
                        filter [currentFilterMenu] [number],
                        scale);
                }
            }
        }
    }


    // Use this for initialization
    static void CreateCardPoolEditorMenu () {
        GameObject Clone;
        UIController UIC;
        Collection = new VisualCard [4, 5];
        CollectionCollider = new GameObject [4, 5];
        Set = new VisualCard [numberOfStacks, 5];
        SetCollider = new GameObject [numberOfStacks, 5];

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

        Clone = CreateSprite ("UI/Butt_S_Value", 90 + 60 * 4, 90, 11, 64, 64, true);
        Clone.name = UIString.SetEditorFilterMenu;
        UIC = Clone.GetComponent<UIController> ();
        UIC.number = 0;
        FilterMenuButton [0] = UIC;
        Clone = CreateSprite ("UI/Butt_S_Type", 90 + 60 * 5, 90, 11, 64, 64, true);
        Clone.name = UIString.SetEditorFilterMenu;
        UIC = Clone.GetComponent<UIController> ();
        UIC.number = 1;
        FilterMenuButton [1] = UIC;
        Clone = CreateSprite ("UI/Butt_S_Ability", 90 + 60 * 6, 90, 11, 64, 64, true);
        Clone.name = UIString.SetEditorFilterMenu;
        UIC = Clone.GetComponent<UIController> ();
        UIC.number = 2;
        FilterMenuButton [2] = UIC;
        Clone = CreateSprite ("UI/Butt_S_Area", 90 + 60 * 7, 90, 11, 64, 64, true);
        Clone.name = UIString.SetEditorFilterMenu;
        UIC = Clone.GetComponent<UIController> ();
        UIC.number = 3;
        FilterMenuButton [3] = UIC;


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

            if (x < numberOfStacks) {
                Clone = CreateSprite ("UI/Panel_Slot_01_SetRow", 960 + 120 * x, 535, 11, 122, 780, false);

                Clone = CreateText ((x + 1).ToString (), 960 + 120 * x, 990, 11, 0.03f);
            }

            for (int y = 0; y < 5; y++) {
                Clone = CreateSprite ("UI/Panel_Slot_01_CollectionCard", 120 + 120 * x, 225 + 156 * y, 11, 120, 150, false);

                Clone =
                    CreateSprite ("UI/Panel_Slot_01_CollectionCard", 120 + 120 * x, 225 + 156 * y, 12, 120, 150, false);
                DestroyImmediate (Clone.GetComponent<SpriteRenderer> ());
                Clone.name = UIString.SetEditorCollectionCard;
                UIC = Clone.GetComponent<UIController> ();
                UIC.x = x;
                UIC.y = y;
                CollectionCollider [x, y] = Clone;



                if (x < numberOfStacks) {
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
    }

    static public void LoadPageUI () {
        if (pageUI != null) {
            pageUI.DestroyButtons ();
        }
        numberOfPages = (filteredCard.Count - 1) / PageCount + 1;
        pageUIObject = CurrentGOUI.gameObject;
        pageUI = pageUIObject.AddComponent<PageUI> ();
        pageUI.Init (8, numberOfPages, new Vector2Int (90, 990), UIString.SetEditorPageButton);
    }
        
}
