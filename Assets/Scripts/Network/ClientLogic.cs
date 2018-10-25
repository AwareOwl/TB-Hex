using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientLogic : MonoBehaviour {

    static public ClientInterface MyInterface;

    static public void LogIn () {
        //BoardEditorMenu.ShowBoardEditorMenu ();
        CardPoolEditor.ShowCardPoolEditorMenu ();
    }
}
