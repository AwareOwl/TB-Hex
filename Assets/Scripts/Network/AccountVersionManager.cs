using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountVersionManager : VersionManager {

    static ClientInterface client;
    static string accountName;

    static public void CheckAccountVersion (ClientInterface client) {
        AccountVersionManager.client = client;
        accountName = client.AccountName;
        string version = ServerData.GetUserKeyData (accountName, ServerData.VersionKey);


        ConvertStringToVersion (version);

        if (GameVersion <= 0) {
            if (PathVersion <= 1) {
                if (HotfixVersion <= 1) {
                    if (DevelopVersion < 6) {
                        Debug.Log ("Wut");
                        ConvertTo0_1_1_6 ();
                    }
                }
            }
        }

        ServerData.SetUserKeyData (accountName, ServerData.VersionKey, GetVersion ());
    }
    
    static public void ConvertTo0_1_1_6 () {
        HandClass hand = new HandClass ();
        hand.LoadFromFile (accountName, 1, 1);
        ServerData.CreatePlayerModeSet (accountName, 2, hand.HandToString (), "");

        client.TargetGetStartingSetName (client.connectionToClient);

        PathVersion = 1;
        HotfixVersion = 1;
        DevelopVersion = 6;
    }

    static public void SetStartingSetNameToAllSets (string accountName, string startingSetName) {
        string [] s3 = ServerData.GetAllPlayerModes (accountName);
        foreach (string s4 in s3) {
            int i4 = int.Parse (s4);
            int [] s5 = ServerData.GetAllPlayerModeSets (accountName, i4);
            foreach (int i6 in s5) {
                ServerData.SetPlayerModeSetIconNumber (accountName, i4, i6, 1);
                ServerData.SetPlayerModeSetName (accountName, i4, i6, Language.StartingSet);
            }
        }
    }

}
