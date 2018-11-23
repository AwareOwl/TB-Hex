using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetEditor : GOUI {

    static public SetEditor instance;


    //public GameObject PageText;
    public int Page;
    public int SelectedCardInCollection = -1;

    public int TopLeftCardInCollection = 0;
    static public float CardScale = 1.5f;


    public List<int> FilteredCards;
    public int FilteredCardsCount;

    public bool [] [] Filters; // Value, TokenType, AbilityType, AbilityRange

    public BasicCardQueue [,] CardQueue = new BasicCardQueue [4, 5];
    public BasicCardInCollection [] CardInCollection = new BasicCardInCollection [20];
    public bool [] CardAvailable = new bool [300];

    public struct BasicCardQueue {
        public int Number;
        public bool Empty;
        public GameObject Card;
        public GameObject Cover;
    }

    public struct BasicCardInCollection {
        public GameObject Card;
        public GameObject Cover;
    }

    private void Start () {
        instance = this;
        CreateCardPoolEditorMenu ();
        CurrentGUI = this;
    }

    static public void ShowSetEditorMenu () {
        DestroyMenu ();
        CurrentCanvas.AddComponent<SetEditor> ();
    }

    //public BasicPageButton [] PageButton = new BasicPageButton [8];

    /*
    public void SetFilters () {
        Filters = new bool [4] [];
        Filters [0] = new bool [8];
        Filters [1] = new bool [AppDefaults.AvailableTokens];
        Filters [2] = new bool [AppDefaults.AvailableAbilities];
        Filters [3] = new bool [9];
        for (int x = 0; x < 4; x++) {
            Filters [x] [0] = true;
        }
    }

    public void SetFilter (int filterType, int filterNumber, bool enabled) {
        Filters [filterType] [filterNumber] = enabled;
        FilterCards ();
        SelectPage ();
    }

    public void FilterCards () {
        FilteredCards = new List<int> ();
        for (int x = 0; x < GameData.CardExisting; x++) {
            if ((Filters [0] [0] || Filters [0] [GameData.Card [x].TokenValue]) &&
                (Filters [1] [0] || Filters [1] [GameData.Card [x].TokenType + 1]) &&
                (Filters [2] [0] || Filters [2] [GameData.Card [x].AbilityType + 1]) &&
                (Filters [3] [0] || Filters [3] [GameData.Card [x].AbilityArea + 1])) {
                FilteredCards.Add (x);
                //Debug.Log (x);
            }
        }
        FilteredCardsCount = FilteredCards.Count;
    }

    public void SelectPage () {
        SelectPage (Page);
    }

    public void SelectPage (bool RefreshTopLeftCard) {
        SelectPage (Page, RefreshTopLeftCard);
    }

    public int PagesCount () {
        return (FilteredCardsCount - 1) / 20 + 1;
    }
    public void SelectPage (int number) {
        for (int x = 0; x < FilteredCards.Count; x++) {
            Page = x / 20;
            if (FilteredCards [x] >= TopLeftCardInCollection) {
                break;
            }
        }
        SelectPage (Page, false);
    }

    public void SelectPage (int number, bool RefreshTopLeftCard) {
        int PageLimit = PagesCount ();
        if (number < PageLimit) {
            Page = ButtonNumberToPageNumber (number);
        }
        if (RefreshTopLeftCard) {
            TopLeftCardInCollection = FilteredCards [Page * 20];
        }
        for (int x = PageLimit; x < PageButton.Length; x++) {
            PageButton [x].Disable ();
        }
        for (int x = 0; x < PageLimit && x < PageButton.Length; x++) {
            PageButton [x].SelectButton (false);
        }
        for (int x = 0; x < PageButton.Length; x++) {
            if (x < PageLimit) {
                int thisNumber = ButtonNumberToPageNumber (x);
                PageButton [x].SetNumber (thisNumber + 1);
                if (thisNumber == Page) {
                    PageButton [x].SelectButton (true);
                }
            } else {
                PageButton [x].SetNumber (x + 1);
            }
        }
        RefreshPage ();
    }

    public int ButtonNumberToPageNumber (int number) {
        int PageLimit = PagesCount ();
        if (number == 0) {
            return 0;
        } else if (number == PageButton.Length - 1) {
            return Mathf.Max (PageLimit - 1, 7);
        } else {
            return Mathf.Max (number, Mathf.Min (PageLimit - 1, Mathf.Min (number + Page - 3, PageLimit + number - PageButton.Length)));
        }
    }

    public void RefreshPage () {
        int MaxX = 4;
        //	EScript.PageText.GetComponent<TextMesh> ().text = "Page " + (page + 1).ToString () + " of " + 
        //	((GameData.CardExisting - 1) / 20 + 1).ToString ();
        for (int x = 0; x < MaxX; x++) {
            for (int y = 0; y < 5; y++) {
                ELoadCardInCollection (x, y);
            }
        }
        SelectedCardInCollection = -1;
    }
    */


    public void ELoadCardInCollection (int x, int y) {
        int MaxX = 4;
        int OnPageNumber = x + y * MaxX;
        int CardNumber = OnPageNumber + 20 * Page;
        int FilteredCardNumber = -1;
        if (CardNumber < FilteredCardsCount) {
            FilteredCardNumber = FilteredCards [CardNumber];
        }
        /*if (CardNumber < FilteredCardsCount && CardAvailable [FilteredCardNumber]) {
            GameData.SetCard (CardInCollection [OnPageNumber].Card, FilteredCardNumber);
            GameData.SetCover (CardInCollection [OnPageNumber].Card, false);
        } else {
            GameData.SetCover (CardInCollection [OnPageNumber].Card, true);
        }*/

    }

    /*
    public class BasicPageButton {
        public GameObject Background;
        public TextMesh Text;

        public BasicPageButton () {
        }

        public BasicPageButton (Transform parent, Vector3 position, int number) {
            GameObject Clone;

            Clone = CreateSprite ("UI/Butt_S_EmptySquare");
            Clone.transform.parent = parent.transform;
            SetSpriteScale (Clone, 60, 60);
            SetInPixPosition (Clone, 90 + 60 * number, 990, -0.002f);
            Clone.GetComponent<ControlScript> ().number = number;
            Clone.name = "SelectPageInCollection";
            Background = Clone;

            Clone = Instantiate (Resources.Load ("PreText")) as GameObject;
            Clone.transform.parent = Background.transform;
            Clone.transform.localScale = Vector3.one * 0.3f;
            Clone.transform.localPosition = new Vector3 (0, 0, -0.001f);
            Text = Clone.GetComponent<TextMesh> ();

            SetNumber (number + 1);
        }

        public void SelectButton (bool selected) {
            if (selected) {
                SetSprite (Background, "UI/Butt_S_EmptySquare_P");
                Text.GetComponent<Renderer> ().material.color = Color.white;
                //	Background.GetComponent<Renderer> ().material.color = Color.white;
            } else {
                SetSprite (Background, "UI/Butt_S_EmptySquare");
                Text.GetComponent<Renderer> ().material.color = Color.black;
                //	Background.GetComponent<Renderer> ().material.color = new Color (0.3f, 0.3f, 0.3f);
            }
        }

        public void Disable () {
            //		Background.GetComponent<Renderer> ().material.color = new Color (0.3f, 0.3f, 0.3f);
            Text.GetComponent<Renderer> ().material.color = new Color (0.5f, 0.5f, 0.5f);
        }

        public void SetNumber (int number) {
            Text.GetComponent<TextMesh> ().text = number.ToString ();
        }
    }
    */

    // Use this for initialization
    void CreateCardPoolEditorMenu () {

        CardScale = 10f * 140 / 1080;
        /*SetFilters ();
        FilterCards ();
        for (int x = 0; x < GameData.CardExisting; x++) {
            CardAvailable [x] = true;
        }*/
        GameObject Clone;

        Clone = CreateSprite ("UI/Panel_Window_01_Sliced", 300, 540, 10, 600, 1080, false);

        Clone = CreateText (Language.AvailableCardPool, 60, 60, 11, 0.04f);
        Clone.GetComponent<TextMesh> ().anchor = TextAnchor.UpperLeft;

        Clone = CreateSprite ("UI/Panel_Window_01_Sliced", 1140, 540, 10, 600, 1080, false);

        Clone = CreateText (Language.YourCardSet, 900, 60, 11, 0.04f);
        Clone.GetComponent<TextMesh> ().anchor = TextAnchor.UpperLeft;

        /*
        GameObject [] Buttons = new GameObject [9];

        for (int x = 0; x < 7; x++) {
            Clone = CreateSprite ("UI/Butt_S_EmptySquare");
            Clone.transform.parent = transform;
            SetSpriteScale (Clone, 64, 64);
            SetInPixPosition (Clone, 330 + 60 * x, 90, -0.002f);
            Clone.GetComponent<ControlScript> ().number = x;
            if (x > 3) {
                SetInPixPosition (Clone, 990 + 60 * x, 90, -0.002f);
            }
            Buttons [x] = Clone;
        }

        // Back to menu

        for (int x = 7; x < 9; x++) {
            Clone = CreateSprite ("UI/Shadow_Butt_M_Rectangle_Sliced");
            Clone.transform.parent = transform;
            SetSpriteScale (Clone, 120, 120);
            SetInPixPosition (Clone, 660 + 120 * (x - 7), 960, -0.001f);
            Clone = CreateSprite ("UI/Butt_S_EmptySquare");
            Clone.transform.parent = transform;
            SetSpriteScale (Clone, 90, 90);
            SetInPixPosition (Clone, 660 + 120 * (x - 7), 960, -0.002f);
            Buttons [x] = Clone;
        }

        //	Buttons [0].GetComponent<Renderer> ().material.mainTexture = Resources.Load ("Textures/Ok") as Texture;
        Buttons [0].name = "ShowFilterList";
        SetSprite (Buttons [0], "UI/Butt_S_Value", true);
        Buttons [1].name = "ShowFilterList";
        SetSprite (Buttons [1], "UI/Butt_S_Type", true);
        Buttons [2].name = "ShowFilterList";
        SetSprite (Buttons [2], "UI/Butt_S_Ability", true);
        Buttons [3].name = "ShowFilterList";
        SetSprite (Buttons [3], "UI/Butt_S_Area", true);
        Buttons [4].name = "RenameSet";
        SetSprite (Buttons [4], "UI/Butt_S_Name", true);
        Buttons [5].name = "GenerateRandomSet";
        SetSprite (Buttons [5], "UI/Butt_S_SetRandomize", true);
        Buttons [6].name = "ListOfSets";
        SetSprite (Buttons [6], "UI/Butt_S_SetList", true);
        Buttons [7].name = "SaveDeck";
        SetSprite (Buttons [7], "UI/Butt_M_Apply", true);
        Buttons [8].name = "BackToMainMenu";
        SetSprite (Buttons [8], "UI/Butt_M_Discard", true);

        for (int x = 0; x < PageButton.Length; x++) {
            PageButton [x] = new BasicPageButton (transform, new Vector3 (-6.275f + 0.5f * x, -4.3f, -0.001f), x);

            if (x == 0) {
                PageButton [x].SelectButton (true);
            } else {
                PageButton [x].SelectButton (false);
            }
        }*/


        //
        for (int x = 0; x < 4; x++) {
            // Pas
            /*
            Clone = CreateSprite ("UI/Panel_Slot_01_SetRow");
            Clone.transform.parent = transform;
            SetSpriteScale (Clone, 122, 780);
            SetInPixPosition (Clone, 960 + 120 * x, 540, 2);


            // Numer pasa
            Clone = Instantiate (Resources.Load ("PreText")) as GameObject;
            Clone.GetComponent<TextMesh> ().text = (x + 1).ToString ();
            Clone.GetComponent<Renderer> ().material.color = Color.black;
            Clone.transform.parent = transform;
            Clone.transform.localScale = new Vector3 (0.325f, 0.325f, 1);
            SetInPixPosition (Clone, 960 + 120 * x, 990, 2);*/

            /*
            for (int y = 0; y < 5; y++) {
                Clone = GameData.CreateCard ();
                Clone.transform.parent = transform;
                SetInPixPosition (Clone, 960 + 120 * x, 225 + 156 * y, 3);
                Clone.transform.localScale = new Vector3 (CardScale * GameData.CardSizeX, CardScale * GameData.CardSizeY, 1);
                Clone.AddComponent<ControlScript> ();
                Clone.GetComponent<ControlScript> ().x = x;
                Clone.GetComponent<ControlScript> ().y = y;
                Clone.name = "CardInDeck";
                GameData.HideCard (Clone);
                GameData.SetCover (Clone, true);
                CardQueue [x, y].Card = Clone;
                CardQueue [x, y].Cover = Clone;
                CardQueue [x, y].Empty = true;
            }*/
        }
        /*
        int MaxX = 4;
        for (int x = 0; x < MaxX; x++) {
            for (int y = 0; y < 5; y++) {
                Clone = GameData.CreateCard (x + y * MaxX);
                Clone.transform.parent = transform;
                SetInPixPosition (Clone, 120 + 120 * x, 225 + 156 * y, -0.003f);
                Clone.transform.localScale = new Vector3 (CardScale * GameData.CardSizeX, CardScale * GameData.CardSizeY, 1);
                Clone.AddComponent<ControlScript> ();
                Clone.GetComponent<ControlScript> ().number = x + y * MaxX;
                Clone.name = "CardCollection";
                CardInCollection [x + y * MaxX].Card = Clone;
                //Clone.GetComponent<Collider> ().enabled = false;

                Clone = CreateSprite ("UI/Panel_Slot_01_CollectionCard");
                Clone.transform.parent = transform;
                SetSpriteScale (Clone, 120, 150);
                SetInPixPosition (Clone, 120 + 120 * x, 225 + 156 * y, -0.002f);
                Clone.name = "Cover";
                CardInCollection [x + y * MaxX].Cover = Clone;

            }
        }

        SelectPage (0);*/
        //ShowFilters ();
    }

    /*
    public class BasicFilterButton {

        GameObject Background;
        GameObject Text;
        GameObject Icon;
        int number;

        public BasicFilterButton () {

        }

        public BasicFilterButton (Transform parent, Vector3 position, int number, bool selected, float scale) {
            GameObject Clone;

            this.number = number;

            Clone = GameObject.CreatePrimitive (PrimitiveType.Quad);
            Clone.GetComponent<Renderer> ().material.shader = Shader.Find ("Sprites/Default");
            Clone.GetComponent<Renderer> ().material.color = new Color (1f, 1f, 1);
            Clone.transform.localScale = new Vector3 (0.4f * scale, 0.4f * scale, 1);
            Clone.transform.localPosition = position;
            Clone.transform.parent = parent;
            Background = Clone;
            Clone = GameObject.CreatePrimitive (PrimitiveType.Quad);
            Clone.AddComponent<UIController> ().number = number;
            if (number == 0) {
                Clone.GetComponent<UIController> ().typeNumber = -1;
                GameObject Clone2 = Instantiate (Resources.Load ("PreText")) as GameObject;
                Clone2.GetComponent<TextMesh> ().text = "All";
                Clone2.GetComponent<Renderer> ().material.color = Color.black;
                Clone2.transform.parent = Clone.transform;
                Clone2.transform.localScale = Vector3.one * (1 - 0.1f * scale);
                Clone2.transform.localPosition = new Vector3 (0, 0, -0.0001f);
                Text = Clone2;
            } else if (FilterType == 0) {
                GameObject Clone2 = Instantiate (Resources.Load ("PreText")) as GameObject;
                Clone2.GetComponent<TextMesh> ().text = number.ToString ();
                Clone2.GetComponent<Renderer> ().material.color = Color.black;
                Clone2.transform.parent = Clone.transform;
                Clone2.transform.localScale = Vector3.one * (1 - 0.1f * scale);
                Clone2.transform.localPosition = new Vector3 (0, 0, -0.0001f);
                Text = Clone2;
            }
            SelectFilter (selected);
            Clone.GetComponent<Renderer> ().material.shader = Shader.Find ("Sprites/Default");
            Clone.transform.parent = Background.transform;
            Clone.transform.localScale = Vector3.one * (1 - 0.1f * scale);
            Clone.transform.localPosition = new Vector3 (0, 0, -0.0001f);
            if (number > 0 && FilterType != 0) {
                switch (FilterType) {
                    case 1:
                        GameData.ConvertToToken (Clone);
                        GameData.SetTokenType (Clone, number - 1);
                        break;
                    case 2:
                        GameData.ConvertToAbilityType (Clone);
                        GameData.SetAbilityType (Clone, number - 1);
                        break;
                    case 3:
                        Clone.GetComponent<Renderer> ().material.mainTexture = GameData.AbilityAreaTexture [number - 1];
                        break;
                }
            } else {
                Clone.GetComponent<Renderer> ().enabled = false;
            }
            switch (FilterType) {
                case 0:
                case 3:
                    Clone.name = "Filter";
                    break;
                case 1:
                    Clone.name = "FilterTokenType";
                    break;
                case 2:
                    Clone.name = "FilterAbilityType";
                    break;
            }
            Icon = Clone;
        }

        public void SelectFilter (bool selected) {
            if (selected) {
                Background.GetComponent<Renderer> ().material.color = Color.white;
                if (Text != null) {
                    Text.GetComponent<Renderer> ().material.color = Color.black;
                }
            } else {
                Background.GetComponent<Renderer> ().material.color = new Color (0.3f, 0.3f, 0.3f);
                if (Text != null) {
                    Text.GetComponent<Renderer> ().material.color = Color.white;
                }
            }
            //Background.GetComponent<Renderer> ().enabled = selected;
        }
    }

    static public int FilterType = -1;

    BasicFilterButton [] FilterButton;

    public void SelectFilter (int number) {
        SelectFilter (FilterType, number);
    }

    public void SelectFilter (int filterType, int number) {
        SelectFilter (filterType, number, !Filters [filterType] [number]);
    }

    public void SelectFilter (int filterType, int number, bool selected) {
        SetFilter (filterType, number, selected);
        FilterButton [number].SelectFilter (selected);
        if (number > 0 && Filters [filterType] [0] && selected) {
            SelectFilter (filterType, 0, false);
        } else if (number > 0 && !selected) {
            bool anySelected = false;
            for (int x = 1; x < Filters [filterType].Length; x++) {
                if (Filters [filterType] [x]) {
                    anySelected = true;
                    break;
                }
            }
            if (!anySelected) {
                SelectFilter (filterType, 0, true);
            }
        }
    }

    GameObject FilterList;

    public void ShowFilters (int filterType) {
        if (FilterList != null) {
            Destroy (FilterList);
        }
        if (filterType == FilterType) {
            FilterType = -1;
            return;
        } else {
            FilterType = filterType;
        }
        GameObject Clone;
        int scale = 1;
        if (FilterType == 3) {
            scale = 2;
        }
        int MaxNumber = 0;
        switch (FilterType) {
            case 0:
                MaxNumber = Filters [0].Length;
                break;
            case 1:
                MaxNumber = GameData.TokenExisting + 1;
                break;
            case 2:
                MaxNumber = GameData.AbilityExisting + 1;
                break;
            case 3:
                MaxNumber = 9;
                break;
        }
        float XCount = 4 / scale;
        float YCount = (int) ((MaxNumber - 1) / XCount + 1);

        Clone = CreateSprite ("UI/Panel_Window_01_Sliced");
        Clone.transform.parent = transform;
        SetSpriteScale (Clone, 300, 120 + (int) (45 * YCount * scale));
        SetInPixPosition (Clone, 720, 60 + (int) (22.5f * YCount * scale), -0.001f);
        GameObject BG = FilterList = Clone;

        FilterButton = new BasicFilterButton [(int) (YCount * XCount)];
        for (int y = 0; y < YCount; y++) {
            for (int x = 0; x < XCount; x++) {
                int number = y * (int) XCount + x;
                if (number < MaxNumber) {
                    FilterButton [number] = new BasicFilterButton (BG.transform,
                        GetInPixPosition ((int) (630 + (x + 0.5f) * 45 * scale),
                        (int) (60 + (y + 0.5f) * 45 * scale),
                        -0.002f),
                        number,
                        Filters [FilterType] [number],
                        scale);
                }
            }
        }
    }
    
    */
    public void LoadPage (int page) {
        int MaxX = 4;
        for (int x = 0; x < MaxX; x++) {
            for (int y = 0; y < 5; y++) {
                //GameData.SetCard (CardInCollection [x + y * MaxX].Card, x + y * MaxX + MaxX * 5 * page);
            }
        }
    }

    // Lib
}
