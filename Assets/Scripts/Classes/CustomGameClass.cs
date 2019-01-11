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

    public ClientInterface [] clients;

    public CustomGameClass (int gameMode, int matchType, string gameName) {
        id = nextId++;
        this.name = gameName;
        this.gameMode = gameMode;
        switch (matchType) {
            case FFA1:
                clients = new ClientInterface [1];
                break;
            case FFA2:
                clients = new ClientInterface [2];
                break;
            case FFA3:
                clients = new ClientInterface [3];
                break;
            case FFA4:
                clients = new ClientInterface [4];
                break;
        }
    }

    public void JoinGame (ClientInterface client) {
        for (int x = 0; x < clients.Length; x++) {
            if (clients [x] == null) {
                JoinSlot (client, x);
                return;
            }
        }
    }

    public void JoinSlot (ClientInterface client, int slot) {
        for (int x = 0; x < clients.Length; x++) {
            if (clients [x] == client && x != slot) {
                clients [x] = null;
            }
        }
        clients [slot] = client;
    }

    public void StartGame () {
        int count = clients.Length;
        PlayerPropertiesClass [] properties = new PlayerPropertiesClass [count];
        for (int x = 0; x < count; x++) {
            if (clients [x] != null) {
                properties [x] = new PlayerPropertiesClass (x + 1, clients [x]);
            }
        }
        ServerLogic.StartMatch (MatchMakingClass.CreateGame (gameMode, properties));
    }

}
