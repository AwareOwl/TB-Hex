using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ListMode {
    SetList = 0,
    GameModeList = 1,
    BoardList = 2,
    RoomUsers = 3,
    RoomList = 4,
    TutorialList = 5
}

public class RowClass : GOUI {

    ListMode listMode;

    GameObject RowBackground;
    GameObject Icon;
    GameObject Text;
    GameObject Edit;
    GameObject Option;

    GameObject numberBackground;
    GameObject numberText;

    int iconNumber;
    string setName;
    public int setId;

    public void Init (int x, ListMode mode) {
        GameObject Clone;
        listMode = mode;
        RowBackground = CreateSprite ("UI/Panel_Slot_01_Sliced", 720, 330 + x * 90, 11, 540, 90, true);

        int py = 330 + x * 90;

        Text = CreateText ("", 540, py, 13, 0.025f);
        Text.GetComponent<TextMesh> ().anchor = TextAnchor.MiddleLeft;

        Icon = CreateSprite ("UI/Butt_S_Name", 495, py, 12, 60, 60, false);
        Icon.GetComponent<Collider> ().enabled = false;
        Icon.GetComponent<UIController> ().number = x;

        Edit = CreateSprite ("UI/Butt_S_Name", 945 - 60, py, 12, 60, 60, true);

        Option = CreateSprite ("UI/Butt_S_Delete", 945, py, 12, 60, 60, true);

        if (listMode == ListMode.RoomUsers) {
            int px2 = 495 + 15;
            int py2 = py + 15;
            numberBackground = CreateSprite ("UI/White", px2, py2, 12, 30, 30, false);
            numberBackground.GetComponent<Collider> ().enabled = false;
            GameObject.DestroyImmediate (numberBackground.GetComponent<Collider> ());
            SetSpriteColor (numberBackground, Color.black);
            
            numberText = CreateText ("1", px2, py2, 13, 0.02f);
            numberText.GetComponent<Renderer> ().material.color = Color.white;
        }

        SetState (2);
    }

    public void SetTeam (int number) {
        numberText.GetComponent<TextMesh> ().text = number.ToString();
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

    void Enable (GameObject [] objects, bool enabled) {
        foreach (GameObject obj in objects) {
            if (obj == null) {
                continue;
            }
            Renderer renderer = obj.GetComponent<Renderer> ();
            Collider collider = obj.GetComponent<Collider> ();
            if (renderer != null) {
                renderer.enabled = enabled;
            }
            if (collider != null) {
                collider.enabled = enabled;
            }
        }
    }

    public void SetState (int mode) {
        switch (mode) {
            case 0:
            case 3:
            case 6:
                switch (listMode) {
                    case (ListMode.SetList):
                        RowBackground.name = UIString.SelectSet;
                        Edit.name = UIString.ShowSetEditor;
                        break;
                    case (ListMode.GameModeList):
                        RowBackground.name = UIString.SelectGameMode;
                        Edit.name = UIString.ShowGameModeEditor;
                        break;
                    case (ListMode.BoardList):
                        RowBackground.name = UIString.GameModeEditorSelectBoard;
                        Edit.name = UIString.GameModeEditorEditBoard;
                        break;
                    case (ListMode.RoomUsers):
                        RowBackground.name = UIString.CustomGameRoomSelectRow;
                        Icon.name = UIString.CustomGameRoomAvatar;
                        break;
                    case (ListMode.RoomList):
                        RowBackground.name = UIString.CustomGameLobbyRow;
                        break;
                    case (ListMode.TutorialList):
                        RowBackground.name = UIString.TutorialMenuRow;
                        break;
                }
                break;
            default:
                switch (listMode) {
                    case (ListMode.RoomUsers):
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
                Enable (new GameObject [] { Icon, Edit, Option, numberBackground, numberText }, true);
                /*Icon.GetComponent<Renderer> ().enabled = true;
                Icon.GetComponent<Collider> ().enabled = true;
                Edit.GetComponent<Renderer> ().enabled = true;
                Edit.GetComponent<Collider> ().enabled = true;
                Option.GetComponent<Renderer> ().enabled = true;
                Option.GetComponent<Collider> ().enabled = true;*/
                SetSprite (Option, "UI/Butt_S_Delete", true);
                switch (listMode) {
                    case (ListMode.SetList):
                        Option.name = UIString.DeleteSet;
                        break;
                    case (ListMode.GameModeList):
                        Option.name = UIString.DeleteGameMode;
                        break;
                    case (ListMode.BoardList):
                        Option.name = UIString.GameModeEditorDeleteBoard;
                        break;
                }
                break;
            case 1:
                Enable (new GameObject [] { Icon, Edit, numberBackground, numberText }, false);
                Enable (new GameObject [] { Option }, true);
                /*Icon.GetComponent<Renderer> ().enabled = false;
                Icon.GetComponent<Collider> ().enabled = false;
                Edit.GetComponent<Renderer> ().enabled = false;
                Edit.GetComponent<Collider> ().enabled = false;
                Option.GetComponent<Renderer> ().enabled = true;
                Option.GetComponent<Collider> ().enabled = true;*/
                SetSprite (Option, "UI/Butt_S_Add", true);
                Text.GetComponent<TextMesh> ().text = Language.EmptySlot;
                switch (listMode) {
                    case (ListMode.SetList):
                        Option.name = UIString.CreateNewSet;
                        break;
                    case (ListMode.GameModeList):
                        Option.name = UIString.CreateNewGameMode;
                        break;
                    case (ListMode.BoardList):
                        Option.name = UIString.GameModeEditorCreateNewBoard;
                        break;
                    case (ListMode.RoomUsers):
                        Option.name = UIString.CustomGameRoomAddAI;
                        break;
                }
                break;
            case 2:
            case 3:
                Enable (new GameObject [] { Icon, Edit, Option, numberBackground, numberText }, false);
                /*Icon.GetComponent<Renderer> ().enabled = false;
                Icon.GetComponent<Collider> ().enabled = false;
                Edit.GetComponent<Renderer> ().enabled = false;
                Edit.GetComponent<Collider> ().enabled = false;
                Option.GetComponent<Renderer> ().enabled = false;
                Option.GetComponent<Collider> ().enabled = false;*/
                if (mode == 2) {
                    Text.GetComponent<TextMesh> ().text = "";
                }
                break;
            case 4:
                Enable (new GameObject [] { Edit }, false);
                Enable (new GameObject [] { Icon, Option, numberBackground, numberText }, true);
                Icon.GetComponent<SpriteRenderer> ().color = Color.white;
                SetSprite (Icon, AppDefaults.avatar [iconNumber]);
                /*Icon.GetComponent<Collider> ().enabled = false;
                Edit.GetComponent<Renderer> ().enabled = false;
                Edit.GetComponent<Collider> ().enabled = false;
                Option.GetComponent<Renderer> ().enabled = true;
                Option.GetComponent<Collider> ().enabled = true;*/
                switch (listMode) {
                    case (ListMode.RoomUsers):
                        Option.name = UIString.CustomGameKickPlayer;
                        break;
                }
                break;
            case 5:
                Enable (new GameObject [] { Edit }, false);
                Enable (new GameObject [] { Icon, Option, numberBackground, numberText }, true);
                Icon.GetComponent<SpriteRenderer> ().color = Color.white;
                SetSprite (Icon, AppDefaults.avatar [iconNumber]);
                /*Icon.GetComponent<Collider> ().enabled = false;
                Edit.GetComponent<Renderer> ().enabled = false;
                Edit.GetComponent<Collider> ().enabled = false;
                Option.GetComponent<Renderer> ().enabled = false;
                Option.GetComponent<Collider> ().enabled = false;*/
                break;
            case 6:
                Icon.GetComponent<Renderer> ().enabled = false;
                Icon.GetComponent<Collider> ().enabled = false;
                Edit.GetComponent<Renderer> ().enabled = false;
                Edit.GetComponent<Collider> ().enabled = false;
                SetSprite (Option, "UI/Butt_S_Help", true);
                switch (listMode) {
                    case (ListMode.GameModeList):
                        Option.name = UIString.ShowGameModeEditor;
                        break;
                    case (ListMode.BoardList):
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
