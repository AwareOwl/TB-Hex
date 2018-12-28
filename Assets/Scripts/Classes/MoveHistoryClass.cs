using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHistoryClass  {

    public MoveHistoryClass prev;

    public int moveId;
    public int x;
    public int y;
    public int playerNumber;
    public int stackNumber;
    public CardClass usedCard;
    public TokenClass playedToken;

    public MoveHistoryClass () {

    }

    public MoveHistoryClass (int moveId, int x, int y, int playerNumber, int stackNumber, CardClass usedCard, TokenClass playedToken, MoveHistoryClass prev) {
        this.moveId = moveId;
        this.x = x;
        this.y = y;
        this.playerNumber = playerNumber;
        this.stackNumber = stackNumber;
        this.usedCard = usedCard;
        this.playedToken = playedToken;
        this.prev = prev;
    }

}
