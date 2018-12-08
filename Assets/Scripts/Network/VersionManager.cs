using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionManager  {

    static public string version;

    static public int GameVersion;
    static public int PathVersion;
    static public int HotfixVersion;
    static public int DevelopVersion;


    static public string GetVersion () {
        return GameVersion + "." + PathVersion + "." + HotfixVersion + "." + DevelopVersion;
    }

    static public string [] GetResource (string path) {
        string s = (Resources.Load (path) as TextAsset).text;
        return s.Split (new string [] { System.Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries);
    }

    static public bool CompareVersion (string reference) {
        return reference == GetVersion ();
    }

    static public void ConvertStringToVersion (string version) {
        if (version == null || version == "") {
            GameVersion = 0;
            PathVersion = 0;
            HotfixVersion = 0;
            DevelopVersion = 0;
        } else {
            string [] words = version.Split ('.');
            GameVersion = int.Parse (words [0]);
            PathVersion = int.Parse (words [1]);
            HotfixVersion = int.Parse (words [2]);
            DevelopVersion = int.Parse (words [3]);
        }
    }
}
