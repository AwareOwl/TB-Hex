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

    public void GenerateRandomHand () {
        CardPoolClass CardPool = new CardPoolClass ();
        CardPool.LoadFromFile (1);
        int count = CardPool.Card.Count;

        float [] CardValue = new float [count];
        for (int x = 0; x < count; x++) {
            CardValue [x] = 1f;
        }

        for (int x = 0; x < numberOfStacks; x++) {
            for (int y = 0; y < 2 || (Random.Range (0, 2) == 0 && y < 5); y++) {
                float SumOfValues = 0;
                foreach (float value in CardValue) {
                    SumOfValues += value;
                }
                float rng = Random.Range (0f, SumOfValues);
                int id = -1;
                for (int z = 0; z < count; z++) {
                    rng -= CardValue [z];
                    if (rng <= 0) {
                        CardValue [z] = 0;
                        id = z;
                        break;
                    }
                }
                stack [x].Add (new CardClass (CardPool.Card [id]));
            }
        }

    }

}
