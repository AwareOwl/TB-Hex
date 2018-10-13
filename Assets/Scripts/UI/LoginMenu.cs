using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LoginMenu : GOUI {

    static public LoginMenu instance;

    static public GameObject BackgroundObject;

    static public GameObject UsernameObject;
    static public GameObject PasswordObject;
    static public GameObject LoginObject;
    static public GameObject RegisterObject;

    static public InputField usernameInput;
    static public InputField passwordInput;
    static public string Username;
    static public string Password;

    private void Start () {
        instance = this;
        CreateLoginMenu ();
        CameraScript.SetStandardCamera ();
        CurrentGUI = this;
    }

    static public void ShowLoginMenu () {
        DestroyMenu ();
        CurrentCanvas.AddComponent<LoginMenu> ();
    }

    override public void DestroyThis () {
        if (instance != null) {
            DestroyImmediate (BackgroundObject);
            
            DestroyImmediate (UsernameObject);
            DestroyImmediate (PasswordObject);
            DestroyImmediate (LoginObject);

            DestroyImmediate (RegisterObject);
            DestroyImmediate (instance);
        }
    }

    private void Update () {
        if (Input.GetKeyDown (KeyCode.Tab)) {
            GameObject obj = EventSystem.current.currentSelectedGameObject;
            if (obj == null) {
                usernameInput.Select ();
            } else if (obj == UsernameObject) {
                passwordInput.Select ();
            } else {
                usernameInput.Select ();
            }
        }
    }


    static public void LogIn () {
        ClientLogic.MyInterface.CmdLogIn (usernameInput.text, passwordInput.text);
    }

    static public void CreateLoginMenu () {
        GameObject Clone;


        int px;
        int py;
        int sx;
        int sy;

        px = 720;
        py = 390;
        sx = 400;
        sy = 60;

        BackgroundObject = CreateSprite ("UI/Panel_Window_01_Sliced", px, 540, 10, sx + 120, 480, false);

        Clone = CreateInputField ("User name", px, py, sx, sy);
        usernameInput = Clone.GetComponent<InputField> ();
        UsernameObject = Clone;

        py += 75;
        
        Clone = CreateInputField ("Password", px, py, sx, sy);
        Clone.GetComponent<InputField> ().contentType = InputField.ContentType.Password;
        passwordInput = Clone.GetComponent<InputField> ();
        PasswordObject = Clone;

        py += 75;

        Clone = CreateSprite ("UI/Butt_M_EmptySquare", true);
        SetInPixScale (Clone, sx, sy);
        SetInPixPosition (Clone, px, py, 11);
        Clone.name = "LogUserIn";
        LoginObject = Clone;

        Clone = CreateText ("Log in", px, py, 12, 0.03f);
        Clone.GetComponent<TextMesh> ().color = Color.black;
        Clone.transform.SetParent (LoginObject.transform);
        AddTextToGameObject (LoginObject, Clone);

        py += 150;

        Clone = CreateSprite ("UI/Butt_M_EmptySquare", true);
        SetInPixScale (Clone, sx, sy);
        SetInPixPosition (Clone, px, py, 11);
        Clone.name = "RegisterMenu";
        RegisterObject = Clone;

        Clone = CreateText ("Register", px, py, 12, 0.03f);
        Clone.GetComponent<TextMesh> ().color = Color.black;
        Clone.transform.SetParent (RegisterObject.transform);
        AddTextToGameObject (RegisterObject, Clone);
    }
}
