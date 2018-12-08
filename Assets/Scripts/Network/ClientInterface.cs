using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientInterface : NetworkBehaviour {

    public string AccountName;
    public string UserName;
    public int GameMode = 2;

    public MatchClass currentMatch;

    public void Start () {
        if (isServer) {
            gameObject.AddComponent<ServerManagement> ();
        }
        if (isLocalPlayer) {
            ClientLogic.MyInterface = this;
            gameObject.AddComponent<InputController> ();
            CmdCompareServerVersion ("0.1.1.0");
        }
    }

    [Command]
    public void CmdLogIn (string username, string password) {
        ServerLogic.LogIn (this, username, password);
    }

    [Command]
    public void CmdRegister (string accountName, string userName, string password, string email) {
        ServerLogic.Register (this, accountName, userName, password, email);
    }

    [TargetRpc]
    public void TargetShowMessage (NetworkConnection target, int messageID) {
        GOUI.ShowMessage (Language.UI [messageID]);
    }

    /*
    [TargetRpc]
    public void TargetShowMatchResult (NetworkConnection target, string winnerName, int winCondition, int limit) {
        GOUI.ShowMessage (Language.GetMatchResult (winnerName, winCondition, limit), "MainMenu");
    }*/

    [TargetRpc]
    public void TargetLogIn (NetworkConnection target, string accountName, string userName) {
        ClientLogic.LogIn (accountName, userName);
    }

    [Command]
    public void CmdJoinGameAgainstAI () {
        ServerLogic.JoinGameAgainstAI (this);
    }

    [TargetRpc]
    public void TargetInvalidSet (NetworkConnection target) {
        GOUI.ShowMessage (Language.GetInvalidSetMessage ());
    }

    [TargetRpc]
    public void TargetInvalidSavedSet (NetworkConnection target) {
        GOUI.ShowMessage (Language.GetInvalidSavedSetMessage ());
    }

    [Command]
    public void CmdCompareServerVersion (string version) {
        ServerLogic.CompareServerVersion (this, version);
    }

    [TargetRpc]
    public void TargetShowLoginMenu (NetworkConnection target) {
        LoginMenu.ShowLoginMenu ();
    }

    [TargetRpc]
    public void TargetInvalidVersionMessage (NetworkConnection target, string serverVersion) {
        GOUI.ShowMessage (Language.GetInvalidGameVersionMessage (serverVersion), "ExitGame");
    }

    [Command]
    public void CmdDownloadCardPoolToEditor () {
        ServerLogic.DownloadCardPoolToEditor (this);
    }

    [TargetRpc]
    public void TargetDownloadCardPoolToEditor (NetworkConnection target, string [] lines) {
        SetEditor.LoadCardPool (lines);
    }


    [Command]
    public void CmdSaveSelectedSet (int selectedSet) {
        ServerLogic.SaveSelectedSet (this, selectedSet);
    }


    [Command]
    public void CmdDownloadSetList () {
        ServerLogic.DownloadSetList (this);
    }

    [TargetRpc]
    public void TargetDownloadSetList (NetworkConnection target, string [] setName, int [] setNumber, int [] iconNumber, bool [] legal, int selectedSet) {
        SetList.LoadSetList (setName, setNumber, iconNumber, legal, selectedSet);
    }


    [Command]
    public void CmdCreateNewSet (string setName) {
        ServerLogic.CreateNewSet (this, setName);
    }


    [Command]
    public void CmdDeleteSet (int id) {
        ServerLogic.DeleteSet (this, id);
    }

    [Command]
    public void CmdSavePlayerModeSet (string [] s, int setId) {
        ServerLogic.SavePlayerModeSet (this, s, setId);
    }

    [Command]
    public void CmdDownloadSetToEditor (int setId) {
        ServerLogic.DownloadSetToEditor (this, setId);
    }

    [TargetRpc]
    public void TargetDownloadSetToEditor (NetworkConnection target, string [] lines) {
        SetEditor.LoadSet (lines);
    }

    [TargetRpc]
    public void TargetDownloadCurrentGameMatch (NetworkConnection target, string [] lines) {
        ClientLogic.LoadCurrentGameMatch (lines);
    }

    [TargetRpc]
    public void TargetDownloadCurrentGameMatchProperties (NetworkConnection target, string [] lines) {
        ClientLogic.LoadCurrentGameMatchProperties (lines);
    }
    [TargetRpc]
    public void TargetDownloadCurrentGameBoard (NetworkConnection target, string [] lines) {
        ClientLogic.LoadCurrentGameBoard (lines);
    }

    [TargetRpc]
    public void TargetDownloadCurrentGamePlayer (NetworkConnection target, int playerNumber, string [] lines) {
        ClientLogic.LoadCurrentGamePlayer (playerNumber, lines);
    }

    [TargetRpc]
    public void TargetDownloadCurrentGamePlayerProperties (NetworkConnection target, int playerNumber, string [] lines) {
        ClientLogic.LoadCurrentGamePlayerProperties (playerNumber, lines);
    }
    
    [TargetRpc]
    public void TargetDownloadCurrentGameHand (NetworkConnection target, int playerNumber, string [] lines) {
        ClientLogic.LoadCurrentGameHand (playerNumber, lines);
    }


    [TargetRpc]
    public void TargetFinishDownloadCurrentGame (NetworkConnection target) {
        InGameUI.ShowInGameUI ();
    }


    [Command]
    public void CmdCurrentGameMakeAMove (int x, int y, int playerNumber, int stackNumber) {
        ServerLogic.CurrentGameMakeAMove (this, x, y, playerNumber, stackNumber);
    }

    [TargetRpc]
    public void TargetCurrentGameMakeAMove (NetworkConnection target, int x, int y, int playerNumber, int stackNumber) {
        if (InGameUI.PlayedMatch != null) {
            InGameUI.PlayedMatch.PlayCard (x, y, playerNumber, stackNumber);
        }
    }

    [TargetRpc]
    public void TargetGetStartingSetName (NetworkConnection target) {
        CmdSetStartingSetName (Language.StartingSet);
    }

    [Command]
    public void CmdSetStartingSetName (string startingSetName) {
        AccountVersionManager.SetStartingSetNameToAllSets (AccountName, startingSetName);
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
