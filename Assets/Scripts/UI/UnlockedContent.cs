using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockedContent : GOUI {

    static public bool cashed;

    static int redirection;

    static bool [] unlockedAbility;
    static bool [] unlockedToken;

    static List <int> unlockedAbilityId = new List<int> ();
    static List<int> unlockedTokenId = new List<int> ();

    // Use this for initialization
    void Start () {
        CreateUnlockedContentMenu ();
        CurrentGOUI = this;
    }

    static public void ShowUnlockedContent () {
        ClientLogic.MyInterface.CmdDownloadDataToUnlockedContentMenu ();
    }

    static public void LoadUnlockedContentMenu (bool [] unlockedAbility, bool [] unlockedToken) {
        UnlockedContent.unlockedAbility = unlockedAbility;
        UnlockedContent.unlockedToken = unlockedToken;
        DestroyMenu ();
        CurrentCanvas.AddComponent<UnlockedContent> ();
    }

    static public void LoadUnlockedContentData (int [] unlockedAbilityId, int [] unlockedTokenId) {
        if (!cashed) {
            UnlockedContent.unlockedAbilityId = new List<int> ();
            UnlockedContent.unlockedTokenId = new List<int> ();
            cashed = true;
        }
        int abilityCount = unlockedAbilityId.Length;
        for (int x = 0; x < abilityCount; x++) {
            UnlockedContent.unlockedAbilityId.Add (unlockedAbilityId [x]);
        }
        int tokenCount = unlockedTokenId.Length;
        for (int x = 0; x < tokenCount; x++) {
            UnlockedContent.unlockedTokenId.Add (unlockedTokenId [x]);
        }

    }

    static public void LoadUnlockedContentMenu (int redirection) {
        UnlockedContent.redirection = redirection;
        DestroyMenu ();
        CurrentCanvas.AddComponent<UnlockedContent> ();
    }


    static public void CreateUnlockedContentMenu () {

        int abilityCount;
        int abilityRows;
        int tokenCount;
        int tokenRows;

        if (cashed) {
            abilityCount = unlockedAbilityId.Count;
            tokenCount = unlockedTokenId.Count;
        } else {
            abilityCount = AppDefaults.AvailableAbilities;
            tokenCount = AppDefaults.AvailableTokens;
        }

        if (abilityCount == 0 && tokenCount == 0) {
            return;
        }


        int shift = 0;

        if (abilityCount > 0) {
            abilityRows = (abilityCount - 1) / 10 + 1;
            shift++;
        } else {
            abilityRows = 0;
        }
        if (tokenCount > 0) {
            tokenRows = (tokenCount - 1) / 10 + 1;
            shift++;
        } else {
            tokenRows = 0;
        }

        GameObject Clone;
        GameObject BackgroundObject;

        int height = 280 + 60 * abilityRows + 60 * tokenRows + shift * 130;
        Clone = CreateSprite ("UI/Panel_Window_01_Sliced", 720, 540, 10, 840, height, false);

        int px = 375;
        int py = 520 - (60 * abilityRows + 60 * tokenRows + shift * 130) / 2;

        if (abilityRows > 0) {
            if (abilityCount > 1) {
                Clone = CreateUIText (Language.UnlockedAbilities + ":", 720, py, 600, 36);
            } else {
                Clone = CreateUIText (Language.UnlockedAbility + ":", 720, py, 600, 36);
            }
            Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;
        }


        for (int y = 0; y < abilityRows; y++) {
            for (int x = 0; x < 10; x++) {
                int number = y * 10 + x;
                if (number >= abilityCount) {
                    continue;
                }
                if (cashed) {
                    number = unlockedAbilityId [number];
                }
                int npx = px + x * 60 + 75;
                int npy = py + y * 60 + 90;
                BackgroundObject = CreateSprite ("UI/Butt_M_EmptySquare", npx, npy, 11, 60, 60, true);
                UIController UIC = BackgroundObject.GetComponent<UIController> ();
                UIC.abilityType = number;
                //BackgroundObject.name = UIString.CardPoolEditorAbilityType;
                if (cashed || unlockedAbility [number]) {
                    Clone = CreateSprite (VisualCard.GetIconPath (number), npx, npy, 12, 45, 45, false);
                    Clone.GetComponent<SpriteRenderer> ().color = AppDefaults.GetAbilityColor (number);
                    Destroy (Clone.GetComponent<Collider> ());
                } else {
                    Clone = CreateSprite ("Textures/Other/Lock", npx, npy, 12, 45, 45, false);
                    Destroy (Clone.GetComponent<Collider> ());
                }
            }
        }

        if (abilityRows > 0) {
            shift = 1;
        } else {
            shift = 0;
        }

        if (tokenRows > 0) {
            py += 60 * abilityRows + shift * 130;

            if (tokenCount > 1) {
                Clone = CreateUIText (Language.UnlockedTokens + ":", 720, py, 600, 36);
            } else {
                Clone = CreateUIText (Language.UnlockedToken + ":", 720, py, 600, 36);
            }
            Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;
        }


        for (int y = 0; y < tokenRows; y++) {
            for (int x = 0; x < 10; x++) {
                int number = y * 10 + x;
                if (number >= tokenCount) {
                    continue;
                }
                if (cashed) {
                    number = unlockedTokenId [number];
                }
                int npx = px + x * 60 + 75;
                int npy = py + y * 60 + 90;
                BackgroundObject = CreateSprite ("UI/Butt_M_EmptySquare", npx, npy, 11, 60, 60, true);
                UIController UIC = BackgroundObject.GetComponent<UIController> ();
                UIC.tokenType = number;

                //BackgroundObject.name = UIString.CardPoolEditorTokenType;
                if (cashed || unlockedToken [number]) {
                    VisualToken VT;

                    VT = new VisualToken ();
                    Clone = VT.Anchor;
                    Clone.transform.SetParent (BackgroundObject.transform);
                    Clone.transform.localEulerAngles = new Vector3 (-90, 0, 0);
                    Clone.transform.localPosition = Vector3.zero;
                    Clone.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
                    VT.SetType (number);
                    DestroyImmediate (VT.Text);
                } else {
                    Clone = CreateSprite ("Textures/Other/Lock", npx, npy, 12, 45, 45, false);
                    Destroy (Clone.GetComponent<Collider> ());
                }
            }
        }

        if (tokenRows > 0) {
            py += 60 * tokenRows + 170;
        } else {
            py += 60 * abilityRows + 170;
        }

        if (cashed) {
            Clone = CreateSprite ("UI/Butt_M_Apply", 1020, py, 11, 90, 90, true);
            switch (redirection) {
                case 0:
                    Clone.name = UIString.ShowMainMenu;
                    break;
                case 1:
                    Clone.name = UIString.ShowPuzzleMenu;
                    break;
            }
        } else {
            Clone = CreateSprite ("UI/Butt_M_Discard", 1020, py, 11, 90, 90, true);
            Clone.name = UIString.ShowProfileMenu;
        }

        cashed = false;
    }

}
