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
        //CreateNewBackground (4);
        CreateNewBackground (Random.Range (1, 5));
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
                break;
            case 4:
                CreateWaterLilyBackground ();
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
            water.transform.GetComponent<Renderer> ().material.color = new Color (0.125f, 0.225f, 0.075f, 0.001f);
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

        for (int x = 0; x < 150; x++) {
            CreateCloud ();
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
    static private void CreateWaterLilyBackground () {
        disabledHeight = -1f;
        gravity = 3f;
        int minX = -1;
        int maxX = 8;
        int minY = -4;
        int maxY = 0;
        int minZ = -1;
        int maxZ = 8;
        int deltaX = maxX - minX + 1;
        int deltaZ = maxZ - minZ + 1;
        GameObject water;
        Material material;
        for (int x = 0; x < (maxY - minY + 1) * 2; x += 2) {
            water = Instantiate (Resources.Load ("Prefabs/PreWater")) as GameObject;
            material = water.transform.GetComponent<Renderer> ().material;
            if (x == 0) {
                material.color = new Color (0.086f, 0.3f, 0.25f, 0.7f);
            } else {
                material.color = new Color (0.4f, 0.3f, 0.1f, 0.3f);
            }
            material.SetFloat ("_Glossiness", 0.7f);
            water.transform.localPosition = new Vector3 (0, -x - 0.1f, 0);
            water.transform.parent = Background.transform;
        }
        BackgroundTiles = new GameObject [deltaX, deltaZ];
        for (int x = minX; x <= maxX; x++) {
            for (int y = minZ; y <= maxZ; y++) {
                int py = Random.Range (minY, maxY);
                GameObject hex = CreateAttachedTile (x, py / 3f, y);
                BackgroundTiles [x - minX, y - minZ] = hex;
            }
        }
        for (int x = minX - 4; x <= maxX + 4; x++) {
            for (int y = minZ - 3; y <= maxZ + 3; y++) {
                if (!(x >= minX && x <= maxX && y >= minZ && y <= maxZ)) {
                    if (Random.Range (0, 4) == 0) {
                        int leaf = 0;
                        int py = Random.Range (minY, maxY);
                        GameObject hex;
                        hex = CreateObject (AppDefaults.LilyFlower, x, 0, y);
                        hex.transform.localScale = Vector3.one * 0.39f;
                        VisualEffectScript VES = hex.AddComponent<VisualEffectScript> ();
                        Color col = new Color (Random.Range (0.89f, 1f), Random.Range (0.47f, 0.50f), Random.Range (0.46f, 0.51f));
                        VES.SetColor (col);
                        VES.SetDrift (true);
                        GameObject add = Instantiate (AppDefaults.Tile) as GameObject;
                        add.transform.parent = hex.transform;
                        add.transform.localScale = new Vector3 (0.05f, 800f, 0.05f);
                        add.transform.localPosition = new Vector3 (0, -200.1f, 0);
                        VES = add.transform.Find ("Tile").gameObject.AddComponent<VisualEffectScript> ();
                        VES.SetColor (col);

                        for (int z = 0; z < 6; z++) {
                            if (Random.Range (0, 6 - z + leaf) == 0) {
                                hex = CreateAttachedTile (x, 0f, y);
                                hex.transform.localScale = Vector3.one * 0.68f;
                                hex.transform.localPosition += new Vector3 (Mathf.Sin (z * Mathf.PI / 3f) * 0.69f, 0, Mathf.Cos (z * Mathf.PI / 3f) * 0.69f);
                                leaf++;
                            }
                        }
                        x += 2;
                        y += 2;
                    }
                }
            }
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
        float minY = -10f;
        float maxY = 3f;
        switch (Theme) {
            case 2:
                material.color = new Color (Random.Range (0.85f, 0.95f), Random.Range (0.85f, 0.95f), Random.Range (0.85f, 0.95f), Random.Range (0.175f, 0.25f));
                cloud.transform.localScale = new Vector3 (Random.Range (3f, 6.5f), Random.Range (3f, 6.5f), 1);
                minY = -8;
                maxY = 2;
                break;
            case 3:
                material.color = new Color (Random.Range (0.65f, 0.9f), Random.Range (0.15f, 0.3f), Random.Range (0.35f, 0.5f), Random.Range (0.1f, 0.2f));
                cloud.transform.localScale = new Vector3 (Random.Range (2f, 6.5f), Random.Range (2f, 6.5f), 1);
                minY = -10;
                maxY = 3;
                break;

        }
        if (onBoardLoad) {
            cloud.transform.localPosition = new Vector3 (Random.Range (-18, 18f), Random.Range (-minY, maxY), Random.Range (-11, 11f));
        } else {
            cloud.transform.localPosition = new Vector3 (Random.Range (18, 18f), Random.Range (-minY, maxY), Random.Range (-11, 11f));
        }
        cloud.transform.localEulerAngles = new Vector3 (90, 0, 0);
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

    static public GameObject CreateAttachedTile (int x, float y, int z) {
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

    static public GameObject CreateObject (Object obj, int x, int y, int z) {
        GameObject Clone = Instantiate (obj) as GameObject;
        Clone.transform.localPosition = VisualTile.TilePosition (x, y, z);
        Clone.transform.parent = Background.transform;
        return Clone;
    }
    static public Color TileColor () {
        return TileColor (false);
    }

    static public Color TileColor (bool enabled) {
        int disabledMultiplier = 0;
        if (!enabled) {
            disabledMultiplier += 1;
        }
        switch (Theme) {
            case 1:
                if (Random.Range (0, 2) == 0) {
                    return new Color (0.5f, Random.Range (0.51f, 0.56f), 0.47f);
                } else {
                    return new Color (Random.Range (0.51f, 0.56f), 0.5f, 0.47f);
                }
            case 2:
                if (Random.Range (0, 2) == 0) {
                    return new Color (Random.Range (0.48f, 0.55f), 0.49f, 0.46f);
                } else {
                    return new Color (0.46f, 0.44f, Random.Range (0.46f, 0.53f));
                }
            case 3:
                if (Random.Range (0, 2) == 0) {
                    return new Color (Random.Range (0.29f, 0.34f), 0.19f, Random.Range (0.18f, 0.21f));
                } else {
                    return new Color (Random.Range (0.25f, 0.29f), 0.19f, Random.Range (0.22f, 0.26f));
                }
            case 4:
                if (Random.Range (0, 2) == 0) {
                    return new Color (Random.Range (0.38f, 0.49f), 0.61f - disabledMultiplier * 0.2f, Random.Range (0.17f, 0.22f) - disabledMultiplier * 0.1f);
                } else {
                    return new Color (Random.Range (0.21f, 0.29f), 0.54f - disabledMultiplier * 0.2f, Random.Range (0.20f, 0.26f) - disabledMultiplier * 0.1f);
                }
        }
        return new Color (1, 1, 1);
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
            case 4:
                tile = Instantiate (AppDefaults.LilyLeaf) as GameObject;
                tile.transform.localScale = new Vector3 (1, 1, 1);
                tile.transform.Find ("Tile").transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
                tile.transform.localEulerAngles = new Vector3 (0, Random.Range (0f, 360f), 0);
                break;
        }
        GameObject add;
        Color col;

        VisualEffectScript AEffect = tile.AddComponent<VisualEffectScript> ();
        AEffect.endPhase = 0;
        VisualEffectScript TEffect = tile.transform.Find ("Tile").gameObject.AddComponent<VisualEffectScript> ();
        switch (Theme) {
            case 1:
                col = TileColor ();
                TEffect.SetColor (col);
                AEffect.SetDrift (true);
                break;
            case 2:
                col = TileColor ();
                TEffect.SetColor (col);
                add = Instantiate (AppDefaults.Tile) as GameObject;
                add.transform.parent = tile.transform;
                add.transform.localScale = new Vector3 (0.4f, 1000f, 0.4f);
                add.transform.localPosition = new Vector3 (0, -250, 0);
                TEffect = add.transform.Find ("Tile").gameObject.AddComponent<VisualEffectScript> ();
                TEffect.SetColor (col);
                break;
            case 3:
                col = TileColor ();
                TEffect.SetColor (col);
                AEffect.SetDrift (true);
                break;
            case 4:
                col = TileColor ();
                TEffect.SetColor (col);
                AEffect.SetDrift (true);
                add = Instantiate (AppDefaults.Tile) as GameObject;
                add.transform.parent = tile.transform;
                add.transform.localScale = new Vector3 (0.05f, 800f, 0.05f);
                add.transform.localPosition = new Vector3 (0, -200.1f, 0);
                TEffect = add.transform.Find ("Tile").gameObject.AddComponent<VisualEffectScript> ();
                TEffect.SetColor (col);
                break;

        }
        return tile;
    }
}
