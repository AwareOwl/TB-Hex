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

    public void PlayCard (int playerNumber, int stackNumber, int x, int y) {
        PlayCard (playerNumber, stackNumber, Board.tile [x, y]);
    }

    public void PlayCard (int playerNumber, int stackNumber, TileClass tile) {
        if (turnOfPlayer == playerNumber && tile.enabled && tile.token == null) {
            PlayerClass player = Player [playerNumber];
            StackClass stack = player.Hand.Stack [stackNumber];
            CardClass card = stack.TopCard ();
            PlayToken (card, playerNumber, tile);
            stack.MoveTopCard ();
            EndTurn ();
        }
    }

    public void PlayToken (CardClass card, int playerNumber, TileClass tile) {
        CreateToken (card, playerNumber, tile);
        tile.token.visualToken.AddPlayAnimation ();
    }

    public void CreateToken (CardClass card, int playerNumber, TileClass tile) {
        tile.CreateToken (card, playerNumber);
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
