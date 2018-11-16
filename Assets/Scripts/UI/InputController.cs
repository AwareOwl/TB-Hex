using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    float timer = 0;
    bool autoRunAI = true;

	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        //Debug.Log (ClientLogic.MyInterface.AccountName);
        /*if (autoRunAI) {
            if (ClientLogic.MyInterface.AccountName != null && ClientLogic.MyInterface.AccountName != "") {
                for (int x = 0; x < 2; x++) {
                    ClientLogic.MyInterface.CmdJoinGameAgainstAI ();
                    InGameUI.PlayedMatch.RunAI ();
                }
            }
            if (timer > 5) {
                timer -= 20;
                RatingClass.SaveEverything ();
            }
        }*/
	}
}
