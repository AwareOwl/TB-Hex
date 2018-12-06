using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetList : GOUI {

    static GameObject Background;

    static int SelectedSet;

    static SetRowClass [] setRow;

    static string [] setName;
    static int [] setId;

    //static GameObject

	// Use this for initialization
	void Start () {
        CreateSetList ();
        CurrentGUI = this;
        ClientLogic.MyInterface.CmdDownloadSetList ();
	}
	
	// Update is called once per frame
	void Update () {

    }

    static public void ShowSetList () {

        DestroyMenu ();
        CurrentCanvas.AddComponent<SetList> ();
    }

    static public void LoadSetList (string [] setName, int [] setId) {
        SetList.setName = setName;
        SetList.setId = setId;
        int count = setName.Length;
        for (int x = 0; x < count; x++) {
            setRow [x].SetState (setName [x], setId [x], 0);
        }
        if (count < setRow.Length) {
            setRow [count].SetState (1);
        }
        for (int x = count + 1; x < setRow.Length; x++) {
            setRow [count].SetState (2);
        }
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

        setRow = new SetRowClass [5];
        for (int x = 0; x < 5; x++) {
            setRow [x] = new SetRowClass (x);
        }


        Clone = CreateSprite ("UI/Butt_M_Apply", 495, 810, 11, 90, 90, true);
        Clone.transform.SetParent (Background.transform);

        Clone = CreateSprite ("UI/Butt_M_Discard", 945, 810, 11, 90, 90, true);
        Clone.transform.SetParent (Background.transform);

        return Background;
    }
}
