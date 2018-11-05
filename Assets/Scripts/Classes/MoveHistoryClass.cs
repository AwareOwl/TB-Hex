using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHistoryClass  {

    public MoveHistoryClass prev;

    public int x;
    public int y;
    public CardClass usedCard;
    public TokenClass playedToken;

    public MoveHistoryClass () {

    }

    public MoveHistoryClass (int x, int y, CardClass usedCard, TokenClass playedToken, MoveHistoryClass prev) {
        this.x = x;
        this.y = y;
        this.usedCard = usedCard;
        this.playedToken = playedToken;
        this.prev = prev;
    }

}
