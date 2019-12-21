using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModePlayerSettingsMenu : GOUI {
   

    static int [] downloadedStatuses;
    static Dropdown [] dropdowns = new Dropdown [4];

    void Start () {
        CreateGameModePlayerSettingsMenu ();
        CurrentGOUI = this;
    }

    static public void ShowGameModePlayerSettingsMenu () {
        ClientLogic.MyInterface.CmdDownloadGameModePlayerSettings (GameModeEditor.gameModeId);
    }

    static public void LoadGameModePlayerSettingsMenu (int [] statuses) {
        downloadedStatuses = statuses;
        DestroyMenu ();
        CurrentCanvas.AddComponent<GameModePlayerSettingsMenu> ();
    }

    static public void Apply () {
        int [] statuses = new int [5];
        for (int x = 0; x < 4; x++) {
            statuses [x + 1] = dropdowns [x].value;
        }
        ClientLogic.MyInterface.CmdSaveGameModePlayerSettings (GameModeEditor.gameModeId, statuses);
    }

    static public void CreateGameModePlayerSettingsMenu () {

        GameObject Clone = CreateBackgroundSprite ("UI/Panel_Window_01_Sliced", 740, 520, 10, 960, 780);
        DestroyImmediate (Clone.GetComponent<UIController> ());
        GameObject.DestroyImmediate (Clone.GetComponent<Collider> ());

        bool edit = GameModeEditor.editMode;

        Clone = CreateUIText (Language.PlayerStatuses + ":", 600, 240, 500, 36);
        Text text = Clone.GetComponent<Text> ();
        text.alignment = TextAnchor.MiddleLeft;


        List<Dropdown.OptionData> allOptions = new List<Dropdown.OptionData> ();
        for (int x = 0; x < Language.StatusDescription.Length; x++) {
            allOptions.Add (new Dropdown.OptionData (Language.StatusDescription [x]));
        }

        for (int x = 0; x < 4; x++) {
            int py = 360 + 90 * x;
            Clone = CreateUIText (Language.Player + " " + (x + 1).ToString(), 600, py, 500, 24);
            text = Clone.GetComponent<Text> ();
            text.alignment = TextAnchor.MiddleLeft;
            Clone = CreateUIDropdown (815, py, 600, 60);
            dropdowns [x] = Clone.GetComponent<Dropdown> ();
            Clone.transform.Find ("Label").GetComponent<Text> ().fontSize = 20;
            Transform trans;
            trans = Clone.transform.Find ("Template");
            trans = trans.Find ("Viewport");
            trans = trans.Find ("Content");
            trans = trans.Find ("Item");
            trans.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, 54);
            trans = trans.Find ("Item Label");
            RectTransform rect = trans.GetComponent<RectTransform> ();
            rect.offsetMax = new Vector2 (-10, rect.offsetMax.y);
            dropdowns [x].options = allOptions;
            dropdowns [x].value = downloadedStatuses [x + 1];
            dropdowns [x].RefreshShownValue ();
            dropdowns [x].interactable = edit;
        }

        if (edit) {
            Clone = CreateSprite ("UI/Butt_M_Apply", 375, 795, 11, 90, 90, true);
            Clone.name = UIString.GameModePlayerSettingsApply;
        }

        Clone = CreateSprite ("UI/Butt_M_Discard", 1110, 795, 11, 90, 90, true);
        Clone.GetComponent<UIController> ().id = GameModeEditor.gameModeId;
        Clone.name = UIString.ShowGameModeEditor;
    }
}
