﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientController : MonoBehaviour {

	// Use this for initialization
	void Start () {

        GOUI.UICanvas = GameObject.Find ("Canvas");
        EnvironmentScript.CreateNewBackground (2);
        
        GOUI.CreateNewCanvas ();
    }
}
