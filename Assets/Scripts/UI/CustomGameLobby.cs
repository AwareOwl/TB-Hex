using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGameLobby : GOUI {

    static RowClass [] row;

    static int selectedId = -1;
    static int currentPage = 0;
    static PageUI pageUI;

    static int [] ids;
    static string [] names;
    static int [] matchTypes;
    static int [] filledSlots;

    // Use this for initialization
    void Start () {
        CurrentGUI = this;
        CreateCustomGameLobby ();
        ClientLogic.MyInterface.CmdLeaveCustomGame ();
        ClientLogic.MyInterface.CmdDownloadListOfCustomGames ();
    }

    static public void ShowCustomGameLobby () {
        DestroyMenu ();
        CurrentCanvas.AddComponent<CustomGameLobby> ();
    }

    static public void SelectRow (int id) {
        selectedId = id;
        ShowPage ();
    }

    static public void JoinCustomGameRoom () {
        if (selectedId != -1) {
            JoinCustomGameRoom (selectedId);
        }
        ShowMessage (Language.BeforeApplyingSelectAnyOption);
    }

    static public void JoinCustomGameRoom (int id) {
        ClientLogic.MyInterface.CmdJoinCustomGameRoom (id);
    }

    static public void LoadData (int [] ids, string [] names, int [] matchTypes, int [] filledSlots) {
        currentPage = 0;
        CustomGameLobby.ids = ids;
        CustomGameLobby.names = names;
        CustomGameLobby.matchTypes = matchTypes;
        CustomGameLobby.filledSlots = filledSlots;
        ShowPage ();
        RefreshPageButtons ();
    }

    static public void RefreshPageButtons () {
        int count = ids.Length;
        int pageLimit = (count) / 5 + 1;
        pageUI.Init (9, pageLimit, new Vector2Int (480, 780), UIString.CustomGameLobbyPageButton);
    }

    static public void ShowPage () {
        ShowPage (currentPage);
    }

    static public void ShowPage (int pageNumber) {
        currentPage = pageNumber;
        for (int x = 0; x < 5; x++) {
            int number = currentPage * 5 + x;
            int count = ids.Length;
            row [x].FreeRow ();
            if (number < count) {
                string name = names [number];
                name += " (" + CustomGameClass.GetMatchTypeName (matchTypes [number]);
                name += " " + filledSlots [number].ToString () + "/";
                name += CustomGameClass.GetNumberOfSlots (matchTypes [number]) + ")";
                row [x].SetState (name, ids [number], 0, true);
                row [x].SetState (3);
            } else {
                row [x].SetState (2);
            }
            if (number >= 0 && number < ids.Length && ids [number] == selectedId) {
                row [x].SelectRow ();
            }
        }
        pageUI.SelectPage (currentPage);
    }
    
    
    static public void CreateCustomGameLobby () {
        GameObject Clone;
        CreateSprite ("UI/Panel_Window_01_Sliced", 720, 540, 10, 690, 960, false);

        Clone = CreateUIText (Language.AvailableGames + ":", 720, 155, 520, 36);

        Clone = CreateSpriteWithText ("UI/Butt_M_EmptySquare", Language.CreateNewGame, 720, 240, 11, 530, 90);
        Clone.name = UIString.ShowCustomGameEditor;

        row = new RowClass [5];
        for (int x = 0; x < 5; x++) {
            row [x] = CurrentGUI.gameObject.AddComponent<RowClass> ();
            row [x].Init (x, RowClass.RoomList);
        }

        pageUI = new PageUI ();


        Clone = CreateSprite ("UI/Butt_M_Apply", 495, 900, 11, 90, 90, true);
        Clone.name = UIString.CustomGameLobbyApply;
        
        Clone = CreateSprite ("UI/Butt_S_Revert", 855, 900, 11, 90, 90, true);
        Clone.name = UIString.RefreshCustomGameLobby;

        Clone = CreateSprite ("UI/Butt_M_Discard", 945, 900, 11, 90, 90, true);
        Clone.name = UIString.ShowMainMenu;
    }

}
