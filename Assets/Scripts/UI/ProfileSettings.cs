using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileSettings : GOUI {

    static InputField inputField;
    static int selectedAvatar;
    static GameObject Selection;

    static List<int> ids;

    // Use this for initialization
    void Awake () {
        CreateProfileSettings ();
        CurrentGOUI = this;
    }

    static public void ApplySettings () {
        ClientLogic.MyInterface.CmdSaveProfileSettings (inputField.text, selectedAvatar);
    }

    static public void LoadData (bool [] bools) {
        ids = new List<int> ();
        int count = bools.Length;
        for (int x = 0; x < count; x++) {
            if (bools [x]) {
                ids.Add (x);
            }
        }

        selectedAvatar = ProfileMenu.avatar;

        DestroyMenu ();
        CurrentCanvas.AddComponent<ProfileSettings> ();
    }

    static public void ShowProfileSettings () {
        ClientLogic.MyInterface.CmdDownloadDataToProfileSettings ();
    }

    static public void SelectAvatar (int avatarNumber) {
        selectedAvatar = avatarNumber;
        MoveSelection ();
    }

    static public void MoveSelection () {
        MoveSelection (selectedAvatar);
    }

    static int maxX = 9;
    static int maxY = 2;

    static public void MoveSelection (int avatarNumber) {
        int idsCount = ids.Count;
        for (int y = 0; y < maxY; y++) {
            for (int x = 0; x < maxX; x++) {
                int number = x + y * maxX;
                if (number >= idsCount) {
                    continue;
                }
                number = ids [number];
                if (number == avatarNumber) {
                    SetInPixPosition (Selection, bx + 120 * x, by + 120 * y, 11);
                }
            }
        }
    }

    static int bx = 240;
    static int by = 480;

    static public void CreateProfileSettings () {
        GameObject Clone;
        
        maxY = (ids.Count - 1) / maxX + 1;
        
        Clone = CreateBackgroundSprite ("UI/Panel_Window_01_Sliced", 720, 540, 10, 150 + maxX * 120, 510 + maxY * 120);

        Clone = CreateUIText (Language.UserName + ":", 720, 390 - maxY * 60, 360, 36);
        Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;

        Clone = CreateInputField (Language.EnterNewUserName, 720, 465 - maxY * 60, 540, 60);
        inputField = Clone.GetComponent<InputField> ();
        inputField.text = ProfileMenu.userName;

        by = 645 - maxY * 60;

        Clone = CreateUIText (Language.ChoseNewAvatar + ":", 720, by - 105, 360, 36);
        Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;

        Clone = CreateSprite ("UI/White", bx, by, 11, 120, 120, false);
        Selection = Clone;

        int idsCount = ids.Count;

        for (int y = 0; y < maxY; y++) {
            for (int x = 0; x < maxX; x++) {
                int number = x + y * maxX;
                if (number < idsCount) {
                    number = ids [number];
                    Clone = CreateSprite ("UI/Panel_Window_01_Sliced", bx + 120 * x, by + 120 * y, 12, 90, 90, false);
                    SetSprite (Clone, AppDefaults.avatar [number]);
                    Clone.GetComponent<UIController> ().number = number;
                    Clone.name = UIString.ProfileSettingsAvatar;
                }
            }
        }

        MoveSelection ();

        Clone = CreateSprite ("UI/Butt_M_Apply", 770 - maxX * 60, 675 + maxY * 60, 11, 90, 90, true);
        Clone.name = UIString.ProfileSettingsApply;

        Clone = CreateSprite ("UI/Butt_M_Discard", 680 + maxX * 60, 675 + maxY * 60, 11, 90, 90, true);
        Clone.name = UIString.GoBackToProfileMenu;


    }
}
