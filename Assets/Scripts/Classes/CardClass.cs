using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClass {

    public int cardNumber;

    public int tokenType;
    public int value;
    public int abilityType;
    public int abilityArea;

    VisualCard visualCard;

    public CardClass () {

    }

    public CardClass (int value, int tokenType, int abilityArea, int abilityType) {
        SetState (value, tokenType, abilityArea, abilityType);
    }

    public CardClass SetCardNumber (int cardNumber) {
        this.cardNumber = cardNumber;
        return this;
    }

    public void SetState (int value, int tokenType, int abilityArea, int abilityType) {
        this.tokenType = tokenType;
        this.value = value;
        this.abilityType = abilityType;
        this.abilityArea = abilityArea;
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

}
