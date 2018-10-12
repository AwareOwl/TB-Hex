using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    static public GameObject CameraObject {
        get {
            return MyCamera.transform.gameObject;
        }
    }
    static public Camera MyCamera {
        get {
            return Camera.main;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}
}
