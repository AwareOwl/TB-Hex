using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class ServerData : MonoBehaviour {

    static public string PasswordKey = "Password";

    static public string AccountNameKey = "AccountName";
    static public string UserNameKey = "UserName";
    static public string BoardNameKey = "BoardName";
    static public string GameModeNameKey = "GameModeName";
    static public string IsDeletedKey = "IsDeleted";
    static public string IsLegalKey = "IsLegal";
    static public string AvatarKey = "Avatar";

    static public string GamesWonKey = "GamesWon";
    static public string GamesLostKey = "GamesLost";
    static public string GamesDrawnKey = "GamesDrawn";
    static public string GamesUnfinishedKey = "GamesUnfinished";

    static public string HasScoreWinConditionKey = "HasScoreWinCondition";
    static public string HasTurnWinConditionKey = "HasTurnWinCondition";
    static public string ScoreWinConditionValueKey = "ScoreWinConditionValue";
    static public string TurnWinConditionValueKey = "TurnWinConditionValue";
    static public string IsAllowedToRotateCardsDuringMatchKey = "IsAllowedToRotateCardsDuringMatch";
    static public string NumberOfStacksKey = "NumberOfStacks";
    static public string MinimumNumberOfCardsInStackKey = "MinimumNumberOfCardsInStack";

    static public string IsCardPoolLegalKey = "IsCardPoolLegalKey";
    static public string OfficialKey = "Official";
    static public string GameModeOfficialKey = "GameModeOfficialKey";
    static public string UserSelectedGameModeKey = "UserSelectedGameMode";
    static public string SetNameKey = "SetName";
    static public string SetIconNumberKey = "SetIconNumber";
    static public string SelectedSetKey = "SelectedSet";

    static public string VersionKey = "Version";
    static public string InitVectorKey = "InitVector";
    static public string EncryptPasswordKey = "EncryptPassword";

    static string BoardProperty = "Board";
    static string GameModeProperty = "GameMode";
    static string CardSetProperty = "CardSet";

    static int DefaultGameMode = 2;

    static public string ServerPath () {
        string path = Application.persistentDataPath + "/";
        if (!Directory.Exists (path)) {
            Directory.CreateDirectory (path);
        }
        return path;
    }
    static public string BackUpPath () {
        string path = @"C:/TokenBattleHexBackUp/";
        if (!Directory.Exists (path)) {
            Directory.CreateDirectory (path);
        }
        return path;
    }

    static public string RatingPath () {
        string path = ServerPath () + "Rating/";
        if (!Directory.Exists (path)) {
            Directory.CreateDirectory (path);
        }
        return path;
    }

    static public void SaveBackUp () {
        int backUpCount = Directory.GetDirectories (BackUpPath ()).Length;
        DirectoryCopy (ServerPath (), BackUpPath () + "/" + backUpCount.ToString (), true);
    }

    static public void DeleteBackUps () {
        string path = BackUpPath ();
        Directory.Delete (path, true);
    }

    private static void DirectoryCopy (string sourceDirName, string destDirName, bool copySubDirs) {
        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo (sourceDirName);

        if (!dir.Exists) {
            throw new DirectoryNotFoundException (
                "Source directory does not exist or could not be found: "
                + sourceDirName);
        }

        DirectoryInfo [] dirs = dir.GetDirectories ();
        // If the destination directory doesn't exist, create it.
        if (!Directory.Exists (destDirName)) {
            Directory.CreateDirectory (destDirName);
        }

        // Get the files in the directory and copy them to the new location.
        FileInfo [] files = dir.GetFiles ();
        foreach (FileInfo file in files) {
            string temppath = Path.Combine (destDirName, file.Name);
            file.CopyTo (temppath, false);
        }

        // If copying subdirectories, copy them and their contents to new location.
        if (copySubDirs) {
            foreach (DirectoryInfo subdir in dirs) {
                string temppath = Path.Combine (destDirName, subdir.Name);
                DirectoryCopy (subdir.FullName, temppath, copySubDirs);
            }
        }
    }

    static public string SaveRatingPlayerWinRatio (string [] lines) {
        string path = RatingPath () + "PlayerWinRatio.txt";
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string SaveRatingCardNumberWinRatio (string [] lines) {
        string path = RatingPath () + "CardNumberWinRatio.txt";
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string SaveRatingCardPopularity (string [] lines) {
        string path = RatingPath () + "CardPopularity.txt";
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string SaveRatingWinnerScore (string [] lines) {
        string path = RatingPath () + "WinnerScore.txt";
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string SaveRatingLoserScore (string [] lines) {
        string path = RatingPath () + "LoserScore.txt";
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string SaveRatingAbilityOnStack (string [] lines) {
        string path = RatingPath () + "AbilityOnStack.txt";
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string RatingAbilityOnRowPath () {
        return RatingPath () + "AbilityOnRow.txt";
    }

    static public string SaveRatingAbilityOnRow (string [] lines) {
        string path = RatingAbilityOnRowPath ();
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string [] GetRatingAbilityOnRow () {
        string path = RatingAbilityOnRowPath ();
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            return lines;
        }
        return null;
    }

    static public string RatingTokenOnRowPath () {
        return RatingPath () + "TokenOnRow.txt";
    }

    static public string SaveRatingTokenOnRow (string [] lines) {
        string path = RatingTokenOnRowPath ();
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string [] GetRatingTokenOnRow () {
        string path = RatingTokenOnRowPath ();
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            return lines;
        }
        return null;
    }

    static public string RatingAbilityTokenOnRowPath () {
        return RatingPath () + "AbilityTokenOnRow.txt";
    }

    static public string SaveRatingAbilityTokenOnRow (string [] lines) {
        string path = RatingAbilityTokenOnRowPath ();
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string [] GetRatingAbilityTokenOnRow () {
        string path = RatingAbilityTokenOnRowPath ();
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            return lines;
        }
        return null;
    }

    static public string SaveRatingSurroundDanger (string [] lines) {
        string path = RatingPath () + "SurroundDanger.txt";
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string SaveRatingMultiTargetDanger (string [] lines) {
        string path = RatingPath () + "MultiTargetDanger.txt";
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string SaveRatingEdgeDanger (string [] lines) {
        string path = RatingPath () + "EdgeDanger.txt";
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string SaveRatingTurn (string [] lines) {
        string path = RatingPath () + "Turn.txt";
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string SaveRatingMapPlayer (string [] lines) {
        string path = RatingPath () + "MapPlayer.txt";
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string SaveRatingNumberOfCards (string [] lines) {
        string path = RatingPath () + "NumberOfCards.txt";
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string RatingAbilityAbilitySynergyPath () {
        return RatingPath () + "AbilityAbilitySynergy.txt";
    }

    static public string [] GetRatingAbilityAbilitySynergy () {
        string path = RatingAbilityAbilitySynergyPath ();
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            return lines;
        }
        return null;
    }

    static public string SaveRatingAbilityAbilitySynergy (string [] lines) {
        string path = RatingAbilityAbilitySynergyPath ();
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string RatingTokenAfterTokenPath () {
        return RatingPath () + "TokenAfterToken.txt";
    }

    static public string SaveRatingTokenAfterToken (string [] lines) {
        string path = RatingTokenAfterTokenPath ();
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string [] GetRatingTokenAfterToken () {
        string path = RatingTokenAfterTokenPath ();
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            return lines;
        }
        return null;
    }

    static public string RatingAbilityAfterAbilityPath () {
        return RatingPath () + "AbilityAfterAbility.txt";
    }

    static public string [] GetRatingAbilityAfterAbility () {
        string path = RatingAbilityAfterAbilityPath ();
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            return lines;
        }
        return null;
    }

    static public string SaveRatingAbilityAfterAbility (string [] lines) {
        string path = RatingAbilityAfterAbilityPath ();
        File.WriteAllLines (path, lines);
        return path;
    }


    static public string RatingAbilityAfterTokenPath () {
        return RatingPath () + "AbilityAfterToken.txt";
    }

    static public string [] GetRatingAbilityAfterToken () {
        string path = RatingAbilityAfterTokenPath ();
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            return lines;
        }
        return null;
    }

    static public string SaveRatingAbilityAfterToken (string [] lines) {
        string path = RatingAbilityAfterTokenPath ();
        File.WriteAllLines (path, lines);
        return path;
    }


    static public string RatingTokenAfterAbilityPath () {
        return RatingPath () + "TokenAfterAbility.txt";
    }

    static public string [] GetRatingTokenAfterAbility () {
        string path = RatingTokenAfterAbilityPath ();
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            return lines;
        }
        return null;
    }

    static public string SaveRatingTokenAfterAbility (string [] lines) {
        string path = RatingTokenAfterAbilityPath ();
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string ContentPath () {
        string path = ServerPath () + "Content/";
        if (!Directory.Exists (path)) {
            Directory.CreateDirectory (path);
        }
        return path;
    }

    static public string GameModeContentPath () {
        string path = ContentPath () + "GameMode/";
        if (!Directory.Exists (path)) {
            Directory.CreateDirectory (path);
        }
        return path;
    }

    static public string GameModeContentPath (int id) {
        string path = GameModeContentPath () + id.ToString () + "/";
        if (!Directory.Exists (path)) {
            Directory.CreateDirectory (path);
        }
        return path;
    }

    static public bool GetGameModeHasScoreWinCondition (int id) {
        string path = GameModeContentPath (id);
        string s = GetKeyData (KeyDataPath (path), HasScoreWinConditionKey);
        if (s != null && s != "") {
            return Convert.ToBoolean (s);
        }
        SetGameModeHasScoreWinCondition (id, true);
        return true;
    }

    static public bool SetGameModeHasScoreWinCondition (int id, bool value) {
        string path = GameModeContentPath (id);
        string s = SetKeyData (KeyDataPath (path), HasScoreWinConditionKey, value.ToString ());
        return value;
    }

    static public int GetGameModeScoreWinConditionValue (int id) {
        string path = GameModeContentPath (id);
        string s = GetKeyData (KeyDataPath (path), ScoreWinConditionValueKey);
        if (s != null && s != "") {
            return int.Parse (s);
        }
        SetGameModeScoreWinConditionValue (id, 500);
        return 500;
    }

    static public int SetGameModeScoreWinConditionValue (int id, int value) {
        string path = GameModeContentPath (id);
        string s = SetKeyData (KeyDataPath (path), ScoreWinConditionValueKey, value.ToString ());
        return value;
    }

    static public bool GetGameModeHasTurnWinCondition (int id) {
        string path = GameModeContentPath (id);
        string s = GetKeyData (KeyDataPath (path), HasTurnWinConditionKey);
        if (s != null && s != "") {
            return Convert.ToBoolean (s);
        }
        SetGameModeHasTurnWinCondition (id, true);
        return true;
    }

    static public bool SetGameModeHasTurnWinCondition (int id, bool value) {
        string path = GameModeContentPath (id);
        string s = SetKeyData (KeyDataPath (path), HasTurnWinConditionKey, value.ToString ());
        return value;
    }

    static public int GetGameModeTurnWinConditionValue (int id) {
        string path = GameModeContentPath (id);
        string s = GetKeyData (KeyDataPath (path), TurnWinConditionValueKey);
        if (s != null && s != "") {
            return int.Parse (s);
        }
        SetGameModeTurnWinConditionValue (id, 40);
        return 40;
    }

    static public int SetGameModeTurnWinConditionValue (int id, int value) {
        string path = GameModeContentPath (id);
        string s = SetKeyData (KeyDataPath (path), TurnWinConditionValueKey, value.ToString ());
        return value;
    }

    static public bool GetGameModeIsAllowedToRotateCardsDuringMatch (int id) {
        string path = GameModeContentPath (id);
        string s = GetKeyData (KeyDataPath (path), IsAllowedToRotateCardsDuringMatchKey);
        if (s != null && s != "") {
            return Convert.ToBoolean (s);
        }
        SetGameModeIsAllowedToRotateCardsDuringMatch (id, false);
        return false;
    }

    static public bool SetGameModeIsAllowedToRotateCardsDuringMatch (int id, bool value) {
        string path = GameModeContentPath (id);
        string s = SetKeyData (KeyDataPath (path), IsAllowedToRotateCardsDuringMatchKey, value.ToString ());
        return value;
    }

    static public int GetGameModeNumberOfStacks (int id) {
        string path = GameModeContentPath (id);
        string s = GetKeyData (KeyDataPath (path), NumberOfStacksKey);
        if (s != null && s != "") {
            return int.Parse (s);
        }
        SetGameModeNumberOfStacks (id, 4);
        return 4;
    }

    static public int SetGameModeNumberOfStacks (int id, int value) {
        string path = GameModeContentPath (id);
        string s = SetKeyData (KeyDataPath (path), NumberOfStacksKey, value.ToString ());
        return value;
    }

    static public int GetGameModeMinimumNumberOfCardsInStack (int id) {
        string path = GameModeContentPath (id);
        string s = GetKeyData (KeyDataPath (path), MinimumNumberOfCardsInStackKey);
        if (s != null && s != "") {
            return int.Parse (s);
        }
        SetGameModeMinimumNumberOfCardsInStack (id, 2);
        return 2;
    }

    static public int SetGameModeMinimumNumberOfCardsInStack (int id, int value) {
        string path = GameModeContentPath (id);
        string s = SetKeyData (KeyDataPath (path), MinimumNumberOfCardsInStackKey, value.ToString ());
        return value;
    }

    static public string GetNextIdPath (string path) {
        return path + "NextId.txt";
    }

    static void SetGameModeNextId (int id) {
        string path = GetNextIdPath (GameModeContentPath ());
        SetNextId (path, id);
    }

    static public int GetGameModeNextId () {
        string path = GetNextIdPath (GameModeContentPath ());
        return GetNextId (path);
    }

    static public int IncrementGameModeNextId () {
        string path = GetNextIdPath (GameModeContentPath ());
        return IncrementNextId (path);
    }

    static public bool CheckIfGameModeIsLegal (int id) {
        CardPoolClass cardPool = new CardPoolClass ();
        cardPool.LoadFromFile (id);
        if (!GetIsCardPoolLegal (id)) {
            SetIsGameModeLegal (id, false);
            return false;
        }
        int [] boards = GetAllLegalGameModeBoard (id);
        if (boards == null || boards.Length == 0) {
            SetIsGameModeLegal (id, false);
            return false;
        }
        SetIsGameModeLegal (id, true);
        return true;
    }

    static public bool GetIsGameModeLegal (int id) {
        string path = GameModeContentPath (id);
        string s = GetKeyData (KeyDataPath (path), IsLegalKey);
        if (s != null && s != "") {
            return Convert.ToBoolean (s);
        }
        return CheckIfGameModeIsLegal (id);
    }

    static public bool SetIsGameModeLegal (int id, bool legal) {
        string path = GameModeContentPath (id);
        SetKeyData (KeyDataPath (path), IsLegalKey, legal.ToString ());
        return legal;
    }

    static public bool GetIsBoardLegal (int id) {
        string path = BoardContentPath (id);
        string s = GetKeyData (KeyDataPath (path), IsLegalKey);
        if (s != null && s != "") {
            return Convert.ToBoolean (s);
        }
        return CheckIfBoardIsLegal (id);
    }

    static public bool CheckIfBoardIsLegal (int id) {
        BoardClass board = new BoardClass ();
        board.LoadFromFile (id);
        foreach (TileClass tile in board.tileList) {
            if (tile.IsEmptyTile ()) {
                SetIsBoardLegal (id, true);
                return true;
            }
        }
        SetIsBoardLegal (id, false);
        return false;
    }

    static public bool SetIsBoardLegal (int id, bool legal) {
        string path = BoardContentPath (id);
        SetKeyData (KeyDataPath (path), IsLegalKey, legal.ToString ());
        return legal;
    }

    static public bool GetIsCardPoolLegal (int id) {
        string path = GameModeContentPath (id);
        string s = GetKeyData (KeyDataPath (path), IsCardPoolLegalKey);
        if (s != null && s != "") {
            return Convert.ToBoolean (s);
        }
        return CheckIfCardPoolIsLegal (id);
    }

    static public bool CheckIfCardPoolIsLegal (int id) {
        CardPoolClass cardPool = new CardPoolClass ();
        cardPool.LoadFromFile (id);
        if (cardPool.Card.Count >= 8) {
            SetIsCardPoolLegal (id, true);
            return true;
        }
        SetIsCardPoolLegal (id, false);
        return false;
    }

    static public bool SetIsCardPoolLegal (int id, bool legal) {
        string path = GameModeContentPath (id);
        SetKeyData (KeyDataPath (path), IsCardPoolLegalKey, legal.ToString ());
        return legal;
    }


    static public int GetNextId (string path) {
        if (!File.Exists (path)) {
            SetNextId (path, 1);
        }
        string Line = File.ReadAllText (path);
        int nextId = int.Parse (Line);
        string prePath = path.Remove (path.LastIndexOf ('/') + 1);
        while (Directory.Exists (prePath + nextId.ToString ())) {
            nextId++;
            SetNextId (path, nextId);
        }
        return nextId;
    }

    static public int IncrementNextId (string path) {
        int CurrentId = GetNextId (path);
        SetNextId (path, CurrentId + 1);
        return CurrentId;
    }

    static void SetNextId (string path, int id) {
        File.WriteAllText (path, id.ToString ());
    }

    static public int CreateNewGameMode (string userName) {
        int id = IncrementGameModeNextId ();
        SetGameModeName (id, "New game mode");
        AddGameModeOwner (id, userName);
        return id;
    }

    static public string GameModeOwnerPath (int id) {
        return GameModeContentPath (id) + "Owner.txt";
    }

    static public string [] GetGameModeOwners (int id) {
        string path = GameModeOwnerPath (id);
        string [] lines;
        if (File.Exists (path)) {
            lines = File.ReadAllLines (path);
        } else {
            lines = new string [0];
        }
        return lines;
    }

    static public string GetBoardName (int boardId) {
        string path = BoardContentPath (boardId);
        string s = GetKeyData (KeyDataPath (path), BoardNameKey);
        return s;
    }

    static public string SetBoardName (int boardId, string name) {
        string path = BoardContentPath (boardId);
        SetKeyData (KeyDataPath (path), BoardNameKey, name);
        return name;
    }

    static public string GetGameModeName (int gameModeId) {
        string path = GameModeContentPath (gameModeId);
        string s = GetKeyData (KeyDataPath (path), GameModeNameKey);
        return s;
    }

    static public string SetGameModeName (int gameModeId, string name) {
        string path = GameModeContentPath (gameModeId);
        SetKeyData (KeyDataPath (path), GameModeNameKey, name);
        return name;
    }

    static public bool GetGameModeDeleted (int gameModeId) {
        string path = GameModeContentPath (gameModeId);
        string s = GetKeyData (KeyDataPath (path), IsDeletedKey);
        return s == true.ToString();
    }

    static public bool SetGameModeDeleted (int gameModeId, bool deleted) {
        string path = GameModeContentPath (gameModeId);
        SetKeyData (KeyDataPath (path), IsDeletedKey, deleted.ToString());
        return deleted;
    }

    static public bool GetBoardDeleted (int boardId) {
        string path = BoardContentPath (boardId);
        string s = GetKeyData (KeyDataPath (path), IsDeletedKey);
        return s == true.ToString ();
    }

    static public bool SetBoardDeleted (int boardId, bool deleted) {
        string path = BoardContentPath (boardId);
        SetKeyData (KeyDataPath (path), IsDeletedKey, deleted.ToString ());
        return deleted;
    }

    static public bool GetBoardIsOfficial (int boardId) {
        string path = BoardContentPath (boardId);
        string s = GetKeyData (KeyDataPath (path), OfficialKey);
        return Convert.ToBoolean (s);
    }

    static public string SetBoardIsOfficial (int boardId, bool isOfficial) {
        string path = BoardContentPath (boardId);
        string isOfficialString = isOfficial.ToString ();
        SetKeyData (KeyDataPath (path), OfficialKey, isOfficialString);
        return isOfficialString;
    }

    static public bool GetGameModeIsOfficial (int gameModeId) {
        string path = GameModeContentPath (gameModeId);
        string s = GetKeyData (KeyDataPath (path), GameModeOfficialKey);
        return Convert.ToBoolean (s);
    }

    static public string SetGameModeIsOfficial (int gameModeId, bool isOfficial) {
        string path = GameModeContentPath (gameModeId);
        string isOfficialString = isOfficial.ToString ();
        SetKeyData (KeyDataPath (path), GameModeOfficialKey, isOfficialString);
        return isOfficialString;
    }

    static public string GameModeBoardsPath (int gameModeId) {
        string path = GameModeContentPath (gameModeId) + "Boards.txt";
        return path;
    }

    static public int [] GetAllLegalGameModeBoard (int gameModeId) {
        int [] ids = GetAllGameModeBoards (gameModeId);
        List<int> legalIds = new List<int> ();
        foreach (int id in ids){
            if (GetIsBoardLegal (id)) {
                legalIds.Add (id);
            }
        }
        return legalIds.ToArray ();
    }

    static public int [] GetAllLegalGameModeBoard (int gameModeId, int matchType) {
        int [] allIds = GetAllLegalGameModeBoard (gameModeId);
        List<int> ids = new List<int> ();
        foreach (int id in allIds) {
            int [] matchTypes = GetBoardMatchTypes (id);
            foreach (int type in matchTypes) {
                if (type == matchType) {
                    ids.Add (id);
                    break;
                }
            }
        }
        return ids.ToArray ();
    }

    static public int [] GetAllGameModeBoards (int gameModeId) {
        string path = GameModeBoardsPath (gameModeId);
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            int count = lines.Length;
            int [] ids = new int [count];
            for (int x = 0; x < count; x++) {
                ids [x] = int.Parse (lines [x]);
            }
            return ids;
        } else {
            return new int [0];
        }
    }

    static public int [] GetAllGameModeMatchTypes (int gameModeId) {
        int [] boardIds = GetAllGameModeBoards (gameModeId);
        List<int> allTypes = new List<int> ();
        foreach (int boardId in boardIds) {
            int [] types = GetBoardMatchTypes (boardId);
            foreach (int type in types) {
                if (!allTypes.Contains (type)) {
                    allTypes.Add (type);
                }
            }
        }
        return allTypes.ToArray ();
    }

    static public string BoardGameModesPath (int boardId) {
        string path = BoardContentPath (boardId) + "GameModes.txt";
        return path;
    }

    static public int [] GetAllBoards () {
        string [] s = Directory.GetDirectories (BoardContentPath ());
        List<int> ids = new List<int> ();
        for (int x = 0; x < s.Length; x++) {
            s [x] = s [x].Substring (s [x].LastIndexOf ('/') + 1);
            int id = int.Parse (s [x]);
            ids.Add (id);
        }
        return ids.ToArray();
    }

    static public int [] GetAllOfficialBoards () {
        int [] ids = GetAllBoards ();
        List<int> officialIds = new List<int> ();
        foreach (int id in ids) {
            if (GetBoardIsOfficial (id)) {
                officialIds.Add (id);
            }
        }
        return officialIds.ToArray ();
    }

    static public string BoardMatchTypesPath (int boardId) {
        return BoardContentPath (boardId) + "MatchTypes.txt";
    }

    static public int [] GetBoardMatchTypes (int boardId) {
        string path = BoardMatchTypesPath (boardId);
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            List<int> matchTypes = new List<int> ();
            foreach (string line in lines) {
                matchTypes.Add (int.Parse (line));
            }
            return matchTypes.ToArray ();
        }
        return new int [0];
    }

    static public void SetBoardMatchTypes (int boardId, int [] lines) {
        string path = BoardMatchTypesPath (boardId);
        List<string> types = new List<string> ();
        foreach (int type in lines) {
            types.Add (type.ToString ());
        }
        File.WriteAllLines (path, types.ToArray());
    }

    static public bool GetBoardMatchType (int boardId, int matchType) {
        int [] matchTypes = GetBoardMatchTypes (boardId);
        foreach (int type in matchTypes) {
            if (type == matchType) {
                return true;
            }
        }
        return false;
    }

    static public void AddBoardMatchType (int boardId, int matchType) {
        int [] matchTypes = GetBoardMatchTypes (boardId);
        bool exists = false;
        List<int> lines = new List<int> ();
        foreach (int type in matchTypes) {
            if (type == matchType) {
                exists = true;
            }
            lines.Add (type);
        }
        if (!exists) {
            lines.Add (matchType);
        }
        SetBoardMatchTypes (boardId, lines.ToArray ());
    }

    static public void RemoveBoardMatchType (int boardId, int matchType) {
        int [] matchTypes = GetBoardMatchTypes (boardId);
        List<int> lines = new List<int> ();
        foreach (int type in matchTypes) {
            if (type != matchType) {
                lines.Add (type);
            }
        }
        SetBoardMatchTypes (boardId, lines.ToArray ());
    }

    static public int [] GetAllBoardGameModes (int boardId) {
        string path = BoardGameModesPath (boardId);
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            int count = lines.Length;
            int [] ids = new int [count];
            for (int x = 0; x < count; x++) {
                ids [x] = int.Parse (lines [x]);
            }
            return ids;
        } else {
            return new int [0];
        }
    }

    static public bool SetGameModeBoard (int gameModeId, int boardId) {
        string path = GameModeBoardsPath (gameModeId);
        int [] ids = GetAllGameModeBoards (gameModeId);
        int count = ids.Length;
        List<string> idString = new List<string> ();
        bool alreadyExists = false;
        for (int x = 0; x < count; x++) {
            if (ids [x] == boardId) {
                alreadyExists = true;
            }
            idString.Add (ids [x].ToString ());
        }
        if (!alreadyExists) {
            idString.Add (boardId.ToString ());
            File.WriteAllLines (path, idString.ToArray ());
        }

        path = BoardGameModesPath (boardId);
        ids = GetAllBoardGameModes (boardId);
        count = ids.Length;
        idString = new List<string> ();
        for (int x = 0; x < count; x++) {
            if (ids [x] == gameModeId) {
                alreadyExists = true;
            }
            idString.Add (ids [x].ToString ());
        }
        if (!alreadyExists) {
            idString.Add (gameModeId.ToString ());
            File.WriteAllLines (path, idString.ToArray ());
        }
        CheckIfGameModeIsLegal (gameModeId);
        return true;
    }

    static public bool RemoveGameModeBoard (int gameModeId, int boardId) {
        string path = GameModeBoardsPath (gameModeId);
        int [] ids = GetAllGameModeBoards (gameModeId);
        int count = ids.Length;
        List<string> idString = new List<string> ();
        bool exist = false;
        for (int x = 0; x < count; x++) {
            if (ids [x] != boardId) {
                idString.Add (ids [x].ToString ());
            } else {
                exist = true;
            }
        }
        if (exist) {
            File.WriteAllLines (path, idString.ToArray ());
        }
        /*
        path = BoardGameModesPath (boardId);
        ids = GetAllBoardGameModes (boardId);
        count = ids.Length;
        idString = new List<string> ();
        for (int x = 0; x < count; x++) {
            if (ids [x] != gameModeId) {
                idString.Add (ids [x].ToString ());
            } else {
                exist = true;
            }
        }
        if (exist) {
            File.WriteAllLines (path, idString.ToArray ());
        }*/
        return true;
    }

    static public string [] SaveGameModeOwners (int id, string [] owners) {
        string path = GameModeOwnerPath (id);
        File.WriteAllLines (path, owners);
        return owners;
    }

    static public bool AddGameModeOwner (int id, string owner) {
        List<string> lines = new List<string> (GetBoardOwners (id));
        if (lines.Exists (x => x == owner)) {
            return false;
        }
        lines.Add (owner);
        SaveGameModeOwners (id, lines.ToArray ());
        AddUserProperty (owner, BoardProperty, id);
        return true;
    }




    static public string CardPoolContentPath (int gameModeId) {
        string path = GameModeContentPath (gameModeId) + "CardPool.txt";
        return path;
    }

    static public string [] GetCardPool (int gameModeId) {
        string path = CardPoolContentPath (gameModeId);
        if (File.Exists (path)) {
            return File.ReadAllLines (path);
        } else {
            return new string [0];
        }
    }

    static public string [] SetCardPool (int gameModeId, string [] lines) {
        string path = CardPoolContentPath (gameModeId);
        File.WriteAllLines (path, lines);
        CheckIfCardPoolIsLegal (gameModeId);
        CheckIfGameModeIsLegal (gameModeId);
        return lines;
    }

    static public string BoardContentPath () {
        string path = ContentPath () + "Board/";
        if (!Directory.Exists (path)) {
            Directory.CreateDirectory (path);
        }
        return path;
    }

    static public int SaveNewBoard (int gameModeId, string userName, string boardName, string [] board) {
        int id = IncrementBoardNextId ();
        SetBoard (id, board);
        AddBoardOwners (id, userName);
        SetBoardName (id, boardName);
        SetGameModeBoard (gameModeId, id);
        SetBoardMatchTypes (id, new int [] { 0, 1 });
        return id;
    }

    static void SetBoardNextId (int id) {
        string path = BoardContentPath () + "NextId.txt";
        if (!File.Exists (path)) {
            File.WriteAllText (path, "1");
        } else {
            File.WriteAllText (path, id.ToString ());
        }
    }

    static int GetBoardNextId () {
        string path = BoardContentPath () + "NextId.txt";
        if (!File.Exists (path)) {
            SetBoardNextId (1);
        }
        string Line = File.ReadAllText (path);
        return int.Parse (Line);
    }

    static public int IncrementBoardNextId () {
        int CurrentId = GetBoardNextId ();
        SetBoardNextId (CurrentId + 1);
        return CurrentId;
    }

    static public string BoardContentPath (int id) {
        string path = BoardContentPath () + id.ToString () + "/";
        if (!Directory.Exists (path)) {
            Directory.CreateDirectory (path);
        }
        return path;
    }

    static public string [] GetBoard (int id) {
        string path = BoardContentPath (id) + "Board.txt";
        string [] lines = File.ReadAllLines (path);
        return lines;
    }

    static public string SetBoard (int id, string [] s) {
        string path = BoardContentPath (id) + "Board.txt";
        File.WriteAllLines (path, s);
        CheckIfBoardIsLegal (id);
        int [] gameModeIds = GetAllBoardGameModes (id);
        foreach (int gameModeId in gameModeIds) {
            CheckIfGameModeIsLegal (gameModeId);
        }
        return path;
    }

    static public string BoardInfoPath (int id) {
        string path = BoardContentPath (id) + "Info.txt";
        return path;
    }

    static public string [] GetBoardInfo (int id) {
        string path = BoardInfoPath (id);
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            return lines;
        }
        return new string [0];
    }

    static public string [] SaveBoardInfo (int id, string [] info) {
        string path = BoardInfoPath (id);
        File.WriteAllLines (path, info);
        return info;
    }

    static public bool SetBoardInfoKey (int id, string keyName, string keyValue) {
        List<string> lines = new List<string> (GetBoardInfo (id));
        keyName = "***" + keyName;
        int index = lines.IndexOf (keyName);
        if (index == -1) {
            lines.Add (keyName);
            lines.Add (keyValue);
        } else {
            lines [index + 1] = keyValue;
        }
        SaveBoardInfo (id, lines.ToArray ());
        return index == -1 ? false : true;
    }

    static public string BoardOwnerPath (int id) {
        return BoardContentPath (id) + "Owner.txt";
    }

    static public string [] GetBoardOwners (int id) {
        string path = BoardOwnerPath (id);
        string [] lines;
        if (File.Exists (path)) {
            lines = File.ReadAllLines (path);
        } else {
            lines = new string [0];
        }
        return lines;
    }

    static public bool IsBoardOwner (int id, string accountName) {
        string [] owners = GetBoardOwners (id);
        foreach (string s in owners) {
            if (s == accountName) {
                return true;
            }
        }
        return false;
    }

    static public bool IsGameModeOwner (int id, string accountName) {
        string [] owners = GetGameModeOwners (id);
        foreach (string s in owners) {
            if (s == accountName) {
                return true;
            }
        }
        return false;
    }

    static public string [] SaveBoardOwners (int id, string [] owners) {
        string path = BoardOwnerPath (id);
        File.WriteAllLines (path, owners);
        return owners;
    }

    static public bool AddBoardOwners (int id, string owner) {
        List<string> lines = new List<string> (GetBoardOwners (id));
        if (lines.Exists (x => x == owner)) {
            return false;
        }
        lines.Add (owner);
        SaveBoardOwners (id, lines.ToArray ());
        AddUserProperty (owner, BoardProperty, id);
        return true;
    }

    static public string PlayerModePath (string owner) {
        string path = UserPath (owner) + "GameMode/";
        if (!Directory.Exists (path)) {
            Directory.CreateDirectory (path);
        }
        return path;
    }

    static public string [] GetAllPlayerModes (string owner) {
        string [] s = Directory.GetDirectories (PlayerModePath (owner));
        for (int x = 0; x < s.Length; x++) {
            s [x] = s [x].Substring (s [x].LastIndexOf ('/') + 1);
        }
        return s;
    }


    static public int [] GetAllGameModes () {
        string [] s = Directory.GetDirectories (GameModeContentPath ());
        List<int> ids = new List<int> ();
        for (int x = 0; x < s.Length; x++) {
            s [x] = s [x].Substring (s [x].LastIndexOf ('/') + 1);
            ids.Add (int.Parse (s [x]));
        }
        return ids.ToArray ();
    }

    static public int [] GetAllOfficialGameModes () {
        int [] allIds = GetAllGameModes ();
        List<int> offIds = new List<int> ();
        foreach (int id in allIds) {
            if (GetGameModeIsOfficial (id)) {
                offIds.Add (id);
            }
        }
        return offIds.ToArray ();
    }

    static public int [] GetAllPlayerModePathes (string owner) {
        string [] s = Directory.GetDirectories (PlayerModePath (owner));
        List<int> ids = new List<int> ();
        for (int x = 0; x < s.Length; x++) {
            s [x] = s [x].Substring (s [x].LastIndexOf ('/') + 1);
            ids.Add (int.Parse (s [x]));
        }
        return ids.ToArray ();
    }

    static public string PlayerModePath (string owner, int gameModeId) {
        string path = PlayerModePath (owner) + gameModeId + "/";
        if (!Directory.Exists (path)) {
            Directory.CreateDirectory (path);
        }
        return path;
    }

    static public string PlayerModeSetPath (string owner, int gameModeId) {
        string path = PlayerModePath (owner, gameModeId) + "Sets/";
        if (!Directory.Exists (path)) {
            Directory.CreateDirectory (path);
        }
        return path;
    }

    static public int GetPlayerModeSetNextId (string owner, int gameModeId) {
        return GetNextId (GetNextIdPath (PlayerModeSetPath (owner, gameModeId)));
    }


    static public string PlayerModeSetPath (string owner, int gameModeId, int setId) {
        string path = PlayerModeSetPath (owner, gameModeId) + setId + "/";
        if (!Directory.Exists (path)) {
            Directory.CreateDirectory (path);
        }
        return path;
    }

    static public int [] GetAllPlayerModeSets (string owner, int gameModeId) {
        string [] s = Directory.GetDirectories (PlayerModeSetPath (owner, gameModeId));
        int [] i = new int [s.Length];
        for (int x = 0; x < s.Length; x++) {
            s [x] = s [x].Substring (s [x].LastIndexOf ('/') + 1);
            i [x] = int.Parse (s [x]);
        }
        return i;
    }

    static public string GetPlayerModeSetName (string owner, int gameModeId, int setId) {
        string path = PlayerModeSetPath (owner, gameModeId, setId);
        string s = GetKeyData (KeyDataPath (path), SetNameKey);
        return s;
    }

    static public string SetPlayerModeSetName (string owner, int gameModeId, int setId, string name) {
        string path = PlayerModeSetPath (owner, gameModeId, setId);
        string s = SetKeyData (KeyDataPath (path), SetNameKey, name);
        return s;
    }

    static public int GetPlayerModeSetIconNumber (string owner, int gameModeId, int setId) {
        string path = PlayerModeSetPath (owner, gameModeId, setId);
        string s = GetKeyData (KeyDataPath (path), SetIconNumberKey);
        return int.Parse (s);
    }

    static public string SetPlayerModeSetIconNumber (string owner, int gameModeId, int setId, int number) {
        string path = PlayerModeSetPath (owner, gameModeId, setId);
        string s = SetKeyData (KeyDataPath (path), SetIconNumberKey, number.ToString ());
        return s;
    }


    static public int CreatePlayerModeSet (string owner, int gameModeId, string [] lines, string name) {
        int id = GetPlayerModeSetNextId (owner, gameModeId);
        SavePlayerModeSet (owner, gameModeId, id, lines);
        SetPlayerModeSetName (owner, gameModeId, id, name);
        SetPlayerModeSetIconNumber (owner, gameModeId, id, 1);
        return id;
    }

    static public void DeletePlayerModeSet (string owner, int gameModeId, int setId) {
        string path = PlayerModeSetPath (owner, gameModeId, setId);
        if (Directory.Exists (path)) {
            Directory.Delete (path, true);
        }
    }


    static public string [] SavePlayerModeSet (string owner, int gameModeId, int setId, string [] lines) {
        string path = PlayerModeSetPath (owner, gameModeId, setId) + "Set.txt";
        File.WriteAllLines (path, lines);
        return lines;
    }

    static public string [] GetPlayerModeSet (string owner, int gameModeId, int setId) {
        string path = PlayerModeSetPath (owner, gameModeId, setId) + "Set.txt";
        string [] lines;
        if (File.Exists (path)) {
            lines = File.ReadAllLines (path);
        } else {
            lines = new string [0];
        }
        return lines;
    }

    static public string UsersPath () {
        string path = ServerPath () + "Users/";
        if (!Directory.Exists (path)) {
            Directory.CreateDirectory (path);
        }
        return path;
    }

    static public string [] GetAllUsers () {
        string [] s = Directory.GetDirectories (UsersPath ());
        for (int x = 0; x < s.Length; x++) {
            s [x] = s [x].Substring (s [x].LastIndexOf ('/') + 1);
        }
        return s;
    }

    static public bool UserExists (string userName) {
        if (Directory.Exists (UserPath (userName))) {
            return true;
        }
        return false;
    }

    static public string UserPath (string userName) {
        return UsersPath () + userName + "/";
    }

    static public string UserDataPath (string userName) {
        if (UserExists (userName)) {
            string path = KeyDataPath (UserPath (userName));
            return path;
        }
        return null;
    }

    static public string UserPropertiesPath (string userName) {
        if (UserExists (userName)) {
            string path = UserPath (userName) + "Properties/";
            if (!Directory.Exists (path)) {
                Directory.CreateDirectory (path);
            }
            return path;
        }
        return null;
    }

    static public string UserPropertiesPath (string userName, string propertyType) {
        if (UserExists (userName)) {
            string path = UserPropertiesPath (userName) + propertyType + "s.txt";
            return path;
        }
        return null;
    }

    static public string [] GetUserProperties (string userName, string propertyType) {
        if (UserExists (userName)) {
            string path = UserPropertiesPath (userName, propertyType);
            if (File.Exists (path)) {
                string [] Lines = File.ReadAllLines (path);
                return Lines;
            }
        }
        return new string [0];
    }

    static public string [] SaveUserProperties (string userName, string propertyType, string [] boards) {
        if (UserExists (userName)) {
            string path = UserPropertiesPath (userName, propertyType);
            File.WriteAllLines (path, boards);
        }
        return boards;
    }

    static public bool AddUserProperty (string userName, string propertyType, int boardId) {
        if (UserExists (userName)) {
            List<string> Lines = new List<string> (GetUserProperties (userName, propertyType));
            if (Lines.Exists (x => x == boardId.ToString ())) {
            }
            Lines.Add (boardId.ToString ());
            SaveUserProperties (userName, propertyType, Lines.ToArray ());
            return true;
        }
        return false;
    }

    static public string GetUserKeyData (string userName, string key) {
        return GetKeyData (UserDataPath (userName), key);
    }

    static public string SetUserKeyData (string userName, string key, string data) {
        return SetKeyData (UserDataPath (userName), key, data);
    }

    static public string GetPlayerModeKeyData (string userName, int gameMode, string key) {
        return GetKeyData (KeyDataPath (PlayerModePath (userName, gameMode)), key);
    }

    static public string SetPlayerModeKeyData (string userName, int gameMode, string key, string data) {
        return SetKeyData (KeyDataPath (PlayerModePath (userName, gameMode)), key, data);
    }


    static public int GetPlayerModeSelectedSet (string accountName, int gameMode) {
        string selectedSet = GetPlayerModeKeyData (accountName, gameMode, SelectedSetKey);
        if (selectedSet == null || selectedSet == "") {
            SetPlayerModeKeyData (accountName, gameMode, SelectedSetKey, "1");
            return 1;
        }
        return int.Parse (selectedSet);
    }


    static public int SetPlayerModeSelectedSet (string accountName, int gameMode, int selectedSet) {
        SetPlayerModeKeyData (accountName, gameMode, SelectedSetKey, selectedSet.ToString());
        return selectedSet;
    }


    static public bool GetPlayerModeSelectedSetExists (string accountName, int gameMode) {
        int selectedSet = GetPlayerModeSelectedSet (accountName, gameMode);
        if (Directory.Exists (PlayerModeSetPath (accountName, gameMode) + selectedSet + "/")) {
            return true;
        }
        return false;
    }

    static public string SetUserKeyData (string userName, string key, int data) {
        return SetKeyData (UserDataPath (userName), key, data.ToString());
    }

    static public string [] GetAllKeyData (string path) {
        if (File.Exists (path)) {
            string [] Lines = File.ReadAllLines (path);
            return Lines;
        } else {
            return new string [0];
        }
    }

    static public string KeyDataPath (string path) {
        return path + "Data.txt";
    }

    static public string GetKeyData (string path, string key) {
        if (File.Exists (path)) {
            string [] Lines = File.ReadAllLines (path);
            int index = Array.IndexOf (Lines, "***" + key);
            if (index >= 0) {
                return Lines [index + 1];
            } else {
                return null;
            }
        }
        return null;
    }

    static public string SetKeyData (string path, string key, string value) {
        List<string> Lines = new List<string> (GetAllKeyData (path));
        string KeyCode = "***" + key;
        int index = Lines.IndexOf (KeyCode);
        if (index >= 0) {
            Lines [index + 1] = value;
        } else {
            Lines.Add (KeyCode);
            Lines.Add (value);
        }
        File.WriteAllLines (path, Lines.ToArray ());
        return key;
    }

    static public void CreateAccount (string accountName, string userName, string password, string email) {
        Directory.CreateDirectory (UserPath (accountName));
        List<string> Lines = new List<string> ();
        Lines.Add ("***Password");
        Lines.Add (EncryptString (password));
        Lines.Add ("***Email");
        Lines.Add (email);
        if (userName != null && userName != "") {
            Lines.Add ("***" + UserNameKey);
            Lines.Add (userName);
        }
        File.WriteAllLines (UserDataPath (accountName), Lines.ToArray ());
        HandClass hand = new HandClass ();
        hand.GenerateRandomHand (1);
        ServerData.SavePlayerModeSet (accountName, 1, 1, hand.HandToString ());
    }

    static public string SetUserName (string accountName, string newUserName) {
        string path = UserPath (accountName);
        string s = SetKeyData (KeyDataPath (path), UserNameKey, newUserName);
        return s;
    }

    static public string GetUserName (string accountName) {
        string path = UserPath (accountName);
        string s = GetKeyData (KeyDataPath (path), AvatarKey);
        if (s != null && s != "") {
            return s;
        } else {
            return "";
        }
    }

    static public int SetUserAvatar (string accountName, int value) {
        string path = UserPath (accountName);
        string s = SetKeyData (KeyDataPath (path), AvatarKey, value.ToString ());
        return value;
    }

    static public int GetUserAvatar (string accountName) {
        string path = UserPath (accountName);
        string s = GetKeyData (KeyDataPath (path), AvatarKey);
        if (s != null && s != "") {
            return int.Parse (s);
        }
        SetUserAvatar (accountName, 1);
        return 1;
    }

    static public int IncrementThisGameModeWon (string accountName, int gameModeId) {
        return SetThisGameModeWon (accountName, gameModeId, GetThisGameModeWon (accountName, gameModeId) + 1);
    }

    static public int SetThisGameModeWon (string accountName, int gameModeId, int value) {
        string path = PlayerModePath (accountName, gameModeId);
        string s = SetKeyData (KeyDataPath (path), GamesWonKey, value.ToString ());
        return value;
    }

    static public int GetThisGameModeWon (string accountName, int gameModeId) {
        string path = PlayerModePath (accountName, gameModeId);
        string s = GetKeyData (KeyDataPath (path), GamesWonKey);
        if (s != null && s != "") {
            return int.Parse (s);
        }
        SetThisGameModeWon (accountName, gameModeId, 0);
        return 0;
    }

    static public int IncrementThisGameModeLost (string accountName, int gameModeId) {
        return SetThisGameModeLost (accountName, gameModeId, GetThisGameModeLost (accountName, gameModeId) + 1);
    }

    static public int SetThisGameModeLost (string accountName, int gameModeId, int value) {
        string path = PlayerModePath (accountName, gameModeId);
        string s = SetKeyData (KeyDataPath (path), GamesLostKey, value.ToString ());
        return value;
    }

    static public int GetThisGameModeLost (string accountName, int gameModeId) {
        string path = PlayerModePath (accountName, gameModeId);
        string s = GetKeyData (KeyDataPath (path), GamesLostKey);
        if (s != null && s != "") {
            return int.Parse (s);
        }
        SetThisGameModeLost (accountName, gameModeId, 0);
        return 0;
    }

    static public int IncrementThisGameModeDrawn (string accountName, int gameModeId) {
        return SetThisGameModeDrawn (accountName, gameModeId, GetThisGameModeDrawn (accountName, gameModeId) + 1);
    }

    static public int SetThisGameModeDrawn (string accountName, int gameModeId, int value) {
        string path = PlayerModePath (accountName, gameModeId);
        string s = SetKeyData (KeyDataPath (path), GamesDrawnKey, value.ToString ());
        return value;
    }

    static public int GetThisGameModeDrawn (string accountName, int gameModeId) {
        string path = PlayerModePath (accountName, gameModeId);
        string s = GetKeyData (KeyDataPath (path), GamesDrawnKey);
        if (s != null && s != "") {
            return int.Parse (s);
        }
        SetThisGameModeDrawn (accountName, gameModeId, 0);
        return 0;
    }

    static public int IncrementThisGameModeUnfinished (string accountName, int gameModeId) {
        return SetThisGameModeUnfinished (accountName, gameModeId, GetThisGameModeUnfinished (accountName, gameModeId) + 1);
    }

    static public int DecrementThisGameModeUnfinished (string accountName, int gameModeId) {
        return SetThisGameModeUnfinished (accountName, gameModeId, GetThisGameModeUnfinished (accountName, gameModeId) - 1);
    }

    static public int SetThisGameModeUnfinished (string accountName, int gameModeId, int value) {
        string path = PlayerModePath (accountName, gameModeId);
        string s = SetKeyData (KeyDataPath (path), GamesUnfinishedKey, value.ToString ());
        return value;
    }

    static public int GetThisGameModeUnfinished (string accountName, int gameModeId) {
        string path = PlayerModePath (accountName, gameModeId);
        string s = GetKeyData (KeyDataPath (path), GamesUnfinishedKey);
        if (s != null && s != "") {
            return int.Parse (s);
        }
        SetThisGameModeUnfinished (accountName, gameModeId, 0);
        return 0;
    }

    static public int GetTotalWon (string accountName) {
        int [] ids = GetAllPlayerModePathes (accountName);
        int sum = 0;
        foreach (int id in ids) {
            sum += GetThisGameModeWon (accountName, id);
        }
        return sum;
    }

    static public int GetTotalLost (string accountName) {
        int [] ids = GetAllPlayerModePathes (accountName);
        int sum = 0;
        foreach (int id in ids) {
            sum += GetThisGameModeLost (accountName, id);
        }
        return sum;
    }

    static public int GetTotalDrawn (string accountName) {
        int [] ids = GetAllPlayerModePathes (accountName);
        int sum = 0;
        foreach (int id in ids) {
            sum += GetThisGameModeDrawn (accountName, id);
        }
        return sum;
    }

    static public int GetTotalUnfinished (string accountName) {
        int [] ids = GetAllPlayerModePathes (accountName);
        int sum = 0;
        foreach (int id in ids) {
            sum += GetThisGameModeUnfinished (accountName, id);
        }
        return sum;
    }

    static public int SetPlayerAvatar (string accountName, int value) {
        string path = UserPath (accountName);
        string s = SetKeyData (KeyDataPath (path), AvatarKey, value.ToString ());
        return value;
    }

    static public string GetServerKeyData (string key) {
        return GetKeyData (KeyDataPath (ServerPath ()), key);
    }

    static public void SetServerKeyData (string key, string value) {
        SetKeyData (KeyDataPath (ServerPath ()), key, value);
    }

    static public void SetInitVector () {
        string IV = GetServerKeyData (InitVectorKey);
        if (IV == null) {
            IV = generateRandomInitVector ();
            SetServerKeyData (InitVectorKey, IV);
        }
        initVector = IV;
        string EP = GetServerKeyData (EncryptPasswordKey);
        if (EP == null) {
            EP = generateRandomInitVector ();
            SetServerKeyData (EncryptPasswordKey, EP);
        }
        encryptPassword = EP;
    }


    static public string generateRandomInitVector () {
        string s = "";
        for (int x = 0; x < 16; x++) {
            s += (char) ('a' + UnityEngine.Random.Range (0, 26));
        }
        return s;
    }

    static private string initVector = "pemgail9uzpgzl88";
    static private string encryptPassword = "Doge";
    private const int keysize = 256;

    static public string UserPassword (string accountName) {
        return GetUserKeyData (accountName, PasswordKey);
    }

    static public int GetUserSelectedGameMode (string accountName) {
        string gameMode = GetUserKeyData (accountName, UserSelectedGameModeKey);
        if (gameMode == null || gameMode == "") {
            SetUserKeyData (accountName, UserSelectedGameModeKey, DefaultGameMode.ToString ());
            return DefaultGameMode;
        }
        return int.Parse (gameMode);
    }

    static public void SetUserSelectedGameMode (string accountName, int gameMode) {
        SetUserKeyData (accountName, UserSelectedGameModeKey, gameMode);
    }

    public static string EncryptString (string plainText) {
        return EncryptString (plainText, encryptPassword);
    }

    //https://tekeye.uk/visual_studio/encrypt-decrypt-c-sharp-string
        public static string EncryptString (string plainText, string passPhrase) {
        byte [] initVectorBytes = Encoding.UTF8.GetBytes (initVector);
        byte [] plainTextBytes = Encoding.UTF8.GetBytes (plainText);
        PasswordDeriveBytes password = new PasswordDeriveBytes (passPhrase, null);
        byte [] keyBytes = password.GetBytes (keysize / 8);
        RijndaelManaged symmetricKey = new RijndaelManaged ();
        symmetricKey.Mode = CipherMode.CBC;
        ICryptoTransform encryptor = symmetricKey.CreateEncryptor (keyBytes, initVectorBytes);
        MemoryStream memoryStream = new MemoryStream ();
        CryptoStream cryptoStream = new CryptoStream (memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write (plainTextBytes, 0, plainTextBytes.Length);
        cryptoStream.FlushFinalBlock ();
        byte [] cipherTextBytes = memoryStream.ToArray ();
        memoryStream.Close ();
        cryptoStream.Close ();
        return Convert.ToBase64String (cipherTextBytes);
    }

    /*
    static public  string Crypt (this string text) {
        Encrypt.EncryptString (textBoxString.Text, textBoxPassword.Text);
        return Convert.ToBase64String (
            System.Security.Cryptography.ProtectedData.Protect (
                Encoding.Unicode.GetBytes (text)));
    }*/



}
