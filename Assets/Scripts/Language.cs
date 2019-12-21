using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Language {

    static string SelectedLanguageKey = "SelectedLanguage";

    static string UIKey = "UI";
    static string UITooltipKey = "UITooltip";
    static string AvatarNameKey = "AvatarName";
    static string TokenNameKey = "TokenName";
    static string TokenDescriptionKey = "TokenDescription";
    static string AbilityNameKey = "AbilityName";
    static string AbilityDescriptionKey = "AbilityDescription";
    static string BossNameKey = "BossName";
    static string BossDescriptionKey = "BossDescription";
    static string StatusDescriptionKey = "StatusDescription";
    static string TutorialTooltipKey = "TutorialTooltip";


    public const int English = 0;
    public const int Polish = 1;

    static public string [] FileName = new string [2] { "ENG", "PL" };

    static public string [] UI;
    static public string [] AvatarName;
    static public string [] AbilityName;
    static public string [] AbilityDescription;
    static public string [] TokenName;
    static public string [] TokenDescription;
    static public string [] BossName;
    static public string [] BossDescription;
    static public string [] StatusDescription;

    static public string [] TutorialTooltip = new string [15];

    static public string CreateLocalNetwork;
    static public string CreateLocalNetworkTooltip;
    static public string JoinLocalNetwork;
    static public string JoinLocalNetworkTooltip;

    static public string AccountName;
    static public string UserName;
    static public string Password;
    static public string ConfirmPassword;
    static public string Email;

    static public string LogIn;
    static public string Register;

    static public string AvailableCardPool;
    static public string YourCardSet;
    static public string SetSavedButInvalid;
    static public string SaveSet;
    static public string GoBackToMenu;
    static public string GenerateRandomSet;
    static public string SetEditorDescription;
    static public string EditSet;
    static public string SelectCardSetTooltip;
    static public string DeleteSet;
    static public string CreateNewSet;
    static public string ChangeSetNameOrIcon;
    static public string SaveSettings;


    static public string EnterNewSetName;
    static public string ChoseNewSetIcon;

    static public string CantStartTheGame;

    static public string YourSetNeedsDCardsInEachStack;

    static public string ClickToLearnMore;

    static public string ExitApp;

    static public string MainMenuButton;
    static public string ExitGameButton;
    static public string Settings;
    static public string ContinueButton;

    // main menu
    static public string PlayAgainstAI;
    static public string QuickOnlineGame;
    static public string BeginGameAgainstAI;

    static public string SelectCardSet;
    static public string EmptySlot;
    static public string StartingSet;
    static public string NewSet;
    static public string ApplySettings;

    static public string ChangeGameVersion;
    static public string OfficialGameVersions;
    static public string PublicGameVersions;
    static public string YourGameVersions;
    static public string ListOfGameVersions;
    static public string GameVersion;
    static public string EditThisGameVersion;
    static public string DeleteThisGameVersion;
    static public string AddNewGameVersion;
    static public string NewGameVersion;
    static public string GoBackToGameVersionSelection;
    static public string EnterNewGameVersionName;

    static public string WorksWellWith;
    static public string IsGoodAgains;
    static public string IsWeakAgainst;

    static public string ViewGameModeEditor;
    static public string GameModeEditorDescription;
    static public string GameModeEditorReadOnlyDescription;
    static public string PlayersSettings;
    static public string PlayerStatuses;

    static public string Player;

    static public string AvailableBoards;
    static public string Tools;
    static public string EditThisBoard;
    static public string AddNewBoard;
    static public string DeleteThisBoard;
    static public string NewBoard;
    static public string ChangeGameVersionName;
    static public string EditAvailableCardPool;
    static public string SaveThisBoard;
    static public string ChangeBoardName;
    static public string GoBackToGameVersionEditor;
    static public string ViewBoardEditor;
    static public string BoardEditorDescription;
    static public string BoardEditorReadOnlyDescription;
    static public string EnterNewBoardName;
    static public string SelectAvailableMatchTypes;

    static public string OpenCardPoolEditor;
    static public string ViewCardPoolEditor;
    static public string CardPoolEditorDescription;
    static public string CardPoolEditorReadOnlyDescription;

    static public string CustomGame;
    static public string AvailableGames;
    static public string CreateNewGame;
    static public string JoinToSelectedGame;
    static public string EnterGameName;
    static public string ReadyToStart;
    static public string KickPlayer;

    static public string EndMatchAfterReachingScoreLimit;
    static public string EndMatchAfterReachingTurnLimit;
    static public string GameModeSettings;
    static public string AllowToRotateAbilityAreaDuringMatch;
    static public string NumberOfStacks;
    static public string MinimumNumberOfCardsInStack;
    static public string UsedCardsArePlacedOnBottomOfStack;
    static public string EveryCardCanBeAddedToSetAnyNumberOfTimes;

    static public string Team;
    
    static public string BossUnlockTooltip;
    static public string PuzzleUnlockTooltip;

    static public string NumberOfTurnsLeftToTheEndOfGame;
    static public string ShowMenu;

    static public string Profile;
    static public string ThisGameVersion;
    static public string AllGameVersions;
    static public string WonGames;
    static public string LostGames;
    static public string DrawnGames;
    static public string UnfinishedGames;
    static public string Level;

    static public string EnterNewUserName;
    static public string ChoseNewAvatar;

    static public string FreeForAll;
    static public string ShowChat;
    static public string HideChat;
    static public string YouHaveBeenKicked;
    static public string EnterText;
    static public string Send;
    static public string BeforeApplyingSelectAnyOption;
    static public string Refresh;

    static public string ChangeSettings;
    static public string Apply;
    static public string Close;

    static public string NextPage;
    static public string PrevPage;

    static public string EditGameModeSettings;
    static public string ViewGameModeSettings;

    static public string AvailableMatchTypes;

    static public string Tutorial;
    static public string Bosses;

    static public string Puzzles;
    static public string PuzzleAbout;
    static public string Unfinished;
    static public string Finished;
    static public string ListOfPuzzles;
    static public string MoreToUnlock;

    static public string Missions;
    static public string Mission;

    static public string ListOfBosses;
    static public string BossMenuAbout;
    static public string OpponentTrait;

    static public string ShowTokenValueFilters;
    static public string ShowTokenTypeFilters;
    static public string ShowAbilityTypeFilters;
    static public string ShowAbilityAreaFilters;

    static public string WaitingInQueue;
    static public string Cancel;

    static public string AIOpponent;

    static public string EnterCode;

    static public string Library;
    static public string LibraryMenuAbout;

    static public string BeginPuzzle;
    static public string GoToTheSetEditor;
    static public string BeginBossFight;
    static public string BeginMission;
    static public string RestartPuzzle;
    static public string RestartMission;
    static public string RestartBoss;

    static public string LevelUpAndFinishPuzzlesToUnlockMoreCards;

    static public string ShowUnlockedContent;
    static public string UnlockedAvatar;
    static public string UnlockedAvatars;
    static public string NewAvatarUnlocked;
    static public string NewAvatarsUnlocked;
    static public string UnlockedAbility;
    static public string UnlockedAbilities;
    static public string NewAbilityUnlocked;
    static public string NewAbilitiesUnlocked;
    static public string UnlockedToken;
    static public string UnlockedTokens;
    static public string NewTokenUnlocked;
    static public string NewTokensUnlocked;

    static public string File;
    static public string TileFilling;
    static public string TokenOwner;
    static public string TokenValue;
    static public string TokenType;
    static public string AbilityType;
    static public string AbilityArea;

    static public string Options;


    static public string ShowSettingsMenu;
    static public string MusicVolume;
    static public string SFXVolume;
    static public string AnimationDuration;
    static public string TimeBetweenTurns;
    static public string PlayedCardDisplayTime;

    static public string YouCanUnlockMoreCardsByFinishingPuzzlesAndIncreasingYourLevel;

    static public int PasswordIsIncorrectKey;
    static public int AccountDoesntExistKey;
    static public int PleaseEnterAccountNameKey;
    static public int PasswordHasToHaveAtLeast8CharactersKey;
    static public int AccountNameCantBeNullKey;
    static public int AccountWithThisNameAlreadyExistsKey;
    static public int AccountCreatedKey;
    static public int InvalidGameVersion;
    static public int SetSavedKey;
    static public int NoSetSelectedKey;
    static public int BoardHasBeenSavedKey;
    static public int BoardHasBeenSavedButIllegalKey;
    static public int CardPoolSavedKey;
    static public int CardPoolSavedButIllegalKey;
    static public int GameVersionDoesNotMeetRequirementsKey;
    static public int SelectedMatchVersionIsNotAvailableKey;
    static public int FailedToConnectToTheGameKey;
    static public int CodeHasExpiredKey;
    static public int InvalidCodeKey;
    static public int YouNeedToSelectSomethingFirstKey;
    static public int SettingsHaveBeenSavedKey;

    static public int YouHaveBeenKickedFromTheRoomKey;

    static public void SetLanguage (int languageKey) {
        PlayerPrefs.SetInt (SelectedLanguageKey, languageKey);
        LoadLanguage ();
        SceneManager.LoadScene ("OfflineScene", LoadSceneMode.Single);
    }

    static Language () {
        LoadLanguage ();
    }

    static public void LoadLanguage () {
        LoadLanguage (PlayerPrefs.GetInt (SelectedLanguageKey));
    }

    static public void LoadLanguage (int language) {
        string path = "Languages/" + FileName [language] + UIKey;
        //string path = "Languages/ENGUI";
        TextAsset asset = Resources.Load (path) as TextAsset;
        string allLines = asset.text;
        string [] lines = allLines.Split (new string [2] { System.Environment.NewLine + "[", "[" }, System.StringSplitOptions.RemoveEmptyEntries);
        UI = new string [lines.Length];
        for (int x = 0; x < lines.Length; x++) {
            int index = lines [x].IndexOf (']');
            if (lines [x].Length > index + 2) {
                UI [x] = lines [x].Substring (index + 2);
            } else {
                UI [x] = "";
            }
        }
        CreateLocalNetwork = UI [0];
        JoinLocalNetwork = UI [1];
        AccountName = UI [2];
        Password = UI [3];
        ConfirmPassword = UI [4];
        Email = UI [5];
        LogIn = UI [6];
        Register = UI [7];
        PasswordIsIncorrectKey = 8;
        AccountDoesntExistKey = 9;
        PleaseEnterAccountNameKey = 10;
        PasswordHasToHaveAtLeast8CharactersKey = 11;
        AccountNameCantBeNullKey = 12;
        AccountWithThisNameAlreadyExistsKey = 13;
        AccountCreatedKey = 14;
        UserName = UI [15];
        InvalidGameVersion = 21;
        AvailableCardPool = UI [22];
        YourCardSet = UI [23];
        PlayAgainstAI = UI [25];
        EditSet = UI [26];
        SetSavedKey = 27;
        SetSavedButInvalid = UI [28];
        CantStartTheGame = UI [29];
        YourSetNeedsDCardsInEachStack = UI [30];
        ExitApp = UI [31];
        SaveSet = UI [32];
        GoBackToMenu = UI [33];
        GenerateRandomSet = UI [34];
        SetEditorDescription = UI [35];
        ClickToLearnMore = UI [36];
        SelectCardSet = UI [37];
        EmptySlot = UI [38];
        AIOpponent = UI [39];
        StartingSet = UI [40];
        NewSet = UI [41];
        NoSetSelectedKey = 42;
        ChangeSetNameOrIcon = UI [43];
        ApplySettings = UI [44];
        EnterNewSetName = UI [45];
        ChoseNewSetIcon = UI [46];
        MainMenuButton = UI [47];
        ExitGameButton = UI [48];
        ContinueButton = UI [49];
        Apply = UI [50];
        SelectCardSetTooltip = UI [51];
        DeleteSet = UI [52];
        CreateNewSet = UI [53];
        Close = UI [54];
        ChangeGameVersion = UI [55];
        OfficialGameVersions = UI [56];
        PublicGameVersions = UI [57];
        YourGameVersions = UI [58];
        ListOfGameVersions = UI [59];
        QuickOnlineGame = UI [60];
        GameVersion = UI [61];
        BeginGameAgainstAI = UI [62];
        EditThisGameVersion = UI [64];
        DeleteThisGameVersion = UI [65];
        AddNewGameVersion = UI [66];
        NewGameVersion = UI [67];
        AvailableBoards = UI [68];
        Tools = UI [69];
        EditThisBoard = UI [70];
        AddNewBoard = UI [71];
        DeleteThisBoard = UI [72];
        NewBoard = UI [73];
        GameModeEditorDescription = UI [74];
        ChangeGameVersionName = UI [75];
        EditAvailableCardPool = UI [76];
        BoardHasBeenSavedKey = 77;
        BoardHasBeenSavedButIllegalKey = 78;
        SaveThisBoard = UI [79];
        ChangeBoardName = UI [80];
        GoBackToGameVersionEditor = UI [81];
        BoardEditorDescription = UI [82];
        GoBackToGameVersionSelection = UI [83];
        OpenCardPoolEditor = UI [84];
        CardPoolSavedKey = 85;
        CardPoolSavedButIllegalKey = 86;
        EnterNewGameVersionName = UI [87];
        EnterNewBoardName = UI [88];
        GameVersionDoesNotMeetRequirementsKey = 89;
        CardPoolEditorDescription = UI [90];
        WaitingInQueue = UI [91];
        Cancel = UI [92];
        CustomGame = UI [93];
        FreeForAll = UI [94];
        ShowChat = UI [95];
        AvailableGames = UI [96];
        CreateNewGame = UI [97];
        JoinToSelectedGame = UI [98];
        EnterGameName = UI [99];
        ReadyToStart = UI [100];
        KickPlayer = UI [101];
        SelectAvailableMatchTypes = UI [102];
        ChangeSettings = UI [103];
        SelectedMatchVersionIsNotAvailableKey = 104;
        YouHaveBeenKicked = UI [105];
        EnterText = UI [106];
        Send = UI [107];
        BeforeApplyingSelectAnyOption = UI [108];
        FailedToConnectToTheGameKey = 109;
        HideChat = UI [110];
        Refresh = UI [112];
        EndMatchAfterReachingScoreLimit = UI [113];
        EndMatchAfterReachingTurnLimit = UI [114];
        GameModeSettings = UI [115];
        AllowToRotateAbilityAreaDuringMatch = UI [116];
        NumberOfStacks = UI [117];
        MinimumNumberOfCardsInStack = UI [118];
        Profile = UI [119];
        ThisGameVersion = UI [120];
        AllGameVersions = UI [121];
        WonGames = UI [122];
        LostGames = UI [123];
        DrawnGames = UI [124];
        UnfinishedGames = UI [125];
        EnterNewUserName = UI [126];
        ChoseNewAvatar = UI [127];
        GameModeEditorReadOnlyDescription = UI [128];
        CardPoolEditorReadOnlyDescription = UI [129];
        BoardEditorReadOnlyDescription = UI [130];
        ViewBoardEditor = UI [131];
        ViewCardPoolEditor = UI [132];
        ViewGameModeEditor = UI [133];
        NextPage = UI [134];
        PrevPage = UI [135];
        EditGameModeSettings = UI [136];
        ViewGameModeSettings = UI [137];
        AvailableMatchTypes = UI [138];
        ShowTokenValueFilters = UI [139];
        ShowTokenTypeFilters = UI [140];
        ShowAbilityTypeFilters = UI [141];
        ShowAbilityAreaFilters = UI [142];
        Puzzles = UI [143];
        PuzzleAbout = UI [144];
        Unfinished = UI [145];
        Finished = UI [146];
        ListOfPuzzles = UI [147];
        UsedCardsArePlacedOnBottomOfStack = UI [148];
        NumberOfTurnsLeftToTheEndOfGame = UI [149];
        ShowMenu = UI [150];
        Level = UI [151];
        MoreToUnlock = UI [152];
        CreateLocalNetworkTooltip = UI [153];
        JoinLocalNetworkTooltip = UI [154];
        UnlockedAvatar = UI [155];
        UnlockedAvatars = UI [156];
        NewAvatarUnlocked = UI [157];
        NewAvatarsUnlocked = UI [158];
        UnlockedAbilities = UI [159];
        UnlockedTokens = UI [160];
        UnlockedAbility = UI [161];
        UnlockedToken = UI [162];
        ShowUnlockedContent = UI [163];
        EnterCode = UI [164];
        Tutorial = UI [166];
        Bosses = UI [167];
        NewAbilityUnlocked = UI [168];
        NewAbilitiesUnlocked = UI [169];
        NewTokenUnlocked = UI [170];
        CodeHasExpiredKey = 171;
        InvalidCodeKey = 172;
        ListOfBosses = UI [173];
        BossMenuAbout = UI [174];
        OpponentTrait = UI [175];
        Missions = UI [176];
        NewTokensUnlocked = UI [177];
        BeginPuzzle = UI [178];
        GoToTheSetEditor = UI [179];
        BeginBossFight = UI [180];
        BeginMission = UI [181];
        YouNeedToSelectSomethingFirstKey = 182;
        LevelUpAndFinishPuzzlesToUnlockMoreCards = UI [183];
        Mission = UI [184];
        RestartPuzzle = UI [185];
        RestartMission = UI [186];
        RestartBoss = UI [187];
        File = UI [188];
        TileFilling = UI [189];
        TokenOwner = UI [190];
        TokenValue = UI [191];
        TokenType = UI [192];
        AbilityType = UI [193];
        AbilityArea = UI [194];
        YouCanUnlockMoreCardsByFinishingPuzzlesAndIncreasingYourLevel = UI [195];
        Library = UI [196];
        WorksWellWith = UI [197];
        IsGoodAgains = UI [198];
        IsWeakAgainst = UI [199];
        LibraryMenuAbout = UI [200];
        PlayersSettings = UI [201];
        Player = UI [202];
        PlayerStatuses = UI [203];
        SettingsHaveBeenSavedKey = 204;
        EveryCardCanBeAddedToSetAnyNumberOfTimes = UI [205];
        Team = UI [210];
        YouHaveBeenKickedFromTheRoomKey = 211;
        BossUnlockTooltip = UI [212];
        PuzzleUnlockTooltip = UI [213];
        Settings = UI [214];
        ShowSettingsMenu = UI [215];
        Options = UI [216];
        MusicVolume = UI [217];
        SFXVolume = UI [218];
        AnimationDuration = UI [219];
        TimeBetweenTurns = UI [220];
        PlayedCardDisplayTime = UI [221];

        LoadNamesAndDescriptions (language);
    }

     static public void LoadNamesAndDescriptions (int language) {
        string prefixPath = "Languages/" + FileName [language];

        AvatarName = LoadLanguageFile (prefixPath + AvatarNameKey);
        TokenName = LoadLanguageFile (prefixPath + TokenNameKey);
        TokenDescription = LoadLanguageFile (prefixPath + TokenDescriptionKey);
        AbilityName = LoadLanguageFile (prefixPath + AbilityNameKey);
        AbilityDescription = LoadLanguageFile (prefixPath + AbilityDescriptionKey);
        BossName = LoadLanguageFile (prefixPath + BossNameKey);
        BossDescription = LoadLanguageFile (prefixPath + BossDescriptionKey);
        StatusDescription = LoadLanguageFile (prefixPath + StatusDescriptionKey);
        TutorialTooltip = LoadLanguageFile (prefixPath + TutorialTooltipKey);

    }

    static public string [] LoadLanguageFile (string path) {
        TextAsset asset;
        string allLines;
        string [] lines;
        string [] output;
        
        asset = Resources.Load (path) as TextAsset;
        allLines = asset.text;
        lines = allLines.Split (new string [2] { System.Environment.NewLine + "[", "[" }, System.StringSplitOptions.RemoveEmptyEntries);
        output = new string [lines.Length];
        for (int x = 0; x < lines.Length; x++) {
            int index = lines [x].IndexOf (']');
            if (lines [x].Length > index + 2) {
                output [x] = lines [x].Substring (index + 2);
            } else {
                output [x] = "";
            }
        }
        return output; 
    }

    static public string GetSetEditorDescription () {
        string s = SetEditorDescription;
        s = s.Replace ("%d1", SetEditor.minimumNumberOfCardsOnStack.ToString());
        return s;
    }

    static public string GetYourSetNeedsAtLeastDCardsInEachStack () {
        return GetYourSetNeedsAtLeastDCardsInEachStack (SetEditor.minimumNumberOfCardsOnStack);
    }

    static public string GetYourSetNeedsAtLeastDCardsInEachStack (int number) {
        string s = YourSetNeedsDCardsInEachStack;
        s = s.Replace ("%d1", number.ToString ());
        return s;
    }

    static public string GetTokenDescription (int tokenType) {
        string s = TokenDescription [tokenType];
        if (InputController.debuggingEnabled) {
            s = "[" + tokenType + "] " + s;
        }
        return s;
    }

    static public string GetAbilityDescription (int abilityType) {
        string s = AbilityDescription [abilityType];
        for (int x = 0; x < AbilityClass.AbilityValue [abilityType].Count; x++) {
            s = s.Replace ("%d" + (x + 1).ToString (), AbilityClass.AbilityValue [abilityType] [x].ToString());
        }
        if (InputController.debuggingEnabled) {
            s = "[" + abilityType + "] " + s;
        }
        return s;
    }

    static public string GetMatchResult (string [] winners, int winCondition, int limit) {
        string s = "";
        switch (winCondition) {
            case 1:
                s += UI [17].Replace ("%d", limit.ToString());
                break;
            case 2:
                s += UI [18];
                break;
            case 3:
                s += UI [19];
                break;
            case 4:
                s += UI [63];
                break;
            case 5:
                s += UI [206];
                break;
            case 6:
                s += UI [209];
                break;
        }
        if (winners.Length > 0) {
            if (winners.Length == 1) {
                s += " " + UI [16].Replace ("%s", winners [0]);
            } else {
                string winnersString = "";
                int count = winners.Length;
                for (int x = 0; x < count; x++) {
                    winnersString += winners [x];
                    if (x + 2 < count) {
                        winnersString += ", ";
                    } else if (x + 2 == count) {
                        winnersString += " " + UI [208] + " ";
                    }
                }
                s += " " + UI [207].Replace ("%s", winnersString);
            }
        } else {
            s += " " + UI [20];
        }
        return s;
    }

    static public string GetInvalidGameVersionMessage (string serverVersion) {
        return UI [21].Replace ("%s", serverVersion);
    }

    static public string GetInvalidSetMessage (int number) {
        return CantStartTheGame + " " + GetYourSetNeedsAtLeastDCardsInEachStack (number);
    }

    static public string GetInvalidSavedSetMessage () {
        return SetSavedButInvalid + " " + GetYourSetNeedsAtLeastDCardsInEachStack ();
    }
}
