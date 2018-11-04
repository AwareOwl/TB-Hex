using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchClass {

    public BoardClass Board;

    int turn = 1;
    int turnOfPlayer = 1;

    int numberOfPlayers;
    public PlayerClass [] Player;

    public MatchPropertiesClass Properties;


    public MatchClass () {

    }

    public void EndTurn () {
        int [] playerIncome = new int [Player.Length];

        foreach (TileClass tile in Board.tileList) {
            TokenClass token = tile.token;
            if (token != null) {
                int value = token.value;
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
        Board.LoadFromFile (Random.Range (1, 4));
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
            PlayToken (tile, card, playerNumber);
            UseAbility (playerNumber, card.abilityArea, card.abilityType, tile);
            UpdateBoard ();
            stack.MoveTopCard ();
            EndTurn ();
        }
    }

    public void UpdateBoard () {
        foreach (TileClass tile in Board.tileList) {
            tile.Update ();
        }
    }

    public void UseAbility (int playerNumber, int abilityArea, int abilityType, TileClass tile) {
        AbilityVector [] vectors = Board.GetAbilityVectors (tile.x, tile.y, abilityArea).ToArray ();
        foreach (AbilityVector vector in vectors) {
            switch (abilityType) {
                case 1:
                    ModifyTempValue (vector.target, -1);
                    break;
                case 2:
                    CreateToken (vector.target, 0, 1, playerNumber);
                    break;
            }
        }
    }

    public void ModifyTempValue (TileClass tile, int value) {
        TokenClass target = tile.token;
        if (target != null) {
            target.ModifyTempValue (value);
        }
    }

    public void PlayToken (TileClass tile, CardClass card, int playerNumber) {
        CreateToken (tile, card, playerNumber);
        tile.token.visualToken.AddPlayAnimation ();
    }

    public void CreateToken (TileClass tile, CardClass card, int playerNumber) {
        tile.CreateToken (card, playerNumber);
    }

    public void CreateToken (TileClass tile, int type, int value, int playerNumber) {
        tile.CreateToken (type, value, playerNumber);
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
