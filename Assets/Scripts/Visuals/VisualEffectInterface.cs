using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffectInterface : MonoBehaviour {


    static public VisualTile GetTile (int x, int y) {
        return InGameUI.PlayedMatch.Board.tile [x, y].visualTile;
    }

    static public void CreateEffectPointingAt (GameObject anchor, Vector3 lookAtPosition, int effectNumber, bool triggered, bool autoDestroy) {

        GameObject Clone = CreateEffect (anchor, VisualCard.GetIconPath (effectNumber), AppDefaults.GetAbilityColor (effectNumber), triggered, autoDestroy);
        Debug.Log (anchor.transform.position + " " + lookAtPosition);
        Clone.transform.LookAt (lookAtPosition);
        Clone.transform.localEulerAngles += new Vector3 (-90, -90, 0);
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
        VES.Init (col, 1, triggered, false);
        VES.basicColor [0] = new Color (col.r, col.g, col.b, 0);
        Clone.transform.parent = anchor.transform;
        Clone.transform.localPosition = new Vector3 (0, 0, 0);
        Clone.transform.localEulerAngles = new Vector3 (90, 0, 0);
        return Clone;
    }

}
