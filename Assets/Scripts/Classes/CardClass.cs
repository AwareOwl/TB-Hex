using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClass {

    int cardNumber;

    int tokenType;
    int value;
    int ability;
    int abilityArea;

    VisualCard visualCard;

    public void SetState (int tokenType, int value, int ability, int abilityArea) {
        this.tokenType = tokenType;
        this.value = value;
        this.ability = ability;
        this.abilityArea = abilityArea;
    }

}
