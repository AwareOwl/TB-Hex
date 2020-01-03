using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppDefaults {

    static public Color green = new Color (0, 0.95f, 0);
    static public Color red = new Color (1f, 0f, 0f);
    static public Color darkRed = new Color (0.95f, 0.05f, 0f);
    static public Color yellow = new Color (1, 1, 0);
    static public Color blue = new Color (0.2f, 0.2f, 1);
    static public Color purple = new Color (0.85f, 0.05f, 1);
    static public Color teal = new Color (0.0f, 0.95f, 0.75f);

    static public int availableTokens = 24; // Always + 1, to include null
    static public int availableAbilities = 76; // Always + 1, to include null
    static public int availableAvatars = 23; // Always + 1, to include null
    static public int availableBosses = 13; // Always + 1, to include null

    static public Sprite [] avatar;
    static public Sprite [] bossAvatar;
    static public Texture [] cloud = new Texture [5];

    static public Shader sprite;
    static public Object Tile;
    static public Object DetailedTile;
    static public Object ChalliceTile;
    static public Object LilyLeaf;
    static public Object LilyFlower;
    static public Object Honeycomb;
    static public Object SmoothTile;
    static public Object Grass;
    static public Object Moutain;

    static public Object Socket;
    static public Object Focus;
    static public Object Snake;
    static public Object Currency;
    static public Object Cannon;

    static public float TokenSpawnHeight = 10f;

    static public Color [] PlayerColor;

    static AppDefaults () {
        LoadPlayerColors ();
        LoadAvatars ();
        LoadBosses ();
        LoadClouds ();
        Tile = Resources.Load ("Prefabs/Tile");
        DetailedTile = Resources.Load ("Prefabs/PreDetailedTile");
        ChalliceTile = Resources.Load ("Prefabs/ChalliceTile");
        LilyLeaf = Resources.Load ("Prefabs/PreLilyLeaf");
        LilyFlower = Resources.Load ("Prefabs/PreLilyFlower");
        Honeycomb = Resources.Load ("Prefabs/PreHoneycomb");
        SmoothTile = Resources.Load ("Prefabs/PreSmoothTile");
        Grass = Resources.Load ("Prefabs/PreGrass");
        Moutain = Resources.Load ("Prefabs/PreMoutain");

        Socket = Resources.Load ("Prefabs/Socket");
        Focus = Resources.Load ("Prefabs/Focus");
        Snake = Resources.Load ("Prefabs/Snake");
        Currency = Resources.Load ("Prefabs/Currency");
        Cannon = Resources.Load ("Prefabs/Cannon");

        sprite = Shader.Find ("Sprites/Default");
    }

    static public void LoadAvatars () {
        avatar = new Sprite [availableAvatars];
        for (int x = 0; x < avatar.Length; x++) {
            string path = "Textures/Avatars/Avatar";
            if (x < 10) {
                path += "0";
            }
            path += x.ToString ();
            avatar [x] = GOUI.GetSprite (path);
        }
    }

    static public void LoadBosses () {
        bossAvatar = new Sprite [availableBosses];
        int count = bossAvatar.Length;
        for (int x = 0; x < count; x++) {
            string path = "Textures/Bosses/Boss";
            if (x < 10) {
                path += "0";
            }
            path += x.ToString ();
            bossAvatar [x] = GOUI.GetSprite (path);
        }
    }

    static public void LoadClouds () {
        for (int x = 1; x < cloud.Length; x++) {
            string path = "Textures/Environment/Cloud";
            if (x < 10) {
                path += "0";
            }
            path += x.ToString ();
            cloud [x] = GOUI.GetTexture (path);
        }
    }

    static public Color GetBorderColorMain (TokenType type) {
        switch (type) {
            // dark
            case TokenType.T0:
            case TokenType.T6:
            case TokenType.T16:
            case TokenType.T17:
                return new Color (0.1f, 0.1f, 0.1f);
            // Yellowish
            case TokenType.T1:
            case TokenType.T12:
                return new Color (0.8f, 0.6f, 0.1f);
            // Darker orange
            case TokenType.T10:
                return new Color (0.55f, 0.275f, 0.1f);
            // Dark yellow
            case TokenType.T2:
            case TokenType.T8:
                return new Color (0.1f, 0.1f, 0.0f);
            // Bright blue
            case TokenType.T13:
                return new Color (0.4f, 0.5f, 1f);
            // Dark blue
            case TokenType.T3:
            case TokenType.T4:
            case TokenType.T5:
            case TokenType.T22:
            case TokenType.T23:
                return new Color (0.0f, 0.05f, 0.2f);
            // Dark green
            case TokenType.T11:
            case TokenType.T7:
                return new Color (0.1f, 0.4f, 0.1f);
            case TokenType.T14:
                return new Color (0.07f, 0.3f, 0.07f);
            case TokenType.T18:
                return new Color (0.05f, 0.05f, 0.4f);
            case TokenType.T19:
                return new Color (0.3f, 0.4f, 0.1f);
            case TokenType.T20:
                return new Color (0.4f, 0, 0);
            case TokenType.T21:
                return new Color (0.2f, 0.15f, 0);
            // Dark pink
                //return new Color (0.6f, 0.4f, 0.3f);
            default:
                return Color.black;
        }
    }
    static public Color GetBorderColorAccent (int type) {
        return GetBorderColorAccent ((TokenType) type);
    }

    static public Color GetBorderColorAccent (TokenType type) {
        switch (type) {
            case TokenType.T2:
                return new Color (0.6f, 0f, 0.1f);
            case TokenType.T3:
                return new Color (0.0f, 0.9f, 0f);
                // Red
            case TokenType.T4:
            case TokenType.T5:
            case TokenType.T11:
            case TokenType.T23:
                return new Color (0.9f, 0, 0);
            case TokenType.T6:
                return new Color (0.7f, 0.7f, 0.7f);
            case TokenType.T7:
            case TokenType.T10:
            case TokenType.T16:
            case TokenType.T21:
                return new Color (0.8f, 0.8f, 0f);
            case TokenType.T14:
                return new Color (0.7f, 0.7f, 0f);
            case TokenType.T8:
                return new Color (0.6f, 0.4f, 0.3f);
            case TokenType.T12:
                return new Color (0.8f, 0.45f, 0.125f);
            case TokenType.T9:
            case TokenType.T13:
                return new Color (0.1f, 0.1f, 0.9f);
            case TokenType.T15:
                return new Color (1, 0.1f, 0.6f);
            case TokenType.T17:
                return new Color (0.5f, 0.5f, 0.5f);
            case TokenType.T18:
                return purple;
            case TokenType.T19:
                return new Color (0.8f, 0.9f, 0.1f);
            case TokenType.T20:
                return new Color (1, 0, 0);
            case TokenType.T22:
                return new Color (0.6f, 0.6f, 0f);
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
    static public Color GetAbilityColor (AbilityType type) {
        return GetAbilityColor ((int) type);
    }

    static public Color GetAbilityColor (int type) {
        switch (type) {
            // Red
            case 1:
            case 8:
            case 12:
            case 21:
            case 24:
            case 41:
            case 44:
            case 57:
                return red;
            case 14:
            case 29:
            case 30:
            case 35:
            case 63:
            case 67:
            case 69:
            case 75:
                return darkRed;
            // Green
            case 2:
            case 18:
            case 25:
            case 31:
            case 39:
            case 48:
            case 53:
                return green;
            // Orange
            case 3:
            case 5:
            case 11:
            case 19:
            case 34:
            case 36:
            case 59:
            case 60:
            case 66:
                return new Color (1, 0.5f, 0);
            // Yellow
            case 6:
            case 9:
            case 13:
            case 15:
            case 23:
            case 26:
            case 27:
            case 28:
            case 32:
            case 43:
            case 52:
            case 54:
            case 55:
            case 56:
            case 65:
            case 68:
            case 74:
                return yellow;
            // Teal
            case 4:
            case 46:
            case 47:
            case 70:
                return teal;
            // Purple
            case 7:
            case 10:
            case 16:
            case 17:
            case 22:
            case 33:
            case 37:
            case 40:
            case 50:
            case 58:
            case 64:
            case 73:
                return purple;
            // Blue
            case 20:
            case 38:
            case 42:
            case 45:
            case 49:
            case 51:
            case 61:
            case 62:
            case 71:
            case 72:
                return blue;
        }
        return Color.white;
    }

}
