using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackClass {

    bool visualised = false;
    int numberOfStacks = 4;
    public int TopCardNumber = 0;
    public int stackNumber;
    List<CardClass> Card = new List<CardClass>();

    public StackClass () {

    }
    public StackClass (int stackNumber) {
        this.stackNumber = stackNumber;
    }

    public void MoveTopCard () {
        int prevTopCardNumber = TopCardNumber;
        TopCardNumber = (TopCardNumber + 1) % Card.Count;
        if (visualised) {
            for (int x = 0; x < Card.Count; x++) {
                UpdateCardAnimation (x);
            }
            Card [prevTopCardNumber].visualCard.Anchor.GetComponent<CardAnimation> ().shuffleTimer = CardAnimation.shuffleTime;
        }
    }

    public CardClass TopCard () {
        return Card [TopCardNumber];
    }

    public void Add (CardClass card) {
        Card.Add (card);
    }

    public void EnableVisual () {
        visualised = true;
        for (int x = 0; x < Card.Count; x++) {
            CardClass card = Card [x];
            card.EnableVisual ();
            UpdateCardAnimation (x);
        }
    }

    public void UpdateCardAnimation (int cardNumber) {
        CardClass card = Card [cardNumber];
        GameObject anchor = card.visualCard.Anchor;
        int position = (Card.Count - TopCardNumber + cardNumber) % Card.Count;
        if (anchor.GetComponent<CardAnimation> () == null) {
            anchor.AddComponent<CardAnimation> ().Init (stackNumber, numberOfStacks, position);
        } else {
            anchor.GetComponent<CardAnimation> ().position = position;
        }
    }
}
