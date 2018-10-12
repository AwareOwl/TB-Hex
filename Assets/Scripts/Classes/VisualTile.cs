using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualTile {

    static VisualTile [,] visualField = new VisualTile [10, 10];

    TileClass tile;

    public GameObject Anchor;
    public GameObject Tile;

    public VisualTile () {
        CreateTile ();
    }

    public VisualTile (TileClass field) {
        this.tile = field;
        if (visualField [field.x, field.y] == null) {
            visualField [field.x, field.y] = this;
        }
    }

    void CreateTile () {
        Anchor = EnvironmentScript.CreateTile ();
        Tile = Anchor.transform.Find ("Tile").gameObject;
        Tile.AddComponent<UIController> ();

    }

    static public Vector3 TilePosition (int x, int y, int z) {
        return new Vector3 (-3.75f + x + (z % 2) * 0.5f, 0 + y, -3.5f + z * Mathf.Sqrt (3) / 2);
    }

    public VisualEffectScript GetAnchorEffect () {
        return new VisualEffectScript ();
    }

}
