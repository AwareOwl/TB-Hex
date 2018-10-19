using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class ServerData : MonoBehaviour {

    static public string ServerPath () {
        string path = @"C:/TokenBattleHex/";
        if (!Directory.Exists (path)) {
            Directory.CreateDirectory (path);
        }
        return path;
    }

    static public string ContentPath () {
        string path = ServerPath () + "Content/";
        if (!Directory.Exists (path)) {
            Directory.CreateDirectory (path);
        }
        return path;
    }

    static public string BoardContentPath () {
        string path = ContentPath () + "Board/";
        if (!Directory.Exists (path)) {
            Directory.CreateDirectory (path);
        }
        return path;
    }

    static void SaveNewBoard (string userName, string boardName, string [] board) {

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

    static public string [] GetBoardInfo (int id) {
        string path = BoardContentPath (id) + "Info.txt";
        string [] lines = File.ReadAllLines (path);
        return lines;
    }

    static public string [] GetBoardOwner (int id) {
        string path = BoardContentPath (id) + "Owner.txt";
        string [] lines = File.ReadAllLines (path);
        return lines;
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
            string path = UserPath (userName) + "Data.txt";
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

    static public string UserBoardPropertiesPath (string userName) {
        if (UserExists (userName)) {
            string path = UserPropertiesPath (userName) + "Board.txt";
            return path;
        }
        return null;
    }

    static public string [] GetUserBoardProperties (string userName) {
        string [] Lines = File.ReadAllLines (UserBoardPropertiesPath (userName));
        return Lines;
    }

    static public string GetUserKeyData (string userName, string key) {
        return GetKeyData (UserDataPath (userName), key);
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

    static public void CreateAccount (string userName, string password, string email) {
        Debug.Log ("Wut");
        Directory.CreateDirectory (UserPath (userName));
        List<string> Lines = new List<string> ();
        Lines.Add ("***Password");
        Lines.Add (password);
        Lines.Add ("***Email");
        Lines.Add (email);
        File.WriteAllLines (UserDataPath (userName), Lines.ToArray ());
    }


    /*
    static public string UpdateUserKeyData (string userName, string key) {
        string [] Lines = File.ReadAllLines (UserDataPath (userName));
        int index = 
    }*/

}
