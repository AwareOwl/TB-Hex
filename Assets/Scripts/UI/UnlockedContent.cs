using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockedContent : GOUI {

    static bool [] unlockedToken;
    static bool [] unlockedAbility;

    // Use this for initialization
    void Start () {
        CreateUnlockedContentMenu ();
        CurrentGOUI = this;
    }

    static public void ShowUnlockedContent () {
        ClientLogic.MyInterface.CmdDownloadDataToPuzzleMenu ();
    }

    static public void LoadUnlockedContent (bool [] unlockedToken, bool [] unlockedAbility) {
        DestroyMenu ();
        UnlockedContent.unlockedToken = unlockedToken;
        UnlockedContent.unlockedAbility = unlockedAbility;
        CurrentCanvas.AddComponent<PuzzleMenu> ();
    }

    static public void CreateUnlockedContentMenu () {

    }

}
