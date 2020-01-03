using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileClass {

    public VisualTile visualTile;

    public int x;
    public int y;

    public bool enabled;
    public bool remains;

    public TokenClass token;
    public BoardClass board;

    public TileClass () {

    }

    public TileClass (BoardClass board, TileClass tile) {
        this.SetXY (tile.x, tile.y);
        this.enabled = tile.enabled;
        this.remains = tile.remains;
        if (tile.token != null) {
            this.AttachToken (new TokenClass (tile.token));
            token.board = board;
        }
        this.board = board;
    }

    public TileClass (BoardClass board, int x, int y) {
        this.board = board;
        SetXY (x, y);
    }

    public bool IsPlayable (int playerNumber) {
        return enabled && (token == null || (token.type == TokenType.T17 && token.owner == playerNumber));
    }

    public bool IsEmptyTile () {
        return enabled && token == null;
    }

    public bool HasRemains () {
        return IsEmptyTile () && remains;
    }

    public bool IsFilledTile () {
        return enabled && token != null && !token.destroyed;
    }

    public int GetTeam () {
        return board.match.GetTeam (token.owner);
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
            visualTile.DelayedEnableTile (enable);
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
            visualTile = null;
        }
        if (token != null) {
            token.DestroyVisual ();
        }
    }

    public TokenClass SetToken (TokenType type, int value, int owner) {
        if (token == null) {
            token = CreateToken (type, value, owner);
        } else {
            token.ChangeState (type, value, owner);
        }
        return token;
    }

    public TokenClass DestroyToken () {
        if (token != null) {
            remains = true;
            if (visualTile != null) {
                visualTile.DelayedCreateRemains ();
            }
            token.DestroyToken ();
            token = null;
        }
        return token;
    }

    public void DestroyRemains () {
        remains = false;
        if (visualTile != null) {
            visualTile.DestroyRemains ();
        }
    }

    public TokenClass CreateToken (TokenType type, int value, int owner) {
        if (enabled){
            if (token == null) {
                token = new TokenClass (this, type, value, owner);
                if (visualTile != null) {
                    token.EnableVisual ();
                }
            } else if (token.type == TokenType.T17) {
                int mergedValue = token.value + value;
                DestroyToken ();
                token = new TokenClass (this, type, mergedValue, owner);
                if (visualTile != null) {
                    token.EnableVisual ();
                }
                token.ChangeType (type);
            }
        }
        return token;
    }

    public void AddToTriggers () {
        if (board != null && token != null) {
            board.NumberOfTypes [(int) token.type]++;
            if (token.tile == null) {
                Debug.Log ("code 3");
            }
            switch (token.type) {
                case TokenType.T9:
                case TokenType.T15:
                case TokenType.T18:
                    //Debug.Log (board.match.real +  " ADD");
                    board.BeforeAbilityTriggers.Add (token);
                    break;
                case TokenType.T14:
                    board.BeforeTokenPlayedTriggers.Add (token);
                    break;
            }
        }
    }

    public void AttachToken (TokenClass token) {
        if (enabled) {
            this.token = token;
            if (token != null) {
                token.SetTile (this);
            }
        } else {
            token.DestroyToken ();
        }
    }

    public void UpdateTempValue () {
        if (token != null) {
            token.UpdateTempValue ();
        }
    }

    public void CheckIfShouldBeDestroyed () {
        if (token != null) {
            token.CheckIfShouldBeDestroyed ();
        }
    }


    public void DestroyIfNecessary () {
        if (token != null) {
            token.DestroyIfNecessary ();
        }
    }
}
