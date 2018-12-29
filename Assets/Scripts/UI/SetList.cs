using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetList : GOUI {

    static GameObject Background;

    static int SelectedId;

    static RowClass [] setRow;

    static string [] setName;
    static int [] setId;
    
	// Use this for initialization
	void Start () {
        CurrentGUI = this;
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
        int count = Mathf.Min (setName.Length, setRow.Length);
        for (int x = 0; x < count; x++) {
            setRow [x].SetState (setName [x], setId [x], iconNumber [x], legal [x]);
        }
        if (count < setRow.Length) {
            setRow [count].SetState (1);
        }
        for (int x = count + 1; x < setRow.Length; x++) {
            setRow [x].SetState (2);
        }
        SelectSet (selectedSet);
    }

    static public void SelectSet (int id) {
        SelectedId = id;
        for (int x = 0; x < setRow.Length; x++) {
            setRow [x].FreeRow ();
            if (setRow [x].setId == id && SelectedId > 0) {
                setRow [x].SelectRow ();
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

        Background = CreateSprite ("UI/Panel_Window_01_Sliced", 720, 540, 10, 690, 780, false);
        Text = CreateText (Language.SelectCardSet, 450, 245, 12, 0.035f);
        Text.GetComponent<TextMesh> ().anchor = TextAnchor.MiddleLeft;
        Text.GetComponent<TextMesh> ().fontStyle = FontStyle.Bold;

        Collider = CreateSprite ("UI/Transparent", 720, 540, 9, 10000, 10000, false);
        Collider.transform.SetParent (Background.transform);

        setRow = new RowClass [5];
        for (int x = 0; x < 5; x++) {
            setRow [x] = CurrentGUI.gameObject.AddComponent<RowClass> ();
            setRow [x].Init (x, RowClass.SetList);
        }


        Clone = CreateSprite ("UI/Butt_M_Apply", 495, 810, 11, 90, 90, true);
        Clone.transform.SetParent (Background.transform);
        Clone.name = UIString.SaveSelectedSet;

        Clone = CreateSprite ("UI/Butt_M_Discard", 945, 810, 11, 90, 90, true);
        Clone.transform.SetParent (Background.transform);
        Clone.name = UIString.ShowMainMenu;

        return Background;
    }
}
