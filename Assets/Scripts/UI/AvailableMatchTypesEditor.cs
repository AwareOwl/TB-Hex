using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableMatchTypesEditor : GOUI {
    
    static int numberOfButtons = 4;
    static GameObject [] typeButtons;
    static bool [] matchTypes;

    static bool editMode;

    static public List<GameObject> Garbage = new List<GameObject> ();

    override public void DestroyThis () {
        foreach (GameObject obj in Garbage) {
            if (obj != null) {
                DestroyImmediate (obj);
            }
        }
    }

    private void Awake () {
        CreateAvailableMatchTypeEditor ();
        CurrentSubGUI = this;
    }

    static public void ShowAvailableMatchTypesEditor () {
        DestroySubMenu ();
        editMode = BoardEditorMenu.editMode;
        CurrentCanvas.AddComponent<AvailableMatchTypesEditor> ();
    }

    static public void ButtonAction (int buttonNumber) {
        if (matchTypes [buttonNumber]) {
            typeButtons [buttonNumber].GetComponent<UIController> ().FreeAndUnlcok ();
        } else {
            typeButtons [buttonNumber].GetComponent<UIController> ().PressAndLock ();
        }
        matchTypes [buttonNumber] = !matchTypes [buttonNumber];
    }

    static public void ApplyChanges () {
        List<int> intTypes = new List<int> ();
        for (int x = 0; x < matchTypes.Length; x++) {
            if (matchTypes [x]) {
                intTypes.Add (x);
            }
        }
        BoardEditorMenu.SaveMatchTypes (intTypes.ToArray());
        DestroySubMenu ();
    }

    static public void CreateAvailableMatchTypeEditor () {

        GameObject Clone;
        Clone = CreateSprite ("UI/Panel_Window_01_Sliced", 720, 540, 20, 660, 420, false);
        Garbage.Add (Clone);

        Clone = CreateUIText (Language.SelectAvailableMatchTypes + ":", 720, 440, 520, 36);
        Garbage.Add (Clone);

        typeButtons = new GameObject [numberOfButtons];

        for (int x = 0; x < numberOfButtons; x++) {
            Clone = CreateSpriteWithText ("UI/Butt_M_EmptySquare",
                CustomGameClass.GetMatchTypeName (x), 540 + 120 * x, 535, 21, 120, 60);
            if (editMode) {
                Clone.name = UIString.AvailableMatchTypesEditorButton;
            } else {
                SetInteractible (Clone, false);
            }
            Clone.GetComponent<UIController> ().number = x;
            typeButtons [x] = Clone;
            Garbage.Add (Clone);
        }

        if (editMode) {
            Clone = CreateSprite ("UI/Butt_M_Apply", 495, 645, 21, 90, 90, true);
            Clone.name = UIString.AvailableMatchTypesEditorApply;
            Garbage.Add (Clone);
        }

        Clone = CreateSprite ("UI/Butt_M_Discard", 945, 645, 21, 90, 90, true);
        Clone.name = UIString.DestroySubMenu;
        Garbage.Add (Clone);

        matchTypes = new bool [numberOfButtons];
        foreach (int type in BoardEditorMenu.matchTypes) {
            matchTypes [type] = true;
            typeButtons [type].GetComponent<UIController> ().PressAndLock ();
        }
    }
}
