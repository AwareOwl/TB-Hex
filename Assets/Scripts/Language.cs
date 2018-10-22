using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Language {

    static string SelectedLanguageKey = "SelectedLanguage";

    public const int English = 0;
    public const int Polish = 1;

    static public string [] FileName = new string [2] { "English", "Polish" };

    static public string [] UI;

    static public string CreateLocalNetwork;
    static public string JoinLocalNetwork;

    static public string AccountName;
    static public string Password;
    static public string ConfirmPassword;
    static public string Email;

    static public string LogIn;
    static public string Register;

    static public int PasswordIsIncorrectKey;
    static public int AccountDoesntExistKey;
    static public int PleaseEnterAccountNameKey;
    static public int PasswordHasToHaveAtLeast8CharactersKey;
    static public int AccountNameCantBeNullKey;
    static public int AccountWithThisNameAlreadyExistsKey;
    static public int AccountCreatedKey;

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
        string path = "Languages/" + FileName [language] + "UI";
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

    }
}
