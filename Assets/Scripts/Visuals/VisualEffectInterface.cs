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

    static public void DelayedRealEffect (int x, int y, AbilityType abilityType, bool triggered) {
        VisualMatch.instance.RealEffect (x, y, abilityType, triggered);
    }

    static public void RealEffect (int x, int y, AbilityType abilityType, bool triggered) {
        Color col = AppDefaults.GetAbilityColor (abilityType);
        VisualEffectScript VES;
        switch (abilityType) {
            case AbilityType.T7:
                VES = CreateSimpleEffect (GetRealAnchor (x, y), "Textures/Ability/Ability07", col, triggered, true);
                VES.rotateToCamera = true;
                VES.SetLastPosition (new Vector3 (0, 1, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 2, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                break;
            case AbilityType.T38:
                VES = CreateSimpleEffect (GetRealAnchor (x, y), "Textures/Ability/Ability38", col, triggered, true);
                VES.rotateToCamera = true;
                VES.SetLastPosition (new Vector3 (0, 1f, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 2, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPhaseTimer (2f);
                break;
            case AbilityType.T42:
                VES = CreateSimpleEffect (GetRealAnchor (x, y), "Textures/Ability/Ability42", col, triggered, true);
                VES.rotateToCamera = true;
                VES.SetLastPosition (new Vector3 (0, 1f, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 2, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPhaseTimer (2f);
                break;
            case AbilityType.T62:
                VES = CreateSimpleEffect (GetRealAnchor (x, y), "Textures/Ability/Ability62", col, triggered, true);
                VES.rotateToCamera = true;
                VES.SetLastPosition (new Vector3 (0, 1f, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 2, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPhaseTimer (2f);
                break;
        }
    }

    static public void DelayedCreateRealEffects (VectorInfo info, AbilityType abilityType) {
        VisualMatch.instance.CreateRealEffects (info, abilityType);
    }

    static public void CreateRealEffects (VectorInfo info, AbilityType abilityType) {
        foreach (TileClass target in info.Triggered1) {
            switch (abilityType) {
                default:
                    CreateRealEffect1 (GetRealAnchor (target.x, target.y), abilityType, true, true);
                    break;
                case AbilityType.T6:
                case AbilityType.T15:
                case AbilityType.T30:
                case AbilityType.T32:
                case AbilityType.T55:
                    CreateRealVectorEffect (GetRealAnchor (target.x, target.y), GetRealAnchor (info.PlayedTokenTile.x, info.PlayedTokenTile.y), abilityType, true, true);
                    break;
                case AbilityType.T74:
                    foreach (TileClass target2 in info.Triggered2) {
                        CreateRealVectorEffect (GetRealAnchor (target2.x, target2.y), GetRealAnchor (target.x, target.y), abilityType, true, true);
                    }
                    break;
                case AbilityType.T66:
                    CreateRealVectorEffect (GetRealAnchor (target.x, target.y), GetRealAnchor (info.PlayedTokenTile.x, info.PlayedTokenTile.y), abilityType, true, true);
                    CreateRealVectorEffect (GetRealAnchor (info.PlayedTokenTile.x, info.PlayedTokenTile.y), GetRealAnchor (target.x, target.y), abilityType, true, true);
                    break;
                case AbilityType.T13:
                case AbilityType.T41:
                case AbilityType.T53:
                case AbilityType.T48:
                    CreateRealVectorEffect (GetRealAnchor (info.PlayedTokenTile.x, info.PlayedTokenTile.y), GetRealAnchor (target.x, target.y), abilityType, true, true);
                    break;
                case AbilityType.T23:
                case AbilityType.T39:
                    CreateRealVectorEffect (GetRealAnchor (info.Triggered2 [0].x, info.Triggered2 [0].y), GetRealAnchor (target.x, target.y), abilityType, true, true);
                    break;
            }
        }
        foreach (TileClass target in info.Triggered2) {
            switch (abilityType) {
                default:
                    CreateRealEffect2 (GetRealAnchor (target.x, target.y), abilityType, true, true);
                    break;
                case AbilityType.T11:
                    CreateRealVectorEffect (GetRealAnchor (target.x, target.y), GetRealAnchor (info.PlayedTokenTile.x, info.PlayedTokenTile.y), abilityType, true, true);
                    CreateRealVectorEffect (GetRealAnchor (info.PlayedTokenTile.x, info.PlayedTokenTile.y), GetRealAnchor (target.x, target.y), abilityType, true, true);
                    break;
                case AbilityType.T18:
                    CreateRealVectorEffect (GetRealAnchor (info.PlayedTokenTile.x, info.PlayedTokenTile.y), GetRealAnchor (target.x, target.y), abilityType, true, true);
                    break;
                case AbilityType.T56:
                    CreateRealVectorEffect (GetRealAnchor (target.x, target.y), GetRealAnchor (info.Triggered1 [0].x, info.Triggered1 [0].y), abilityType, true, true);
                    break;
            }
        }
        foreach (TileClass target in info.NotTriggered) {

        }
        foreach (AbilityVector vector in info.TriggeredVector) {
            CreateRealVectorEffect (GetRealAnchor (vector.x, vector.y), GetRealAnchor (vector.pushX, vector.pushY), abilityType, true, true);

        }

        CreateRealConstantEffect (GetRealAnchor (info.PlayedTokenTile.x, info.PlayedTokenTile.y), abilityType, true, true);
    }

    static public void CreateRealConstantEffect (GameObject anchor, AbilityType abilityType, bool triggered, bool autoDestroy) {
        Color col = AppDefaults.GetAbilityColor (abilityType);
        VisualEffectScript VES;
        switch (abilityType) {
            case AbilityType.T49:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/SwordP1", col, triggered, autoDestroy);
                VES.SetLastPosition (new Vector3 (0, 1, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 1.2f, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetPhaseTimer (2f);
                VES.rotateToCamera = true;
                VES = CreateSimpleEffect (anchor, "Textures/Effects/SwordP2", col, triggered, autoDestroy);
                VES.SetLastPosition (new Vector3 (0, 1, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0.5f, 1.05f, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetPhaseTimer (2f);
                VES.rotateToCamera = true;
                VES = CreateSimpleEffect (anchor, "Textures/Effects/SwordP3", col, triggered, autoDestroy);
                VES.SetLastPosition (new Vector3 (0, 1, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (-0.5f, 0.95f, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetPhaseTimer (2f);
                VES.rotateToCamera = true;
                VES = CreateSimpleEffect (anchor, "Textures/Effects/SwordP4", col, triggered, autoDestroy);
                VES.SetLastPosition (new Vector3 (0, 1, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 0.8f, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetPhaseTimer (2f);
                VES.rotateToCamera = true;
                break;
            case AbilityType.T50:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/LittleStar", col, triggered, autoDestroy);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetPhaseTimer (1.6f);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetRotateVector (new Vector3 (0, 0, 360));
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                break;
        }
    }

    static public void CreateRealEffect1 (GameObject anchor, AbilityType abilityType, bool triggered, bool autoDestroy) {
        Color col = AppDefaults.GetAbilityColor (abilityType);
        VisualEffectScript VES;
        switch (abilityType) {
            case AbilityType.T1:
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
            case AbilityType.T2:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Promote2", col, triggered, autoDestroy);
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 1, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.rotateToCamera = true;
                break;
            case AbilityType.T3:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/DeleteDis", col, triggered, autoDestroy);
                VES.SetLastScale (new Vector3 (0, 1f, 1f));
                VES.SetPosition (new Vector3 (0, 0f, 0));
                VES.transform.localEulerAngles = new Vector3 (90, -40, 0);
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (1.25f, 1, 1));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                break;
            case AbilityType.T4:
                VES = CreateSimpleEffect (anchor, "Textures/Ability/Ability04", col, triggered, autoDestroy);
                VES.SetLastScale (new Vector3 (1f, 1f, 1f));
                VES.SetLastPosition (new Vector3 (0, 0.25f, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 0.75f, 0));
                VES.SetLastScale (new Vector3 (2f, 2f, 2f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.rotateToCamera = true;
                break;
            case AbilityType.T8:
            case AbilityType.T73:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Dot", col, triggered, autoDestroy);
                VES.SetLastScale (new Vector3 (0.35f, 2f, 0.35f));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetPhaseTimer (0.5f);
                VES.SetLastPosition (new Vector3 (0, 10, 0));
                VES.rotateToCamera = true;
                break;
            case AbilityType.T9:
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
            case AbilityType.T10:
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
            case AbilityType.T12:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Thunder2", col, triggered, autoDestroy);
                VES.SetLastColor (new Color (col.r * 2, col.g, col.b, 1));
                VES.SetLastPosition (new Vector3 (0, 5, 0));
                VES.SetScale (new Vector3 (1, 10, 1));
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r * 2, col.g, col.b, 0));
                VES.rotateToCameraVertical = true;
                break;
            case AbilityType.T14:
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
            case AbilityType.T16:
            case AbilityType.T33:
            case AbilityType.T52:
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
            case AbilityType.T17:
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
            case AbilityType.T18:
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
            case AbilityType.T19:
                VES = CreateSimpleEffect (anchor, "Textures/Ability/Ability19", col, triggered, autoDestroy);
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                break;
            case AbilityType.T20:
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
            case AbilityType.T21:
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
            case AbilityType.T22:
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
            case AbilityType.T24:
            case AbilityType.T47:
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
            case AbilityType.T25:
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
            case AbilityType.T26:
            case AbilityType.T43:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Promote2", col, triggered, autoDestroy);
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 1, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.rotateToCamera = true;
                break;
            case AbilityType.T27:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Promote2", col, triggered, autoDestroy);
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 1, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.rotateToCamera = true;
                break;
            case AbilityType.T28:
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
            case AbilityType.T29:
            case AbilityType.T63:
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
            case AbilityType.T31:
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

            case AbilityType.T35:
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

            case AbilityType.T36:
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
            case AbilityType.T37:
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
            case AbilityType.T40:
                /*
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Dot", col, triggered, autoDestroy);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.SetLastPosition (new Vector3 (0, 2, 0));
                VES.SetScale (new Vector3 (0, 2, 0));
                VES.AddPhase ();
                VES.SetPhaseTimer (1f);
                VES.SetLastPosition (new Vector3 (0, 2, 0));
                VES.SetLastScale (new Vector3 (0.5f, 0, 0.5f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.rotateToCameraVertical = true;*/

                VES = CreateSimpleEffect (anchor, "Textures/Effects/Ring", col, triggered, autoDestroy);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.SetLastPosition (new Vector3 (0, 2, 0));
                VES.SetScale (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetPhaseTimer (1.25f);
                VES.SetLastScale (new Vector3 (0.75f, 0.75f, 0.75f));
                VES.SetPhaseTimer (0.75f);
                VES.SetLastScale (new Vector3 (1.5f, 1.5f, 1.5f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);

                VES = CreateSimpleEffect (anchor, "Textures/Effects/Ring", col, triggered, autoDestroy);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.SetLastPosition (new Vector3 (0, 2, 0));
                VES.SetScale (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetPhaseTimer (1.25f);
                VES.SetLastScale (new Vector3 (0, 0, 0));
                VES.SetPhaseTimer (0.75f);
                VES.SetLastScale (new Vector3 (0.75f, 0.75f, 0.75f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                break;
            case AbilityType.T44:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Diamond", col, triggered, autoDestroy);
                VES.SetLastScale (new Vector3 (0.5f, 1f, 0.5f));
                VES.SetLastPosition (new Vector3 (0, 0.75f, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.AddPhase ();
                VES.SetPhaseTimer (0.25f);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.AddPhase ();
                VES.SetPhaseTimer (1.75f);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.rotateToCameraVertical = true;
                break;
            case AbilityType.T46:
                VES = CreateSimpleEffect (anchor, "Textures/Ability/Ability46", col, triggered, autoDestroy);
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 1, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetPhaseTimer (2f);
                VES.rotateToCamera = true;
                break;
            case AbilityType.T50:
                VES = CreateSimpleEffect (anchor, "Textures/Ability/Ability50", col, triggered, autoDestroy);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetPhaseTimer (1f);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetRotateVector (new Vector3 (0, 0, 360));
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                VES = CreateSimpleEffect (anchor, "Textures/Effects/LittleStar", col, triggered, autoDestroy);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetPhaseTimer (1f);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.AddPhase ();
                VES.SetPhaseTimer (1f);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetRotateVector (new Vector3 (0, 0, 360));
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                break;
            case AbilityType.T51:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/CardCut1", col, triggered, autoDestroy);
                VES.SetLastPosition (new Vector3 (0, 1, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 1.2f, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetPhaseTimer (2f);
                VES.rotateToCamera = true;
                VES = CreateSimpleEffect (anchor, "Textures/Effects/CardCut2", col, triggered, autoDestroy);
                VES.SetLastScale (new Vector3 (0, 0, 0));
                VES.SetLastPosition (new Vector3 (-0.2f, 1, 0));
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (1, 1, 1));
                VES.SetLastPosition (new Vector3 (0, 1, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetPhaseTimer (2f);
                VES.rotateToCamera = true;
                VES = CreateSimpleEffect (anchor, "Textures/Effects/CardCut3", col, triggered, autoDestroy);
                VES.SetLastPosition (new Vector3 (0, 1, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 0.8f, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetPhaseTimer (2f);
                VES.rotateToCamera = true;
                break;
            case AbilityType.T54:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Increment", col, triggered, autoDestroy);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.SetLastPosition (new Vector3 (0, -0.5f, 0));
                VES.AddPhase ();
                VES.SetLastPhaseTimer (0.5f);
                VES.SetLastPosition (new Vector3 (0, 0.3f, 0));
                VES.AddPhase ();
                VES.SetLastPhaseTimer (1.25f);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.rotateToCameraVertical = true;
                break;
            case AbilityType.T57:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Comet2", col, triggered, autoDestroy);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastScale (new Vector3 (1, 1, 1));
                VES.SetLastPosition (new Vector3 (0, 0.5f, 0));
                VES.AddPhase ();
                VES.SetLastPhaseTimer (0.25f);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.AddPhase ();
                VES.SetLastPhaseTimer (1.5f);
                VES.SetLastScale (new Vector3 (0f, 0.8f, 1));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.rotateToCameraVertical = true;
                break;
            case AbilityType.T58:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Ring", col, triggered, autoDestroy);
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastScale (new Vector3 (1, 1, 1));
                VES.AddPhase ();
                VES.SetLastPhaseTimer (0.25f);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.AddPhase ();
                VES.SetLastPhaseTimer (1.75f);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Dot", col, triggered, autoDestroy);
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastScale (new Vector3 (1, 1, 1));
                VES.AddPhase ();
                VES.SetLastPhaseTimer (0.25f);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.AddPhase ();
                VES.SetLastPhaseTimer (1.75f);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastScale (new Vector3 (0, 0, 0));
                break;
            case AbilityType.T59:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Dot", col, triggered, autoDestroy);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.SetLastScale (new Vector3 (1.2f, 1.6f, 1.6f));
                VES.SetLastPosition (new Vector3 (0, 6, 0));
                VES.AddPhase ();
                VES.SetLastPhaseTimer (0.5f);
                VES.SetLastPosition (new Vector3 (0, 0f, 0));
                VES.AddPhase ();
                VES.SetLastPhaseTimer (0.25f);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.rotateToCameraVertical = true;
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Ignite", col, triggered, autoDestroy);
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                VES.SetLastScale (new Vector3 (1, 1, 1));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetLastPhaseTimer (0.5f);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.AddPhase ();
                VES.SetLastPhaseTimer (0.25f);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.AddPhase ();
                VES.SetLastPhaseTimer (1.5f);
                VES.SetLastScale (new Vector3 (3, 3, 3));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                break;
            case AbilityType.T60:
                VES = CreateSimpleEffect (anchor, "Textures/Ability/Ability60", col, triggered, autoDestroy);
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.SetLastScale (new Vector3 (1, 1, 1));
                VES.SetLastPosition (new Vector3 (0, 0f, 0));
                VES.AddPhase ();
                VES.SetLastPhaseTimer (1f);
                VES.SetLastScale (new Vector3 (0.6f, 0.6f, 0.6f));
                VES.AddPhase ();
                VES.SetLastPhaseTimer (1f);
                VES.SetLastScale (new Vector3 (0f, 0f, 0f));
                VES.SetRotateVector (new Vector3 (0, -75, 0));
                break;
            case AbilityType.T64:
                for (int x = -1; x < 2; x += 2) {
                    VES = CreateSimpleEffect (anchor, "Textures/Effects/HalfMoon2", col, triggered, autoDestroy);
                    VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                    VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                    VES.SetLastScale (new Vector3 (1, 1, 1));
                    VES.SetLastPosition (new Vector3 (0, 0f, 0));
                    VES.AddPhase ();
                    VES.SetLastPhaseTimer (2f);
                    VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                    VES.SetRotateVector (new Vector3 (0, 90 * x, 0));
                }
                break;
            case AbilityType.T67:
                for (int x = 0; x < 3; x++) {
                    VES = CreateSimpleEffect (anchor, "Textures/Effects/Needle", col, triggered, autoDestroy);
                    VES.gameObject.transform.localEulerAngles = new Vector3 (0, 0, 45 - x * 45);
                    VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                    VES.SetLastScale (new Vector3 (0.4f, 0.6f, 0.4f));
                    VES.SetLastPosition (new Vector3 (-0.5f + x * 0.5f, 1 + (x % 2) * 0.4f, 0));
                    VES.AddPhase ();
                    VES.SetLastPhaseTimer (0.5f);
                    VES.SetLastScale (new Vector3 (0.2f, 0.3f, 0.2f));
                    VES.SetLastPosition (new Vector3 (0, 0f, 0));
                    VES.AddPhase ();
                    VES.SetLastPhaseTimer (1.5f);
                    VES.SetLastScale (new Vector3 (0f, 0f, 0f));
                    VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                    VES.rotateToCameraVertical = true;
                }
                break;
            case AbilityType.T68:
                for (int x = 0; x < 2; x++) {
                    for (int y = 0; y < 2; y++) {
                        VES = CreateSimpleEffect (anchor, "Textures/Effects/Dot", col, triggered, autoDestroy);
                        VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                        VES.SetLastScale (new Vector3 (0.6f, 0.6f, 0.6f));
                        VES.SetLastPosition (new Vector3 (-0.4f + x * 0.8f, 0, -0.4f + y * 0.8f));
                        VES.AddPhase ();
                        VES.SetLastPhaseTimer (1f);
                        VES.SetLastScale (new Vector3 (0.5f, 0.5f, 0.5f));
                        VES.SetLastPosition (new Vector3 (0, 0f, 0));
                        VES.SetJumpAnimation (true);
                        VES.AddPhase ();
                        VES.SetLastPhaseTimer (1f);
                        VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                        VES.rotateToCamera = true;
                    }
                }
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Dot", col, triggered, autoDestroy);
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.SetLastScale (new Vector3 (0, 0, 0));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetLastPhaseTimer (0.8f);
                VES.AddPhase ();
                VES.SetLastPhaseTimer (0.1f);
                VES.AddPhase ();
                VES.SetLastPhaseTimer (1.1f);
                VES.SetLastScale (new Vector3 (1.5f, 1.5f, 1.5f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                break;
            case AbilityType.T69:
                for (int x = 0; x < 2; x++) {
                    for (int y = 0; y < 2; y++) {
                        VES = CreateSimpleEffect (anchor, "Textures/Effects/Dot", col, triggered, autoDestroy);
                        VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                        VES.SetLastScale (new Vector3 (0.7f, 0.7f, 0.7f));
                        VES.SetLastPosition (new Vector3 (0, 0, 0));
                        VES.AddPhase ();
                        VES.SetLastPhaseTimer (2f);
                        VES.SetLastScale (new Vector3 (0, 0, 0));
                        VES.SetLastPosition (new Vector3 (-0.6f + x * 1.2f, 0.6f, -0.6f + y * 1.2f));
                        VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                        VES.rotateToCamera = true;
                    }
                }
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Dot", col, triggered, autoDestroy);
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.SetLastScale (new Vector3 (0, 0, 0));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetLastPhaseTimer (0.8f);
                VES.AddPhase ();
                VES.SetLastPhaseTimer (1f);
                VES.SetLastScale (new Vector3 (2, 2, 2));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                break;
            case AbilityType.T70:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Comet2", col, triggered, autoDestroy);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.SetLastScale (new Vector3 (2, 0, 1));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetLastPhaseTimer (2f);
                VES.SetLastScale (new Vector3 (0, 2, 1));
                VES.SetLastPosition (new Vector3 (0, 1f, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                break;
        }
    }

    static public void CreateRealEffect2 (GameObject anchor, AbilityType abilityType, bool triggered, bool autoDestroy) {
        Color col = AppDefaults.GetAbilityColor (abilityType);
        VisualEffectScript VES;
        switch (abilityType) {
            case AbilityType.T2:
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
            case AbilityType.T8:
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
            case AbilityType.T73:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Comet2", col, triggered, autoDestroy);
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
            case AbilityType.T21:
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
            case AbilityType.T22:
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
            case AbilityType.T26:
            case AbilityType.T43:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Beak", col, triggered, autoDestroy);
                VES.SetLastPosition (new Vector3 (0, 1, 0));
                VES.AddPhase ();
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.SetLastColor (new Color (1, 0, 0, 0));
                VES.rotateToCamera = true;
                break;
            case AbilityType.T28:
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
            case AbilityType.T30:
            case AbilityType.T48:
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
            case AbilityType.T33:
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
            case AbilityType.T44:
                VES = CreateSimpleEffect (anchor, "Textures/Ability/Ability44", col, triggered, autoDestroy);
                VES.transform.localEulerAngles = new Vector3 (90, 0, 0);
                VES.SetLastScale (new Vector3 (2.5f, 2.5f, 2.5f));
                VES.SetLastPosition (new Vector3 (0, 0.5f, 0));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0.4f));
                VES.AddPhase ();
                VES.SetLastPhaseTimer (1.5f);
                VES.AddPhase ();
                VES.SetLastPhaseTimer (0.5f);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetRotateVector (new Vector3 (0, 0, 360));
                break;
            case AbilityType.T53:
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
            case AbilityType.T54:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Increment", col, triggered, autoDestroy);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.SetLastScale (new Vector3 (1, 1.6f, 1));
                VES.SetLastPosition (new Vector3 (0, -0.5f, 0));
                VES.AddPhase ();
                VES.SetPhaseTimer (0.5f);
                VES.SetLastPosition (new Vector3 (0, 0.5f, 0));
                VES.AddPhase ();
                VES.SetPhaseTimer (1f);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.rotateToCameraVertical = true;
                break;
        }
    }

    static public void CreateRealTokenEffect (TileClass sourceTile, TokenType tokenType) {
        GameObject source = GetRealAnchor (sourceTile);
        Color col = Color.white;
        VisualEffectScript VES = null;
        switch (tokenType) {
            case TokenType.T5:
                col = AppDefaults.red;
                break;
            case TokenType.T9:
                col = AppDefaults.blue;
                break;
            case TokenType.T12:
                col = AppDefaults.yellow;
                break;
            case TokenType.T13:
                col = AppDefaults.teal;
                break;
            case TokenType.T15:
                col = AppDefaults.purple;
                break;
            case TokenType.T16:
                col = AppDefaults.yellow;
                break;
        }
        switch (tokenType) {
            case TokenType.T5:
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
            case TokenType.T9:
                VES = CreateSimpleEffect (source, "Textures/Effects/BlockStatus", col, true, true);
                break;
            case TokenType.T12:
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
            case TokenType.T13:
                VES = CreateSimpleEffect (source, "Textures/Effects/TurnStatus", col, true, true);
                break;
            case TokenType.T15:
                VES = CreateSimpleEffect (source, "Textures/Effects/SpecialStatus", col, true, true);
                break;
            case TokenType.T16:
                VES = CreateSimpleEffect (source, "Textures/Effects/RenownStatus", col, true, true);
                break;
        }
        switch (tokenType) {
            case TokenType.T9:
            case TokenType.T13:
            case TokenType.T15:
            case TokenType.T16:
                VES.SetLastScale (new Vector3 (0.6f, 0.6f, 0.6f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0f));
                VES.SetLastPosition (new Vector3 (0, 0.5f, 0));
                VES.AddPhase ();
                VES.SetPhaseTimer (0.5f);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1));
                VES.SetLastPosition (new Vector3 (0, 0.65f, 0));
                VES.AddPhase ();
                VES.SetPhaseTimer (1f);
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0f));
                VES.SetLastPosition (new Vector3 (0, 0.65f, 0));
                VES.rotateToCamera = true;
                VES.SetPhaseTimer (0.5f);
                break;
        }
    }

    static public void CreateRealTokenVectorEffect (TileClass sourceTile, TileClass destinationTile, TokenType tokenType) {
        GameObject source = GetRealAnchor (sourceTile);
        GameObject destination = GetRealAnchor (destinationTile);
        Color col = Color.white;
        VisualEffectScript VES;
        switch (tokenType) {
            case TokenType.T3:
                col = AppDefaults.green;
                break;
            case TokenType.T4:
            case TokenType.T5:
            case TokenType.T23:
                col = AppDefaults.red;
                break;
            case TokenType.T22:
                col = new Color (0.9f, 0.9f, 0);
                break;
        }
        switch (tokenType) {
            case TokenType.T3:
            case TokenType.T4:
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
            case TokenType.T5:
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
            case TokenType.T22:
            case TokenType.T23:
                VES = CreateSimpleEffect (source, "Textures/Effects/Dot", col, true, true);
                VES.SetLastScale (new Vector3 (0, 0, 0));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetLastPhaseTimer (0.25f);
                VES.SetLastScale (new Vector3 (0.6f, 0.6f, 0.6f));
                VES.SetLastPosition (new Vector3 (0, 0.5f, 0));
                VES.AddPhase ();
                VES.SetLastPhaseTimer (0.5f);
                VES.SetLastPosition (destination.transform.position - source.transform.position);
                VES.AddPhase ();
                VES.rotateToCamera = true;
                VES.SetLastScale (new Vector3 (1, 1, 1));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPhaseTimer (1.25f);

                VES = CreateSimpleEffect (source, "Textures/Effects/Dot", col, true, true);
                VES.SetLastScale (new Vector3 (0, 0, 0));
                VES.SetLastPosition (destination.transform.position - source.transform.position);
                VES.AddPhase ();
                VES.SetLastPhaseTimer (0.75f);
                VES.AddPhase ();
                VES.rotateToCamera = true;
                VES.SetLastScale (new Vector3 (1, 1, 1));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPhaseTimer (1.25f);
                break;

        }
    }

    static public void CreateRealVectorEffect (GameObject anchor, GameObject lookAtGameObject, AbilityType abilityType, bool triggered, bool autoDestroy) {
        Color col = AppDefaults.GetAbilityColor (abilityType);
        VisualEffectScript VES;
        switch (abilityType) {
            case AbilityType.T5:
            case AbilityType.T34:
                VES = CreateEffectPointingAt (anchor, lookAtGameObject.transform.position, abilityType, triggered, autoDestroy);
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                break;
            case AbilityType.T6:
            case AbilityType.T55:
            case AbilityType.T56:
            case AbilityType.T65:
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
            case AbilityType.T11:
            case AbilityType.T19:
                CreateEffectPointingAt (anchor, lookAtGameObject.transform.position, AbilityType.T5, triggered, autoDestroy);
                break;
            case AbilityType.T13:
                VES = CreateEffectPointingAt (anchor, lookAtGameObject.transform.position, "Textures/Effects/Moon", col, triggered, autoDestroy);
                VES.SetLastPosition (new Vector3 (0, 0f, 0));
                VES.AddPhase ();
                VES.SetLastPosition (lookAtGameObject.transform.position - anchor.transform.position);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                break;
            case AbilityType.T15:
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
            case AbilityType.T74:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Dot", col, triggered, autoDestroy);
                //VES.SetJumpAnimation (true);
                VES.SetLastScale (new Vector3 (0.7f, 0.7f, 0.7f));
                VES.SetLastPosition (new Vector3 (0, 0f, 0));
                VES.AddPhase ();
                VES.SetLastPosition (lookAtGameObject.transform.position - anchor.transform.position);
                VES.AddPhase ();
                VES.rotateToCamera = true;
                VES.SetLastScale (new Vector3 (1.05f, 1.05f, 1.05f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetJumpAnimation (true);
                break;
            case AbilityType.T23:
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
            case AbilityType.T30:
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
            case AbilityType.T32:
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
            case AbilityType.T39:
            case AbilityType.T53:
                VES = CreateEffectPointingAt (anchor, lookAtGameObject.transform.position, "Textures/Effects/Ring", col, triggered, autoDestroy);
                VES.SetLastScale (new Vector3 (0.9f, 0.9f, 0.9f));
                VES.SetLastPosition (new Vector3 (0, 0f, 0));
                VES.AddPhase ();
                VES.SetLastPhaseTimer (0.6f);
                VES.SetLastPosition (lookAtGameObject.transform.position - anchor.transform.position);
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPhaseTimer (1.4f);
                break;
            case AbilityType.T41:
                VES = CreateEffectPointingAt (anchor, lookAtGameObject.transform.position, "Textures/Effects/Dot", col, triggered, autoDestroy);
                VES.SetLastScale (new Vector3 (0.4f, 0.4f, 0.4f));
                VES.SetLastPosition (new Vector3 (0, 0f, 0));
                VES.AddPhase ();
                VES.SetLastPhaseTimer (0.75f);
                VES.SetLastScale (new Vector3 (0.8f, 0.6f, 0.8f));
                VES.SetLastPosition (lookAtGameObject.transform.position - anchor.transform.position);
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (0.6f, 0.8f, 0.6f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                VES.SetLastPhaseTimer (1.25f);

                VES = CreateSimpleEffect (lookAtGameObject, "Textures/Effects/Ring", col, true, true);
                VES.gameObject.transform.localEulerAngles = new Vector3 (90, 0, 0);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0.0f));
                VES.SetLastPosition (new Vector3 (0, 0, 0));
                VES.AddPhase ();
                VES.SetPhaseTimer (0.8f);
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0.7f));
                VES.SetLastScale (new Vector3 (0.1f, 0.1f, 0.1f));
                VES.SetLastPhaseTimer (0.1f);
                VES.AddPhase ();
                VES.SetLastScale (new Vector3 (1, 1, 1));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0.2f));
                VES.SetLastPhaseTimer (1.1f);
                break;
            case AbilityType.T48:
                VES = CreateSimpleEffect (anchor, "Textures/Effects/Seed", col, true, true);
                VES.SetLastPosition (new Vector3 (0, 1.5f, 0));
                VES.SetLastScale (new Vector3 (0.8f, 0.8f, 0.8f));
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0.0f));
                VES.AddPhase ();
                VES.SetPhaseTimer (0.4f);
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r, col.g, col.b, 1f));
                VES.SetLastPhaseTimer (0.5f);
                VES.AddPhase ();
                VES.SetLastPosition (lookAtGameObject.transform.position - anchor.transform.position);
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0.1f));
                VES.SetLastPhaseTimer (1.1f);
                VES.rotateToCamera = true;
                break;
            case AbilityType.T66:
                VES = CreateEffectPointingAt (anchor, lookAtGameObject.transform.position, AbilityType.T5, triggered, autoDestroy);
                VES.AddPhase ();
                VES.SetLastColor (new Color (col.r, col.g, col.b, 0));
                break;
        }
    }


    static public VisualEffectScript CreateEffectPointingAt (GameObject anchor, Vector3 lookAtPosition, AbilityType abilityType, bool triggered, bool autoDestroy) {
        return CreateEffectPointingAt (anchor, lookAtPosition, VisualCard.GetIconPath (abilityType), AppDefaults.GetAbilityColor (abilityType), triggered, autoDestroy);
    }

    static public VisualEffectScript CreateEffectPointingAt (GameObject anchor, Vector3 lookAtPosition, string path, Color color, bool triggered, bool autoDestroy) {
        GameObject Clone = CreateEffect (anchor, path, color, triggered, autoDestroy);
        Clone.transform.LookAt (lookAtPosition);
        Clone.transform.localEulerAngles += new Vector3 (-90, -90, 0);
        return Clone.GetComponent<VisualEffectScript> ();
    }

    static public GameObject CreateEffect1 (GameObject anchor, AbilityType effectNumber, bool triggered, bool autoDestroy) {
        Color col = AppDefaults.GetAbilityColor (effectNumber);
        AbilityType effectTextureNumber = effectNumber;
        string texturePath = VisualCard.GetIconPath (effectTextureNumber);
        switch (effectNumber) {
            case AbilityType.T74:
                texturePath = VisualCard.GetIconPath (8) + "b";
                break;
            default:
                break;
        }
        return CreateEffect (anchor, texturePath, col, triggered, autoDestroy);
    }

    static public void CreateEffect2 (GameObject anchor, AbilityType effectNumber, bool triggered, bool autoDestroy) {
        Color col = AppDefaults.GetAbilityColor (effectNumber);
        AbilityType effectTextureNumber = effectNumber;
        string texturePath = VisualCard.GetIconPath (effectTextureNumber) + "b";
        switch (effectNumber) {
            case AbilityType.T5:
            case AbilityType.T8:
            case AbilityType.T11:
            case AbilityType.T18:
            case AbilityType.T21:
            case AbilityType.T22:
            case AbilityType.T23:
            case AbilityType.T28:
            case AbilityType.T34:
            case AbilityType.T39:
            case AbilityType.T40:
            case AbilityType.T73:
                texturePath = VisualCard.GetIconPath (8) + "b";
                break;
            case AbilityType.T26:
                col = AppDefaults.red;
                texturePath = VisualCard.GetIconPath (26);
                break;
            case AbilityType.T30:
            case AbilityType.T33:
            case AbilityType.T44:
            case AbilityType.T48:
            case AbilityType.T53:
            case AbilityType.T54:
                return;
            case AbilityType.T43:
                col = AppDefaults.red;
                texturePath = VisualCard.GetIconPath (43);
                break;
            case AbilityType.T56:
                col = AppDefaults.red;
                texturePath = VisualCard.GetIconPath (56);
                break;
            case AbilityType.T57:
                col = AppDefaults.yellow;
                texturePath = VisualCard.GetIconPath (57);
                break;
            case AbilityType.T74:
                col = AppDefaults.yellow;
                texturePath = VisualCard.GetIconPath (effectNumber);
                break;
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
