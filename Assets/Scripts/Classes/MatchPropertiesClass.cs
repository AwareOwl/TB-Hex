using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchPropertiesClass {

    public int turnLimit = 40;
    public int scoreLimit = 500;

    public string [] MatchPropertiesToString () {
        List<string> s = new List<string> ();
        s.Add (turnLimit.ToString());
        s.Add (scoreLimit.ToString ());
        return s.ToArray ();
    }

    public void LoadFromString (string [] lines) {
        turnLimit = int.Parse (lines [0]);
        scoreLimit = int.Parse (lines [1]);
    }
}
