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
        if (userName == null || userName == "") {
            userName = accountName;
        }
        client.UserName = userName;
        client.GameMode = ServerData.GetUserSelectedGameMode (accountName);
        client.TargetLogIn (client.connectionToClient, accountName, userName);

        AccountVersionManager.CheckAccountVersion (client);
    }

    static public void ChangeGameMode (ClientInterface client, int gameMode) {
        if (!ServerData.GetIsGameModeLegal (gameMode)) {
            client.TargetShowMessage (client.connectionToClient, Language.GameVersionDoesNotMeetRequirementsKey);
            return;
        }
        client.GameMode = gameMode;
        ServerData.SetUserSelectedGameMode (client.AccountName, gameMode);
    }

    static public MatchClass JoinGameAgainstAI (ClientInterface client) {
        HandClass hand1 = new HandClass ();
        string accountName = client.AccountName;
        int gameMode = client.GameMode;
        if (!ServerData.GetIsGameModeLegal (gameMode)) {
            client.TargetShowMessage (client.connectionToClient, Language.GameVersionDoesNotMeetRequirementsKey);
            return null;
        }
        if (!InputController.autoRunAI) {
            int selectedSet = ServerData.GetPlayerModeSelectedSet (accountName, gameMode);
            if (!ServerData.GetPlayerModeSelectedSetExists (accountName, gameMode)) {
                client.TargetShowMessage (client.connectionToClient, Language.NoSetSelectedKey);
                return null;
            }
            hand1.LoadFromFile (client.AccountName, client.GameMode, selectedSet);
            if (!hand1.IsValid (gameMode)) {
                int minimumNumberOfCardsInStack = ServerData.GetGameModeMinimumNumberOfCardsInStack (gameMode);
                client.TargetInvalidSet (client.connectionToClient, minimumNumberOfCardsInStack);
                return null;
            }
        } else {
            hand1.GenerateRandomHand (gameMode);
        }
        HandClass hand2 = new HandClass ();
        hand2.GenerateRandomHand (gameMode);
        MatchClass match = MatchMakingClass.CreateGame (gameMode, 1, new PlayerPropertiesClass [] {
            new PlayerPropertiesClass (1, InputController.autoRunAI, client.AccountName, client.UserName, hand1, client),
            new PlayerPropertiesClass (2, true, "AI opponent", "AI opponent", hand2, null) });
        StartMatch (match);
        return match;
    }

    static public void JoinQuickMatchQueue (ClientInterface client) {
        HandClass hand1 = new HandClass ();
        string accountName = client.AccountName;
        int gameMode = client.GameMode;
        int selectedSet = ServerData.GetPlayerModeSelectedSet (accountName, gameMode);
        if (!ServerData.GetPlayerModeSelectedSetExists (accountName, gameMode)) {
            client.TargetShowMessage (client.connectionToClient, Language.NoSetSelectedKey);
            return;
        }
        hand1.LoadFromFile (client.AccountName, client.GameMode, selectedSet);
        if (!hand1.IsValid (gameMode)) {
            int minimumNumberOfCardsInStack = ServerData.GetGameModeMinimumNumberOfCardsInStack (gameMode);
            client.TargetInvalidSet (client.connectionToClient, minimumNumberOfCardsInStack);
            return;
        }
        MatchMakingClass.JoinQuickQueue (client);
    }

    static public void JoinCustomGameRoom (ClientInterface client, int id) {
        if (!CustomGameManager.JoinGame (client, id)) {
            client.TargetShowMessage (client.connectionToClient, Language.FailedToConnectToTheGameKey);
        }
    }

    static public void LeaveQuickMatchQueue (ClientInterface client) {
        MatchMakingClass.LeaveQuickQueue (client);
        client.TargetShowMainMenu (client.connectionToClient);
    }

    static public void StartMatch (MatchClass match) {
        for (int x = 1; x <= match.numberOfPlayers; x++) {
            PlayerClass player = match.Player [x];
            if (player != null) {
                match.turnOfPlayer = x;
                break;
            }
        }
        for (int x = 1; x <= match.numberOfPlayers; x++) {
            PlayerClass player = match.Player [x];
            if (player == null) {
                continue;
            }
            ClientInterface client = player.properties.client;
            if (client != null) {
                client.currentMatch = match;
                if (!InputController.autoRunAI) {
                    DownloadGame (client, match);
                }
            }
        }
        for (int x = 1; x <= match.numberOfPlayers; x++) {
            PlayerClass player = match.Player [x];
            if (player != null) {
                if (player.properties.AI != null) {
                    match.RunAI ();
                }
                break;
            }
        }
    }

    static public void DownloadGame (ClientInterface client, MatchClass match) {
        client.TargetDownloadCurrentGameMatch (client.connectionToClient, match.MatchToString ());
        client.TargetDownloadCurrentGameMatchProperties (client.connectionToClient, 
            match.Properties.MatchPropertiesToString ());
        client.TargetDownloadCurrentGameBoard (client.connectionToClient,
            match.Board.BoardToString ());
        for (int x = 0; x <= match.numberOfPlayers; x++) {
            PlayerClass player = match.Player [x];
            if (player == null) {
                continue;
            }
            client.TargetDownloadCurrentGamePlayer (client.connectionToClient, 
                x, player.PlayerToString ());
            PlayerPropertiesClass playerProperties = player.properties;
            if (playerProperties != null) {
                client.TargetDownloadCurrentGamePlayerProperties (client.connectionToClient,
                    x, playerProperties.PlayerPropertiesToString ());
                if (client.playerNumber == playerProperties.playerNumber) {
                    HandClass hand = playerProperties.hand;
                    client.TargetDownloadCurrentGameHand (client.connectionToClient,
                        x, hand.ModeHandToString ());
                }
            }
        }
        client.TargetFinishDownloadCurrentGame (client.connectionToClient, client.playerNumber);
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


    static public void DownloadDataToSetEditor (ClientInterface client, int setId) {
        string accountName = client.AccountName;
        int gameMode = client.GameMode;
        string [] cardPool = ServerData.GetCardPool (gameMode);
        string [] set = ServerData.GetPlayerModeSet (accountName, gameMode, setId);
        string name = ServerData.GetPlayerModeSetName (accountName, gameMode, setId);
        int iconNumber = ServerData.GetPlayerModeSetIconNumber (accountName, gameMode, setId);
        int numberOfStacks = ServerData.GetGameModeNumberOfStacks (gameMode);
        int minimumNumberOfCardsOnStack = ServerData.GetGameModeNumberOfStacks (gameMode);
        client.TargetDownloadDataToSetEditor (client.connectionToClient, cardPool, set, name, iconNumber, numberOfStacks, minimumNumberOfCardsOnStack);
    }


    static public void DownloadCardPoolToCardPoolEditor (ClientInterface client, int gameModeId) {
        client.TargetDownloadCardPoolToEditor (client.connectionToClient, gameModeId, ServerData.GetCardPool (gameModeId));
    }

    static public void DownloadListOfCustomGames (ClientInterface client) {
        List<int> ids = new List<int> ();
        List<string> names = new List<string> ();
        List<int> matchTypes = new List<int> ();
        List<int> filledSlots = new List<int> ();
        CustomGameClass [] list = CustomGameManager.GetCustomGames (client.GameMode);
        foreach (CustomGameClass customGame in list) {
            ids.Add (customGame.id);
            names.Add (customGame.name);
            matchTypes.Add (customGame.matchType);
            filledSlots.Add (customGame.NumberOfPlayers ());
        }

        client.TargetDownloadListOfCustomGames (client.connectionToClient, 
            ids.ToArray(), names.ToArray (), matchTypes.ToArray(), filledSlots.ToArray());
    }

    static public void DownloadCustomGameRoom (CustomGameClass customGame) {
        List<int> avatars = new List<int> ();
        List<string> userNames = new List<string> ();
        List<bool> AI = new List<bool> ();

        int count = CustomGameClass.GetNumberOfSlots (customGame.matchType);
        for (int x = 0; x < count; x++) {
            bool tAI = customGame.AI [x];
            ClientInterface tClient = customGame.clients [x];
            if (tClient != null) {
                avatars.Add (ServerData.GetUserAvatar (tClient.AccountName));
            } else {
                avatars.Add (2);
            }
            if (tClient != null) {
                userNames.Add (tClient.UserName);
            } else if (tAI) {
                userNames.Add ("AI Opponent");
            } else {
                userNames.Add ("");
            }
            AI.Add (tAI);
        }
        foreach (ClientInterface client in customGame.clients) {
            if (client != null) {
                bool isHost = client == customGame.host;
                client.TargetDownloadCustomGameRoom (client.connectionToClient,
                    isHost, customGame.name, customGame.matchType, 
                    avatars.ToArray (), userNames.ToArray (), AI.ToArray ());
            }
        }
    }


    static public void DownloadSetList (ClientInterface client) {
        string accountName = client.AccountName;
        int gameMode = client.GameMode;
        List<int> ids = new List<int> (ServerData.GetAllPlayerModeSets (accountName, gameMode));
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
            legal [x] = hand.IsValid (gameMode);

        }
        int selectedSet = ServerData.GetPlayerModeSelectedSet (accountName, gameMode);
        client.TargetDownloadSetList (client.connectionToClient, setNames, intIds, iconNumbers, legal, selectedSet);
    }

    static public void DownloadGameModeLists (ClientInterface client) {
        string accountName = client.AccountName;
        int [] list = ServerData.GetAllGameModes ();
        List<int> officialIds = new List<int> ();
        List<int> publicIds = new List<int> ();
        List<int> yourIds = new List<int> ();
        List<string> officialNames = new List<string> ();
        List<string> publicNames = new List<string> ();
        List<string> yourNames = new List<string> ();
        List<bool> yourIsLegal = new List<bool> ();

        foreach (int id in list) {
            string [] owners = ServerData.GetGameModeOwners (id);
            if (ServerData.GetGameModeIsOfficial (id)) {
                officialIds.Add (id);
            } else {
                if (ServerData.GetIsGameModeLegal (id) && !ServerData.GetGameModeDeleted (id)) {
                    publicIds.Add (id);
                }
            }
            foreach (string owner in owners) {
                if (client.AccountName == owner && !ServerData.GetGameModeDeleted (id)) {
                    yourIds.Add (id);
                    if (ServerData.GetIsGameModeLegal (id)) {
                        yourIsLegal.Add (true);
                    } else {
                        yourIsLegal.Add (false);
                    }
                }
            }
        }
        officialIds.Sort ((a, b) => (b.CompareTo (a)));
        publicIds.Sort ((a, b) => (b.CompareTo (a)));
        yourIds.Sort ((a, b) => (a.CompareTo (b)));

        foreach (int id in officialIds) {
            officialNames.Add (ServerData.GetGameModeName (id));
        }
        foreach (int id in publicIds) {
            publicNames.Add (ServerData.GetGameModeName (id));
        }
        foreach (int id in yourIds) {
            yourNames.Add (ServerData.GetGameModeName (id));
        }

        client.TargetDownloadGameModeLists (client.connectionToClient, client.GameMode,
            officialNames.ToArray (), publicNames.ToArray (), yourNames.ToArray(), officialIds.ToArray (), publicIds.ToArray (), yourIds.ToArray(), yourIsLegal.ToArray());
    }

    static public void DownloadGameModeSettings (ClientInterface client, int id) {
        bool hasScoreWinCondition = ServerData.GetGameModeHasScoreWinCondition (id);
        int scoreWinConditionValue = ServerData.GetGameModeScoreWinConditionValue (id);
        bool hasTurnWinCondition = ServerData.GetGameModeHasTurnWinCondition (id);
        int turnWinConditionValue = ServerData.GetGameModeTurnWinConditionValue (id);
        bool isAllowedToRotateCardsDuringMatch = ServerData.GetGameModeIsAllowedToRotateCardsDuringMatch (id);
        int numberOfStacks = ServerData.GetGameModeNumberOfStacks (id);
        int minimumNumberOfCardsInStack = ServerData.GetGameModeMinimumNumberOfCardsInStack (id);

        client.TargetDownloadGameModeSettingsToEditor (client.connectionToClient, 
            hasScoreWinCondition, scoreWinConditionValue, hasTurnWinCondition, turnWinConditionValue,
            isAllowedToRotateCardsDuringMatch, numberOfStacks, minimumNumberOfCardsInStack);
    }

    static public void SaveGameModeSettings (ClientInterface client, int id,
        bool hasScoreWinCondition, int scoreWinConditionValue, bool hasTurnWinCondition, int turnWinConditionValue,
        bool isAllowedToRotateCardsDuringMatch, int numberOfStacks, int minimumNumberOfCardsInStack) {
        if (!ServerData.IsGameModeOwner (id, client.AccountName)) {
            return;
        }
        ServerData.SetGameModeHasScoreWinCondition (id, hasScoreWinCondition);
        ServerData.SetGameModeScoreWinConditionValue (id, scoreWinConditionValue);
        ServerData.SetGameModeHasTurnWinCondition (id, hasTurnWinCondition);
        ServerData.SetGameModeTurnWinConditionValue (id, turnWinConditionValue);
        ServerData.SetGameModeIsAllowedToRotateCardsDuringMatch (id, isAllowedToRotateCardsDuringMatch);
        ServerData.SetGameModeNumberOfStacks (id, numberOfStacks);
        ServerData.SetGameModeMinimumNumberOfCardsInStack (id, minimumNumberOfCardsInStack);
    }

    static public void DownloadProfileData (ClientInterface client) {
        string accountName = client.AccountName;
        int gameModeId = client.GameMode;
        string displayName = client.UserName;
        int avatarNumber = ServerData.GetUserAvatar (accountName);
        int thisGameModeWon = ServerData.GetThisGameModeWon (accountName, gameModeId);
        int thisGameModeLost = ServerData.GetThisGameModeLost (accountName, gameModeId);
        int thisGameModeDrawn = ServerData.GetThisGameModeDrawn (accountName, gameModeId);
        int thisGameModeUnfinished = ServerData.GetThisGameModeUnfinished (accountName, gameModeId);
        int totalWon = ServerData.GetTotalWon (accountName);
        int totalLost = ServerData.GetTotalLost (accountName);
        int totalDrawn = ServerData.GetTotalDrawn (accountName);
        int totalUnfinished = ServerData.GetTotalUnfinished (accountName);
        client.TargetDownloadProfileDataToMenu (client.connectionToClient,
            displayName, avatarNumber, thisGameModeWon, thisGameModeLost, thisGameModeDrawn, thisGameModeUnfinished, totalWon, totalLost, totalDrawn, totalUnfinished);

    }

    static public void SaveProfileSettings (ClientInterface client, string userName, int avatar) {
        string accountName = client.AccountName;
        if (userName != null && userName != "") {
            client.UserName = userName;
            ServerData.SetUserName (accountName, userName);
        }
        ServerData.SetUserAvatar (accountName, avatar);
        client.TargetRefreshProfileSettings (client.connectionToClient, userName, avatar);
    }

    static public void CreateNewGameMode (ClientInterface client, string name) {
        int id = ServerData.GetGameModeNextId ();
        ServerData.CreateNewGameMode (client.AccountName);
        ServerData.SetGameModeName (id, name);
        DownloadGameModeLists (client);
    }

    static public void CreateNewBoard (ClientInterface client, int gameModeId, string name) {
        BoardClass board = new BoardClass ();
        board.CreateNewBoard ();
        ServerData.SaveNewBoard (gameModeId, client.AccountName, name, board.BoardToString ());
        DownloadGameModeToEditor (client, gameModeId);
    }

    static public void DeleteGameMode (ClientInterface client, int gameModeId) {
        ServerData.SetGameModeDeleted (gameModeId, true);
        DownloadGameModeLists (client);
    }

    static public void DeleteBoard (ClientInterface client, int gameModeId, int boardId) {
        DeleteBoard (client, boardId);
        DownloadGameModeToEditor (client, gameModeId);
    }

    static public void DeleteBoard (ClientInterface client, int boardId) {
        if (!ServerData.IsBoardOwner (boardId, client.AccountName)) {
            return;
        }
        ServerData.SetBoardDeleted (boardId, true);
        int [] ids = ServerData.GetAllBoardGameModes (boardId);
        foreach (int id in ids) {
            ServerData.RemoveGameModeBoard (id, boardId);
            ServerData.CheckIfGameModeIsLegal (id);
        }
    }

    static public void DownloadGameModeToEditor (ClientInterface client, int gameModeId) {
        string [] owners = ServerData.GetGameModeOwners (gameModeId);
        string accountName = client.AccountName;
        bool isClientOwner = false;
        foreach (string s in owners) {
            if (accountName == s) {
                isClientOwner = true;
                break;
            }
        }
        if (!isClientOwner) {
            return;
        }
        int [] ids = ServerData.GetAllGameModeBoards (gameModeId);
        int count = ids.Length;
        string [] names = new string [count];
        bool [] isLegal = new bool [count];
        for (int x = 0; x < count; x++) {
            names [x] = ServerData.GetBoardName (ids [x]);
            isLegal [x] = ServerData.GetIsBoardLegal (ids [x]);
        }
        string gameModeName = ServerData.GetGameModeName (gameModeId);
        client.TargetDownloadGameModeToEditor (client.connectionToClient, gameModeId, gameModeName, names, ids, isLegal);
    }

    static public void DownloadBoardToEditor (ClientInterface client, int boardId) {
        string boardName = ServerData.GetBoardName (boardId);
        string [] board = ServerData.GetBoard (boardId);
        int [] matchTypes = ServerData.GetBoardMatchTypes (boardId);
        client.TargetDownloadBoardToEditor (client.connectionToClient, boardId, boardName, board, matchTypes);
    }

    static public void SaveBoard (ClientInterface client, int boardId, string [] board) {
        if (!ServerData.IsBoardOwner (boardId, client.AccountName)) {
            return;
        }
        ServerData.SetBoard (boardId, board);
        if (ServerData.GetIsBoardLegal (boardId)) {
            client.TargetShowMessage (client.connectionToClient, Language.BoardHasBeenSavedKey);
        } else {
            client.TargetShowMessage (client.connectionToClient, Language.BoardHasBeenSavedButIllegalKey);
        }
    }

    static public void SaveBoardMatchTypes (ClientInterface client, int boardId, int [] matchTypes) {
        if (!ServerData.IsBoardOwner (boardId, client.AccountName)) {
            return;
        }
        ServerData.SetBoardMatchTypes (boardId, matchTypes);
    }

    static public void CreateCustomGame (ClientInterface client, string gameName, int matchType) {
        int [] availableMatchTypes = ServerData.GetAllGameModeMatchTypes (client.GameMode);
        foreach (int type in availableMatchTypes) {
            if (type == matchType) {
                CustomGameManager.CreateCustomGame (client, matchType, gameName);
                return;
            }
        }
        client.TargetShowMessage (client.connectionToClient, Language.SelectedMatchVersionIsNotAvailableKey);
    }

    static public void SaveCardPool (ClientInterface client, int gameModeId, string [] cardPool) {
        if (!ServerData.IsGameModeOwner (gameModeId, client.AccountName)) {
            return;
        }
        ServerData.SetCardPool (gameModeId, cardPool);
        if (ServerData.GetIsCardPoolLegal (gameModeId)) {
            client.TargetShowMessage (client.connectionToClient, Language.CardPoolSavedKey);
        } else {
            client.TargetShowMessage (client.connectionToClient, Language.CardPoolSavedButIllegalKey);
        }
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
        int gameMode = client.GameMode;
        hand.LoadFromString (gameMode, lines);
        if (hand.IsValid (gameMode)) {
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

    static public void SaveGameModeProperties (ClientInterface client, int gameModeId, string gameModeName, int iconNumber) {
        string accountName = client.AccountName;
        if (!ServerData.IsGameModeOwner (gameModeId, accountName)){
            return;
        }
        ServerData.SetGameModeName (gameModeId, gameModeName);
    }

    static public void SaveBoardProperties (ClientInterface client, int boardId, string boardName, int iconNumber) {
        string accountName = client.AccountName;
        if (!ServerData.IsBoardOwner (boardId, accountName)){
            return;
        }
        ServerData.SetBoardName (boardId, boardName);
    }


    static public void CurrentGameMakeAMove (ClientInterface client, int x, int y, int playerNumber, int stackNumber) {
        if (InputController.debuggingEnabled) {
            Debug.Log ("Play card command verified on server");
        }
        client.currentMatch.PlayCard (x, y, playerNumber, stackNumber);
        //VisualMatch.instance.ShowMatchResult (client, winnerName, winCondition, limit);
    }


    static public void TargetCurrentGameMakeAMove (ClientInterface client, int moveId, int x, int y, int playerNumber, int stackNumber, int abilityType, int abilityArea, int tokenType, int tokenValue) {
        client.TargetCurrentGameMakeAMove (client.connectionToClient, moveId, x, y, playerNumber, stackNumber, abilityType, abilityArea, tokenType, tokenValue);
        //VisualMatch.instance.ShowMatchResult (client, winnerName, winCondition, limit);
    }

    static public void CurrentGameRotateAbilityArea (ClientInterface client, int playerNumber, int stackNumber) {
        MatchClass match = client.currentMatch;
        if (!match.Properties.allowToRotateAbilityAreas) {
            return;
        }
        match.Player [playerNumber].RotateTopCard (stackNumber);
        client.TargetCurrentGameRotateAbilityArea (client.connectionToClient, stackNumber);
    }

    static public void CurrentGameFetchMissingMoves (ClientInterface client, int lastMoveId) {
        CurrentGameFetchMissingMoves (client, lastMoveId, client.currentMatch.LastMove);
    }

    static public void CurrentGameFetchMissingMoves (ClientInterface client, int lastMoveId, MoveHistoryClass mHistory) {
        if (mHistory != null && mHistory.moveId > lastMoveId) {
            CurrentGameFetchMissingMoves (client, lastMoveId, mHistory.prev);
            CardClass card = mHistory.usedCard;
            TargetCurrentGameMakeAMove (client, mHistory.moveId, mHistory.x, mHistory.y, mHistory.playerNumber, mHistory.stackNumber, card.abilityType, card.abilityArea, card.tokenType, card.value);
        }
    }

    static public void CurrentGameConcede (ClientInterface client) {
        if (client.currentMatch != null) {
            client.currentMatch.Concede (client.playerNumber);
            client.currentMatch = null;
        }
    }

}
