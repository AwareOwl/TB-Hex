using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManagement : MonoBehaviour {

    bool AutoDeleteAllFinished = true;

    private void Start () {
        ServerData.SaveBackUp ();
    }

    // Update is called once per frame
    void Update () {
        MatchMakingClass.matches.RemoveAll (match => match.finished);
	}
}
