using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileClass {

    public VisualTile visualTile;

    public int x;
    public int y;

    public bool enabled;

    public TokenClass token;

    public TileClass () {

    }

    public TileClass (int x, int y) {
        SetXY (x, y);
    }

    void SetXY (int x, int y) {
        this.x = x;
        this.y = y;
    }

    public void EnableTile (bool enable) {
        enabled = enable;
        if (visualTile != null) {
            visualTile.EnableTile (enable);
        }
    }

    public void EnableVisual () {
        if (visualTile == null) {
            visualTile = new VisualTile (this);
            if (token != null) {
                token.EnableVisual ();
            }
        }
    }

    public TokenClass CreateToken (int type, int value, int owner) {
        token = new TokenClass (this, type, value, owner);
        if (visualTile != null) {
            token.EnableVisual ();
        }
        return token;
    }

    public TileClass (TileClass fieldReference) {
        SetXY (fieldReference.x, fieldReference.y);
        AttachToken (new TokenClass (fieldReference.token));
    }

    public void AttachToken (TokenClass token) {
        this.token = token;
        if (token != null) {
            token.tile = this;
        }
    }

}
