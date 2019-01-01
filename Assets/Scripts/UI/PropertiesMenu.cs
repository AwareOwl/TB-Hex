using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertiesMenu : GOUI {

    public const int setPropertiesKey = 0;
    public const int gameModePropertiesKey = 1;
    public const int boardPropertiesKey = 2;

    static GameObject InputObject;
    static GameObject BackgroundObject;
    static GameObject [,] IconBackground;
    static GameObject [,] IconImage;
    static List<GameObject> Garbage = new List<GameObject> ();

    static int iconX;
    static int iconY;
    static int iconNumber = -1;

    static int propertiesMode;

    static bool iconOptions;
    static string name;

    public void Start () {
        CreateSetPropertiesMenu ();
        CurrentSubGUI = this;
    }

    override public void DestroyThis () {
        if (IconBackground != null) {
            foreach (GameObject obj in IconBackground) {
                if (obj != null) {
                    DestroyImmediate (obj);
                }
            }
        }
        if (IconImage != null) {
            foreach (GameObject obj in IconImage) {
                if (obj != null) {
                    DestroyImmediate (obj);
                }
            }
        }
        if (Garbage != null) {
            foreach (GameObject obj in Garbage) {
                if (obj != null) {
                    DestroyImmediate (obj);
                }
            }
        }
        if (InputObject != null) {
            DestroyImmediate (InputObject);
        }
        if (BackgroundObject != null) {
            DestroyImmediate (BackgroundObject);
        }
    }

    static public void ShowSetPropertiesMenu (int propertiesMode, string name, int iconNumber) {
        PropertiesMenu.propertiesMode = propertiesMode;
        switch (propertiesMode) {
            case 0:
                iconOptions = true;
                break;
            case 1:
            case 2:
                iconOptions = false;
                break;
        }
        PropertiesMenu.iconNumber = iconNumber;
        PropertiesMenu.name = name;
        CurrentCanvas.AddComponent<PropertiesMenu> ();
    }

    static public void ApplySetProperties () {
        string name = InputObject.GetComponent<InputField> ().text;
        switch (propertiesMode) {
            case 0:
                SetEditor.ApplySetProperties (name, iconNumber);
                break;
            case 1:
                GameModeEditor.ApplyProperties (name, iconNumber);
                break;
            case 2:
                BoardEditorMenu.ApplyProperties (name, iconNumber);
                break;
        }
        DestroySubMenu ();
    }

    static public void SelectIcon (int x, int y, int number) {
        if (iconNumber != -1) {
            SelectIcon (iconNumber, false);
        }
        SelectIcon (number, true);
        iconX = x;
        iconY = y;
        iconNumber = number;
    }

    static public void SelectIcon (int number, bool enabled) {
        int width = IconBackground.GetLength (0);
        number--;
        int px = number % width;
        int py = number / width;
        SelectIcon (px, py, number + 1, enabled);
    }

    static public void SelectIcon (int x, int y, int number, bool enabled) {
        if (enabled) {
            IconBackground [x, y].GetComponent<Image> ().color = Color.white;
            //IconImage [x, y].GetComponent<Image> ().color = AppDefaults.GetAbilityColor (number);
        } else {
            IconBackground [x, y].GetComponent<Image> ().color = new Color (0.3f, 0.3f, 0.3f);
            //IconImage [x, y].GetComponent<Image> ().color = AppDefaults.GetAbilityColor (number);
        }
        IconImage [x, y].GetComponent<Image> ().color = AppDefaults.GetAbilityColor (number);
    }

    static public void CreateSetPropertiesMenu () {
        GameObject Clone;
        int maxY = 0;
        int shift = 0;
        if (iconOptions) {
            maxY = 3;
            shift = 1;
        }


        BackgroundObject = CreateUIImage ("UI/Panel_Window_01_Sliced", 720, 540, 740, 400 + 75 * maxY + 140 * shift, false);
        BackgroundObject.GetComponent<Image> ().type = Image.Type.Tiled;

        Clone = CreateUIImage ("UI/Transparent", 720, 540, 10000, 10000, false);
        Clone.transform.SetParent (BackgroundObject.transform);
        Garbage.Add (Clone);

        string text = "";
        switch (propertiesMode) {
            case 0:
                text = Language.EnterNewSetName;
                break;
            case 1:
                text = Language.EnterNewGameVersionName;
                break;
            case 2:
                text = Language.EnterNewBoardName;
                break;
        }

        Clone = CreateUIText (text, 720, 425 - 75 * maxY / 2 - 60 * shift, 550, 36);
        Garbage.Add (Clone);

        Clone = CreateInputField (Language.EnterNewSetName, 720, 500 - 75 * maxY / 2 - 60 * shift, 500, 60);
        Clone.GetComponent<InputField> ().text = SetEditor.setName;
        InputObject = Clone;

        if (iconOptions) {

            Clone = CreateUIText (Language.ChoseNewSetIcon, 720, 530 - 75 * maxY / 2, 520, 36);
            Garbage.Add (Clone);
            int maxX = 8;

            IconBackground = new GameObject [maxX, maxY];
            IconImage = new GameObject [maxX, maxY];

            for (int y = 0; y < maxY; y++) {
                for (int x = 0; x < maxX; x++) {
                    int number = x + y * maxX + 1;
                    if (number >= AppDefaults.AvailableAbilities) {
                        continue;
                    }
                    Clone = CreateUIImage ("UI/White", 460 + 75 * x, 505 + 75 * y, 60, 60, true);
                    IconBackground [x, y] = Clone;
                    Clone = CreateUIButton (VisualCard.GetIconPath (number), 460 + 75 * x, 505 + 75 * y, 60, 60, true);
                    Clone.name = UIString.SetEditorSelectIcon;
                    UIController UIC = Clone.GetComponent<UIController> ();
                    UIC.x = x;
                    UIC.y = y;
                    UIC.number = number;
                    IconImage [x, y] = Clone;
                    SelectIcon (x, y, number, false);
                    if (number == SetEditor.iconNumber) {
                        SelectIcon (x, y, number, true);
                    }
                }
            }
        }
        Clone = CreateUIButton ("UI/Butt_M_Apply", 465, 625 + 75 * maxY / 2 + 60 * shift, 90, 90, true);
        Clone.name = UIString.SetEditorApplySetProperties;
        Garbage.Add (Clone);
        Clone = CreateUIButton ("UI/Butt_M_Discard", 975, 625 + 75 * maxY / 2 + 60 * shift, 90, 90, true);
        Clone.name = UIString.DestroySubMenu;
        Garbage.Add (Clone);
    }
}
