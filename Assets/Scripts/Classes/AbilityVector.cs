using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityVector {

    public int x;
    public int y;
    public TileClass target;

    public int pushX;
    public int pushY;
    public TileClass pushTarget;

    public int flipX;
    public int flipY;
    public TileClass flipTarget;

    public AbilityVector (BoardClass board, int sx, int sy, int relativeNumber) {
        switch (relativeNumber) {
            case 1:
                x = sx - 1;
                y = sy;
                pushX = sx - 2;
                pushY = sy;
                flipX = sx + 1;
                flipY = sy;
                break;
            case 2:
                x = sx + 1;
                y = sy;
                pushX = sx + 2;
                pushY = sy;
                flipX = sx - 1;
                flipY = sy;
                break;
            case 3:
                y = sy + 1;
                pushX = sx - 1;
                pushY = sy + 2;
                flipY = sy - 1;
                break;
            case 4:
                y = sy - 1;
                pushX = sx + 1;
                pushY = sy - 2;
                flipY = sy + 1;
                break;
            case 5:
                y = sy + 1;
                pushX = sx + 1;
                pushY = sy + 2;
                flipY = sy - 1;
                break;
            case 6:
                y = sy - 1;
                pushX = sx - 1;
                pushY = sy - 2;
                flipY = sy + 1;
                break;
        }
        switch (sy % 2) {
            case 0:
                switch (relativeNumber) {
                    case 3:
                        x = sx - 1;
                        flipX = sx;
                        break;
                    case 4:
                        x = sx;
                        flipX = sx - 1;
                        break;
                    case 5:
                        x = sx;
                        flipX = sx - 1;
                        break;
                    case 6:
                        x = sx - 1;
                        flipX = sx;
                        break;
                }
                break;
            case 1:
                switch (relativeNumber) {
                    case 3:
                        x = sx;
                        flipX = sx +1;
                        break;
                    case 4:
                        x = sx + 1;
                        flipX = sx;
                        break;
                    case 5:
                        x = sx + 1;
                        flipX = sx;
                        break;
                    case 6:
                        x = sx;
                        flipX = sx + 1;
                        break;
                }
                break;
        }
        if (board.IsTileInBounds (x, y)) {
            target = board.tile [x, y];
        }
        if (board.IsTileInBounds (pushX, pushY)) {
            pushTarget = board.tile [pushX, pushY];
        }
        if (board.IsTileInBounds (flipX, flipY)) {
            flipTarget = board.tile [flipX, flipY];
        }
    }
   }
