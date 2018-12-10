using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : GOUI {

    static GameObject BackgroundObject;
    static GameObject [] Button;

    override public void DestroyThis () {
        DestroyImmediate (BackgroundObject);
        foreach (GameObject obj in Button) {
            DestroyImmediate (obj);
        }
        DestroyImmediate (this);
    }

    public void Start () {

        CreateInGameMenu ();
        CurrentSubGUI = this;
    }

    static public void ShowInGameMenu () {
        CurrentCanvas.AddComponent<InGameMenu> ();
    }

    static public void CreateInGameMenu () {
        GameObject Clone;
        BackgroundObject = CreateSprite ("UI/Panel_Window_01_Sliced", 720, 540, 10, 390, 330, false);

        int count = 3;
        Button = new GameObject [count];

        for (int x = 0; x < count; x++) {
            Button [x] = CreateSprite ("UI/Butt_M_EmptySquare", 720, 465 + 75 * x, 11, 270, 60, false);
            Clone = CreateText (Language.MainMenuButton, 720, 465 + 75 * x, 12, 0.03f);
            AddTextToGameObject (Button [x], Clone);
            Clone.transform.SetParent (Button [x].transform);
            switch (x) {
                case 0:
                    Clone.GetComponent<TextMesh> ().text = Language.ContinueButton;
                    Button [x].name = UIString.DestroySubMenu;
                    break;
                case 1:
                    Clone.GetComponent<TextMesh> ().text = Language.MainMenuButton;
                    Button [x].name = UIString.ShowMainMenu;
                    break;
                case 2:
                    Clone.GetComponent<TextMesh> ().text = Language.ExitGameButton;
                    Button [x].name = UIString.ExitApp;
                    break;
            }
        }

    }
}
