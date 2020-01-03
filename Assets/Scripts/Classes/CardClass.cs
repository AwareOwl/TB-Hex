using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType {
    T0 = 0,
    T1 = 1,
    T2 = 2,
    T3 = 3,
    T4 = 4,
    T5 = 5,
    T6 = 6,
    T7 = 7,
    T8 = 8,
    T9 = 9,
    T10 = 10,
    T11 = 11,
    T12 = 12,
    T13 = 13,
    T14 = 14,
    T15 = 15,
    T16 = 16,
    T17 = 17,
    T18 = 18,
    T19 = 19,
    T20 = 20,
    T21 = 21,
    T22 = 22,
    T23 = 23,
    T24 = 24,
    T25 = 25,
    T26 = 26,
    T27 = 27,
    T28 = 28,
    T29 = 29,
    T30 = 30,
    T31 = 31,
    T32 = 32,
    T33 = 33,
    T34 = 34,
    T35 = 35,
    T36 = 36,
    T37 = 37,
    T38 = 38,
    T39 = 39,
    T40 = 40,
    T41 = 41,
    T42 = 42,
    T43 = 43,
    T44 = 44,
    T45 = 45,
    T46 = 46,
    T47 = 47,
    T48 = 48,
    T49 = 49,
    T50 = 50,
    T51 = 51,
    T52 = 52,
    T53 = 53,
    T54 = 54,
    T55 = 55,
    T56 = 56,
    T57 = 57,
    T58 = 58,
    T59 = 59,
    T60 = 60,
    T61 = 61,
    T62 = 62,
    T63 = 63,
    T64 = 64,
    T65 = 65,
    T66 = 66,
    T67 = 67,
    T68 = 68,
    T69 = 69,
    T70 = 70,
    T71 = 71,
    T72 = 72,
    T73 = 73,
    T74 = 74,
    T75 = 75,
    T76 = 76,
    T77 = 77,
    T78 = 78,
    T79 = 79,
    T80 = 80,
    T81 = 81,
    T82 = 82,
    T83 = 83,
    T84 = 84,
    T85 = 85,
    T86 = 86,
    T87 = 87,
    T88 = 88,
    T89 = 89,
    T90 = 90,
    T91 = 91,
    T92 = 92,
    T93 = 93,
    T94 = 94,
    T95 = 95,
    T96 = 96,
    T97 = 97,
    T98 = 98,
    T99 = 99,
    NULL = -1
}

public class CardClass {

    public int cardNumber;

    public TokenType tokenType;
    public int tokenValue;
    public AbilityType abilityType;
    public int abilityArea;

    public VisualCard visualCard;

    public CardClass () {

    }

    public CardClass (CardClass card) {
        SetState (card.tokenValue, card.tokenType, card.abilityArea, card.abilityType);
        this.cardNumber = card.cardNumber;
    }

    public CardClass (int value, int tokenType, int abilityArea, int abilityType) {
        SetState (value, (TokenType) tokenType, abilityArea, (AbilityType) abilityType);
    }

    public CardClass (int value, TokenType tokenType, int abilityArea, AbilityType abilityType) {
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

    public void RotateArea (int rotateAmount = 1) {
        if (abilityArea > 0 && abilityArea < 4) {
            rotateAmount %= 3;
            abilityArea = (300 + abilityArea + rotateAmount - 1) % 3 + 1;
            //abilityArea = (abilityArea + rotateAmount + 301) % 3 + 1;
        }
        SetState (tokenValue, tokenType, abilityArea, abilityType);
        if (visualCard != null) {
            visualCard.SetState (this);
        }
    }


    public int GetAbilityArea (AbilityType abilityType, int abilityArea) {
        switch (abilityType) {
            case AbilityType.T0:
            case AbilityType.T11:
            case AbilityType.T22:
            case AbilityType.T38:
            case AbilityType.T45:
            case AbilityType.T49:
            case AbilityType.T61:
            case AbilityType.T62:
            case AbilityType.T72:
                return 0;
            default:
                return abilityArea;
        }
    }

    public void DelayedSetState () {
        if (visualCard != null) {
            visualCard.DelayedSetState (tokenValue, tokenType, abilityArea, abilityType);
        }
    }

    public void SetState (int tokenValue, TokenType tokenType, int abilityArea, AbilityType abilityType) {
        this.tokenType = tokenType;
        this.tokenValue = tokenValue;
        this.abilityType = abilityType;
        this.abilityArea = GetAbilityArea (abilityType, abilityArea);
    }

    public override string ToString () {
        return cardNumber + " " + tokenValue + " " + (int) tokenType + " " + abilityArea + " " + (int) abilityType;
    }

    public void ConvertFromString (string line) {
        string [] s = line.Split (' ');
        cardNumber = int.Parse (s [0]);
        tokenValue = int.Parse (s [1]);
        tokenType = (TokenType) (int.Parse (s [2]));
        abilityType = (AbilityType) (int.Parse (s [4]));
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
