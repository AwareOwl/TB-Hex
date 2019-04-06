using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockedContent : GOUI {

    static bool [] unlockedToken;
    static bool [] unlockedAbility;

    // Use this for initialization
    void Start () {
        CreateUnlockedContentMenu ();
        CurrentGOUI = this;
    }

    static public void ShowUnlockedContent () {
        ClientLogic.MyInterface.CmdDownloadDataToUnlockedContentMenu ();
    }

    static public void LoadUnlockedContentMenu (bool [] unlockedAbility, bool [] unlockedToken) {
        DestroyMenu ();
        UnlockedContent.unlockedToken = unlockedToken;
        UnlockedContent.unlockedAbility = unlockedAbility;
        CurrentCanvas.AddComponent<UnlockedContent> ();
    }

    static public void CreateUnlockedContentMenu () {
        GameObject Clone;
        GameObject BackgroundObject;
        Clone = CreateSprite ("UI/Panel_Window_01_Sliced", 720, 540, 10, 840, 960, false);

        int px = 375;
        int py = 180;

        Clone = CreateUIText (Language.UnlockedAbilities + ":", 720, py, 600, 36);
        Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;

        for (int y = 0; y < 5; y++) {
            for (int x = 0; x < 10; x++) {
                int number = y * 10 + x;
                if (number >= AppDefaults.AvailableAbilities) {
                    continue;
                }
                int npx = px + x * 60 + 75;
                int npy = py + y * 60 + 90;
                BackgroundObject = CreateSprite ("UI/Butt_M_EmptySquare", npx, npy, 11, 60, 60, true);
                UIController UIC = BackgroundObject.GetComponent<UIController> ();
                UIC.abilityType = number;
                //BackgroundObject.name = UIString.CardPoolEditorAbilityType;
                if (unlockedAbility [number]) {
                    Clone = CreateSprite (VisualCard.GetIconPath (number), npx, npy, 12, 45, 45, false);
                    Clone.GetComponent<SpriteRenderer> ().color = AppDefaults.GetAbilityColor (number);
                    Destroy (Clone.GetComponent<Collider> ());
                } else {
                    Clone = CreateSprite ("Textures/Other/Lock", npx, npy, 12, 45, 45, false);
                    Destroy (Clone.GetComponent<Collider> ());
                }
            }
        }


        py = 610;

        Clone = CreateUIText (Language.UnlockedTokens + ":", 720, py, 600, 36);
        Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;

        for (int y = 0; y < 2; y++) {
            for (int x = 0; x < 10; x++) {
                int number = y * 10 + x;
                if (number >= AppDefaults.AvailableTokens) {
                    continue;
                }
                int npx = px + x * 60 + 75;
                int npy = py + y * 60 + 90;
                BackgroundObject = CreateSprite ("UI/Butt_M_EmptySquare", npx, npy, 11, 60, 60, true);
                UIController UIC = BackgroundObject.GetComponent<UIController> ();
                UIC.tokenType = number;

                //BackgroundObject.name = UIString.CardPoolEditorTokenType;
                if (unlockedToken [number]) {
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

        Clone = CreateSprite ("UI/Butt_M_Discard", 1020, 900, 11, 90, 90, true);
        Clone.name = UIString.ShowProfileMenu;
    }

}
