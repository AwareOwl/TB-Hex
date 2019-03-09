using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomGameEditor : GOUI {

    static int numberOfButtons = 4;
    static public InputField inputField;

    static public bool [] available;
    static public GameObject [] typeButtons;
    static public int selectedType = 1;

    // Use this for initialization
    void Start () {
        CurrentGOUI = this;
        CreateCustomGameEditor ();
        ClientLogic.MyInterface.CmdDownloadCustomGameEditorData ();
    }

    static public void ShowCustomGameEditor () {
        DestroyMenu ();
        CurrentCanvas.AddComponent<CustomGameEditor> ();
    }

    static public void SelectType () {
        SelectType (selectedType);
    }

    static public void LoadData (int [] matchTypes) {
        GameObject Clone;
        typeButtons = new GameObject [numberOfButtons];
        available = new bool [numberOfButtons];

        for (int y = 0; y < matchTypes.Length; y++) {
            int x = matchTypes [y];
            available [matchTypes [y]] = true;
            Clone = CreateSpriteWithText ("UI/Butt_M_EmptySquare",
                CustomGameClass.GetMatchTypeName (x), 540 + 120 * x, 520, 11, 120, 60);
            Clone.name = UIString.CustomGameEditorTypeButton;
            Clone.GetComponent<UIController> ().number = x;
            typeButtons [x] = Clone;
        }
        SelectType ();
    }

    static public void SelectType (int type) {
        selectedType = type;
        foreach (GameObject obj in typeButtons) {
            if (obj != null) {
                obj.GetComponent<UIController> ().FreeAndUnlcok ();
            }
        }
        if (typeButtons [type] != null) {
            typeButtons [type].GetComponent<UIController> ().PressAndLock ();
        }
    }

    static public void CreateGame () {
        string name = inputField.text;
        if (name == null || name == "") {
            name = Language.CustomGame;
        }
        ClientLogic.MyInterface.CmdCreateCustomGame (name, selectedType);
    }

    static public void CreateCustomGameEditor () {
        GameObject Clone;
        CreateSprite ("UI/Panel_Window_01_Sliced", 720, 540, 10, 690, 420, false);

        inputField = CreateInputField (Language.EnterGameName, 720, 435, 510, 60).GetComponent <InputField>();



        Clone = CreateSprite ("UI/Butt_M_Apply", 495, 630, 11, 90, 90, true);
        Clone.name = UIString.CustomGameEditorApply;

        Clone = CreateSprite ("UI/Butt_M_Discard", 945, 630, 11, 90, 90, true);
        Clone.name = UIString.ShowCustomGameLobby;
    }
}
