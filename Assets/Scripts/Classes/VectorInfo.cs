using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorInfo {

    public int strongestValue = 0;
    public List<AbilityVector> StrongestTargets = new List<AbilityVector>();

    public int weakestValue = 999;
    public List<AbilityVector> WeakestTargets = new List<AbilityVector> ();

    public VectorInfo () {

    }

    public VectorInfo (AbilityVector [] vectors) {
        foreach (AbilityVector vector in vectors) {
            if (vector.target != null && vector.target.IsFilledTile ()) {
                int value = vector.target.token.value;
                if (strongestValue < value) {
                    StrongestTargets = new List<AbilityVector> ();
                    strongestValue = value;
                }
                if (strongestValue == value) {
                    StrongestTargets.Add (vector);
                }
                if (weakestValue > value) {
                    WeakestTargets = new List<AbilityVector> ();
                    weakestValue = value;
                }
                if (weakestValue == value) {
                    WeakestTargets.Add (vector);
                }
            }

        }
    }
}
