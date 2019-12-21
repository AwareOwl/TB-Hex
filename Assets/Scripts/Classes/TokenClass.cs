using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenClass {

    public VisualToken visualToken;

    public BoardClass board;
    public TileClass tile;

    public int type;
    public int tempValue;
    public int value;
    public int owner;
    public bool destroyed;

    int _x;
    int _y;

    public int x {
        get {
            if (tile != null) {
                return tile.x;
            } else {
                return _x;
            }
        }
    }

    public int y {
        get {
            if (tile != null) {
                return tile.y;
            } else {
                return _y;
            }
        }
    }

    public TokenClass () {

    }

    public TokenClass (TokenClass tokenReference) {
        SetState (tokenReference.type, tokenReference.value, tokenReference.owner);
    }

    public TokenClass (TileClass tile, int type, int value, int owner) {
        this.tile = tile;
        if (tile != null) {
            tile.token = this;
            board = tile.board;
        }
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
    public void SetOwner (int owner) {
        this.owner = owner;
    }

    public void SetTempValue (int value) {
        this.tempValue = value;
    }

    public void RemoveType () {
        RemoveFromTriggers ();
    }

    public void SetType (int type) {
        this.type = type;
        if (tile != null) {
            tile.AddToTriggers ();
        }
    }

    public void ChangeType (int type) {
        RemoveType ();
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
        /*Debug.Log (tile);
        Debug.Log (tile.board);
        Debug.Log (tile.board.tile.GetLength (0));
        Debug.Log (tile.board.tile.GetLength (1));
        Debug.Log (x);
        Debug.Log (y);
        tile = tile.board.tile [x, y];*/
        _x = x;
        _y = y;
        tile.token = null;
        tile = null;
        DestroyToken (x, y);
    }

    public void DestroyToken () {
        DestroyToken (x, y);
    }

    public void DestroyToken (int x, int y) {
            //Debug.Log ("Destroy token");
        if (board == null) {
            return;
        }
        MatchClass match = board.match;
        if (match != null) {
                //Debug.Log (match.real + " Destroy token");
            match.DestroyToken (this, x, y);
            //board.NumberOfTypes [type]--;
        }
        if (tile != null) {
            tile.token = null;
        }
        DelayedDestroyVisual ();
    }

    public void RemoveFromTriggers () {
        if (board != null) {
            board.NumberOfTypes [type]--;
            switch (type) {
                case 9:
                case 15:
                case 18:
                    //Debug.Log ("Remove");
                    board.BeforeAbilityTriggers.Remove (this);
                    break;
                case 14:
                    board.BeforeTokenPlayedTriggers.Remove (this);
                    break;
            }
        }
    }
}
