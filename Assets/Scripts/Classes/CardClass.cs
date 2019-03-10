using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClass {

    public int cardNumber;

    public int tokenType;
    public int tokenValue;
    public int abilityType;
    public int abilityArea;

    public VisualCard visualCard;

    public CardClass () {

    }

    public CardClass (CardClass card) {
        SetState (card.tokenValue, card.tokenType, card.abilityArea, card.abilityType);
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
        SetState (tokenValue, tokenType, abilityArea, abilityType);
        if (visualCard != null) {
            visualCard.SetState (this);
        }
    }

    public int GetAbilityArea (int abilityType, int abilityArea) {
        switch (abilityType) {
            case 0:
            case 11:
            case 22:
            case 38:
                return 0;
            default:
                return abilityArea;
        }
    }

    public void DelayedSetState () {
        visualCard.DelayedSetState (tokenValue, tokenType, abilityArea, abilityType);
    }

    public void SetState (int tokenValue, int tokenType, int abilityArea, int abilityType) {
        this.tokenType = tokenType;
        this.tokenValue = tokenValue;
        this.abilityType = abilityType;
        this.abilityArea = GetAbilityArea (abilityType, abilityArea);
    }

    public string ToString () {
        return cardNumber + " " + tokenValue + " " + tokenType + " " + abilityArea + " " + abilityType;
    }

    public void ConvertFromString (string line) {
        string [] s = line.Split (' ');
        cardNumber = int.Parse (s [0]);
        tokenValue = int.Parse (s [1]);
        tokenType = int.Parse (s [2]);
        abilityType = int.Parse (s [4]);
        abilityArea = int.Parse (s [3]);
        abilityArea = GetAbilityArea (abilityType, abilityArea);
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
