using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchClass {

    public BoardClass Board;

    public int turn = 1;
    public int turnOfPlayer = 1;

    public int numberOfPlayers;
    public PlayerClass [] Player;

    public PlayerClass winner;
    public bool finished;
    public int winCondition;
    public bool real = true;
    public bool updateBoard = false;

    public MoveHistoryClass ThisTurnMove;
    public MoveHistoryClass LastMove;
    public int lastMoveId = 0;

    public MatchPropertiesClass Properties;

    public MatchClass prevMatch;

    public VisualMatch visualMatch;

    public string [] MatchToString () {
        List<string> s = new List<string> ();
        s.Add (turn.ToString ());
        s.Add (turnOfPlayer.ToString ());
        s.Add (numberOfPlayers.ToString ());
        s.Add (lastMoveId.ToString ());
        return s.ToArray ();
    }

    public void LoadFromString (string [] lines) {
        turn = int.Parse (lines [0]);
        turnOfPlayer = int.Parse (lines [1]);
        numberOfPlayers = int.Parse (lines [2]);
        lastMoveId = int.Parse (lines [3]);
        Player = new PlayerClass [numberOfPlayers + 1];
    }


    public MatchClass () {

    }

    public MatchClass (MatchClass match) {
        this.Board = new BoardClass (this, match.Board);
        this.turn = match.turn;
        this.turnOfPlayer = match.turnOfPlayer;
        this.numberOfPlayers = match.numberOfPlayers;
        this.Player = new PlayerClass [this.numberOfPlayers + 1];
        for (int x = 0; x <= this.numberOfPlayers; x++) {
            PlayerClass player = match.Player [x];
            if (player != null) {
                this.Player [x] = new PlayerClass (match.Player [x]);
            }
        }
        this.LastMove = match.LastMove;
        this.Properties = match.Properties;
        this.prevMatch = match.prevMatch;
    }

    public void Concede (int playerNumber) {
        if (finished) {
            return;
        }
        PlayerPropertiesClass concededPlayer = Player [playerNumber].properties;
        concededPlayer.conceded = true;
        if (concededPlayer.client != null) {
            ServerLogic.AddExperience (concededPlayer.accountName, turn);
        }
        PlayerPropertiesClass winner = null;
        foreach (PlayerClass player in Player) {
            PlayerPropertiesClass properties = player.properties;
            if (properties != null && !properties.conceded) {
                if (winner != null) {
                    return;
                }
                winner = properties;
                ClientInterface client = properties.client;
                if (client != null) {
                    client.TargetCurrentGameConcede (client.connectionToClient, playerNumber);
                }
            }
        }
        FinishGame (4, 0);
    }

    public void EndTurn () {
        int [] playerIncome = new int [Player.Length];

        foreach (TileClass tile in Board.tileList) {
            TokenClass token = tile.token;
            if (token != null) {
                if (token.owner >= playerIncome.Length) {
                    continue;
                }
                int tokenValue = token.value;
                int value = tokenValue;
                VectorInfo info = GetTokenVectorInfo (tile, token);
                switch (token.type) {
                    case 1:
                    case 12:
                        value *= 2;
                        break;
                    case 2:
                        value *= -1;
                        break;
                    case 6:
                        if (token.owner != turnOfPlayer) {
                            value = 0;
                        }
                        break;
                    case 10:
                        value += info.emptyTileCount;
                        break;
                }
                foreach (AbilityVector vector in info.vectors) {
                    TokenClass targetToken = vector.target.token;
                    if (targetToken != null) {
                        int targetType = targetToken.type;
                        int targetValue = targetToken.value;
                        switch (targetType) {
                            case 8:
                                if (tokenValue < targetValue) {
                                    value = 0;
                                }
                                break;
                        }
                    }
                }
                playerIncome [token.owner] += value;
            }
        }
        for (int x = 0; x < Player.Length; x++) {
            PlayerClass player = Player [x];
            if (player != null) {
                player.SetScoreIncome (playerIncome [x]);
                player.AddScore (playerIncome [x]);
                player.UpdateVisuals (this);
            }
        }
        CheckFinishCondition ();
        if (!finished) {
            AfterTurn ();
        }

        //AIClass AI = Player [1].properties.AI;
        // Debug.Log (AI.TurnToWinPredict (this, 1) + " " + AI.TurnToWinPredict (this, 2) + " " + AI.CalculateMatchValue (this, 1) + " " + AI.CalculateMatchValue (this, 2));
    }

    public void AfterTurn () {
        bool actionMade = false;
        bool nextPlayerTurn = true;
        foreach (TileClass tile in Board.tileList) {
            if (!tile.IsFilledTile ()) {
                continue;
            }
            TokenClass token = tile.token;
            int tokenType = token.type;
            VectorInfo info = GetTokenVectorInfo (tile, token);
            switch (tokenType) {
                case 5:
                    ModifyTempValue (tile, -1);
                    break;
                case 13:
                    nextPlayerTurn = false;
                    ChangeType (tile, 0);
                    break;
            }
            foreach (TileClass target in info.Triggered1) {
                switch (tokenType) {
                    case 3:
                    case 4:
                        if (visualMatch != null) {
                            if (!actionMade) {
                                VisualMatch.GlobalTimer += 0.5f;
                            }
                            visualMatch.CreateRealTokenVectorEffect (tile, target, tokenType);
                        }
                        actionMade = true;
                        break;
                }
                switch (tokenType) {
                    case 3:
                        ModifyTempValue (target, 1);
                        break;
                    case 4:
                        ModifyTempValue (target, -1);
                        break;
                }
            }
        }

        UpdateBoard ();
        UpdateVisuals ();
        NewTurn (nextPlayerTurn);
    }

    public int TurnsLeft () {
        return Properties.turnLimit - turn + 1;
    }

    public void NewTurn (bool nextPlayerTurn) {
        ThisTurnMove = null;
        if (nextPlayerTurn) {
            IncrementTurnOfPlayer ();
        }
        turn++;
        if (visualMatch != null && Properties.turnWinCondition) {
            visualMatch.UpdateTurnsLeft (TurnsLeft ());
        }
    }

    public void IncrementTurnOfPlayer () {
        int newTurnOfPlayer = turnOfPlayer;
        do {
            newTurnOfPlayer = Mathf.Max (1, (newTurnOfPlayer + 1) % (numberOfPlayers + 1));
        } while (!AbleToExecuteTurn (newTurnOfPlayer) && newTurnOfPlayer != turnOfPlayer);
        SetTurnOfPlayer (newTurnOfPlayer);
    }

    public bool AbleToExecuteTurn (int playerNumber) {
        PlayerClass player = Player [playerNumber];
        if (player == null) {
            return false;
        }
        HandClass hand = player.GetHand ();
        return !player.properties.conceded && (hand == null || player.hand.atLeast1Enabled) && player.properties.enabled;
    }

    public void SetTurnOfPlayer (int turnOfPlayer) {
        this.turnOfPlayer = turnOfPlayer;
        if (visualMatch != null) {
            foreach (PlayerClass player in Player) {
                if (player != null) {
                    VisualPlayer vPlayer = player.visualPlayer;
                    if (vPlayer != null) {
                        vPlayer.DelayedSetActivePlayer (player.properties.playerNumber == turnOfPlayer);
                    }
                }
            }
        }
    }

    public void UpdateVisuals () {
        foreach (TileClass tile in Board.tileList) {
            if (!tile.IsFilledTile ()) {
                continue;
            }
            TokenClass token = tile.token;
            int tokenType = token.type;
            VisualToken vToken = token.visualToken;
            if (vToken != null) {
                VectorInfo info = GetTokenVectorInfo (tile, token);
                switch (tokenType) {
                    case 3:
                    case 4:
                        if (info.Triggered1.Count == 1) {
                            visualMatch.RotateTo (token.visualToken, tile, info.Triggered1 [0]);
                        } else {

                            visualMatch.NullRotateTo (token.visualToken);
                        }
                        break;
                }
            }
        }
    }

    public void CheckFinishCondition () {
        if (Properties.scoreWinCondition) {
            for (int x = 1; x <= numberOfPlayers; x++) {
                if (Player [x] != null && Player [x].score >= Properties.scoreLimit) {
                    FinishGame (1, Properties.scoreLimit);
                    return;
                }
            }
        }
        if (Properties.turnWinCondition) {
            if (turn >= Properties.turnLimit) {
                FinishGame (2, Properties.turnLimit);
                return;
            }
        }
        if (Board.GetEmptyTiles ().Count == 0) {
            FinishGame (3, 0);
            return;
        }
        bool atLeast1PlayerAbleToExecuteTurn = false;
        for (int x = 1; x <= numberOfPlayers; x++) {
            if (AbleToExecuteTurn (x)) {
                atLeast1PlayerAbleToExecuteTurn = true;
            }
        }
        if (!atLeast1PlayerAbleToExecuteTurn) {
            FinishGame (3, 0);
            return;
        }
    }

    public void FinishGame (int winCondition, int limit) {
        this.winCondition = winCondition;
        finished = true;
        for (int x = 1; x <= numberOfPlayers; x++) {
            if (winner == null) {
                winner = Player [x];
            } else if (Player [x] != null) {
                if (winner.score < Player [x].score) {
                    winner = Player [x];
                } else if (winner.score == Player [x].score) {
                    winner = null;
                }
            }
        }
        if (real) {
            for (int x = 1; x <= numberOfPlayers; x++) {
                PlayerClass player = Player [x];
                if (player != null) {
                    PlayerPropertiesClass properties = player.properties;
                    if (properties != null) {
                        ClientInterface client = properties.client;
                        if (client != null) {
                            string accountName = client.AccountName;
                            if (winner == player) {
                                ServerData.IncrementThisGameModeWon (accountName, Properties.gameMode);
                            } else if (winner != null) {
                                ServerData.IncrementThisGameModeLost (accountName, Properties.gameMode);
                            } else {
                                ServerData.IncrementThisGameModeDrawn (accountName, Properties.gameMode);
                            }
                            ServerData.DecrementThisGameModeUnfinished (accountName, Properties.gameMode);
                        }
                    }
                }
            }
            //Debug.Log ("GameFinished: " + winner.playerNumber + " " + Player [1].score + " " + Player [2].score + " " + turn);
        }
    }

    public void ShowMatchResults (PlayerClass player) {
        int limit = 0;
        switch (winCondition) {
            case 1:
                limit = Properties.scoreLimit;
                break;
            case 2:
                limit = Properties.turnLimit;
                break;
        }
        int experienceGain = 0;
        if (player != null) {
            PlayerPropertiesClass properties = player.properties;
            if (InputController.autoRunAI) {
                RatingClass.AnalyzeStatistics (this);
            }
            if (properties != null) {

                ClientInterface client = properties.client;
                if (client != null) {
                    string accountName = client.AccountName;
                    int matchType = 0;
                    // Puzzle
                    if (Player.Length == 3) {
                        AIClass AI = Player [2].properties.AI;
                        if (AI != null && AI.puzzle && winner == Player [1]) {
                            if (!ServerData.GetUserFinishedPuzzle (accountName, Properties.gameMode)) {
                                client.SavePuzzleResult (Properties.gameMode);
                                matchType = 0;
                                experienceGain += 40;
                            }
                        }
                    }

                    if (winner == player) {
                        experienceGain += turn * 2;
                    } else if (!properties.conceded) {
                        experienceGain += turn;
                    }

                    //

                    ServerLogic.AddExperience (accountName, experienceGain);
                    string winnerName = "";
                    if (winner != null) {
                        winnerName = winner.properties.displayName;
                    }
                    int level = ServerData.GetUserLevel (accountName);
                    int currentExperience = ServerData.GetUserExperience (accountName);
                    int maxExperience = ServerLogic.ExperienceNeededToLevelUp (level);
                    client.TargetShowMatchResult (client.connectionToClient, matchType, winnerName, winCondition, limit, level, currentExperience, maxExperience, experienceGain);
                }
            }
        }
    }

    public void NewMatch (int gameMode, int matchType, int numberOfPlayers) {
        this.numberOfPlayers = numberOfPlayers;
        Properties = new MatchPropertiesClass (gameMode);
        SetPlayers ();

        Board = new BoardClass (this);
        Board.LoadRandomFromGameMode (gameMode, matchType);
    }
    public void RotateAbilityArea (int playerNumber, int stackNumber) {
        Player [playerNumber].RotateTopCard (stackNumber);
    }

    public void MakeRandomMove () {
        List<TileClass> tiles = Board.GetEmptyTiles ();
        if (tiles.Count == 0) {
            return;
        }
        TileClass tile = tiles [Random.Range (0, tiles.Count)];
        PlayCard (tile.x, tile.y, turnOfPlayer, Random.Range (0, 4));
    }

    public void RunAI () {
        Vector3Int output = Player [turnOfPlayer].properties.AI.FindBestMove (this);
        int x = output.x;
        int y = output.y;
        //Debug.Log (x + " " + y + " " + Board.tile [x, y].enabled + " " + Board.tile [x, y].IsEmptyTile () + " ... " + Player [turnOfPlayer].GetTopCard (output.z).cardNumber);
        PlayCard (output.x, output.y, turnOfPlayer, output.z);
    }

    public void PlayCard (int x, int y, int playerNumber, int stackNumber) {
        if (InputController.debuggingEnabled) {
            Debug.Log ("Play card order recieved");
        }
        PlayerClass player = Player [playerNumber];
        CardClass card = player.GetTopCard (stackNumber);
        StackClass stack = Player [playerNumber].GetStack (stackNumber);
        if (real) {
            for (int p = 1; p < 3; p++) {
                //Debug.Log (turn + " " + p + " " + Player [p].hand.atLeast1Enabled);
                for (int s = 0; s < 4; s++) {
                    if (InputController.debuggingEnabled) {
                        Debug.Log (turn + " " + p + " " + Player [p].hand.atLeast1Enabled + " " + s + " " + Player [p].GetStack (s).atLeast1Enabled);
                    }
                }
            }
        }
        if (!Properties.usedCardsArePutOnBottomOfStack && stack != null && !stack.atLeast1Enabled) {
            return;
        }
        PlayCard (lastMoveId + 1, x, y, playerNumber, stackNumber, card.abilityType, card.abilityArea, card.tokenType, card.tokenValue);
    }

    public void PlayCard (int moveID, int x, int y, int playerNumber, int stackNumber, int abilityType, int abilityArea, int tokenType, int tokenValue) {
        if (InputController.debuggingEnabled && real) {
            Debug.Log (moveID);
            Debug.Log (lastMoveId);
        }
        if (lastMoveId + 1 != moveID) {
            return;
        }
        TileClass tile = Board.tile [x, y];
        if (InputController.debuggingEnabled && real) {
            Debug.Log ("Checking conditions.");
            Debug.Log ("Finished: " + finished);
            Debug.Log ("Turn of player: " + turnOfPlayer + ", your number:" + playerNumber);
            Debug.Log ("Tile enabled: " + tile.enabled);
            Debug.Log ("Tile is empty: " + (tile.token == null).ToString ());
            if (tile.token != null) {
                Debug.Log ("Token value: " + tile.token.value);
                Debug.Log ("Token temp value: " + tile.token.tempValue);
                Debug.Log ("Token destoryed: " + tile.token.destroyed);
                Debug.Log ("Token owner: " + tile.token.owner);
                Debug.Log ("Token type: " + tile.token.type);
            }
        }
        if (!finished && turnOfPlayer == playerNumber && tile.enabled && tile.token == null) {
            PlayerClass player = Player [playerNumber];
            CardClass card = new CardClass (tokenValue, tokenType, abilityArea, abilityType);
            VisualPlayCard (playerNumber, card);
            PlayCard (x, y, playerNumber, stackNumber, card);

            FinishMove (x, y, playerNumber, stackNumber, abilityType, abilityArea, tokenType, tokenValue);
        }
    }

    public void FinishMove (int x, int y, int playerNumber, int stackNumber, int abilityType, int abilityArea, int tokenType, int tokenValue) {
        lastMoveId++;
        if (real) {
            foreach (PlayerClass player2 in Player) {
                if (player2 == null || player2.properties == null) {
                    continue;
                }
                ClientInterface client = player2.properties.client;
                if (client != null) {
                    ServerLogic.TargetCurrentGameMakeAMove (client, lastMoveId, x, y, playerNumber, stackNumber, abilityType, abilityArea, tokenType, tokenValue);
                    if (finished) {
                        ShowMatchResults (player2);
                    }
                    //Debug.Log ("Test " + x + " " + y + " " + playerNumber + " " + stackNumber);
                }
            }
        }

        PlayerClass nextPlayer = Player [turnOfPlayer];
        AIClass AI = nextPlayer.properties.AI;
        if (!finished && real && AI != null) {
            RunAI ();
        }
    }

    public void PlayCard (int x, int y, int playerNumber, int stackNumber, CardClass card) {
        PlayCard (Board.tile [x, y], playerNumber, stackNumber, card);
    }

    public void PlayCard (TileClass tile, int playerNumber, int stackNumber, CardClass card) {
        prevMatch = new MatchClass (this);
        PlayerClass player = Player [playerNumber];
        TokenClass token = PlayToken (tile, card, playerNumber);
        int abilityType = card.abilityType;
        HandClass hand = player.GetHand ();
        int topCardNumber = 0;
        if (hand != null) {
            StackClass stack = player.GetStack (stackNumber);
            topCardNumber = player.GetTopCardNumber (stackNumber);
        }
        SaveThisTurnMove (tile.x, tile.y, playerNumber, stackNumber, topCardNumber, card, token);
        player.MoveTopCard (stackNumber, topCardNumber, !Properties.usedCardsArePutOnBottomOfStack);
        UseAbility (tile, playerNumber, stackNumber, topCardNumber, card.abilityArea, abilityType);
        SaveLastMove (tile.x, tile.y, playerNumber, stackNumber, topCardNumber, card, token);
        AILearning (abilityType);
        updateBoard = true;
        UpdateBoard ();
        UpdateVisuals ();
        player.VisualMoveTopCard (stackNumber, topCardNumber, !Properties.usedCardsArePutOnBottomOfStack);
        EndTurn ();
    }

    public void VisualPlayCard (int playerNumber, CardClass card) {
        if (visualMatch != null) {
            bool player = playerNumber == ClientLogic.MyInterface.playerNumber;
            int playerPosition = 0;
            if (!player) {
                for (int x = 1; x < Player.Length; x++) {
                    if (Player [x] != null) {
                        if (x == playerNumber) {
                            break;
                        }
                        if (x != ClientLogic.MyInterface.playerNumber) {
                            playerPosition++;
                        }
                    }
                }
            }
            visualMatch.PlayCard (playerNumber, player, playerPosition, card);
        }
    }

    public void SaveThisTurnMove (int x, int y, int playerNumber, int stackNumber, int playedCardNumber, CardClass playedCard, TokenClass token) {
        ThisTurnMove = new MoveHistoryClass (lastMoveId + 1, x, y, playerNumber, stackNumber, playedCardNumber, playedCard, token, LastMove);
    }

    public void SaveLastMove (int x, int y, int playerNumber, int stackNumber, int playedCardNumber, CardClass playedCard, TokenClass token) {
        LastMove = ThisTurnMove;
        Player [playerNumber].LastMove = new MoveHistoryClass (lastMoveId + 1, x, y, playerNumber, stackNumber, playedCardNumber, playedCard, token, Player [playerNumber].LastMove);
    }

    public CardClass GetLastPlayedCard () {
        return GetPlayedCard (LastMove);
    }

    public CardClass GetPlayedCard (MoveHistoryClass move) {
        if (LastMove == null) {
            return null;
        } else {
            return Player [move.playerNumber].GetCard (move.stackNumber, move.usedCardNumber);
        }
    }

    public void UpdateBoard () {
        if (!updateBoard) {
            return;
        }
        if (visualMatch != null) {
            VisualMatch.GlobalTimer += 0.5f;
        }
        updateBoard = false;
        foreach (TileClass tile in Board.tileList) {
            tile.UpdateTempValue ();
        }
        foreach (TileClass tile in Board.tileList) {
            tile.Update ();
        }
        UpdateBoard ();
    }

    public VectorInfo GetVectorInfo (int x, int y, int playerNumber, int abilityArea, int abilityType, TokenClass token) {
        return GetVectorInfo (Board.tile [x, y], abilityArea, abilityType, token);
    }

    public VectorInfo GetVectorInfo (TileClass tile, int abilityArea, int abilityType, TokenClass token) {
        AbilityVector [] vectors = Board.GetAbilityVectors (tile.x, tile.y, abilityArea);
        VectorInfo info = new VectorInfo (vectors, token);
        info.CheckAbilityTriggers (this, tile, abilityType, token);
        return info;
    }

    public void TokenLeavesTile (TokenClass token) {
        switch (token.type) {
            case 13:
                if (ThisTurnPlayedToken () != null) {
                    SetDestroy (ThisTurnPlayedTile ());
                }
                break;
        }

    }

    public void DestroyToken (TokenClass token, int x, int y) {
        TokenLeavesTile (token);
        TileClass tile = Board.GetTile (x, y);
        VectorInfo VI = GetTokenVectorInfo (tile, token);
        int tokenType = token.type;
        switch (tokenType) {
            case 5:
                if (visualMatch != null) {
                    visualMatch.CreateRealTokenEffect (tile, tokenType);
                }
                break;
            case 9:
                Board.BeforeAbilityTriggers.Remove (tile);
                break;
            case 14:
                Board.BeforeTokenPlayedTriggers.Remove (tile);
                break;
            case 12:
                if (visualMatch != null) {
                    visualMatch.CreateRealTokenEffect (tile, tokenType);
                }
                Player [token.owner].score -= 20;
                break;
        }
        foreach (TileClass target in VI.Triggered1) {
            switch (tokenType) {
                case 5:
                    if (visualMatch != null) {
                        visualMatch.CreateRealTokenVectorEffect (tile, target, tokenType);
                    }
                    ModifyTempValue (target, -3);
                    break;
            }
        }
    }

    public VectorInfo GetTokenVectorInfo (TileClass tile, TokenClass token) {
        AbilityVector [] vectors = Board.GetAbilityVectors (tile.x, tile.y, 4);
        VectorInfo info = new VectorInfo (vectors, token);
        info.CheckTokenTriggers (this, tile, token);
        return info;
    }

    public void UseAbility (TileClass tile, int playerNumber, int stackNumber, int cardNumber, int abilityArea, int abilityType) {
        abilityType = VerifyAbilityType (tile, abilityType);
        VectorInfo info = GetVectorInfo (tile, abilityArea, abilityType, tile.token);
        if (visualMatch != null) {
            VisualEffectInterface.DelayedCreateRealEffects (info, abilityType);
            VisualMatch.GlobalTimer += 0.5f;
        }
        if (tile.token == null) {
            Debug.Log (51);
        }
        int tokenType = tile.token.type;
        UseAbilityTrigger1 (info, tile, playerNumber, abilityType, tokenType);
        UseAbilityTrigger2 (info, tile, playerNumber, abilityType, tokenType);
        UseAbilityVector (info, tile, playerNumber, abilityType, tokenType);
        UseAbilityConstant (info, tile, playerNumber, stackNumber, cardNumber, abilityType, tokenType);
    }

    public void AILearning (int abilityType) {
        if (!real) {
            return;
        }
        for (int x = 1; x <= numberOfPlayers; x++) {
            PlayerClass player = Player [x];
            if (player != null) {
                PlayerPropertiesClass properties = player.properties;
                if (properties != null) {
                    AIClass AI = properties.AI;
                    if (AI != null) {
                        switch (abilityType) {
                            case 5:
                                AI.edgeDanger += 3;
                                break;
                        }
                    }
                }
            }

        }
    }

    public int VerifyAbilityType (TileClass tile, int abilityType) {
        int newAbilityType = abilityType;
        List<TileClass> removeFromList = new List<TileClass> ();
        foreach (TileClass triggeredTile in Board.BeforeAbilityTriggers) {
            if (triggeredTile != null) {
                TokenClass triggeredToken = triggeredTile.token;
                if (triggeredToken != null) {
                    int triggeredType = triggeredToken.type;
                    switch (triggeredType) {
                        case 9:
                            if (abilityType != 0 && tile != triggeredTile) {
                                newAbilityType = 0;
                                ChangeType (triggeredTile, 0);
                                removeFromList.Add (triggeredTile);
                            }
                            break;

                    }
                } else {
                    //Debug.Log ("Wut");
                }

            } else {
                Debug.Log ("wut2");
            }
        }
        foreach (TileClass triggeredTile in removeFromList) {
            Board.BeforeAbilityTriggers.Remove (triggeredTile);
        }
        //Debug.Log ("2");

        abilityType = newAbilityType;
        switch (abilityType) {
            case 7:
                CardClass lastPlayedCard = GetLastPlayedCard ();
                bool triggered = LastMove != null && lastPlayedCard.abilityType != abilityType;
                if (visualMatch != null) {
                    VisualEffectInterface.DelayedRealEffect (tile.x, tile.y, abilityType, triggered);
                }
                if (triggered) {
                    abilityType = lastPlayedCard.abilityType;
                }
                break;
            case 38:
                if (visualMatch != null) {
                    VisualEffectInterface.DelayedRealEffect (tile.x, tile.y, abilityType, true);
                }
                break;
        }
        return abilityType;
    }

    public void UseAbilityTrigger1 (VectorInfo info, TileClass tile, int playerNumber, int abilityType, int tokenType) {
        foreach (TileClass target in info.Triggered1) {
            if (target == null) {
                Debug.Log ("30");
            }
            switch (abilityType) {
                case 1:
                    ModifyTempValue (target, -1);
                    break;
                case 2:
                    ModifyTempValue (target, 1);
                    break;
                case 3:
                    DisableEmptyTile (target);
                    break;
                case 4:
                    Player [playerNumber].AddScore (GetTokenValue (target));
                    break;
                case 6:
                    ModifyTempValue (tile, -1);
                    break;
                case 8:
                    ModifyTempValue (LastPlayedToken ().tile, -1);
                    break;
                case 9:
                    ModifyTempValue (target, 2);
                    break;
                case 10:
                    ChangeType (target, 0);
                    break;
                case 12:
                    ModifyTempValue (target, -4);
                    break;
                case 13:
                    ModifyTempValue (target, tile.token.value - target.token.value);
                    break;
                case 14:
                    SetDestroy (target);
                    break;
                case 15:
                    ModifyTempValue (tile, target.token.value);
                    ModifyTempValue (target, -target.token.value);
                    break;
                case 16:
                case 33:
                    ChangeType (target, tokenType);
                    break;
                case 17:
                    ChangeType (tile, target.token.type);
                    ChangeType (target, tokenType);
                    break;
                case 18:
                    CreateToken (target, LastPlayedToken ().type, LastPlayedToken ().value, playerNumber);
                    break;
                case 19:
                    SwapToken (tile, target);
                    break;
                case 21:
                    SetDestroy (target);
                    break;
                case 23:
                    ModifyTempValue (target, 1);
                    ModifyTempValue (info.Triggered2 [0], -1);
                    break;
                case 24:
                    ModifyTempValue (target, -info.allyCount);
                    break;
                case 25:
                case 31:
                    CreateToken (target, 0, 1, playerNumber);
                    break;
                case 26:
                    ModifyTempValue (target, 1);
                    break;
                case 27:
                    ModifyTempValue (target, 1);
                    break;
                case 28:
                    ModifyTempValue (target, LastPlayedToken ().value);
                    break;
                case 29:
                case 30:
                case 35:
                    SetDestroy (target);
                    break;
                case 32:
                    ModifyTempValue (tile, 1);
                    break;
                case 39:
                    CreateToken (target, info.Triggered2 [0].token.type, 1, playerNumber);
                    break;
                case 41:
                    ModifyTempValue (target, -3);
                    break;
                case 43:
                    ModifyTempValue (target, -1);
                    break;
            }
        }
    }

    public void UseAbilityTrigger2 (VectorInfo info, TileClass tile, int playerNumber, int abilityType, int tokenType) {
        foreach (TileClass target in info.Triggered2) {
            if (target == null) {
                Debug.Log ("31");
            }
            switch (abilityType) {
                case 2:
                    CreateToken (target, 0, 1, playerNumber);
                    break;
                case 11:
                    SwapToken (tile, target);
                    break;
                case 21:
                    ModifyTempValue (target, -info.sumOfAlliesValues);
                    break;
                case 22:
                    ChangeType (target, tokenType);
                    break;
                case 26:
                    ModifyTempValue (target, -1);
                    break;
                case 28:
                    ModifyTempValue (target, LastPlayedToken ().value);
                    break;
                case 43:
                    ModifyTempValue (target, 1);
                    break;
            }
        }
    }

    public void UseAbilityVector (VectorInfo info, TileClass tile, int playerNumber, int abilityType, int tokenType) {
        foreach (AbilityVector vector in info.TriggeredVector) {
            if (vector == null) {
                Debug.Log ("21");
            }
            switch (abilityType) {
                case 5:
                case 34:
                    if (vector.target == null) {
                        Debug.Log ("20");
                    }
                    MoveToken (vector.target, vector.pushX, vector.pushY);
                    break;
            }
        }
    }

    public void UseAbilityConstant (VectorInfo info, TileClass tile, int playerNumber, int stackNumber, int cardNumber, int abilityType, int tokenType) {
        switch (abilityType) {
            case 20:
                if (info.TargetPlayers != null) {
                    foreach (int pNumber in info.TargetPlayers) {
                        if (Player.Length <= pNumber) {
                            continue;
                        }
                        PlayerClass player = Player [pNumber];
                        if (player == null) {
                            continue;
                        }
                        HandClass hand = player.GetHand ();
                        if (hand != null) {
                            for (int y = 0; y < hand.stack.Length; y++) {
                                player.MoveTopCard (y);
                            }
                        }
                    }
                }
                break;
            case 30:
            case 33:
                if (Player [playerNumber].score >= 20) {
                    Player [playerNumber].AddScore (-20);
                }
                break;
            case 36:
                if (info.Triggered1.Count == 2) {
                    SwapToken (info.Triggered1 [0], info.Triggered1 [1]);
                }
                break;
            case 37:
                if (info.Triggered1.Count != 2) {
                    break;
                }
                TileClass tile1 = info.Triggered1 [0];
                TileClass tile2 = info.Triggered1 [1];
                int type1 = tile1.token.type;
                int type2 = tile2.token.type;
                ChangeType (tile1, type2);
                ChangeType (tile2, type1);
                break;
            case 38: 
                {
                    PlayerClass player = Player [playerNumber];
                    if (player.hand == null) {
                        break;
                    }
                    CardClass card = player.GetCard (stackNumber, cardNumber);
                    ModifyCardValue (card, -1);
                }
                break;
            case 40: {
                if (info.remainsCount < 2) {
                    ChangeType (tile, 0);
                }
            }
            break;
            case 42: {
                PlayerClass player = Player [playerNumber];
                if (player.hand == null) {
                    break;
                }
                CardClass card = player.GetCard (stackNumber, cardNumber);
                SetCardValue (card, 1);
            }
            break;
            /*
            case 40:
                if (info.TargetPlayers != null) {
                    foreach (int pNumber in info.TargetPlayers) {
                        if (Player.Length <= pNumber) {
                            continue;
                        }
                        PlayerClass player = Player [pNumber];
                        if (player == null) {
                            continue;
                        }
                        CardClass card = player.GetLastMoveCard ();
                        if (card != null) {
                            card.tokenValue--;
                        }
                    }
                }
                break;*/
        }
    }

    public void ModifyCardValue (CardClass card, int value) {
        card.tokenValue += value;
        card.DelayedSetState ();
    }

    public void SetCardValue (CardClass card, int value) {
        card.tokenValue = value;
        card.DelayedSetState ();
    }

    public void ChangeType (TileClass tile, int newType) {
        tile.token.ChangeType (newType);
    }

    public TileClass ThisTurnPlayedTile () {
        if (ThisTurnMove == null) {
            return null;
        }
        TokenClass token = ThisTurnMove.playedToken;
        if (token != null) {
            return Board.tile [token.x, token.y];
        }
        return null;
    }

    public TokenClass ThisTurnPlayedToken () {
        if (ThisTurnMove == null) {
            return null;
        }
        return ThisTurnPlayedTile ().token;
    }

    public TileClass LastPlayedTile () {
        if (LastMove == null) {
            return null;
        }
        TokenClass token = LastMove.playedToken;
        if (token != null) {
            if (ThisTurnMove != null) {
                TokenClass token2 = ThisTurnMove.playedToken;
                if (token2 != null && token.x == token2.x && token.y == token2.y) {
                    return null;
                }
            }
            return Board.tile [token.x, token.y];
        }
        return null;
    }

    public TokenClass LastPlayedToken () {
        if (LastMove == null || LastPlayedTile () == null) {
            return null;
        }
        return LastPlayedTile ().token;
    }

    public void ModifyTempValue (TileClass tile, int value) {
        if (tile != null) {
            TokenClass target = tile.token;
            if (target != null) {
                updateBoard = true;
                target.ModifyTempValue (value);
            }
        }
    }

    public void SetDestroy (TileClass tile) {
        if (tile != null) {
            TokenClass target = tile.token;
            if (target != null) {
                //updateBoard = true;
                target.destroyed = true;
            }
        }
    }

    public TokenClass PlayToken (TileClass tile, CardClass card, int playerNumber) {
        if (tile != null) {
            TokenClass token = CreateToken (tile, card.tokenType, card.tokenValue + Board.NumberOfTypes [7] - Board.NumberOfTypes [11], playerNumber);
            //TokenClass token = tile.CreateToken (card, playerNumber);
            if (tile.visualTile != null) {
                tile.token.visualToken.DelayedAddPlayAnimation ();
            }
            foreach (TileClass triggeredTile in Board.BeforeTokenPlayedTriggers) {
                if (triggeredTile != null) {
                    TokenClass triggeredToken = triggeredTile.token;
                    if (triggeredToken != null) {
                        int triggeredType = triggeredToken.type;
                        switch (triggeredType) {
                            case 14:
                                if (triggeredToken.value < token.value && RelationLogic.IsEnemy (triggeredTile, token.owner)) {
                                    ModifyTempValue (triggeredTile, 1);
                                }
                                break;

                        }
                    }
                }
            }
            return token;
        }
        return null;
    }

    public TokenClass CreateToken (TileClass tile, int type, int value, int playerNumber) {
        if (tile != null) {
            return tile.CreateToken (type, value, playerNumber);
        }
        return null;
    }

    public void DisableEmptyTile (TileClass tile) {
        if (tile != null && tile.token == null) {
            DisableTile (tile);
        }
    }

    public void DisableTile (TileClass tile) {
        if (tile != null) {
            tile.EnableTile (false);
        }
    }

    public void MoveToken (TileClass sourceTile, int destX, int destY) {
        TileClass destinationTile = Board.GetTile (destX, destY);
        if (sourceTile != null && sourceTile.token != null) {
            TokenClass sourceToken = sourceTile.token;
            TokenLeavesTile (sourceToken);
            if (destinationTile != null && destinationTile.enabled) {
                if (destinationTile.token == null) {
                    sourceTile.AttachToken (null);
                    destinationTile.AttachToken (sourceToken);
                }
            } else {
                sourceToken.MoveToDisabledTile (destX, destY);
                //DestroyToken (sourceTile);
            }
        }
    }

    public void SwapToken (TileClass sourceTile, TileClass destinationTile) {
        if (sourceTile != null && sourceTile.token != null) {
            TokenClass sourceToken = sourceTile.token;
            if (destinationTile != null && destinationTile.enabled) {
                TokenClass destinationToken = destinationTile.token;
                if (destinationToken != null) {
                    TokenLeavesTile (destinationToken);
                    TokenLeavesTile (sourceToken);
                    sourceTile.AttachToken (destinationToken);
                    destinationTile.AttachToken (sourceToken);
                }
            }
        }
    }

    public int GetTokenValue (TileClass tile) {
        if (tile != null && tile.token != null) {
            return tile.token.value;
        }
        return 0;
    }

    void SetPlayers () {
        Player = new PlayerClass [numberOfPlayers + 1];
        Player [0] = new PlayerClass ();
    }

    public void SetPlayer (int PlayerNumber, PlayerClass player) {
        if (player == null) {
            return;
        }
        Player [PlayerNumber] = player;
        PlayerPropertiesClass properties = player.properties;
        properties.playerNumber = PlayerNumber;
        ClientInterface client = properties.client;
        if (client != null) {
            client.playerNumber = PlayerNumber;
        }
        AIClass AI = properties.AI;
        if (AI != null) {
            AI.MaxEmptyTileCount = Board.GetEmptyTilesCount ();
        }
    }

    public void EnableVisuals () {
        if (visualMatch == null) {
            visualMatch = InGameUI.instance.gameObject.AddComponent<VisualMatch> ();
            visualMatch.EnableVisual ();
        }
        Board.EnableVisualisation ();
        for (int x = 1; x < Player.Length; x++) {
            if (Player [x] != null) {
                Player [x].EnableVisuals ();
            }
        }
    }

    public void DestroyVisuals () {
        Board.DestroyAllVisuals ();
        foreach (PlayerClass player in Player) {
            if (player != null) {
                player.DestroyVisuals ();
            }
        }
        if (visualMatch != null) {
            visualMatch.DestroyVisuals ();
        }
    }

}
