using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppDefaults {

    static public Color Green = new Color (0, 0.95f, 0);
    static public Color Red = Color.red;
    static public Color Yellow = new Color (1, 1, 0);

    static public int AvailableTokens = 13; // Always + 1, to include null
    static public int AvailableAbilities = 39; // Always + 1, to include null
    static public int AvailableAvatars = 10; // Always + 1, to include null

    static public Sprite [] Avatar;
    static public Texture [] Cloud = new Texture [5];

    static public Shader sprite;
    static public Object Tile;
    static public Object ChalliceTile;
    static public Object LilyLeaf;
    static public Object LilyFlower;
    static public Object Honeycomb;
    static public Object SmoothTile;
    static public Object Grass;

    static public float TokenSpawnHeight = 10f;

    static public Color [] PlayerColor;

    static AppDefaults () {
        LoadPlayerColors ();
        LoadAvatars ();
        LoadClouds ();
        Tile = Resources.Load ("Prefabs/Tile");
        ChalliceTile = Resources.Load ("Prefabs/ChalliceTile");
        LilyLeaf = Resources.Load ("Prefabs/PreLilyLeaf");
        LilyFlower = Resources.Load ("Prefabs/PreLilyFlower");
        Honeycomb = Resources.Load ("Prefabs/PreHoneycomb");
        SmoothTile = Resources.Load ("Prefabs/PreSmoothTile");
        Grass = Resources.Load ("Prefabs/PreGrass");

        sprite = Shader.Find ("Sprites/Default");
    }

    static public void LoadAvatars () {
        Avatar = new Sprite [AvailableAvatars];
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
        for (int x = 1; x < Cloud.Length; x++) {
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
            // dark
            case 0:
            case 6:
                return new Color (0.1f, 0.1f, 0.1f);
            // Yellowish
            case 1:
            case 12:
                return new Color (0.8f, 0.6f, 0.1f);
            // Darker orange
            case 10:
                return new Color (0.55f, 0.275f, 0.1f);
            // Dark yellow
            case 2:
            case 8:
                return new Color (0.1f, 0.1f, 0.0f);
            // Bright blue
            case 13:
                return new Color (0.5f, 0.5f, 1f);
            // Dark blue
            case 3:
            case 4:
            case 5:
                return new Color (0.0f, 0.05f, 0.2f);
            // Dark green
            case 11:
            case 7:
                return new Color (0.1f, 0.4f, 0.1f);
            // Dark pink
            case 100:
                return new Color (0.6f, 0.4f, 0.3f);
            // Dark Orange
            //case 13:
                return new Color (0.7f, 0.4f, 0.1f);
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
                // Red
            case 4:
            case 5:
            case 11:
            case 13:
                return new Color (0.9f, 0, 0);
            case 6:
                return new Color (0.7f, 0.7f, 0.7f);
            case 7:
            case 10:
                return new Color (0.8f, 0.8f, 0f);
            case 8:
                return new Color (0.6f, 0.4f, 0.3f);
            case 12:
                return new Color (0.8f, 0.45f, 0.125f);
            case 9:
                return new Color (0.1f, 0.1f, 0.9f);
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
            case 29:
            case 30:
            case 35:
                return Red;
            // Green
            case 2:
            case 18:
            case 25:
            case 31:
                return Green;
            // Orange
            case 3:
            case 5:
            case 11:
            case 19:
            case 34:
            case 36:
                return new Color (1, 0.5f, 0);
            // Yellow
            case 4:
            case 6:
            case 9:
            case 13:
            case 15:
            case 23:
            case 26:
            case 27:
            case 28:
            case 32:
                return Yellow;
            // Purple
            case 7:
            case 10:
            case 16:
            case 17:
            case 22:
            case 33:
            case 37:
                return new Color (0.8f, 0, 1);
            // Blue
            case 20:
            case 38:
            case 40:
                return new Color (0.2f, 0.2f, 1);
        }
        return Color.white;
    }

}
