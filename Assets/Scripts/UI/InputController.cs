using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log (ClientLogic.MyInterface.AccountName);
        if (ClientLogic.MyInterface.AccountName != null && ClientLogic.MyInterface.AccountName != "") {
            ClientLogic.MyInterface.CmdJoinGameAgainstAI ();
            InGameUI.PlayedMatch.RunAI ();
        }
	}
}
