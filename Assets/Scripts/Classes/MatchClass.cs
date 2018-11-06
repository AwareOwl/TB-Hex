﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchClass {

    public BoardClass Board;

    int turn = 1;
    int turnOfPlayer = 1;

    int numberOfPlayers;
    public PlayerClass [] Player;

    MoveHistoryClass LastMove;

    public MatchPropertiesClass Properties;


    public MatchClass () {

    }

    public void EndTurn () {
        int [] playerIncome = new int [Player.Length];

        foreach (TileClass tile in Board.tileList) {
            TokenClass token = tile.token;
            if (token != null) {
                int value = token.value;
                if (token.type == 1) {
                    value *= 2;
                }
                playerIncome [token.owner] += value;
            }
        }
        for (int x = 0; x < Player.Length; x++) {
            PlayerClass player = Player [x];
            player.SetScoreIncome (playerIncome [x]);
            player.AddScore (playerIncome [x]);
            player.UpdateVisuals (this);
        }

        turnOfPlayer = Mathf.Max (1, (turnOfPlayer + 1) % numberOfPlayers);
        turn++;
        if (turn >= Properties.turnLimit) {
            FinishGame ();
        }
    }

    public void FinishGame () {

    }

    public MatchClass (int numberOfPlayers) {
        this.numberOfPlayers = numberOfPlayers;
    }

    public void NewMatch () {
        Properties = new MatchPropertiesClass ();
        SetPlayers (null);

        Board = new BoardClass ();
        Board.LoadFromFile (Random.Range (1, 5));
    }

    public void MoveTopCard (int playerNumber, int stackNumber) {
        Player [playerNumber].MoveTopCard (stackNumber);
    }

    public void PlayCard (int x, int y, int playerNumber, int stackNumber) {
        PlayCard (Board.tile [x, y], playerNumber, stackNumber);
    }

    public void PlayCard (TileClass tile, int playerNumber, int stackNumber) {
        if (turnOfPlayer == playerNumber && tile.enabled && tile.token == null) {
            PlayerClass player = Player [playerNumber];
            StackClass stack = player.Hand.Stack [stackNumber];
            CardClass card = stack.TopCard ();
            TokenClass token = PlayToken (tile, card, playerNumber);
            UseAbility (playerNumber, card.abilityArea, card.abilityType, tile);
            SaveLastMove (tile.x, tile.y, card, token, playerNumber);
            UpdateBoard ();
            stack.MoveTopCard ();
            EndTurn ();
        }
    }

    public void SaveLastMove (int x, int y, CardClass playedCard, TokenClass token, int playerNumber) {
        LastMove = new MoveHistoryClass (x, y, playedCard, token, LastMove);
        Player [playerNumber].LastMove = new MoveHistoryClass (x, y, playedCard, token, Player [playerNumber].LastMove);
    }

    public void UpdateBoard () {
        foreach (TileClass tile in Board.tileList) {
            tile.Update ();
        }
    }

    public void UseAbility (int playerNumber, int abilityArea, int abilityType, TileClass tile) {
        if (abilityType == 7) {
            if (LastMove != null && LastMove.usedCard.abilityType != abilityType) {
                abilityType = LastMove.usedCard.abilityType;
            }
        }
        AbilityVector [] vectors = Board.GetAbilityVectors (tile.x, tile.y, abilityArea).ToArray ();
        foreach (AbilityVector vector in vectors) {
            switch (abilityType) {
                case 1:
                    ModifyTempValue (vector.target, -1);
                    break;
                case 2:
                    ModifyTempValue (vector.target, 1);
                    CreateToken (vector.target, 0, 1, playerNumber);
                    break;
                case 3:
                    DisableEmptyTile (vector.target);
                    break;
                case 4:
                    Player [playerNumber].AddScore (GetTokenValue (vector.target));
                    break;
                case 5:
                    MoveToken (vector.target, vector.pushTarget);
                    break;
                case 6:
                    if (vector.target != null && vector.target.IsEmptyTile ()) {
                        ModifyTempValue (tile, -1);
                    }
                    break;
                case 8:
                    if (vector.target != null && vector.target.IsFilledTile () && LastPlayedToken () != null) {
                        ModifyTempValue (LastPlayedToken ().tile, -1);
                    }
                    break;
                case 10:
                    if (vector.target != null && vector.target.IsFilledTile ()) {
                        vector.target.token.SetType (0);
                    }
                    break;
            }
        }
        switch (abilityType) {
            case 11:
                SwapToken (tile, LastPlayedTile ());
                break;
        }
    }

    public TileClass LastPlayedTile () {
        TokenClass token = LastPlayedToken ();
        if (token != null) {
            return token.tile;
        }
        return null;
    }

    public TokenClass LastPlayedToken () {
        if (LastMove == null) {
            return null;
        }
        return LastMove.playedToken;
    }

    public void ModifyTempValue (TileClass tile, int value) {
        if (tile != null) {
            TokenClass target = tile.token;
            if (target != null) {
                target.ModifyTempValue (value);
            }
        }
    }

    public TokenClass PlayToken (TileClass tile, CardClass card, int playerNumber) {
        if (tile != null) {
            TokenClass token = CreateToken (tile, card, playerNumber);
            tile.token.visualToken.AddPlayAnimation ();
            return token;
        }
        return null;
    }

    public TokenClass CreateToken (TileClass tile, CardClass card, int playerNumber) {
        if (tile != null) {
            return tile.CreateToken (card, playerNumber);
        }
        return null;
    }

    public TokenClass CreateToken (TileClass tile, int type, int value, int playerNumber) {
        if (tile != null) {
            return tile.CreateToken (type, value, playerNumber);
        }
        return null;
    }

    public TokenClass CreateToken (TileClass tile, TokenClass token) {
        if (tile != null) {
            return tile.CreateToken (token);
        }
        return null;
    }

    public void DisableEmptyTile (TileClass tile) {
        if (tile != null && tile.token == null) {
            DisableTile (tile);
        }
    }

    public void DisableTile (TileClass tile) {
        if (tile != null) {
            tile.EnableTile (false);
        }
    }

    public void MoveToken (TileClass sourceTile, TileClass destinationTile) {
        if (sourceTile != null && sourceTile.token != null) {
            TokenClass sourceToken = sourceTile.token;
            if (destinationTile != null && destinationTile.enabled) {
                TokenClass destinationToken = destinationTile.token;
                if (destinationToken == null) {
                    sourceTile.AttachToken (null);
                    //CreateToken (destinationTile, sourceToken);
                    destinationTile.AttachToken (sourceToken);
                }
            } else {
                DestroyToken (sourceTile);
            }
        }
    }

    public void SwapToken (TileClass sourceTile, TileClass destinationTile) {
        if (sourceTile != null && sourceTile.token != null) {
            TokenClass sourceToken = sourceTile.token;
            if (destinationTile != null && destinationTile.enabled) {
                TokenClass destinationToken = destinationTile.token;
                if (destinationToken != null) {
                    sourceTile.AttachToken (destinationToken);
                    destinationTile.AttachToken (sourceToken);
                }
            }
        }
    }

    public void DestroyToken (TileClass tile) {
        if (tile != null && tile.token != null) {
            tile.token.DestroyToken ();
        }
    }

    public int GetTokenValue (TileClass tile) {
        if (tile != null && tile.token != null) {
            return tile.token.value;
        }
        return 0;
    }

    void LoadBoard () {
        Board = new BoardClass ();
        Board.LoadFromFile (1);
    }

    void SetPlayers (MatchClass match) {
        Player = new PlayerClass [numberOfPlayers + 1];
        for (int x = 0; x <= numberOfPlayers; x++) {
            Player [x] = new PlayerClass (x);
            if (match == null) {
                Player [x].Hand.GenerateRandomHand ();
            }
        }
        EnableVisuals ();
    }

    public void EnableVisuals () {
        for (int x = 1; x < Player.Length; x++) {
            Player [x].EnableVisuals ();
        }
    }
    
}
