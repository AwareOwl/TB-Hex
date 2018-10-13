﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardEditorMenu : GOUI {

    static public BoardEditorMenu instance;

    static public GameObject BackgroundObject;

    static public int TileType;
    static public int Owner;
    static public int Value;
    static public int TokenType;

    static int [] NumberOfButtons = new int [] { 4, 3, 5, 9, 8 };
    static int [] SelectedButtons = new int [NumberOfButtons.Length];
    static GameObject [] [] Buttons;

    private void Start () {
        instance = this;
        CreateBoardEditorMenu ();
        CameraScript.SetBoardEditorCamera ();
        CurrentGUI = this;
    }

    static public void ShowBoardEditorMenu () {
        DestroyMenu ();
        CurrentCanvas.AddComponent<BoardEditorMenu> ();
        EnvironmentScript.CreateRandomBoard ();
    }

    static public void CreateBoardEditorMenu () {

        Buttons = new GameObject [NumberOfButtons.Length] [];
        for (int x = 0; x < NumberOfButtons.Length; x++) {
            Buttons [x] = new GameObject [NumberOfButtons [x]];
        }

        int sx = 390;
        int sy = 150;

        int px = sx / 2;
        int py = sy / 2 + 30;

        int maxX = 5;
        int maxY = 1;

        int dy = 50;
     

        AddButtons (px, py, maxX, maxY);

        py += 120 + dy;

        AddButtons (px, py, maxX, maxY);

        py += 120 + dy;

        AddButtons (px, py, maxX, maxY);

        maxY += 1;

        py += 150 + dy;

        AddButtons (px, py, maxX, maxY);

        maxY += 1;

        py += 210 + dy;

        AddButtons (px, py, maxX, maxY);

        for (int x = 1; x < 5; x++) {
            SelectButton (x, 0);
        }


    }

    static public void SelectButton (int type, int number) {
        SelectedButtons [type] = number;
        foreach (GameObject button in Buttons [type]) {
            button.GetComponent<UIController> ().FreeAndUnlcok ();
        }
        Buttons [type] [number].GetComponent<UIController> ().PressAndLock ();
    }

    static int type = 0;

    static void AddButtons (int px, int py, int maxX, int maxY) {

        GameObject Clone;
        VisualToken VT;

        int sx = 90 + 60 * maxX;
        int sy = 140 + 60 * maxY;
        int dy = 70;

        BackgroundObject = CreateSprite ("UI/Panel_Window_01_Sliced", px, py, 10, sx, sy, false);
        switch (type) {

            case 0:
                Clone = CreateText ("File", px, py - sy / 2 + dy, 12, 0.02f);
                break;
            case 1:
                Clone = CreateText ("Tile filling", px, py - sy / 2 + dy, 12, 0.02f);
                break;
            case 2:
                Clone = CreateText ("Owner", px, py - sy / 2 + dy, 12, 0.02f);
                break;
            case 3:
                Clone = CreateText ("Token value", px, py - sy / 2 + dy, 12, 0.02f);
                break;
            case 4:
                Clone = CreateText ("Token type", px, py - sy / 2 + dy, 12, 0.02f);
                break;
        }

        for (int y = 0; y < maxY; y++) {
            for (int x = 0; x < maxX; x++) {
                int number = x + y * maxX;
                if (number >= NumberOfButtons [type]) {
                    continue;
                }
                int npx = px + x * 60 + 75 - sx / 2;
                int npy = py + y * 60 + 125 - sy / 2;
                if (type == 0) {
                    switch (x) {
                        case 0:
                            BackgroundObject = CreateSprite ("UI/Butt_M_Apply", npx, npy, 11, 60, 60, false);
                            break;
                        case 1:
                            BackgroundObject = CreateSprite ("UI/Butt_S_SetList", npx, npy, 11, 60, 60, false);
                            break;
                        case 2:
                            BackgroundObject = CreateSprite ("UI/Butt_S_Delete", npx, npy, 11, 60, 60, false);
                            break;
                        case 3:
                            BackgroundObject = CreateSprite ("UI/Butt_M_Discard", npx, npy, 11, 60, 60, false);
                            break;
                    }
                }
                if (type > 0) {
                    BackgroundObject = CreateSprite ("UI/Butt_M_EmptySquare", npx, npy, 11, 60, 60, false);
                    switch (type) {
                        case 1:
                            BackgroundObject.name = "BoardEditorTileType";
                            GameObject tile = Instantiate (AppDefaults.Tile) as GameObject;
                            tile.transform.SetParent (BackgroundObject.transform);
                            tile.transform.localEulerAngles = new Vector3 (-90, 0, 0);
                            tile.transform.localPosition = Vector3.zero;
                            tile.transform.localScale = new Vector3 (0.4f, 0.2f, 0.4f);
                            switch (x) {
                                case 0:
                                    tile.transform.Find ("Tile").gameObject.AddComponent<VisualEffectScript> ().Init (new Color (0.2f, 0.2f, 0.2f), false, true);
                                    break;
                                case 1:
                                    tile.transform.Find ("Tile").gameObject.AddComponent<VisualEffectScript> ().Init (new Color (0.6f, 0.6f, 0.6f), false, true);
                                    break;
                                case 2:
                                    tile.transform.Find ("Tile").gameObject.AddComponent<VisualEffectScript> ().Init (new Color (0.6f, 0.6f, 0.6f), false, true);
                                    VT = new VisualToken ();
                                    Clone = VT.Anchor;
                                    Clone.transform.SetParent (tile.transform);
                                    Clone.transform.localEulerAngles = new Vector3 (0, 0, 0);
                                    Clone.transform.localPosition = /*Vector3.zero;*/ new Vector3 (0, 0.3f, 0);
                                    Clone.transform.localScale = Vector3.one;
                                    DestroyImmediate (VT.Text);
                                    break;
                            }
                            break;
                        case 2:
                            BackgroundObject.name = "BoardEditorOwner";
                            Clone = CreateSprite ("UI/White", npx, npy, 12, 30, 30, false);
                            Clone.GetComponent<SpriteRenderer> ().color = AppDefaults.PlayerColor [x];
                            Destroy (Clone.GetComponent<Collider> ());
                            break;
                        case 3:
                            BackgroundObject.name = "BoardEditorValue";
                            Clone = CreateText ((number + 1).ToString(), npx, npy, 12, 0.03f);
                            AddTextToGameObject (BackgroundObject, Clone);
                            break;
                        case 4:
                            BackgroundObject.name = "BoardEditorTokenType";
                            VT = new VisualToken ();
                            Clone = VT.Anchor;
                            Clone.transform.SetParent (BackgroundObject.transform);
                            Clone.transform.localEulerAngles = new Vector3 (-90, 0, 0);
                            Clone.transform.localPosition = Vector3.zero;
                            Clone.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
                            VT.SetTokenType (number);
                            DestroyImmediate (VT.Text);
                            break;
                    }

                }
                BackgroundObject.GetComponent<UIController> ().number = number;
                Buttons [type] [number] = BackgroundObject;

            }
        }
        type++;
    }
}