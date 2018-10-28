using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchClass {

    BoardClass Board;

    int numberOfPlayers;
    PlayerClass [] Player;


    MatchClass () {

    }

    MatchClass (int numberOfPlayers) {
        this.numberOfPlayers = numberOfPlayers;
    }

    public void NewMatch () {
        SetPlayers ();
    }

    void LoadBoard () {
        Board = new BoardClass ();
        Board.LoadFromFile (1);
    }

    void SetPlayers () {
        Player = new PlayerClass [numberOfPlayers];
        for (int x = 0; x < numberOfPlayers; x++) {
            Player [x] = new PlayerClass ();
        }
    }
    
}
