using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
        LoginReward (client);
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
        AIClass AI1 = null;
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
            AI1 = new AIClass ();
            hand1.GenerateRandomHand (gameMode, AI1);
        }
        HandClass hand2 = new HandClass ();
        AIClass AI2 = new AIClass ();
        //hand2.Init (1);
        //hand2.stack [0].Add (new CardClass (2, 0, 4, 51));
        hand2.GenerateRandomHand (gameMode, AI2);
        MatchClass match = MatchMakingClass.CreateGame (gameMode, 1, new PlayerPropertiesClass [] {
            new PlayerPropertiesClass (1, AI1, client.AccountName, client.UserName, hand1, client),
            new PlayerPropertiesClass (2, AI2, "AI opponent", "AI opponent", hand2, null) });
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
            if (player == null || player.properties == null) {
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
            match.properties.MatchPropertiesToString ());
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
                    HandClass hand = player.hand;
                    client.TargetDownloadCurrentGameHand (client.connectionToClient,
                        x, hand.ModeHandToString ());
                }
            }
        }
        client.TargetDownloadCurrentGamePlayedMove (client.connectionToClient, (match.LastMove != null) ? match.LastMove.PlayedToString () : null);
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

    static public void EnterCode (ClientInterface client, string code) {
        switch (code) {
            case "cowlevel":
                if (System.DateTime.Now.Year < 2020 || (System.DateTime.Now.Month <= 3 && System.DateTime.Now.Year <= 2020)) {
                    Debug.Log ("Test");
                    //UnlockAllAvatars (client);
                    UnlockAllTokensAndAbilities (client);
                    client.TargetLoadUnlockedContentData (client.connectionToClient, 2);
                } else {
                    client.TargetShowMessage (client.connectionToClient, Language.CodeHasExpiredKey);
                }
                break;
            default:
                client.TargetShowMessage (client.connectionToClient, Language.InvalidCodeKey);
                break;

        }

    }

    static public void UnlockAllAvatars (ClientInterface client) {
        string accountName = client.AccountName;
        bool [] previouslyUnlockedAvatars = ServerData.GetUserUnlockedAvatars (accountName);
        int avatarCount = AppDefaults.availableAvatars;
        List<int> avatarNumber = new List<int> ();
        for (int x = 1; x < avatarCount; x++) {
            if (!previouslyUnlockedAvatars [x]) {
                avatarNumber.Add (x);
            }
        }
        ServerData.UnlockAllAvatars (accountName);
        if (client != null) {
            if (avatarNumber.Count > 0) {
                client.TargetDownloadUnlockedContentData3 (client.connectionToClient, avatarNumber.ToArray (), null, null);
            }
        }
    }

    static public void UnlockAllTokensAndAbilities (ClientInterface client) {
        string accountName = client.AccountName;
        bool [] previouslyUnlockedAbilities = ServerData.GetUserUnlockedAbilities (accountName);
        int abilityCount = AppDefaults.availableAbilities;
        List<int> abilityNumber = new List<int> ();
        for (int x = 0; x < abilityCount; x++) {
            if (!previouslyUnlockedAbilities [x]) {
                abilityNumber.Add (x);
            }
        }
        bool [] previouslyUnlockedTokens = ServerData.GetUserUnlockedTokens (accountName);
        int tokenCount = AppDefaults.availableTokens;
        List<int> tokenNumber = new List<int> ();
        for (int x = 0; x < tokenCount; x++) {
            if (!previouslyUnlockedTokens [x]) {
                tokenNumber.Add (x);
            }
        }
        ServerData.UnlockAllAbilities (accountName);
        ServerData.UnlockAllTokens (accountName);
        if (client != null) {
            if (abilityNumber.Count > 0 || tokenNumber.Count > 0) {
                client.TargetDownloadUnlockedContentData3 (client.connectionToClient, null, abilityNumber.ToArray (), tokenNumber.ToArray ());
            }
        }
    }

    static public void LoginReward (ClientInterface client) {
        string accountName = client.AccountName;
        LevelUpReward (client, accountName, ServerData.GetUserLevel (accountName));
    }

    static public void LevelUpReward (ClientInterface client, string accountName, int reachedLevel) {
        bool [] previouslyUnlockedAbilities = ServerData.GetUserUnlockedAbilities (accountName);
        bool [] previouslyUnlockedTokens = ServerData.GetUserUnlockedTokens (accountName);
        List<int> abilityNumber = new List<int> ();
        List<int> tokenNumber = new List<int> ();
        if (reachedLevel >= 2) {
            if (!previouslyUnlockedAbilities [20]) {
                abilityNumber.Add (20);
            }
        }
        if (reachedLevel >= 3) {
            if (!previouslyUnlockedAbilities [28]) {
                abilityNumber.Add (28);
            }
        }
        if (reachedLevel >= 4) {
            if (!previouslyUnlockedAbilities [38]) {
                abilityNumber.Add (38);
            }
        }
        if (reachedLevel >= 5) {
            if (!previouslyUnlockedAbilities [42]) {
                abilityNumber.Add (42);
            }
        }
        if (reachedLevel >= 6) {
            if (!previouslyUnlockedTokens [13]) {
                tokenNumber.Add (13);
            }
        }
        if (reachedLevel >= 7) {
            if (!previouslyUnlockedTokens [14]) {
                tokenNumber.Add (14);
            }
        }
        if (reachedLevel >= 8) {
            if (!previouslyUnlockedAbilities [45]) {
                abilityNumber.Add (45);
            }
        }
        if (reachedLevel >= 9) {
            if (!previouslyUnlockedAbilities [46]) {
                abilityNumber.Add (46);
            }
        }
        if (reachedLevel >= 10) {
            if (!previouslyUnlockedAbilities [48]) {
                abilityNumber.Add (48);
            }
        }
        if (reachedLevel >= 11) {
            if (!previouslyUnlockedAbilities [49]) {
                abilityNumber.Add (49);
            }
        }
        if (reachedLevel >= 12) {
            if (!previouslyUnlockedAbilities [62]) {
                abilityNumber.Add (62);
            }
        }
        if (abilityNumber.Count > 0) {
            ServerData.SetUserUnlockedAbilities (accountName, abilityNumber.ToArray ());
        }
        if (tokenNumber.Count > 0) {
            ServerData.SetUserUnlockedTokens (accountName, tokenNumber.ToArray ());
        }
        if (client != null) {
            if (abilityNumber.Count > 0 || tokenNumber.Count > 0) {
                client.TargetDownloadUnlockedContentData2 (client.connectionToClient, null, abilityNumber.ToArray (), tokenNumber.ToArray ());
            }
        }
    }

    static public void SaveBossResult (ClientInterface client, int id, PlayerClass [] player) {
        string accountName = client.AccountName;
        string bossName = ServerData.GetGameModeName (id);
        int bossNumber = BossMenu.GameModeNameToBossID (bossName);
        ServerData.SetUserFinishedBoss (accountName, id);
        bool [] previouslyUnlockedAvatars = ServerData.GetUserUnlockedAvatars (accountName);
        int unlockedAvatar = bossNumber + 9;
        ServerData.SetUserUnlockedAvatar (accountName, unlockedAvatar);
        if (!previouslyUnlockedAvatars [unlockedAvatar]) {
            if (client != null) {
                client.TargetDownloadUnlockedContentData2 (client.connectionToClient, new int [1] { unlockedAvatar }, null, null );
            }
        }
        if (bossNumber == 7) {
            if (player [2].lost && player [3].lost) {
                ServerData.SetUserUnlockedBoss (accountName, 8);
            }
            if (player [2].lost && player [4].lost) {
                ServerData.SetUserUnlockedBoss (accountName, 10);
            }
            if (player [3].lost && player [4].lost) {
                ServerData.SetUserUnlockedBoss (accountName, 9);
            }
        }
        if (bossNumber == 11) {
            ServerData.SetUserUnlockedBoss (accountName, 12);
        }
    }

    static public void SaveTutorialResult (ClientInterface client, int id) {
        string accountName = client.AccountName;
        string tutorialName = ServerData.GetGameModeName (id);
        ServerData.SetUserFinishedTutorial (accountName, id);
    }

    static public void SavePuzzleResult (ClientInterface client, string accountName, int id) {
        ServerData.SetUserFinishedPuzzle (accountName, id);
        CardPoolClass cardPool = new CardPoolClass ();
        cardPool.LoadFromString (ServerData.GetCardPool (id));
        List<CardClass> cards = cardPool.Card;
        int count = cards.Count;
        bool [] previouslyUnlockedAbilities = ServerData.GetUserUnlockedAbilities (accountName);
        bool [] previouslyUnlockedTokens = ServerData.GetUserUnlockedTokens (accountName);
        List<int> abilities = new List<int> ();
        List<int> tokens = new List<int> ();
        for (int x = 0; x < count; x++) {
            CardClass card = cards [x];
            int aType = card.abilityType;
            int tType = card.tokenType;
            if (!abilities.Contains (aType) && !previouslyUnlockedAbilities [aType]) {
                abilities.Add (aType);
            }
            if (!tokens.Contains (tType) && !previouslyUnlockedTokens [tType]) {
                tokens.Add (tType);
            }
        }
        int [] abilityArray = abilities.ToArray ();
        int [] tokenArray = tokens.ToArray ();
        ServerData.SetUserUnlockedAbilities (accountName, abilityArray);
        ServerData.SetUserUnlockedTokens (accountName, tokenArray);
        if (client != null) {
            client.TargetDownloadUnlockedContentData2 (client.connectionToClient, null, abilityArray, tokenArray);
        }
    }

    static public void DownloadDataToUnlockedContentMenu (ClientInterface client) {
        string accountName = client.AccountName;
        bool [] abilities = ServerData.GetUserUnlockedAbilities (accountName);
        bool [] tokens = ServerData.GetUserUnlockedTokens (accountName);
        client.TargetDownloadUnlockedContentData (client.connectionToClient, abilities, tokens);

    }

    static public void DownloadDataToBossMenu (ClientInterface client) {
        string accountName = client.AccountName;
        int [] list = ServerData.GetAllGameModes ();
        bool [] unlockedBosses = ServerData.GetUserUnlockedBosses (accountName);
        List<int> officialIds = new List<int> ();
        List<int> officialBossNumbers = new List<int> ();
        List<bool> officialFinished = new List<bool> ();
        List<bool> unlocked = new List<bool> ();

        int [] finished = ServerData.GetUserFinishedBosses (accountName);

        foreach (int id in list) {
            string [] owners = ServerData.GetGameModeOwners (id);
            if (ServerData.GetGameModeIsOfficial (id) && ServerData.GetGameModeIsBoss (id)) {
                officialIds.Add (id);
            }
        }
        officialIds.Sort ((a, b) => (a.CompareTo (b)));

        foreach (int id in officialIds) {
            int number = BossMenu.GameModeNameToBossID (ServerData.GetGameModeName (id));
            officialBossNumbers.Add (number);
            officialFinished.Add (System.Array.IndexOf (finished, id) != -1);
            if (number < unlockedBosses.Length) {
                unlocked.Add (unlockedBosses [number]);
            } else {
                unlocked.Add (true);
            }
        }

        client.TargetDownloadBossList (client.connectionToClient,
            officialBossNumbers.ToArray (), officialIds.ToArray (), officialFinished.ToArray (), unlocked.ToArray ());
    }

    static public void DownloadDataToProfileSettings (ClientInterface client) {
        string accountName = client.AccountName;
        bool [] avatars = ServerData.GetUserUnlockedAvatars (accountName);

        client.TargetDownloadDataToProfileSettings (client.connectionToClient, avatars);
    }

    static public void DownloadDataToTutorialMenu (ClientInterface client) {
        string accountName = client.AccountName;
        int [] list = ServerData.GetAllGameModes ();
        List<int> officialIds = new List<int> ();
        List<string> officialNames = new List<string> ();

        foreach (int id in list) {
            string [] owners = ServerData.GetGameModeOwners (id);
            if (ServerData.GetGameModeIsOfficial (id) && ServerData.GetGameModeIsTutorial (id)) {
                if (CheckIfUserShouldBeAllowedToDoPuzzle (accountName, id)) {
                    officialIds.Add (id);
                }
            }
        }
        officialIds.Sort ((a, b) => (a.CompareTo (b)));

        foreach (int id in officialIds) {
            officialNames.Add (ServerData.GetGameModeName (id));
        }

        client.TargetDownloadDataToTutorialMenu (client.connectionToClient,
            officialNames.ToArray (), officialIds.ToArray ());
    }


    static public void DownloadDataToPuzzleMenu (ClientInterface client) {
        string accountName = client.AccountName;
        int [] list = ServerData.GetAllGameModes ();
        List<int> officialIds = new List<int> ();
        List<string> officialNames = new List<string> ();
        List<bool> officialFinished = new List<bool> ();

        int [] finished = ServerData.GetUserFinishedPuzzles (accountName);
        int toUnlockCount = 0;

        foreach (int id in list) {
            string [] owners = ServerData.GetGameModeOwners (id);
            if (ServerData.GetGameModeIsOfficial (id) && ServerData.GetGameModeIsPuzzle (id)) {
                if (CheckIfUserShouldBeAllowedToDoPuzzle (accountName, id)) {
                    officialIds.Add (id);
                } else {
                    toUnlockCount++;
                }
                //officialIds.Add (id);
            }
        }
        officialIds.Sort ((a, b) => (a.CompareTo (b)));

        foreach (int id in officialIds) {
            officialNames.Add (ServerData.GetGameModeName (id));
            officialFinished.Add (System.Array.IndexOf (finished, id) != -1);
        }

        client.TargetDownloadPuzzleList (client.connectionToClient,
            officialNames.ToArray (), officialIds.ToArray (), officialFinished.ToArray (), toUnlockCount);
    }

    static public bool CheckIfUserShouldBeAllowedToDoPuzzle (string accountName, int id) {
        int differencesCount = 0;
        bool [] availableAbilities = ServerData.GetUserUnlockedAbilities (accountName);
        bool [] availableTokens = ServerData.GetUserUnlockedTokens (accountName);
        CardPoolClass cardPool = new CardPoolClass ();
        cardPool.LoadFromString (ServerData.GetCardPool (id));
        foreach (CardClass card in cardPool.Card) {
            int aType = card.abilityType;
            if (!availableAbilities [aType]){
                differencesCount++;
                availableAbilities [aType] = true;
            }
            int tType = card.tokenType;
            if (!availableTokens [tType]) {
                differencesCount++;
                availableTokens [tType] = true;
            }
        }
        if (differencesCount <= 1) {
            return true;
        }
        return false;
    }

    static public void DownloadPreviewToPuzzleMenu (ClientInterface client, int id) {
        string name = ServerData.GetGameModeName (id);
        int [] ids = ServerData.GetAllGameModeBoards (id);
        string [] board = ServerData.GetBoard (ids [0]);
        string [] cardPool = ServerData.GetCardPool (id);
        client.TargetDownloadPreviewToPuzzleMenu (client.connectionToClient,
            name, board, cardPool);
    }

    static public MatchClass StartPuzzle (ClientInterface client, int id) {
        CardPoolClass cardPool = new CardPoolClass ();
        cardPool.LoadFromString (ServerData.GetCardPool (id));

        HandClass hand1 = new HandClass ();
        for (int x = 0; x < 4; x++) {
            hand1.stack [x].Add (cardPool.Card [x]);
        }
        string accountName = client.AccountName;
        int gameMode = id;
        HandClass hand2 = new HandClass ();
        AIClass AI2 = new AIClass ();
        hand2.GenerateRandomHand (gameMode, AI2);
        MatchClass match = MatchMakingClass.CreateGame (gameMode, 1, new PlayerPropertiesClass [] {
            new PlayerPropertiesClass (1, null, client.AccountName, client.UserName, hand1, client),
            new PlayerPropertiesClass (2, AI2, "AI opponent", ServerData.GetGameModeName (id), hand2, null) });
        PlayerClass player = match.Player [2];
        PlayerPropertiesClass properties = player.properties;
        player.enabled = false;
        properties.AI.puzzle = true;
        match.properties.special = true;
        match.properties.specialType = 1;
        StartMatch (match);
        return match;
    }

    static public MatchClass StartBoss (ClientInterface client, int id, string bossName2, string bossName3 = "", string bossName4 = "") {
        
        string accountName = client.AccountName;
        int gameMode = id;

        CardPoolClass cardPool = new CardPoolClass ();
        cardPool.LoadFromString (ServerData.GetCardPool (id));

        HandClass hand1 = new HandClass ();
        int selectedSet = ServerData.GetAllPlayerModeSets (accountName, gameMode) [0];
        hand1.LoadFromFile (client.AccountName, gameMode, selectedSet);
        if (!hand1.IsValid (gameMode)) {
            int minimumNumberOfCardsInStack = ServerData.GetGameModeMinimumNumberOfCardsInStack (gameMode);
            client.TargetInvalidSet (client.connectionToClient, minimumNumberOfCardsInStack);
            return null;
        }

        AIClass AI2 = new AIClass ();
        string gameModeName = ServerData.GetGameModeName (id);
        int bossNumber = int.Parse (gameModeName.Substring (gameModeName.IndexOf ('#') + 1));
        HandClass hand2 = new HandClass ();
        hand2.GenerateBossHand (bossNumber);
        HandClass hand3 = new HandClass ();
        HandClass hand4 = new HandClass ();
        switch (bossNumber) {
            case 6:
                hand3.GenerateBossHand (bossNumber, 3);
                break;
            case 7:
                hand3.GenerateBossHand (bossNumber, 3);
                hand4.GenerateBossHand (bossNumber, 4);
                break;
        }

        MatchClass match = null;
        switch (bossNumber) {
            case 6: {
                AIClass AI3 = new AIClass ();
                match = MatchMakingClass.CreateGame (gameMode, 1, new PlayerPropertiesClass [] {
                    new PlayerPropertiesClass (1, null, client.AccountName, client.UserName, hand1, client),
                    new PlayerPropertiesClass (2, AI2, "AI opponent", bossName2, hand2, null),
                    new PlayerPropertiesClass (1, AI3, "AI opponent", bossName3, hand3, null) });
                }
        break;
            case 7: {
                AIClass AI3 = new AIClass ();
                AIClass AI4 = new AIClass ();
                match = MatchMakingClass.CreateGame (gameMode, 1, new PlayerPropertiesClass [] {
                    new PlayerPropertiesClass (1, null, client.AccountName, client.UserName, hand1, client),
                    new PlayerPropertiesClass (2, AI2, "AI opponent", bossName2, hand2, null),
                    new PlayerPropertiesClass (2, AI3, "AI opponent", bossName3, hand3, null),
                    new PlayerPropertiesClass (2, AI4, "AI opponent", bossName4, hand4, null) });
            }
            break;
            default:
                match = MatchMakingClass.CreateGame (gameMode, 1, new PlayerPropertiesClass [] {
                    new PlayerPropertiesClass (1, null, client.AccountName, client.UserName, hand1, client),
                    new PlayerPropertiesClass (2, AI2, "AI opponent", bossName2, hand2, null) });
                break;
        }
        for (int x = 2; x <= 4; x++) {
            if (match.numberOfPlayers < x) {
                break;
            }
            PlayerPropertiesClass properties = match.Player [x].properties;
            properties.AI.boss = true;
            switch (x) {
                default:
                    switch (bossNumber) {
                        default:
                            properties.avatar = bossNumber + 9;
                            properties.specialStatus = bossNumber;
                            break;
                        case 7:
                            properties.avatar = 18;
                            properties.specialStatus = 7;
                            break;
                    }
                    break;
                case 3:
                    switch (bossNumber) {
                        case 6:
                            properties.avatar = 16;
                            properties.specialStatus = 0;
                            break;
                        case 7:
                            properties.avatar = 19;
                            properties.specialStatus = 7;
                            break;
                    }
                    break;
                case 4:
                    switch (bossNumber) {
                        case 7:
                            properties.avatar = 17;
                            properties.specialStatus = 7;
                            break;
                    }
                    break;
            }
        }
        match.properties.special = true;
        match.properties.specialType = 2;
        StartMatch (match);
        return match;
    }

    static public MatchClass StartTutorial (ClientInterface client, int id, string tutorialName) {

        string accountName = client.AccountName;
        int gameMode = id;

        string gameModeName = ServerData.GetGameModeName (id);
        int tutorialNumber = int.Parse (gameModeName.Substring (gameModeName.IndexOf ('#') + 1));

        HandClass hand1 = new HandClass ();
        hand1.GenerateTutorialHand (tutorialNumber, true);

        AIClass AI2 = new AIClass ();
        PlayerPropertiesClass properties;

        MatchClass match = null;
        switch (tutorialNumber) {
            case 1:
                match = MatchMakingClass.CreateGame (gameMode, 0, new PlayerPropertiesClass [] {
            new PlayerPropertiesClass (1, null, client.AccountName, client.UserName, hand1, client) });

                match.properties.scoreLimit = 24;
                match.properties.turnWinCondition = false;
                break;
            case 2:
                match = MatchMakingClass.CreateGame (gameMode, 1, new PlayerPropertiesClass [] {
            new PlayerPropertiesClass (1, null, client.AccountName, client.UserName, hand1, client),
                new PlayerPropertiesClass (2, AI2, "AI opponent", tutorialName, null, null) });
                match.properties.scoreLimit = 50;
                match.properties.turnWinCondition = false;


                properties = match.Player [2].properties;
                match.Player [2].enabled = false;
                break;
            case 3:
            case 4:
                HandClass hand2 = new HandClass ();
                hand2.GenerateTutorialHand (tutorialNumber, false);

                match = MatchMakingClass.CreateGame (gameMode, 1, new PlayerPropertiesClass [] {
            new PlayerPropertiesClass (1, null, client.AccountName, client.UserName, hand1, client),
            new PlayerPropertiesClass (2, AI2, "AI opponent", tutorialName, hand2, null) });

                if (tutorialNumber == 3) {
                    match.properties.scoreLimit = 125;
                    match.properties.turnWinCondition = false;
                } else {
                    match.properties.scoreLimit = 250;
                    match.properties.turnLimit = 10;
                }

                properties = match.Player [2].properties;
                break;
        }
        match.properties.special = true;
        match.properties.specialType = 3;
        match.properties.specialId = tutorialNumber;
        StartMatch (match);
        return match;
    }



    static public void EditBossSet (ClientInterface client, int bossId) {
        string accountName = client.AccountName;
        int gameMode = bossId;
        if (!ServerData.GameModeContentExists (gameMode)) {
            client.TargetShowMessage (client.connectionToClient, Language.YouNeedToSelectSomethingFirstKey);
            return;
        }
        int [] setIds = ServerData.GetAllPlayerModeSets (accountName, gameMode);
        int setId;
        HandClass hand = new HandClass ();
        if (setIds.Length == 0) {
            setId = ServerData.CreatePlayerModeSet (accountName, gameMode, hand.HandToString(), "Set");
        } else {
            setId = setIds [0];
        }
        string [] cardPool = ServerData.GetCardPool (gameMode);
        bool [] unlockedAbilities = ServerData.GetUserUnlockedAbilities (accountName);
        bool [] unlockedTokens = ServerData.GetUserUnlockedTokens (accountName);
        hand.LoadFromFile (accountName, gameMode, setId);
        string [] set = hand.HandToString ();
        string name = "";
        int iconNumber = 1;
        bool usedCardsArePutOnBottomOfStack = true;
        int numberOfStacks = 4;
        int minimumNumberOfCardsOnStack = 2;
        bool everyCardCanBeAddedToSetAnyNumberOfTimes = ServerData.GetGameModeEveryCardCanBeAddedToSetAnyNumberOfTimes (gameMode);
        client.TargetDownloadDataToSetEditor (client.connectionToClient, setId, 1, cardPool, unlockedAbilities, unlockedTokens, set, 
            name, iconNumber, usedCardsArePutOnBottomOfStack, numberOfStacks, minimumNumberOfCardsOnStack, everyCardCanBeAddedToSetAnyNumberOfTimes);

    }


    static public void DownloadDataToSetEditor (ClientInterface client, int setId) {
        string accountName = client.AccountName;
        int gameMode = client.GameMode;
        string [] cardPool = ServerData.GetCardPool (gameMode);
        bool [] unlockedAbilities = ServerData.GetUserUnlockedAbilities (accountName);
        bool [] unlockedTokens = ServerData.GetUserUnlockedTokens (accountName);
        HandClass hand = new HandClass ();
        hand.LoadFromFile (accountName, gameMode, setId);
        string [] set = hand.HandToString ();
        string name = ServerData.GetPlayerModeSetName (accountName, gameMode, setId);
        int iconNumber = ServerData.GetPlayerModeSetIconNumber (accountName, gameMode, setId);
        bool usedCardsArePutOnBottomOfStack = ServerData.GetGameModeUsedCardsArePutOnBottomOfStack (gameMode);
        int numberOfStacks = ServerData.GetGameModeNumberOfStacks (gameMode);
        int minimumNumberOfCardsOnStack = ServerData.GetGameModeMinimumNumberOfCardsInStack (gameMode);
        bool everyCardCanBeAddedToSetAnyNumberOfTimes = ServerData.GetGameModeEveryCardCanBeAddedToSetAnyNumberOfTimes (gameMode);
        client.TargetDownloadDataToSetEditor (client.connectionToClient, setId, 0, cardPool, unlockedAbilities, unlockedTokens, set,
            name, iconNumber, usedCardsArePutOnBottomOfStack, numberOfStacks, minimumNumberOfCardsOnStack, everyCardCanBeAddedToSetAnyNumberOfTimes);
    }


    static public void DownloadCardPoolToCardPoolEditor (ClientInterface client, int gameModeId) {
        string accountName = client.AccountName;
        bool isClientOwner = ServerData.IsGameModeOwner (gameModeId, accountName);
        client.TargetDownloadCardPoolToEditor (client.connectionToClient, isClientOwner, gameModeId, ServerData.GetCardPool (gameModeId));
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
        List<int> teams = new List<int> ();

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
            teams.Add (customGame.team [x]);
        }
        foreach (ClientInterface client in customGame.clients) {
            if (client != null) {
                bool isHost = client == customGame.host;
                client.TargetDownloadCustomGameRoom (client.connectionToClient,
                    isHost, customGame.name, customGame.matchType, 
                    avatars.ToArray (), userNames.ToArray (), AI.ToArray (), teams.ToArray());
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
                if (!ServerData.GetGameModeIsPuzzle (id) &&! ServerData.GetGameModeIsTutorial (id) && !ServerData.GetGameModeIsBoss (id)) {
                    officialIds.Add (id);
                }
            } else {
                if (ServerData.GetIsGameModeLegal (id) && !ServerData.GetGameModeDeleted (id)) {
                    publicIds.Add (id);
                }
            }
            foreach (string owner in owners) {
                if (accountName == owner && !ServerData.GetGameModeDeleted (id)) {
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
        string accountName = client.AccountName;
        bool isClientOwner = ServerData.IsGameModeOwner (id, accountName);
        bool hasScoreWinCondition = ServerData.GetGameModeHasScoreWinCondition (id);
        int scoreWinConditionValue = ServerData.GetGameModeScoreWinConditionValue (id);
        bool hasTurnWinCondition = ServerData.GetGameModeHasTurnWinCondition (id);
        int turnWinConditionValue = ServerData.GetGameModeTurnWinConditionValue (id);
        bool isAllowedToRotateCardsDuringMatch = ServerData.GetGameModeIsAllowedToRotateCardsDuringMatch (id);
        int numberOfStacks = ServerData.GetGameModeNumberOfStacks (id);
        int minimumNumberOfCardsInStack = ServerData.GetGameModeMinimumNumberOfCardsInStack (id);
        bool usedCardsArePutOnBottomOfStack = ServerData.GetGameModeUsedCardsArePutOnBottomOfStack (id);
        bool everyCardCanBeAddedToSetAnyNumberOfTimes = ServerData.GetGameModeEveryCardCanBeAddedToSetAnyNumberOfTimes (id);


        client.TargetDownloadGameModeSettingsToEditor (client.connectionToClient, 
            isClientOwner,
            hasScoreWinCondition, scoreWinConditionValue, hasTurnWinCondition, turnWinConditionValue,
            isAllowedToRotateCardsDuringMatch, numberOfStacks, minimumNumberOfCardsInStack, usedCardsArePutOnBottomOfStack, everyCardCanBeAddedToSetAnyNumberOfTimes);
    }

    static public void SaveGameModeSettings (ClientInterface client, int id,
        bool hasScoreWinCondition, int scoreWinConditionValue, bool hasTurnWinCondition, int turnWinConditionValue,
        bool isAllowedToRotateCardsDuringMatch, int numberOfStacks, int minimumNumberOfCardsInStack, bool usedCardsArePutOnBottomOfStack,
        bool everyCardCanBeAddedToSetAnyNumberOfTimes) {
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
        ServerData.SetGameModeUsedCardsArePutOnBottomOfStack (id, usedCardsArePutOnBottomOfStack);
        ServerData.SetGameModeEveryCardCanBeAddedToSetAnyNumberOfTimes (id, everyCardCanBeAddedToSetAnyNumberOfTimes);
    }

    static public void SaveGameModePlayerSettings (ClientInterface client, int id, int [] statuses) {
        if (!ServerData.IsGameModeOwner (id, client.AccountName)) {
            return;
        }
        ServerData.SetGameModeStatuses (id, statuses);
        client.TargetShowMessage (client.connectionToClient, Language.SettingsHaveBeenSavedKey);
    }

    static public void DownloadGameModePlayerSettings (ClientInterface client, int id) {
        int [] statuses = ServerData.GetGameModeStatuses (id);
        client.TargetDownloadGameModePlayerSettings (client.connectionToClient, statuses);
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
        int level = ServerData.GetUserLevel (accountName);
        int currentExperience = ServerData.GetUserExperience (accountName);
        int neededExperience = ExperienceNeededToLevelUp (level);
        client.TargetDownloadProfileDataToMenu (client.connectionToClient,
            displayName, avatarNumber, thisGameModeWon, thisGameModeLost, thisGameModeDrawn, thisGameModeUnfinished, totalWon, totalLost, totalDrawn, totalUnfinished, 
            level, currentExperience, neededExperience);

    }



    static public int ExperienceNeededToLevelUp (int level) {
        return 170 + 30 * level;
    }

    static public void AddExperience (ClientInterface client, string accountName, int experienceToAdd) {
        int currentLevel = ServerData.GetUserLevel (accountName);
        int previousLevel = currentLevel;
        int currentExperience = ServerData.GetUserExperience (accountName);
        currentExperience += experienceToAdd;
        while (true) {
            int experienceNeeded = ExperienceNeededToLevelUp (currentLevel);
            if (currentExperience >= experienceNeeded) {
                currentExperience -= experienceNeeded;
                currentLevel++;
            } else {
                break;
            }
        }
        ServerData.SetUserLevel (accountName, currentLevel);
        if (currentLevel > previousLevel) {
            LevelUpReward (client, accountName, currentLevel);
        }
        ServerData.SetUserExperience (accountName, currentExperience);
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
        int [] ids = ServerData.GetAllGameModeBoards (gameModeId);
        int count = ids.Length;
        string [] names = new string [count];
        bool [] isLegal = new bool [count];
        for (int x = 0; x < count; x++) {
            names [x] = ServerData.GetBoardName (ids [x]);
            isLegal [x] = ServerData.GetIsBoardLegal (ids [x]);
        }
        string gameModeName = ServerData.GetGameModeName (gameModeId);
        client.TargetDownloadGameModeToEditor (client.connectionToClient, isClientOwner, gameModeId, gameModeName, names, ids, isLegal);
    }

    static public void DownloadBoardToEditor (ClientInterface client, int boardId) {
        string [] owners = ServerData.GetBoardOwners (boardId);
        string accountName = client.AccountName;
        bool isClientOwner = false;
        foreach (string s in owners) {
            if (accountName == s) {
                isClientOwner = true;
                break;
            }
        }
        string boardName = ServerData.GetBoardName (boardId);
        string [] board = ServerData.GetBoard (boardId);
        int [] matchTypes = ServerData.GetBoardMatchTypes (boardId);
        client.TargetDownloadBoardToEditor (client.connectionToClient, isClientOwner, boardId, boardName, board, matchTypes);
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
        SavePlayerModeSet (client, client.GameMode, lines, setId);
        //client.TargetDownloadCardPoolToEditor (client.connectionToClient, ServerData.GetCardPool (1));
    }

    static public void SavePlayerModeSet (ClientInterface client, int gameMode, string [] lines, int setId) {
        ServerData.SavePlayerModeSet (client.AccountName, gameMode, setId, lines);
        HandClass hand = new HandClass ();
        hand.LoadFromFileString (gameMode, lines);
        if (hand.IsValid (gameMode)) {
            client.TargetShowMessage (client.connectionToClient, Language.SetSavedKey);
        } else {
            client.TargetInvalidSavedSet (client.connectionToClient);
        }
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
        if (!match.properties.allowToRotateAbilityAreas) {
            return;
        }
        match.RotateAbilityArea (playerNumber, stackNumber);
        client.TargetCurrentGameRotateAbilityArea (client.connectionToClient, stackNumber);
    }

    static public void CurrentGameFetchMissingMoves (ClientInterface client, int lastMoveId) {
        CurrentGameFetchMissingMoves (client, lastMoveId, client.currentMatch.LastMove);
    }

    static public void CurrentGameFetchMissingMoves (ClientInterface client, int lastMoveId, MoveHistoryClass mHistory) {
        if (mHistory != null && mHistory.moveId > lastMoveId) {
            CurrentGameFetchMissingMoves (client, lastMoveId, mHistory.prev);
            CardClass card = mHistory.usedCard;
            TargetCurrentGameMakeAMove (client, mHistory.moveId, mHistory.x, mHistory.y, mHistory.playerNumber, mHistory.stackNumber, card.abilityType, card.abilityArea, card.tokenType, card.tokenValue);
        }
    }

    static public void CurrentGameConcede (ClientInterface client) {
        if (client.currentMatch != null) {
            client.currentMatch.Concede (client.playerNumber);
            client.currentMatch = null;
        }
    }

}
