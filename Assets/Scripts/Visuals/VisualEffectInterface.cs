using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffectInterface : MonoBehaviour {

    static public GameObject GetRealAnchor (int x, int y) {
        return InGameUI.GetRealAnchor (x, y);
    }

    static public VisualTile GetTile (int x, int y) {
        return InGameUI.PlayedMatch.Board.tile [x, y].visualTile;
    }

    static public void DelayedCreateRealEffects (VectorInfo info, int abilityType) {
        VisualMatch.instance.CreateRealEffects (info, abilityType);
    }

    static public void CreateRealEffects (VectorInfo info, int abilityType) {
        foreach (TileClass target in info.Triggered1) {
            switch (abilityType) {
                default:
                    CreateRealEffect1 (GetRealAnchor (target.x, target.y), abilityType, true, true);
                    break;
                case 6:
                    CreateRealVectorEffect (GetRealAnchor (target.x, target.y), GetRealAnchor (info.PlayedTokenTile.x, info.PlayedTokenTile.y), abilityType, true, true);
                    break;
                case 13:
                    CreateRealVectorEffect (GetRealAnchor (info.PlayedTokenTile.x, info.PlayedTokenTile.y), GetRealAnchor (target.x, target.y), abilityType, true, true);
                    break;
            }
        }
        foreach (TileClass target in info.Triggered2) {
            switch (abilityType) {
                default:
                    CreateRealEffect2 (GetRealAnchor (target.x, target.y), abilityType, true, true);
                    break;
                case 11:
                    CreateRealVectorEffect (GetRealAnchor (target.x, target.y), GetRealAnchor (info.PlayedTokenTile.x, info.PlayedTokenTile.y), abilityType, true, true);
                    CreateRealVectorEffect (GetRealAnchor (info.PlayedTokenTile.x, info.PlayedTokenTile.y), GetRealAnchor (target.x, target.y), abilityType, true, true);
                    break;
            }
        }
        foreach (TileClass target in info.NotTriggered) {

        }
        foreach (AbilityVector vector in info.TriggeredVector) {
            CreateRealVectorEffect (GetRealAnchor (vector.x, vector.y), GetRealAnchor (vector.pushX, vector.pushY), abilityType, true, true);

        }
    }

    static public void CreateRealEffect1 (GameObject anchor, int effectNumber, bool triggered, bool autoDestroy) {
        Color col = AppDefaults.GetAbilityColor (effectNumber);
        VisualEffectScript VES;
        switch (effectNumber) {
            case 1:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Comet2", col, triggered, autoDestroy);
                VES.SetLastPosition (new Vector3 (0, 10f, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 0.4f, 0));
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Dot", col, triggered, autoDestroy);
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                VES.SetScale (new Vector3 (0.5f, 0.5f, 0.5f));
                VES.SetLastPosition (new Vector3 (0, 9.6f, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (2f, 2f, 2f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                break;
            case 2:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Promote2", col, triggered, autoDestroy);
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 1, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.rotateToCamera = true;
                break;
            case 3:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/DeleteDis", col, triggered, autoDestroy);
                VES.SetLastScale (new Vector3 (0, 1f, 1f));
                VES.SetPosition (new Vector3 (0, 0f, 0));
                VES.transform.localEulerAngles = new Vector3 (90, -40, 0);
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (1.25f, 1, 1));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                break;
            case 4:
                VES = CreateSimpleEffect (anchor, "Textures/Ability/Ability04", col, triggered, autoDestroy);
                VES.SetLastScale (new Vector3 (1f, 1f, 1f));
                VES.SetLastPosition (new Vector3 (0, 0.25f, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 0.75f, 0));
                VES.SetLastScale (new Vector3 (2f, 2f, 2f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.rotateToCamera = true;
                break;
            case 8:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Dot", col, triggered, autoDestroy);
                VES.SetLastScale (new Vector3 (0.5f, 2f, 0.5f));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetPhaseTimer (0.5f);
                VES.SetLastPosition (new Vector3 (0, 10, 0));
                VES.rotateToCamera = true;
                break;
            case 9:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Promote2", col, triggered, autoDestroy);
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 1, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.rotateToCamera = true;
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Promote2", col, triggered, autoDestroy);
                VES.SetLastPosition (new Vector3 (0, 0.5f, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 1.5f, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.rotateToCamera = true;
                break;
            case 10:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Snap1", col, triggered, autoDestroy);
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0.5f, 0f, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Snap2", col, triggered, autoDestroy);
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (-0.5f, 0f, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                break;
            case 12:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Thunder2", col, triggered, autoDestroy);
                VES.SetLastColor (new Color (col.r * 2, col.g, col.b, 1));
                VES.SetLastPosition (new Vector3 (0, 5, 0));
                VES.SetScale (new Vector3 (1, 10, 1));
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r * 2, col.g, col.b, 0));
                VES.rotateToCameraVertical = true;
                break;
        }
    }

    static public void CreateRealEffect2 (GameObject anchor, int effectNumber, bool triggered, bool autoDestroy) {
        Color col = AppDefaults.GetAbilityColor (effectNumber);
        VisualEffectScript VES;
        switch (effectNumber) {
            case 2:
                VES = CreateSimpleEffect (anchor, "Textures/Ability/Ability02", col, triggered, autoDestroy);
                VES.SetLastScale (new Vector3 (0.8f, 0.8f, 0.8f));
                VES.SetLastPosition (new Vector3 (0, 0.25f, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 0.75f, 0));
                VES.SetLastScale (new Vector3 (1.1f, 1.1f, 1.1f));
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastScale (new Vector3 (1.8f, 1.8f, 1.8f));
                VES.rotateToCamera = true;
                break;
            case 8:
                VES = CreateSimpleEffect (anchor, "Textures/Ability/Ability08", col, triggered, autoDestroy);
                VES.SetLastScale (new Vector3 (1, 1, 1));
                VES.SetLastPosition (new Vector3 (0, 10, 0));
                VES.AddPhase ();
                VES.SetPhaseTimer (1);
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetLastPhaseTimer (0.5f);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.rotateToCameraVertical = true;
                break;
        }
    }

    static public void CreateRealVectorEffect (GameObject anchor, GameObject lookAtGameObject, int effectNumber, bool triggered, bool autoDestroy) {
        Color col = AppDefaults.GetAbilityColor (effectNumber);
        VisualEffectScript VES;
        switch (effectNumber) {
            case 5:
                CreateEffectPointingAt (anchor, lookAtGameObject.transform.position, effectNumber, triggered, autoDestroy);
                break;
            case 6:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Dot", col, triggered, autoDestroy);
                VES.SetJumpAnimation (true);
                VES.SetLastScale (new Vector3 (1f, 1f, 1f));
                VES.SetLastPosition (new Vector3 (0, 0f, 0));
                VES.AddPhase ();
                VES.SetLastPosition (lookAtGameObject.transform.position - anchor.transform.position);
                VES.AddPhase ();
                VES.rotateToCamera = true;
                VES.SetLastScale (new Vector3 (2f, 2f, 2f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                break;
            case 11:
                CreateEffectPointingAt (anchor, lookAtGameObject.transform.position, 5, triggered, autoDestroy);
                break;
            case 13:
                VES = CreateEffectPointingAt (anchor, lookAtGameObject.transform.position, "Textures/Effects/Moon", col, triggered, autoDestroy);
                VES.SetLastPosition (new Vector3 (0, 0f, 0));
                VES.AddPhase ();
                VES.SetLastPosition (lookAtGameObject.transform.position - anchor.transform.position);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                break;
        }
    }


    static public VisualEffectScript CreateEffectPointingAt (GameObject anchor, Vector3 lookAtPosition, int effectNumber, bool triggered, bool autoDestroy) {
        return CreateEffectPointingAt (anchor, lookAtPosition, VisualCard.GetIconPath (effectNumber), AppDefaults.GetAbilityColor (effectNumber), triggered, autoDestroy);
    }

    static public VisualEffectScript CreateEffectPointingAt (GameObject anchor, Vector3 lookAtPosition, string path, Color color, bool triggered, bool autoDestroy) {
        GameObject Clone = CreateEffect (anchor, path, color, triggered, autoDestroy);
        Clone.transform.LookAt (lookAtPosition);
        Clone.transform.localEulerAngles += new Vector3 (-90, -90, 0);
        return Clone.GetComponent<VisualEffectScript> ();
    }

    static public void CreateEffect1 (GameObject anchor, int effectNumber, bool triggered, bool autoDestroy) {
        CreateEffect (anchor, VisualCard.GetIconPath (effectNumber), AppDefaults.GetAbilityColor (effectNumber), triggered, autoDestroy);
    }
    static public void CreateEffect2 (GameObject anchor, int effectNumber, bool triggered, bool autoDestroy) {
        int effectTextureNumber;
        switch (effectNumber) {
            case 5:
            case 8:
            case 11:
                effectTextureNumber = 8;
                break;
            default:
                effectTextureNumber = effectNumber;
                break;
        }
        CreateEffect (anchor, VisualCard.GetIconPath (effectTextureNumber) + "b", AppDefaults.GetAbilityColor (effectNumber), triggered, autoDestroy);
    }

    static public GameObject CreateEffect (GameObject anchor, string texturePath, Color effectColor, bool triggered, bool autoDestroy) {
        GameObject Clone = GameObject.CreatePrimitive (PrimitiveType.Quad);
        DestroyImmediate (Clone.GetComponent<Collider> ());
        Renderer renderer = Clone.GetComponent<Renderer> ();
        renderer.material.shader = Shader.Find ("Sprites/Default");
        renderer.material.mainTexture = Resources.Load (texturePath) as Texture;
        VisualEffectScript VES = Clone.AddComponent<VisualEffectScript> ();
        Color col = effectColor;
        VES.Init (col, 1, triggered, autoDestroy);
        VES.basicColor [0] = new Color (col.r, col.g, col.b, 0);
        Clone.transform.parent = anchor.transform;
        Clone.transform.localPosition = new Vector3 (0, 0, 0);
        Clone.transform.localEulerAngles = new Vector3 (90, 0, 0);
        Clone.transform.localScale = new Vector3 (1, 1, 1);
        return Clone;
    }

    static public VisualEffectScript CreateSimpleEffect (GameObject anchor, string texturePath, Color effectColor, bool triggered, bool autoDestroy) {
        GameObject Clone = GameObject.CreatePrimitive (PrimitiveType.Quad);
        DestroyImmediate (Clone.GetComponent<Collider> ());
        Renderer renderer = Clone.GetComponent<Renderer> ();
        renderer.material.shader = Shader.Find ("Sprites/Default");
        renderer.material.mainTexture = Resources.Load (texturePath) as Texture;
        VisualEffectScript VES = Clone.AddComponent<VisualEffectScript> ();
        Color col = effectColor;
        VES.Init (col, 0, triggered, autoDestroy);
        Clone.transform.parent = anchor.transform;
        return VES;
    }

}
