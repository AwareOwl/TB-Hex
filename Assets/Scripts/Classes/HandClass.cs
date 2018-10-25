using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandClass  {

    int numberOfStacks;
    List<CardClass> [] Stack;

    HandClass () {
        Init (4);
    }

    HandClass (int numberOfStacks) {
        Init (numberOfStacks);
    }

    public void Init (int numberOfStacks) {
        this.numberOfStacks = numberOfStacks;
        Stack = new List<CardClass> [numberOfStacks];
        for (int x = 0; x < numberOfStacks; x++) {
            Stack [x] = new List<CardClass> ();
        }
    }

}
