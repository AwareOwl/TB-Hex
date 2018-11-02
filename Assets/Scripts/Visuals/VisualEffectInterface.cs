using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffectInterface : MonoBehaviour {


    static public VisualTile GetTile (int x, int y) {
        return InGameUI.PlayedMatch.Board.tile [x, y].visualTile;
    }

    static public void CreateEffect (GameObject anchor, int effectNumber, bool triggered, bool autoDestroy) {
        GameObject Clone = GameObject.CreatePrimitive (PrimitiveType.Quad);
        DestroyImmediate (Clone.GetComponent<Collider> ());
        Renderer renderer = Clone.GetComponent<Renderer> ();
        renderer.material.shader = Shader.Find ("Sprites/Default");
        renderer.material.mainTexture = Resources.Load ("Textures/Ability/Ability0" + effectNumber) as Texture;
        renderer.material.color = AppDefaults.GetAbilityColor (effectNumber);
        Clone.transform.parent = anchor.transform;
        Clone.transform.localPosition = new Vector3 (0, 0, 0);
        Clone.transform.localEulerAngles = new Vector3 (90, 0, 0);

    }

}
