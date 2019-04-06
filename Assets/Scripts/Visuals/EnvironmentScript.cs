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
    static public GameObject [,] BackgroundThemeTiles;

    static public List<GameObject> Clouds = new List<GameObject> ();

    static int Theme = 1;

    static public float disabledHeight = -0.5f;
    static public float themeDisabledHeight = -0.5f;
    static public float themeEnabledHeight = -0.5f;
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
        CreateNewBackground (5);
        //CreateNewBackground (Random.Range (1, 7));
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
                break;
            case 5:
                CreateHoneycombBackground ();
                break;
            case 6:
                CreateMoutainBackground ();
                break;
                /*
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

    static void CreateArrays (int deltaX, int deltaZ) {
        BackgroundTiles = new GameObject [deltaX, deltaZ];
        BackgroundThemeTiles = new GameObject [deltaX, deltaZ];
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
        int deltaX = maxX - minX + 1;
        int deltaZ = maxZ - minZ + 1;
        CreateArrays (deltaX, deltaZ);
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
        int deltaX = maxX - minX + 1;
        int deltaZ = maxZ - minZ + 1;
        CreateArrays (deltaX, deltaZ);
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
        int deltaX = maxX - minX + 1;
        int deltaZ = maxZ - minZ + 1;
        CreateArrays (deltaX, deltaZ);
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
        CreateArrays (deltaX, deltaZ);
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

    static private void CreateHoneycombBackground () {
        disabledHeight = -1f;
        themeDisabledHeight = -0.65f;
        themeEnabledHeight = -0.65f;
        gravity = 3f;
        int minX = -1;
        int maxX = 8;
        float minY = -0.85f;
        float maxY = -0.25f;
        int minZ = -1;
        int maxZ = 8;
        int deltaX = maxX - minX + 1;
        int deltaZ = maxZ - minZ + 1;
        GameObject water;
        Material material;
        
        CreateArrays (deltaX, deltaZ);

        water = Instantiate (Resources.Load ("Prefabs/PreWater")) as GameObject;
        material = water.transform.GetComponent<Renderer> ().material;
        material.shader = Shader.Find ("Sprites/Default");
        material.color = new Color (0.8f, 0.775f, 0.775f, 1f);
//material.color = new Color (0.12f, 0.09f, 0.06f, 1f);
        water.transform.localPosition = new Vector3 (0, -3f, 0);
        water.transform.parent = Background.transform;

        int [,] levels = GenerateRandomLevel (deltaX, deltaZ, 2);

        for (int x = minX; x <= maxX; x++) {
            for (int y = minZ; y <= maxZ; y++) {
                float py = -1;
                GameObject hex = CreateAttachedTile (x, py - levels [x - minX, y - minZ], y);
                BackgroundTiles [x - minX, y - minZ] = hex;
            }
        }

        float minTX = VisualTile.TilePosition (minX, 0, minZ).x - 1.1f;
        float minTZ = VisualTile.TilePosition (minX, 0, minZ).z - 0.7f;
        float maxTX = VisualTile.TilePosition (maxX, 0, maxZ).x + 1.1f;
        float maxTZ = VisualTile.TilePosition (maxX, 0, maxZ).z + 0.5f;

        /*
        for (int i = 0; i < 800; i++) {
            Vector3 pos = new Vector3 (Random.Range (-12f, 12), -0.1f, Random.Range (-7f, 5));
            if ((pos.x <= maxTX && pos.x >= minTX) && (pos.z <= maxTZ && pos.z >= minTZ)) {
                i--;
                continue;
            }
            
            GameObject Clone;
            Clone = Instantiate (AppDefaults.Grass) as GameObject;
            Clone.transform.localPosition = pos;
            Clone.transform.parent = Background.transform;
            Clone.transform.localScale = new Vector3 (Random.Range (0.3f, 0.7f), Random.Range (0.4f, 0.7f), 1);
            Clone.transform.localEulerAngles = new Vector3 (Random.Range (12f, 23), Random.Range (-30f, 30), Random.Range (-10f, 10));
            VisualEffectScript VES = Clone.AddComponent<VisualEffectScript> ();
            material = Clone.transform.Find ("Grass").GetComponent<Renderer> ().material;
            //material.shader = Shader.Find ("Sprites/Default");
            //Clone.transform.Find ("Tile").GetComponent<Renderer> ().material.SetFloat ("_Glossiness", 0.2f);
            //BackgroundThemeTiles [x - minX, y - minZ] = Clone;
            Color col = new Color (Random.Range (0.19f, 0.23f), Random.Range (0.22f, 0.25f), Random.Range (0.14f, 0.15f));
            VES.SetColor (col);
        }*/

        /*
        for (int x = minX; x <= maxX; x++) {
            for (int y = minZ; y <= maxZ; y++) {
                GameObject hex;
                float py = themeDisabledHeight;
                Color col = TileColorTheme (false);
                if (x == minX || x == maxX || y == minZ || y == maxZ) {
                    //py -= 0.2f;
                }
                hex = CreateObject (AppDefaults.Honeycomb, x, py - levels [x - minX, y - minZ], y);
                hex.transform.localScale = new Vector3 (1.15f, 1.5f, 1.15f);
                VisualEffectScript VES = hex.AddComponent<VisualEffectScript> ();
                Renderer renderer = hex.transform.Find ("Tile").GetComponent<Renderer> ();
                renderer.material.SetFloat ("_Glossiness", 0.00f);
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                BackgroundThemeTiles [x - minX, y - minZ] = hex;
                VES.SetColor (col);
                /*GameObject add = Instantiate (AppDefaults.Tile) as GameObject;
                add.transform.parent = hex.transform;
                add.transform.localScale = new Vector3 (0.05f, 800f, 0.05f);
                add.transform.localPosition = new Vector3 (0, -200.1f, 0);
                VES = add.transform.Find ("Tile").gameObject.AddComponent<VisualEffectScript> ();
                VES.SetColor (col);*/

           /* }
        }*/
    }

    static public void CreateMoutainBackground () {
        disabledHeight = -1f;
        themeDisabledHeight = -0.65f;
        themeEnabledHeight = -1;
        gravity = 3f;
        int minX = -1;
        int maxX = 8;
        float minY = -1.7f;
        float maxY = -0.9f;
        int minZ = -1;
        int maxZ = 8;
        int deltaX = maxX - minX + 1;
        int deltaZ = maxZ - minZ + 1;
        GameObject water;
        Material material;

        CreateArrays (deltaX, deltaZ);

        /*water = Instantiate (Resources.Load ("Prefabs/PreWater")) as GameObject;
        material = water.transform.GetComponent<Renderer> ().material;
        material.color = new Color (0.125f, 0.225f, 0.075f, 0.4f);
        water.transform.localPosition = new Vector3 (0, -10, 0);
        water.transform.parent = Background.transform;*/

        for (int x = -10; x < -5; x++) {
            water = Instantiate (Resources.Load ("Prefabs/PreWater")) as GameObject;
            material = water.transform.GetComponent<Renderer> ().material;
            material.color = new Color (0.125f, 0.225f, 0.075f, 0.1f);
            water.transform.localPosition = new Vector3 (0, x, 0);
            water.transform.parent = Background.transform;
        }

        int margin = 5;

        for (int x = minX - margin; x <= maxX + margin; x++) {
            for (int y = minZ - margin; y <= maxZ + margin; y++) {
                float py = Random.Range (minY, maxY);
                float delta = 0;
                if (x < minX) {

                    delta += minX - x;
                    //delta = x - minX;
                }
                if (x > maxX) {
                    //delta = maxX - x;
                    delta +=  x - maxX;
                }
                if (y < minZ) {
                    delta +=  minZ - y;
                    //delta = y - minZ;
                }
                if (y > maxZ) {
                    //delta = maxZ - y;
                    delta +=  y - maxZ;
                }
                delta /= 2;
                delta = Mathf.Pow (delta, 2);
                py -= delta;
                GameObject hex = CreateAttachedTile (x, py, y);
                if (x >= minX && x <= maxX && y >= minZ && y <= maxZ) {
                    BackgroundTiles [x - minX, y - minZ] = hex;
                }
            }
        }
        for (int x = 0; x < 50; x++) {
            CreateCloud ();
        }
    }
    

    static public int [,] GenerateRandomLevel (int sizeX, int sizeY, int levels) {
        int [,] value = new int [sizeX, sizeY];
        int leftIterations = (sizeX * sizeY) * 6 / 10;
        int startX = Random.Range (0, sizeX);
        int startY = Random.Range (0, sizeY);
        List<Vector2Int> pos = new List<Vector2Int> ();
        pos.Add (new Vector2Int (startX, startY));
        value [startX, startY] = 1;
        while (leftIterations > 0) {
            int index = Random.Range (0, pos.Count);
            int curX = pos [index].x;
            int curY = pos [index].y;
            value [curX, curY] = 1;
            if (curX > 0) {
                if (value [curX - 1, curY] == 0) {
                    pos.Add (new Vector2Int (curX - 1, curY));
                }
            }
            if (curY > 0) {
                if (value [curX, curY - 1] == 0) {
                    pos.Add (new Vector2Int (curX, curY - 1));
                }
            }
            if (curX + 1 < sizeX) {
                if (value [curX + 1, curY] == 0) {
                    pos.Add (new Vector2Int (curX + 1, curY));
                }
            }
            if (curY + 1 < sizeY) {
                if (value [curX, curY + 1] == 0) {
                    pos.Add (new Vector2Int (curX, curY + 1));
                }
            }
            pos.RemoveAt (index);
            leftIterations--;
        }
        return value;
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
                minY = -4;
                maxY = 2;
                break;
            case 3:
                material.color = new Color (Random.Range (0.65f, 0.9f), Random.Range (0.15f, 0.3f), Random.Range (0.35f, 0.5f), Random.Range (0.1f, 0.2f));
                cloud.transform.localScale = new Vector3 (Random.Range (2f, 6.5f), Random.Range (2f, 6.5f), 1);
                minY = -5;
                maxY = 3;
                break;
            case 6:
                material.color = new Color (Random.Range (0.85f, 0.95f), Random.Range (0.85f, 0.95f), Random.Range (0.85f, 0.95f), Random.Range (0.3f, 0.5f));
                cloud.transform.localScale = new Vector3 (Random.Range (3f, 8.2f), Random.Range (3, 8.2f), 1);
                minY = -6;
                maxY = -2;
                break;

        }
        if (onBoardLoad) {
            cloud.transform.localPosition = new Vector3 (Random.Range (-20, 20f), Random.Range (minY, maxY), Random.Range (-11, 11f));
        } else {
            cloud.transform.localPosition = new Vector3 (Random.Range (20, 20f), Random.Range (minY, maxY), Random.Range (-11, 11f));
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
        if (BackgroundTiles != null && x >= 0 && z >= 0 && x < BackgroundTiles.GetLength (0) - 1 && z < BackgroundTiles.GetLength (1) - 1 && BackgroundTiles [x + 1, z + 1] != null) {
            return BackgroundTiles [x + 1, z + 1];
        }
        GameObject hex = CreateTile ();
        hex.transform.localPosition = VisualTile.TilePosition (x, y, z);
        //hex.GetComponent<VisualEffectScript> ().SetPosition (VisualTile.TilePosition (x, y, z));
        return hex;
    }

    static public GameObject CreateObject (Object obj, int x, float y, int z) {
        GameObject Clone = Instantiate (obj) as GameObject;
        Clone.transform.localPosition = VisualTile.TilePosition (x, y, z);
        Clone.transform.parent = Background.transform;
        return Clone;
    }

    static public Color TileColorTheme () {
        return TileColorTheme (false);
    }

    static public Color TileColorTheme (bool enabled) {
        return TileColorTheme (enabled, Theme);
    }

    static public Color ConvertToTileColorTheme (Color col) {
        switch (Theme) {
            case 5:
                return col + new Color (0.04f, 0.06f, 0, 0);
            default:
                return col;
        }
    }

    static public Color TileColorTheme (bool enabled, int theme) {
        int disabledMultiplier = 0;
        if (!enabled) {
            disabledMultiplier = 1;
        }
        switch (theme) {
            case 5:
                return new Color (Random.Range (0.59f, 0.61f) - disabledMultiplier * 0.28f, Random.Range (0.51f, 0.52f) - disabledMultiplier * 0.24f, 0.275f - disabledMultiplier * 0.11f);
            default:
                return new Color (1, 1, 1);
        }
    }

    static public Color TileColorMain () {
        return TileColorMain (false);
    }

    static public Color TileColorMain (bool enabled) {
        return TileColorMain (enabled, Theme);
    }

    static public Color TileColorMain (bool enabled, int theme) {
        int disabledMultiplier = 0;
        if (!enabled) {
            disabledMultiplier = 1;
        }
        switch (theme) {
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
            case 5:
                if (Random.Range (0, 2) == 0) {
                    return new Color (Random.Range (0.61f, 0.65f) * (1 - disabledMultiplier * 0.08f), Random.Range (0.38f, 0.46f) * (1 - disabledMultiplier * 0.11f), 0.07f * (1 + disabledMultiplier * 2f));
                } else {
                    return new Color (Random.Range (0.60f, 0.63f) * (1 - disabledMultiplier * 0.07f), Random.Range (0.45f, 0.47f) * (1 - disabledMultiplier * 0.12f), 0.07f * (1 + disabledMultiplier * 2f));
                }
            case 6:
                float sc = Random.Range (0.5f, 0.65f) * (1 - disabledMultiplier * 0.425f);
                if (Random.Range (0, 2) == 0) {
                    return new Color (sc * 0.96f, sc * 0.96f, sc * 1.08f);
                    //return new Color (Random.Range (0.61f, 0.63f) * (1 - disabledMultiplier * 0.39f), Random.Range (0.35f, 0.45f) * (1 - disabledMultiplier * 0.31f), 0.13f - disabledMultiplier * 0.06f);
                } else {
                    return new Color (sc * 0.92f, sc, sc);
                    //return new Color (Random.Range (0.57f, 0.60f) * (1 - disabledMultiplier * 0.39f), Random.Range (0.43f, 0.49f) * (1 - disabledMultiplier * 0.33f), 0.14f - disabledMultiplier * 0.07f);
                }
                break;
            default:
                return new Color (1, 1, 1);
        }
    }

    static public GameObject CreateTile () {
        GameObject tile = null;

        float scale1 = 0;
        float scale2 = 0;

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
            case 5:
                tile = Instantiate (AppDefaults.Tile) as GameObject;
                tile.transform.localScale = new Vector3 (1, 1, 1);
                tile.transform.Find ("Tile").transform.localScale = new Vector3 (0.55f, 0.55f, 0.15f);
                break;
            case 6:
                tile = Instantiate (AppDefaults.Moutain) as GameObject;
                tile.transform.localScale = new Vector3 (1, 1, 1);
                scale1 = Random.Range (0.9f, 2f);
                scale2 = Random.Range (1.9f, 2.2f);
                tile.transform.Find ("Tile").transform.localScale = new Vector3 (scale2, scale2, scale1);
                tile.transform.Find ("Tile").transform.localPosition = new Vector3 (0, -scale1 / 2, 0);
                break;
        }
        GameObject add;
        Color col;

        VisualEffectScript AEffect = tile.AddComponent<VisualEffectScript> ();
        AEffect.endPhase = 0;
        VisualEffectScript TEffect = tile.transform.Find ("Tile").gameObject.AddComponent<VisualEffectScript> ();
        switch (Theme) {
            case 1:
                col = TileColorMain ();
                TEffect.SetColor (col);
                AEffect.SetDrift (true);
                break;
            case 2:
                col = TileColorMain ();
                TEffect.SetColor (col);
                add = Instantiate (AppDefaults.Tile) as GameObject;
                add.transform.parent = tile.transform;
                add.transform.localScale = new Vector3 (0.4f, 1000f, 0.4f);
                add.transform.localPosition = new Vector3 (0, -250, 0);
                TEffect = add.transform.Find ("Tile").gameObject.AddComponent<VisualEffectScript> ();
                TEffect.SetColor (ConvertToTileColorTheme (col));
                break;
            case 3:
                col = TileColorMain ();
                TEffect.SetColor (col);
                AEffect.SetDrift (true);
                break;
            case 4:
                col = TileColorMain ();
                TEffect.SetColor (col);
                AEffect.SetDrift (true);
                add = Instantiate (AppDefaults.Tile) as GameObject;
                add.transform.parent = tile.transform;
                add.transform.localScale = new Vector3 (0.05f, 800f, 0.05f);
                add.transform.localPosition = new Vector3 (0, -200.1f, 0);
                TEffect = add.transform.Find ("Tile").gameObject.AddComponent<VisualEffectScript> ();
                TEffect.SetColor (ConvertToTileColorTheme (col));
                break;
            case 5:
                col = TileColorMain ();
                TEffect.SetColor (col);
                tile.transform.Find ("Tile").GetComponent<Renderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                add = Instantiate (AppDefaults.Honeycomb) as GameObject;
                add.transform.localScale = new Vector3 (1.15f, 0.7f, 1.15f);
                add.transform.parent = tile.transform;
                add.transform.localPosition = new Vector3 (0, -0.25f, 0);
                VisualEffectScript VES = add.transform.Find ("Tile").gameObject.AddComponent<VisualEffectScript> ();
                Renderer renderer = add.transform.Find ("Tile").GetComponent<Renderer> ();
                renderer.material.SetFloat ("_Glossiness", 0.00f);
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                add.name = "Add";
                VES.SetColor (ConvertToTileColorTheme (col));
                //tile.transform.Find ("Tile").GetComponent<Renderer> ().material.SetFloat ("_Glossiness", 0.2f);
                break;
            case 6:
                col = TileColorMain ();
                //tile.transform.Find ("Tile").GetComponent<Renderer> ().receiveShadows = false;
                tile.transform.Find ("Tile").GetComponent<Renderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                TEffect.SetColor (col);
                add = Instantiate (AppDefaults.Tile) as GameObject;
                add.transform.parent = tile.transform;
                add.transform.localScale = new Vector3 (scale2, 1000f, scale2);
                add.transform.localPosition = new Vector3 (0, -250 - scale1, 0);
                //add.transform.Find ("Tile").GetComponent<Renderer> ().receiveShadows = false;
                add.transform.Find ("Tile").GetComponent <Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                add.name = "Add";
                TEffect = add.transform.Find ("Tile").gameObject.AddComponent<VisualEffectScript> ();
                TEffect.SetColor (ConvertToTileColorTheme (col));
                break;

        }
        return tile;
    }
}
