using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelationLogic {


    static public bool IsFilledTile (TileClass tile) {
        return tile != null && tile.IsFilledTile ();
    }

    static public bool IsEnemy (TileClass tile, int playerNumber) {
        return IsFilledTile (tile) && tile.token.owner != playerNumber && tile.token.owner != 0;
    }

}
