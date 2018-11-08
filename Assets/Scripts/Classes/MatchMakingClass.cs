using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchMakingClass {

    static public List<MatchClass> matches = new List<MatchClass> ();

    static public MatchClass FindMatch (string accountName) {
        foreach (MatchClass match in matches) {
            foreach (PlayerClass player in match.Player) {
                if (player.properties != null && player.properties.accountName == accountName) {
                    return match;
                }
            }
        }
        return null;
    }


    static public void CreateGame (PlayerPropertiesClass [] properties) {
        MatchClass match = new MatchClass ();
        match.NewMatch (properties.Length);
        for (int x = 0; x < properties.Length; x++) {
            match.SetPlayer (x + 1, new PlayerClass (properties [x]));
        }
        matches.Add (match);
        InGameUI.ShowInGameUI (match);
    }
}
