using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualTile {

    public TileClass tile;

    public GameObject Anchor;
    public GameObject Tile;
    public GameObject Collider;
    public GameObject Border;
    public GameObject Remains;

    public VisualTile () {
    }

    public VisualTile (TileClass tile) {
        this.tile = tile;
        CreateTile (tile.x, tile.y);
        EnableTile (tile.enabled);
        if (tile.remains) {
            CreateRemains ();
        }
    }

    public void DelayedCreateRemains () {
        if (VisualMatch.instance != null) {
            VisualMatch.instance.CreateRemains (this);
        } else {
            CreateRemains ();
        }
    }

    public void CreateRemains () {
        GameObject Clone;
        if (Remains == null) {
            Clone = GameObject.CreatePrimitive (PrimitiveType.Quad);
            Clone.transform.parent = Anchor.transform;
            Clone.transform.localEulerAngles = new Vector3 (-90, 0, 0);
            Clone.transform.localPosition = new Vector3 (0, 0.09f, 0);
            Clone.transform.localScale = new Vector3 (1, 1, 1);
            Clone.GetComponent<Renderer> ().material.shader = AppDefaults.sprite;
            Clone.GetComponent<Renderer> ().material.mainTexture = Resources.Load ("Textures/Effects/Dot") as Texture;
            Clone.GetComponent<Renderer> ().material.color = new Color (0.1f, 0.1f, 0.1f, 0.2f);
            Collider.GetComponent<UIController> ().number = 1;
            Remains = Clone;
        }
    }

    public void DestroyRemains () {
        if (Remains != null) {
            GameObject.DestroyImmediate (Remains);
        }
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
        Collider.GetComponent<UIController> ().tile = this.tile;
        Collider.GetComponent<UIController> ().x = x;
        Collider.GetComponent<UIController> ().y = y;
        GameObject Hover = Collider.transform.Find ("Hover").gameObject;
        Collider.GetComponent<UIController> ().HoverObject = Hover;
        Hover.GetComponent<Renderer> ().material.color = new Color (0, 0, 0, 0);
        Hover.AddComponent<VisualEffectScript> ();
        Border = Collider.transform.Find ("Quad").gameObject;
        Border.GetComponent<Renderer> ().enabled = false;
        GameObject.DestroyImmediate (EnvironmentScript.BackgroundTiles [x + 1, y + 1]);

    }

    public void DestroyVisual () {
        GameObject.DestroyImmediate (Anchor);
        GameObject.DestroyImmediate (Tile);
        GameObject.DestroyImmediate (Collider);
    }

    public void EnableTile (bool enable) {
        Tile.GetComponent<VisualEffectScript> ().SetColor (EnvironmentScript.TileColor (enable));
        if (enable) {
            Anchor.GetComponent<VisualEffectScript> ().PushToHeight (0);
            Border.GetComponent<Renderer> ().enabled = false;
        } else {
            Anchor.GetComponent<VisualEffectScript> ().PushToHeight (EnvironmentScript.disabledHeight);
            EnvironmentScript.BackgroundTiles [tile.x + 1, tile.y + 1] = Anchor;
            Border.GetComponent<Renderer> ().enabled = true;
        }
    }

    public void DelayedEnableTile (bool enable) {
        if (VisualMatch.instance != null) {
            VisualMatch.instance.EnableTile (this, enable);
        } else {
            EnableTile (enable);
        }
    }

    static public Vector3 TilePosition (int x, float y, int z) {
        return new Vector3 (-3.75f + x + (Mathf.Abs (z) % 2) * 0.5f, 0 + y, -3.5f + z * Mathf.Sqrt (3) / 2);
    }

    public VisualEffectScript GetAnchorEffect () {
        return new VisualEffectScript ();
    }

}
