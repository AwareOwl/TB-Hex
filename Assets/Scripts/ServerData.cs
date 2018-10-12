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

    static public string GetUserKeyData (string userName, string key) {
        string [] Lines = File.ReadAllLines (UserDataPath (userName));
        int index = Array.IndexOf (Lines, key);
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
