using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour {

    static public ResourcesManager Instance;
    public GameObject BackgroundTheme7;
    public GameObject BackgroundTheme8;
    public GameObject BackgroundTheme9;
    public GameObject TileTheme7;
    public GameObject TileTheme8;
    public GameObject [] TileTheme9;

    // Use this for initialization
    void OnEnable () {
        DontDestroyOnLoad (transform.root);
        Instance = this;
    }
    public GameObject GetBackgroundTheme (int theme) {
        switch (theme) {
            case 7:
                return BackgroundTheme7;
            case 8:
                return BackgroundTheme8;
            case 9:
                return BackgroundTheme9;
        }
        return null;
    }

    public GameObject GetTileTheme (int theme) {
        switch (theme) {
            case 7:
                return TileTheme7;
            case 8:
                return TileTheme7;
            case 9:
                return TileTheme9 [Random.Range (0, TileTheme9.Length)];
        }
        return null;
    }
}
