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

    public TokenClass (TokenClass tokenReference) {
        SetState (tokenReference.type, tokenReference.value, tokenReference.owner);
    }
    public TokenClass (TileClass tile, int type, int value, int owner) {
        this.tile = tile;
        SetState (type, value, owner);
    }

    public void EnableVisual () {
        if (visualToken == null) {
            visualToken = new VisualToken (this);
            visualToken.DelayedAddCreateAnimation ();
            //visualToken.DelayedAddCreateAnimation ();
        }
    }

    public void RefreshVisual () {
        if (visualToken != null) {
            visualToken.DelayedSetState ();
        }
    }

    public void DelayedDestroyVisual () {
        if (visualToken != null) {
            visualToken.DelayedDestroyToken ();
            visualToken = null;
        }
    }

    public void DestroyVisual () {
        if (visualToken != null) {
            visualToken.DestroyToken ();
            visualToken = null;
        }
    }

    public void UpdateTempValue () {
        UpdateValue ();
    }

    public void Update () {
        if (value <= 0 || destroyed) {
            tile.DestroyToken ();
        }
    }

    public void SetValue (int value) {
        this.value = value;
        RefreshVisual ();
    }

    public void ModifyTempValue (int value) {
        this.tempValue += value;
    }

    public void SetTempValue (int value) {
        this.tempValue = value;
    }

    public void SetType (int type) {
        if (tile != null) {
            BoardClass board = tile.board;
            if (board != null) {
                board.NumberOfTypes [type]++;
            }
        }
        this.type = type;
    }

    public void ChangeType (int type) {
        BoardClass board = tile.board;
        if (board != null) {
            board.NumberOfTypes [this.type]--;
        }
        SetType (type);
    }

    public void UpdateValue () {
        SetValue (tempValue);
    }

    public void SetState (int type, int value, int owner) {
        SetType (type);
        this.value = value;
        this.tempValue = value;
        this.owner = owner;
        RefreshVisual ();
    }

    public void ChangeState (int type, int value, int owner) {
        ChangeType (type);
        this.value = value;
        this.tempValue = value;
        this.owner = owner;
        RefreshVisual ();
    }

    public void SetTile (TileClass tile) {
        this.tile = tile;
        if (visualToken != null) {
            visualToken.DelayedSetTile (tile.visualTile.Anchor);
        }
    }


    public void MoveToDisabledTile (int x, int y) {
        if (visualToken != null) {
            visualToken.DelayedMoveToDisabledTile (x, y);
            visualToken = null;
        }
        DestroyToken (x, y);
    }

    public void DestroyToken () {
        DestroyToken (x, y);
    }

    public void DestroyToken (int x, int y) {
        BoardClass board = tile.board;
        MatchClass match = board.match;
        if (match != null) {
            match.DestroyToken (this, x, y);
            board.NumberOfTypes [type]--;
        }
        tile.token = null;
        DelayedDestroyVisual ();
    }
}
