using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenClass {
    
    public TileClass tile;

    public int type;
    public int tempValue;
    public int value;
    public int owner;
    public bool destroyed;

    public int x {
        get {
            return tile.x;
        }
    }

    public int y {
        get {
            return tile.y;
        }
    }

    public TokenClass () {

    }

    public TokenClass (int type, int value, int owner) {
        SetState (type, value, owner);
    }

    public void SetState (int type, int value, int owner) {
        this.type = type;
        this.value = value;
        this.tempValue = value;
        this.owner = owner;
    }

    public TokenClass (TokenClass tokenReference) {
        SetState (tokenReference.type, tokenReference.value, tokenReference.owner);
    }
}
