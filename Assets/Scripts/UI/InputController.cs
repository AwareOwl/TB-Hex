﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {
    

    float timer = 0;
    bool autoRunAI = true;

    private void Start () {
    }

    // Update is called once per frame
    void Update () {
        //Time.timeScale = 10;
        //Debug.Log (Input.mousePosition.x * 1080/Screen.height);
        //Tooltip.CreateComplexTooltip (null);
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
