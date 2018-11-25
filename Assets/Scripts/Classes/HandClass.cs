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

    public bool IsValid () {
        bool valid = true;
        for (int x = 0; x < stack.Length; x++) {
            if (stack [x].card.Count < 2) {
                valid = false;
            }
        }

        return valid;
    }

    public int GetStackSize (int stackNumber) {
        return stack [stackNumber].card.Count;
    }

    public CardClass GetCard (int stackNumber, int cardNumber) {
        return stack [stackNumber].card [cardNumber];
    }

    public void RemoveCard (int stackNumber, int cardNumber) {
        stack [stackNumber].RemoveCard (cardNumber);
    }

    public void SetCard (int stackNumber, int cardNumber, CardClass card) {
        stack [stackNumber].SetCard (cardNumber, card);
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
        output += 1 / scale - 1f;
        output *= scale;
        output -= 1 / scale - 1f;
        output = Mathf.Max (output, 0.25f);
        return output;
    }
    public void GenerateRandomHand () {
        CardPoolClass CardPool = new CardPoolClass ();
        CardPool.LoadFromFile (1);
        GenerateRandomHand (CardPool);
    }

    public void GenerateRandomHand (CardPoolClass CardPool) {
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
                    modifier [z] *= Normalize (RatingClass.abilityOnRow [card.abilityType, card.AreaSize (), y], 25);
                    if (y > 0) {
                        CardClass prevCard = stack [x].card [y - 1];
                        modifier [z] *= Normalize (RatingClass.abilityAfterAbility [
                            card.abilityType, card.AreaSize(),
                            prevCard.abilityType, prevCard.AreaSize()], 10);
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
                        Mathf.Max (abilityArea, abilityArea2)], 8);
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

    public string [] HandToString () {
        List<string> s = new List<string> ();
        for (int x = 0; x < numberOfStacks; x++) {
            string s2 = "";
            for (int y = 0; y < stack [x].card.Count; y++) {
                CardClass card = GetCard (x, y);
                if (card != null) {
                    s2 += card.cardNumber + " " + card.abilityArea + " ";
                }
            }
            s.Add (s2);
        }
        return s.ToArray();
    }

    public void LoadFromFile (string accountName, int gameMode, int setId) {
        LoadFromString (ServerData.GetPlayerModeSet (accountName, gameMode, setId));
    }

    public void LoadFromString (string [] lines) {
        CardPoolClass cardPool = new CardPoolClass ();
        cardPool.LoadFromFile (1);
        stack = new StackClass [4];
        for (int x = 0; x < 4; x++) {
            stack [x] = new StackClass ();
            if (lines.Length <= x) {
                continue;
            }
            string [] word = lines [x].Split (new char [] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            for (int y = 0; y < word.Length / 2; y++) {
                stack [x].card.Add (cardPool.Card [int.Parse (word [y * 2])]);
                stack [x].card [y].abilityArea = int.Parse (word [y * 2 + 1]);
            }
        }
    }

}
