using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileSettings : GOUI {

    // Use this for initialization
    void Awake () {
        CreateProfileSettings ();
        CurrentGUI = this;
    }

    static public void ShowProfileSettings () {

        DestroyMenu ();
        CurrentCanvas.AddComponent<ProfileSettings> ();
    }

    static public void CreateProfileSettings () {
        GameObject Clone;
        
        Clone = CreateSprite ("UI/Panel_Window_01_Sliced", 720, 540, 10, 1110, 810, false);

        Clone = CreateUIText (Language.UserName + ":", 720, 240, 360, 36);
        Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;

        Clone = CreateInputField (Language.EnterNewUserName, 720, 315, 500, 60);
        Clone.GetComponent<InputField> ().text = SetEditor.setName;

        int bx = 345;
        int by = 510;

        Clone = CreateUIText (Language.ChoseNewAvatar + ":", 720, by - 105, 360, 36);
        Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;

        Clone = CreateSprite ("UI/White", bx, by, 11, 150, 150, false);

        for (int y = 0; y < 2; y++) {
            for (int x = 0; x < 6; x++) {
                int number = x + y * 6 + 1;
                if (number < AppDefaults.AvailableAvatars) {
                    Clone = CreateSprite ("UI/Panel_Window_01_Sliced", bx + 150 * x, by + 150 * y, 12, 120, 120, false);
                    SetSprite (Clone, AppDefaults.Avatar [number]);
                }
            }
        }

        Clone = CreateSprite ("UI/Butt_M_Apply", 285, 825, 11, 90, 90, true);
        Clone.name = UIString.ShowMainMenu;

        Clone = CreateSprite ("UI/Butt_M_Discard", 1155, 825, 11, 90, 90, true);
        Clone.name = UIString.ShowMainMenu;


    }
}
