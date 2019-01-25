using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentScript : MonoBehaviour {

    static GameObject _BackGround;

    static GameObject Background {
        get {
            if (_BackGround == null) {
                _BackGround = new GameObject ();
                _BackGround.AddComponent <EnvironmentScript>();
            }
            return _BackGround;
        }
        set {
            _BackGround = value;
        }
    }

    static public GameObject [,] BackgroundTiles;

    static public List<GameObject> Clouds = new List<GameObject> ();

    static int Theme = 1;

    static public float disabledHeight = -0.5f;
    static public float gravity = 0.5f;

    // Use this for initialization
    void Awake () {
    }

    private void Start () {
    }

    Vector3 Wind = new Vector3 (-0.4f, 0, 0);

    // Update is called once per frame
    void Update () {
        Wind = Wind * 0.999f + new Vector3 (Random.Range (-1.5f, 0), 0, Random.Range (-0.8f, 0.8f)) * 0.001f;
        foreach (GameObject cloud in Clouds) {
            cloud.transform.localPosition += Wind * Time.deltaTime;
            if (cloud.transform.localPosition.x < -18 || cloud.transform.localPosition.z < -12 || cloud.transform.localPosition.z > 12) {
                SetRandomCloudProperties (cloud, false);
            }
        }
    }

    static public void CreateNewBackground () {
        CreateNewBackground (3);
        //CreateNewBackground (Random.Range (1, 3));
    }

    static public void CreateNewBackground (int theme) {
        Clouds = new List<GameObject> ();
        DestroyImmediate (Background);
        Theme = theme;
        switch (theme) {
            case 1:
                CreateWaterBackground ();
                break;
            case 2:
                CreateTowerBackground ();
                break;
            case 3:
                CreateChalliceBackground ();
                break;/*
            case 3:
                CreateLavaBackground ();
                break;*/
        }
    }

    static public void CreateRandomBoard () {
        for (int x = 0; x < 8; x++) {
            for (int y = 0; y < 8; y++) {
                if (true) {
                    //if (Random.Range (0, 2) != 0) {
                    //VisualTile tile = new VisualTile ();
                    if (Random.Range (0, 3) == 0) {
                        CreateAttachedTile (x, 0, y);
                    }

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

                token.Base.GetComponent<VisualEffectScript> ().SetColor (AppDefaults.PlayerColor [Random.Range (0, 2)]);
            }
        }

    }

    static private void CreateWaterBackground () {
        disabledHeight = -0.5f;
        gravity = 0.5f;
        int minX = -1;
        int maxX = 8;
        int minY = -5;
        int maxY = -1;
        int minZ = -1;
        int maxZ = 8;
        /*GameObject water = Instantiate (Resources.Load ("Prefabs/PreWater")) as GameObject;
        water.transform.localPosition = new Vector3 (0, 0, 0);
        water.transform.parent = Background.transform;
        */
        for (int x = 0; x < (maxY - minY + 1) * 2; x++) {
            GameObject water = Instantiate (Resources.Load ("Prefabs/PreWater")) as GameObject;
            if (x == 0) {
                water.transform.GetComponent<Renderer> ().material.color = new Color (0, 0.55f, 1, 0.16f);
            } else {
                water.transform.GetComponent<Renderer> ().material.color = new Color (0, 0.55f, 1, 0.03f);
            }
            water.transform.localPosition = new Vector3 (0, -x * 0.75f, 0);
            water.transform.parent = Background.transform;
        }
        BackgroundTiles = new GameObject [maxX - minX + 1, maxZ - minZ + 1];
        for (int x = minX; x <= maxX; x++) {
            for (int y = minZ; y <= maxZ; y++) {
                int py = Random.Range (minY, maxY);
                GameObject hex = CreateAttachedTile (x, py, y);
                BackgroundTiles [x - minX, y - minZ] = hex;
            }
        }
    }
    static private void CreateTowerBackground () {
        disabledHeight = -1f;
        gravity = 3f;
        int minX = -1;
        int maxX = 8;
        int minY = -13;
        int maxY = -3;
        int minZ = -1;
        int maxZ = 8;
        for (int x = 0; x < (maxY - minY + 1) * 2; x++) {
            GameObject water = Instantiate (Resources.Load ("Prefabs/PreWater")) as GameObject;
            water.transform.GetComponent<Renderer> ().material.color = new Color (0.125f, 0.225f, 0.075f, 0.002f);
            water.transform.localPosition = new Vector3 (0, -x, 0);
            water.transform.parent = Background.transform;
        }
        BackgroundTiles = new GameObject [maxX - minX + 1, maxZ - minZ + 1];
        for (int x = minX; x <= maxX; x++) {
            for (int y = minZ; y <= maxZ; y++) {
                int py = Random.Range (minY, maxY);
                GameObject hex = CreateAttachedTile (x, py, y);
                BackgroundTiles [x - minX, y - minZ] = hex;
            }
        }
    }
    static private void CreateChalliceBackground () {
        disabledHeight = -1f;
        gravity = 3f;
        int minX = -1;
        int maxX = 8;
        int minY = -8;
        int maxY = -2;
        int minZ = -1;
        int maxZ = 8;
        GameObject water;
        Material material;
        for (int x = 0; x < (maxY - minY + 1) * 2; x += 2) {
            water = Instantiate (Resources.Load ("Prefabs/PreWater")) as GameObject;
            material = water.transform.GetComponent<Renderer> ().material;
            material.color = new Color (0.45f, 0.05f, 0.275f, 0.15f);
            material.SetFloat ("_Glossiness", 0.95f);
            water.transform.localPosition = new Vector3 (0, -x - 0.1f, 0);
            water.transform.parent = Background.transform;
        }
        /*water = Instantiate (Resources.Load ("Prefabs/PreWater")) as GameObject;
        water.transform.GetComponent<Renderer> ().material.color = new Color (0.2f, 0f, 0.15f, 1f);
        water.transform.localPosition = new Vector3 (0, -20, 0);
        water.transform.parent = Background.transform;*/
        BackgroundTiles = new GameObject [maxX - minX + 1, maxZ - minZ + 1];
        for (int x = minX; x <= maxX; x++) {
            for (int y = minZ; y <= maxZ; y++) {
                int py = Random.Range (minY, maxY);
                GameObject hex = CreateAttachedTile (x, py, y);
                BackgroundTiles [x - minX, y - minZ] = hex;
            }
        }

        for (int x = 0; x < 100; x++) {
            CreateCloud ();
        }
    }

    static public void CreateCloud () {
        GameObject cloud = GameObject.CreatePrimitive (PrimitiveType.Quad);
        DestroyImmediate (cloud.GetComponent<Collider> ());
        Material material;
        material = cloud.GetComponent<Renderer> ().material;
        material.shader = AppDefaults.sprite;
        material.mainTexture = AppDefaults.Cloud [Random.Range (1, 4)];
        SetRandomCloudProperties (cloud, true);
        cloud.transform.parent = Background.transform;
        Clouds.Add (cloud);
    }

    static public void SetRandomCloudProperties (GameObject cloud, bool onBoardLoad) {
        Material material = cloud.GetComponent<Renderer> ().material;
        material.color = new Color (Random.Range (0.65f, 0.9f), Random.Range (0.15f, 0.3f), Random.Range (0.35f, 0.5f), Random.Range (0.1f, 0.2f));
        if (onBoardLoad) {
            cloud.transform.localPosition = new Vector3 (Random.Range (-18, 18f), Random.Range (-10, 3f), Random.Range (-11, 11f));
        } else {
            cloud.transform.localPosition = new Vector3 (Random.Range (18, 18f), Random.Range (-10, 3f), Random.Range (-11, 11f));
        }
        cloud.transform.localEulerAngles = new Vector3 (90, 0, 0);
        cloud.transform.localScale = new Vector3 (Random.Range (2f, 6.5f), Random.Range (2f, 6.5f), 1);
    }
    /*
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
    }*/

    static public GameObject CreateAttachedTile (int x, int y, int z) {
        GameObject hex = CreateTile (x, y, z);
        hex.transform.parent = Background.transform;
        return hex;
    }
    static public GameObject CreateTile (int x, float y, int z) {
        GameObject hex = CreateTile ();
        hex.transform.localPosition = VisualTile.TilePosition (x, y, z);
        //hex.GetComponent<VisualEffectScript> ().SetPosition (VisualTile.TilePosition (x, y, z));
        return hex;
    }

    static public GameObject CreateTile () {
        GameObject tile = null;

        switch (Theme) {
            case 1:
            case 2:
                tile = Instantiate (AppDefaults.Tile) as GameObject;
                tile.transform.localScale = new Vector3 (1, 1, 1);
                tile.transform.Find ("Tile").transform.localScale = new Vector3 (0.5f, 0.5f, 0.15f);
                break;
            case 3:
                tile = Instantiate (AppDefaults.ChalliceTile) as GameObject;
                tile.transform.localScale = new Vector3 (1, 1, 1);
                tile.transform.Find ("Tile").transform.localScale = new Vector3 (0.5f, 0.5f, 0.1f);
                break;
        }
        GameObject add;
        Color col;

        VisualEffectScript AEffect = tile.AddComponent<VisualEffectScript> ();
        AEffect.endPhase = 0;
        VisualEffectScript TEffect = tile.transform.Find ("Tile").gameObject.AddComponent<VisualEffectScript> ();
        switch (Theme) {
            case 1:
                if (Random.Range (0, 2) == 0) {
                    TEffect.SetColor (new Color (0.5f, Random.Range (0.51f, 0.56f), 0.47f));
                } else {
                    TEffect.SetColor (new Color (Random.Range (0.51f, 0.56f), 0.5f, 0.47f));
                }
                AEffect.SetDrift (true);
                break;
            case 2:
                if (Random.Range (0, 2) == 0) {
                    col = new Color (Random.Range (0.48f, 0.55f), 0.49f, 0.46f);
                } else {
                    col = new Color (0.46f, 0.44f, Random.Range (0.46f, 0.53f));
                }
                TEffect.SetColor (col);
                add = Instantiate (AppDefaults.Tile) as GameObject;
                add.transform.parent = tile.transform;
                add.transform.localScale = new Vector3 (0.4f, 1000f, 0.4f);
                add.transform.localPosition = new Vector3 (0, -250, 0);
                break;
            case 3:
                if (Random.Range (0, 2) == 0) {
                    col = new Color (Random.Range (0.29f, 0.34f), Random.Range (0.18f, 0.20f), Random.Range (0.18f, 0.21f));
                } else {
                    col = new Color (Random.Range (0.25f, 0.29f), Random.Range (0.18f, 0.20f), Random.Range (0.22f, 0.26f));
                }
                TEffect.SetColor (col);
                AEffect.SetDrift (true);
                break;

        }
        return tile;
    }
}
