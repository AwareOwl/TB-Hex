using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowClass : GOUI {

    public const int SetList = 0;
    public const int GameModeList = 1;
    public const int BoardList = 2;
    public const int RoomUsers = 3;
    public const int RoomList = 4;

    int listMode;

    GameObject RowBackground;
    GameObject Icon;
    GameObject Text;
    GameObject Edit;
    GameObject Option;

    int iconNumber;
    string setName;
    public int setId;

    public void Init (int x, int mode) {
        GameObject Clone;
        listMode = mode;
        RowBackground = CreateSprite ("UI/Panel_Slot_01_Sliced", 720, 330 + x * 90, 11, 540, 90, true);

        int py = 330 + x * 90;

        Text = CreateText ("", 540, py, 13, 0.025f);
        Text.GetComponent<TextMesh> ().anchor = TextAnchor.MiddleLeft;

        Icon = CreateSprite ("UI/Butt_S_Name", 495, py, 12, 60, 60, false);
        Icon.GetComponent<Collider> ().enabled = false;

        Edit = CreateSprite ("UI/Butt_S_Name", 945 - 60, py, 12, 60, 60, true);

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
        this.iconNumber = iconNumber;
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
            case 3:
            case 6:
                switch (listMode) {
                    case (SetList):
                        RowBackground.name = UIString.SelectSet;
                        Edit.name = UIString.ShowSetEditor;
                        break;
                    case (GameModeList):
                        RowBackground.name = UIString.SelectGameMode;
                        Edit.name = UIString.ShowGameModeEditor;
                        break;
                    case (BoardList):
                        RowBackground.name = UIString.GameModeEditorSelectBoard;
                        Edit.name = UIString.GameModeEditorEditBoard;
                        break;
                    case (RoomUsers):
                        RowBackground.name = UIString.CustomGameRoomSelectRow;
                        //Edit.name = UIString.GameModeEditorEditBoard;
                        break;
                    case (RoomList):
                        RowBackground.name = UIString.CustomGameLobbyRow;
                        //Edit.name = UIString.GameModeEditorEditBoard;
                        break;
                }
                break;
            default:
                switch (listMode) {
                    case (RoomUsers):
                        RowBackground.name = UIString.CustomGameRoomSelectRow;
                        break;
                    default:
                        RowBackground.name = "";
                        break;
                }
                break;

        }

        switch (mode) {
            case 0:
                Icon.GetComponent<Renderer> ().enabled = true;
                Icon.GetComponent<Collider> ().enabled = true;
                Edit.GetComponent<Renderer> ().enabled = true;
                Edit.GetComponent<Collider> ().enabled = true;
                Option.GetComponent<Renderer> ().enabled = true;
                Option.GetComponent<Collider> ().enabled = true;
                SetSprite (Option, "UI/Butt_S_Delete", true);
                switch (listMode) {
                    case (SetList):
                        Option.name = UIString.DeleteSet;
                        break;
                    case (GameModeList):
                        Option.name = UIString.DeleteGameMode;
                        break;
                    case (BoardList):
                        Option.name = UIString.GameModeEditorDeleteBoard;
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
                switch (listMode) {
                    case (SetList):
                        Option.name = UIString.CreateNewSet;
                        break;
                    case (GameModeList):
                        Option.name = UIString.CreateNewGameMode;
                        break;
                    case (BoardList):
                        Option.name = UIString.GameModeEditorCreateNewBoard;
                        break;
                    case (RoomUsers):
                        Option.name = UIString.CustomGameRoomAddAI;
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
            case 4:
                Icon.GetComponent<Renderer> ().enabled = true;
                Icon.GetComponent<SpriteRenderer> ().color = Color.white;
                SetSprite (Icon, AppDefaults.Avatar [iconNumber]);
                Icon.GetComponent<Collider> ().enabled = false;
                Edit.GetComponent<Renderer> ().enabled = false;
                Edit.GetComponent<Collider> ().enabled = false;
                Option.GetComponent<Renderer> ().enabled = true;
                Option.GetComponent<Collider> ().enabled = true;
                switch (listMode) {
                    case (RoomUsers):
                        Option.name = UIString.CustomGameKickPlayer;
                        break;
                }
                break;
            case 5:
                Icon.GetComponent<Renderer> ().enabled = true;
                Icon.GetComponent<SpriteRenderer> ().color = Color.white;
                SetSprite (Icon, AppDefaults.Avatar [iconNumber]);
                Icon.GetComponent<Collider> ().enabled = false;
                Edit.GetComponent<Renderer> ().enabled = false;
                Edit.GetComponent<Collider> ().enabled = false;
                Option.GetComponent<Renderer> ().enabled = false;
                Option.GetComponent<Collider> ().enabled = false;
                break;
            case 6:
                Icon.GetComponent<Renderer> ().enabled = false;
                Icon.GetComponent<Collider> ().enabled = false;
                Edit.GetComponent<Renderer> ().enabled = false;
                Edit.GetComponent<Collider> ().enabled = false;
                SetSprite (Option, "UI/Butt_S_Help", true);
                switch (listMode) {
                    case (GameModeList):
                        Option.name = UIString.ShowGameModeEditor;
                        break;
                    case (BoardList):
                        Option.name = UIString.GameModeEditorEditBoard;
                        break;
                }
                Option.GetComponent<Renderer> ().enabled = true;
                Option.GetComponent<Collider> ().enabled = true;
                break;

        }
        if (mode != 3) {
            //RowBackground.GetComponent<UIController> ().id = -1;
            //Edit.GetComponent<UIController> ().id = -1;
            //Option.GetComponent<UIController> ().id = -1;
            setId = -1;
        }
        RowBackground.GetComponent<SpriteRenderer> ().color = Color.white;
    }

    }
