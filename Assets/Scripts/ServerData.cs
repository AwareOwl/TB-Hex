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

    static public string VersionKey = "Version";
    static public string InitVectorKey = "InitVector";
    static public string EncryptPasswordKey = "EncryptPassword";

    static string BoardProperty = "Board";
    static string GameModeProperty = "GameMode";
    static string CardSetProperty = "CardSet";

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

    static public string PlayerModePath (string owner) {
        string path = UserPath (owner) + "GameMode/";
        if (!Directory.Exists (path)) {
            Directory.CreateDirectory (path);
        }
        return path;
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

    static public string PlayerModeSetPath (string owner, int gameModeId, int setId) {
        string path = PlayerModeSetPath (owner, gameModeId) + setId + "/";
        if (!Directory.Exists (path)) {
            Directory.CreateDirectory (path);
        }
        return path;
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
        hand.GenerateRandomHand ();
        ServerData.SavePlayerModeSet (accountName, 1, 1, hand.HandToString ());
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
