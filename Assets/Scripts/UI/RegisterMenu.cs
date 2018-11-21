using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RegisterMenu : GOUI {

    static public RegisterMenu instance;

    static public GameObject BackgroundObject;

    static public GameObject AccountNameObject;
    static public GameObject UserNameObject;
    static public GameObject PasswordObject;
    static public GameObject ConfirmPasswordObject;
    static public GameObject EmailObject;

    static public GameObject LoginObject;
    static public GameObject RegisterObject;

    static public InputField accountNameInput;
    static public InputField userNameInput;
    static public InputField passwordInput;
    static public InputField confirmPasswordInput;
    static public InputField emailInput;

    static public string Username;
    static public string Password;

    private void Start () {
        instance = this;
        CreateRegisterMenu ();
        CurrentGUI = this;
    }

    static public void ShowRegisterMenu () {
        DestroyMenu ();
        CurrentCanvas.AddComponent<RegisterMenu> ();
    }

    override public void DestroyThis () {
        if (instance != null) {
            DestroyImmediate (BackgroundObject);

            DestroyImmediate (AccountNameObject);
            DestroyImmediate (UserNameObject);
            DestroyImmediate (PasswordObject);
            DestroyImmediate (ConfirmPasswordObject);
            DestroyImmediate (EmailObject);

            DestroyImmediate (RegisterObject);
            DestroyImmediate (LoginObject);

            DestroyImmediate (instance);
        }
    }

    private void Update () {
        if (Input.GetKeyDown (KeyCode.Tab)) {
            GameObject obj = EventSystem.current.currentSelectedGameObject;
            if (obj == null) {
                accountNameInput.Select ();
            } else if (obj == AccountNameObject) {
                userNameInput.Select ();
            } else if (obj == UserNameObject) {
                passwordInput.Select ();
            } else if (obj == PasswordObject) {
                confirmPasswordInput.Select ();
            } else if (obj == ConfirmPasswordObject) {
                emailInput.Select ();
            } else {
                accountNameInput.Select ();
            }
        }
    }


    static public void Register () {
        if (passwordInput.text == confirmPasswordInput.text) {
            ClientLogic.MyInterface.CmdRegister (accountNameInput.text, userNameInput.text, passwordInput.text, emailInput.text);
        } else {
            ShowMessage ("Passwords don't match.");
        }
    }

    static public void CreateRegisterMenu () {
        GameObject Clone;


        int px;
        int py;
        int sx;
        int sy;

        px = 720;
        py = 285;
        sx = 400;
        sy = 60;

        BackgroundObject = CreateSprite ("UI/Panel_Window_01_Sliced", px, 540, 10, sx + 120, 705, false);

        Clone = CreateInputField (Language.AccountName, px, py, sx, sy);
        accountNameInput = Clone.GetComponent<InputField> ();
        AccountNameObject = Clone;
        Clone.name = "AccountNameObject";

        py += 75;

        Clone = CreateInputField (Language.UserName, px, py, sx, sy);
        userNameInput = Clone.GetComponent<InputField> ();
        UserNameObject = Clone;
        Clone.name = "UserNameObject";

        py += 75;

        Clone = CreateInputField (Language.Password, px, py, sx, sy);
        Clone.GetComponent<InputField> ().contentType = InputField.ContentType.Password;
        passwordInput = Clone.GetComponent<InputField> ();
        PasswordObject = Clone;
        Clone.name = "PasswordObject";

        py += 75;

        Clone = CreateInputField (Language.ConfirmPassword, px, py, sx, sy);
        Clone.GetComponent<InputField> ().contentType = InputField.ContentType.Password;
        confirmPasswordInput = Clone.GetComponent<InputField> ();
        ConfirmPasswordObject = Clone;
        Clone.name = "ConfirmPasswordObject";

        py += 75;

        Clone = CreateInputField (Language.Email, px, py, sx, sy);
        Clone.GetComponent<InputField> ().contentType = InputField.ContentType.EmailAddress;
        emailInput = Clone.GetComponent<InputField> ();
        EmailObject = Clone;
        Clone.name = "EmailObject";

        py += 75;

        Clone = CreateSprite ("UI/Butt_M_EmptySquare", true);
        SetInPixScale (Clone, sx, sy);
        SetInPixPosition (Clone, px, py, 11);
        Clone.name = "RegisterUser";
        RegisterObject = Clone;

        Clone = CreateText (Language.Register, px, py, 12, 0.03f);
        Clone.GetComponent<TextMesh> ().color = Color.black;
        Clone.transform.SetParent (RegisterObject.transform);
        AddTextToGameObject (RegisterObject, Clone);

        py += 150;

        Clone = CreateSprite ("UI/Butt_M_EmptySquare", true);
        SetInPixScale (Clone, sx, sy);
        SetInPixPosition (Clone, px, py, 11);
        Clone.name = "LoginMenu";
        LoginObject = Clone;

        Clone = CreateText (Language.LogIn, px, py, 12, 0.03f);
        Clone.GetComponent<TextMesh> ().color = Color.black;
        Clone.transform.SetParent (LoginObject.transform);
        AddTextToGameObject (LoginObject, Clone);
    }
}
