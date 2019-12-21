using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGameClass {

    public const int FFA1 = 0;
    public const int FFA2 = 1;
    public const int FFA3 = 2;
    public const int FFA4 = 3;

    static int nextId;

    public int id;
    public string name;
    public int gameMode;
    public int matchType;

    public ClientInterface host;

    public ClientInterface [] clients;
    public bool [] AI;
    public int [] team;

    public int NumberOfPlayers () {
        int count = 0;

        for (int x = 0; x < clients.Length; x++) {
            if (clients [x] != null || AI [x]) {
                count++;
            }
        }

        return count;
    }

    static public int GetNumberOfSlots (int matchType) {
        switch (matchType) {
            case 0:
                return 1;
            case 1:
                return 2;
            case 2:
                return 3;
            case 3:
                return 4;
            default:
                return 0;
        }

    }

    static public string GetMatchTypeName (int matchType) {
        switch (matchType) {
            case 0:
                return "1FFA";
            case 1:
                return "2FFA";
            case 2:
                return "3FFA";
            case 3:
                return "4FFA";
            default:
                return "Unknown";
        }
    }

    public CustomGameClass (int gameMode, int matchType, string gameName) {
        id = nextId++;
        this.name = gameName;
        this.gameMode = gameMode;
        this.matchType = matchType;
        int count = GetNumberOfSlots (matchType);
        clients = new ClientInterface [count];
        AI = new bool [count];

        team = new int [count];
        for (int x = 0; x < count; x++) {
            team [x] = x + 1;
        }
    }

    public bool JoinGame (ClientInterface client) {
        for (int x = 0; x < clients.Length; x++) {
            if (clients [x] == null && !AI [x]) {
                JoinSlot (client, x);
                return true;
            }
        }
        return false;
    }

    public void JoinSlot (ClientInterface client, int slot) {
        for (int x = 0; x < clients.Length; x++) {
            if (clients [x] == client && x != slot) {
                clients [x] = null;
            }
        }
        clients [slot] = client;
        if (host == null) {
            host = client;
        }
        RefreshRoomForClients ();
    }

    public void ChangeSlot (int previousSlot, int newSlot) {
        if (clients [newSlot] == null && !AI [newSlot]) {
            clients [newSlot] = clients [previousSlot];
            clients [previousSlot] = null;
        }
        RefreshRoomForClients ();
    }

    public void ChangeTeam (int slot) {
        team [slot] = team [slot] % team.Length + 1;
    }

    public void AddAI (ClientInterface client, int slot) {
        if (!host == client) {
            return;
        }
        if (clients [slot] == null) {
            AI [slot] = true;
        }
        RefreshRoomForClients ();
    }

    public void KickPlayer (ClientInterface client, int slot) {
        if (!host == client) {
            return;
        }
        RemovePlayer (slot);
    }

    public void RemovePlayer (int slot) {
        if (host == clients [slot]) {
            host = null;
            bool replaced = false;
            for (int x = 0; x < clients.Length; x++) {
                if (clients [x] != null && x != slot) {
                    host = clients [x];
                    replaced = true;
                    break;
                }
            }
            if (!replaced) {
                clients [slot].TargetShowMessage (clients [slot].connectionToClient, Language.YouHaveBeenKickedFromTheRoomKey);
                CustomGameManager.RemoveCustomGame (this);
                return;
            }
        }
        if (clients [slot] != null) {
            clients [slot].TargetShowMessage (clients [slot].connectionToClient, Language.YouHaveBeenKickedFromTheRoomKey);
        }
        clients [slot] = null;
        AI [slot] = false;
        RefreshRoomForClients ();
    }

    public void RefreshRoomForClients () {
        ServerLogic.DownloadCustomGameRoom (this);
    }

    public void StartGame () {
        int count = clients.Length;
        PlayerPropertiesClass [] properties = new PlayerPropertiesClass [count];
        for (int x = 0; x < count; x++) {
            if (clients [x] != null) {
                properties [x] = new PlayerPropertiesClass (team [x], clients [x]);
            } else if (AI [x]) {
                HandClass hand2 = new HandClass ();
                AIClass AI = new AIClass ();
                hand2.GenerateRandomHand (gameMode, AI);
                properties [x] = new PlayerPropertiesClass (team [x], AI, "AI opponent", "AI opponent", hand2, null);
            } else {
                properties [x] = null;
            }

        }
        ServerLogic.StartMatch (MatchMakingClass.CreateGame (gameMode, matchType, properties));
    }

}
