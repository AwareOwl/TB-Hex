using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Language : MonoBehaviour {

    public const int English = 0;
    public const int Polish = 1;

    static public string [] FileName = new string [2] { "English", "Polish" };

    static public string CreateLocalNetwork;
    static public string JoinLocalNetwork;

    static public string AccountName;
    static public string Password;
    static public string ConfirmPassword;
    static public string Email;

    static Language () {

    }

    static void LoadLanguage (int language) {
        TextAsset asset = Resources.Load ("Resources/Languages/" + FileName + "UI.txt") as TextAsset;
        string allLines = asset.text;
        string [] lines = allLines.Split ('[');
    }
}
