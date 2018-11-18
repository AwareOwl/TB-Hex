using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientLogic : MonoBehaviour {

    static public ClientInterface MyInterface;

    static public void LogIn () {
        Time.timeScale = 0.5f;
        //BoardEditorMenu.ShowBoardEditorMenu ();
        //CardPoolEditor.ShowCardPoolEditorMenu ();
        ClientLogic.MyInterface.CmdJoinGameAgainstAI ();
    }
}
