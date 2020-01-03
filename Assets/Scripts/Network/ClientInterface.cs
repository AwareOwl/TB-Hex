using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientInterface : NetworkBehaviour {

    public string AccountName;
    public string UserName;
    public int GameMode = 2;

    public MatchClass currentMatch;
    public int playerNumber;

    public void Start () {
        if (isServer) {
            gameObject.AddComponent<ServerManagement> ().enabled = true;
        }
        if (isLocalPlayer) {
            ClientLogic.MyInterface = this;
            gameObject.AddComponent<InputController> ();
            CmdCompareServerVersion ("0.16.0.3");
        }
    }

    [Command]
    public void CmdLogIn (string username, string password) {
        ServerLogic.LogIn (this, username, password);
    }

    [Command]
    public void CmdRegister (string accountName, string userName, string password, string email) {
        ServerLogic.Register (this, accountName, userName, password, email);
    }

    [TargetRpc]
    public void TargetShowMessage (NetworkConnection target, int messageID) {
        Debug.Log (Language.UI [messageID]);
        if (messageID == Language.YouHaveBeenKickedFromTheRoomKey) {
            GOUI.ShowMessage (Language.UI [messageID], UIString.ShowCustomGameLobby);
            return;
        }
        GOUI.ShowMessage (Language.UI [messageID]);
    }

    [Command]
    public void CmdChangeGameMode (int gameModeId) {
        ServerLogic.ChangeGameMode (this, gameModeId);
    }

    
    [TargetRpc]
    public void TargetShowMatchResult (NetworkConnection target, int matchType, string [] winnersNames, int winCondition, int limit, 
        int level, int currentExperience, int maxExperience, int experienceGain) {
        MatchClass pm = InGameUI.PlayedMatch;
        if (pm != null) {
            VisualMatch vm = pm.visualMatch;
            if (vm != null) {
                vm.ShowMatchResult (matchType, winnersNames, winCondition, limit, level, currentExperience, maxExperience, experienceGain);
            }
        }
    }

    [TargetRpc]
    public void TargetLogIn (NetworkConnection target, string accountName, string userName) {
        ClientLogic.LogIn (accountName, userName);
    }

    public void SaveBossResult (int id, PlayerClass [] player) {
        if (isServer) {
            ServerLogic.SaveBossResult (this, id, player);
        }
    }

    public void SaveTutorialResult (int id) {
        if (isServer) {
            ServerLogic.SaveTutorialResult (this, id);
        }
    }

    public void SavePuzzleResult (int id) {
        if (isServer) {
            ServerLogic.SavePuzzleResult (this, AccountName, id);
        }
    }

    [Command]
    public void CmdEnterCode (string s) {
        ServerLogic.EnterCode (this, s);
    }

    [Command]
    public void CmdJoinGameAgainstAI () {
        ServerLogic.JoinGameAgainstAI (this);
    }

    [Command]
    public void CmdStartBoss (int id, string name2, string name3, string name4) {
        ServerLogic.StartBoss (this, id, name2, name3, name4);
    }



    [Command]
    public void CmdStartTutorial (int id, string name) {
        ServerLogic.StartTutorial (this, id, name);
    }

    [Command]
    public void CmdStartPuzzle (int id) {
        ServerLogic.StartPuzzle (this, id);
    }

    [Command]
    public void CmdJoinQuickMatchQueue () {
        ServerLogic.JoinQuickMatchQueue (this);
    }

    [TargetRpc]
    public void TargetShowQuickMatchQueue (NetworkConnection target) {
        QueueMenu.ShowQueueMenu ();
    }

    [Command]
    public void CmdLeaveQuickQueue () {
        ServerLogic.LeaveQuickMatchQueue (this);
    }

    [TargetRpc]
    public void TargetShowMainMenu (NetworkConnection target) {
        MainMenu.ShowMainMenu ();
    }

    [TargetRpc]
    public void TargetInvalidSet (NetworkConnection target, int number) {
        GOUI.ShowMessage (Language.GetInvalidSetMessage (number));
    }

    [TargetRpc]
    public void TargetInvalidSavedSet (NetworkConnection target) {
        GOUI.ShowMessage (Language.GetInvalidSavedSetMessage ());
    }

    [Command]
    public void CmdCompareServerVersion (string version) {
        ServerLogic.CompareServerVersion (this, version);
    }

    [TargetRpc]
    public void TargetShowLoginMenu (NetworkConnection target) {
        LoginMenu.ShowLoginMenu ();
    }

    [TargetRpc]
    public void TargetInvalidVersionMessage (NetworkConnection target, string serverVersion) {
        GOUI.ShowMessage (Language.GetInvalidGameVersionMessage (serverVersion), "ExitGame");
    }

    [Command]
    public void CmdDownloadDataToUnlockedContentMenu () {
        ServerLogic.DownloadDataToUnlockedContentMenu (this);
    }

    [TargetRpc]
    public void TargetDownloadUnlockedContentData (NetworkConnection target, bool [] abilities, bool [] tokens) {
        UnlockedContent.LoadUnlockedContentMenu (abilities, tokens);
    }

    [TargetRpc]
    public void TargetDownloadUnlockedContentData2 (NetworkConnection target, int [] avatars, int [] abilities, int [] tokens) {
        UnlockedContent.LoadUnlockedContentData (avatars, abilities, tokens);
    }

    [TargetRpc]
    public void TargetDownloadUnlockedContentData3 (NetworkConnection target, int [] avatars, int [] abilities, int [] tokens) {
        UnlockedContent.LoadUnlockedContentData (avatars, abilities, tokens);
    }

    [TargetRpc]
    public void TargetLoadUnlockedContentData (NetworkConnection target, int redirection) {
        UnlockedContent.LoadUnlockedContentMenu (redirection);
    }

    [Command]
    public void CmdDownloadDataToBossMenu () {
        ServerLogic.DownloadDataToBossMenu (this);
    }

    [TargetRpc]
    public void TargetDownloadBossList (NetworkConnection target, int [] bossNumbers, int [] officialIds, bool [] officialFinished, bool [] unlocked) {
        BossMenu.LoadBossMenu (bossNumbers, officialIds, officialFinished, unlocked);
    }

    [Command]
    public void CmdDownloadDataToProfileSettings () {
        ServerLogic.DownloadDataToProfileSettings (this);
    }

    [TargetRpc]
    public void TargetDownloadDataToProfileSettings (NetworkConnection target, bool [] ids) {
        ProfileSettings.LoadData (ids);
    }

    [Command]
    public void CmdDownloadDataToTutorialMenu () {
        ServerLogic.DownloadDataToTutorialMenu (this);
    }

    [TargetRpc]
    public void TargetDownloadDataToTutorialMenu (NetworkConnection target, string [] officialNames, int [] officialIds) {
        TutorialMenu.LoadTutorialMenu (officialNames, officialIds);
    }


    [Command]
    public void CmdDownloadDataToPuzzleMenu () {
        ServerLogic.DownloadDataToPuzzleMenu (this);
    }

    [TargetRpc]
    public void TargetDownloadPuzzleList (NetworkConnection target, string [] officialNames, int [] officialIds, bool [] officialFinished, int toUnlockCount) {
        PuzzleMenu.LoadPuzzleMenu (officialNames, officialIds, officialFinished, toUnlockCount);
    }

    [Command]
    public void CmdDownloadPreviewToPuzzleMenu (int id) {
        ServerLogic.DownloadPreviewToPuzzleMenu (this, id);
    }

    [TargetRpc]
    public void TargetDownloadPreviewToPuzzleMenu (NetworkConnection target, string puzzleName, string [] puzzleBoard, string [] puzzleCardPool) {
        PuzzleMenu.LoadPuzzlePreview (puzzleName, puzzleBoard, puzzleCardPool);
    }

    [Command]
    public void CmdEditBossSet (int bossId) {
        ServerLogic.EditBossSet (this, bossId);
    }

    [Command]
    public void CmdDownloadDataToSetEditor (int setId) {
        ServerLogic.DownloadDataToSetEditor (this, setId);
    }

    [TargetRpc]
    public void TargetDownloadDataToSetEditor (NetworkConnection target, int setId, int editorMode, string [] cardPool, bool [] unlockedAbilities, bool [] unlockedTokens, string [] set,
        string name, int icon, 
        bool usedCardsArePutOnBottomOfStack, int numberOfStacks, int minimumNumberOfCardsPerStack, bool everyCardCanBeAddedToSetAnyNumberOfTimes) {
        SetEditor.LoadData (setId, editorMode, cardPool, unlockedAbilities, unlockedTokens, set, 
            name, icon, usedCardsArePutOnBottomOfStack, numberOfStacks, minimumNumberOfCardsPerStack, everyCardCanBeAddedToSetAnyNumberOfTimes);
    }
    
    [Command]
    public void CmdDownloadCardPoolToEditor (int gameModeId) {
        ServerLogic.DownloadCardPoolToCardPoolEditor (this, gameModeId);
    }

    [TargetRpc]
    public void TargetDownloadCardPoolToEditor (NetworkConnection target, bool isClientOwner, int gameModeId, string [] lines) {
        CardPoolEditor.LoadDataToEditor (gameModeId, lines);
    }


    [Command]
    public void CmdSaveSelectedSet (int selectedSet) {
        ServerLogic.SaveSelectedSet (this, selectedSet);
    }


    [Command]
    public void CmdDownloadSetList () {
        ServerLogic.DownloadSetList (this);
    }

    [TargetRpc]
    public void TargetDownloadSetList (NetworkConnection target, string [] setName, int [] setNumber, int [] iconNumber, bool [] legal, int selectedSet) {
        SetList.LoadSetList (setName, setNumber, iconNumber, legal, selectedSet);
    }


    [Command]
    public void CmdCreateNewSet (string setName) {
        ServerLogic.CreateNewSet (this, setName);
    }


    [Command]
    public void CmdDeleteSet (int id) {
        ServerLogic.DeleteSet (this, id);
    }

    [Command]
    public void CmdSavePlayerModeSet (string [] s, int setId) {
        ServerLogic.SavePlayerModeSet (this, s, setId);
    }

    [Command]
    public void CmdSavePlayerModeSet2 (int gameMode, string [] s, int setId) {
        ServerLogic.SavePlayerModeSet (this, gameMode, s, setId);
    }

    [Command]
    public void CmdSaveSetProperties (int setId, string setName, int iconNumber) {
        ServerLogic.SaveSetProperties (this, setId, setName, iconNumber);
    }

    [Command]
    public void CmdSaveGameModeProperties (int setId, string setName, int iconNumber) {
        ServerLogic.SaveGameModeProperties (this, setId, setName, iconNumber);
    }

    [Command]
    public void CmdSaveBoardProperties (int setId, string setName, int iconNumber) {
        ServerLogic.SaveBoardProperties (this, setId, setName, iconNumber);
    }

    [Command]
    public void CmdDownloadGameModeLists () {
        ServerLogic.DownloadGameModeLists (this);
    }

    [Command]
    public void CmdDownloadCurrentGame () {
        ServerLogic.DownloadGame (this, MatchMakingClass.FindMatch (AccountName));
    }

    [TargetRpc]
    public void TargetDownloadGameModeLists (NetworkConnection target, int gameMode,
        string [] officialNames, string [] publicNames, string [] yourNames, 
        int [] officialIds, int [] publicIds, int [] yourIds, bool [] yourIsLegal) {
        GameModeMenu.UpdateLists (gameMode, officialNames, publicNames, yourNames, officialIds, publicIds, yourIds, yourIsLegal);
    }
    

    [TargetRpc]
    public void TargetDownloadCurrentGameMatch (NetworkConnection target, string [] lines) {
        ClientLogic.LoadCurrentGameMatch (lines);
    }

    [TargetRpc]
    public void TargetDownloadCurrentGameMatchProperties (NetworkConnection target, string [] lines) {
        ClientLogic.LoadCurrentGameMatchProperties (lines);
    }
    [TargetRpc]
    public void TargetDownloadCurrentGameBoard (NetworkConnection target, string [] lines) {
        ClientLogic.LoadCurrentGameBoard (lines);
    }

    [TargetRpc]
    public void TargetDownloadCurrentGamePlayer (NetworkConnection target, int playerNumber, string [] lines) {
        ClientLogic.LoadCurrentGamePlayer (playerNumber, lines);
    }

    [TargetRpc]
    public void TargetDownloadCurrentGamePlayerProperties (NetworkConnection target, int playerNumber, string [] lines) {
        ClientLogic.LoadCurrentGamePlayerProperties (playerNumber, lines);
    }
    
    [TargetRpc]
    public void TargetDownloadCurrentGameHand (NetworkConnection target, int playerNumber, string [] lines) {
        ClientLogic.LoadCurrentGameHand (playerNumber, lines);
    }

    [TargetRpc]
    public void TargetDownloadCurrentGamePlayedMove (NetworkConnection target, string [] lines) {
        ClientLogic.LoadCurrentGamePlayedMove (lines);
    }


    [TargetRpc]
    public void TargetFinishDownloadCurrentGame (NetworkConnection target, int playerNumber) {
        this.playerNumber = playerNumber;
        InGameUI.ShowInGameUI ();
    }


    [Command]
    public void CmdCurrentGameMakeAMove (int x, int y, int stackNumber) {
        if (InputController.debuggingEnabled) {
            Debug.Log ("Play card command sent to server");
        }
        ServerLogic.CurrentGameMakeAMove (this, x, y, playerNumber, stackNumber);
    }

    [Command]
    public void CmdCurrentGameRotateAbilityArea (int stackNumber) {
        ServerLogic.CurrentGameRotateAbilityArea (this, playerNumber, stackNumber);
    }

    [TargetRpc]
    public void TargetCurrentGameRotateAbilityArea (NetworkConnection target, int stackNumber) {
        InGameUI.RotateAbilityArea (playerNumber, stackNumber);
    }

    [TargetRpc]
    public void TargetCurrentGameMakeAMove (NetworkConnection target, int moveId, int x, int y, int playerNumber, int stackNumber, int abilityType, int abilityArea, int tokenType, int tokenValue) {
        if (InGameUI.PlayedMatch != null) {
            InGameUI.PlayedMatch.PlayCard (moveId, x, y, playerNumber, stackNumber, (AbilityType) abilityType, abilityArea, (TokenType) tokenType, tokenValue);
        }
    }

    [Command]
    public void CmdCurrentGameFetchMissingMoves (int lastMoveId) {
        ServerLogic.CurrentGameFetchMissingMoves (this, lastMoveId);
    }

    //public void CmdUpdate

    [TargetRpc]
    public void TargetGetStartingSetName (NetworkConnection target) {
        CmdSetStartingSetName (Language.StartingSet);
    }

    [Command]
    public void CmdCurrentGameConcede () {
        ServerLogic.CurrentGameConcede (this);
    }

    [TargetRpc]
    public void TargetCurrentGameConcede (NetworkConnection target, int playerNumber) {
        InGameUI.PlayedMatch.Concede (playerNumber);
    }

    [Command]
    public void CmdSetStartingSetName (string startingSetName) {
        AccountVersionManager.SetStartingSetNameToAllSets (AccountName, startingSetName);
    }

    [Command]
    public void CmdDownloadGameModeSettings (int id) {
        ServerLogic.DownloadGameModeSettings (this, id);
    }

    [TargetRpc]
    public void TargetDownloadGameModeSettingsToEditor (NetworkConnection target, 
        bool isClientOwner,
        bool hasScoreWinCondition, int scoreWinConditionValue, bool hasTurnWinCondition, int turnWinConditionValue,
        bool isAllowedToRotateCardsDuringMatch, int numberOfStacks, int minimumNumberOfCardsInStack, bool usedCardsArePutOnBottomOfStack,
        bool everyCardCanBeAddedToSetAnyNumberOfTimes) {
        GameModeSettingsEditor.ShowGameModeSettingsEditor (
            isClientOwner, 
            hasScoreWinCondition, scoreWinConditionValue, hasTurnWinCondition, turnWinConditionValue, 
            isAllowedToRotateCardsDuringMatch, numberOfStacks, minimumNumberOfCardsInStack, usedCardsArePutOnBottomOfStack, everyCardCanBeAddedToSetAnyNumberOfTimes);
    }

    [Command]
    public void CmdSaveGameModeSettings (int id,
        bool hasScoreWinCondition, int scoreWinConditionValue, bool hasTurnWinCondition, int turnWinConditionValue,
        bool isAllowedToRotateCardsDuringMatch, int numberOfStacks, int minimumNumberOfCardsInStack, bool usedCardsArePutOnBottomOfStack,
        bool everyCardCanBeAddedToSetAnyNumberOfTimes) {
        ServerLogic.SaveGameModeSettings (this, id,
            hasScoreWinCondition, scoreWinConditionValue, hasTurnWinCondition, turnWinConditionValue,
            isAllowedToRotateCardsDuringMatch, numberOfStacks, minimumNumberOfCardsInStack, usedCardsArePutOnBottomOfStack, everyCardCanBeAddedToSetAnyNumberOfTimes);
    }

    [Command]
    public void CmdSaveGameModePlayerSettings (int id, int [] statuses) {
        ServerLogic.SaveGameModePlayerSettings (this, id, statuses);
    }

    [Command]
    public void CmdDownloadGameModePlayerSettings (int id) {
        ServerLogic.DownloadGameModePlayerSettings (this, id);
    }

    [TargetRpc]
    public void TargetDownloadGameModePlayerSettings (NetworkConnection target, int [] statuses) {
        GameModePlayerSettingsMenu.LoadGameModePlayerSettingsMenu (statuses);
    }

    [Command]
    public void CmdDownloadProfileData () {
        ServerLogic.DownloadProfileData (this);
    }

    [TargetRpc]
    public void TargetDownloadProfileDataToMenu (NetworkConnection target,
        string userName, int avatar, int thisModeWon, int thisModeLost, int thisModeDrawn, int thisModeUnfinished, int totalWon, int totalLost, int totalDrawn, int totalUnfinished,
        int level, int currentExperience, int neededExperience) {
        ProfileMenu.ShowProfileMenu (userName, avatar, thisModeWon, thisModeLost, thisModeDrawn, thisModeUnfinished, totalWon, totalLost, totalDrawn, totalUnfinished,
            level, currentExperience, neededExperience);
    }

    [Command]
    public void CmdSaveProfileSettings (string userName, int avatar) {
        ServerLogic.SaveProfileSettings (this, userName, avatar);
    }

    [TargetRpc]
    public void TargetRefreshProfileSettings (NetworkConnection target, string userName, int avatar) {
        ClientLogic.RefreshProfileSettings (this, userName, avatar);
    }

    [Command]
    public void CmdCreateNewGameMode (string name) {
        ServerLogic.CreateNewGameMode (this, name);
    }

    [Command]
    public void CmdDeleteGameMode (int id) {
        ServerLogic.DeleteGameMode (this, id);
    }

    [Command]
    public void CmdDownloadGameModeToEditor (int gameModeId) {
        ServerLogic.DownloadGameModeToEditor (this, gameModeId);
    }

    [TargetRpc]
    public void TargetDownloadGameModeToEditor (NetworkConnection target, bool editMode, int gameModeId, string gameModeName, string [] boardNames, int [] boardIds, bool [] boardIsLegal) {
        GameModeEditor.LoadDataToEditor (editMode, gameModeId, gameModeName, boardNames, boardIds, boardIsLegal);
    }

    [Command]
    public void CmdCreateNewBoard (int gameModeId, string name) {
        ServerLogic.CreateNewBoard (this, gameModeId, name);
    }

    [Command]
    public void CmdDeleteBoard (int gameModeId, int boardId) {
        ServerLogic.DeleteBoard (this, gameModeId, boardId);
    }

    [Command]
    public void CmdDownloadBoard (int boardId) {
        ServerLogic.DownloadBoardToEditor (this, boardId);
    }

    [TargetRpc]
    public void TargetDownloadBoardToEditor (NetworkConnection target, bool isClientOwner, int boardId, string boardName, string [] board, int [] matchTypes) {
        BoardEditorMenu.LoadDataToEditor (boardId, isClientOwner, boardName, board, matchTypes);
    }

    [Command]
    public void CmdDownloadCustomGameEditorData () {
        TargetLoadCustomGameEditorData (this.connectionToClient, ServerData.GetAllGameModeMatchTypes (GameMode));
    }

    [TargetRpc]
    public void TargetLoadCustomGameEditorData (NetworkConnection target, int [] gameModeMatchTypes) {
        CustomGameEditor.LoadData (gameModeMatchTypes);
    }

    [Command]
    public void CmdSaveBoardMatchTypes (int boardId, int [] matchTypes) {
        ServerLogic.SaveBoardMatchTypes (this, boardId, matchTypes);
    }

    [Command]
    public void CmdSaveBoard (int boardId, string [] board) {
        ServerLogic.SaveBoard (this, boardId, board);
    }

    [Command]
    public void CmdSaveCardPool (int gameModeId, string [] cardpool) {
        ServerLogic.SaveCardPool (this, gameModeId, cardpool);
    }

    [Command]
    public void CmdDownloadListOfCustomGames () {
        ServerLogic.DownloadListOfCustomGames (this);
    }

    [TargetRpc]
    public void TargetDownloadListOfCustomGames (NetworkConnection target, 
        int [] ids, string [] names, int [] matchTypes, int [] filledSlots) {
        CustomGameLobby.LoadData (ids, names, matchTypes, filledSlots);
    }

    [Command]
    public void CmdCreateCustomGame (string gameName, int matchType) {
        ServerLogic.CreateCustomGame (this, gameName, matchType);
    }

    [Command]
    public void CmdLeaveCustomGame () {
        CustomGameManager.LeaveCustomGame (this);
    }

    [Command]
    public void CmdJoinCustomGameRoom (int id) {
        ServerLogic.JoinCustomGameRoom (this, id);
    }

    [Command]
    public void CmdCustomGameMoveToDifferentSlot (int newSlotNumber) {
        CustomGameManager.ChangeSlot (this, newSlotNumber);
    }

    [Command]
    public void CmdCustomGameChangeTeam (int slotNumber) {
        CustomGameManager.ChangeTeam (this, slotNumber);
    }

    [Command]
    public void CmdCustomGameRoomStartMatch () {
        CustomGameManager.StartCustomGame (this);
    }


    [TargetRpc]
    public void TargetDownloadCustomGameRoom (NetworkConnection target, 
       bool host, string roomName, int matchType, int [] avatars, string [] names, bool [] AIs, int [] teams) {
        ClientLogic.LoadCustomGameRoom (host, roomName, matchType, avatars, names, AIs, teams);
    }

    [Command]
    public void CmdCustomGameAddAI (int slotNumber) {
        CustomGameManager.AddAI (this, slotNumber);
    }

    [Command]
    public void CmdCustomGameKickPlayer (int slotNumber) {
        CustomGameManager.KickPlayer (this, slotNumber);
    }

    [Command]
    public void CmdChatSendMessage (string message) {
        RpcChatSendMessage (UserName, message);
    }

    [ClientRpc]
    public void RpcChatSendMessage (string userName, string message) {
        ChatUI.RecieveMessage (userName, message);
    }

    /*
    [TargetRpc]
    public void TargetAccountDoesntExist (NetworkConnection target) {
        Debug.Log ("Account doesn't exists");
    }

    [TargetRpc]
    public void TargetPasswordIncorrect (NetworkConnection target) {
        Debug.Log ("PasswordIncorrect");
    }

    [TargetRpc]
    public void TargetLoggedSuccesfully (NetworkConnection target) {
        Debug.Log ("LoggedSuccesfully");
    }

    [TargetRpc]
    public void TargetUserExists (NetworkConnection target) {
        Debug.Log ("User already exists");
    }

    [TargetRpc]
    public void TargetNullUsername (NetworkConnection target) {
        Debug.Log ("Username name can't be null");
    }

    [TargetRpc]
    public void TargetPasswordLegnth (NetworkConnection target) {
        Debug.Log ("Password has to have at least 8 characters");
    }

    [TargetRpc]
    public void TargetAccountCreated (NetworkConnection target) {
        Debug.Log ("Account created");
    }*/
}
