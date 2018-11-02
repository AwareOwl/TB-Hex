using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualTile {

    public GameObject Anchor;
    public GameObject Tile;
    public GameObject Collider;

    public VisualTile () {
    }

    public VisualTile (TileClass field) {
        CreateTile (field.x, field.y);
        EnableTile (field.enabled);
    }

    void CreateTile (int x, int y) {
        Anchor = EnvironmentScript.CreateTile (x, -0.5f, y);
        Tile = Anchor.transform.Find ("Tile").gameObject;
        //Tile.AddComponent<UIController> ();
        Collider = GameObject.Instantiate (Resources.Load ("Prefabs/TileCollider")) as GameObject;
        Collider.transform.localPosition = TilePosition (x, 0.12f, y);
        Collider.transform.localScale = new Vector3 (0.5f, 0.5f, 0.2f);
        Collider.name = "Tile";
        Collider.AddComponent<UIController> ();
        Collider.GetComponent<UIController> ().x = x;
        Collider.GetComponent<UIController> ().y = y;
        Collider.GetComponent<UIController> ().HoverObject = Collider.transform.Find ("Hover").gameObject;

    }

    public void DestroyVisual () {
        GameObject.DestroyImmediate (Anchor);
        GameObject.DestroyImmediate (Tile);
        GameObject.DestroyImmediate (Collider);
    }

    public void EnableTile (bool enable) {
        if (enable) {
            Anchor.GetComponent<VisualEffectScript> ().PushItToHeight (0);
        } else {
            Anchor.GetComponent<VisualEffectScript> ().PushItToHeight (-0.5f);
        }
    }

    static public Vector3 TilePosition (int x, float y, int z) {
        return new Vector3 (-3.75f + x + (z % 2) * 0.5f, 0 + y, -3.5f + z * Mathf.Sqrt (3) / 2);
    }

    public VisualEffectScript GetAnchorEffect () {
        return new VisualEffectScript ();
    }

}
