using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppDefaults {

    static public Color Green = new Color (0, 0.95f, 0);
    static public Color Red = Color.red;

    static public int AvailableTokens = 5; 
    static public int AvailableAbilities = 21; // Always + 1, to include null


    static public Shader sprite;
    static public Object Tile;

    static public float TokenSpawnHeight = 10f;

    static public Color [] PlayerColor;

    static AppDefaults () {
        LoadPlayerColors ();
        Tile = Resources.Load ("Prefabs/Tile");
        sprite = Shader.Find ("Sprites/Default");
    }

    static public Color GetBorderColorMain (int type) {
        switch (type) {
            case 0:
                return new Color (0.1f, 0.1f, 0.1f);
            case 1:
                return new Color (0.8f, 0.6f, 0.1f);
            case 2:
                return new Color (0.1f, 0.1f, 0.0f);
            case 3:
                return new Color (0.0f, 0.05f, 0.2f);
            case 4:
                return new Color (0.0f, 0.05f, 0.2f);
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

    static public Color GetBorderColorAccent (int type) {
        switch (type) {
            case 2:
                return new Color (0.6f, 0f, 0.1f);
            case 3:
                return new Color (0.0f, 0.9f, 0f);
            case 4:
                return new Color (0.9f, 0, 0);;
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
            case 14:
                return Red;
            // Green
            case 2:
            case 9:
            case 18:
                return Green;
            // Orange
            case 3:
            case 5:
            case 11:
            case 19:
                return new Color (1, 0.5f, 0);
            // Yellow
            case 4:
            case 6:
            case 13:
            case 15:
                return new Color (1, 1, 0);
            // Purple
            case 7:
            case 10:
            case 16:
            case 17:
                return new Color (0.8f, 0, 1);
            // Blue
            case 20:
                return new Color (0.2f, 0.2f, 1);
        }
        return Color.white;
    }

}
