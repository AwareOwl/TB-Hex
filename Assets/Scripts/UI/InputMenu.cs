using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputMenu : GOUI {

    List<GameObject> Garbage = new List <GameObject>();

    static string text;

    static InputField inputField;

	// Use this for initialization
	void Start () {
        CreateInputMenu ();
        CurrentSubGOUI = this;
    }
    override public void DestroyThis () {
        if (Garbage != null) {
            foreach (GameObject obj in Garbage) {
                if (obj != null) {
                    DestroyImmediate (obj);
                }
            }
        }
    }

    static public void ShowInputMenu (string text) {
        switch (text) {
            case UIString.PromoCode:
                InputMenu.text = Language.EnterCode;
                break;
        }
        CurrentCanvas.AddComponent<InputMenu> ();
    }

    static public void Apply () {
        ClientLogic.MyInterface.CmdEnterCode (inputField.text);
    }


    public void CreateInputMenu () {
        GameObject Clone;

        Clone = CreateUIImage ("UI/Panel_Window_01_Sliced", 720, 540, 650, 400, false);
        DestroyImmediate (Clone.GetComponent<UIController> ());
        Clone.GetComponent<Image> ().type = Image.Type.Tiled;
        Garbage.Add (Clone);

        Clone = CreateUIText (text, 720, 425, 550, 36);
        Garbage.Add (Clone);

        Clone = CreateInputField (Language.EnterCode, 720, 500, 500, 60);
        DestroyImmediate (Clone.GetComponent<UIController> ());
        Garbage.Add (Clone);
        inputField = Clone.GetComponent<InputField> ();

        Clone = CreateUIButton ("UI/Butt_M_Apply", 510, 625, 90, 90, true);
        Clone.name = UIString.InputMenuApplyInput;
        Garbage.Add (Clone);

        Clone = CreateUIButton ("UI/Butt_M_Discard", 930, 625, 90, 90, true);
        Clone.name = UIString.DestroySubMenu;
        Garbage.Add (Clone);
    }
}
