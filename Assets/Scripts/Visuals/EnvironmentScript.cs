using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentScript : MonoBehaviour {

    static GameObject _BackGround;

    static GameObject Background {
        get {
            if (_BackGround == null) {
                _BackGround = new GameObject ();
            }
            return _BackGround;
        }
        set {
            _BackGround = value;
        }
    }

    static GameObject [,,] BackgroundTiles;

    static int Theme = 1;

    // Use this for initialization
    void Awake () {
    }

    private void Start () {
        CreateNewBackground (1);
    }

    // Update is called once per frame
    void Update () {

    }

    static public void CreateNewBackground (int theme) {
        Theme = theme;
        switch (theme) {
            case 1:
                CreateWaterBackground ();
                break;
            case 2:
                CreateTowerBackground ();
                break;
            case 3:
                CreateLavaBackground ();
                break;
        }
    }

    static public void CreateRandomBoard () {
        for (int x = 0; x < 8; x++) {
            for (int y = 0; y < 8; y++) {
                if (true) {
                    //if (Random.Range (0, 2) != 0) {
                    //VisualTile tile = new VisualTile ();
                    CreateAttachedTile (x, 0, y);

                    /*VisualToken token;
                    if (Random.Range (0, 3) == -1) {
                        token = new VisualToken ();
                        token.Anchor.transform.parent = tile.Anchor.transform;
                        token.Anchor.transform.localPosition = new Vector3 (0, 0.4f, 0);

                        token.Base.GetComponent<VisualEffectScript> ().Init (AppDefaults.PlayerColor [Random.Range (0, 4)], false, true);
                    }*/

                }
            }
        }
    }

    static public void CreateNormalBoard () {
        GameObject hex;
        List<GameObject> Hexes = new List<GameObject> ();
        for (int x = 0; x < 9; x++) {
            /*hex = CreateAttachedTile (x, 0, 0);
            hex = CreateAttachedTile (0, 0, x);
            hex = CreateAttachedTile (x, 0, 8);
            hex = CreateAttachedTile (8, 0, x);*/
        }
        for (int x = 2; x < 6; x++) {
            hex = CreateAttachedTile (x, 0, 1);
            Hexes.Add (hex);
        }
        for (int x = 2; x < 7; x++) {
            hex = CreateAttachedTile (x, 0, 2);
            Hexes.Add (hex);
        }
        for (int x = 1; x < 7; x++) {
            hex = CreateAttachedTile (x, 0, 3);
            Hexes.Add (hex);
        }
        for (int x = 1; x < 8; x++) {
            hex = CreateAttachedTile (x, 0, 4);
            Hexes.Add (hex);
        }
        for (int x = 1; x < 7; x++) {
            hex = CreateAttachedTile (x, 0, 5);
            Hexes.Add (hex);
        }
        for (int x = 2; x < 7; x++) {
            hex = CreateAttachedTile (x, 0, 6);
            Hexes.Add (hex);
        }
        for (int x = 2; x < 6; x++) {
            hex = CreateAttachedTile (x, 0, 7);
            Hexes.Add (hex);
        }
        VisualToken token;
        for (int x = 0; x < Hexes.Count; x++) {
            if (Random.Range (0, 3) != 0) {
                token = new VisualToken ();
                token.Anchor.transform.parent = Hexes [x].transform;
                token.Anchor.transform.localPosition = new Vector3 (0, 0.4f, 0);

                token.Base.GetComponent<VisualEffectScript> ().Init (AppDefaults.PlayerColor [Random.Range (0, 2)], false, true);
            }
        }

    }

    static private void CreateWaterBackground () {
        int minX = -1;
        int maxX = 10;
        int minY = -5;
        int maxY = -1;
        int minZ = -1;
        int maxZ = 10;
        /*GameObject water = Instantiate (Resources.Load ("Prefabs/PreWater")) as GameObject;
        water.transform.localPosition = new Vector3 (0, 0, 0);
        water.transform.parent = Background.transform;
        */
        for (int x = 0; x < (maxY - minY + 1) * 2; x++) {
            GameObject water = Instantiate (Resources.Load ("Prefabs/PreWater")) as GameObject;
            if (x == 0) {
                water.transform.GetComponent<Renderer> ().material.color = new Color (0, 0.55f, 1, 0.12f);
            } else {
                water.transform.GetComponent<Renderer> ().material.color = new Color (0, 0.55f, 1, 0.04f);
            }
            water.transform.localPosition = new Vector3 (0, -x * 0.75f, 0);
            water.transform.parent = Background.transform;
        }
        BackgroundTiles = new GameObject [maxX - minX + 1, maxY - minY + 1, maxZ - minZ + 1];
        for (int x = 0; x < 100; x++) {
            int px = Random.Range (minX, maxX);
            int py = Random.Range (minY, maxY);
            int pz = Random.Range (minZ, maxZ);
            int rx = px - minX;
            int ry = py - minY;
            int rz = pz - minZ;
            if (BackgroundTiles [rx, ry, rz] == null) {
                GameObject hex = CreateAttachedTile (px, py, pz);
                BackgroundTiles [rx, ry, rz] = hex;
            } else {
                x--;
            }
        }
    }
    static private void CreateTowerBackground () {
        int minX = -1;
        int maxX = 10;
        int minY = -10;
        int maxY = -1;
        int minZ = -1;
        int maxZ = 10;
        for (int x = 0; x < (maxY - minY + 1) * 2; x++) {
            GameObject water = Instantiate (Resources.Load ("Prefabs/PreWater")) as GameObject;
            water.transform.GetComponent<Renderer> ().material.color = new Color (0.2f, 1f, 0.1f, 0.0025f);
            water.transform.localPosition = new Vector3 (0, -x, 0);
            water.transform.parent = Background.transform;
        }
        BackgroundTiles = new GameObject [maxX - minX + 1, maxY - minY + 1, maxZ - minZ + 1];
        for (int x = 0; x < 50; x++) {
            int px = Random.Range (minX, maxX);
            int py = Random.Range (minY, maxY);
            int pz = Random.Range (minZ, maxZ);
            int rx = px - minX;
            int ry = py - minY;
            int rz = pz - minZ;
            if (BackgroundTiles [rx, ry, rz] == null) {
                GameObject hex = CreateAttachedTile (px, py, pz);
                BackgroundTiles [rx, ry, rz] = hex;
            } else {
                x--;
            }
        }
    }

    static private void CreateLavaBackground () {
        int minX = -1;
        int maxX = 10;
        int minY = -2;
        int maxY = 0;
        int minZ = -1;
        int maxZ = 10;
        for (int x = 0; x < (maxY - minY + 1) * 2; x++) {
            GameObject water = Instantiate (Resources.Load ("Prefabs/PreWater")) as GameObject;
            if (x == 0) {
                water.transform.GetComponent<Renderer> ().material.color = new Color (0.85f, 0.3f, 0, 0.94f);
            } else {
                water.transform.GetComponent<Renderer> ().material.color = new Color (0.85f, 0.3f, 0, 0.4f);
            }
            water.transform.parent = Background.transform;
            water.transform.localPosition = new Vector3 (0, -0.1f - x, 0);
        }
        BackgroundTiles = new GameObject [maxX - minX + 1, maxY - minY + 1, maxZ - minZ + 1];
        for (int x = 0; x < 25; x++) {
            int px = Random.Range (minX, maxX);
            int py = Random.Range (minY, maxY);
            int pz = Random.Range (minZ, maxZ);
            int rx = px - minX;
            int ry = py - minY;
            int rz = pz - minZ;
            if (BackgroundTiles [rx, ry, rz] == null) {
                GameObject hex = CreateAttachedTile (px, py, pz);
                BackgroundTiles [rx, ry, rz] = hex;
            } else {
                x--;
            }
        }
    }

    static public GameObject CreateAttachedTile (int x, int y, int z) {
        GameObject hex = CreateTile ();
        hex.transform.parent = Background.transform;
        hex.transform.localPosition = VisualTile.TilePosition (x, y, z);
        return hex;
    }

    static public GameObject CreateTile () {
        GameObject tile = Instantiate (AppDefaults.Tile) as GameObject;
        tile.transform.localScale = new Vector3 (1, 0.3f, 1);
        GameObject add;
        Color col;

        VisualEffectScript AEffect = tile.AddComponent<VisualEffectScript> ();
        VisualEffectScript TEffect = tile.transform.Find ("Tile").gameObject.AddComponent<VisualEffectScript> ();
        switch (Theme) {
            case 1:
                if (Random.Range (0, 2) == 0) {
                    TEffect.Init (new Color (0.47f, Random.Range (0.49f, 0.55f), 0.48f), false, true);
                } else {
                    TEffect.Init (new Color (0.47f, 0.48f, Random.Range (0.49f, 0.55f)), false, true);
                }
                AEffect.Floating [0] = true;
                break;
            case 2:
                col = new Color (Random.Range (0.49f, 0.53f), Random.Range (0.49f, 0.53f), Random.Range (0.49f, 0.53f));
                TEffect.Init ((col), false, true);
                add = Instantiate (AppDefaults.Tile) as GameObject;
                add.transform.parent = tile.transform;
                add.transform.localScale = new Vector3 (0.4f, 1000f, 0.4f);
                add.transform.localPosition = new Vector3 (0, -100, 0);
                break;
            case 3:
                col = new Color (Random.Range (0.39f, 0.43f), Random.Range (0.29f, 0.33f), Random.Range (0.29f, 0.33f));
                TEffect.Init ((col), false, true);
                AEffect.Floating [0] = true;
                add = Instantiate (AppDefaults.Tile) as GameObject;
                col = new Color (Random.Range (0.93f, 1f), Random.Range (0.58f, 0.65f), Random.Range (0.28f, 0.35f), 0.7f);
                add.AddComponent<VisualEffectScript> ();
                add.GetComponent<VisualEffectScript> ().Init ((col), false, true);
                add.transform.parent = tile.transform;
                add.transform.localScale = new Vector3 (1.05f, 1, 1.05f);
                add.transform.localPosition = new Vector3 (0, -0.1f, 0);
                break;

        }
        return tile;
    }
}
