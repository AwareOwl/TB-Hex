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
        }
    }

    static public void KickPlayer (ClientInterface client, int slotNumber) {
        foreach (CustomGameClass customGame in customGames) {
            if (customGame.host == client) {
                customGame.KickPlayer (client, slotNumber);
            }
        }
    }

    static public void StartCustomGame (ClientInterface client) {
        CustomGameClass game = null;
        foreach (CustomGameClass customGame in customGames) {
            if (customGame.host == client) {
                game.StartGame ();
            }
        }
        customGames.Remove (game);
    }
}
