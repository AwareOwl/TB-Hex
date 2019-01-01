using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPoolClass {

    int cardPoolID = 0;

    public List<CardClass> Card = new List<CardClass> ();

    public CardClass AddCard () {
        CardClass newCard = new CardClass ().SetCardNumber (cardPoolID++);
        Card.Add (newCard);
        return newCard;
    }

    public void SetCard (int cardNumber, int tokenValue, int tokenType, int abilityArea, int abilityType) {
        CardClass card = Card.Find (x => x.cardNumber == cardNumber);
        if (card == null) {
            card = AddCard ();
        } 
        card.SetState (tokenValue, tokenType, abilityArea, abilityType);
    }

    public List<string> ToString () {
        List<string> lines = new List<string> ();
        foreach (CardClass card in Card) {
            lines.Add (card.ToString ());
        }
        return lines;
    }

    public string [] CardPoolToString () {
        List<string> lines = new List<string> ();
        foreach (CardClass card in Card) {
            lines.Add (card.ToString ());
        }
        return lines.ToArray();
    }

    public void LoadFromString (string [] lines) {
        foreach (string line in lines) {
            AddCard ().ConvertFromString (line);
        }
    }

    public void LoadFromFile (int GameModeId) {
        LoadFromString (ServerData.GetCardPool (GameModeId));
    }
}
