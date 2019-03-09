using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileMenu : GOUI {

    static string userName;
    static public int avatar;
    static int thisModeWon;
    static int thisModeLost;
    static int thisModeDrawn;
    static int thisModeUnfinished;
    static int totalWon;
    static int totalLost;
    static int totalDrawn;
    static int totalUnfinished;

    // Use this for initialization
    void Awake () {
        CreateProfileMenu ();
        CurrentGOUI = this;
    }

    static public void ShowProfileMenu () {
        ClientLogic.MyInterface.CmdDownloadProfileData ();
    }

    static public void GoBackToProfileMenu () {
        DestroyMenu ();
        CurrentCanvas.AddComponent<ProfileMenu> ();
    }

    static public void ShowProfileMenu (string userName, int avatar) {
        ProfileMenu.userName = userName;
        ProfileMenu.avatar = avatar;

        DestroyMenu ();
        CurrentCanvas.AddComponent<ProfileMenu> ();
    }

    static public void ShowProfileMenu (
        string userName, int avatar, int thisModeWon, int thisModeLost, int thisModeDrawn, int thisModeUnfinished, int totalWon, int totalLost, int totalDrawn, int totalUnfinished) {
        //ClientLogic.MyInterface.CmdDownloadGameModeSettings (GameModeEditor.gameModeId);

        ProfileMenu.userName = userName;
        ProfileMenu.avatar = avatar;
        ProfileMenu.thisModeWon = thisModeWon;
        ProfileMenu.thisModeLost = thisModeLost;
        ProfileMenu.thisModeDrawn = thisModeDrawn;
        ProfileMenu.thisModeUnfinished = thisModeUnfinished;
        ProfileMenu.totalWon = totalWon;
        ProfileMenu.totalLost = totalLost;
        ProfileMenu.totalDrawn = totalDrawn;
        ProfileMenu.totalUnfinished = totalUnfinished;

        DestroyMenu ();
        CurrentCanvas.AddComponent<ProfileMenu> ();
    }


    static public void CreateProfileMenu () {
        GameObject Clone;

        Clone = CreateSprite ("UI/Panel_Window_01_Sliced", 720, 540, 10, 1200, 660, false);

        Clone = CreateSprite ("UI/Panel_Window_01_Sliced", 360, 525, 11, 300, 300, false);
        SetSprite (Clone, AppDefaults.Avatar [avatar]);

        Clone = CreateUIText (ClientLogic.MyInterface.UserName, 330, 315, 240, 36);
        Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;

        Clone = CreateSprite ("UI/Butt_S_Name", 480, 315, 11, 60, 60, false);
        Clone.name = UIString.ShowProfileSettings;

        int firstRowY = 390;
        int firstColumnX = 690;
        int secondColumnX = 885;
        int thirdColumnX = 1110;

        Clone = CreateUIText (Language.ThisGameVersion, secondColumnX, firstRowY, 520, 24);
        Clone = CreateUIText (Language.AllGameVersions, thirdColumnX, firstRowY, 520, 24);

        Clone = CreateUIText (Language.WonGames, firstColumnX, firstRowY + 60 * 1, 240, 24);
        Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;
        Clone = CreateUIText (Language.LostGames, firstColumnX, firstRowY + 60 * 2, 240, 24);
        Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;
        Clone = CreateUIText (Language.DrawnGames, firstColumnX, firstRowY + 60 * 3, 240, 24);
        Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;
        Clone = CreateUIText (Language.UnfinishedGames, firstColumnX, firstRowY + 60 * 4, 240, 24);
        Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;

        Clone = CreateUIText (thisModeWon.ToString (), secondColumnX, firstRowY + 60 * 1, 520, 24);
        Clone = CreateUIText (thisModeLost.ToString (), secondColumnX, firstRowY + 60 * 2, 520, 24);
        Clone = CreateUIText (thisModeDrawn.ToString (), secondColumnX, firstRowY + 60 * 3, 520, 24);
        Clone = CreateUIText (thisModeUnfinished.ToString (), secondColumnX, firstRowY + 60 * 4, 520, 24);

        Clone = CreateUIText (totalWon.ToString (), thirdColumnX, firstRowY + 60 * 1, 520, 24);
        Clone = CreateUIText (totalLost.ToString (), thirdColumnX, firstRowY + 60 * 2, 520, 24);
        Clone = CreateUIText (totalDrawn.ToString (), thirdColumnX, firstRowY + 60 * 3, 520, 24);
        Clone = CreateUIText (totalUnfinished.ToString (), thirdColumnX, firstRowY + 60 * 4, 520, 24);

        Clone = CreateSprite ("UI/Butt_M_Discard", 1200, 755, 11, 90, 90, true);
        Clone.name = UIString.ShowMainMenu;


    }
}
