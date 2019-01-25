using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileMenu : GOUI {

    static string userName;
    static int avatar;
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
        CurrentGUI = this;
    }

    static public void ShowProfileMenu () {
        ClientLogic.MyInterface.CmdDownloadProfileData ();
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
        CurrentCanvas.AddComponent<GameModeSettingsEditor> ();
    }


    static public void CreateProfileMenu () {
        GameObject Clone;

        Clone = CreateSprite ("UI/Panel_Window_01_Sliced", 720, 540, 10, 1200, 660, false);

        Clone = CreateSprite ("UI/Panel_Window_01_Sliced", 520, 240, 11, 300, 300, false);
        SetSprite (Clone, AppDefaults.Avatar [2]);

        Clone = CreateSprite ("UI/Butt_S_Name", 720, 240, 11, 60, 60, false);

        Clone = CreateUIText (ClientLogic.MyInterface.UserName, 720, 300, 520, 36);

        Clone = CreateUIText (Language.ThisGameVersion, 720, 440, 520, 36);
        Clone = CreateUIText (Language.AllGameVersions, 920, 440, 520, 36);

        Clone = CreateUIText (Language.WonGames, 620, 540, 520, 36);
        Clone = CreateUIText (Language.LostGames, 620, 640, 520, 36);
        Clone = CreateUIText (Language.DrawnGames, 620, 740, 520, 36);
        Clone = CreateUIText (Language.UnfinishedGames, 620, 840, 520, 36);


    }
}
