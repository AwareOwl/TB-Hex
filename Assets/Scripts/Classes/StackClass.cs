using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackClass {
    
    public List<CardClass> card = new List<CardClass>();
    public List<bool> enabled = new List<bool> ();

    public bool atLeast1Enabled = true;
    public int topCardNumber;

    public CardClass getTopCard () {
        return card [topCardNumber];
    }

    public void MoveTopCard () {
        topCardNumber = NextCardNumber ();
    }

    public int NextCardNumber () {
        int number = topCardNumber;
        int stackSize = GetStackSize ();
        do {
            number++;
            number %= stackSize;
        } while (number != topCardNumber && !enabled [number]);
        return number;
    }

    public void DisableCard (int number) {
        enabled [number] = false;
        CheckIfAtLeast1Enabled ();
        //Debug.Log (atLeast1Enabled);
    }

    public void CheckIfAtLeast1Enabled () {
        atLeast1Enabled = false;
        int count = enabled.Count;
        for (int x = 0; x < count; x++) {
            if (enabled [x]) {
                atLeast1Enabled = true;
                return;
            }
        }
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
        //Debug.Log ("Wat");
        card = new List<CardClass> ();
        for (int x = 0; x < stack.card.Count; x++) {
            Add (new CardClass (stack.GetCard (x)));
            enabled [x] = stack.enabled [x];
            //Debug.Log (enabled [x]);
        }
        this.atLeast1Enabled = stack.atLeast1Enabled;
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
        this.enabled.Add (true);
    }

}
