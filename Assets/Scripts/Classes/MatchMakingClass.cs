﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchMakingClass {

    static public List<MatchClass> matches = new List<MatchClass> ();
    static public List<QueuePosition> quickQueue = new List<QueuePosition> ();

    static public MatchClass FindMatch (string accountName) {
        foreach (MatchClass match in matches) {
            foreach (PlayerClass player in match.Player) {
                if (player != null && player.properties != null && player.properties.accountName == accountName && !player.properties.conceded) {
                    return match;
                }
            }
        }
        return null;
    }

    static public void JoinQuickQueue (ClientInterface client) {
        QueuePosition sameGameMode = null;
        foreach (QueuePosition pos in quickQueue) {
            if (client.AccountName == pos.client.AccountName) {
                return;
            }
            if (client.GameMode == pos.client.GameMode) {
                sameGameMode = pos;
            }
        }
        if (sameGameMode != null) {
            quickQueue.Remove (sameGameMode);
            PlayerPropertiesClass [] properties = new PlayerPropertiesClass [2];
            int playerNumber = Random.Range (0, 2);
            properties [playerNumber++] = new PlayerPropertiesClass (1, client);
            properties [playerNumber % 2] = new PlayerPropertiesClass (2, sameGameMode.client);
            ServerLogic.StartMatch (CreateGame (client.GameMode, 1, properties));
        } else {
            quickQueue.Add (new QueuePosition (client));
            client.TargetShowQuickMatchQueue (client.connectionToClient);
        }
    }

    static public void LeaveQuickQueue (ClientInterface client) {
        foreach (QueuePosition pos in quickQueue) {
            if (client.AccountName == pos.client.AccountName) {
                quickQueue.Remove (pos);
                return;
            }
        }
    }


    static public MatchClass CreateGame (int gameMode, int matchType, PlayerPropertiesClass [] properties) {
        MatchClass match = new MatchClass ();
        match.NewMatch (gameMode, matchType, properties.Length);
        int [] statuses = ServerData.GetGameModeStatuses (gameMode);
        for (int x = 0; x < properties.Length; x++) {
            if (properties [x] != null) {
                match.SetPlayer (x + 1, new PlayerClass (properties [x]));
                PlayerClass player = match.Player [x + 1];
                if (player != null) {
                    PlayerPropertiesClass pProperties = player.properties;
                    if (pProperties != null) {
                        pProperties.specialStatus = statuses [x + 1];
                        ClientInterface client = pProperties.client;
                        if (client != null) {
                            ServerData.IncrementThisGameModeUnfinished (client.AccountName, gameMode);
                        }
                    }
                }
            }
        }
        matches.Add (match);
        //InGameUI.ShowInGameUI (match);
        return match;
    }
}
