using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClass : VisualClass {

    public int cardNumber;

    public int tokenType;
    public int value;
    public int abilityType;
    public int abilityArea;

    public VisualCard visualCard;

    public CardClass () {

    }

    public CardClass (CardClass card) {
        SetState (card.value, card.tokenType, card.abilityArea, card.abilityType);
        this.cardNumber = card.cardNumber;
    }

    public CardClass (int value, int tokenType, int abilityArea, int abilityType) {
        SetState (value, tokenType, abilityArea, abilityType);
    }

    public int AreaSize () {
        switch (abilityArea) {
            case 1:
            case 2:
            case 3:
                return 1;
            case 4:
                return 2;
            default:
                return 0;
        }
    }

    public CardClass SetCardNumber (int cardNumber) {
        this.cardNumber = cardNumber;
        return this;
    }

    public void RotateArea () {
        if (abilityArea > 0 && abilityArea < 4) {
            abilityArea = (abilityArea % 3 + 1);
        }
        SetState (value, tokenType, abilityArea, abilityType);
    }

    public void SetState (int value, int tokenType, int abilityArea, int abilityType) {
        this.tokenType = tokenType;
        this.value = value;
        this.abilityType = abilityType;
        switch (abilityType) {
            case 0:
            case 11:
                this.abilityArea = 0;
                break;
            default:
                this.abilityArea = abilityArea;
                break;
        }
    }

    public string ToString () {
        return cardNumber + " " + value + " " + tokenType + " " + abilityArea + " " + abilityType;
    }

    public void ConvertFromString (string line) {
        string [] s = line.Split (' ');
        cardNumber = int.Parse (s [0]);
        value = int.Parse (s [1]);
        tokenType = int.Parse (s [2]);
        abilityArea = int.Parse (s [3]);
        abilityType = int.Parse (s [4]);
    }

    public void EnableVisual () {
        if (visualCard == null) {
            visualCard = new VisualCard (this);
        }
    }

    public void DestroyVisual () {
        if (visualCard != null) {
            visualCard.DestroyVisual ();
            visualCard = null;
        }
    }

}
