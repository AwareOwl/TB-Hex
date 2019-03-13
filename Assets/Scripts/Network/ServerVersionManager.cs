using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerVersionManager : VersionManager {

    static public void CheckServerVersion () {
        string version = ServerData.GetServerKeyData (ServerData.VersionKey);

        ConvertStringToVersion (version);

        if (GameVersion < 1) {
            if (PathVersion < 1) {
                if (HotfixVersion < 1) {
                    if (DevelopVersion < 34) {
                        ConvertTo0_0_0_34 ();
                    }
                    if (DevelopVersion < 37) {
                        ConvertTo0_0_0_37 ();
                    }
                    HotfixVersion = 1;
                    DevelopVersion = 0;
                }
                PathVersion = 1;
                HotfixVersion = 0;
                DevelopVersion = 0;
            }
            if (PathVersion <= 1) {
                if (HotfixVersion <= 1) {
                    if (DevelopVersion < 10) {
                        ConvertTo0_1_1_10 ();
                    }
                    if (DevelopVersion < 11) {
                        ConvertTo0_1_1_11 ();
                    }
                    if (DevelopVersion < 12) {
                        ConvertTo0_1_1_12 ();
                    }
                    if (DevelopVersion < 13) {
                        ConvertTo0_1_1_13 ();
                    }
                }
                PathVersion = 2;
                HotfixVersion = 0;
                DevelopVersion = 0;
            }
            if (PathVersion <= 2) {
                if (HotfixVersion <= 0) {
                    if (DevelopVersion < 1) {
                        ConvertTo0_2_0_1 ();
                    }
                    if (DevelopVersion < 11) {
                        ConvertTo0_2_0_11 ();
                    }
                    if (DevelopVersion < 12) {
                        ConvertTo0_2_0_12 ();
                    }
                    if (DevelopVersion < 13) {
                        ConvertTo0_2_0_13 ();
                    }
                }
                PathVersion = 3;
                HotfixVersion = 0;
                DevelopVersion = 0;
            }
            if (PathVersion <= 3) {
                PathVersion = 4;
                HotfixVersion = 0;
                DevelopVersion = 0;
            }
            if (PathVersion <= 4) {
                if (HotfixVersion <= 0) {
                    if (DevelopVersion < 7) {
                        ConvertTo0_4_0_7 ();
                    }
                    if (DevelopVersion < 8) {
                        ConvertTo0_4_0_8 ();
                    }
                    if (DevelopVersion < 11) {
                        ConvertTo0_4_0_11 ();
                    }
                }
                PathVersion = 5;
                HotfixVersion = 0;
                DevelopVersion = 0;
            }
            if (PathVersion <= 5) {
                if (HotfixVersion <= 0) {
                    if (DevelopVersion < 5) {
                        ConvertTo0_5_0_5 ();
                    }
                }
                PathVersion = 6;
                HotfixVersion = 0;
                DevelopVersion = 0;
            }
            if (PathVersion <= 6) {
                if (HotfixVersion <= 0) {
                    if (DevelopVersion < 5) {
                        ConvertTo0_6_0_5 ();
                    }
                    if (DevelopVersion < 11) {
                        ConvertTo0_6_0_11 ();
                    }
                }
                PathVersion = 7;
                HotfixVersion = 0;
                DevelopVersion = 0;
            }
            if (PathVersion <= 7) {
                if (HotfixVersion <= 0) {
                    if (DevelopVersion < 6) {
                        ConvertTo0_7_0_6 ();
                    }
                    if (DevelopVersion < 9) {
                        ConvertTo0_7_0_9 ();
                    }
                }
            }
        }
    }

    static public void FinalizeServerVersion () {
        ServerData.SetServerKeyData (ServerData.VersionKey, GetVersion ());
        RatingClass.LoadAbility_AbilitySynergy ();
        RatingClass.LoadAbility_TokenSynergy ();

        RatingClass.LoadAbilityAfterAbility ();
        RatingClass.LoadAbilityAfterToken ();
        RatingClass.LoadTokenAfterAbility ();
        RatingClass.LoadTokenAfterToken ();

        RatingClass.LoadAbilityOnRow ();
        RatingClass.LoadAbilityStackSize ();
        RatingClass.LoadTokenOnRow ();
        RatingClass.LoadTokenStackSize ();
        RatingClass.LoadAbilityTokenOnRow ();
    }

    static public void ExportRating () {
        RatingData.SaveRatingAbility_AbilitySynergy (GetResource ("ExportFolder/Rating/AbilityAbilitySynergy"));
        RatingData.SaveRatingAbility_TokenSynergy (GetResource ("ExportFolder/Rating/AbilityTokenSynergy"));

        RatingData.SaveRatingAbilityAfterAbility (GetResource ("ExportFolder/Rating/AbilityAfterAbility"));
        RatingData.SaveRatingAbilityAfterToken (GetResource ("ExportFolder/Rating/AbilityAfterToken"));
        RatingData.SaveRatingTokenAfterAbility (GetResource ("ExportFolder/Rating/TokenAfterAbility"));
        RatingData.SaveRatingTokenAfterToken (GetResource ("ExportFolder/Rating/TokenAfterToken"));

        RatingData.SaveRatingAbilityOnRow (GetResource ("ExportFolder/Rating/AbilityOnRow"));
        RatingData.SaveRatingAbilityStackSize (GetResource ("ExportFolder/Rating/AbilityStackSize"));
        RatingData.SaveRatingTokenOnRow (GetResource ("ExportFolder/Rating/TokenOnRow"));
        RatingData.SaveRatingTokenStackSize (GetResource ("ExportFolder/Rating/TokenStackSize"));
        RatingData.SaveRatingAbilityTokenOnRow (GetResource ("ExportFolder/Rating/AbilityTokenOnRow"));
    }


    static public void ConvertTo0_7_0_9 () {
        ExportRating ();

        GameVersion = 0;
        PathVersion = 7;
        HotfixVersion = 0;
        DevelopVersion = 9;
    }

    static public void ConvertTo0_7_0_6 () {
        for (int x = 1; x <= 6; x++) {
            int numberOfTurns = 3;
            switch (x) {
                case 1:
                case 6:
                    numberOfTurns = 2;
                    break;
                case 4:
                    numberOfTurns = 4;
                    break;
            }
            ServerData.CreateNewPuzzle ("Path0.6.0", "Puzzle #" + x.ToString (),
                GetResource ("ExportFolder/Puzzles/Boards/Board" + x.ToString ()),
                GetResource ("ExportFolder/Puzzles/CardPools/CardPool" + x.ToString ()),
                numberOfTurns);
        }
        
        GameVersion = 0;
        PathVersion = 7;
        HotfixVersion = 0;
        DevelopVersion = 6;
    }

    static public void ConvertTo0_6_0_11 () {
        ExportRating ();
        GameVersion = 0;
        PathVersion = 6;
        HotfixVersion = 0;
        DevelopVersion = 11;
    }

    //static public void 
    static public void ConvertTo0_6_0_5 () {
        int newId;
        newId = ServerData.CreateNewGameMode ("");
        ServerData.SetCardPool (newId, GetResource ("ExportFolder/v0.6/CardPools/CardPool"));
        ServerData.SetGameModeName (newId, "Version 0.6.0");
        ServerData.SetGameModeIsOfficial (newId, true);

        int newBoardId;
        for (int x = 0; x < 2; x++) {
            newBoardId = ServerData.SaveNewBoard (newId, "Path0.6.0.0", "Board " + (9 + x).ToString (),
            GetResource ("ExportFolder/v0.6/Boards/Board" + (9 + x).ToString ()));
            ServerData.SetBoardIsOfficial (newBoardId, true);
        }

        int gM1 = 4;
        int [] officialGameModeIds = ServerData.GetAllOfficialGameModes ();
        foreach (int id in officialGameModeIds) {
            if (ServerData.GetGameModeName (id) == "Version 0.1.0") {
                gM1 = id;
            }
        }

        int [] gM1boards = ServerData.GetAllGameModeBoards (gM1);
        foreach (int gmb in gM1boards) {
            ServerData.RemoveGameModeBoard (gM1, gmb);
        }

        int [] officialBoards = ServerData.GetAllOfficialBoards ();
        foreach (int offId in officialBoards) {
            string name = ServerData.GetBoardName (offId);
            switch (name) {
                case "Board 1":
                case "Board 2":
                case "Board 3":
                case "Board 4":
                    ServerData.SetGameModeBoard (gM1, offId);
                    break;

            }
            if (name == "Board6") {
                ServerData.SetBoardName (offId, "Board 6");
            }
            if (name == "Board7") {
                ServerData.SetBoardName (offId, "Board 7");
            }
            if (name == "Board8") {
                ServerData.SetBoardName (offId, "Board 8");
            }
            if (name != "Board6") {
                ServerData.SetGameModeBoard (newId, offId);
            }
        }

        CopyPlayerGameModeSets ("Version 0.5.0", newId);

        GameVersion = 0;
        PathVersion = 6;
        HotfixVersion = 0;
        DevelopVersion = 5;
    }

    static public void ConvertTo0_5_0_5 () {
        ExportRating ();

        int newId;

        newId = ServerData.CreateNewGameMode ("");
        ServerData.SetCardPool (newId, GetResource ("ExportFolder/v0.5/CardPools/CardPool"));
        ServerData.SetGameModeName (newId, "Version 0.5.0");
        ServerData.SetGameModeIsOfficial (newId, true);

        int [] officialBoards = ServerData.GetAllOfficialBoards ();
        foreach (int offId in officialBoards) {
            ServerData.SetGameModeBoard (newId, offId);
        }


        int prevMode = 4;
        int [] officialGameModeIds = ServerData.GetAllOfficialGameModes ();
        foreach (int id in officialGameModeIds) {
            if (ServerData.GetGameModeName (id) == "Version 0.4.0") {
                prevMode = id;
            }
        }

        string [] users = ServerData.GetAllUsers ();
        foreach (string user in users) {
            int [] ids = ServerData.GetAllPlayerModeSets (user, prevMode);
            foreach (int id in ids) {
                int newSetId = ServerData.CreatePlayerModeSet (user, newId, ServerData.GetPlayerModeSet (user, prevMode, id), ServerData.GetPlayerModeSetName (user, prevMode, id));
                ServerData.SetPlayerModeSetIconNumber (user, newId, newSetId, ServerData.GetPlayerModeSetIconNumber (user, prevMode, id));
            }
            if (ServerData.GetUserSelectedGameMode (user) == prevMode) {
                ServerData.SetUserSelectedGameMode (user, newId);
            }
        }

        GameVersion = 0;
        PathVersion = 5;
        HotfixVersion = 0;
        DevelopVersion = 5;
    }

    static public void ConvertTo0_4_0_11 () {
        GameVersion = 0;
        PathVersion = 4;
        HotfixVersion = 0;
        DevelopVersion = 11;
    }

    static public void ConvertTo0_4_0_8 () {
        int id4 = 3;
        int [] officialGameModeIds = ServerData.GetAllOfficialGameModes ();
        foreach (int id in officialGameModeIds) {
            if (ServerData.GetGameModeName (id) == "Version 0.4.0") {
                id4 = id;
            }
        }

        string [] users = ServerData.GetAllUsers ();
        foreach (string user in users) {
            int prevMode = 3;
            int [] ids = ServerData.GetAllPlayerModeSets (user, prevMode);
            foreach (int id in ids) {
                int newId = ServerData.CreatePlayerModeSet (user, id4, ServerData.GetPlayerModeSet (user, prevMode, id), ServerData.GetPlayerModeSetName (user, prevMode, id));
                ServerData.SetPlayerModeSetIconNumber (user, id4, newId, ServerData.GetPlayerModeSetIconNumber (user, prevMode, id));
            }
            if (ServerData.GetUserSelectedGameMode (user) == prevMode) {
                ServerData.SetUserSelectedGameMode (user, id4);
            }
        }

        GameVersion = 0;
        PathVersion = 4;
        HotfixVersion = 0;
        DevelopVersion = 8;
    }

    static public void ConvertTo0_4_0_7 () {

        int [] boardIds = ServerData.GetAllBoards ();
        foreach (int boardId in boardIds) {
            ServerData.SetBoardMatchTypes (boardId, new int [] { 0, 1 });
        }

        int newId;

        for (int y = 1; y <= 5; y++) {
            ServerData.SetBoardIsOfficial (y, true);
        }

        newId = ServerData.CreateNewGameMode ("");
        ServerData.SetCardPool (newId, GetResource ("ExportFolder/v0.4/CardPools/CardPool"));
        ServerData.SetGameModeName (newId, "Version 0.4.0");
        ServerData.SetGameModeIsOfficial (newId, true);

        int [] officialBoards = ServerData.GetAllOfficialBoards ();
        foreach (int offId in officialBoards) {
            ServerData.SetGameModeBoard (newId, offId);
        }

        int newBoardId;
        for (int x = 0; x < 3; x++) {
            newBoardId = ServerData.SaveNewBoard (newId, "Path0.4.0.0", "Board" + (6 + x).ToString (),
                GetResource ("ExportFolder/v0.4/Boards/Board" + (6 + x).ToString ()));
            ServerData.SetBoardIsOfficial (newBoardId, true);
            switch (x) {
                case 1:
                    ServerData.RemoveBoardMatchType (newBoardId, 1);
                    ServerData.AddBoardMatchType (newBoardId, 3);
                    break;
                case 2:
                    ServerData.RemoveBoardMatchType (newBoardId, 1);
                    ServerData.AddBoardMatchType (newBoardId, 2);
                    break;
            }
        }

        GameVersion = 0;
        PathVersion = 4;
        HotfixVersion = 0;
        DevelopVersion = 7;
    }

    static public void ConvertTo0_2_0_13 () {
        string [] users = ServerData.GetAllUsers ();
        foreach (string user in users) {
            ServerData.SetUserSelectedGameMode (user, 3);
            int [] ids = ServerData.GetAllPlayerModeSets (user, 2);
            foreach (int id in ids) {
                int newId = ServerData.CreatePlayerModeSet (user, 3, ServerData.GetPlayerModeSet (user, 2, id), ServerData.GetPlayerModeSetName (user, 2, id));
                ServerData.SetPlayerModeSetIconNumber (user, 3, newId, ServerData.GetPlayerModeSetIconNumber (user, 2, id));
            }
        }
        GameVersion = 0;
        PathVersion = 2;
        HotfixVersion = 0;
        DevelopVersion = 13;
    }

    static public void ConvertTo0_2_0_12 () {
        for (int y = 1; y <= 5; y++) {
            ServerData.SetBoardName (y, "Board " + y.ToString());
        }
        GameVersion = 0;
        PathVersion = 2;
        HotfixVersion = 0;
        DevelopVersion = 12;
    }

    static public void ConvertTo0_2_0_11 () {
        ServerData.SetGameModeIsOfficial (1, true);
        ServerData.SetGameModeIsOfficial (2, true);
        ServerData.SetCardPool (3, GetResource ("ExportFolder/v0.3/CardPools/CardPool"));
        ServerData.SetGameModeName (3, "Version 0.3.0");
        for (int y = 1; y <= 5; y++) {
            ServerData.SetGameModeBoard (3, y);
        }
        ServerData.SetGameModeIsOfficial (3, true);
        GameVersion = 0;
        PathVersion = 2;
        HotfixVersion = 0;
        DevelopVersion = 11;
    }

    static public void ConvertTo0_2_0_1 () {
        GameVersion = 0;
        PathVersion = 2;
        HotfixVersion = 0;
        DevelopVersion = 1;
    }

    static public void ConvertTo0_1_1_13 () {
        ServerData.SetGameModeName (1, "Version 0.1.0");
        ServerData.SetGameModeName (2, "Version 0.2.0");
        GameVersion = 0;
        PathVersion = 1;
        HotfixVersion = 1;
        DevelopVersion = 13;
    }

    static public void ConvertTo0_1_1_12 () {
        ServerData.SaveNewBoard (1, "Path0.2.0.0", "Board5", GetResource ("ExportFolder/v0.2/Boards/Board5"));
        ServerData.SetGameModeBoard (2, 5);
        GameVersion = 0;
        PathVersion = 1;
        HotfixVersion = 1;
        DevelopVersion = 12;
    }

    static public void ConvertTo0_1_1_11 () {
        ServerData.SetGameModeName (1, "Version 0.1.0");
        ServerData.SetGameModeName (2, "Version 0.2.0");
        GameVersion = 0;
        PathVersion = 1;
        HotfixVersion = 1;
        DevelopVersion = 11;
    }

    static public void ConvertTo0_1_1_10 () {
        ServerData.SetCardPool (2, GetResource ("ExportFolder/v0.2/CardPools/CardPool"));
        for (int x = 1; x < 3; x++) {
            for (int y = 1; y < 5; y++) {
                ServerData.SetGameModeBoard (x, y);
            }
        }
        GameVersion = 0;
        PathVersion = 1;
        HotfixVersion = 1;
        DevelopVersion = 10;
    }

    static public void ConvertTo0_0_0_37 () {
        string [] s = ServerData.GetAllUsers ();
        foreach (string s2 in s) {
            ServerData.SetUserKeyData (s2, ServerData.PasswordKey,
                ServerData.EncryptString (ServerData.UserPassword (s2)));
            HandClass hand = new HandClass ();
            hand.GenerateRandomHand (1, null);
            ServerData.SavePlayerModeSet (s2, 1, 1, hand.HandToString ());
        }
        DevelopVersion = 37;
    }

    static public void ConvertTo0_0_0_34 () {
        ServerData.SaveNewBoard (1, "Path0.1.0.0", "Board1", GetResource ("ExportFolder/v0.1/Boards/Board1"));
        ServerData.SaveNewBoard (1, "Path0.1.0.0", "Board2", GetResource ("ExportFolder/v0.1/Boards/Board2"));
        ServerData.SaveNewBoard (1, "Path0.1.0.0", "Board3", GetResource ("ExportFolder/v0.1/Boards/Board3"));
        ServerData.SaveNewBoard (1, "Path0.1.0.0", "Board4", GetResource ("ExportFolder/v0.1/Boards/Board4"));
        ServerData.SetCardPool (1, GetResource ("ExportFolder/v0.1/CardPools/CardPool"));
        GameVersion = 0;
        PathVersion = 0;
        HotfixVersion = 0;
        DevelopVersion = 34;
    }

    static public void CopyPlayerGameModeSets (string prevModeName,  int newId) {

        int prevMode = 4;
        int [] officialGameModeIds = ServerData.GetAllOfficialGameModes ();
        foreach (int id in officialGameModeIds) {
            if (ServerData.GetGameModeName (id) == prevModeName) {
                prevMode = id;
            }
        }

        string [] users = ServerData.GetAllUsers ();
        foreach (string user in users) {
            int [] ids = ServerData.GetAllPlayerModeSets (user, prevMode);
            foreach (int id in ids) {
                int newSetId = ServerData.CreatePlayerModeSet (user, newId, ServerData.GetPlayerModeSet (user, prevMode, id), ServerData.GetPlayerModeSetName (user, prevMode, id));
                ServerData.SetPlayerModeSetIconNumber (user, newId, newSetId, ServerData.GetPlayerModeSetIconNumber (user, prevMode, id));
            }
            if (ServerData.GetUserSelectedGameMode (user) == prevMode) {
                ServerData.SetUserSelectedGameMode (user, newId);
            }
        }
    }
}
