using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {
    

    float timer = 0;
    static public bool autoRunAI = false;

    private void Start () {
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
                    ServerLogic.JoinGameAgainstAI (ClientLogic.MyInterface).RunAI ();
                    //Debug.Log (InGameUI.PlayedMatch);
                    //InGameUI.PlayedMatch.RunAI ();
                }
            }
            if (timer > 5) {
                timer -= 20;
                RatingClass.SaveEverything ();
            }
        }
	}
}
