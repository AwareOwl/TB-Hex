using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {
    

    float timer = 0;
    float timer2 = 0;
    static public bool autoRunAI = false;

    static public bool debuggingEnabled;

    private void Start () {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update () {
        //Time.timeScale = 10;
        //Debug.Log (Input.mousePosition.x * 1080/Screen.height);
        //Tooltip.CreateComplexTooltip (null);
        timer += Time.deltaTime;
        timer2 += Time.deltaTime;
        //Debug.Log (ClientLogic.MyInterface.AccountName);
        if (autoRunAI) {
            if (Input.GetKey ("r") && Input.GetKeyDown ("a")) {
                ServerLogic.JoinGameAgainstAI (ClientLogic.MyInterface);
            }
            if (Input.GetKey ("s") && Input.GetKeyDown ("m")) {
                //for (int x = 0; x < 10; x++) {
                    //Debug.Log ("Start");
                    //ServerLogic.JoinGameAgainstAI (ClientLogic.MyInterface);
                //}
            }
            if (ClientLogic.MyInterface.AccountName != null && ClientLogic.MyInterface.AccountName != "") {
                if (timer2 > 1) {
                    //Debug.Log ("Start");
                    ServerLogic.JoinGameAgainstAI (ClientLogic.MyInterface);
                    timer2-= 1.8f;
                }
                for (int x = 0; x < 1; x++) {
                    //ClientLogic.MyInterface.CmdJoinGameAgainstAI ();
                    //Debug.Log ("Starting match");
                    //ServerLogic.JoinGameAgainstAI (ClientLogic.MyInterface);
                    //Debug.Log (InGameUI.PlayedMatch);
                    //InGameUI.PlayedMatch.RunAI ();
                }
            }
            if (timer > 5) {
                if (ClientLogic.MyInterface.AccountName != null && ClientLogic.MyInterface.AccountName != "") {
                    //ServerLogic.JoinGameAgainstAI (ClientLogic.MyInterface);
                }
                timer -= 40;
                RatingClass.SaveEverything ();
                //Debug.Log (AIClass.maxCardValue);
                AIClass.maxCardValue = 0;
                AIRatingMenu.RefreshPage ();
            }
        }
        if (Input.GetKey ("d") && Input.GetKeyDown ("b")) {
            Debug.Log ("Debugging enabled");
            debuggingEnabled = true;
        }
        if (Input.GetKey ("b") && Input.GetKeyDown ("e")) {
            BoardEditorMenu.ShowBoardEditorMenu ();
        }
        if (Input.GetKey ("p") && Input.GetKeyDown ("m")) {
            ClientLogic.MyInterface.CmdJoinGameAgainstAI ();
        }
    }
}
