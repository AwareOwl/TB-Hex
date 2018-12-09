using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPropertiesMenu : GOUI {

    static GameObject InputObject;
    static GameObject BackgroundObject;
    static GameObject [,] IconBackground;
    static GameObject [,] IconImage;
    static List<GameObject> Garbage = new List<GameObject> ();

    static int iconX;
    static int iconY;
    static int iconNumber;

    public void Start () {
        CreateSetPropertiesMenu ();
        CurrentSubGUI = this;
    }

    override public void DestroyThis () {
        foreach (GameObject obj in IconBackground) {
            DestroyImmediate (obj);
        }
        foreach (GameObject obj in IconImage) {
            DestroyImmediate (obj);
        }
        foreach (GameObject obj in Garbage) {
            DestroyImmediate (obj);
        }
        if (InputObject != null) {
            DestroyImmediate (InputObject);
        }
        if (BackgroundObject != null) {
            DestroyImmediate (BackgroundObject);
        }
    }

    static public void ShowSetPropertiesMenu () {
        CurrentCanvas.AddComponent<SetPropertiesMenu> ();
    }

    static public void ApplySetProperties () {
        SetEditor.setName = InputObject.GetComponent<InputField> ().text;
        SetEditor.iconNumber = iconNumber;
        SetEditor.ApplySetProperties ();
        DestroySubMenu ();
    }

    static public void SelectIcon (int x, int y, int number) {
        SelectIcon (iconX, iconY, iconNumber, false);
        SelectIcon (x, y, number, true);
        iconX = x;
        iconY = y;
        iconNumber = number;
    }

    static public void SelectIcon (int x, int y, int number, bool enabled) {
        if (enabled) {
            IconBackground [x, y].GetComponent<Image> ().color = Color.white;
            IconImage [x, y].GetComponent<Image> ().color = AppDefaults.GetAbilityColor (number);
        } else {
            IconBackground [x, y].GetComponent<Image> ().color = new Color (0.3f, 0.3f, 0.3f);
            IconImage [x, y].GetComponent<Image> ().color = AppDefaults.GetAbilityColor (number);
        }
    }

    static public void CreateSetPropertiesMenu () {
        GameObject Clone;

        BackgroundObject = CreateUIImage ("UI/Panel_Window_01_Sliced", 720, 540, 750, 750, false);
        BackgroundObject.GetComponent<Image> ().type = Image.Type.Tiled;

        Clone = CreateUIImage ("UI/Transparent", 720, 540, 10000, 10000, false);
        Clone.transform.SetParent (BackgroundObject.transform);
        Garbage.Add (Clone);

        Clone = CreateUIText (Language.EnterNewSetName, 720, 255, 520, 36);
        Garbage.Add (Clone);

        Clone = CreateInputField (Language.EnterNewSetName, 720, 330, 500, 60);
        Clone.GetComponent<InputField> ().text = SetEditor.setName;
        InputObject = Clone;

        Clone = CreateUIText (Language.ChoseNewSetIcon, 720, 420, 520, 36);
        Garbage.Add (Clone);

        int maxY = 3;
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
        Clone = CreateUIButton ("UI/Butt_M_Apply", 465, 795, 90, 90, true);
        Clone.name = UIString.SetEditorApplySetProperties;
        Garbage.Add (Clone);
        Clone = CreateUIButton ("UI/Butt_M_Discard", 975, 795, 90, 90, true);
        Clone.name = UIString.DestroySubMenu;
        Garbage.Add (Clone);
    }
}
