using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
                    if (DevelopVersion < 10) {
                        ConvertTo0_7_0_10 ();
                    }
                }
                PathVersion = 8;
                HotfixVersion = 0;
                DevelopVersion = 0;
            }
            if (PathVersion <= 8) {
                if (HotfixVersion <= 0) {
                    if (DevelopVersion < 2) {
                        ConvertTo0_8_0_2 ();
                    }
                    if (DevelopVersion < 5) {
                        ConvertTo0_8_0_5 ();
                    }
                    if (DevelopVersion < 6) {
                        ConvertTo0_8_0_6 ();
                    }
                }
                PathVersion = 9;
                HotfixVersion = 0;
                DevelopVersion = 0;
            }
            if (PathVersion <= 9) {
                if (HotfixVersion <= 0) {
                    if (DevelopVersion < 2) {
                        ConvertTo0_9_0_2 ();
                    }
                    if (DevelopVersion < 3) {
                        ConvertTo0_9_0_3 ();
                    }
                    if (DevelopVersion < 4) {
                        ConvertTo0_9_0_4 ();
                    }
                    if (DevelopVersion < 5) {
                        ConvertTo0_9_0_5 ();
                    }
                    if (DevelopVersion < 6) {
                        ConvertTo0_9_0_6 ();
                    }
                    if (DevelopVersion < 7) {
                        ConvertTo0_9_0_7 ();
                    }
                    if (DevelopVersion < 8) {
                        ConvertTo0_9_0_8 ();
                    }
                }
                PathVersion = 10;
                HotfixVersion = 0;
                DevelopVersion = 0;
            }
            if (PathVersion <= 10) {
                if (DevelopVersion < 1) {
                    ConvertTo0_10_0_1 ();
                }
                if (DevelopVersion < 2) {
                    ConvertTo0_10_0_2 ();
                }
                if (DevelopVersion < 3) {
                    ConvertTo0_10_0_3 ();
                }
                if (DevelopVersion < 4) {
                    ConvertTo0_10_0_4 ();
                }
                if (DevelopVersion < 5) {
                    ConvertTo0_10_0_5 ();
                }
                PathVersion = 11;
                HotfixVersion = 0;
                DevelopVersion = 0;
            }
            if (PathVersion <= 11) {
                if (DevelopVersion < 1) {
                    ConvertTo0_11_0_1 ();
                }
                if (DevelopVersion < 2) {
                    ConvertTo0_11_0_2 ();
                }
                if (DevelopVersion < 3) {
                    ConvertTo0_11_0_3 ();
                }
                if (DevelopVersion < 4) {
                    ConvertTo0_11_0_4 ();
                }
                PathVersion = 12;
                HotfixVersion = 0;
                DevelopVersion = 0;
            }
            if (PathVersion <= 12) {
                if (HotfixVersion <= 0) {
                    if (DevelopVersion < 1) {
                        ConvertTo0_12_0_1 ();
                    }
                    if (DevelopVersion < 2) {
                        ConvertTo0_12_0_2 ();
                    }
                    if (DevelopVersion < 3) {
                        ConvertTo0_12_0_3 ();
                    }
                    HotfixVersion = 1;
                }
                if (HotfixVersion <= 1) {
                    ConvertTo0_12_1_3 ();
                    HotfixVersion = 2;
                }
                if (HotfixVersion <= 2) {
                    ConvertTo0_12_2_3 ();
                }
                PathVersion = 13;
                HotfixVersion = 0;
                DevelopVersion = 0;
            }
            if (PathVersion <= 13) {
                if (HotfixVersion <= 0) {
                    if (DevelopVersion < 1) {
                        ConvertTo0_13_0_1 ();
                    }
                    if (DevelopVersion < 3) {
                        ConvertTo0_13_0_3 ();
                    }
                    if (DevelopVersion < 4) {
                        ConvertTo0_13_0_4 ();
                    }
                    if (DevelopVersion < 5) {
                        ConvertTo0_13_0_5 ();
                    }
                }
                PathVersion = 14;
                HotfixVersion = 0;
                DevelopVersion = 0;
            }
            if (PathVersion <= 14) {
                if (HotfixVersion <= 0) {
                    if (DevelopVersion < 1) {
                        ConvertTo0_14_0_1 ();
                    }
                    if (DevelopVersion < 2) {
                        ConvertTo0_14_0_2 ();
                    }
                    if (DevelopVersion < 3) {
                        ConvertTo0_14_0_3 ();
                    }
                    if (DevelopVersion < 4) {
                        ConvertTo0_14_0_4 ();
                    }
                }
                PathVersion = 15;
                HotfixVersion = 0;
                DevelopVersion = 0;
            }
            if (PathVersion <= 15) {
                if (HotfixVersion <= 0) {
                    if (DevelopVersion < 1) {
                        ConvertTo0_15_0_1 ();
                    }
                    if (DevelopVersion < 2) {
                        ConvertTo0_15_0_2 ();
                    }
                    if (DevelopVersion < 3) {
                        ConvertTo0_15_0_3 ();
                    }
                }
            }
        }
    }

    static public void FinalizeServerVersion () {
        ServerData.SetServerKeyData (ServerData.VersionKey, GetVersion ());



        RatingClass.LoadAbility_AbilitySynergy ();
        RatingClass.LoadAbility_TokenSynergy ();
        RatingClass.LoadToken_TokenSynergy ();

        RatingClass.LoadAbilityAfterAbility ();
        RatingClass.LoadAbilityAfterToken ();
        RatingClass.LoadTokenAfterAbility ();
        RatingClass.LoadTokenAfterToken ();

        RatingClass.LoadAbilityOnRow ();
        RatingClass.LoadTokenOnRow ();
        RatingClass.LoadAbilityTokenOnRow ();

        RatingClass.LoadAbilityStackSize ();
        RatingClass.LoadTokenStackSize ();
        RatingClass.LoadAbilityTokenStackSize ();

        UpdateDefaultServerVersion ();
    }

    static void UpdateDefaultServerVersion () {
        int prevMode = 4;
        int [] officialGameModeIds = ServerData.GetAllOfficialGameModes ();
        foreach (int id in officialGameModeIds) {
            if (ServerData.GetGameModeName (id) == "Version 0.15.0") {
                prevMode = id;
            }
        }
        ServerData.DefaultGameMode = prevMode;
    }

    static bool ratingExported;

    static public void ExportRating () {
        if (ratingExported) {
            return;
        }
        RatingData.SaveRatingAbility_AbilitySynergy (GetResource ("ExportFolder/Rating/Ability_AbilitySynergy"));
        RatingData.SaveRatingAbility_TokenSynergy (GetResource ("ExportFolder/Rating/Ability_TokenSynergy"));
        RatingData.SaveRatingToken_TokenSynergy (GetResource ("ExportFolder/Rating/Token_TokenSynergy"));

        RatingData.SaveRatingAbilityAfterAbility (GetResource ("ExportFolder/Rating/AbilityAfterAbility"));
        RatingData.SaveRatingAbilityAfterToken (GetResource ("ExportFolder/Rating/AbilityAfterToken"));
        RatingData.SaveRatingTokenAfterAbility (GetResource ("ExportFolder/Rating/TokenAfterAbility"));
        RatingData.SaveRatingTokenAfterToken (GetResource ("ExportFolder/Rating/TokenAfterToken"));

        RatingData.SaveRatingAbilityOnRow (GetResource ("ExportFolder/Rating/AbilityOnRow"));
        RatingData.SaveRatingTokenOnRow (GetResource ("ExportFolder/Rating/TokenOnRow"));
        RatingData.SaveRatingAbilityTokenOnRow (GetResource ("ExportFolder/Rating/AbilityTokenOnRow"));

        RatingData.SaveRatingAbilityStackSize (GetResource ("ExportFolder/Rating/AbilityStackSize"));
        RatingData.SaveRatingTokenStackSize (GetResource ("ExportFolder/Rating/TokenStackSize"));
        RatingData.SaveRatingAbilityTokenStackSize (GetResource ("ExportFolder/Rating/AbilityTokenStackSize"));
        ratingExported = true;
    }
    static public void ConvertTo0_15_0_3 () {

        int newId;
        newId = ServerData.CreateNewGameMode ("");
        ServerData.SetCardPool (newId, GetResource ("ExportFolder/v0.15/CardPools/CardPool"));
        ServerData.SetGameModeName (newId, "Version 0.15.0");
        ServerData.SetGameModeIsOfficial (newId, true);

        int [] officialBoards = ServerData.GetAllOfficialNonSpecialBoards ();
        foreach (int offId in officialBoards) {
            string name = ServerData.GetBoardName (offId);
            if (name != "Board 6") {
                ServerData.SetGameModeBoard (newId, offId);
            }
        }

        CopyPlayerGameModeSets ("Version 0.14.0", newId);

        GameVersion = 0;
        PathVersion = 15;
        HotfixVersion = 0;
        DevelopVersion = 3;
    }

    static public void ConvertTo0_15_0_2 () {
        for (int x = 12; x <= 12; x++) {
            int gameModeId = ServerData.CreateNewBoss ("Boss #" + x.ToString (),
            GetResource ("ExportFolder/Bosses/Boards/Board" + x.ToString ()),
            GetResource ("ExportFolder/Bosses/CardPools/CardPool" + x.ToString ()));
        }

        GameVersion = 0;
        PathVersion = 15;
        HotfixVersion = 0;
        DevelopVersion = 2;
    }

    static public void ConvertTo0_15_0_1 () {
        for (int x = 11; x <= 11; x++) {
            int gameModeId = ServerData.CreateNewBoss ("Boss #" + x.ToString (),
            GetResource ("ExportFolder/Bosses/Boards/Board" + x.ToString ()),
            GetResource ("ExportFolder/Bosses/CardPools/CardPool" + x.ToString ()));
        }

        for (int x = 72; x <= 75; x++) {
            int numberOfTurns = 4;
            ServerData.CreateNewPuzzle ("Puzzle #" + x.ToString (),
                        GetResource ("ExportFolder/Puzzles/Boards/Board" + x.ToString ()),
                        GetResource ("ExportFolder/Puzzles/CardPools/CardPool" + x.ToString ()),
                        numberOfTurns);
        }

        GameVersion = 0;
        PathVersion = 15;
        HotfixVersion = 0;
        DevelopVersion = 1;
    }

    static public void ConvertTo0_14_0_4 () {
        ExportRating ();

        GameVersion = 0;
        PathVersion = 14;
        HotfixVersion = 0;
        DevelopVersion = 4;
    }

    static public void ConvertTo0_14_0_3 () {
        for (int x = 6; x <= 10; x++) {
            int gameModeId = ServerData.CreateNewBoss ("Boss #" + x.ToString (),
            GetResource ("ExportFolder/Bosses/Boards/Board" + x.ToString ()),
            GetResource ("ExportFolder/Bosses/CardPools/CardPool" + x.ToString ()));
            switch (x) {
                case 6:
                    ServerData.SetGameModeScoreWinConditionValue (gameModeId, 1000);
                    ServerData.SetGameModeTurnWinConditionValue (gameModeId, 50);
                    break;
                case 7:
                    ServerData.SetGameModeScoreWinConditionValue (gameModeId, 1000);
                    ServerData.SetGameModeTurnWinConditionValue (gameModeId, 50);
                    break;
            }
        }

        GameVersion = 0;
        PathVersion = 14;
        HotfixVersion = 0;
        DevelopVersion = 3;
    }

    static public void ConvertTo0_14_0_2 () {
        ExportRating ();

        int newId;
        newId = ServerData.CreateNewGameMode ("");
        ServerData.SetCardPool (newId, GetResource ("ExportFolder/v0.14/CardPools/CardPool"));
        ServerData.SetGameModeName (newId, "Version 0.14.0");
        ServerData.SetGameModeIsOfficial (newId, true);

        int [] officialBoards = ServerData.GetAllOfficialNonSpecialBoards ();
        foreach (int offId in officialBoards) {
            string name = ServerData.GetBoardName (offId);
            if (name != "Board 6") {
                ServerData.SetGameModeBoard (newId, offId);
            }
        }

        CopyPlayerGameModeSets ("Version 0.13.0", newId);

        GameVersion = 0;
        PathVersion = 14;
        HotfixVersion = 0;
        DevelopVersion = 2;
    }

    static public void ConvertTo0_14_0_1 () {

        for (int x = 61; x <= 71; x++) {
            int numberOfTurns = 4;
            ServerData.CreateNewPuzzle ("Puzzle #" + x.ToString (),
                        GetResource ("ExportFolder/Puzzles/Boards/Board" + x.ToString ()),
                        GetResource ("ExportFolder/Puzzles/CardPools/CardPool" + x.ToString ()),
                        numberOfTurns);
        }

        GameVersion = 0;
        PathVersion = 14;
        HotfixVersion = 0;
        DevelopVersion = 1;
    }

    static public void ConvertTo0_13_0_4 () {
        ExportRating ();

        GameVersion = 0;
        PathVersion = 13;
        HotfixVersion = 0;
        DevelopVersion = 4;
    }

    static public void ConvertTo0_13_0_3 () {
        for (int x = 5; x <= 5; x++) {
            ServerData.CreateNewBoss ("Boss #" + x.ToString (),
            GetResource ("ExportFolder/Bosses/Boards/Board" + x.ToString ()),
            GetResource ("ExportFolder/Bosses/CardPools/CardPool" + x.ToString ()));
        }

        GameVersion = 0;
        PathVersion = 13;
        HotfixVersion = 0;
        DevelopVersion = 3;
    }

    static public void ConvertTo0_13_0_5 () {
        int [] officialGameModes = ServerData.GetAllOfficialGameModes ();
        foreach (int offId in officialGameModes) {
            int [] boardIds = ServerData.GetAllGameModeBoards (offId);
            foreach (int boardId in boardIds) {
                if (ServerData.GetBoardIsBoss (boardId) || ServerData.GetBoardIsTutorial (boardId)){
                    ServerData.RemoveGameModeBoard (offId, boardId);
                }
            }
        }

        GameVersion = 0;
        PathVersion = 13;
        HotfixVersion = 0;
        DevelopVersion = 5;
    }

    static public void ConvertTo0_13_0_1 () {

        int newId;
        newId = ServerData.CreateNewGameMode ("");
        ServerData.SetCardPool (newId, GetResource ("ExportFolder/v0.13/CardPools/CardPool"));
        ServerData.SetGameModeName (newId, "Version 0.13.0");
        ServerData.SetGameModeIsOfficial (newId, true);

        int [] officialBoards = ServerData.GetAllOfficialNonSpecialBoards ();
        foreach (int offId in officialBoards) {
            string name = ServerData.GetBoardName (offId);
            if (name != "Board 6") {
                ServerData.SetGameModeBoard (newId, offId);
            }
        }

        int newBoardId;
        newBoardId = ServerData.SaveNewBoard (newId, "", "Board 13",
        GetResource ("ExportFolder/v0.13/Boards/Board13"));
        ServerData.SetBoardIsOfficial (newBoardId, true);

        CopyPlayerGameModeSets ("Version 0.12.0", newId);


        for (int x = 57; x <= 60; x++) {
            int numberOfTurns = 4;
            ServerData.CreateNewPuzzle ("Puzzle #" + x.ToString (),
                GetResource ("ExportFolder/Puzzles/Boards/Board" + x.ToString ()),
                GetResource ("ExportFolder/Puzzles/CardPools/CardPool" + x.ToString ()),
                numberOfTurns);
        }

        for (int x = 4; x <= 4; x++) {
            ServerData.CreateNewBoss ("Boss #" + x.ToString (),
            GetResource ("ExportFolder/Bosses/Boards/Board" + x.ToString ()),
            GetResource ("ExportFolder/Bosses/CardPools/CardPool" + x.ToString ()));
        }

        GameVersion = 0;
        PathVersion = 13;
        HotfixVersion = 0;
        DevelopVersion = 1;
    }

    static public void ConvertTo0_12_2_3 () {
        GameVersion = 0;
        PathVersion = 12;
        HotfixVersion = 2;
        DevelopVersion = 3;
    }

    static public void ConvertTo0_12_1_3 () {
        GameVersion = 0;
        PathVersion = 12;
        HotfixVersion = 1;
        DevelopVersion = 3;
    }

    static public void ConvertTo0_12_0_3 () {

        int newId;
        newId = ServerData.CreateNewGameMode ("");
        ServerData.SetCardPool (newId, GetResource ("ExportFolder/v0.12/CardPools/CardPool"));
        ServerData.SetGameModeName (newId, "Version 0.12.0");
        ServerData.SetGameModeIsOfficial (newId, true);

        int [] officialBoards = ServerData.GetAllOfficialNonPuzzleBoards ();
        foreach (int offId in officialBoards) {
            string name = ServerData.GetBoardName (offId);
            if (name != "Board 6") {
                ServerData.SetGameModeBoard (newId, offId);
            }
        }

        CopyPlayerGameModeSets ("Version 0.11.0", newId);

        GameVersion = 0;
        PathVersion = 12;
        HotfixVersion = 0;
        DevelopVersion = 3;
    }


    static public void ConvertTo0_12_0_2 () {
        for (int x = 51; x <= 56; x++) {
            int numberOfTurns = 4;
            ServerData.CreateNewPuzzle ("Puzzle #" + x.ToString (),
                GetResource ("ExportFolder/Puzzles/Boards/Board" + x.ToString ()),
                GetResource ("ExportFolder/Puzzles/CardPools/CardPool" + x.ToString ()),
                numberOfTurns);
        }

        GameVersion = 0;
        PathVersion = 12;
        HotfixVersion = 0;
        DevelopVersion = 2;
    }

    static public void ConvertTo0_12_0_1 () {
        ExportRating ();

        for (int x = 3; x <= 3; x++) {
            ServerData.CreateNewBoss ("Boss #" + x.ToString (),
            GetResource ("ExportFolder/Bosses/Boards/Board" + x.ToString ()),
            GetResource ("ExportFolder/Bosses/CardPools/CardPool" + x.ToString ()));
        }

        GameVersion = 0;
        PathVersion = 12;
        HotfixVersion = 0;
        DevelopVersion = 1;
    }

    static public void ConvertTo0_11_0_4 () {

        int newId;
        newId = ServerData.CreateNewGameMode ("");
        ServerData.SetCardPool (newId, GetResource ("ExportFolder/v0.11/CardPools/CardPool"));
        ServerData.SetGameModeName (newId, "Version 0.11.0");
        ServerData.SetGameModeIsOfficial (newId, true);

        int [] officialBoards = ServerData.GetAllOfficialNonPuzzleBoards ();
        foreach (int offId in officialBoards) {
            string name = ServerData.GetBoardName (offId);
            if (name != "Board 6") {
                ServerData.SetGameModeBoard (newId, offId);
            }
        }

        CopyPlayerGameModeSets ("Version 0.10.0", newId);

        GameVersion = 0;
        PathVersion = 11;
        HotfixVersion = 0;
        DevelopVersion = 4;
    }

    static public void ConvertTo0_11_0_3 () {
        for (int x = 2; x <= 2; x++) {
            ServerData.CreateNewBoss ("Boss #" + x.ToString (),
            GetResource ("ExportFolder/Bosses/Boards/Board" + x.ToString ()),
            GetResource ("ExportFolder/Bosses/CardPools/CardPool" + x.ToString ()));
        }

        GameVersion = 0;
        PathVersion = 11;
        HotfixVersion = 0;
        DevelopVersion = 3;
    }

    static public void ConvertTo0_11_0_2 () {
        ExportRating ();

        GameVersion = 0;
        PathVersion = 11;
        HotfixVersion = 0;
        DevelopVersion = 2;
    }

    static public void ConvertTo0_11_0_1 () {
        string [] users = ServerData.GetAllUsers ();
        for (int x = 45; x <= 50; x++) {
            int numberOfTurns = 4;
            ServerData.CreateNewPuzzle ("Puzzle #" + x.ToString (),
                GetResource ("ExportFolder/Puzzles/Boards/Board" + x.ToString ()),
                GetResource ("ExportFolder/Puzzles/CardPools/CardPool" + x.ToString ()),
                numberOfTurns);
        }

        GameVersion = 0;
        PathVersion = 11;
        HotfixVersion = 0;
        DevelopVersion = 1;
    }

    static public void ConvertTo0_10_0_5 () {

        string [] users = ServerData.GetAllUsers ();
        for (int x = 1; x <= 4; x++) {
            ServerData.CreateNewTutorial ("Tutorial #" + x.ToString (),
            GetResource ("ExportFolder/Tutorials/Boards/Board" + x.ToString ()));
        }

        GameVersion = 0;
        PathVersion = 10;
        HotfixVersion = 0;
        DevelopVersion = 5;
    }

    static public void ConvertTo0_10_0_4 () {
        string [] users = ServerData.GetAllUsers ();
        for (int x = 1; x <= 1; x++) {
            ServerData.CreateNewBoss ("Boss #" + x.ToString (),
            GetResource ("ExportFolder/Bosses/Boards/Board" + x.ToString ()),
            GetResource ("ExportFolder/Bosses/CardPools/CardPool" + x.ToString ()));
        }

        GameVersion = 0;
        PathVersion = 10;
        HotfixVersion = 0;
        DevelopVersion = 4;
    }

    static public void ConvertTo0_10_0_3 () {

        ExportRating ();

        GameVersion = 0;
        PathVersion = 10;
        HotfixVersion = 0;
        DevelopVersion = 3;
    }

    static public void ConvertTo0_10_0_2 () {

        int newId;
        newId = ServerData.CreateNewGameMode ("");
        ServerData.SetCardPool (newId, GetResource ("ExportFolder/v0.10/CardPools/CardPool"));
        ServerData.SetGameModeName (newId, "Version 0.10.0");
        ServerData.SetGameModeIsOfficial (newId, true);

        int [] officialBoards = ServerData.GetAllOfficialNonPuzzleBoards ();
        foreach (int offId in officialBoards) {
            string name = ServerData.GetBoardName (offId);
            if (name != "Board 6") {
                ServerData.SetGameModeBoard (newId, offId);
            }
        }

        int newBoardId;
        newBoardId = ServerData.SaveNewBoard (newId, "", "Board 12",
        GetResource ("ExportFolder/v0.10/Boards/Board12"));
        ServerData.SetBoardIsOfficial (newBoardId, true);

        CopyPlayerGameModeSets ("Version 0.9.0", newId);

        GameVersion = 0;
        PathVersion = 10;
        HotfixVersion = 0;
        DevelopVersion = 2;
    }

    static public void ConvertTo0_10_0_1 () {
        string [] users = ServerData.GetAllUsers ();
        for (int x = 44; x <= 44; x++) {
            int numberOfTurns = 4;
            ServerData.CreateNewPuzzle ("Puzzle #" + x.ToString (),
                GetResource ("ExportFolder/Puzzles/Boards/Board" + x.ToString ()),
                GetResource ("ExportFolder/Puzzles/CardPools/CardPool" + x.ToString ()),
                numberOfTurns);
        }

        GameVersion = 0;
        PathVersion = 10;
        HotfixVersion = 0;
        DevelopVersion = 1;
    }

    static public void ConvertTo0_9_0_8 () {
        int [] gameModes = ServerData.GetAllOfficialGameModes ();
        foreach (int gameMode in gameModes) {
            if (ServerData.GetGameModeName (gameMode) == "Puzzle #30") {
                int boardId = ServerData.GetAllGameModeBoards (gameMode) [0];
                ServerData.SetBoard (boardId, GetResource ("ExportFolder/Puzzles/Boards/Board30"));
            }
        }

        GameVersion = 0;
        PathVersion = 9;
        HotfixVersion = 0;
        DevelopVersion = 8;
    }


    static public void ConvertTo0_9_0_7 () {

        int newId;
        newId = ServerData.CreateNewGameMode ("");
        ServerData.SetCardPool (newId, GetResource ("ExportFolder/v0.9/CardPools/CardPool"));
        ServerData.SetGameModeName (newId, "Version 0.9.0");
        ServerData.SetGameModeIsOfficial (newId, true);

        int [] officialBoards = ServerData.GetAllOfficialNonPuzzleBoards ();
        foreach (int offId in officialBoards) {
            string name = ServerData.GetBoardName (offId);
            if (name != "Board 6") {
                Debug.Log (offId);
                ServerData.SetGameModeBoard (newId, offId);
            }
        }

        CopyPlayerGameModeSets ("Version 0.7.0", newId);

        GameVersion = 0;
        PathVersion = 9;
        HotfixVersion = 0;
        DevelopVersion = 7;
    }


    static public void ConvertTo0_9_0_6 () {
        ExportRating ();

        GameVersion = 0;
        PathVersion = 9;
        HotfixVersion = 0;
        DevelopVersion = 6;
    }

    static public void ConvertTo0_9_0_5 () {
        int [] gameModes = ServerData.GetAllGameModes ();
        foreach (int gameMode in gameModes) {
            if (ServerData.GetGameModeIsOfficial (gameMode)) {
                ServerData.SaveGameModeOwners (gameMode, new string [0]);
            }
        }
        int [] boards = ServerData.GetAllBoards ();
        foreach (int board in boards) {
            if (ServerData.GetBoardIsOfficial (board)) {
                ServerData.SaveBoardOwners (board, new string [0]);
            }
        }
        string [] users = ServerData.GetAllUsers ();
        foreach (string user in users) {
            int [] boardProperties = ServerData.GetUserBoardProperties (user);
            List<int> gameModeIds = new List<int> ();
            List<int> boardIds = new List<int> ();
            int count = boardProperties.Length;
            for (int x = 0; x < count; x++) {
                int id = boardProperties [x];
                if (ServerData.BoardContentExists (id) && !ServerData.GetBoardIsOfficial (id)) {
                    boardIds.Add (id);
                }
                if (ServerData.GameModeContentExists (id) && !ServerData.GetGameModeIsOfficial (id)) {
                    gameModeIds.Add (id);
                }
            }
            ServerData.SaveUserBoardProperties (user, boardIds.ToArray());
            ServerData.SaveUserGameModeProperties (user, gameModeIds.ToArray ());
        }

        GameVersion = 0;
        PathVersion = 9;
        HotfixVersion = 0;
        DevelopVersion = 5;
    }

    static public void ConvertTo0_9_0_4 () {
        string [] users = ServerData.GetAllUsers ();
        for (int x = 31; x <= 43; x++) {
            int numberOfTurns = 4;
            ServerData.CreateNewPuzzle ("Puzzle #" + x.ToString (),
                GetResource ("ExportFolder/Puzzles/Boards/Board" + x.ToString ()),
                GetResource ("ExportFolder/Puzzles/CardPools/CardPool" + x.ToString ()),
                numberOfTurns);
        }

        GameVersion = 0;
        PathVersion = 9;
        HotfixVersion = 0;
        DevelopVersion = 4;
    }

    static public void ConvertTo0_9_0_3 () {
        string [] users = ServerData.GetAllUsers ();
        foreach (string user in users) {
            ServerLogic.LevelUpReward (null, user, ServerData.GetUserLevel (user));
        }

        GameVersion = 0;
        PathVersion = 9;
        HotfixVersion = 0;
        DevelopVersion = 3;
    }

    static public void ConvertTo0_9_0_2 () {
        string [] users = ServerData.GetAllUsers ();
        foreach (string user in users) {
            string path;
            int count;
            path = ServerData.UserUnlockedAbilitiesPath (user);
            File.Delete (path);
            path = ServerData.UserUnlockedTokensPath (user);
            File.Delete (path);
            int [] puzzleId = ServerData.GetUserFinishedPuzzles (user);
            count = puzzleId.Length;
            for (int x = 0; x < count; x++) {
                ServerLogic.SavePuzzleResult (null, user, puzzleId [x]);
            }
        }

        GameVersion = 0;
        PathVersion = 9;
        HotfixVersion = 0;
        DevelopVersion = 2;
    }

    static public void ConvertTo0_8_0_6 () {

        string [] users = ServerData.GetAllUsers ();
        foreach (string user in users) {
            int [] puzzleId = ServerData.GetUserFinishedPuzzles (user);
            int count = puzzleId.Length;
            for (int x = 0; x < count; x++) {
                ServerLogic.SavePuzzleResult (null, user, puzzleId [x]);
            }
        }

        GameVersion = 0;
        PathVersion = 8;
        HotfixVersion = 0;
        DevelopVersion = 6;
    }

    static public void ConvertTo0_8_0_5 () {
        for (int x = 18; x <= 30; x++) {
            int numberOfTurns = 4;
            ServerData.CreateNewPuzzle ("Puzzle #" + x.ToString (),
                GetResource ("ExportFolder/Puzzles/Boards/Board" + x.ToString ()),
                GetResource ("ExportFolder/Puzzles/CardPools/CardPool" + x.ToString ()),
                numberOfTurns);
        }

        GameVersion = 0;
        PathVersion = 8;
        HotfixVersion = 0;
        DevelopVersion = 5;
    }

    static public void ConvertTo0_8_0_2 () {
        for (int x = 7; x <= 17; x++) {
            int numberOfTurns = 4;
            switch (x) {
                case 8:
                    numberOfTurns = 3;
                    break;
            }
            ServerData.CreateNewPuzzle ("Puzzle #" + x.ToString (),
                GetResource ("ExportFolder/Puzzles/Boards/Board" + x.ToString ()),
                GetResource ("ExportFolder/Puzzles/CardPools/CardPool" + x.ToString ()),
                numberOfTurns);
        }

        GameVersion = 0;
        PathVersion = 8;
        HotfixVersion = 0;
        DevelopVersion = 2;
    }

    static public void ConvertTo0_7_0_10 () {

        int newId;
        newId = ServerData.CreateNewGameMode ("");
        ServerData.SetCardPool (newId, GetResource ("ExportFolder/v0.7/CardPools/CardPool"));
        ServerData.SetGameModeName (newId, "Version 0.7.0");
        ServerData.SetGameModeIsOfficial (newId, true);

        int newBoardId;
        newBoardId = ServerData.SaveNewBoard (newId, "Path0.7.0.0", "Board 11",
        GetResource ("ExportFolder/v0.7/Boards/Board11"));
        ServerData.SetBoardIsOfficial (newBoardId, true);

        int [] officialBoards = ServerData.GetAllOfficialNonPuzzleBoards ();
        foreach (int offId in officialBoards) {
            string name = ServerData.GetBoardName (offId);
            if (name != "Board 6") {
                Debug.Log (offId);
                ServerData.SetGameModeBoard (newId, offId);
            }
        }

        CopyPlayerGameModeSets ("Version 0.6.0", newId);


        GameVersion = 0;
        PathVersion = 7;
        HotfixVersion = 0;
        DevelopVersion = 10;
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
            ServerData.CreateNewPuzzle ("Puzzle #" + x.ToString (),
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

        int [] officialBoards = ServerData.GetAllOfficialNonPuzzleBoards ();
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

        int [] officialBoards = ServerData.GetAllOfficialNonPuzzleBoards ();
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

        int [] boardIds = ServerData.GetAllNonPuzzleBoards ();
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

        int [] officialBoards = ServerData.GetAllOfficialNonPuzzleBoards ();
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
