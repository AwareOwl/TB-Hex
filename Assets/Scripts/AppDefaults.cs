using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppDefaults {

    static public int AvailableTokens = 2; 
    static public int AvailableAbilities = 13; // Always + 1, to include null

    static public Object Tile;

    static public float TokenSpawnHeight = 10f;

    static public Color [] PlayerColor;

    static AppDefaults () {
        LoadPlayerColors ();
        Tile = Resources.Load ("Prefabs/Tile");
    }
    static public Color GetBorderColorMain (int type) {
        switch (type) {
            case 1:
                return new Color (0.8f, 0.6f, 0.1f);
            case 2:
                return new Color (0.6f, 0.6f, 0.6f);
            case 3:
            case 4:
                return new Color (0.0f, 0.2f, 0.6f);
            case 6:
                return new Color (0.0f, 0.6f, 0.2f);
            case 5:
                return new Color (0.6f, 0.0f, 0.2f);
            case 7:
                return new Color (0.5f, 0.7f, 0.2f);
            default:
                return Color.black;
        }
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

    static public Color GetAbilityColor (int type) {
        switch (type) {
            // Red
            case 1:
            case 8:
            case 12:
                return Color.red;
            // Green
            case 2:
            case 9:
                return new Color (0, 0.95f, 0);
            // Orange
            case 3:
            case 5:
            case 11:
                return new Color (1, 0.5f, 0);
            // Yellow
            case 4:
            case 6:
                return new Color (1, 1, 0);
            // Purple
            case 7:
            case 10:
                return new Color (0.8f, 0, 1);
        }
        return Color.white;
    }

}
