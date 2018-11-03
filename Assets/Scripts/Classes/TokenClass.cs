using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenClass {

    public VisualToken visualToken;

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

    public void EnableVisual () {
        if (visualToken == null) {
            visualToken = new VisualToken (this);
            visualToken.AddCreateAnimation ();
        }
    }

    public void RefreshVisual () {
        if (visualToken != null) {
            visualToken.SetState ();
        }
    }

    public void DestroyVisual () {
        if (visualToken != null) {
            visualToken.DestroyToken ();
        }
    }

    public TokenClass (TileClass tile, int type, int value, int owner) {
        this.tile = tile;
        SetState (type, value, owner);
    }

    public void SetState (int type, int value, int owner) {
        this.type = type;
        this.value = value;
        this.tempValue = value;
        this.owner = owner;
        RefreshVisual ();
    }

    public void DestroyToken () {
        visualToken.DestroyToken ();
    }

    public TokenClass (TokenClass tokenReference) {
        SetState (tokenReference.type, tokenReference.value, tokenReference.owner);
    }
}
