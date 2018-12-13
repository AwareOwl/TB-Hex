using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackClass {
    
    public List<CardClass> card = new List<CardClass>();

    public StackClass () {

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
            Add (card);
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
