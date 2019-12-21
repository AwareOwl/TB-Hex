using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class LibraryData : ServerData {
    
    static public string LibraryPath () {
        string path = ServerPath () + "Library/";
        if (!Directory.Exists (path)) {
            Directory.CreateDirectory (path);
        }
        return path;
    }

    static public string WorksWellWithPath () {
        return LibraryPath () + "WorksWellWith.txt";
    }

    static public string SaveWorksWellWith (string lines) {
        string path = WorksWellWithPath ();
        File.WriteAllText (path, lines);
        return path;
    }

    static public string GetWorksWellWith () {
        string path = WorksWellWithPath ();
        if (File.Exists (path)) {
            string lines = File.ReadAllText (path);
            return lines;
        }
        return null;
    }

    static public string IsGoodAgainstPath () {
        return LibraryPath () + "IsGoodAgainst.txt";
    }

    static public string SaveIsGoodAgainst (string lines) {
        string path = IsGoodAgainstPath ();
        File.WriteAllText (path, lines);
        return path;
    }

    static public string GetIsGoodAgainst () {
        string path = IsGoodAgainstPath ();
        if (File.Exists (path)) {
            string lines = File.ReadAllText (path);
            return lines;
        }
        return null;
    }

    static public string IsWeakAgainstPath () {
        return LibraryPath () + "IsWeakAgainst.txt";
    }

    static public string SaveIsWeakAgainst (string lines) {
        string path = IsWeakAgainstPath ();
        File.WriteAllText (path, lines);
        return path;
    }

    static public string GetIsWeakAgainst () {
        string path = IsWeakAgainstPath ();
        if (File.Exists (path)) {
            string lines = File.ReadAllText (path);
            return lines;
        }
        return null;
    }

    static public string ListToString (List <int> [,] [] values) {
        string s = "";
        for (int x1 = 0; x1 < values.GetLength (0); x1++) {
            for (int x2 = 0; x2 < values.GetLength (1); x2++) {
                List<int> [] aList = values [x1, x2];
                int count3 = aList.Length;
                for (int x3 = 0; x3 < count3; x3++) {
                    List<int> list = aList [x3];
                    int count4 = list.Count;
                    for (int x4 = 0; x4 < count4; x4++) {
                        s += list [x4] + " ";
                    }
                    s += ";";
                }
                s += ":";
            }
            s += "'";
        }
        return s;
    }

    static public List<int> [,] [] StringToJaggedList (string s) {
        List<int> [,] [] values = new List<int> [2, 2] [];
        string [] s1 = s.Split (new char [1] { '\'' });
        int count1 = s1.Length - 1;
        for (int x1 = 0; x1 < count1; x1++) {
            string [] s2 = s1 [x1].Split (new char [1] { ':' });
            int count2 = s2.Length - 1;
            for (int x2 = 0; x2 < count2; x2++) {
                string [] s3 = s2 [x2].Split (new char [1] { ';' });
                int count3 = s3.Length - 1;
                int count32;
                if (x1 == 0) {
                    count32 = AppDefaults.availableTokens;
                } else {
                    count32 = AppDefaults.availableAbilities;
                }
                values [x1, x2] = new List<int> [count32];
                List <int> [] tempList = values [x1, x2];
                for (int x3 = 0; x3 < count32; x3++) {
                    List<int> tempList2 = tempList [x3] = new List<int> ();
                    if (x3 >= count3) {
                        continue;
                    }
                    string [] s4 = s3 [x3].Split (new char [1] { ' ' });
                    int count4 = s4.Length - 1;
                    for (int x4 = 0; x4 < count4; x4++) {
                        if (s4 [x4] != null && s4[x4] != "") {
                            tempList2.Add (int.Parse (s4 [x4]));
                        }
                    }
                }
            }
        }
        return values;
    }

}
