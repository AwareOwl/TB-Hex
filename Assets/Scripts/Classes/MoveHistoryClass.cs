using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHistoryClass  {


    public int moveId;
    public int x;
    public int y;
    public int playerNumber;
    public int stackNumber;
    public int usedCardNumber;
    public CardClass usedCard;
    public TokenClass playedToken;

    public MoveHistoryClass prev;

    public MoveHistoryClass () {

    }

    public MoveHistoryClass (int moveId, int x, int y, int playerNumber, int stackNumber, int usedCardNumber, CardClass usedCard, TokenClass playedToken, MoveHistoryClass prev) {
        this.moveId = moveId;
        this.x = x;
        this.y = y;
        this.playerNumber = playerNumber;
        this.stackNumber = stackNumber;
        this.usedCardNumber = usedCardNumber;
        this.usedCard = usedCard;
        this.playedToken = playedToken;

        this.prev = prev;
    }

    public string [] PlayedToString () {
        List<string> s = new List<string> ();
        s.Add (moveId.ToString ());
        s.Add (playedToken.x.ToString ());
        s.Add (playedToken.y.ToString ());
        s.Add (playerNumber.ToString ());
        s.Add (stackNumber.ToString ());
        s.Add (usedCardNumber.ToString ());
        return s.ToArray();
    }

    public void PlayedLoadFromString (MatchClass match, string [] s) {
        moveId = int.Parse (s [0]);
        x = int.Parse (s [1]);
        y = int.Parse (s [2]);
        playedToken = match.Board.GetTile (x, y).token;
        playerNumber = int.Parse (s [3]);
        stackNumber = int.Parse (s [4]);
        usedCardNumber = int.Parse (s [5]);
        usedCard = match.Player [playerNumber].GetCard (stackNumber, usedCardNumber);
    }

}
