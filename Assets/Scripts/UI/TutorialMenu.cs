using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMenu : GOUI {

    static RowClass [] row;

    static string [] officialNames;
    static int [] officialIds;

    static int selectedRow = -1;
    static int selectedId = -1;

    // Use this for initialization
    void Start () {
        CurrentGOUI = this;
        CreateTutorialMenu ();
        RefreshPage ();
    }

    static public void ShowTutorialMenu () {
        ClientLogic.MyInterface.CmdDownloadDataToTutorialMenu ();
    }

    static public void LoadTutorialMenu (string [] officialNames, int [] officialIds) {
        TutorialMenu.officialNames = officialNames;
        TutorialMenu.officialIds = officialIds;
        DestroyMenu ();
        CurrentCanvas.AddComponent<TutorialMenu> ();
    }

    static public void RefreshPage () {
        int numberOfRows = row.Length;
        for (int x = 1; x < numberOfRows; x++) {
            int number = x - 1;
            int count = officialNames.Length;
            row [x].FreeRow ();
            if (number < count) {
                row [x].SetState (GameModeNameToMissionName ( officialNames [number]), officialIds [number], 0, true);
                row [x].SetState (3);
                if (officialIds [number] == selectedId) {
                    row [x].SelectRow ();
                }
            }
        }
    }

    static public void Apply () {
        if (selectedId > -1) {
            ClientLogic.MyInterface.CmdStartTutorial (selectedId, GameModeNameToMissionName (officialNames [selectedRow - 1]));
        }
    }

    static public int GameModeNameToTutorialNumber (string gameModeName) {
        return int.Parse (gameModeName.Substring (gameModeName.IndexOf ('#') + 1));
    }

    static public string GameModeNameToMissionName (string gameModeName) {
        return Language.Mission + " #" + GameModeNameToTutorialNumber (gameModeName);
    }


    static public void SelectMission (int id) {
        selectedId = id;
        for (int x = 1; x < row.Length; x++) {
            row [x].FreeRow ();
            if (row [x].setId == id && selectedId > 0) {
                row [x].SelectRow ();
                selectedRow = x;
            }
        }
    }

    static void CreateTutorialMenu () {
        GameObject Clone;
        GameObject Text;

        Clone = CreateSprite ("UI/Panel_Window_01_Sliced", 720, 585, 10, 690, 750, false);
        DestroyImmediate (Clone.GetComponent<UIController> ());

        Text = CreateText (Language.Tutorial, 450, 320, 12, 0.035f);
        Text.GetComponent<TextMesh> ().anchor = TextAnchor.MiddleLeft;
        Text.GetComponent<TextMesh> ().fontStyle = FontStyle.Bold;

        row = new RowClass [5];
        for (int x = 1; x < 5; x++) {
            row [x] = CurrentGOUI.gameObject.AddComponent<RowClass> ();
            row [x].Init (x, ListMode.TutorialList);
        }

        Clone = CreateSprite ("UI/Butt_M_Apply", 495, 840, 11, 90, 90, true);
        Clone.name = UIString.TutorialMenuApply;

        Clone = CreateSprite ("UI/Butt_M_Discard", 945, 840, 11, 90, 90, true);
        Clone.name = UIString.ShowMainMenu;

    }


}
