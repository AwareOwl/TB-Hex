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

    public bool IsEmptyTile () {
        return enabled && token == null;
    }

    public TileClass (int x, int y) {
        SetXY (x, y);
    }

    void SetXY (int x, int y) {
        this.x = x;
        this.y = y;
    }

    public void EnableTile (bool enable) {
        this.enabled = enable;
        if (!enable) {
            DestroyToken ();
        }
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

    public void DestroyVisual () {
        if (visualTile != null) {
            visualTile.DestroyVisual ();
        }
    }

    public TokenClass SetToken (int type, int value, int owner) {
        if (token == null) {
            token = CreateToken (type, value, owner);
        } else {
            token.SetState (type, value, owner);
        }
        return token;
    }

    public TokenClass DestroyToken () {
        if (token != null) {
            token.DestroyToken ();
            token = null;
        }
        return token;
    }

    public TokenClass CreateToken (TokenClass token) {
        if (token != null) {
            return CreateToken (token.type, token.value, token.owner);
        }
        return null;
    }

    public TokenClass CreateToken (CardClass card, int owner) {
        if (card != null) {
            return CreateToken (card.tokenType, card.value, owner);
        }
        return null;
    }


    public TokenClass CreateToken (int type, int value, int owner) {
        if (enabled && token == null) {
            token = new TokenClass (this, type, value, owner);
            if (visualTile != null) {
                token.EnableVisual ();
            }
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


    public void Update () {
        if (token != null) {
            token.Update ();
        }
    }
}
