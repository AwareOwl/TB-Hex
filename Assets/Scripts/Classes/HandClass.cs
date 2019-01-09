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

    public HandClass (ClientInterface client) {
        string accountName = client.AccountName;
        int gameMode = client.GameMode;
        int selectedSet = ServerData.GetPlayerModeSelectedSet (accountName, gameMode);
        if (!ServerData.GetPlayerModeSelectedSetExists (accountName, gameMode)) {
            client.TargetShowMessage (client.connectionToClient, Language.NoSetSelectedKey);
            return;
        }
        LoadFromFile (client.AccountName, client.GameMode, selectedSet);
        if (!IsValid ()) {
            client.TargetInvalidSet (client.connectionToClient);
            return;
        }
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
        StackClass stack2 = stack [stackNumber];
        if (stack2.card.Count > cardNumber) {
            return stack2.card [cardNumber];
        } else {
            return null;
        }
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
        }
    }

    public float Normalize (float value, float scale) {
        float output = value * 2;
        output += 1 / scale - 1f;
        output *= scale;
        output -= 1 / scale - 1f;
        output = Mathf.Max (output, 0.5f);
        return output;
    }
    public void GenerateRandomHand (int gameMode) {
        CardPoolClass CardPool = new CardPoolClass ();
        CardPool.LoadFromFile (gameMode);
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
                    modifier [z] *= Normalize (RatingClass.abilityOnRow [card.abilityType, card.AreaSize (), y], 20)
                        * Normalize (RatingClass.tokenOnRow [card.tokenType, card.value, y], 20);
                    if (y > 0) {
                        CardClass prevCard = stack [x].card [y - 1];
                        modifier [z] *= Normalize (RatingClass.abilityAfterAbility [
                            card.abilityType, card.AreaSize(),
                            prevCard.abilityType, prevCard.AreaSize()], 8);
                        modifier [z] *= Normalize (RatingClass.abilityAfterToken [
                            card.abilityType, card.AreaSize (),
                            prevCard.tokenType, prevCard.value], 8);
                        modifier [z] *= Normalize (RatingClass.tokenAfterAbility [
                            card.tokenType, card.value,
                            prevCard.abilityType, prevCard.AreaSize ()], 8);
                        modifier [z] *= Normalize (RatingClass.tokenAfterToken [
                            card.tokenType, card.value,
                            prevCard.tokenType, prevCard.value], 8);
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

    public string [] ModeHandToString () {
        List<string> s = new List<string> ();
        for (int x = 0; x < numberOfStacks; x++) {
            string s2 = "";
            for (int y = 0; y < stack [x].card.Count; y++) {
                CardClass card = GetCard (x, y);
                if (card != null) {
                    s2 += card.abilityType + " " + card.abilityArea + " " + card.tokenType + " " + card.value.ToString() + " ";
                }
            }
            s.Add (s2);
        }
        return s.ToArray ();
    }

    public void LoadFromFile (string accountName, int gameMode, int setId) {
        LoadFromString (gameMode, ServerData.GetPlayerModeSet (accountName, gameMode, setId));
    }

    public void LoadFromString (int gameMode, string [] lines) {
        CardPoolClass cardPool = new CardPoolClass ();
        cardPool.LoadFromFile (gameMode);
        LoadFromString (cardPool, lines);
    }
    public void LoadFromString (CardPoolClass cardPool, string [] lines) {
        Init (4);
        for (int x = 0; x < 4; x++) {
            if (lines.Length <= x) {
                continue;
            }
            string [] word = lines [x].Split (new char [] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            for (int y = 0; y < word.Length / 2; y++) {
                int cardNumber = int.Parse (word [y * 2]);
                if (cardPool.Card.Count <= cardNumber) {
                    continue;
                }
                stack [x].card.Add (cardPool.Card [cardNumber]);
                int abilityArea = int.Parse (word [y * 2 + 1]);
                if (stack [x].card [y].abilityArea < 3 && abilityArea < 3) {
                    stack [x].card [y].abilityArea = abilityArea;
                }
            }
        }
    }

    public void LoadFromModeString (string [] lines) {
        Init (4);
        for (int x = 0; x < 4; x++) {
            if (lines.Length <= x) {
                continue;
            }
            string [] word = lines [x].Split (new char [] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            for (int y = 0; y < word.Length / 4; y++) {
                CardClass card = new CardClass (int.Parse (word [y * 4 + 3]), int.Parse (word [y * 4 + 2]), int.Parse (word [y * 4 + 1]), int.Parse (word [y * 4]));
                stack [x].card.Add (card);
            }
        }
    }

}
