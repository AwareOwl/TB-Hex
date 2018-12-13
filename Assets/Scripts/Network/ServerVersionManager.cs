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
                    DevelopVersion = 0;
                }
                HotfixVersion = 1;
            }
            PathVersion = 1;
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
            }
        }

    }

    static public void FinalizeServerVersion () {
        ServerData.SetServerKeyData (ServerData.VersionKey, GetVersion ());
        RatingClass.LoadAbilityAbilitySynergy ();
        RatingClass.LoadAbilityAfterAbility ();
        RatingClass.LoadAbilityOnRow ();
        RatingClass.LoadTokenOnRow ();
    }
    /*
    static public void ConvertTo0_1_1_5 () {
        string [] s = ServerData.GetAllUsers ();

        foreach (string s2 in s) {
            string [] s3 = ServerData.GetAllPlayerModes (s2);
            foreach (string s4 in s3) {
                int i4 = int.Parse (s4);
                string [] s5 = ServerData.GetAllPlayerModeSets (s2, i4);
                foreach (string s6 in s5) {
                    int i6 = int.Parse (s6);
                    ServerData.SetPlayerModeSetIconNumber (s2, i4, i6, 1);
                }
            }
        }
        PathVersion = 1;
        HotfixVersion = 1;
        DevelopVersion = 4;
    }

    static public void ConvertTo0_1_1_4 () {
        string [] s = ServerData.GetAllUsers ();

        foreach (string s2 in s) {
            string [] s3 = ServerData.GetAllPlayerModes (s2);
            foreach (string s4 in s3) {
                int i4 = int.Parse (s4);
                string [] s5 = ServerData.GetAllPlayerModeSets (s2, i4);
                foreach (string s6 in s5) {
                    int i6 = int.Parse (s6);

                    ServerData.SetPlayerModeSetName (s2, i4, i6, Language.StartingSet);
                }
            }
        }
        PathVersion = 1;
        HotfixVersion = 1;
        DevelopVersion = 4;
    }*/
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
        ServerData.SaveRatingAbilityAbilitySynergy (GetResource ("ExportFolder/Rating/AbilityAbilitySynergy"));
        ServerData.SaveRatingAbilityAfterAbility (GetResource ("ExportFolder/Rating/AbilityAfterAbility"));
        ServerData.SaveRatingAbilityOnRow (GetResource ("ExportFolder/Rating/AbilityOnRow"));
        ServerData.SaveRatingTokenOnRow (GetResource ("ExportFolder/Rating/TokenOnRow"));
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
            hand.GenerateRandomHand ();
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
        ServerData.SaveRatingAbilityAbilitySynergy (GetResource ("ExportFolder/Rating/AbilityAbilitySynergy"));
        ServerData.SaveRatingAbilityAfterAbility (GetResource ("ExportFolder/Rating/AbilityAfterAbility"));
        ServerData.SaveRatingAbilityOnRow (GetResource ("ExportFolder/Rating/AbilityOnRow"));
        GameVersion = 0;
        PathVersion = 0;
        HotfixVersion = 0;
        DevelopVersion = 34;
    }
}
