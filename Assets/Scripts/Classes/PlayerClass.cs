using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass {

    public int score;
    public int scoreIncome;
    public int previousScoreIncome;

    public bool enabled = true;
    public bool lost = false;

    public float AIValue;


    public PlayerPropertiesClass properties;

    public HandClass hand;

    public VisualTeam visualTeam;

    public MoveHistoryClass LastMove;

    public PlayerClass () {

    }

    public void RotateTopCard (int stackNumber, int rotateAmount = 1) {
        GetStack (stackNumber).RotateTopAbilityArea (rotateAmount);
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
        s.Add (previousScoreIncome.ToString ());
        s.Add (enabled.ToString ());
        s.Add (lost.ToString ());
        return s.ToArray ();
    }

    public void LoadFromString (string [] lines) {
        score = int.Parse (lines [0]);
        scoreIncome = int.Parse (lines [1]);
        previousScoreIncome = int.Parse (lines [2]);
        enabled = Convert.ToBoolean (lines [3]);
        lost = Convert.ToBoolean (lines [4]);
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
        this.enabled = player.enabled;
        this.lost = player.lost;

        this.properties = player.properties;
        this.hand = new HandClass (player.hand);
        this.LastMove = player.LastMove;
    }

    public int GetNumberOfStacks () {
        return hand.GetNumberOfStacks ();
    }

    public StackClass GetStack (int stackNumber) {
        if (hand == null) {
            return null;
        }
        return hand.GetStack (stackNumber);
    }

    public int GetStackSize (int stackNumber) {
        return hand.GetStackSize (stackNumber);
    }

    public CardClass GetCard (int stackNumber, int cardNumber) {
        return hand.GetCard (stackNumber, cardNumber);
    }

    public CardClass GetTopCard (int stackNumber) {
        return hand.stack [stackNumber].getTopCard ();
    }

    public int GetTopCardNumber (int stackNumber) {
        return hand.stack [stackNumber].topCardNumber;
    }

    public void MoveTopCard (int stackNumber) {
        MoveTopCard (stackNumber, GetTopCardNumber (stackNumber), true, false);
    }

    public void MoveCardToTheTop (int stackNumber, int cardNumber) {
        if (hand == null || !hand.stack [stackNumber].enabled [cardNumber]) {
            return;
        }
        hand.stack [stackNumber].topCardNumber = cardNumber;
        int stackSize = GetStackSize (stackNumber);
        if (visualTeam != null) {
            for (int x = 0; x < stackSize; x++) {
                DelayedUpdateCardVisuals (stackNumber, x);
            }
            DelayedShuffleCardVisual (stackNumber, cardNumber);
        }
    }

    public void MoveTopCard (int stackNumber, int cardNumber, bool visual, bool disable) {
        if (hand == null || !hand.stack [stackNumber].enabled [cardNumber]) {
            return;
        }
        StackClass stack = GetStack (stackNumber);
        int topCard = GetTopCardNumber (stackNumber);
        stack.MoveTopCard ();
        int stackSize = GetStackSize (stackNumber);
        if (disable) {
            hand.DisableCard (stackNumber, cardNumber);
        }
        if (!visual) {
            return;
        }
        if (visualTeam != null) {
            for (int x = 0; x < stackSize; x++) {
                DelayedUpdateCardVisuals (stackNumber, x);
            }
            DelayedShuffleCardVisual (stackNumber, cardNumber);
        }
    }

    public void VisualMoveTopCard (int stackNumber, int cardNumber, bool disable) {
        if (hand == null) {
            return;
        }
        int topCard = GetTopCardNumber (stackNumber);
        int stackSize = GetStackSize (stackNumber);
        if (visualTeam != null) {
            for (int x = 0; x < stackSize; x++) {
                DelayedUpdateCardVisuals (stackNumber, x);
            }
            if (!disable) {
                DelayedShuffleCardVisual (stackNumber, cardNumber);
            } else {
                DelayedDestroyCardVisual (stackNumber, cardNumber);
            }
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
            anchor.GetComponent<CardAnimation> ().SetPosition (position);
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
        SoundManager.PlayAudioClip (MyAudioClip.ShufflingCard);
    }

    public void DelayedDestroyCardVisual (int stackNumber, int cardNumber) {
        CardClass card = GetCard (stackNumber, cardNumber);
        VisualCard vCard = card.visualCard;
        if (vCard != null) {
            VisualMatch.instance.DestroyCardVisual (vCard);
            card.visualCard = null;
        }
    }
    public void SetScoreIncome (int scoreIncome) {
        this.previousScoreIncome = this.scoreIncome;
        this.scoreIncome = scoreIncome;
    }

    public void AddScore (int scoreToAdd) {
        this.score += scoreToAdd;
    }

    public void UpdateVisuals (MatchClass match) {
        if (visualTeam != null) {
            visualTeam.DelayedUpdateVisuals (match);
        }
    }

    public void EnableVisuals () {
        if (properties == null) {
            return;
        }
        if (visualTeam == null) {
            visualTeam = new VisualTeam ();

            if (properties.playerNumber == InGameUI.myPlayerNumber) {
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
        if (visualTeam != null) {
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
            visualTeam = null;
        }
    }


}
