using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    static Vector3 DesiredPosition;

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

    static public void SetBoardEditorCamera () {
        DesiredPosition = new Vector3 (-1.7f, 8, -6);
    }

    static public void SetStandardCamera () {
        DesiredPosition = new Vector3 (0, 8, -6);
    }

    static public void SetOfflineCamera () {
        DesiredPosition = new Vector3 (0, 5, -10);
    }

    // Use this for initialization
    void Awake () {
        SetOfflineCamera ();
	}
	
	// Update is called once per frame
	void Update () {
        float delta = Mathf.Min (Time.deltaTime * 4, 1);
        transform.localPosition = transform.localPosition * (1 - delta) + DesiredPosition * delta;
	}
}
