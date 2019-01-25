using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppDefaults {

    static public Color Green = new Color (0, 0.95f, 0);
    static public Color Red = Color.red;

    static public int AvailableTokens = 10; // Always + 1, to include null
    static public int AvailableAbilities = 30; // Always + 1, to include null

    static public Sprite [] Avatar = new Sprite [5];
    static public Texture [] Cloud = new Texture [5];

    static public Shader sprite;
    static public Object Tile;
    static public Object ChalliceTile;

    static public float TokenSpawnHeight = 10f;

    static public Color [] PlayerColor;

    static AppDefaults () {
        LoadPlayerColors ();
        LoadAvatars ();
        LoadClouds ();
        Tile = Resources.Load ("Prefabs/Tile");
        ChalliceTile = Resources.Load ("Prefabs/ChalliceTile");
        sprite = Shader.Find ("Sprites/Default");
    }

    static public void LoadAvatars () {
        for (int x = 0; x < Avatar.Length; x++) {
            string path = "Textures/Avatars/Avatar";
            if (x < 10) {
                path += "0";
            }
            path += x.ToString ();
            Avatar [x] = GOUI.GetSprite (path);
        }
    }

    static public void LoadClouds () {
        for (int x = 1; x < Avatar.Length; x++) {
            string path = "Textures/Environment/Cloud";
            if (x < 10) {
                path += "0";
            }
            path += x.ToString ();
            Cloud [x] = GOUI.GetTexture (path);
        }
    }

    static public Color GetBorderColorMain (int type) {
        switch (type) {
            case 0:
            case 6:
                return new Color (0.1f, 0.1f, 0.1f);
            case 1:
                return new Color (0.8f, 0.6f, 0.1f);
            case 2:
            case 8:
                return new Color (0.1f, 0.1f, 0.0f);
            case 3:
            case 4:
            case 5:
                return new Color (0.0f, 0.05f, 0.2f);
            case 7:
                return new Color (0.1f, 0.4f, 0.1f);
            case 10:
                return new Color (0.1f, 0.9f, 0.1f);
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
            case 5:
                return new Color (0.9f, 0, 0);
            case 6:
                return new Color (0.7f, 0.7f, 0.7f);
            case 7:
                return new Color (0.8f, 0.8f, 0f);
            case 8:
                return new Color (0.6f, 0.4f, 0.3f);
            case 9:
                return new Color (0.1f, 0.1f, 0.9f);
            case 10:
                return new Color (0.1f, 0.9f, 0.1f);
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
            case 21:
            case 24:
                return Red;
            // Green
            case 2:
            case 9:
            case 18:
            case 25:
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
            case 23:
            case 26:
            case 27:
            case 28:
                return new Color (1, 1, 0);
            // Purple
            case 7:
            case 10:
            case 16:
            case 17:
            case 22:
            case 29:
                return new Color (0.8f, 0, 1);
            // Blue
            case 20:
                return new Color (0.2f, 0.2f, 1);
        }
        return Color.white;
    }

}
