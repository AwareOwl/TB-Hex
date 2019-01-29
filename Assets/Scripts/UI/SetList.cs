using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetList : GOUI {

    static GameObject Background;

    static int SelectedId;

    static RowClass [] row;

    static string [] setName;
    static int [] setId;
    static int [] iconNumber;
    static bool [] legal;

    static int currentPage;

    static PageUI pageUI;
    
	// Use this for initialization
	void Start () {
        CurrentGUI = this;
        currentPage = 0;
        CreateSetList ();
        ClientLogic.MyInterface.CmdDownloadSetList ();
	}

    static public void ShowSetList () {

        DestroyMenu ();
        CurrentCanvas.AddComponent<SetList> ();
    }

    static public void LoadSetList (string [] setName, int [] setId, int [] iconNumber, bool [] legal, int selectedSet) {
        SetList.setName = setName;
        SetList.setId = setId;
        SetList.iconNumber = iconNumber;
        SetList.legal = legal;
        RefreshPageButtons ();
        RefreshPage ();
        SelectSet (selectedSet);
    }

    static public void RefreshPageButtons () {
        int count = setName.Length;
        int pageLimit = (count) / 5 + 1;
        pageUI.Init (9, pageLimit, new Vector2Int (480, 780), UIString.SetListPageButton);
    }

    static public void SelectPage (int pageNumber) {
        currentPage = pageNumber;
        RefreshPage ();
    }

    static public void RefreshPage () {
        for (int x = 0; x < 5; x++) {
            int numberOfRows = row.Length;
            int number = currentPage * numberOfRows + x;
            int count = setName.Length;
            row [x].FreeRow ();
            if (number < count) {
                row [x].SetState (setName [number], setId [number], iconNumber [number], legal [number]);
                if (setId [number] == SelectedId) {
                    row [x].SelectRow ();
                }
            } else if (number == count) {
                row [x].SetState (1);
            } else {
                row [x].SetState (2);
            }
        }
        pageUI.SelectPage (currentPage);
    }


    static public void SelectSet (int id) {
        SelectedId = id;
        for (int x = 0; x < row.Length; x++) {
            row [x].FreeRow ();
            if (row [x].setId == id && SelectedId > 0) {
                row [x].SelectRow ();
            }
        }
    }

    static public void SaveSelection () {
        ClientLogic.MyInterface.CmdSaveSelectedSet (SelectedId);
        MainMenu.ShowMainMenu ();
    }

    static public void DeleteSet (int id) {
        ClientLogic.MyInterface.CmdDeleteSet (id);
    }
    

    static public GameObject CreateSetList () {
        GameObject Collider;
        GameObject Text;
        GameObject Clone;

        Background = CreateSprite ("UI/Panel_Window_01_Sliced", 720, 585, 10, 690, 870, false);
        Text = CreateText (Language.SelectCardSet, 450, 245, 12, 0.035f);
        Text.GetComponent<TextMesh> ().anchor = TextAnchor.MiddleLeft;
        Text.GetComponent<TextMesh> ().fontStyle = FontStyle.Bold;

        Collider = CreateSprite ("UI/Transparent", 720, 540, 9, 10000, 10000, false);
        Collider.transform.SetParent (Background.transform);

        row = new RowClass [5];
        for (int x = 0; x < 5; x++) {
            row [x] = CurrentGUI.gameObject.AddComponent<RowClass> ();
            row [x].Init (x, RowClass.SetList);
        }

        GameObject pageUIObject = new GameObject ();
        pageUI = pageUIObject.AddComponent<PageUI> ();

        Clone = CreateSprite ("UI/Butt_M_Apply", 495, 900, 11, 90, 90, true);
        Clone.transform.SetParent (Background.transform);
        Clone.name = UIString.SaveSelectedSet;

        Clone = CreateSprite ("UI/Butt_M_Discard", 945, 900, 11, 90, 90, true);
        Clone.transform.SetParent (Background.transform);
        Clone.name = UIString.ShowMainMenu;

        return Background;
    }
}
