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
        if (!IsValid (gameMode)) {
            int minimumNumberOfCardsInStack = ServerData.GetGameModeMinimumNumberOfCardsInStack (gameMode);
            client.TargetInvalidSet (client.connectionToClient, minimumNumberOfCardsInStack);
            return;
        }
    }

    public int GetNumberOfStacks () {
        return stack.Length;
    }

    public StackClass GetStack (int stackNumber) {
        return stack [stackNumber];
    }

    public bool IsValid (int gameMode) {
        int numberOfStacks = ServerData.GetGameModeNumberOfStacks (gameMode);
        if (stack.Length < numberOfStacks) {
            return false;
        }
        int minimumNumberOfCardsInStack = ServerData.GetGameModeMinimumNumberOfCardsInStack (gameMode);
        for (int x = 0; x < numberOfStacks; x++) {
            if (stack [x].card.Count < minimumNumberOfCardsInStack) {
                return false;
            }
        }
        return true;
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
        output = Mathf.Clamp (output, 0.8f, 2.1f);
        return output;
    }

    public void GenerateRandomHand (int gameModeId, AIClass AI) {
        CardPoolClass CardPool = new CardPoolClass ();
        CardPool.LoadFromFile (gameModeId);
        int minimumNumberOfCardsOnStack = ServerData.GetGameModeMinimumNumberOfCardsInStack (gameModeId);
        GenerateRandomHand (CardPool, /*RatingClass.PopularityRating (gameModeId)*/ null, AI, minimumNumberOfCardsOnStack);
    }

    public void GenerateRandomHand (CardPoolClass CardPool, float [] CardValue, AIClass AI, int minimumNumberOfCardsOnStack) {
        int count = CardPool.Card.Count;

        if (AI == null) {
            AI = new AIClass ();
        }

        //if (CardValue == null) {
        if (true) {
            CardValue = new float [count];
            for (int x = 0; x < count; x++) {
                CardValue [x] = 1f;
            }
        }
        float [] modifier = new float [count];
        
        int [] stackSize = new int [numberOfStacks];
        float SumOfValues = -1;

        for (int x = 0; x < numberOfStacks; x++) {
            stackSize [x] = minimumNumberOfCardsOnStack;
            while (stackSize [x] < 5 && Random.Range (0, 2) == 0) {
                stackSize [x]++;
            }
        }

        for (int y = 0; y < 5; y++) {
            if (SumOfValues == 0) {
                break;
            }
            for (int x = 0; x < numberOfStacks; x++) {
                if (stackSize [x] <= y) {
                    continue;
                }
                SumOfValues = 0;
                for (int z = 0; z < CardValue.Length; z++) {
                    CardClass card = CardPool.Card [z];
                    modifier [z] = CardValue [z];
                    modifier [z] *= Normalize (RatingClass.abilityOnRow [card.abilityType, card.AreaSize (), y], AI.abilityRow)
                        * Normalize (RatingClass.tokenOnRow [card.tokenType, card.value, y], AI.tokenRow)
                        * Normalize (RatingClass.abilityTokenOnRow [card.abilityType, card.tokenType, y], AI.abilityTokenRow)
                        * Normalize (RatingClass.abilityStackSize [card.abilityType, card.AreaSize (), stackSize [x]], AI.tokenRow)
                        * Normalize (RatingClass.tokenStackSize [card.tokenType, card.value, stackSize [x]], AI.tokenRow);
                    if (y > 0) {
                        CardClass prevCard = stack [x].card [y - 1];
                        modifier [z] *= Normalize (RatingClass.abilityAfterAbility [
                            card.abilityType, card.AreaSize(),
                            prevCard.abilityType, prevCard.AreaSize()], AI.abilityAfterAbility);
                        modifier [z] *= Normalize (RatingClass.abilityAfterToken [
                            card.abilityType, card.AreaSize (),
                            prevCard.tokenType, prevCard.value], AI.abilityAfterToken);
                        modifier [z] *= Normalize (RatingClass.tokenAfterAbility [
                            card.tokenType, card.value,
                            prevCard.abilityType, prevCard.AreaSize ()], AI.tokenAfterAbility);
                        modifier [z] *= Normalize (RatingClass.tokenAfterToken [
                            card.tokenType, card.value,
                            prevCard.tokenType, prevCard.value], AI.tokenAfterToken);
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
                int tokenType = CardPool.Card [id].tokenType;
                int tokenValue = CardPool.Card [id].value;
                for (int z = 0; z < count; z++) {
                    int abilityType2 = CardPool.Card [z].abilityType;
                    int abilityArea2 = CardPool.Card [z].AreaSize ();
                    int tokenType2 = CardPool.Card [z].tokenType;
                    int tokenValue2 = CardPool.Card [z].value;
                    if (abilityType < abilityType2) {
                        CardValue [z] *= Normalize (RatingClass.abilityAbilitySynergy [abilityType, abilityArea, abilityType2, abilityArea2], AI.abilityAbilitySynergy);
                    } else {
                        CardValue [z] *= Normalize (RatingClass.abilityAbilitySynergy [abilityType2, abilityArea2, abilityType, abilityArea], AI.abilityAbilitySynergy);
                    }
                    CardValue [z] *= Normalize (RatingClass.abilityTokenSynergy [abilityType, abilityArea, tokenType2, tokenValue2], AI.abilityTokenSynergy);
                    CardValue [z] *= Normalize (RatingClass.abilityTokenSynergy [abilityType2, abilityArea2, tokenType, tokenValue], AI.abilityTokenSynergy);

                }

                stack [x].Add (new CardClass (CardPool.Card [id]));
                CardClass newCard = stack [x].card [y];
                if (newCard.abilityArea == 1) {
                    newCard.abilityArea = Random.Range (1, 4);
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

    public void LoadFromFile (string accountName, int gameModeId, int setId) {
        LoadFromString (gameModeId, ServerData.GetPlayerModeSet (accountName, gameModeId, setId));
    }

    public void LoadFromString (int gameModeId, string [] lines) {
        CardPoolClass cardPool = new CardPoolClass ();
        cardPool.LoadFromFile (gameModeId);
        int numberOfStacks = ServerData.GetGameModeNumberOfStacks (gameModeId);
        LoadFromString (cardPool, lines, numberOfStacks);
    }

    public void LoadFromString (CardPoolClass cardPool, string [] lines, int numberOfStacks) {
        Init (numberOfStacks);
        for (int x = 0; x < numberOfStacks; x++) {
            if (lines.Length <= x) {
                continue;
            }
            string [] word = lines [x].Split (new char [] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            int row = 0;
            for (int y = 0; y < word.Length / 2; y++) {
                int cardNumber = int.Parse (word [y * 2]);
                if (cardPool.Card.Count <= cardNumber) {
                    continue;
                }
                stack [x].card.Add (cardPool.Card [cardNumber]);
                int abilityArea = int.Parse (word [y * 2 + 1]);
                if (stack [x].card [row].abilityArea < 3 && abilityArea < 3) {
                    stack [x].card [row].abilityArea = abilityArea;
                }
                row ++;
            }
        }
    }

    public void LoadFromModeString (string [] lines) {
        Init (numberOfStacks);
        for (int x = 0; x < numberOfStacks; x++) {
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
