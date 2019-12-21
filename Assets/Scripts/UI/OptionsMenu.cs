using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : GOUI {

    public enum OptionsMenuMode {
        normal,
        inGame,
    }

    static OptionsMenuMode currentMode;

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
        CurrentSubGOUI = this;
    }

    static public void ShowOptionsMenu (OptionsMenuMode mode) {
        currentMode = mode;
        CurrentCanvas.AddComponent<OptionsMenu> ();
    }

    static public void CreateInGameMenu () {
        GameObject Clone;

        int count = 3;
        if (currentMode == OptionsMenuMode.inGame) {
            count = 4;
        }

        BackgroundObject = CreateUIImage ("UI/Panel_Window_01_Sliced", 720, 540, 390, 105 + count * 75, false);
        BackgroundObject.GetComponent<Image> ().type = Image.Type.Sliced;

        Clone = CreateSprite ("UI/Transparent", 720, 540, 19, 10000, 10000, false);
        Clone.transform.SetParent (BackgroundObject.transform);
        Button = new GameObject [count];

        for (int x = 0; x < count; x++) {
            int py = (int) (540 + 75 * (x - count / 2f + 0.5f));
            Button [x] = CreateUIButton ("UI/Butt_M_EmptySquare", 720, py, 270, 60, false);
            Button [x].GetComponent<Image> ().type = Image.Type.Sliced;
            Clone = CreateUIText (Language.MainMenuButton, 720, py, 500, 36);
            DestroyImmediate (Clone.GetComponent<Collider> ());
            AddTextToGameObject (Button [x], Clone);
            Clone.transform.SetParent (Button [x].transform);
            switch (x) {
                case 0:
                    Clone.GetComponent<Text> ().text = Language.ContinueButton;
                    Button [x].name = UIString.DestroySubMenu;
                    break;
                case 1:
                    switch (currentMode) {
                        case OptionsMenuMode.inGame:
                            Clone.GetComponent<Text> ().text = Language.MainMenuButton;
                            Button [x].name = UIString.ShowMainMenu;
                            break;
                        case OptionsMenuMode.normal:
                            Clone.GetComponent<Text> ().text = Language.Settings;
                            Button [x].name = UIString.ShowSettingsMenu;
                            break;
                    }
                    break;
                    break;
                case 2:
                    switch (currentMode){
                        case OptionsMenuMode.inGame:
                            Clone.GetComponent<Text> ().text = Language.Settings;
                            Button [x].name = UIString.ShowSettingsMenu;
                            break;
                        case OptionsMenuMode.normal:
                            Clone.GetComponent<Text> ().text = Language.ExitGameButton;
                            Button [x].name = UIString.ExitApp;
                            break;
                    }
                    break;
                case 3:
                    Clone.GetComponent<Text> ().text = Language.ExitGameButton;
                    Button [x].name = UIString.ExitApp;
                    break;
            }
        }

    }
}
