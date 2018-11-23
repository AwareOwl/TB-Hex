using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass {

    public int playerNumber;
    public int score;
    public int scoreIncome;

    public PlayerPropertiesClass properties;

    public int [] topCardNumber;

    public VisualPlayer visualPlayer;

    public MoveHistoryClass LastMove;

    public PlayerClass () {

    }

    public PlayerClass (PlayerPropertiesClass properties) {
        this.properties = properties;
        this.topCardNumber = new int [properties.hand.stack.Length];
        for (int x = 0; x < topCardNumber.Length; x++) {
            topCardNumber [x] = 0;
        }
    }

    public PlayerClass (PlayerClass player) {
        this.playerNumber = player.playerNumber;
        this.score = player.score;
        this.scoreIncome = player.scoreIncome;
        this.properties = player.properties;
        if (player.topCardNumber != null) {
            this.topCardNumber = new int [player.topCardNumber.Length];
            for (int x = 0; x < topCardNumber.Length; x++) {
                this.topCardNumber [x] = player.topCardNumber [x];
            }
        }
        this.LastMove = player.LastMove;
    }

    public HandClass GetHand () {
        return properties.hand;
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
        return GetHand ().GetCard (stackNumber, topCardNumber [stackNumber]);
    }

    public void MoveTopCard (int stackNumber) {
        int topCard = topCardNumber [stackNumber];
        int stackSize = GetStackSize (stackNumber);
        topCardNumber [stackNumber] = (topCard + 1) % stackSize;
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
        int position = (stackSize - topCardNumber [stackNumber] + cardNumber) % stackSize;
        if (card.visualCard != null) {
            if (VisualMatch.instance != null) {
                VisualMatch.instance.UpdateCardVisuals (this, stackNumber, cardNumber, position);
            } else {
                UpdateCardVisuals (stackNumber, cardNumber, position);
            }
        } 
    }

    public void UpdateCardVisuals (int stackNumber, int cardNumber, int position) {
        CardClass card = GetCard (stackNumber, cardNumber);
        if (card.visualCard == null) {
            return;
        }
        GameObject anchor = card.visualCard.Anchor;
        if (anchor.GetComponent<CardAnimation> () == null) {
            anchor.AddComponent<CardAnimation> ().Init (stackNumber, GetNumberOfStacks (), position);
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

            if (playerNumber == InGameUI.MyPlayerNumber) {
                for (int x = 0; x < GetNumberOfStacks (); x++) {
                    for (int y = 0; y < GetStackSize (x); y++) {
                        CardClass card = GetCard (x, y);
                        card.EnableVisual ();
                        DelayedUpdateCardVisuals (x, y);
                    }
                }
            }
        }
    }

    public void DestroyVisuals () {
        if (visualPlayer != null) {
            for (int x = 0; x < GetNumberOfStacks (); x++) {
                for (int y = 0; y < GetStackSize (x); y++) {
                    CardClass card = GetCard (x, y);
                    if (card.visualCard != null) {
                        card.DestroyVisual ();
                    }
                }
            }
            visualPlayer = null;
        }
    }


}
