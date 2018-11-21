using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandClass  {

    int numberOfStacks;
    public StackClass [] stack;

    public HandClass () {
        Init (4);
    }

    public HandClass (int numberOfStacks) {
        Init (numberOfStacks);
    }

    public int GetNumberOfStacks () {
        return stack.Length;
    }

    public StackClass GetStack (int stackNumber) {
        return stack [stackNumber];
    }

    public int GetStackSize (int stackNumber) {
        return stack [stackNumber].card.Count;
    }

    public CardClass GetCard (int stackNumber, int cardNumber) {
        return stack [stackNumber].card [cardNumber];
    }

    public void Init (int numberOfStacks) {
        this.numberOfStacks = numberOfStacks;
        stack = new StackClass [numberOfStacks];
        for (int x = 0; x < numberOfStacks; x++) {
            stack [x] = new StackClass ();
            stack [x].stackNumber = x;
        }
    }

    public float Normalize (float value, float scale) {
        float output = value * 2;
        output += 100 / scale - 100f;
        output *= scale;
        output -= 100 / scale - 100f;
        output = Mathf.Max (value, 0.275f);
        return output;
    }

    public void GenerateRandomHand () {
        CardPoolClass CardPool = new CardPoolClass ();
        CardPool.LoadFromFile (1);
        int count = CardPool.Card.Count;

        float [] CardValue = new float [count];
        float [] modifier = new float [count];
        for (int x = 0; x < count; x++) {
            CardValue [x] = 1f;
        }

        bool [] finished = new bool [numberOfStacks];
        float SumOfValues = -1;

        for (int y = 0; y < 5; y++) {
            if (SumOfValues == 0) {
                break;
            }
            for (int x = 0; x < numberOfStacks; x++) {
                if (finished [x]) {
                    continue;
                }
                SumOfValues = 0;
                for (int z = 0; z < CardValue.Length; z++) {
                    CardClass card = CardPool.Card [z];
                    modifier [z] = CardValue [z];
                    modifier [z] *= Normalize (RatingClass.abilityOnRow [card.abilityType, card.AreaSize (), y], 30);
                    if (y > 0) {
                        CardClass prevCard = stack [x].card [y - 1];
                        modifier [z] *= Normalize (RatingClass.abilityAfterAbility [
                            card.abilityType, card.AreaSize(),
                            prevCard.abilityType, prevCard.AreaSize()], 20);
                    }
                    SumOfValues += modifier [z];
                }
                if (SumOfValues == 0) {
                    break;
                }
                float rng = Random.Range (0f, SumOfValues);
                int id = -1;
                for (int z = 0; z < count; z++) {
                    rng -= modifier [z];
                    if (rng <= 0) {
                        id = z;
                        break;
                    }
                }
                CardValue [id] = 0;
                int abilityType = CardPool.Card [id].abilityType;
                int abilityArea = CardPool.Card [id].AreaSize ();
                for (int z = 0; z < count; z++) {
                    int abilityType2 = CardPool.Card [z].abilityType;
                    int abilityArea2 = CardPool.Card [id].AreaSize ();
                    CardValue [z] *= Normalize (RatingClass.abilityAbilitySynergy [
                        Mathf.Min (abilityType, abilityType2),
                        Mathf.Min (abilityArea, abilityArea2),
                        Mathf.Max (abilityType, abilityType2),
                        Mathf.Max (abilityArea, abilityArea2)], 4);
                }

                stack [x].Add (new CardClass (CardPool.Card [id]));
                CardClass newCard = stack [x].card [y];
                if (newCard.abilityArea == 1) {
                    newCard.abilityArea = Random.Range (1, 4);
                }
                if (y > 0) {
                    finished [x] = Random.Range (0, 2) == 0;
                }
            }
        }
    }

    override public string ToString () {
        string s = "";
        for (int x = 0; x < numberOfStacks; x++) {
            for (int y = 0; y < stack [x].card.Count; y++) {
                s += GetCard (x, y) + " ";
            }
            s += System.Environment.NewLine;
        }
        return s;
    }

}
