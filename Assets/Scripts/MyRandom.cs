using System;
using System.Collections;
using System.Collections.Generic;

public class MyRandom {
    
    private static readonly Random _global = new Random ();
    [ThreadStatic] private static Random _local;

    public int Next () {
        if (_local == null) {
            lock (_global) {
                if (_local == null) {
                    int seed = _global.Next ();
                    _local = new Random (seed);
                }
            }
        }

        return _local.Next ();
    }

    public int Range (int x, int y) {
        int output = Next ();
        output %= (y - x);
        output += x;
        return output;
    }
}