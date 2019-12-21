using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelationLogic {


    static public bool IsFilledTile (TileClass tile) {
        return tile != null && tile.IsFilledTile ();
    }

    static public bool IsEnemyTeam (int teamNumber1, int teamNumber2) {
        return teamNumber1 != teamNumber2 && teamNumber1 != 0 && teamNumber2 != 0;
    }

    static public bool IsAllyTeam (int teamNumber1, int teamNumber2) {
        return teamNumber1 == teamNumber2 && teamNumber1 != 0 && teamNumber2 != 0;
    }

}
