using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityClass {

    static public List<int> [] AbilityValue;

    static AbilityClass () {
        AbilityValue = new List<int> [AppDefaults.AvailableAbilities];
        for (int x = 0; x < AppDefaults.AvailableAbilities; x++) {
            AbilityValue [x] = new List<int> ();
        }
        AbilityValue [1].Add (1);
        AbilityValue [2].Add (1);
        AbilityValue [2].Add (1);
        AbilityValue [6].Add (1);
        AbilityValue [8].Add (1);
        AbilityValue [9].Add (2);
        AbilityValue [12].Add (4);
    }

}
