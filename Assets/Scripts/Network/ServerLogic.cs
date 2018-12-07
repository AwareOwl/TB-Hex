using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerLogic : MonoBehaviour {
    
    static public void LogIn (ClientInterface client, string userName, string password) {
        if (userName != null && userName != "") {
            if (ServerData.UserExists (userName)) {
                if (VerifyUserPassword (userName, password)) {
                    //JoinGameAgainstAI (client);
                    CompleteLogIn (client, userName);

                } else {
                    client.TargetShowMessage (client.connectionToClient, Language.PasswordIsIncorrectKey);
                }
            } else {
                client.TargetShowMessage (client.connectionToClient, Language.AccountDoesntExistKey);
            }
        } else {
            client.TargetShowMessage (client.connectionToClient, Language.PleaseEnterAccountNameKey);
        }
    }

    static public void Register (ClientInterface client, string accountName, string userName, string password, string email) {
        if (accountName != null && accountName != "") {
            if (!ServerData.UserExists (accountName)) {
                if (password.Length >= 8) {
                    ServerData.CreateAccount (accountName, userName, password, email);
                    client.TargetShowMessage (client.connectionToClient, Language.AccountCreatedKey);
                } else {
                    client.TargetShowMessage (client.connectionToClient, Language.PasswordHasToHaveAtLeast8CharactersKey);
                }
            } else {
                client.TargetShowMessage (client.connectionToClient, Language.AccountWithThisNameAlreadyExistsKey);
            }
        } else {
            client.TargetShowMessage (client.connectionToClient, Language.AccountNameCantBeNullKey);
        }
    }

    static public bool VerifyUserPassword (string accountName, string password) {
        password = ServerData.EncryptString (password);
        string userPassword = ServerData.UserPassword (accountName);
        if (password.CompareTo (userPassword) == 0) {
            return true;
        }
        return false;
    }

    static public void CompleteLogIn (ClientInterface client, string accountName) {
        client.AccountName = accountName;
        string userName = ServerData.GetUserKeyData (accountName, ServerData.UserNameKey);
        if (userName == "null" || userName == "") {
            userName = accountName;
        }
        client.UserName = userName;
        client.GameMode = ServerData.GetUserSelectedGameMode (accountName);
        client.TargetLogIn (client.connectionToClient, accountName, userName);
    }

    static public MatchClass JoinGameAgainstAI (ClientInterface client) {
        HandClass hand1 = new HandClass ();
        //hand1.GenerateRandomHand ();
        hand1.LoadFromFile (client.AccountName, client.GameMode, 1);
        if (!hand1.IsValid ()) {
            client.TargetInvalidSet (client.connectionToClient);
            return null;
        }
        HandClass hand2 = new HandClass ();
        hand2.GenerateRandomHand ();
        MatchClass match = MatchMakingClass.CreateGame (new PlayerPropertiesClass [] {
            new PlayerPropertiesClass (1, InputController.autoRunAI, client.AccountName, client.UserName, hand1, client),
            new PlayerPropertiesClass (2, true, "AI opponent", "AI opponent", hand2, null) });
        client.currentMatch = match;
        if (!InputController.autoRunAI) {
            DownloadGame (client, match);
        }
        return match;
    }

    static public void DownloadGame (ClientInterface client, MatchClass match) {
        client.TargetDownloadCurrentGameMatch (client.connectionToClient, match.MatchToString ());
        client.TargetDownloadCurrentGameMatchProperties (client.connectionToClient, 
            match.Properties.MatchPropertiesToString ());
        client.TargetDownloadCurrentGameBoard (client.connectionToClient,
            match.Board.BoardToString ());
        for (int x = 0; x <= match.numberOfPlayers; x++) {
            PlayerClass player = match.Player [x];
            client.TargetDownloadCurrentGamePlayer (client.connectionToClient, 
                x, player.PlayerToString ());
            PlayerPropertiesClass playerProperties = player.properties;
            if (playerProperties != null) {
                client.TargetDownloadCurrentGamePlayerProperties (client.connectionToClient,
                    x, playerProperties.PlayerPropertiesToString ());
                HandClass hand = playerProperties.hand;
                client.TargetDownloadCurrentGameHand (client.connectionToClient,
                    x, hand.HandToString ());
            }
        }
        client.TargetFinishDownloadCurrentGame (client.connectionToClient);
    }
    /*
    static public void DelayedShowMatchResult (ClientInterface client, string winnerName, int winCondition, int limit) {
        VisualMatch.instance.ShowMatchResult (client, winnerName, winCondition, limit);
    }

    static public void ShowMatchResult (ClientInterface client, string winnerName, int winCondition, int limit) {
        client.TargetShowMatchResult (client.connectionToClient, winnerName, winCondition, limit);
    }*/

    static public void CompareServerVersion (ClientInterface client, string clientVersion) {
        if (VersionManager.CompareServerVersion (clientVersion)) {
            client.TargetShowLoginMenu (client.connectionToClient);
        } else {
            client.TargetInvalidVersionMessage (client.connectionToClient, VersionManager.GetServerVersion ());
        }
    }


    static public void DownloadCardPoolToEditor (ClientInterface client) {
        client.TargetDownloadCardPoolToEditor (client.connectionToClient, ServerData.GetCardPool (client.GameMode));
    }


    static public void DownloadSetList (ClientInterface client) {
        string [] ids = ServerData.GetAllPlayerModeSets (client.AccountName, client.GameMode);
        int count = ids.Length;
        int [] intIds = new int [count];
        string [] setNames = new string [count];
        int [] iconNumbers = new int [count];
        for (int x = 0; x < count; x++) {
            intIds [x] = int.Parse (ids [x]);
            setNames [x] = ServerData.GetPlayerModeSetName (client.AccountName, client.GameMode, intIds [x]);
            iconNumbers [x] = ServerData.GetPlayerModeSetIconNumber (client.AccountName, client.GameMode, intIds [x]);

        }
        client.TargetDownloadSetList (client.connectionToClient, setNames, intIds, iconNumbers);
    }


    static public void CreateNewSet (ClientInterface client, string name) {
        HandClass hand = new HandClass ();
        ServerData.CreatePlayerModeSet (client.AccountName, client.GameMode, hand.HandToString(), name);
        DownloadSetList (client);
    }


    static public void DeleteSet (ClientInterface client, int id) {
        ServerData.DeletePlayerModeSet (client.AccountName, client.GameMode, id);
        DownloadSetList (client);
    }

    static public void SavePlayerModeSet (ClientInterface client, string [] lines) {
        ServerData.SavePlayerModeSet (client.AccountName, client.GameMode, 1, lines);
        HandClass hand = new HandClass ();
        hand.LoadFromString (lines);
        if (hand.IsValid ()) {
            client.TargetShowMessage (client.connectionToClient, Language.SetSavedKey);
        } else {
            client.TargetInvalidSavedSet (client.connectionToClient);
        }
        //client.TargetDownloadCardPoolToEditor (client.connectionToClient, ServerData.GetCardPool (1));
    }


    static public void DownloadSetToEditor (ClientInterface client) {
        client.TargetDownloadSetToEditor (client.connectionToClient, 
            ServerData.GetPlayerModeSet (client.AccountName, client.GameMode, 1));
    }


    static public void CurrentGameMakeAMove (ClientInterface client, int x, int y, int playerNumber, int stackNumber) {
        client.currentMatch.PlayCard (x, y, playerNumber, stackNumber);
        //VisualMatch.instance.ShowMatchResult (client, winnerName, winCondition, limit);
    }


    static public void TargetCurrentGameMakeAMove (ClientInterface client, int x, int y, int playerNumber, int stackNumber) {
        client.TargetCurrentGameMakeAMove (client.connectionToClient, x, y, playerNumber, stackNumber);
        //VisualMatch.instance.ShowMatchResult (client, winnerName, winCondition, limit);
    }
}
