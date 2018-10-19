using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppDefaults {

    static public Object Tile;

    static public float TokenSpawnHeight = 10f;

    static public Color [] PlayerColor;

    static AppDefaults () {
        LoadPlayerColors ();
        Tile = Resources.Load ("Prefabs/Tile");
    }

    static void LoadPlayerColors () {
        PlayerColor = new Color [7];
        for (int x = 0; x < PlayerColor.Length; x++) {
            PlayerColor [0] = new Color (0.6f, 0.6f, 0.6f);
            PlayerColor [1] = new Color (0.4f, 0.8f, 0.4f);
            PlayerColor [2] = new Color (0.8f, 0.4f, 0.4f);
            PlayerColor [3] = new Color (0.4f, 0.4f, 0.8f);
            PlayerColor [4] = new Color (0.4f, 0.8f, 0.8f);
            PlayerColor [5] = new Color (0.8f, 0.4f, 0.8f);
            PlayerColor [6] = new Color (0.3f, 0.8f, 0.8f);
        }
    }

}
