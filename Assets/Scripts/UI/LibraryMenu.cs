using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LibraryMenu : GOUI {

    static int pageLimit = 2;

    static public int abilityPage = 0;

    static bool loaded = false;
    static List<int> [,] [] WorksWellWithID;
    static List<int> [,] [] IsGoodAgainstID;
    static List<int> [,] [] IsWeakAgainstID;

    static GameObject [] [] Buttons;
    static GameObject [] [] ButtonImage;
    static GameObject [] [] ButtonText;

    static int type;

    static int selectedType = 0;

    static bool editMode = true;
    static int selectedEditMode;
    static int [] NumberOfButtons = new int [] { 3, 8, 6, 2, 13 };
    static int [] Selected = new int [] { 0, 1, 0, 0, 1 };

    // Use this for initialization
    void Start () {
        NumberOfButtons [0] = AppDefaults.availableTokens;
        NumberOfButtons [1] = AppDefaults.availableAbilities;
        WorksWellWithID = new List<int> [2, 2] [];
        IsGoodAgainstID = new List<int> [2, 2] [];
        IsWeakAgainstID = new List<int> [2, 2] [];
        for (int x = 0; x < 2; x++) {
            for (int y = 0; y < 2; y++) {
                WorksWellWithID [x, y] = new List<int> [NumberOfButtons [x]];
                IsGoodAgainstID [x, y] = new List<int> [NumberOfButtons [x]];
                IsWeakAgainstID [x, y] = new List<int> [NumberOfButtons [x]];
                for (int z = 0; z < NumberOfButtons [x]; z++) {
                    WorksWellWithID [x, y] [z] = new List<int> ();
                    IsGoodAgainstID [x, y] [z] = new List<int> ();
                    IsWeakAgainstID [x, y] [z] = new List<int> ();
                    /*while (Random.Range (0, 3) > 0) {
                        WorksWellWithID [x, y] [z].Add (Random.Range (0, NumberOfButtons [y]));
                    }
                    while (Random.Range (0, 3) > 0) {
                        IsGoodAgainstID [x, y] [z].Add (Random.Range (0, NumberOfButtons [y]));
                    }
                    while (Random.Range (0, 3) > 0) {
                        IsWeakAgainstID [x, y] [z].Add (Random.Range (0, NumberOfButtons [y]));
                    }*/
                }
            }
        }
        CreateLibraryMenu ();
        CurrentGOUI = this;

        LoadAbilityPage ();

        LoadData ();

        DestroyTemplateButtons ();
    }
	
	// Update is called once per frame
	void Update () {
        if (editMode) {
            if (Input.GetKeyDown ("1")) {
                selectedEditMode = 1;
            }
            if (Input.GetKeyDown ("2")) {
                selectedEditMode = 2;
            }
            if (Input.GetKeyDown ("3")) {
                selectedEditMode = 3;
            }
            if (Input.GetKeyDown ("s")) {
                LibraryData.SaveWorksWellWith (LibraryData.ListToString (WorksWellWithID));
                LibraryData.SaveIsGoodAgainst (LibraryData.ListToString (IsGoodAgainstID));
                LibraryData.SaveIsWeakAgainst (LibraryData.ListToString (IsWeakAgainstID));
            }
            if (Input.GetKeyDown ("l")) {
                LoadData ();
            }
        }
    }

    static public void LoadData () {
        if (editMode) {
            WorksWellWithID = LibraryData.StringToJaggedList (LibraryData.GetWorksWellWith ());
            IsGoodAgainstID = LibraryData.StringToJaggedList (LibraryData.GetIsGoodAgainst ());
            IsWeakAgainstID = LibraryData.StringToJaggedList (LibraryData.GetIsWeakAgainst ());
        } else if (!loaded) {
            WorksWellWithID = LibraryData.StringToJaggedList ((Resources.Load ("ExportFolder/Library/WorksWellWith") as TextAsset).text);
            IsGoodAgainstID = LibraryData.StringToJaggedList ((Resources.Load ("ExportFolder/Library/IsGoodAgainst") as TextAsset).text);
            IsWeakAgainstID = LibraryData.StringToJaggedList ((Resources.Load ("ExportFolder/Library/IsWeakAgainst") as TextAsset).text);
            loaded = true;
        }
    }

    static public void  EditElementToggle (int elementType, int elementNumber) {
        if (!editMode || selectedEditMode == 0) {
            return;
        }
        List <int> list = null;
        int number = Selected [selectedType];
        if (elementType == 1) {
            elementNumber = AbilityButtonNumberToAbilityNumber (elementNumber);
        }
        switch (selectedEditMode) {
            case 1:
                list = WorksWellWithID [selectedType, elementType] [Selected [selectedType]];
                break;
            case 2:
                list = IsGoodAgainstID [selectedType, elementType] [Selected [selectedType]];
                break;
            case 3:
                list = IsWeakAgainstID [selectedType, elementType] [Selected [selectedType]];
                break;
        }
        if (list.Contains (elementNumber)) {
            list.Remove (elementNumber);
        } else {
            list.Add (elementNumber);
        }
        LoadElement ();
    }

    static public void NextAbilityPage () {
        abilityPage++;
        abilityPage = Mathf.Min (abilityPage, pageLimit - 1);
        LoadAbilityPage ();
    }

    static public void PrevAbilityPage () {
        abilityPage--;
        abilityPage = Mathf.Max (abilityPage, 0);
        LoadAbilityPage ();
    }

    static public int AbilityButtonNumberToAbilityNumber (int buttonNumber) {
        return buttonNumber + abilityPage * (NumberOfButtons [1] - 2);
    }

    static public void SelectButton (int type, int number) {
        for (int x = 0; x < 2; x++) {
            foreach (GameObject button in Buttons [x]) {
                if (button != null) {
                    button.GetComponent<UIController> ().FreeAndUnlcok ();
                }
            }
        }

        Selected [type] = number;
        selectedType = type;
        /*
        if (type == 1) {
            Selected [type] = AbilityButtonNumberToAbilityNumber (number);
        } else {
            Selected [type] = number;
        }*/
        foreach (GameObject button in Buttons [type]) {
            if (button != null) {
                UIController UIC = button.GetComponent<UIController> ();
                UIC.FreeAndUnlcok ();
                if (IsUICActive (UIC, number)) {
                    UIC.PressAndLock ();
                }
            }
        }/*
        if (Buttons [type] != null) {
            if (Buttons [type] [number] != null) {
                Buttons [type] [number].GetComponent<UIController> ().PressAndLock ();
            }
        }*/
        LoadElement ();
    }

    static bool IsUICActive (UIController UIC, int number) {
        return (selectedType == 1 && UIC.abilityType == number) || (selectedType == 0 && UIC.tokenType == number);
    }

    static GameObject ElementPreview;
    static GameObject ElementName;
    static GameObject ElementDescription;

    static GameObject [] WorksWellWith = new GameObject [8];
    static GameObject [] IsGoodAgainst = new GameObject [8];
    static GameObject [] IsWeakAgainst = new GameObject [8];

    static public GameObject CreateElement (Transform anchor, int type, int number) {
        switch (type) {
            case 0:
                return CreateToken (anchor, number);
            case 1:
                return CreateAbility (anchor, number);
        }
        return null;
    }

    static public GameObject CreateToken (Transform background, int number) {
        GameObject Clone;
        VisualToken VT;
        VT = new VisualToken ();
        Clone = VT.Anchor;
        Clone.transform.SetParent (background.transform);
        Clone.transform.localEulerAngles = new Vector3 (-90, 0, 0);
        Clone.transform.localPosition = Vector3.zero;
        Clone.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
        VT.SetType (number);
        background.GetComponent<UIController> ().tokenType = number;
        background.name = UIString.LibraryMenuTokenType;
        DestroyImmediate (VT.Text);
        return Clone;
    }

    static public GameObject CreateAbility (Transform background, int number) {
        GameObject Clone;
        UIController UIC = background.GetComponent<UIController> ();
        background.name = UIString.LibraryMenuAbilityType;
        Clone = CreateSprite (VisualCard.GetIconPath (number), 150, 150, 12, 45, 45, false);
        Clone.transform.SetParent (background);
        Clone.transform.localPosition = Vector3.zero;
        background.GetComponent<UIController> ().abilityType = number;
        Destroy (Clone.GetComponent<Collider> ());
        Clone.GetComponent<SpriteRenderer> ().color = AppDefaults.GetAbilityColor (number);
        return Clone;
    }

    static public void LoadElement () {
        GameObject background;
        GameObject Clone = null;

        if (ElementPreview != null) {
            DestroyImmediate (ElementPreview);
        }
        for (int x = 0; x < 8; x++) {
            if (WorksWellWith [x] != null) {
                DestroyImmediate (WorksWellWith [x]);
            }
            if (IsGoodAgainst [x] != null) {
                DestroyImmediate (IsGoodAgainst [x]);
            }
            if (IsWeakAgainst [x] != null) {
                DestroyImmediate (IsWeakAgainst [x]);
            }
        }

        int number = Selected [selectedType];

        background = CreateSprite ("", 570, 200, 11, 150, 150, true);
        ElementPreview = background;
        switch (selectedType) {
            case 0:
                Clone = CreateToken (background.transform, number);
                Clone.transform.localScale = new Vector3 (2, 2, 2);

                ElementName.GetComponent<Text> ().text = Language.TokenName [number];
                ElementDescription.GetComponent<Text> ().text = Language.GetTokenDescription (number);
                break;
            case 1:
                Clone = CreateAbility (background.transform, number);
                SetInPixScale (Clone, 750, 750);

                ElementName.GetComponent<Text> ().text = Language.AbilityName [number];
                ElementDescription.GetComponent<Text> ().text = Language.GetAbilityDescription (number);
                break;
        }
        int c1 = 0;
        int c2 = 0;
        for (int y = 0; y < 3; y++) {
            switch (y) {
                case 0:
                    c1 = WorksWellWithID [selectedType, 0] [number].Count;
                    c2 = WorksWellWithID [selectedType, 1] [number].Count + c1;
                    break;
                case 1:
                    c1 = IsGoodAgainstID [selectedType, 0] [number].Count;
                    c2 = IsGoodAgainstID [selectedType, 1] [number].Count + c1;
                    break;
                case 2:
                    c1 = IsWeakAgainstID [selectedType, 0] [number].Count;
                    c2 = IsWeakAgainstID [selectedType, 1] [number].Count + c1;
                    break;
            }
            for (int x = 0; x < 8; x++) {
                if (x >= c2) {
                    break;
                }
                background = CreateSprite ("UI/Butt_M_EmptySquare", 510 + x * 90, 450 + y * 210, 11, 90, 90, true);
                int secondType = 0;
                int index = x;
                if (x >= c1) {
                    secondType = 1;
                    index -= c1;
                }
                switch (y) {
                    case 0:
                        Clone = CreateElement (background.transform, secondType, WorksWellWithID [selectedType, secondType] [number] [index]);
                        WorksWellWith [x] = background;
                        break;
                    case 1:
                        Clone = CreateElement (background.transform, secondType, IsGoodAgainstID [selectedType, secondType] [number] [index]);
                        IsGoodAgainst [x] = background;
                        break;
                    case 2:
                        Clone = CreateElement (background.transform, secondType, IsWeakAgainstID [selectedType, secondType] [number] [index]);
                        IsWeakAgainst [x] = background;
                        break;
                }
                Clone.transform.localScale = Clone.transform.localScale * 1.5f;
            }
        }
    }

    static public void LoadAbilityPage () {
        type = 1;
        int count = Buttons [type].Length;
        for (int x = 0; x < count; x++) {
            int number = AbilityButtonNumberToAbilityNumber (x);
            GameObject button = Buttons [type] [x];
            GameObject buttonText = ButtonText [type] [x];
            GameObject buttonImage = ButtonImage [type] [x];
            if (button != null) {
                button.GetComponent<Renderer> ().enabled = true;
                button.GetComponent<Collider> ().enabled = true;
                buttonImage.GetComponent<Renderer> ().enabled = false;
                UIController UIC = button.GetComponent<UIController> ();
                UIC.FreeAndUnlcok ();
                if (abilityPage < pageLimit - 1 && x == NumberOfButtons [type] - 1) {
                    button.name = UIString.LibraryMenuNextAbilityPage;
                    SetText (buttonText, ">");
                } else if (abilityPage > 0 && x == 0) {
                    button.name = UIString.LibraryMenuPrevAbilityPage;
                    SetText (buttonText, "<");
                } else {
                    button.name = UIString.LibraryMenuAbilityType;
                    if (number < AppDefaults.availableAbilities) {
                        SetSprite (buttonImage, VisualCard.GetIconPath (number));
                        buttonImage.GetComponent<SpriteRenderer> ().color = AppDefaults.GetAbilityColor (number);
                        UIC.abilityType = number;
                        buttonImage.GetComponent<Renderer> ().enabled = true;
                    } else {
                        button.GetComponent<Renderer> ().enabled = false;
                        button.GetComponent<Collider> ().enabled = false;
                    }
                    SetText (buttonText, "");
                }
            }
        }
    }
 
    static public void ShowLibrary () {
        DestroyMenu ();
        CurrentCanvas.AddComponent<LibraryMenu> ();
    }

    static public void CreateLibraryMenu () {
        Text text;

        GameObject Clone = CreateSprite ("UI/Panel_Window_01_Sliced", 900, 520, 10, 1050, 1030, false);
        DestroyImmediate (Clone.GetComponent<UIController> ());
        GameObject.DestroyImmediate (Clone.GetComponent<Collider> ());

        Clone = CreateSprite ("UI/Butt_S_Help", 1335, 90, 11, 64, 64, true);
        Clone.name = UIString.LibraryMenuAbout;

        Clone = CreateSprite ("UI/Butt_M_Discard", 1320, 930, 11, 90, 90, true);
        Clone.name = UIString.ShowMainMenu;

        Clone = CreateUIText ("", 960, 120, 500, 36);
        text = Clone.GetComponent<Text> ();
        text.alignment = TextAnchor.MiddleLeft;
        ElementName = Clone;

        Clone = CreateUIText ("", 960, 420, 500, 24);
        text = Clone.GetComponent<Text> ();
        text.alignment = TextAnchor.UpperLeft;
        ElementDescription = Clone;

        Clone = CreateUIText (Language.WorksWellWith, 720, 360, 500, 24);
        text = Clone.GetComponent<Text> ();
        text.alignment = TextAnchor.MiddleLeft;

        Clone = CreateUIText (Language.IsGoodAgains, 720, 570, 500, 24);
        text = Clone.GetComponent<Text> ();
        text.alignment = TextAnchor.MiddleLeft;

        Clone = CreateUIText (Language.IsWeakAgainst, 720, 780, 500, 24);
        text = Clone.GetComponent<Text> ();
        text.alignment = TextAnchor.MiddleLeft;

        Buttons = new GameObject [NumberOfButtons.Length] [];
        ButtonText = new GameObject [NumberOfButtons.Length] [];
        ButtonImage = new GameObject [NumberOfButtons.Length] [];


        int maxX = 5;
        int maxY = 5;

        int sx = 45 + maxX * 60;
        int sy = 150;

        int px = sx / 2 + 30;
        int py = sy / 2 + 30 * maxY;

        int dy = 50;

        type = 0;

        AddButtons (px, py, maxX, maxY);

        maxY = 8;

        py += 60 + (maxY + 5) * 30 + dy;

        AddButtons (px, py, maxX, maxY);

        LoadElement ();
    }


    static void AddButtons (int px, int py, int maxX, int maxY) {
        int count = NumberOfButtons [type] = Mathf.Min (NumberOfButtons [type], maxX * maxY);
        
        Buttons [type] = new GameObject [count];
        ButtonText [type] = new GameObject [count];
        ButtonImage [type] = new GameObject [count];


        GameObject BackgroundObject;
        GameObject Clone;
        VisualToken VT;

        int sx = 90 + 60 * maxX;
        int sy = 140 + 60 * maxY;
        int dy = 70;

        BackgroundObject = CreateSprite ("UI/Panel_Window_01_Sliced", px, py, 10, sx, sy, false);
        DestroyImmediate (BackgroundObject.GetComponent<UIController> ());

        switch (type) {

            case 0:
                Clone = CreateText (Language.TokenType, px, py - sy / 2 + dy, 12, 0.02f);
                break;
            case 1:
                Clone = CreateText (Language.AbilityType, px, py - sy / 2 + dy, 12, 0.02f);
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
                BackgroundObject = CreateSprite ("UI/Butt_M_EmptySquare", npx, npy, 11, 60, 60, true);
                UIController UIC = BackgroundObject.GetComponent<UIController> ();
                switch (type) {
                    case 0:
                        UIC.tokenType = number;
                        BackgroundObject.name = UIString.LibraryMenuTokenType;
                        VT = new VisualToken ();
                        Clone = VT.Anchor;
                        Clone.transform.SetParent (BackgroundObject.transform);
                        Clone.transform.localEulerAngles = new Vector3 (-90, 0, 0);
                        Clone.transform.localPosition = Vector3.zero;
                        Clone.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
                        VT.SetType (number);
                        DestroyImmediate (VT.Text);
                        break;
                    case 1:
                        BackgroundObject.name = UIString.LibraryMenuAbilityType;
                        Clone = CreateSprite (VisualCard.GetIconPath (number), npx, npy, 12, 45, 45, false);
                        Destroy (Clone.GetComponent<Collider> ());
                        ButtonText [type] [number] = CreateText ("", npx, npy, 13, 0.03f);
                        ButtonImage [type] [number] = Clone;
                        UIC.Text = ButtonText [type] [number];
                        Clone.GetComponent<SpriteRenderer> ().color = AppDefaults.GetAbilityColor (number);
                        break;
                }
                if (IsUICActive (UIC, Selected [type])) {
                    UIC.PressAndLock ();
                }
                UIC.number = number;
                Buttons [type] [number] = BackgroundObject;

            }
        }
        type++;
    }
}
