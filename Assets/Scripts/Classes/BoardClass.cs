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
        Debug.Log (tile);
        for (int x = 0; x < sx; x++) {
            for (int y = 0; y < sy; y++) {
                CreateTile (x, y);
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

    public TokenClass CreateToken (int x, int y, int type, int value, int owner) {
        return tile [x, y].CreateToken (type, value, owner);
    }
}
