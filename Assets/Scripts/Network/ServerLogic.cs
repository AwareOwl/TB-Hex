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

        AccountVersionManager.CheckAccountVersion (client);
    }

    static public MatchClass JoinGameAgainstAI (ClientInterface client) {
        HandClass hand1 = new HandClass ();
        //hand1.GenerateRandomHand ();
        string accountName = client.AccountName;
        int gameMode = client.GameMode;
        int selectedSet = ServerData.GetPlayerModeSelectedSet (accountName, gameMode);
        if (!ServerData.GetPlayerModeSelectedSetExists (accountName, gameMode)) {
            client.TargetShowMessage (client.connectionToClient, Language.NoSetSelectedKey);
            return null;
        }
        hand1.LoadFromFile (client.AccountName, client.GameMode, selectedSet);
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
        if (ServerVersionManager.CompareVersion (clientVersion)) {
            client.TargetShowLoginMenu (client.connectionToClient);
        } else {
            client.TargetInvalidVersionMessage (client.connectionToClient, VersionManager.GetVersion ());
        }
    }

    static public void SaveSelectedSet (ClientInterface client, int selectedSetId) {
        ServerData.SetPlayerModeSelectedSet (client.AccountName, client.GameMode, selectedSetId);
    }


    static public void DownloadCardPoolToEditor (ClientInterface client) {
        client.TargetDownloadCardPoolToEditor (client.connectionToClient, ServerData.GetCardPool (client.GameMode));
    }


    static public void DownloadSetList (ClientInterface client) {
        string accountName = client.AccountName;
        int gameMode = client.GameMode;
        string [] idsToParse = ServerData.GetAllPlayerModeSets (accountName, gameMode);
        List<int> ids = new List<int> ();
        foreach (string s in idsToParse) {
            ids.Add (int.Parse (s));
        }
        ids.Sort ();
        int count = ids.Count;
        int [] intIds = new int [count];
        string [] setNames = new string [count];
        int [] iconNumbers = new int [count];
        bool [] legal = new bool [count];
        for (int x = 0; x < count; x++) {
            intIds [x] = ids [x];
            setNames [x] = ServerData.GetPlayerModeSetName (accountName, gameMode, intIds [x]);
            iconNumbers [x] = ServerData.GetPlayerModeSetIconNumber (accountName, gameMode, intIds [x]);
            HandClass hand = new HandClass ();
            hand.LoadFromFile (accountName, gameMode, intIds [x]);
            legal [x] = hand.IsValid ();

        }
        int selectedSet = ServerData.GetPlayerModeSelectedSet (accountName, gameMode);
        client.TargetDownloadSetList (client.connectionToClient, setNames, intIds, iconNumbers, legal, selectedSet);
    }

    static public void DownloadGameModeLists (ClientInterface client) {
        string accountName = client.AccountName;
        string [] list = ServerData.GetAllGameModes ();
        List<int> officialIds = new List<int> ();
        List<int> publicIds = new List<int> ();
        List<int> yourIds = new List<int> ();
        List<string> officialNames = new List<string> ();
        List<string> publicNames = new List<string> ();
        List<string> yourNames = new List<string> ();



        client.TargetDownloadGameModeLists (client.connectionToClient, null, null, null, null, null, null);
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

    static public void SavePlayerModeSet (ClientInterface client, string [] lines, int setId) {
        ServerData.SavePlayerModeSet (client.AccountName, client.GameMode, setId, lines);
        HandClass hand = new HandClass ();
        hand.LoadFromString (lines);
        if (hand.IsValid ()) {
            client.TargetShowMessage (client.connectionToClient, Language.SetSavedKey);
        } else {
            client.TargetInvalidSavedSet (client.connectionToClient);
        }
        //client.TargetDownloadCardPoolToEditor (client.connectionToClient, ServerData.GetCardPool (1));
    }

    static public void SaveSetProperties (ClientInterface client, int setId, string setName, int iconNumber) {
        string accountName = client.AccountName;
        int gameMode = client.GameMode;
        ServerData.SetPlayerModeSetName (accountName, gameMode, setId, setName);
        ServerData.SetPlayerModeSetIconNumber (accountName, gameMode, setId, iconNumber);
    }


    static public void DownloadSetToEditor (ClientInterface client, int setId) {
        string accountName = client.AccountName;
        int gameMode = client.GameMode;
        string [] lines = ServerData.GetPlayerModeSet (accountName, gameMode, setId);
        string name = ServerData.GetPlayerModeSetName (accountName, gameMode, setId);
        int iconNumber = ServerData.GetPlayerModeSetIconNumber (accountName, gameMode, setId);
        client.TargetDownloadSetToEditor (client.connectionToClient, lines, name, iconNumber);
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
