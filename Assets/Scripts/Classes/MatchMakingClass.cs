using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchMakingClass {


    static public void CreateGame (PlayerPropertiesClass [] properties) {
        MatchClass match = new MatchClass ();
        match.NewMatch (properties.Length);
        for (int x = 0; x < properties.Length; x++) {
            match.SetPlayer (x + 1, new PlayerClass (properties [x]));
        }
        InGameUI.ShowInGameUI (match);
    }
}
