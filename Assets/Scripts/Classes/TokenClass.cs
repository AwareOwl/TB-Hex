using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TokenType {
    T0 = 0,
    T1 = 1,
    T2 = 2,
    T3 = 3,
    T4 = 4,
    T5 = 5,
    T6 = 6,
    T7 = 7,
    T8 = 8,
    T9 = 9,
    T10 = 10,
    T11 = 11,
    T12 = 12,
    T13 = 13,
    T14 = 14,
    T15 = 15,
    T16 = 16,
    T17 = 17,
    T18 = 18,
    T19 = 19,
    T20 = 20,
    T21 = 21,
    T22 = 22,
    T23 = 23,
    T24 = 24,
    T25 = 25,
    T26 = 26,
    T27 = 27,
    T28 = 28,
    T29 = 29,
    T30 = 30,
    T31 = 31,
    T32 = 32,
    T33 = 33,
    T34 = 34,
    T35 = 35,
    T36 = 36,
    T37 = 37,
    T38 = 38,
    T39 = 39,
    T40 = 40,
    T41 = 41,
    T42 = 42,
    T43 = 43,
    T44 = 44,
    T45 = 45,
    T46 = 46,
    T47 = 47,
    T48 = 48,
    T49 = 49,
    T50 = 50,
    T51 = 51,
    T52 = 52,
    T53 = 53,
    T54 = 54,
    T55 = 55,
    T56 = 56,
    T57 = 57,
    T58 = 58,
    T59 = 59,
    T60 = 60,
    T61 = 61,
    T62 = 62,
    T63 = 63,
    T64 = 64,
    T65 = 65,
    T66 = 66,
    T67 = 67,
    T68 = 68,
    T69 = 69,
    T70 = 70,
    T71 = 71,
    T72 = 72,
    T73 = 73,
    T74 = 74,
    T75 = 75,
    T76 = 76,
    T77 = 77,
    T78 = 78,
    T79 = 79,
    T80 = 80,
    T81 = 81,
    T82 = 82,
    T83 = 83,
    T84 = 84,
    T85 = 85,
    T86 = 86,
    T87 = 87,
    T88 = 88,
    T89 = 89,
    T90 = 90,
    T91 = 91,
    T92 = 92,
    T93 = 93,
    T94 = 94,
    T95 = 95,
    T96 = 96,
    T97 = 97,
    T98 = 98,
    T99 = 99,
    NULL = -1
}


public class TokenClass {

    public VisualToken visualToken;

    public BoardClass board;
    public TileClass tile;

    public TokenType type;
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

    public TokenClass (TileClass tile, TokenType type, int value, int owner) {
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
        SetValue (tempValue);
        CheckIfShouldBeDestroyed ();
    }

    public void CheckIfShouldBeDestroyed () {
        if (value <= 0) {
            destroyed = true;
        }
    }

    public void DestroyIfNecessary () {
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

    public void SetType (TokenType type) {
        this.type = type;
        if (tile != null) {
            tile.AddToTriggers ();
        }
    }

    public void ChangeType (TokenType type) {
        RemoveType ();
        SetType (type);
    }

    public void SetState (TokenType type, int value, int owner) {
        SetType (type);
        this.value = value;
        this.tempValue = value;
        this.owner = owner;
        RefreshVisual ();
    }

    public void ChangeState (TokenType type, int value, int owner) {
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
            board.NumberOfTypes [(int) type]--;
            switch (type) {
                case TokenType.T9:
                case TokenType.T15:
                case TokenType.T18:
                    //Debug.Log ("Remove");
                    board.BeforeAbilityTriggers.Remove (this);
                    break;
                case TokenType.T14:
                    board.BeforeTokenPlayedTriggers.Remove (this);
                    break;
            }
        }
    }
}
