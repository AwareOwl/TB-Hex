using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class ServerData : MonoBehaviour {

    static string UserNameKey = "UserName";
    static string BoardNameKey = "BoardName";
    static string GameModeNameKey = "GameModeName";

    static string BoardProperty = "Board";
    static string GameModeProperty = "GameMode";
    static string CardSetProperty = "CardSet";

    static public string ServerPath () {
        string path = @"C:/TokenBattleHex/";
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

    static public string SaveRatingPlayerWinRatio (string [] lines) {
        string path = RatingPath () + "PlayerWinRatio.txt";
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string SaveRatingTurn (string [] lines) {
        string path = RatingPath () + "Turn.txt";
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

    static void SetGameModeNextId (int id) {
        string path = GameModeContentPath () + "NextId.txt";
        if (!File.Exists (path)) {
            File.WriteAllText (path, "1");
        } else {
            File.WriteAllText (path, id.ToString ());
        }
    }

    static int GetGameModeNextId () {
        string path = GameModeContentPath () + "NextId.txt";
        if (!File.Exists (path)) {
            SetGameModeNextId (1);
        }
        string Line = File.ReadAllText (path);
        return int.Parse (Line);
    }

    static public int IncrementGameModeNextId () {
        int CurrentId = GetGameModeNextId ();
        SetGameModeNextId (CurrentId + 1);
        return CurrentId;
    }

    static public void CreateNewGameMode (string userName) {
        int id = IncrementGameModeNextId ();
        string path = GameModeContentPath (id);
        SetKeyData (KeyDataPath (path), GameModeNameKey, "New game mode");
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
        return lines;
    }

    static public string BoardContentPath () {
        string path = ContentPath () + "Board/";
        if (!Directory.Exists (path)) {
            Directory.CreateDirectory (path);
        }
        return path;
    }

    static public void SaveNewBoard (string userName, string boardName, string [] board) {
        int id = IncrementBoardNextId ();
        SetBoard (id, board);
        AddBoardOwners (id, userName);
        SetBoardInfoKey (id, BoardNameKey, boardName);
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
        } else{
            lines [index + 1] = keyValue;
        }
        SaveBoardInfo (id, lines.ToArray ());
        return index == -1 ? false: true;
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

    static public string [] SaveBoardOwners (int id, string [] owners) {
        string path = BoardOwnerPath (id);
        File.WriteAllLines (path, owners);
        return owners;
    }

    static public bool AddBoardOwners (int id, string owner) {
        List <string> lines = new List<string> (GetBoardOwners (id));
        if (lines.Exists (x => x == owner)) {
            return false;
        }
        lines.Add (owner);
        SaveBoardOwners (id, lines.ToArray ());
        AddUserProperty (owner, BoardProperty, id);
        return true;
    }

    static public string UsersPath () {
        string path = ServerPath () + "Users/";
        if (!Directory.Exists (path)) {
            Directory.CreateDirectory (path);
        }
        return path;
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
        string [] Lines = File.ReadAllLines (path);
        int index = Array.IndexOf (Lines, "***" + key);
        if (index >= 0) {
            return Lines [index + 1];
        } else {
            return null;
        }
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
        return key;
    }

    static public void CreateAccount (string userName, string password, string email) {
        Directory.CreateDirectory (UserPath (userName));
        List<string> Lines = new List<string> ();
        Lines.Add ("***Password");
        Lines.Add (password);
        Lines.Add ("***Email");
        Lines.Add (email);
        File.WriteAllLines (UserDataPath (userName), Lines.ToArray ());
    }

}
