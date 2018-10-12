using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileClass {

    VisualTile visualTile;

    public int x;
    public int y;

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

    public void EnableVisual () {
        if (visualTile == null) {
            visualTile = new VisualTile (this);

        }
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
