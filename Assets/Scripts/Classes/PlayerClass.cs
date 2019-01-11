using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass {

    public int score;
    public int scoreIncome;

    public PlayerPropertiesClass properties;

    public int [] topCardNumber;

    public VisualPlayer visualPlayer;

    public MoveHistoryClass LastMove;

    public PlayerClass () {

    }

    public string [] PlayerToString () {
        List <string> s = new List<string> ();
        s.Add (score.ToString ());
        s.Add (scoreIncome.ToString ());
        if (topCardNumber == null) {
            s.Add (0.ToString ());
        } else {
            s.Add (topCardNumber.Length.ToString ());
            for (int x = 0; x < topCardNumber.Length; x++) {
                s.Add (topCardNumber [x].ToString ());
            }
        }
        return s.ToArray ();
    }

    public void LoadFromString (string [] lines) {
        score = int.Parse (lines [0]);
        scoreIncome = int.Parse (lines [1]);
        topCardNumber = new int [int.Parse (lines [2])];
        for (int x = 0; x < topCardNumber.Length; x++) {
            topCardNumber [x] = int.Parse (lines [3 + x]);
        }
    }

    public PlayerClass (PlayerPropertiesClass properties) {
        this.properties = properties;
        this.topCardNumber = new int [properties.hand.stack.Length];
        for (int x = 0; x < topCardNumber.Length; x++) {
            topCardNumber [x] = 0;
        }
    }

    public PlayerClass (PlayerClass player) {
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
        if (properties == null) {
            return null;
        }
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
        if (GetHand () == null) {
            return;
        }
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
            anchor.AddComponent<CardAnimation> ().Init (card.visualCard, stackNumber, GetNumberOfStacks (), position);
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
            if (properties.hand != null) {
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
