using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockedContent : GOUI {

    const int maxX = 12;

    static public bool cached;

    static int redirection;
    
    static bool [] unlockedAbility;
    static bool [] unlockedToken;

    static List<int> unlockedAvatarId = new List<int> ();
    static List<int> unlockedAbilityId = new List<int> ();
    static List<int> unlockedTokenId = new List<int> ();

    // Use this for initialization
    void Start () {
        CreateUnlockedContentMenu ();
        CurrentGOUI = this;
    }

    static public void ShowUnlockedNewContent () {
        if (cached) {
            LoadUnlockedContentMenu (0);
        }
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

    static public void LoadUnlockedContentData (int [] unlockedAvatarId, int [] unlockedAbilityId, int [] unlockedTokenId) {
        if (!cached) {
            if (unlockedAvatarId.Length == 0 && unlockedAbilityId.Length == 0 && unlockedTokenId.Length == 0) {
                return;
            }
            UnlockedContent.unlockedAvatarId = new List<int> ();
            UnlockedContent.unlockedAbilityId = new List<int> ();
            UnlockedContent.unlockedTokenId = new List<int> ();
            cached = true;
        }
        int abilityCount = unlockedAbilityId.Length;
        for (int x = 0; x < abilityCount; x++) {
            UnlockedContent.unlockedAbilityId.Add (unlockedAbilityId [x]);
        }
        int avatarCount = unlockedAvatarId.Length;
        for (int x = 0; x < avatarCount; x++) {
            UnlockedContent.unlockedAvatarId.Add (unlockedAvatarId [x]);
        }
        int tokenCount = unlockedTokenId.Length;
        for (int x = 0; x < tokenCount; x++) {
            UnlockedContent.unlockedTokenId.Add (unlockedTokenId [x]);
        }

    }

    static public void LoadUnlockedContentMenu (int redirection) {
        if (!cached) {
            return;
        }
        UnlockedContent.redirection = redirection;
        DestroyMenu ();
        CurrentCanvas.AddComponent<UnlockedContent> ();
    }


    static public void CreateUnlockedContentMenu () {

        int avatarCount;
        int avatarRows;
        int abilityCount;
        int abilityRows;
        int tokenCount;
        int tokenRows;

        if (cached) {
            avatarCount = unlockedAvatarId.Count;
            abilityCount = unlockedAbilityId.Count;
            tokenCount = unlockedTokenId.Count;

            if (avatarCount == 0 && abilityCount == 0 && tokenCount == 0) {
                return;
            }

        } else {
            avatarCount = 0;
            abilityCount = AppDefaults.availableAbilities;
            tokenCount = AppDefaults.availableTokens;
        }


        int shift = 0;


        if (avatarCount > 0) {
            avatarRows = (avatarCount - 1) / maxX + 1;
            shift++;
        } else {
            avatarRows = 0;
        }
        if (abilityCount > 0) {
            abilityRows = (abilityCount - 1) / maxX + 1;
            shift++;
        } else {
            abilityRows = 0;
        }
        if (tokenCount > 0) {
            tokenRows = (tokenCount - 1) / maxX + 1;
            shift++;
        } else {
            tokenRows = 0;
        }

        GameObject Clone;
        GameObject BackgroundObject;

        int height = 280 + 60 * (avatarRows + abilityRows + tokenRows) + shift * 130;
        Clone = CreateSprite ("UI/Panel_Window_01_Sliced", 720, 540, 10, 240 + 60 * maxX, height, false);

        int px = 720 - 30 * maxX - 45;
        int py = 520 - (60 * (avatarRows + abilityRows + tokenRows) + shift * 130) / 2;

        if (avatarRows > 0) {

            string text = "";
            if (cached) {
                if (avatarCount > 1) {
                    text = Language.NewAvatarsUnlocked + ":";
                } else {
                    text = Language.NewAvatarUnlocked + ":";
                }
            } else {
                if (avatarCount > 1) {
                    text = Language.UnlockedAvatars + ":";
                } else {
                    text = Language.UnlockedAvatar + ":";
                }
            }
            Clone = CreateUIText (text, 720, py, 60 * maxX, 36);
            Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;
        }

        for (int y = 0; y < avatarRows; y++) {
            for (int x = 0; x < maxX; x++) {
                int number = y * maxX + x;
                if (number >= avatarCount) {
                    continue;
                }
                if (cached) {
                    number = unlockedAvatarId [number];
                }
                int npx = px + x * 60 + 75;
                int npy = py + y * 60 + 90;
                BackgroundObject = CreateSprite ("UI/Butt_M_EmptySquare", npx, npy, 11, 60, 60, true);
                UIController UIC = BackgroundObject.GetComponent<UIController> ();
                UIC.number = number;
                BackgroundObject.name = UIString.Avatar;
                if (cached) {
                    Clone = CreateSprite ("", npx, npy, 12, 45, 45, false);
                    SetSprite (Clone, AppDefaults.avatar [number]);
                    SetSpriteScale (Clone, 60, 60);
                    Destroy (Clone.GetComponent<Collider> ());
                }
            }
        }
        

        if (avatarRows > 0) {
            py += 60 * avatarRows + 130;
        }

        if (abilityRows > 0) {
            string text = "";
            if (cached) {
                if (abilityCount > 1) {
                    text = Language.NewAbilitiesUnlocked + ":";
                } else {
                    text = Language.NewAbilityUnlocked + ":";
                }
            } else {
                if (abilityCount > 1) {
                    text = Language.UnlockedAbilities + ":";
                } else {
                    text = Language.UnlockedAbility + ":";
                }
            }
            Clone = CreateUIText (text, 720, py, 60 * maxX, 36);
            Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;
        }

        for (int y = 0; y < abilityRows; y++) {
            for (int x = 0; x < maxX; x++) {
                int number = y * maxX + x;
                if (number >= abilityCount) {
                    continue;
                }
                if (cached) {
                    number = unlockedAbilityId [number];
                }
                int npx = px + x * 60 + 75;
                int npy = py + y * 60 + 90;
                BackgroundObject = CreateSprite ("UI/Butt_M_EmptySquare", npx, npy, 11, 60, 60, true);
                UIController UIC = BackgroundObject.GetComponent<UIController> ();
                UIC.abilityType = (AbilityType) number;
                //Debug.Log (cashed + " " + y + " " + x);
                if (cached || unlockedAbility [number]) {
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
            py += 60 * abilityRows + 130;
        }

        if (tokenRows > 0) {

            string text = "";
            if (cached) {
                if (tokenCount > 1) {
                    text = Language.NewTokensUnlocked + ":";
                } else {
                    text = Language.NewTokenUnlocked + ":";
                }
            } else {
                if (tokenCount > 1) {
                    text = Language.UnlockedTokens + ":";
                } else {
                    text = Language.UnlockedToken + ":";
                }
            }
            Clone = CreateUIText (text, 720, py, 60 * maxX, 36);
            Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;
        }


        for (int y = 0; y < tokenRows; y++) {
            for (int x = 0; x < maxX; x++) {
                int number = y * maxX + x;
                TokenType tokenType = (TokenType) number;
                if (number >= tokenCount) {
                    continue;
                }
                if (cached) {
                    number = unlockedTokenId [number];
                }
                int npx = px + x * 60 + 75;
                int npy = py + y * 60 + 90;
                BackgroundObject = CreateSprite ("UI/Butt_M_EmptySquare", npx, npy, 11, 60, 60, true);
                UIController UIC = BackgroundObject.GetComponent<UIController> ();
                UIC.tokenType = tokenType;

                //BackgroundObject.name = UIString.CardPoolEditorTokenType;
                if (cached || unlockedToken [number]) {
                    VisualToken VT;

                    VT = new VisualToken ();
                    Clone = VT.Anchor;
                    Clone.transform.SetParent (BackgroundObject.transform);
                    Clone.transform.localEulerAngles = new Vector3 (-90, 0, 0);
                    Clone.transform.localPosition = Vector3.zero;
                    Clone.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
                    VT.SetType (tokenType);
                    DestroyImmediate (VT.Text);
                } else {
                    Clone = CreateSprite ("Textures/Other/Lock", npx, npy, 12, 45, 45, false);
                    Destroy (Clone.GetComponent<Collider> ());
                }
            }
        }

        if (tokenRows > 0) {
            py += 60 * tokenRows + 130;
        }

        py += 40;

        if (cached) {
            Clone = CreateSprite ("UI/Butt_M_Apply", 1020, py, 11, 90, 90, true);
            switch (redirection) {
                case 0:
                    Clone.name = UIString.ShowMainMenu;
                    break;
                case 1:
                    Clone.name = UIString.ShowPuzzleMenu;
                    break;
                case 2:
                    Clone.name = UIString.ShowProfileMenu;
                    break;
                case 3:
                    Clone.name = UIString.ShowBossMenu;
                    break;
                case 4:
                    Clone.name = UIString.ShowTutorialMenu;
                    break;
            }
        } else {
            Clone = CreateSprite ("UI/Butt_M_Discard", 720 + 30 * maxX, py, 11, 90, 90, true);
            Clone.name = UIString.ShowProfileMenu;
        }

        cached = false;
    }

}
