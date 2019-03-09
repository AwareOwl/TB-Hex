using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleMenu : GOUI {

    static PageUI pageUI;

	// Use this for initialization
	void Start () {
        DestroyTemplateButtons ();
        CreatePuzzleMenu ();
        CurrentGOUI = this;
    }

    static public void ShowPuzzleMenu () {

    }

    static public void LoadPuzzleMenu () {
        DestroyMenu ();
        CurrentCanvas.AddComponent<PuzzleMenu> ();
    }

    static public void CreatePuzzleMenu () {

        GameObject Clone;

        Clone = CreateSprite ("UI/Panel_Window_01_Sliced", 720, 540, 10, 1440, 1080, false);
        //Clone = CreateSprite ("UI/White", 720, 540, 11, 5, 1080, false);

        Clone = CreateUIText (Language.ListOfPuzzles, 325, 120, 500, 36);
        Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;

        Clone = CreateUIText ("Puzzle #9", 1015, 120, 500, 36);
        Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;

        /*Clone = CreateUIText (Language.PuzzleAbout, 360, 210, 600, 24);
        Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;*/

        Clone = CreateSprite ("UI/Butt_S_Help", 1320, 90, 11, 64, 64, true);
        Clone.name = UIString.SetEditorAbout;

        Clone = CreateSpriteWithText ("UI/Butt_M_EmptySquare", Language.Unfinished, 210, 240, 11, 300, 60);
        Clone = CreateSpriteWithText ("UI/Butt_M_EmptySquare", Language.Finished, 510, 240, 11, 300, 60);

        for (int x = 0; x < 9; x++) {
            Clone = CreateSpriteWithText ("UI/Panel_Slot_01_Sliced", "Puzzle #" + (x + 9).ToString(), 360, 300 + 60 * x, 11, 600, 60);
            GameObject Text;
            Text = Clone.transform.Find ("Text").gameObject;
            Text.transform.parent = CurrentCanvas.transform;
            Text.GetComponent<TextMesh> ().anchor = TextAnchor.MiddleLeft;
            SetInPixPosition (Text, 90, 300 + 60 * x, 12);

            int number = Random.Range (1, 20);
            Clone = CreateSprite (VisualCard.GetIconPath (number), 600, 300 + 60 * x, 12, 45, 45, true);
            Clone.GetComponent<Renderer> ().material.color = AppDefaults.GetAbilityColor (number);
        }

        for (int x = 0; x < 8; x++) {
            for (int y = 0; y < 8; y++) {
                if (Random.Range (0, 2) == 0) {
                    continue;
                }
                GameObject tile;
                GameObject Tile;
                tile = Instantiate (AppDefaults.Tile) as GameObject;
                tile.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
                Tile = tile.transform.Find ("Tile").gameObject;
                Tile.transform.localScale = new Vector3 (0.5f, 0.5f, 0.15f);
                tile.transform.parent = CurrentCanvas.transform;
                tile.transform.localEulerAngles = new Vector3 (-90, 0, 0);
                VisualEffectScript TEffect = Tile.gameObject.AddComponent<VisualEffectScript> ();
                TEffect.SetColor (EnvironmentScript.TileColor (true, 1) * 0.75f);

                Vector3 V3 = VisualTile.TilePosition (x, 0, y);
                SetInPixPosition (tile, 1095 + (int) (V3.x * 60), 540 + (int) (V3.z * 60), 12);

                if (Random.Range (0, 2) == 0) {
                    continue;
                }

                VisualToken VT = new VisualToken ();
                VT.SetState (Random.Range (0, 3), Random.Range (0, 10), Random.Range (1, 7));
                GameObject token = VT.Anchor;
                token.transform.parent = tile.transform;
                token.transform.localPosition = new Vector3 (0, 0.1f, 0);
                token.transform.localScale = new Vector3 (1, 1, 1);
                token.transform.localEulerAngles = new Vector3 (0, 0, 0);


            }
        }

        GameObject pageUIObject = new GameObject ();
        pageUI = pageUIObject.AddComponent<PageUI> ();
        pageUI.Init (10, 16, new Vector2Int (90, 870), "");

        Clone = CreateSprite ("UI/Butt_M_Apply", 105, 975, 11, 90, 90, true);

        Clone = CreateSprite ("UI/Butt_M_Discard", 615, 975, 11, 90, 90, true);
        Clone.name = UIString.ShowMainMenu;

        for (int x = 0; x < 4; x++) {
            Clone = CreateSprite ("UI/Panel_Slot_01_CollectionCard", 915 + 120 * x, 930, 11, 120, 150, false);
            VisualCard VC = SetEditor.LoadCard (new CardClass (Random.Range (1, 5), Random.Range (1, 10), Random.Range (1, 5), Random.Range (1, 30)));

            SetInPixPosition (VC.Anchor, 915 + 120 * x, 925, 12);
        }

    }
}
