using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass {

    public int score;
    public int scoreIncome;

    public PlayerPropertiesClass properties;

    public HandClass hand;

    public VisualPlayer visualPlayer;

    public MoveHistoryClass LastMove;

    public PlayerClass () {

    }

    public void RotateTopCard (int stackNumber) {
        GetStack (stackNumber).RotateTopAbilityArea ();
    }

    public CardClass GetLastMoveCard () {
        if (LastMove == null) {
            return null;
        }
        return GetCard (LastMove.stackNumber, LastMove.usedCardNumber);
    }

    public string [] PlayerToString () {
        List <string> s = new List<string> ();
        s.Add (score.ToString ());
        s.Add (scoreIncome.ToString ());
        return s.ToArray ();
    }

    public void LoadFromString (string [] lines) {
        score = int.Parse (lines [0]);
        scoreIncome = int.Parse (lines [1]);
    }

    public PlayerClass (PlayerPropertiesClass properties) {
        if (properties == null) {
            return;
        }
        this.properties = properties;
        this.hand = new HandClass (properties.startingHand);
    }

    public PlayerClass (PlayerClass player) {
        this.score = player.score;
        this.scoreIncome = player.scoreIncome;
        this.properties = player.properties;
        this.hand = player.hand;
        this.LastMove = player.LastMove;
    }

    public HandClass GetHand () {
        if (properties == null) {
            return null;
        }
        return hand;
    }

    public int GetNumberOfStacks () {
        return GetHand ().GetNumberOfStacks ();
    }

    public StackClass GetStack (int stackNumber) {
        return GetHand ().GetStack (stackNumber);
    }

    public int GetStackSize (int stackNumber) {
        return GetHand ().GetStackSize (stackNumber);
    }

    public CardClass GetCard (int stackNumber, int cardNumber) {
        return GetHand ().GetCard (stackNumber, cardNumber);
    }

    public CardClass GetTopCard (int stackNumber) {
        return GetHand ().stack [stackNumber].getTopCard ();
    }

    public int GetTopCardNumber (int stackNumber) {
        return GetHand ().stack [stackNumber].topCardNumber;
    }

    public void MoveTopCard (int stackNumber) {
        if (GetHand () == null) {
            return;
        }
        StackClass stack = GetStack (stackNumber);
        stack.MoveTopCard ();
        int topCard = GetTopCardNumber (stackNumber);
        int stackSize = GetStackSize (stackNumber);
        if (visualPlayer != null) {
            for (int x = 0; x < stackSize; x++) {
                DelayedUpdateCardVisuals (stackNumber, x);
            }
            DelayedShuffleCardVisual (stackNumber, topCard);
        }
    }

    public void DelayedUpdateCardVisuals (int stackNumber, int cardNumber) {
        CardClass card = GetCard (stackNumber, cardNumber);
        int stackSize = GetStackSize (stackNumber);
        int position = (stackSize - GetTopCardNumber (stackNumber) + cardNumber) % stackSize;
        if (card.visualCard != null) {
            if (VisualMatch.instance != null) {
                VisualMatch.instance.UpdateCardVisuals (this, stackNumber, stackSize, cardNumber, position);
            } else {
                UpdateCardVisuals (stackNumber, stackSize, cardNumber, position);
            }
        } 
    }

    public void UpdateCardVisuals (int stackNumber, int stackSize, int cardNumber, int position) {
        CardClass card = GetCard (stackNumber, cardNumber);
        if (card.visualCard == null) {
            return;
        }
        GameObject anchor = card.visualCard.Anchor;
        if (anchor.GetComponent<CardAnimation> () == null) {
            anchor.AddComponent<CardAnimation> ().Init (card.visualCard, stackNumber, stackSize, GetNumberOfStacks (), position);
        } else {
            anchor.GetComponent<CardAnimation> ().position = position;
        }
    }

    public void DelayedShuffleCardVisual (int stackNumber, int cardNumber) {
        CardClass card = GetCard (stackNumber, cardNumber);
        if (card.visualCard != null) {
            VisualMatch.instance.ShuffleCardVisual (this, card);
        }
    }

    public void ShuffleCardVisual (CardClass card) {
        GameObject anchor = card.visualCard.Anchor;
        anchor.GetComponent<CardAnimation> ().shuffleTimer = CardAnimation.shuffleTime;
    }

    public void SetScoreIncome (int scoreIncome) {
        this.scoreIncome = scoreIncome;
    }

    public void AddScore (int scoreToAdd) {
        this.score += scoreToAdd;
    }

    public void UpdateVisuals (MatchClass match) {
        if (visualPlayer != null) {
            visualPlayer.UpdateVisuals (match);
        }
    }

    public void EnableVisuals () {
        if (visualPlayer == null) {
            visualPlayer = new VisualPlayer ();

            if (properties.playerNumber == InGameUI.MyPlayerNumber) {
                for (int x = 0; x < GetNumberOfStacks (); x++) {
                    for (int y = 0; y < GetStackSize (x); y++) {
                        CardClass card = GetCard (x, y);
                        card.EnableVisual ();
                        VisualCard vCard = card.visualCard;
                        vCard.Background.GetComponent<UIController> ().x = x;
                        vCard.Background.name = UIString.InGameHandCard;
                        DelayedUpdateCardVisuals (x, y);
                    }
                }
            }
        }
    }

    public void DestroyVisuals () {
        if (visualPlayer != null) {
            if (hand != null) {
                for (int x = 0; x < GetNumberOfStacks (); x++) {
                    for (int y = 0; y < GetStackSize (x); y++) {
                        CardClass card = GetCard (x, y);
                        if (card.visualCard != null) {
                            card.DestroyVisual ();
                        }
                    }
                }
            }
            visualPlayer = null;
        }
    }


}
