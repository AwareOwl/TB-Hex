using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossMenu : GOUI {

    static PageUI pageUI;

    static public List<GameObject> garbage = new List<GameObject> ();

    static public Text bossNameText;
    static public Text bossDescriptionText;

    static public GameObject [] bossRow;
    static public GameObject [] bossRowText;

    static UIController [] typeButton = new UIController [2];

    static int [] bossNumber;
    static int [] id;
    static bool [] finished;
    static bool [] unlocked;

    static int lockedCount;

    static List<int> finishedIndex;
    static List<int> unfinishedIndex;

    static public int pageType;

    static int selectedRow = -1;
    static public int selectedId = -1;

    static int [] currentPage = new int [2];

    // Use this for initialization
    void Start () {
        DestroyTemplateButtons ();
        CreateBossMenu ();
        CurrentGOUI = this;
    }

    static public void ShowBossMenu () {
        ClientLogic.MyInterface.CmdDownloadDataToBossMenu ();
    }

    static public void LoadBossMenu (int [] bossNumber, int [] id, bool [] finished, bool [] unlocked) {
        BossMenu.bossNumber = bossNumber;
        BossMenu.id = id;
        BossMenu.finished = finished;
        BossMenu.unlocked = unlocked;
        SortFinished ();
        DestroyMenu ();
        CurrentCanvas.AddComponent<BossMenu> ();
    }

    static public void SortFinished () {
        lockedCount = 0;
        finishedIndex = new List<int> ();
        unfinishedIndex = new List<int> ();
        int count = id.Length;
        for (int x = 0; x < count; x++) {
            if (unlocked [x]) {
                if (finished [x]) {
                    finishedIndex.Add (x);
                } else {
                    unfinishedIndex.Add (x);
                }
            } else {
                lockedCount++;
            }
        }
    }

    static public void StartFight () {
        int number = bossNumber [RowNumberToNumber (selectedRow)];
        switch (number) {
            default:
                ClientLogic.MyInterface.CmdStartBoss (selectedId, Language.BossName [number], "", "");
                break;
            case 6:
                ClientLogic.MyInterface.CmdStartBoss (selectedId, Language.BossName [number], Language.BossName [7], "");
                break;
            case 7:
                ClientLogic.MyInterface.CmdStartBoss (selectedId, Language.BossName [9], Language.BossName [10], Language.BossName [8]);
                break;
        }
    }

    static public void Apply () {
        ClientLogic.MyInterface.CmdEditBossSet (selectedId);
        //ClientLogic.MyInterface.CmdStartBoss (selectedId, Language.BossName [bossNumber]);
    }

    static public List<int> SelectedList () {
        switch (pageType) {
            case 0:
                return unfinishedIndex;
            case 1:
                return finishedIndex;
            default:
                return null;
        }
    }

    static public void SelectType (int type) {
        pageType = type;
        for (int x = 0; x < 2; x++) {
            typeButton [x].FreeAndUnlcok ();
        }
        typeButton [type].PressAndLock ();
        
        List<int> list = SelectedList ();
        int count = bossRow.GetLength (0);
        int bossCount = list.Count;
        int pageCount;
        switch (type) {
            case 0:
                pageCount = Mathf.Max (1, (bossCount) / count + 1);
                break;
            default:
                pageCount = Mathf.Max (1, (bossCount - 1) / count + 1);
                break;
        }
        pageUI.Init (10, pageCount, new Vector2Int (90, 870), UIString.BossMenuPageButton);
        Mathf.Min (currentPage [pageType], pageCount - 1);

        RefreshPage ();
    }

    static public void SelectPage (int number) {
        currentPage [pageType] = number;
        RefreshPage ();
    }

    static public void RefreshPage () {
        List<int> list = SelectedList ();
        int count = bossRow.GetLength (0);
        int bossCount = list.Count;
        for (int x = 0; x < count; x++) {
            GameObject Clone = bossRow [x];
            UIController UIC = Clone.GetComponent<UIController> ();
            UIC.FreeAndUnlcok ();
            GameObject Text = bossRowText [x];
            int number = x + currentPage [pageType] * count;


            Text.GetComponent<Renderer> ().material.color = Color.black;
            if (number < bossCount) {
                SetSprite (Clone, "UI/Panel_Slot_01_Sliced", true);
                if (RowNumberToId (x) == selectedId) {
                    UIC.PressAndLock ();
                }
                
                Text.GetComponent<TextMesh> ().text = Language.BossName [bossNumber [list[number]]];
                Clone.name = UIString.BossMenuRow;
                /*} else if (number == puzzleCount && toUnlockCount > 0 && pageType == 0) {
                    SetSprite (Clone, "UI/Panel_Slot_01_Sliced_D", true);
                    Text.GetComponent<TextMesh> ().text = toUnlockCount + " " + Language.MoreToUnlock;
                    Text.GetComponent<Renderer> ().material.color = new Color (0.4f, 0.4f, 0.4f);*/
            } else if (number == bossCount && lockedCount > 0 && pageType == 0) {
                SetSprite (Clone, "UI/Panel_Slot_01_Sliced_D", true);
                Text.GetComponent<TextMesh> ().text = lockedCount + " " + Language.MoreToUnlock;
                Text.GetComponent<Renderer> ().material.color = new Color (0.4f, 0.4f, 0.4f);
                Clone.name = UIString.BossMenuLocked;

            } else {
                SetSprite (Clone, "UI/Panel_Slot_01_Sliced_D", true);
                Text.GetComponent<TextMesh> ().text = "";
                Clone.name = UIString.BossMenuRow;
            }
        }
        pageUI.SelectPage (currentPage [pageType]);
    }

    static public void SelectRow (int rowNumber) {
        selectedRow = rowNumber;
        if (SelectedList ().Count > RowNumberToNumber (rowNumber)) {
            int id = RowNumberToId (rowNumber);
            selectedId = id;
            CreateBossPreview (bossNumber [SelectedList ()[RowNumberToNumber (selectedRow)]]);
            RefreshPage ();
        }
    }

    static public int GameModeNameToBossID (string gameModeName) {
        return int.Parse (gameModeName.Substring (gameModeName.IndexOf ('#') + 1));
    }

    static public string GameModeNameToBossName (string gameModeName) {
        return Language.BossName [GameModeNameToBossID (gameModeName)];
    }

    static public int RowNumberToId (int rowNumber) {
        return id [SelectedList () [RowNumberToNumber (rowNumber)]];
    }

    static public int RowNumberToNumber (int rowNumber) {
        int count = bossRow.GetLength (0);
        return rowNumber + currentPage [pageType] * count;
    }


    static public void CreateBossMenu () {

        GameObject Clone;
        UIController UIC;

        Clone = CreateSprite ("UI/Panel_Window_01_Sliced", 720, 540, 10, 1440, 1080, false);
        DestroyImmediate (Clone.GetComponent<UIController> ());
        //Clone = CreateSprite ("UI/White", 720, 540, 11, 5, 1080, false);

        Clone = CreateUIText (Language.ListOfBosses, 325, 120, 500, 36);
        Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;

        Clone = CreateUIText ("", 1015, 120, 500, 36);
        Text text = Clone.GetComponent<Text> ();
        text.alignment = TextAnchor.MiddleLeft;
        bossNameText = text;


        Clone = CreateUIText ("", 1030, 955, 570, 24);
        text = Clone.GetComponent<Text> ();
        text.alignment = TextAnchor.UpperLeft;
        bossDescriptionText = text;

        GameObject pageUIObject = new GameObject ();
        pageUI = pageUIObject.AddComponent<PageUI> ();


        /*Clone = CreateUIText (Language.BossAbout, 360, 210, 600, 24);
        Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;*/

        Clone = CreateSprite ("UI/Butt_S_Help", 1350, 90, 11, 64, 64, true);
        Clone.name = UIString.BossMenuAbout;


        /*GameObject pageUIObject = new GameObject ();
        pageUI = pageUIObject.AddComponent<PageUI> ();*/

        bossRow = new GameObject [9];
        bossRowText = new GameObject [9];

        for (int x = 0; x < 9; x++) {
            Clone = CreateSpriteWithText ("UI/Panel_Slot_01_Sliced", "", 360, 300 + 60 * x, 11, 600, 60);
            UIC = Clone.GetComponent<UIController> ();
            GameObject Text;
            Text = UIC.Text;
            Text.transform.parent = CurrentCanvas.transform;
            Text.GetComponent<TextMesh> ().anchor = TextAnchor.MiddleLeft;
            bossRowText [x] = Text;
            SetInPixPosition (Text, 90, 300 + 60 * x, 12);
            Clone.name = UIString.BossMenuRow;
            UIC.number = x;
            UIC.Text = null;
            bossRow [x] = Clone;
        }

        for (int x = 0; x < 2; x++) {
            string name = "";
            switch (x) {
                case 0:
                    name = Language.Unfinished;
                    break;
                case 1:
                    name = Language.Finished;
                    break;
            }
            Clone = CreateSpriteWithText ("UI/Butt_M_EmptySquare", name, 210 + 300 * x, 240, 11, 300, 60);
            Clone.name = UIString.BossMenuType;
            UIC = Clone.GetComponent<UIController> ();
            UIC.number = x;
            typeButton [x] = UIC;
        }

        SelectType (0);
        
        Clone = CreateSprite ("UI/Butt_M_Apply", 105, 975, 11, 90, 90, true);
        Clone.name = UIString.BossMenuApply;

        Clone = CreateSprite ("UI/Butt_M_Discard", 615, 975, 11, 90, 90, true);
        Clone.name = UIString.ShowMainMenu;
    }

    static GameObject avatar;

    static public void CreateBossPreview (int id) {
        GameObject Clone;

        bossNameText.text = Language.BossName [id];
        bossDescriptionText.text = 
            Language.BossDescription [id] +
            System.Environment.NewLine +
            System.Environment.NewLine +
            Language.OpponentTrait + ":" +
            System.Environment.NewLine +
            Language.StatusDescription [id];


        if (avatar == null) {
            Clone = CreateSprite ("UI/Panel_Window_01_Sliced", 1020, 435, 11, 450, 450, false);
            avatar = Clone;
        }
        SetSprite (avatar, AppDefaults.bossAvatar [id]);
        
    }
}
