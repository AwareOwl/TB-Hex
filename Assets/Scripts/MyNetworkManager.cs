﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MyNetworkManager : NetworkManager {
    
   /* private void OnFailedToConnect (NetworkConnectionError error) {
        Debug.Log ("Test");
    }*/









    private void OnFailedToConnect (NetworkIdentity error) {
        Debug.Log ("Test");
    }







    static public void StartNewHost () {
        singleton.StartHost ();
    }
    static public void StartNewClient () {
        singleton.StartClient ();
    }

    private void OnConnectedToServer () {
        Debug.Log ("Success");
    }


    // Use this for initialization
    void Start () {

        //singleton
        //singleton.StartClient ();
       // singleton.StartHost (

    }
	
	// Update is called once per frame
	void Update () {
        
    }
}
