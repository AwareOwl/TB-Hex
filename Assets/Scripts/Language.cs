using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Language {

    static string SelectedLanguageKey = "SelectedLanguage";

    static string UIKey = "UI";
    static string UITooltipKey = "UITooltip";
    static string TokenNameKey = "TokenName";
    static string TokenDescriptionKey = "TokenDescription";
    static string AbilityNameKey = "AbilityName";
    static string AbilityDescriptionKey = "AbilityDescription";


    public const int English = 0;
    public const int Polish = 1;

    static public string [] FileName = new string [2] { "ENG", "PL" };

    static public string [] UI;
    static public string [] UITooltip;
    static public string [] AbilityName;
    static public string [] AbilityDescription;
    static public string [] TokenName;
    static public string [] TokenDescription;

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

    static public string GameModeEditorDescription;

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
    static public string BoardEditorDescription;
    static public string EnterNewBoardName;
    static public string SelectAvailableMatchTypes;

    static public string OpenCardPoolEditor;
    static public string CardPoolEditorDescription;

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

    static public string Profile;
    static public string ThisGameVersion;
    static public string AllGameVersions;
    static public string WonGames;
    static public string LostGames;
    static public string DrawnGames;
    static public string UnfinishedGames;

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

    static public string WaitingInQueue;
    static public string Cancel;

    static public string AIOpponent;

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


        path = "Languages/" + FileName [language] + UITooltipKey;
        asset = Resources.Load (path) as TextAsset;
        allLines = asset.text;
        lines = allLines.Split (new string [2] { System.Environment.NewLine + "[", "[" }, System.StringSplitOptions.RemoveEmptyEntries);
        UITooltip = new string [lines.Length];
        for (int x = 0; x < lines.Length; x++) {
            int index = lines [x].IndexOf (']');
            if (lines [x].Length > index + 2) {
                UITooltip [x] = lines [x].Substring (index + 2);
            } else {
                UITooltip [x] = "";
            }
        }

        CreateLocalNetworkTooltip = UITooltip [0];
        JoinLocalNetworkTooltip = UITooltip [1];

        LoadNamesAndDescriptions (language);
    }

    static public void LoadNamesAndDescriptions (int language) {
        string path;
        TextAsset asset;
        string allLines;
        string [] lines;

        path = "Languages/" + FileName [language] + TokenNameKey;
        asset = Resources.Load (path) as TextAsset;
        allLines = asset.text;
        lines = allLines.Split (new string [2] { System.Environment.NewLine + "[", "[" }, System.StringSplitOptions.RemoveEmptyEntries);
        TokenName = new string [lines.Length];
        for (int x = 0; x < lines.Length; x++) {
            int index = lines [x].IndexOf (']');
            if (lines [x].Length > index + 2) {
                TokenName [x] = lines [x].Substring (index + 2);
            } else {
                TokenName [x] = "";
            }
        }

        path = "Languages/" + FileName [language] + TokenDescriptionKey;
        asset = Resources.Load (path) as TextAsset;
        allLines = asset.text;
        lines = allLines.Split (new string [2] { System.Environment.NewLine + "[", "[" }, System.StringSplitOptions.RemoveEmptyEntries);
        TokenDescription = new string [lines.Length];
        for (int x = 0; x < lines.Length; x++) {
            int index = lines [x].IndexOf (']');
            if (lines [x].Length > index + 2) {
                TokenDescription [x] = lines [x].Substring (index + 2);
            } else {
                TokenDescription [x] = "";
            }
        }

        path = "Languages/" + FileName [language] + AbilityNameKey;
        asset = Resources.Load (path) as TextAsset;
        allLines = asset.text;
        lines = allLines.Split (new string [2] { System.Environment.NewLine + "[", "[" }, System.StringSplitOptions.RemoveEmptyEntries);
        AbilityName = new string [lines.Length];
        for (int x = 0; x < lines.Length; x++) {
            int index = lines [x].IndexOf (']');
            if (lines [x].Length > index + 2) {
                AbilityName [x] = lines [x].Substring (index + 2);
            } else {
                AbilityName [x] = "";
            }
        }

        path = "Languages/" + FileName [language] + AbilityDescriptionKey;
        asset = Resources.Load (path) as TextAsset;
        allLines = asset.text;
        lines = allLines.Split (new string [2] { System.Environment.NewLine + "[", "[" }, System.StringSplitOptions.RemoveEmptyEntries);
        AbilityDescription = new string [lines.Length];
        for (int x = 0; x < lines.Length; x++) {
            int index = lines [x].IndexOf (']');
            if (lines [x].Length > index + 2) {
                AbilityDescription [x] = lines [x].Substring (index + 2);
            } else {
                AbilityDescription [x] = "";
            }
        }
    }

    static public string GetAbilityDescription (int abilityType) {
        string s = AbilityDescription [abilityType];
        for (int x = 0; x < AbilityClass.AbilityValue [abilityType].Count; x++) {
            s = s.Replace ("%d" + (x + 1).ToString (), AbilityClass.AbilityValue [abilityType] [x].ToString());
        }
        return s;
    }

    static public string GetMatchResult (string winner, int winCondition, int limit) {
        string s = "";
        switch (winCondition) {
            case 1:
                s += UI [17].Replace ("%d", limit.ToString());
                break;
            case 2:
                s += UI [18].Replace ("%d", limit.ToString ());
                break;
            case 3:
                s += UI [19];
                break;
            case 4:
                s += UI [63];
                break;
        }
        if (winner != null && winner != "") {
            s += " " + UI [16].Replace ("%s", winner);
        } else {
            s += " " + UI [20];
        }
        return s;
    }

    static public string GetInvalidGameVersionMessage (string serverVersion) {
        return UI [21].Replace ("%s", serverVersion);
    }

    static public string GetInvalidSetMessage () {
        return CantStartTheGame + " " + YourSetNeedsDCardsInEachStack;
    }

    static public string GetInvalidSavedSetMessage () {
        return SetSavedButInvalid + " " + YourSetNeedsDCardsInEachStack;
    }
}
