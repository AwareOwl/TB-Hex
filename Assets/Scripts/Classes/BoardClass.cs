using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardClass {

    bool visualised = false;

    public int boardTemplateId;

    public TileClass [,] tile;
    public List<TileClass> tileList = new List<TileClass> ();

    public BoardClass () {

    }

    public BoardClass (BoardClass board) {
        int width = board.tile.GetLength (0);
        int height = board.tile.GetLength (1);
        this.tile = new TileClass [width, height];
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                this.tile [x, y] = new TileClass (board.tile [x, y]);
                if (tile [x, y].enabled) {
                    EnableTile (x, y, true);
                }
            }
        }
    }

    public void EnableTile (int x, int y) {
        EnableTile (x, y, true);
    }
    public void DisableTile (int x, int y) {
        EnableTile (x, y, false);
    }

    public List<TileClass> GetEmptyTiles () {
        List<TileClass> tiles = new List<TileClass> ();
        foreach (TileClass tile in tileList) {
            if (tile.IsEmptyTile ()) {
                tiles.Add (tile);
            }
        }
        return tiles;
    }

    public int GetEmptyTilesCount () {
        int count = 0;
        foreach (TileClass tile in tileList) {
            if (tile.IsEmptyTile ()) {
                count++;
            }
        }
        return count;
    }

    public void EnableTile (int x, int y, bool enable) {
        TileClass tile = this.tile [x, y];
        tile.EnableTile (enable);
        if (enable) {
            tileList.Add (tile);
        } else {
            tileList.Remove (tile);
        }
    }

    public void VisualiseTile (TileClass tile) {
        if (visualised) {
            tile.EnableVisual ();
        }

    }

    public void EnableVisualisation () {
        visualised = true;
        if (tile != null) {
            foreach (TileClass tile in tile) {
                if (tile.enabled || BoardEditorMenu.instance != null) {
                    VisualiseTile (tile);
                }
            }
        }
    }

    public void CreateNewBoard () {
        CreateNewBoard (8, 8);
    }

    public void CreateNewBoard (int sx, int sy) {
        tile = new TileClass [sx, sy];
        for (int x = 0; x < sx; x++) {
            for (int y = 0; y < sy; y++) {
                CreateTile (x, y);
            }
        }
    }

    public void DestroyAllVisuals () {
        if (tile != null) {
            foreach (TileClass tile in tile) {
                if (tile != null) {
                    tile.DestroyVisual ();
                }
            }
        }
    }

    TileClass CreateTile (int x, int y) {
        tile [x, y] = new TileClass (x, y);
        if (tile [x, y].enabled || BoardEditorMenu.instance != null) {
            VisualiseTile (tile [x, y]);
        }
        return tile [x, y];
    }

    public TokenClass SetToken (int x, int y, int type, int value, int owner) {
        return tile [x, y].SetToken (type, value, owner);
    }

    public TokenClass DestroyToken (int x, int y) {
        return tile [x, y].DestroyToken ();
    }

    public void SaveBoard (string userName, string boardName) {
        ServerData.SaveNewBoard (userName, boardName, BoardToString ().ToArray ());
    }

    public void SaveBoard (int id) {
        ServerData.SetBoard (id, BoardToString ().ToArray ());
    }

    public List <string> BoardToString () {
        List<string> s = new List<string> ();
        string s3 = "";
        foreach (TileClass tile in this.tile) {
            string s2 = "";
            s2 += tile.x + " " + tile.y;
            s2 += " ";
            s2 += tile.enabled ? 1 : 0;
            if (tile.token != null) {
                s2 += " " + tile.token.type;
                s2 += " " + tile.token.value;
                s2 += " " + tile.token.owner;
            }
            s.Add (s2);
            s3 += s2 + Environment.NewLine;
        }
        Debug.Log (s3);
        return s;
    }

    public void LoadFromFile (int id) {
        LoadBoard (ServerData.GetBoard (id), 8, 8);
        boardTemplateId = id;
    }

    public void LoadBoard (string [] board, int x, int y) {
        DestroyAllVisuals ();
        CreateNewBoard (x, y);
        foreach (string line in board) {
            string [] s = line.Split (' ');
            int [] i = Array.ConvertAll (s, Int32.Parse);
            int px = i [0];
            int py = i [1];
            if (i [2] == 1) {
                EnableTile (px, py);
            } else {
                DisableTile (px, py);
            }
            if (i.Length > 3) {
                tile [px, py].CreateToken (i [3], i [4], i [5]);
            }
        }
    }

    public TileClass GetTile (int x, int y) {
        if (IsTileInBounds (x, y)) {
            return tile [x, y];
        } else {
            return null;
        }
    }

    public bool IsTileInBounds (int x, int y) {
        if (x >= 0 && y >= 0 && x < tile.GetLength (0) && y < tile.GetLength (1)) {
            return true;
        }
        return false;
    }

    public bool IsTileEnabled (int x, int y) {
        if (IsTileInBounds (x, y) && tile [x, y].enabled) {
            return true;
        }
        return false;
    }

    public bool IsTileEnabledAndEmpty (int x, int y) {
        if (IsTileEnabled (x, y) && tile [x, y].token == null) {
            return true;
        }
        return false;
    }

    public List <AbilityVector> GetAbilityVectors (int x, int y, int abilityArea) {
        List <AbilityVector> list = new List<AbilityVector> ();
        if (abilityArea == 1 || abilityArea == 4) {
            list.Add (new AbilityVector (this, x, y, 1));
            list.Add (new AbilityVector (this, x, y, 2));
        }
        if (abilityArea == 2 || abilityArea == 4) {
            list.Add (new AbilityVector (this, x, y, 3));
            list.Add (new AbilityVector (this, x, y, 4));
        }
        if (abilityArea == 3 || abilityArea == 4) {
            list.Add (new AbilityVector (this, x, y, 5));
            list.Add (new AbilityVector (this, x, y, 6));
        }
        return list;
    }
}
