using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientLogic : MonoBehaviour {

    static public ClientInterface MyInterface;

    static public void LogIn (string accountName, string userName) {
        MyInterface.AccountName = accountName;
        MyInterface.UserName = userName;

        Time.timeScale = 1f;
        //BoardEditorMenu.ShowBoardEditorMenu ();
        //CardPoolEditor.ShowCardPoolEditorMenu ();
        ClientLogic.MyInterface.CmdJoinGameAgainstAI ();
    }
}
