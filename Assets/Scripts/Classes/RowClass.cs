using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowClass : GOUI {

    public const int SetList = 0;
    public const int GameModeList = 1;

    int mode;

    GameObject RowBackground;
    GameObject Icon;
    GameObject Text;
    GameObject Edit;
    GameObject Option;

    string setName;
    public int setId;

    public void Init (int x, int mode) {
        GameObject Clone;
        RowBackground = CreateSprite ("UI/Panel_Slot_01_Sliced", 720, 330 + x * 90, 11, 540, 90, true);
        RowBackground.name = UIString.SelectSet;

        int py = 330 + x * 90;

        Text = CreateText ("", 540, py, 13, 0.025f);
        Text.GetComponent<TextMesh> ().anchor = TextAnchor.MiddleLeft;

        Icon = CreateSprite ("UI/Butt_S_Name", 495, py, 12, 60, 60, false);
        Icon.GetComponent<Collider> ().enabled = false;

        Edit = CreateSprite ("UI/Butt_S_Name", 945 - 60, py, 12, 60, 60, true);
        switch (mode) {
            case (SetList):
                Edit.name = UIString.ShowSetEditor;
                break;
            case (GameModeList):
                Edit.name = UIString.ShowGameModeEditor;
                break;
        }

        Option = CreateSprite ("UI/Butt_S_Delete", 945, py, 12, 60, 60, true);

        SetState (2);
    }

    public void SelectRow () {
        RowBackground.GetComponent<UIController> ().PressAndLock ();
    }

    public void FreeRow () {
        RowBackground.GetComponent<UIController> ().FreeAndUnlcok ();
    }

    public void SetState (string setName, int setId, int iconNumber, bool legal) {
        SpriteRenderer editRenderer = Edit.GetComponent<SpriteRenderer> ();
        Text.GetComponent<TextMesh> ().text = setName;
        SetSprite (Icon, VisualCard.GetIconPath (iconNumber));
        Icon.GetComponent<SpriteRenderer> ().color = AppDefaults.GetAbilityColor (iconNumber);

        SetState (0);
        this.setId = setId;
        RowBackground.GetComponent<UIController> ().id = setId;
        Edit.GetComponent<UIController> ().id = setId;
        Option.GetComponent<UIController> ().id = setId;
        if (!legal) {
            RowBackground.GetComponent<SpriteRenderer> ().color = new Color (1, 0.65f, 0.65f);
        }
    }

    public void SetState (int mode) {
        switch (mode) {
            case 0:
                Icon.GetComponent<Renderer> ().enabled = true;
                Icon.GetComponent<Collider> ().enabled = true;
                Edit.GetComponent<Renderer> ().enabled = true;
                Edit.GetComponent<Collider> ().enabled = true;
                Option.GetComponent<Renderer> ().enabled = true;
                Option.GetComponent<Collider> ().enabled = true;
                SetSprite (Option, "UI/Butt_S_Delete", true);
                switch (mode) {
                    case (SetList):
                        Option.name = UIString.DeleteSet;
                        break;
                    case (GameModeList):
                        Option.name = UIString.DeleteGameMode;
                        break;
                }
                break;
            case 1:
                Icon.GetComponent<Renderer> ().enabled = false;
                Icon.GetComponent<Collider> ().enabled = false;
                Edit.GetComponent<Renderer> ().enabled = false;
                Edit.GetComponent<Collider> ().enabled = false;
                Option.GetComponent<Renderer> ().enabled = true;
                Option.GetComponent<Collider> ().enabled = true;
                SetSprite (Option, "UI/Butt_S_Add", true);
                Text.GetComponent<TextMesh> ().text = Language.EmptySlot;
                switch (mode) {
                    case (SetList):
                        Option.name = UIString.CreateNewSet;
                        break;
                    case (GameModeList):
                        Option.name = UIString.CreateNewGameMode;
                        break;
                }
                break;
            case 2:
            case 3:
                Icon.GetComponent<Renderer> ().enabled = false;
                Icon.GetComponent<Collider> ().enabled = false;
                Edit.GetComponent<Renderer> ().enabled = false;
                Edit.GetComponent<Collider> ().enabled = false;
                Option.GetComponent<Renderer> ().enabled = false;
                Option.GetComponent<Collider> ().enabled = false;
                if (mode == 2) {
                    Text.GetComponent<TextMesh> ().text = "";
                }
                break;

        }
        RowBackground.GetComponent<UIController> ().id = -1;
        Edit.GetComponent<UIController> ().id = -1;
        Option.GetComponent<UIController> ().id = -1;
        setId = -1;
        RowBackground.GetComponent<SpriteRenderer> ().color = Color.white;
    }

    }
