﻿using System.Collections;
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

    public void DestroyVisual () {
        if (visualToken != null) {
            visualToken.DelayedDestroyToken ();
            visualToken = null;
        }
    }

    public void Update () {
        UpdateValue ();
        if (value <= 0 || destroyed) {
            DestroyToken ();
        }
    }

    public TokenClass (TileClass tile, int type, int value, int owner) {
        this.tile = tile;
        SetState (type, value, owner);
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
        this.type = type;
    }

    public void UpdateValue () {
        SetValue (tempValue);
    }

    public void SetState (int type, int value, int owner) {
        this.type = type;
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
        DestroyToken ();
    }

    public void DestroyToken () {
        tile.token = null;
        DestroyVisual ();
    }
}
