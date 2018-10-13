using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardClass {

    TileClass [,] tile;
    List<TileClass> tileList = new List<TileClass> ();

    public void EnableTile (int x, int y) {
        tile [x, y].enabled = true;
        tileList.Add (tile [x, y]);
    }
    public void DisableTile (int x, int y) {
        tile [x, y].enabled = false;
        tileList.Remove (tile [x, y]);
    }

    void CreateFields (int x, int y) {
        tile = new TileClass [x, y];
    }

    public BoardClass () {

    }

    public void CreateNewBoard () {
        tile = new TileClass [8, 8];
        
    }

    TileClass CopyTile (TileClass fieldReference) {
        return CreateTile (fieldReference.x, fieldReference.y);
    }

    TileClass CreateTile (int x, int y) {
        tile [x, y] = new TileClass (x, y);
        tileList.Add (tile [x, y]);
        return tile [x, y];
    }

    public void CopyBoard (BoardClass match) {
        CreateTile (match.tile.GetLength (0), match.tile.GetLength (1));
        foreach (TileClass tempField in match.tileList) {
            CopyTile (tempField);
        }
    }
}
