using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPoolEditor : GOUI {

    static public CardPoolEditor instance;

    static public CardPoolClass EditedCardPool;

    static public GameObject BackgroundObject;

    static public int TileType;
    static public int Owner;
    static public int Value;
    static public int TokenType;

    static int [] NumberOfButtons = new int [] { 4, 8, 20, 2, 50 };
    static int [] Selected = new int [] { 0, 1, 1, 0, 0 };
    static GameObject [] [] Buttons;

    private void Start () {
        instance = this;
        CreateCardPoolEditorMenu ();
        CurrentGUI = this;

        EditedCardPool = new CardPoolClass ();

        CreateSampleCards ();
        //EditedCardPool.EnableVisualisation ();
        //EditedCardPool.LoadFromFile (1);
    }

    static public void CreateSampleCards () {
        for (int y = 0; y < 4; y++) {
            for (int x = 0; x < 5; x++) {
                int number = x + y * 5;
                VisualCard card = new VisualCard ();
                card.Anchor.transform.SetParent (GOUI.CurrentCanvas.transform);
                card.Anchor.transform.localPosition = new Vector3 (-1.7f + x * 1.3f, 3.1f - 1.4f * y, 5);
                card.Anchor.transform.localEulerAngles = new Vector3 (-90, 0, 0);
                if (number < 11) {
                    card.SetState (Random.Range (0, 8), Random.Range (1, 6), Random.Range (0, 2) * 3, number);
                } else {
                    card.SetState (Random.Range (0, 8), Random.Range (1, 6), Random.Range (0, 2) * 3, Random.Range (0, 7));
                }
            }
        }
    }

    static public void ShowCardPoolEditorMenu () {
        DestroyMenu ();
        CurrentCanvas.AddComponent<CardPoolEditor> ();
    }

    static public void CreateCardPoolEditorMenu () {

        Buttons = new GameObject [NumberOfButtons.Length] [];
        for (int x = 0; x < NumberOfButtons.Length; x++) {
            Buttons [x] = new GameObject [NumberOfButtons [x]];
        }

        int sx = 345;
        int sy = 150;

        int px = sx / 2;
        int py = sy / 2 + 30;

        int maxX = 4;
        int maxY = 1;

        int dy = 50;


        AddButtons (px, py, maxX, maxY);

        maxY += 1;

        py += 150 + dy;

        AddButtons (px, py, maxX, maxY);

        maxY += 3;

        py += 270 + dy;

        AddButtons (px, py, maxX, maxY);

        py += 270 + dy;

        maxY = 2;

        AddButtons (px, py, maxX, maxY);

        maxX = 17;

        sx = 90 + 60 * maxX;
        px += sx / 2 + px - 30;
        //py += 150 + dy;

        AddButtons (px, py, maxX, maxY);

        for (int x = 1; x < 5; x++) {
            SelectButton (x, Selected [x]);
        }


    }

    static public void SelectButton (int type, int number) {
        Selected [type] = number;
        foreach (GameObject button in Buttons [type]) {
            if (button != null) {
                button.GetComponent<UIController> ().FreeAndUnlcok ();
            }
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
                Clone = CreateText ("Token Value", px, py - sy / 2 + dy, 12, 0.02f);
                break;
            case 2:
                Clone = CreateText ("Token Type", px, py - sy / 2 + dy, 12, 0.02f);
                break;
            case 3:
                Clone = CreateText ("Ability Area", px, py - sy / 2 + dy, 12, 0.02f);
                break;
            case 4:
                Clone = CreateText ("Ability Type", px, py - sy / 2 + dy, 12, 0.02f);
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
                            BackgroundObject.name = "SaveBoard";
                            break;
                        case 1:
                            BackgroundObject = CreateSprite ("UI/Butt_S_SetList", npx, npy, 11, 60, 60, false);
                            BackgroundObject.name = "LoadBoard";
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
                    if (type == 3) {
                        BackgroundObject = CreateSprite ("UI/Butt_M_EmptySquare", npx + (x * 2 + 1) * 30, npy + (y * 2 + 1) * 30, 11, 120, 120, false);
                    } else {
                        BackgroundObject = CreateSprite ("UI/Butt_M_EmptySquare", npx, npy, 11, 60, 60, false);
                    }
                    switch (type) {
                        case 1:
                            BackgroundObject.name = "BoardEditorValue";
                            Clone = CreateText ((number + 1).ToString (), npx, npy, 12, 0.03f);
                            AddTextToGameObject (BackgroundObject, Clone);
                            break;
                        case 2:
                            BackgroundObject.name = "BoardEditorTokenType";
                            VT = new VisualToken ();
                            Clone = VT.Anchor;
                            Clone.transform.SetParent (BackgroundObject.transform);
                            Clone.transform.localEulerAngles = new Vector3 (-90, 0, 0);
                            Clone.transform.localPosition = Vector3.zero;
                            Clone.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
                            VT.SetType (number);
                            DestroyImmediate (VT.Text);
                            break;
                        case 4:
                            Clone = CreateSprite (VisualCard.GetIconPath (number), npx, npy, 12, 45, 45, false);
                            Clone.GetComponent<SpriteRenderer> ().color = VisualCard.GetAbilityColor (number);
                            //Clone.GetComponent<SpriteRenderer> ().color = AppDefaults.PlayerColor [x];
                            Destroy (Clone.GetComponent<Collider> ());
                            /*Clone = GameObject.CreatePrimitive (PrimitiveType.Quad);
                            Clone.GetComponent<Renderer> ().material.shader = Shader.Find ("Sprites/Default");
                            Clone.transform.SetParent (BackgroundObject.transform);
                            Clone.transform.localEulerAngles = Vector3.zero;
                            Clone.transform.localPosition = Vector3.zero;
                            Clone.transform.localScale = new Vector3 (0.4f, 0.2f, 0.4f);
                            VisualCard.SetAbilityIconInObject (Clone, number);*/
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
