using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandClass  {

    int numberOfStacks;
    public StackClass [] stack;

    public bool atLeast1Enabled = true;

    public HandClass () {
        Init (4);
    }

    public HandClass (int numberOfStacks) {
        Init (numberOfStacks);
    }

    public HandClass (HandClass hand) {
        if (hand == null) {
            return;
        }
        this.numberOfStacks = hand.numberOfStacks;
        this.stack = new StackClass [numberOfStacks];
        for (int x = 0; x < numberOfStacks; x++) {
            stack [x] = new StackClass (hand.stack [x]);
        }
        this.atLeast1Enabled = hand.atLeast1Enabled;
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
        if (stackNumber < 0 || stackNumber >= stack.Length) {
            return null;
        }
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

    public void DisableCard (int stackNumber, int cardNumber) {
        stack [stackNumber].DisableCard (cardNumber);
        CheckIfAtLeast1Enabled ();
    }

    public void CheckIfAtLeast1Enabled () {
        atLeast1Enabled = false;
        int count = stack.Length;
        for (int x = 0; x < count; x++) {
            if (stack [x].atLeast1Enabled) {
                atLeast1Enabled = true;
            }
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
        output *= 0.99f;
        output = Mathf.Clamp (output, 0.4f, 5.2f);
        //output = Mathf.Clamp (output, 0.8f, 1.2f);
        //output = 1;
        return output;
    }

    public void GenerateTutorialHand (int id, bool player) {
        if (player) {
            switch (id) {
                case 1:
                    Init (1);
                    stack [0].Add (new CardClass (4, 0, 0, 0));
                    break;
                case 2:
                    Init (1);
                    stack [0].Add (new CardClass (4, 0, 0, 0));
                    stack [0].Add (new CardClass (3, 0, 1, 1));
                    break;
                case 3:
                    Init (2);
                    stack [0].Add (new CardClass (4, 0, 0, 0));
                    stack [0].Add (new CardClass (3, 0, 1, 1));
                    stack [1].Add (new CardClass (2, 0, 1, 5));
                    stack [1].Add (new CardClass (2, 0, 1, 2));
                    break;
                case 4:
                    Init (4);
                    stack [0].Add (new CardClass (4, 0, 0, 0));
                    stack [0].Add (new CardClass (3, 0, 1, 1));
                    stack [1].Add (new CardClass (2, 0, 1, 5));
                    stack [1].Add (new CardClass (2, 0, 1, 2));
                    stack [2].Add (new CardClass (2, 1, 0, 0));
                    stack [2].Add (new CardClass (2, 0, 4, 9));
                    stack [3].Add (new CardClass (1, 1, 4, 5));
                    stack [3].Add (new CardClass (2, 0, 4, 1));
                    break;
            }
        } else {
            switch (id) {
                case 2:
                    Init (1);
                    stack [0].Add (new CardClass (2, 0, 4, 1));
                    stack [0].Add (new CardClass (3, 0, 0, 0));
                    break;
                case 3:
                    Init (1);
                    stack [0].Add (new CardClass (2, 0, 4, 1));
                    stack [0].Add (new CardClass (3, 0, 0, 0));
                    stack [0].Add (new CardClass (2, 0, 1, 2));
                    stack [0].Add (new CardClass (4, 0, 0, 0));
                    break;
                case 4:
                    Init (1);
                    stack [0].Add (new CardClass (2, 0, 4, 1));
                    stack [0].Add (new CardClass (2, 1, 0, 0));
                    stack [0].Add (new CardClass (1, 1, 1, 2));
                    stack [0].Add (new CardClass (2, 1, 0, 0));
                    break;
            }
        }
    }


    public void GenerateBossHand (int id, int playerNumber = 2) {
        switch (playerNumber) {
            case 2:
                switch (id) {
                    case 1:
                        Init (4);
                        stack [0].Add (new CardClass (1, 0, 1, 25));
                        stack [1].Add (new CardClass (1, 0, 2, 25));
                        stack [2].Add (new CardClass (1, 0, 3, 25));
                        stack [3].Add (new CardClass (3, 0, 4, 15));
                        break;
                    case 2:
                        Init (4);
                        stack [0].Add (new CardClass (2, 0, Random.Range (1, 4), 1));
                        stack [0].Add (new CardClass (2, 0, Random.Range (1, 4), 1));
                        stack [0].Add (new CardClass (2, 0, Random.Range (1, 4), 1));
                        stack [0].Add (new CardClass (2, 0, Random.Range (1, 4), 1));
                        stack [1].Add (new CardClass (1, 6, 4, 1));
                        stack [2].Add (new CardClass (2, 0, 4, 7));
                        stack [3].Add (new CardClass (1, 15, 4, 7));
                        break;
                    case 3:
                        Init (4);
                        stack [0].Add (new CardClass (3, 10, 4, 31));
                        stack [1].Add (new CardClass (1, 2, 1, 25));
                        stack [2].Add (new CardClass (1, 2, 2, 25));
                        stack [3].Add (new CardClass (1, 2, 3, 25));
                        break;
                    case 4:
                        Init (3);
                        stack [0].Add (new CardClass (1, 0, 4, 5));
                        stack [0].Add (new CardClass (1, 0, 4, 5));
                        stack [0].Add (new CardClass (1, 0, 4, 5));
                        stack [0].Add (new CardClass (1, 0, 4, 5));
                        stack [0].Add (new CardClass (3, 0, 4, 5));
                        stack [1].Add (new CardClass (1, 6, 4, 12));
                        stack [1].Add (new CardClass (1, 6, 4, 12));
                        stack [1].Add (new CardClass (1, 6, 4, 12));
                        stack [1].Add (new CardClass (1, 6, 4, 12));
                        stack [1].Add (new CardClass (3, 0, 4, 12));
                        stack [2].Add (new CardClass (2, 0, 4, 34));
                        stack [2].Add (new CardClass (2, 0, 4, 34));
                        stack [2].Add (new CardClass (2, 0, 4, 34));
                        stack [2].Add (new CardClass (2, 0, 4, 34));
                        stack [2].Add (new CardClass (4, 0, 4, 34));
                        break;
                    case 5:
                        Init (1);
                        stack [0].Add (new CardClass (1, 0, 0, 49));
                        break;
                    case 6:
                        Init (4);
                        stack [0].Add (new CardClass (5, 0, 4, 29));
                        stack [1].Add (new CardClass (7, 0, 4, 31));
                        stack [2].Add (new CardClass (5, 0, 4, 35));
                        stack [3].Add (new CardClass (4, 0, 4, 59));
                        break;
                    case 7:
                        Init (3);
                        stack [0].Add (new CardClass (2, 0, 1, 1));
                        stack [0].Add (new CardClass (2, 0, 2, 1));
                        stack [0].Add (new CardClass (2, 0, 3, 1));
                        stack [0].Add (new CardClass (2, 0, 4, 1));
                        stack [1].Add (new CardClass (4, 5, 0, 0));
                        stack [1].Add (new CardClass (3, 5, 0, 0));
                        stack [1].Add (new CardClass (2, 5, 0, 0));
                        stack [1].Add (new CardClass (1, 5, 0, 0));
                        stack [2].Add (new CardClass (2, 0, 4, 24));
                        stack [2].Add (new CardClass (2, 0, 4, 24));
                        stack [2].Add (new CardClass (3, 0, 4, 24));
                        break;
                    case 8:
                        Init (4);
                        stack [0].Add (new CardClass (3, 2, 4, 2));
                        stack [1].Add (new CardClass (1, 2, 1, 25));
                        stack [1].Add (new CardClass (1, 2, 2, 25));
                        stack [1].Add (new CardClass (1, 2, 3, 25));
                        stack [2].Add (new CardClass (1, 0, 4, 60));
                        stack [3].Add (new CardClass (2, 2, 4, 31));
                        break;
                    case 9:
                        Init (4);
                        stack [0].Add (new CardClass (1, 6, 4, 24));
                        stack [1].Add (new CardClass (1, 2, 4, 35));
                        stack [2].Add (new CardClass (1, 5, 0, 0));
                        stack [3].Add (new CardClass (1, 1, 0, 42));
                        break;
                    case 10:
                        Init (4);
                        stack [0].Add (new CardClass (1, 20, 4, 31));
                        stack [1].Add (new CardClass (1, 6, 1, 69));
                        stack [1].Add (new CardClass (1, 6, 2, 69));
                        stack [1].Add (new CardClass (1, 6, 3, 69));
                        stack [2].Add (new CardClass (1, 2, 4, 46));
                        stack [3].Add (new CardClass (1, 20, 0, 61));
                        stack [3].Add (new CardClass (1, 20, 0, 61));
                        stack [3].Add (new CardClass (1, 20, 0, 61));
                        stack [3].Add (new CardClass (1, 20, 0, 61));
                        stack [3].Add (new CardClass (1, 20, 0, 61));
                        break;
                    case 11:
                        Init (4);
                        stack [0].Add (new CardClass (3, 6, 1, 59));
                        stack [0].Add (new CardClass (5, 6, 1, 59));
                        stack [0].Add (new CardClass (7, 6, 1, 59));
                        stack [0].Add (new CardClass (8, 6, 1, 59));
                        stack [1].Add (new CardClass (3, 6, 2, 59));
                        stack [1].Add (new CardClass (5, 6, 2, 59));
                        stack [1].Add (new CardClass (7, 6, 2, 59));
                        stack [1].Add (new CardClass (8, 6, 2, 59));
                        stack [2].Add (new CardClass (3, 6, 3, 59));
                        stack [2].Add (new CardClass (5, 6, 3, 59));
                        stack [2].Add (new CardClass (7, 6, 3, 59));
                        stack [2].Add (new CardClass (8, 6, 3, 59));
                        stack [3].Add (new CardClass (2, 6, 4, 8));
                        break;
                    case 12:
                        Init (2);
                        stack [0].Add (new CardClass (5, 6, 1, 25));
                        stack [0].Add (new CardClass (1, 6, 4, 2));
                        stack [0].Add (new CardClass (1, 6, 4, 2));
                        stack [0].Add (new CardClass (1, 6, 4, 2));
                        stack [0].Add (new CardClass (1, 6, 0, 0));
                        stack [1].Add (new CardClass (2, 6, 4, 25));
                        stack [1].Add (new CardClass (1, 1, 4, 2));
                        stack [1].Add (new CardClass (1, 1, 4, 2));
                        stack [1].Add (new CardClass (1, 1, 4, 2));
                        stack [1].Add (new CardClass (1, 1, 4, 2));
                        break;
                }
                break;
            case 3:
                switch (id) {
                    case 6:
                        Init (4);
                        stack [0].Add (new CardClass (1, 0, 1, 1));
                        stack [0].Add (new CardClass (3, 13, 1, 1));
                        stack [1].Add (new CardClass (1, 20, 1, 25));
                        stack [1].Add (new CardClass (2, 13, 1, 25));
                        stack [2].Add (new CardClass (1, 0, 1, 70));
                        stack [2].Add (new CardClass (3, 13, 1, 70));
                        stack [3].Add (new CardClass (2, 0, 0, 0));
                        stack [3].Add (new CardClass (4, 13, 0, 0));
                        stack [0].Add (new CardClass (1, 0, 1, 1));
                        stack [0].Add (new CardClass (3, 13, 4, 1));
                        stack [1].Add (new CardClass (1, 20, 1, 25));
                        stack [1].Add (new CardClass (1, 13, 4, 25));
                        stack [2].Add (new CardClass (1, 0, 1, 70));
                        stack [2].Add (new CardClass (4, 13, 1, 70));
                        break;
                    case 7:
                        Init (4);
                        stack [0].Add (new CardClass (3, 0, 1, 10));
                        stack [0].Add (new CardClass (3, 0, 2, 10));
                        stack [0].Add (new CardClass (3, 0, 3, 10));
                        stack [0].Add (new CardClass (3, 0, 4, 10));
                        stack [1].Add (new CardClass (3, 0, 1, 46));
                        stack [1].Add (new CardClass (3, 0, 2, 46));
                        stack [1].Add (new CardClass (3, 0, 3, 46));
                        stack [2].Add (new CardClass (3, 0, 1, 47));
                        stack [2].Add (new CardClass (3, 0, 2, 47));
                        stack [2].Add (new CardClass (3, 0, 3, 47));
                        stack [3].Add (new CardClass (3, 0, 1, 70));
                        stack [3].Add (new CardClass (3, 0, 2, 70));
                        stack [3].Add (new CardClass (3, 0, 3, 70));
                        break;
                }
                break;
            case 4:
                switch (id) {

                    case 7:
                        Init (2);
                        stack [0].Add (new CardClass (1, 0, 1, 2));
                        stack [0].Add (new CardClass (1, 0, 2, 2));
                        stack [0].Add (new CardClass (1, 0, 3, 2));
                        stack [0].Add (new CardClass (1, 6, 4, 2));
                        stack [1].Add (new CardClass (1, 6, 1, 52));
                        stack [1].Add (new CardClass (1, 6, 2, 52));
                        stack [1].Add (new CardClass (1, 6, 3, 52));
                        stack [1].Add (new CardClass (1, 20, 4, 52));
                        break;
                }
                break;
        }
    }

    public void GenerateRandomHand (int gameModeId, AIClass AI) {
        CardPoolClass CardPool = new CardPoolClass ();
        CardPool.LoadFromFile (gameModeId);
        int minimumNumberOfCardsOnStack = ServerData.GetGameModeMinimumNumberOfCardsInStack (gameModeId);
        numberOfStacks = ServerData.GetGameModeNumberOfStacks (gameModeId);
        bool usedCardsArePutOnBottomOfStack = ServerData.GetGameModeUsedCardsArePutOnBottomOfStack (gameModeId);
        GenerateRandomHand (CardPool, /*RatingClass.PopularityRating (gameModeId)*/ null, AI, usedCardsArePutOnBottomOfStack, numberOfStacks, minimumNumberOfCardsOnStack);
    }

    public void GenerateRandomHand (CardPoolClass CardPool, float [] CardValue, AIClass AI, bool usedCardsArePutOnBottomOfStack, int numberOfStacks, int minimumNumberOfCardsOnStack) {
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

        int usedCount = 0;
        for (int x = 0; x < numberOfStacks; x++) {
            stackSize [x] = minimumNumberOfCardsOnStack;
        }
        usedCount = numberOfStacks * minimumNumberOfCardsOnStack;

        for (int y = 0; y < 5; y++) {
            for (int x = 0; x < numberOfStacks; x++) {
                if (stackSize [x] == y && (Random.Range (0, 2) == 0 || !usedCardsArePutOnBottomOfStack)) {
                    stackSize [x]++;
                    usedCount++;
                }
                if (usedCount == count) {
                    break;
                }
            }
            if (usedCount == count) {
                break;
            }
        }

        for (int x = 0; x < numberOfStacks; x++) {
            if (SumOfValues == 0) {
                break;
            }
            for (int y = 0; y < 5; y++) {
                if (stackSize [x] <= y) {
                    continue;
                }
                SumOfValues = 0;
                for (int z = 0; z < CardValue.Length; z++) {
                    CardClass card = CardPool.Card [z];
                    modifier [z] = CardValue [z];
                    modifier [z] *= Normalize (RatingClass.abilityOnRow [(int) card.abilityType, card.AreaSize (), y], AI.abilityRow)
                        * Normalize (RatingClass.tokenOnRow [(int) card.tokenType, card.tokenValue, y], AI.tokenRow)
                        * Normalize (RatingClass.abilityTokenOnRow [(int) card.abilityType, (int) card.tokenType, y], AI.abilityTokenRow)

                        * Normalize (RatingClass.abilityStackSize [(int) card.abilityType, card.AreaSize (), stackSize [x]], AI.tokenRow)
                        * Normalize (RatingClass.tokenStackSize [(int) card.tokenType, card.tokenValue, stackSize [x]], AI.tokenRow)
                        * Normalize (RatingClass.abilityTokenStackSize [(int) card.tokenType, (int) card.tokenType, stackSize [x]], AI.tokenRow);
                    if (y > 0) {
                        CardClass prevCard = stack [x].card [y - 1];
                        modifier [z] *= Normalize (RatingClass.abilityAfterAbility [
                            (int) card.abilityType, card.AreaSize(),
                            (int) prevCard.abilityType, prevCard.AreaSize()], AI.abilityAfterAbility);
                        modifier [z] *= Normalize (RatingClass.abilityAfterToken [
                            (int) card.abilityType, card.AreaSize (),
                            (int) prevCard.tokenType, prevCard.tokenValue], AI.abilityAfterToken);
                        modifier [z] *= Normalize (RatingClass.tokenAfterAbility [
                            (int) card.tokenType, card.tokenValue,
                            (int) prevCard.abilityType, prevCard.AreaSize ()], AI.tokenAfterAbility);
                        modifier [z] *= Normalize (RatingClass.tokenAfterToken [
                            (int) card.tokenType, card.tokenValue,
                            (int) prevCard.tokenType, prevCard.tokenValue], AI.tokenAfterToken);
                    }
                    AIClass.maxCardValue = Mathf.Max (AIClass.maxCardValue, modifier [z]);
                    modifier [z] = Mathf.Min (modifier [z], 100000);
                    modifier [z] = Mathf.Sqrt (modifier [z]);
                    SumOfValues += modifier [z];
                    //SumOfValues += Mathf.Sqrt (modifier [z]);
                }
                if (SumOfValues == 0) {
                    break;
                }
                float rng = Random.Range (0f, SumOfValues);
                int id = -1;
                for (int z = 0; z < count; z++) {
                    rng -= modifier [z];
                    //rng -= Mathf.Sqrt (modifier [z]);
                    if (rng <= 0) {
                        id = z;
                        break;
                    }
                }
                CardValue [id] = 0;
                int abilityType = (int) CardPool.Card [id].abilityType;
                int abilityArea = CardPool.Card [id].AreaSize ();
                int tokenType = (int) CardPool.Card [id].tokenType;
                int tokenValue = CardPool.Card [id].tokenValue;
                for (int z = 0; z < count; z++) {
                    int abilityType2 = (int) CardPool.Card [z].abilityType;
                    int abilityArea2 = CardPool.Card [z].AreaSize ();
                    int tokenType2 = (int) CardPool.Card [z].tokenType;
                    int tokenValue2 = CardPool.Card [z].tokenValue;
                    if (abilityType < abilityType2) {
                        CardValue [z] *= Normalize (RatingClass.ability_AbilitySynergy [abilityType, abilityArea, abilityType2, abilityArea2], AI.ability_AbilitySynergy);
                    } else {
                        CardValue [z] *= Normalize (RatingClass.ability_AbilitySynergy [abilityType2, abilityArea2, abilityType, abilityArea], AI.ability_AbilitySynergy);
                    }
                    if (tokenType < tokenType2) {
                        CardValue [z] *= Normalize (RatingClass.token_TokenSynergy [tokenType, tokenValue, tokenType2, tokenValue2], AI.token_TokenSynergy);
                    } else {
                        CardValue [z] *= Normalize (RatingClass.token_TokenSynergy [tokenType2, tokenValue2, tokenType, tokenValue], AI.token_TokenSynergy);
                    }
                    CardValue [z] *= Normalize (RatingClass.ability_TokenSynergy [abilityType, abilityArea, tokenType2, tokenValue2], AI.ability_TokenSynergy);
                    CardValue [z] *= Normalize (RatingClass.ability_TokenSynergy [abilityType2, abilityArea2, tokenType, tokenValue], AI.ability_TokenSynergy);

                    if (CardValue [z] != 0) {
                        CardValue [z] = Mathf.Sqrt (CardValue [z]);
                        CardValue [z] = Mathf.Clamp (CardValue [z], 0.012f, 100000f);
                    }

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

    public void GenerateStartingSet (int gameMode) {
        CardPoolClass cardPool = new CardPoolClass ();
        cardPool.LoadFromFile (gameMode);
        stack [0].Add (cardPool.Card [0]);
        stack [0].Add (cardPool.Card [1]);
        stack [1].Add (cardPool.Card [32]);
        stack [1].Add (cardPool.Card [2]);
        stack [2].Add (cardPool.Card [15]);
        stack [2].Add (cardPool.Card [12]);
        stack [3].Add (cardPool.Card [6]);
        stack [3].Add (cardPool.Card [5]);
    }

    public string [] ModeHandToString () {
        List<string> s = new List<string> ();
        for (int x = 0; x < numberOfStacks; x++) {
            s.Add (stack [x].topCardNumber.ToString());
            string s2 = "";
            for (int y = 0; y < stack [x].card.Count; y++) {
                CardClass card = GetCard (x, y);
                if (card != null) {
                    s2 += (int) card.abilityType + " " + card.abilityArea + " " + (int) card.tokenType + " " + card.tokenValue.ToString() + " ";
                }
            }
            s.Add (s2);
        }
        return s.ToArray ();
    }

    public void LoadFromFile (string accountName, int gameModeId, int setId) {
        LoadFromFileString (gameModeId, ServerData.GetPlayerModeSet (accountName, gameModeId, setId));
        bool [] unlockedAbilities = ServerData.GetUserUnlockedAbilities (accountName);
        bool [] unlockedTokens = ServerData.GetUserUnlockedTokens (accountName);
       // DebugString ();
        for (int x = 0; x < numberOfStacks; x++) {
            StackClass tStack = stack [x];
            StackClass newStack = new StackClass ();
            for (int y = 0; y < tStack.card.Count; y++) {
                CardClass card = tStack.card [y];
                //Debug.Log (card.abilityArea);
                if (!unlockedAbilities [(int) card.abilityType] || !unlockedTokens [(int) card.tokenType]) {
                } else {
                    newStack.Add (card);
                }
            }
            stack [x] = newStack;
        }
        //DebugString ();
    }

    public void LoadFromFileString (int gameModeId, string [] lines) {
        CardPoolClass cardPool = new CardPoolClass ();
        cardPool.LoadFromFile (gameModeId);
        int numberOfStacks = ServerData.GetGameModeNumberOfStacks (gameModeId);
        LoadFromFileString (cardPool, lines, numberOfStacks);
        //DebugString ();
    }

    public void LoadFromFileString (CardPoolClass cardPool, string [] lines, int numberOfStacks) {
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
                stack [x].Add (cardPool.Card [cardNumber]);
                int abilityArea = int.Parse (word [y * 2 + 1]);
                if (stack [x].card [row].abilityArea == 0) {

                } else if (stack [x].card [row].abilityArea <= 3 && abilityArea <=3) {
                    stack [x].card [row].abilityArea = Mathf.Max (abilityArea, 1);
                }
                row ++;
            }
        }
        //DebugString ();
    }

    public void LoadFromModeString (string [] lines) {
        Init (numberOfStacks);
        for (int x = 0; x < numberOfStacks; x++) {
            if (lines.Length <= x) {
                continue;
            }
            stack [x].topCardNumber = int.Parse (lines [x * 2]);
            string [] word = lines [x * 2 + 1].Split (new char [] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            for (int y = 0; y < word.Length / 4; y++) {
                CardClass card = new CardClass (int.Parse (word [y * 4 + 3]), int.Parse (word [y * 4 + 2]), int.Parse (word [y * 4 + 1]), int.Parse (word [y * 4]));
                stack [x].Add (card);
            }
        }
    }

    public void DebugString () {
        string s = "";
        for (int x = 0; x < numberOfStacks; x++) {
            StackClass stack = this.stack [x];
            for (int y = 0; y < stack.card.Count; y++) {
                s += stack.card [y].abilityArea + " ";
            }
            s += System.Environment.NewLine;
        }
        Debug.Log (s);
    }

}
