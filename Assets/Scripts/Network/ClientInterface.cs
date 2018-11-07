using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientInterface : NetworkBehaviour {

    public string AccountName;
    public int GameMode = 1;

    public void Start () {
        if (isLocalPlayer) {
            ClientLogic.MyInterface = this;
        }
    }

    [Command]
    public void CmdLogIn (string username, string password) {
        ServerLogic.LogIn (this, username, password);
    }

    [Command]
    public void CmdRegister (string username, string password, string email) {
        ServerLogic.Register (this, username, password, email);
    }

    [TargetRpc]
    public void TargetShowMessage (NetworkConnection target, int messageID) {
        GOUI.ShowMessage (Language.UI [messageID]);
    }

    [TargetRpc]
    public void TargetLogIn (NetworkConnection target) {
        ClientLogic.LogIn ();
    }

    [Command]
    public void CmdJoinGameAgainstAI () {
        ServerLogic.JoinGameAgainstAI (this);
    }
    /*
    [TargetRpc]
    public void TargetAccountDoesntExist (NetworkConnection target) {
        Debug.Log ("Account doesn't exists");
    }

    [TargetRpc]
    public void TargetPasswordIncorrect (NetworkConnection target) {
        Debug.Log ("PasswordIncorrect");
    }

    [TargetRpc]
    public void TargetLoggedSuccesfully (NetworkConnection target) {
        Debug.Log ("LoggedSuccesfully");
    }

    [TargetRpc]
    public void TargetUserExists (NetworkConnection target) {
        Debug.Log ("User already exists");
    }

    [TargetRpc]
    public void TargetNullUsername (NetworkConnection target) {
        Debug.Log ("Username name can't be null");
    }

    [TargetRpc]
    public void TargetPasswordLegnth (NetworkConnection target) {
        Debug.Log ("Password has to have at least 8 characters");
    }

    [TargetRpc]
    public void TargetAccountCreated (NetworkConnection target) {
        Debug.Log ("Account created");
    }*/
}
