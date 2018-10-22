using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardClass {

    bool visualised = true;

    TileClass [,] tile;
    List<TileClass> tileList = new List<TileClass> ();

    public void EnableTile (int x, int y) {
        EnableTile (x, y, true);
    }
    public void DisableTile (int x, int y) {
        EnableTile (x, y, false);
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
                VisualiseTile (tile);
            }
        }
    }
    
    public BoardClass () {

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
                Debug.Log ("Wut");

                if (tile != null) {
                    tile.DestroyVisual ();
                }
            }
        }
    }

    TileClass CopyTile (TileClass fieldReference) {
        int x = fieldReference.x;
        int y = fieldReference.y;
        TileClass tile = CreateTile (x, y);
        EnableTile (x, y, fieldReference.enabled);
        return tile;
    }

    TileClass CreateTile (int x, int y) {
        tile [x, y] = new TileClass (x, y);
        VisualiseTile (tile [x, y]);
        return tile [x, y];
    }

    public void CopyBoard (BoardClass match) {
        CreateTile (match.tile.GetLength (0), match.tile.GetLength (1));
        foreach (TileClass tempField in match.tileList) {
            CopyTile (tempField);
        }
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
}
