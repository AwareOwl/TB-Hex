using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackClass {
    
    public List<CardClass> card = new List<CardClass>();

    public int topCardNumber;

    public CardClass getTopCard () {
        return card [topCardNumber];
    }

    public void MoveTopCard () {

        int topCard = topCardNumber;
        int stackSize = GetStackSize ();
        topCardNumber = (topCard + 1) % stackSize;
    }

    public void RotateTopAbilityArea () {
        RotateAbilityArea (topCardNumber);
    }

    public void RotateAbilityArea (int cardNumber) {
        card [cardNumber].RotateArea ();
    }

    public StackClass () {

    }

    public StackClass (StackClass stack) {
        card = new List<CardClass> ();
        for (int x = 0; x < stack.card.Count; x++) {
            card.Add (new CardClass (stack.GetCard (x)));
        }
    }

    public int GetStackSize () {
        return card.Count;
    }

    public CardClass GetCard (int cardNumber) {
        return card [cardNumber];
    }

    public void SetCard (int index, CardClass card) {
        while (this.card.Count < index) {
            Add (null);
        }
        if (this.card.Count == index) {
            Add (new CardClass (card));
        } else {
            this.card [index] = card;
        }
    }

    public void RemoveCard (int index) {
        if (card.Count >= index) {
            card [index] = null;
        }
    }

    public void Add (CardClass card) {
        this.card.Add (card);
    }

}
