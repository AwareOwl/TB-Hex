using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManagement : MonoBehaviour {

    bool AutoDeleteAllFinished = true;

    private void Awake () {
        ServerData.SaveBackUp ();
        //ServerData.DeleteBackUps ();
        ServerData.SetInitVector ();
        ServerVersionManager.CheckServerVersion ();
        ServerVersionManager.FinalizeServerVersion ();
    }

    // Update is called once per frame
    void Update () {
        MatchMakingClass.matches.RemoveAll (match => match.finished);
	}
}
