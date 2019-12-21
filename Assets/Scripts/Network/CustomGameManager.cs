using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGameManager {
    static public List<CustomGameClass> customGames = new List<CustomGameClass> ();

    static public CustomGameClass [] GetCustomGames (int gameMode) {
        List<CustomGameClass> games = new List<CustomGameClass> ();
        foreach (CustomGameClass customGame in customGames) {
            if (customGame.gameMode == gameMode) {
                games.Add (customGame);
            }
        }
        return games.ToArray ();
    }

    static public void CreateCustomGame (ClientInterface client, int matchType, string gameName) {
        CustomGameClass newCustomGame = new CustomGameClass (client.GameMode, matchType, gameName);
        newCustomGame.JoinGame (client);
        customGames.Add (newCustomGame);
    }

    static public void AddAI (ClientInterface client, int slotNumber) {
        foreach (CustomGameClass customGame in customGames) {
            if (customGame.host == client) {
                customGame.AddAI (client, slotNumber);
            }
            return;
        }
    }

    static public void KickPlayer (ClientInterface client, int slotNumber) {
        foreach (CustomGameClass customGame in customGames) {
            if (customGame.host == client) {
                customGame.KickPlayer (client, slotNumber);
                return;
            }
        }
    }

    static public bool JoinGame (ClientInterface client, int id) {
        foreach (CustomGameClass customGame in customGames) {
            if (customGame.id == id) {
                customGame.JoinGame (client);
                return true;
            }
        }
        return false;
    }

    static public void LeaveCustomGame (ClientInterface client) {
        foreach (CustomGameClass customGame in customGames) {
            int count = customGame.clients.Length;
            for (int x = 0; x < count; x++) {
                if (customGame.clients [x] == client) {
                    customGame.RemovePlayer (x);
                    return;
                }
            }
        }
    }

    static public void StartCustomGame (ClientInterface client) {
        CustomGameClass game = null;
        foreach (CustomGameClass customGame in customGames) {
            if (customGame.host == client) {
                game = customGame;
                customGame.StartGame ();
            }
        }
        if (game != null) {
            RemoveCustomGame (game);
        }
    }

    static public void RemoveCustomGame (CustomGameClass game) {
        customGames.Remove (game);
    }

    static public void ChangeSlot (ClientInterface client, int newSlot) {
        foreach (CustomGameClass customGame in customGames) {
            int count = customGame.clients.Length;
            for (int x = 0; x < count; x++) {
                if (customGame.clients [x] == client) {
                    customGame.ChangeSlot (x, newSlot);
                    return;
                }
            }
        }
    }

    static public void ChangeTeam (ClientInterface client, int slot) {
        foreach (CustomGameClass customGame in customGames) {
            if (customGame.host == client) {
                customGame.ChangeTeam (slot);
                customGame.RefreshRoomForClients ();
            }
        }
    }
}
