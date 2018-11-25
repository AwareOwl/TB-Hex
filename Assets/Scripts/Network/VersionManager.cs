﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionManager  {

    static public string version;

    static public int GameVersion;
    static public int PathVersion;
    static public int HotfixVersion;
    static public int DevelopVersion;

    static public void CheckServerVersion () {
        string version = ServerData.GetServerKeyData (ServerData.VersionKey);

        if (version == null || version == "") {
            GameVersion = 0;
            PathVersion = 0;
            HotfixVersion = 0;
            DevelopVersion = 0;
        } else {
            string [] words = version.Split ('.');
            GameVersion = int.Parse (words [0]);
            PathVersion = int.Parse (words [1]);
            HotfixVersion = int.Parse (words [2]);
            DevelopVersion = int.Parse (words [3]);
        }

        if (GameVersion < 1) {
            if (PathVersion < 1) {
                if (HotfixVersion < 1) {
                    if (DevelopVersion < 34) {
                        ConvertTo0_0_0_34 ();
                    }
                    if (DevelopVersion < 37) {
                        ConvertTo0_0_0_37 ();
                    }
                }
            }
        }

    }

    static public void FinalizeServerVersion () {
        ServerData.SetServerKeyData (ServerData.VersionKey, GetServerVersion ());
        RatingClass.LoadAbilityAbilitySynergy ();
        RatingClass.LoadAbilityAfterAbility ();
        RatingClass.LoadAbilityOnRow ();
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
        ServerData.SaveNewBoard ("Path0.1.0.0", "Board1", GetResource ("ExportFolder/v0.1/Boards/Board1"));
        ServerData.SaveNewBoard ("Path0.1.0.0", "Board2", GetResource ("ExportFolder/v0.1/Boards/Board2"));
        ServerData.SaveNewBoard ("Path0.1.0.0", "Board3", GetResource ("ExportFolder/v0.1/Boards/Board3"));
        ServerData.SaveNewBoard ("Path0.1.0.0", "Board4", GetResource ("ExportFolder/v0.1/Boards/Board4"));
        ServerData.SetCardPool (1, GetResource ("ExportFolder/v0.1/CardPools/CardPool"));
        ServerData.SaveRatingAbilityAbilitySynergy (GetResource ("ExportFolder/Rating/AbilityAbilitySynergy"));
        ServerData.SaveRatingAbilityAfterAbility (GetResource ("ExportFolder/Rating/AbilityAfterAbility"));
        ServerData.SaveRatingAbilityOnRow (GetResource ("ExportFolder/Rating/AbilityOnRow"));
        GameVersion = 0;
        PathVersion = 0;
        HotfixVersion = 0;
        DevelopVersion = 34;
    }

    static public string [] GetResource (string path) {
        string s = (Resources.Load (path) as TextAsset).text;
        return s.Split (new string [] { System.Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries);
    }

    static public string GetServerVersion () {
        return GameVersion + "." + PathVersion + "." + HotfixVersion + "." + DevelopVersion;
    }

    static public bool CompareServerVersion (string reference) {
        return reference == GetServerVersion ();
    }

}