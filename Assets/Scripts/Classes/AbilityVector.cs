using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityVector {

    public int x;
    public int y;

    public int pushX;
    public int pushY;

    public int flipX;
    public int flipY;

    public AbilityVector (int sx, int sy, int relativeNumber) {
        switch (relativeNumber) {
            case 1:
                x = sx - 1;
                y = sy;
                break;
            case 2:
                x = sx + 1;
                y = sy;
                break;
            case 3:
                y = sy + 1;
                break;
            case 4:
                y = sy - 1;
                break;
            case 5:
                y = sy + 1;
                break;
            case 6:
                y = sy - 1;
                break;
        }
        switch (sy % 2) {
            case 0:
                switch (relativeNumber) {
                    case 3:
                        x = sx - 1;
                        break;
                    case 4:
                        x = sx;
                        break;
                    case 5:
                        x = sx;
                        break;
                    case 6:
                        x = sx - 1;
                        break;
                }
                break;
            case 1:
                switch (relativeNumber) {
                    case 3:
                        x = sx;
                        break;
                    case 4:
                        x = sx + 1;
                        break;
                    case 5:
                        x = sx + 1;
                        break;
                    case 6:
                        x = sx;
                        break;
                }
                break;
        }
    }

    public void Init (int sx, int sy, int tx, int ty) {
        x = tx;
        y = ty;

        pushX = tx * 2 - sx;
        pushY = ty * 2 - sy;

        flipX = sx * 2 - tx;
        flipY = sy * 2 - sy;
    }
    }
