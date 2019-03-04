using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {
    

    float timer = 0;
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
        //Debug.Log (ClientLogic.MyInterface.AccountName);
        if (autoRunAI) {
            if (ClientLogic.MyInterface.AccountName != null && ClientLogic.MyInterface.AccountName != "") {
                for (int x = 0; x < 2; x++) {
                    //ClientLogic.MyInterface.CmdJoinGameAgainstAI ();
                    //Debug.Log ("Starting match");
                    ServerLogic.JoinGameAgainstAI (ClientLogic.MyInterface);
                    //Debug.Log (InGameUI.PlayedMatch);
                    //InGameUI.PlayedMatch.RunAI ();
                }
            }
            if (timer > 5) {
                timer -= 30;
                RatingClass.SaveEverything ();
                Debug.Log (AIClass.maxCardValue);
                AIClass.maxCardValue = 0;
                AIRatingMenu.RefreshPage ();
            }
        }
        if (Input.GetKey ("d") && Input.GetKeyDown ("b")) {
            Debug.Log ("Debugging enabled");
            debuggingEnabled = true;
        }
	}
}
