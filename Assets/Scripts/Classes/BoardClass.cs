using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardClass {

    TileClass [,] field;
    List<TileClass> fieldList = new List<TileClass> ();
    

    void CreateFields (int x, int y) {
        field = new TileClass [x, y];
    }

    TileClass CopyTile (TileClass fieldReference) {
        return CreateTile (fieldReference.x, fieldReference.y);
    }

    TileClass CreateTile (int x, int y) {
        field [x, y] = new TileClass (x, y);
        fieldList.Add (field [x, y]);
        return field [x, y];
    }

    public void CopyBoard (BoardClass match) {
        CreateTile (match.field.GetLength (0), match.field.GetLength (1));
        foreach (TileClass tempField in match.fieldList) {
            CopyTile (tempField);
        }
    }
}
