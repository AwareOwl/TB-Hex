using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffectInterface : MonoBehaviour {
    static public GameObject GetRealAnchor (TileClass tile) {
        return GetRealAnchor (tile.x, tile.y);
    }

    static public GameObject GetRealAnchor (int x, int y) {
        return InGameUI.GetRealAnchor (x, y);
    }

    static public void SetRotateTo (GameObject anchor, GameObject source, GameObject destination) {
        Vector3 delta = destination.transform.position - source.transform.position;
        float rot = - Mathf.Atan2 (delta.z, delta.x) * 180f / Mathf.PI;
        anchor.GetComponent<VisualEffectScript> ().SetRotateTo (new Vector3 (0, rot, 0));
    }

    static public void DelayedRealEffect (int x, int y, int abilityType, bool triggered) {
        VisualMatch.instance.RealEffect (x, y, abilityType, triggered);
    }

    static public void RealEffect (int x, int y, int abilityType, bool triggered) {
        Color col = AppDefaults.GetAbilityColor (abilityType);
        VisualEffectScript VES;
        switch (abilityType) {
            case 7:
                VES = CreateSimpleEffect (GetRealAnchor (x, y), "Textures/Ability/Ability07", col, triggered, true);
                VES.rotateToCamera = true;
                VES.SetLastPosition (new Vector3 (0, 1, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 2, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                break;
            case 38:
                VES = CreateSimpleEffect (GetRealAnchor (x, y), "Textures/Ability/Ability38", col, triggered, true);
                VES.rotateToCamera = true;
                VES.SetLastPosition (new Vector3 (0, 1f, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 2, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPhaseTimer (2f);
                break;
        }
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
                case 15:
                case 30:
                case 32:
                    CreateRealVectorEffect (GetRealAnchor (target.x, target.y), GetRealAnchor (info.PlayedTokenTile.x, info.PlayedTokenTile.y), abilityType, true, true);
                    break;
                case 13:
                    CreateRealVectorEffect (GetRealAnchor (info.PlayedTokenTile.x, info.PlayedTokenTile.y), GetRealAnchor (target.x, target.y), abilityType, true, true);
                    break;
                case 23:
                    CreateRealVectorEffect (GetRealAnchor (info.Triggered2 [0].x, info.Triggered2 [0].y), GetRealAnchor (target.x, target.y), abilityType, true, true);
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
                case 18:
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
                VES.SetLastScale (new Vector3 (0.35f, 2f, 0.35f));
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
            case 14:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Dot", col, triggered, autoDestroy);
                VES.SetLastPosition (new Vector3 (0, 1, 0));
                VES.SetScale (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 0f, 0));
                VES.SetLastScale (new Vector3 (0.75f, 0.75f, 0.75f));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetPhaseTimer (0.75f);
                VES.rotateToCamera = true;
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Dot", col, triggered, autoDestroy);
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                VES.SetLastScale (new Vector3 (0, 0, 0));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (1.5f, 1.5f, 1.5f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetPhaseTimer (0.75f);
                break;
            case 16:
            case 33:
                VES = CreateSimpleEffect (anchor, "UI/White", col, triggered, autoDestroy);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0.4f));
                VES.SetLastPosition (new Vector3 (0, 5, 0));
                VES.SetScale (new Vector3 (0.2f, 10, 0.2f));
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (0.5f, 10, 0.5f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetPhaseTimer (2);
                VES.rotateToCameraVertical = true;
                break;
            case 17:
                VES = CreateSimpleEffect (anchor, "Textures/Ability/Ability17", col, triggered, autoDestroy);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.SetScale (new Vector3 (1, 0.5f, 1));
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (1.2f, 2.25f, 1.2f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                VES.SetPhaseTimer (1.2f);
                break;
            case 18:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Gen1", col, triggered, autoDestroy);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPosition (new Vector3 (-0.5f, 0, 0));
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Gen2", col, triggered, autoDestroy);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPosition (new Vector3 (0.5f, 0, 0));
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                break;
            case 19:
                VES = CreateSimpleEffect (anchor, "Textures/Ability/Ability19", col, triggered, autoDestroy);
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                break;
            case 20:
                VES = CreateSimpleEffect (anchor, "Textures/Ability/Ability20", col, triggered, autoDestroy);
                VES.SetLastScale (new Vector3 (0.8f, 0.8f, 0.8f));
                VES.SetLastPosition (new Vector3 (0, 0.25f, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 0.65f, 0));
                VES.SetLastScale (new Vector3 (1.1f, 1.1f, 1.1f));
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPosition (new Vector3 (0, 0.85f, 0));
                VES.SetLastScale (new Vector3 (1.3f, 1.3f, 1.3f));
                VES.rotateToCamera = true;
                break;
            case 21:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Dagger", col, triggered, autoDestroy);
                VES.SetLastScale (new Vector3 (1, 1, 1));
                VES.SetLastPosition (new Vector3 (0, 0.5f, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 0.7f, 0));
                VES.SetLastPhaseTimer (0.5f);
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, -0.2f, 0));
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPhaseTimer (1f);
                VES.rotateToCameraVertical = true;
                break;
            case 22:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Dot", col, triggered, autoDestroy);
                VES.SetLastScale (new Vector3 (0.4f, 0.4f, 0.4f));
                VES.SetLastPosition (new Vector3 (0, 0.2f, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 1f, 0));
                VES.SetLastScale (new Vector3 (1f, 1f, 1f));
                VES.AddPhase ();
                VES.SetLastColor (new Color (1, 1, 1, 1f));
                VES.AddPhase ();
                VES.SetLastColor (new Color (1, 1, 1, 0f));
                VES.rotateToCamera = true;
                break;
            case 24:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Flames", col, triggered, autoDestroy);
                VES.SetLastScale (new Vector3 (0.6f, 0.1f, 1));
                VES.SetLastPosition (new Vector3 (0, 0.05f, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0.5f));
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (0.8f, 0.8f, 1f));
                VES.SetLastPosition (new Vector3 (0f, 0.45f, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.SetLastPhaseTimer (0.5f);
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (0.6f, 0.4f, 1f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPhaseTimer (1.5f);
                VES.rotateToCamera = true;
                break;
            case 25:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Dot", col, triggered, autoDestroy);
                VES.SetLastScale (new Vector3 (0.4f, 0.4f, 0.4f));
                VES.SetLastPosition (new Vector3 (0, 0f, 0));
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (0.6f, 0.6f, 1f));
                VES.SetLastPosition (new Vector3 (0, 0.5f, 0));
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (0.6f, 0f, 1f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.rotateToCamera = true;
                break;
            case 26:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Promote2", col, triggered, autoDestroy);
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 1, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.rotateToCamera = true;
                break;
            case 27:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Promote2", col, triggered, autoDestroy);
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 1, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.rotateToCamera = true;
                break;
            case 28:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/3gram", col, triggered, autoDestroy);
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                VES.SetLastScale (new Vector3 (1f, 1f, 1));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1f));
                VES.SetRotateVector (new Vector3 (0, 90, 0));
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPhaseTimer (1.75f);
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Flames", col, triggered, autoDestroy);
                VES.SetLastScale (new Vector3 (0.6f, 0.1f, 1));
                VES.SetLastPosition (new Vector3 (0, 0.05f, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0.5f));
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (0.8f, 0.8f, 1f));
                VES.SetLastPosition (new Vector3 (0f, 0.45f, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.SetLastPhaseTimer (0.5f);
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (0.6f, 0.4f, 1f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPhaseTimer (1.5f);
                VES.rotateToCamera = true;
                break;
            case 29:
                VES = CreateSimpleEffect (anchor, "Textures/Ability/Ability29", col, triggered, autoDestroy);
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                VES.SetLastScale (new Vector3 (1f, 1f, 1));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1f));
                VES.SetRotateVector (new Vector3 (0, 45, 0));
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPhaseTimer (1.75f);
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Spike", col, triggered, autoDestroy);
                VES.SetLastColor (new Color (col.r * 2, col.g, col.b, 1));
                VES.SetLastPosition (new Vector3 (0, 2, 0));
                VES.SetScale (new Vector3 (0.4f, 4, 1));
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (0, 4, 1));
                VES.SetLastColor (new Color (col.r * 2, col.g, col.b, 0));
                VES.rotateToCameraVertical = true;
                break;
            case 31:
                VES = CreateSimpleEffect (anchor, "Textures/Ability/Ability31", col, triggered, autoDestroy);
                VES.SetLastScale (new Vector3 (0f, 0f, 0f));
                VES.SetLastPosition (new Vector3 (0.05f, 0f, 0));
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (1, 1, 1));
                VES.SetLastPosition (new Vector3 (0.1f, 0.5f, 0));
                VES.AddPhase ();
                VES.rotateToCamera = true;
                VES.SetLastScale (new Vector3 (1.1f, 1.1f, 1.1f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                break;

            case 35:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Dot", col, triggered, autoDestroy);
                VES.SetLastPosition (new Vector3 (0, 1, 0));
                VES.SetScale (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 0f, 0));
                VES.SetLastScale (new Vector3 (0.75f, 0.75f, 0.75f));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetPhaseTimer (0.75f);
                VES.rotateToCamera = true;
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Dot", col, triggered, autoDestroy);
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                VES.SetLastScale (new Vector3 (0, 0, 0));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (1.5f, 1.5f, 1.5f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetPhaseTimer (0.75f);

                VES = CreateSimpleEffect (anchor, "Textures/Effects/Spike", col, triggered, autoDestroy);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.SetLastPosition (new Vector3 (0, 1, 0));
                VES.SetScale (new Vector3 (0.4f, 0, 1));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 1, 0));
                VES.SetLastScale (new Vector3 (0, 2, 1));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.rotateToCameraVertical = true;
                break;

            case 36:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Ring", col, triggered, autoDestroy);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.SetScale (new Vector3 (1, 1, 1));
                VES.AddPhase ();
                VES.SetPhaseTimer (0.5f);
                VES.SetLastScale (new Vector3 (1.2f, 1.2f, 1.2f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.AddPhase ();
                VES.SetPhaseTimer (1.25f);
                VES.SetLastScale (new Vector3 (1f, 1f, 1));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                break;
            case 37:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Snake", col, triggered, autoDestroy);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.SetScale (new Vector3 (1, 1, 1));
                VES.AddPhase ();
                VES.SetPhaseTimer (2f);
                VES.SetLastScale (new Vector3 (1.2f, 1.2f, 1.2f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetRotateVector (new Vector3 (0, 0, 320));
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
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
                VES.SetLastScale (new Vector3 (1.25f, 1.25f, 1.25f));
                VES.SetLastPosition (new Vector3 (0, 10, 0));
                VES.AddPhase ();
                VES.SetPhaseTimer (1);
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetLastPhaseTimer (0.5f);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.rotateToCameraVertical = true;
                break;
            case 21:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Spike", col, triggered, autoDestroy);
                VES.SetLastScale (new Vector3 (0, 0, 0));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (0.5f, 1.1f, 1.1f));
                VES.SetLastPosition (new Vector3 (0, 0.5f, 0));
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPhaseTimer (1f);
                VES.rotateToCameraVertical = true;
                break;
            case 22:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/HalfMoon", col, triggered, autoDestroy);
                VES.SetLastPosition (new Vector3 (0, 0.6f, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 1f, 0));
                VES.AddPhase ();
                VES.SetLastColor (new Color (1, 1, 1, 1f));
                VES.AddPhase ();
                VES.SetLastColor (new Color (1, 1, 1, 0f));
                VES.rotateToCamera = true;
                break;
            case 26:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Beak", col, triggered, autoDestroy);
                VES.SetLastPosition (new Vector3 (0, 1, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.SetLastColor (new Color (1, 0, 0, 0));
                VES.rotateToCamera = true;
                break;
            case 28:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/3gram", col, triggered, autoDestroy);
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                VES.SetLastScale (new Vector3 (1f, 1f, 1));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1f));
                VES.SetRotateVector (new Vector3 (0, 90, 0));
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPhaseTimer (1.75f);
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Flames", col, triggered, autoDestroy);
                VES.SetLastScale (new Vector3 (0.6f, 0.1f, 1));
                VES.SetLastPosition (new Vector3 (0, 0.05f, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0.5f));
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (0.8f, 0.8f, 1f));
                VES.SetLastPosition (new Vector3 (0f, 0.45f, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.SetLastPhaseTimer (0.5f);
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (0.6f, 0.4f, 1f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPhaseTimer (1.5f);
                VES.rotateToCamera = true;
                break;
            case 30:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Coin", col, triggered, autoDestroy);
                VES.SetLastScale (new Vector3 (0.3f, 0.3f, 0.3f));
                VES.SetLastPosition (new Vector3 (0, 0.2f, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 2f, 0));
                VES.SetLastScale (new Vector3 (0.4f, 0.4f, 0.4f));
                VES.SetLastPhaseTimer (0.5f);
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (0.5f, 0.5f, 0.5f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPhaseTimer (2f);
                VES.rotateToCamera = true;
                break;
            case 33:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Coin", col, triggered, autoDestroy);
                VES.SetLastScale (new Vector3 (0.2f, 0.2f, 0.2f));
                VES.SetLastPosition (new Vector3 (0, 0.2f, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 2f, 0));
                VES.SetLastScale (new Vector3 (0.3f, 0.3f, 0.3f));
                VES.SetLastPhaseTimer (0.5f);
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (0.4f, 0.4f, 0.4f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPhaseTimer (2f);
                VES.rotateToCamera = true;
                VES = CreateSimpleEffect (anchor, "Textures/Effects/OpenedEye", col, triggered, autoDestroy);
                VES.SetLastScale (new Vector3 (1f, 1f, 1f));
                VES.SetLastPosition (new Vector3 (0, 2, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.AddPhase ();
                VES.SetLastPhaseTimer (0.5f);
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.SetLastPhaseTimer (1f);
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (1.2f, 1.2f, 1.2f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPhaseTimer (1f);
                VES.rotateToCamera = true;
                break;
        }
    }

    static public void CreateRealTokenEffect (TileClass sourceTile, int effectNumber) {
        GameObject source = GetRealAnchor (sourceTile);
        Color col = Color.white;
        VisualEffectScript VES;
        switch (effectNumber) {
            case 5:
                col = AppDefaults.Red;
                break;
            case 12:
                col = AppDefaults.Yellow;
                break;
        }
        switch (effectNumber) {
            case 5:
                VES = CreateSimpleEffect (source, "Textures/Effects/Ignite", col, true, true);
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0.4f));
                VES.SetLastScale (new Vector3 (0.8f, 0.8f, 0.8f));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetPhaseTimer (1);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0.2f));
                VES.SetLastScale (new Vector3 (2.1f, 2.1f, 2.1f));
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (2.4f, 2.4f, 2.4f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0.025f));
                VES.SetLastPhaseTimer (5f);
                break;
            case 12:
                VES = CreateSimpleEffect (source, "Textures/Effects/Coin", col, true, true);
                VES.SetLastScale (new Vector3 (0.3f, 0.3f, 0.3f));
                VES.SetLastPosition (new Vector3 (0, 0.2f, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 2f, 0));
                VES.SetLastScale (new Vector3 (0.4f, 0.4f, 0.4f));
                VES.SetLastPhaseTimer (0.5f);
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (1f, 1f, 1f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPhaseTimer (1f);
                VES.rotateToCamera = true;
                break;
        }
    }

    static public void CreateRealTokenVectorEffect (TileClass sourceTile, TileClass destinationTile, int effectNumber) {
        GameObject source = GetRealAnchor (sourceTile);
        GameObject destination = GetRealAnchor (destinationTile);
        Color col = Color.white;
        VisualEffectScript VES;
        switch (effectNumber) {
            case 3:
                col = AppDefaults.Green;
                break;
            case 4:
            case 5:
                col = AppDefaults.Red;
                break;
        }
        switch (effectNumber) {
            case 3:
            case 4:
                VES = CreateSimpleEffect (source, "Textures/Effects/Dot", col, true, true);
                VES.SetJumpAnimation (true);
                VES.SetLastScale (new Vector3 (0.6f, 0.6f, 0.6f));
                VES.SetLastPosition (new Vector3 (0, 0f, 0));
                VES.AddPhase ();
                VES.SetLastPosition (destination.transform.position - source.transform.position);
                VES.AddPhase ();
                VES.rotateToCamera = true;
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetPhaseTimer (1.25f);
                break;
            case 5:
                VES = CreateEffectPointingAt (source, destination.transform.position, "Textures/Effects/ExplosionParticle", col, true, true);
                VES.SetLastScale (new Vector3 (0.6f, 0.6f, 0.6f));
                VES.SetLastPosition (new Vector3 (0, 0f, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1.2f));
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (0.8f, 0.8f, 0.8f));
                VES.SetLastPosition (destination.transform.position - source.transform.position);
                VES.SetPhaseTimer (0.5f);
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (1, 1, 1));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPhaseTimer (1);
                break;

        }
    }

    static public void CreateRealVectorEffect (GameObject anchor, GameObject lookAtGameObject, int effectNumber, bool triggered, bool autoDestroy) {
        Color col = AppDefaults.GetAbilityColor (effectNumber);
        VisualEffectScript VES;
        switch (effectNumber) {
            case 5:
            case 34:
                VES = CreateEffectPointingAt (anchor, lookAtGameObject.transform.position, effectNumber, triggered, autoDestroy);
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
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
            case 19:
                CreateEffectPointingAt (anchor, lookAtGameObject.transform.position, 5, triggered, autoDestroy);
                break;
            case 13:
                VES = CreateEffectPointingAt (anchor, lookAtGameObject.transform.position, "Textures/Effects/Moon", col, triggered, autoDestroy);
                VES.SetLastPosition (new Vector3 (0, 0f, 0));
                VES.AddPhase ();
                VES.SetLastPosition (lookAtGameObject.transform.position - anchor.transform.position);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                break;
            case 15:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Dot", col, triggered, autoDestroy);
                //VES.SetJumpAnimation (true);
                VES.SetLastScale (new Vector3 (1f, 1f, 1f));
                VES.SetLastPosition (new Vector3 (0, 0f, 0));
                VES.AddPhase ();
                VES.SetLastPosition (lookAtGameObject.transform.position - anchor.transform.position);
                VES.AddPhase ();
                VES.rotateToCamera = true;
                VES.SetLastScale (new Vector3 (1.5f, 1.5f, 1.5f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                break;
            case 23:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Dot", col, triggered, autoDestroy);
                VES.SetJumpAnimation (true);
                VES.SetLastScale (new Vector3 (0.8f, 0.8f, 0.8f));
                VES.SetLastPosition (new Vector3 (0, 0.1f, 0));
                VES.AddPhase ();
                VES.SetLastPosition (lookAtGameObject.transform.position - anchor.transform.position);
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.rotateToCamera = true;
                VES.SetPhaseTimer (1.5f);
                break;
            case 30:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Dot", col, triggered, autoDestroy);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPosition (lookAtGameObject.transform.position - anchor.transform.position + new Vector3 (0, 2f, 0));
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (0.4f, 0.4f, 0.4f));
                VES.SetLastPhaseTimer (0.5f);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (0.7f, 0.7f, 0.7f));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.SetLastPhaseTimer (0.75f);
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.rotateToCamera = true;
                VES.SetLastPhaseTimer (0.5f);
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Dot", col, triggered, autoDestroy);
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                VES.SetScale (new Vector3 (0, 0, 0));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetLastPhaseTimer (1.25f);
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (0.5f, 0.5f, 0.5f));
                VES.SetLastPhaseTimer (0.5f);
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (1.7f, 1.7f, 1.7f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                break;
            case 32:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Dot", col, triggered, autoDestroy);
                VES.SetLastScale (new Vector3 (0.7f, 0.7f, 0.7f));
                VES.SetLastPosition (new Vector3 (0, 0f, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 1f, 0));
                VES.SetLastPhaseTimer (0.5f);
                VES.AddPhase ();
                VES.SetLastPhaseTimer (0.5f);
                VES.AddPhase ();
                VES.SetLastPosition (lookAtGameObject.transform.position - anchor.transform.position);
                VES.AddPhase ();
                VES.rotateToCamera = true;
                VES.SetLastScale (new Vector3 (1.4f, 1.4f, 1.4f));
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

    static public GameObject CreateEffect1 (GameObject anchor, int effectNumber, bool triggered, bool autoDestroy) {
        Color col = AppDefaults.GetAbilityColor (effectNumber);
        return CreateEffect (anchor, VisualCard.GetIconPath (effectNumber), col, triggered, autoDestroy);
    }
    static public void CreateEffect2 (GameObject anchor, int effectNumber, bool triggered, bool autoDestroy) {
        Color col = AppDefaults.GetAbilityColor (effectNumber);
        int effectTextureNumber = effectNumber;
        string texturePath = VisualCard.GetIconPath (effectTextureNumber) + "b";
        switch (effectNumber) {
            case 5:
            case 8:
            case 11:
            case 18:
            case 21:
            case 22:
            case 23:
            case 28:
            case 34:
                texturePath = VisualCard.GetIconPath (8) + "b";
                break;
            case 26:
                col = AppDefaults.Red;
                texturePath = VisualCard.GetIconPath (26);
                break;
            case 30:
            case 33:
                return;
            default:
                break;
        }
        CreateEffect (anchor, texturePath, col, triggered, autoDestroy);
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
