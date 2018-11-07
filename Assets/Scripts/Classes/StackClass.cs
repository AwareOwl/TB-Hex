using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackClass {
    
    public int stackNumber;
    public List<CardClass> card = new List<CardClass>();

    public StackClass () {

    }
    public StackClass (int stackNumber) {
        this.stackNumber = stackNumber;
    }

    public int GetStackSize () {
        return card.Count;
    }

    public CardClass GetCard (int cardNumber) {
        return card [cardNumber];
    }

    public void Add (CardClass card) {
        this.card.Add (card);
    }

}
