using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MatchClass {

    public BoardClass Board;

    public int turn = 1;
    public int turnOfPlayer = 1;

    public int numberOfPlayers;
    public PlayerClass [] Player;

    public List<PlayerClass> winner = new List<PlayerClass> (0);
    public bool finished;
    public int winCondition;
    public bool real = true;
    bool _updateBoard = false;

    public bool updateBoard {
        get {
            //Debug.Log ("Test");
            return _updateBoard;
        }
        set {
            _updateBoard = value;
        }
    }

    public MoveHistoryClass ThisTurnMove;
    public MoveHistoryClass LastMove;
    public int lastMoveId = 0;

    public MatchPropertiesClass properties;

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
        this.Player = new PlayerClass [match.Player.Length];
        for (int x = 0; x < this.Player.Length; x++) {
            PlayerClass player = match.Player [x];
            if (player != null) {
                this.Player [x] = new PlayerClass (match.Player [x]);
            }
        }
        this.LastMove = match.LastMove;
        this.properties = match.properties;
        this.prevMatch = match.prevMatch;
    }

    public void Concede (string accountName) {
        for (int x = 1; x < Player.Length; x++) {
            if (Player [x] != null && Player [x].properties != null && Player [x].properties.accountName == accountName) {
                Concede (x);
            }
        }
    }

    public void Concede (int playerNumber) {
        if (finished) {
            return;
        }
        PlayerPropertiesClass concededPlayer = Player [playerNumber].properties;
        concededPlayer.conceded = true;
        if (concededPlayer.client != null) {
            ServerLogic.AddExperience (concededPlayer.client, concededPlayer.accountName, turn);
        }
        PlayerPropertiesClass winner = null;
        foreach (PlayerClass player in Player) {
            if (player == null) {
                continue;
            }
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

        bool [] cantGetIncomeFromClosedTokens = new bool [Player.Length];
        for (int x = 1; x < Player.Length; x++) {
            PlayerPropertiesClass properties = GetPlayerProperties (x);
            if (properties == null) {
                continue;
            }
            if (properties.specialStatus == 8) {
                for (int y = 1; y < Player.Length; y++) {
                    PlayerPropertiesClass properties2 = GetPlayerProperties (y);
                    if (properties.team != properties2.team) {
                        cantGetIncomeFromClosedTokens [y] = true;
                    }
                }
            }
        }

        foreach (TileClass tile in Board.tileList) {
            TokenClass token = tile.token;
            if (token != null) {
                if (token.owner >= playerIncome.Length) {
                    continue;
                }
                int tokenValue = token.value;
                int value = tokenValue;
                VectorInfo info = GetTokenVectorInfo (tile, token);
                if (cantGetIncomeFromClosedTokens [token.owner] && info.emptyTileCount == 0) {
                    continue;
                }
                // Additive values
                switch (token.type) {
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
                            case 19:
                                value++;
                                break;
                        }
                    }
                }

                // multiplicative values
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
                }

                playerIncome [token.owner] += value;
            } else {
                for (int x = 1; x <= numberOfPlayers; x++) {
                    PlayerClass player = Player [x];
                    if (player == null) {
                        continue;
                    }
                    PlayerPropertiesClass properties = player.properties;
                    if (player.properties == null) {
                        continue;
                    }
                    switch (player.properties.specialStatus) {
                        case 1:
                            playerIncome [x]++;
                            break;
                    }
                }
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
        CheckFinishCondition (turnOfPlayer);
        if (!finished) {
            AfterTurn ();
        }

        //AIClass AI = Player [1].properties.AI;
        // Debug.Log (AI.TurnToWinPredict (this, 1) + " " + AI.TurnToWinPredict (this, 2) + " " + AI.CalculateMatchValue (this, 1) + " " + AI.CalculateMatchValue (this, 2));
    }

    public void AfterTurn () {
        /*if (!real) {

            Debug.Log ("Things happening");
        }*/
        bool nextPlayerTurn = true;
        bool [] destroyIsolatedTokens = new bool [numberOfPlayers + 1];
        bool [] destroyTokens = new bool [numberOfPlayers + 1];

        for (int x = 1; x <= numberOfPlayers; x++) {
            PlayerClass player = Player [x];
            if (player == null) {
                continue;
            }
            PlayerPropertiesClass properties = player.properties;
            if (properties != null) {
                switch (properties.specialStatus) {
                    case 4:
                        for (int y = 1; y <= numberOfPlayers; y++) {
                            if (x != y) {
                                destroyIsolatedTokens [y] = true;
                                if (visualMatch) {
                                    SoundManager.AddAudioClipToQueue (MyAudioClip.AbilityTokenDestruction);
                                }
                            }
                        }
                        break;
                    case 10:
                        if (turnOfPlayer != x) {
                            break;
                        }
                        for (int y = 1; y <= numberOfPlayers; y++) {
                            PlayerClass player2 = GetPlayer (y);
                            if (player2 == null) {
                                continue;
                            }
                            if (player.previousScoreIncome <= player2.previousScoreIncome &&
                                player.scoreIncome > player2.scoreIncome) {
                                nextPlayerTurn = false;
                                if (visualMatch) {
                                    SoundManager.AddAudioClipToQueue (MyAudioClip.TokenTrigger);
                                }
                            }
                        }
                        break;
                    case 11:
                        if (turnOfPlayer != player.properties.playerNumber) {
                            break;
                        }
                        for (int y = 1; y <= numberOfPlayers; y++) {
                            PlayerClass player2 = GetPlayer (y);
                            if (player2 == null) {
                                continue;
                            }
                            if (player2.scoreIncome < 12) {
                                continue;
                            }
                            if (x != y) {
                                destroyTokens [y] = true;
                                if (visualMatch) {
                                    SoundManager.AddAudioClipToQueue (MyAudioClip.AbilityTokenDestruction);
                                }
                            }
                        }
                        break;

                }
            }
            HandClass hand = player.hand;
            if (hand == null) {
                continue;
            }
            foreach (StackClass stack in hand.stack) {
                CardClass card = stack.getTopCard ();
                if (card == null) {
                    continue;
                }
                switch (card.abilityType) {
                    case 61:
                        ModifyCardValue (card, 2);
                        break;
                }
            }
        }
        foreach (TileClass tile in Board.tileList) {
            if (!tile.IsFilledTile ()) {
                continue;
            }
            TokenClass token = tile.token;
            int tokenType = token.type;
            VectorInfo info = GetTokenVectorInfo (tile, token);
            if (token.owner <= numberOfPlayers && ((destroyIsolatedTokens [token.owner] && info.targets.Count == 0) || destroyTokens [token.owner])) {
                SetDestroy (tile);
            }
            PlayerClass player = GetPlayer (token.owner);
            if (player != null && player.properties != null) {
                switch (player.properties.specialStatus) {
                    case 12:
                        TokenClass thisTurnPlayedToken = ThisTurnPlayedToken ();
                        if (turnOfPlayer == player.properties.playerNumber && thisTurnPlayedToken != null && tile.token.type != thisTurnPlayedToken.type) {
                            ChangeType (tile, thisTurnPlayedToken.type);
                            if (visualMatch) {
                                SoundManager.AddAudioClipToQueue (MyAudioClip.MatchEvent);
                            }
                        }
                        break;
                }
            }
            switch (tokenType) {
                case 5:
                    ModifyTempValue (tile, -1);
                    if (visualMatch) {
                        SoundManager.AddAudioClipToQueue (MyAudioClip.TokenNegativeRegularAbility);
                    }
                    break;
                case 13:
                    //Debug.Log ("Extra turn token " + tile.x + " " + tile.y);
                    nextPlayerTurn = false;
                    ChangeType (tile, 0);
                    SoundManager.AddAudioClipToQueue (MyAudioClip.TokenTrigger);
                    break;
                case 16:
                    if (prevMatch != null) {
                        TokenClass prevToken = prevMatch.Board.GetTile (tile.x, tile.y).token;
                        if (prevToken != null) {
                            int delta = prevToken.value - tile.token.value;
                            if (delta != 0) {
                                ModifyTempValue (tile, delta);
                                if (visualMatch != null) {
                                    visualMatch.CreateRealTokenEffect (tile, tokenType);
                                    SoundManager.AddAudioClipToQueue (MyAudioClip.TokenPositiveRegularAbility);
                                }
                            }
                        }
                    }
                    break;
                case 20:
                    SetDestroy (tile);
                    SoundManager.AddAudioClipToQueue (MyAudioClip.AbilityTokenDestruction);
                    break;
            }
            switch (tokenType) {
                case 13:
                    if (visualMatch != null) {
                        visualMatch.CreateRealTokenEffect (tile, tokenType);
                    }
                    break;
            }
            foreach (TileClass target in info.Triggered1) {
                switch (tokenType) {
                    case 3:
                    case 4:
                        if (visualMatch != null) {
                            visualMatch.CreateRealTokenVectorEffect (tile, target, tokenType);
                        }
                        break;
                    case 22:
                    case 23:
                        if (visualMatch != null) {
                            visualMatch.CreateRealTokenVectorEffect (tile, target, tokenType);
                        }
                        break;
                }
                switch (tokenType) {
                    case 3:
                        ModifyTempValue (target, 1);
                        if (visualMatch) {
                            SoundManager.AddAudioClipToQueue (MyAudioClip.TokenPositiveAbility);
                        }
                        break;
                    case 4:
                        ModifyTempValue (target, -1);
                        if (visualMatch) {
                            SoundManager.AddAudioClipToQueue (MyAudioClip.TokenNegativeAbility);
                        }
                        break;
                    case 22:
                        ModifyTempValue (tile, -1);
                        ModifyTempValue (target, 1);
                        if (visualMatch) {
                            SoundManager.AddAudioClipToQueue (MyAudioClip.TokenPositiveAbility);
                        }
                        break;
                    case 23:
                        ModifyTempValue (target, -1);
                        if (visualMatch) {
                            SoundManager.AddAudioClipToQueue (MyAudioClip.TokenNegativeAbility);
                        }
                        break;
                }
            }
        }
        if (visualMatch) {
            SoundManager.PlayQueuedAudioClips ();
        }

        UpdateBoard ();
        UpdateVisuals ();
        NewTurn (nextPlayerTurn);
    }

    public int TurnsLeft () {
        return properties.turnLimit - turn + 1;
    }

    public int GetTeam (TileClass tile) {
        TokenClass token = tile.token;
        if (token == null) {
            return 0;
        } else {
            return GetTeam (token.owner);
        }
    }

    public int GetTeam (int playerNumber) {
        PlayerPropertiesClass properties = GetPlayerProperties (playerNumber);
        if (properties == null) {
            return 0;
        } else {
            return properties.team;
        }
    }

    public void NewTurn (bool nextPlayerTurn) {
        //Debug.Log ("NewTurn");
        ThisTurnMove = null;
        if (nextPlayerTurn) {
            //Debug.Log (turnOfPlayer);
            IncrementTurnOfPlayer ();
            //Debug.Log (turnOfPlayer);
        }
        CheckFinishCondition (turnOfPlayer);
        turn++;
        if (visualMatch != null) {
            if (properties.turnWinCondition) {
                visualMatch.UpdateTurnsLeft (TurnsLeft ());
            }
            if (real) {
                VisualMatch.GlobalTimer += AppSettings.TimeBetweenTurns;
                TutorialManager.NewState (TutorialManager.newTurnState);
            }
        }
        if (true) {
            string s = turn.ToString () + " " + real.ToString();
            for (int x = 1; x <= numberOfPlayers; x++) {
                if (GetPlayerProperties (x) == null) {
                    continue;
                }
                s += "Player: " + x + System.Environment.NewLine;
                if (Player [x].hand == null) {
                    continue;
                }
                StackClass [] stack = Player [x].hand.stack;
                for (int y = 0; y < stack.Length; y++) {
                    s += "Stack: " + y + ", Top card: " + stack [y].topCardNumber + System.Environment.NewLine;
                }
            }
            //Debug.Log (s);
        }
    }

    public void IncrementTurnOfPlayer () {
        if (real) {
            //Debug.Log ("1TurnOfPlayer " + turnOfPlayer);
            //Debug.Log ("NumberOfPlayers: " + numberOfPlayers);
        }
        int newTurnOfPlayer = turnOfPlayer;
        do {
            newTurnOfPlayer = Mathf.Max (1, (newTurnOfPlayer + 1) % (numberOfPlayers + 1));
            if (real) {
               // Debug.Log ("NewTurnOfPlayer: " + newTurnOfPlayer);
            }
        } while (!AbleToExecuteTurn (newTurnOfPlayer) && newTurnOfPlayer != turnOfPlayer);
        SetTurnOfPlayer (newTurnOfPlayer);
        if (real) {
            //Debug.Log ("2TurnOfPlayer " + turnOfPlayer);
        }
    }

    public bool AbleToExecuteTurn (int playerNumber) {
        PlayerClass player = Player [playerNumber];
        if (player == null || player.properties == null) {
            return false;
        }
        if (player.properties.conceded || !player.enabled) {
            return false;
        }
        HandClass hand = player.hand;
        if (!(hand == null || player.hand.atLeast1Enabled)) {
            if (real) {
                //Debug.Log ("BreakAfterCondition3");
            }
            return false;
        }
        foreach (TileClass tile in Board.tileList) {
            if (tile.IsPlayable (playerNumber)) {
                return true;
            }
        }
        return false;
    }

    public void SetTurnOfPlayer (int turnOfPlayer) {
        this.turnOfPlayer = turnOfPlayer;
        if (visualMatch != null) {
            foreach (PlayerClass player in Player) {
                if (player != null) {
                    VisualTeam vPlayer = player.visualTeam;
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

    public void CheckFinishCondition (int playerNumber) {
        // Counting number of player tokens
        int [] teamScore = GetTeamScore ();
        int [] tokenCount = new int [5];
        bool [] enabledTeams = new bool [5];
        bool atLeastOneDisabled = false;
        foreach (TileClass tile in Board.tileList) {
            if (tile.IsFilledTile ()) {
                /*if (!real) {
                    Debug.Log (tile.token.value + " " + tile.token.tempValue + " " + tile.token.owner + " " + tile.token.destroyed + " " + tile.x + " " + tile.y);
                }*/
                tokenCount [tile.token.owner]++;
            }
        }
        for (int x = 1; x <= numberOfPlayers; x++) {
            PlayerClass player = Player [x];
            if (player == null) {
                continue;
            }
            PlayerPropertiesClass properties = player.properties;
            if (properties == null) {
                continue;
            }
            if (properties.specialStatus == 6) {
                for (int y = 1; y <= numberOfPlayers; y++) {
                    PlayerClass player2 = Player [y];
                    if (player2 == null) {
                        continue;
                    }
                    PlayerPropertiesClass properties2 = player2.properties;
                    if (properties2 == null) {
                        continue;
                    }
                    if (properties.team != properties2.team && tokenCount [y] == 0) {
                        FinishGame (5, 0);
                    }
                }
            }
            if (properties.specialStatus == 7 && tokenCount [x] == 0 && !player.lost) {
                player.enabled = false;
                player.lost = true;
                ShowMatchResults (player);
            }
            if (!player.lost) {
                enabledTeams [properties.team] = true;
            } else {
                atLeastOneDisabled = true;
            }
        }
        if (atLeastOneDisabled) {
            int enabledTeamsCount = 0;
            for (int x = 0; x < enabledTeams.Length; x++) {
                if (enabledTeams [x]) {
                    enabledTeamsCount++;
                }
            }
            if (enabledTeamsCount == 1) {
                FinishGame (6, 0);
            }
        }
        
        if (properties.scoreWinCondition) {
            for (int x = 1; x <= numberOfPlayers; x++) {
                PlayerPropertiesClass playerProperties = GetPlayerProperties (x);
                if (playerProperties != null && teamScore [playerProperties.team] > properties.scoreLimit) {
                    FinishGame (1, properties.scoreLimit);
                    return;
                }
            }
        }
        if (properties.turnWinCondition) {
            if (turn >= properties.turnLimit) {
                FinishGame (2, properties.turnLimit);
                return;
            }
        }
        if (Board.GetPlayableTiles (playerNumber).Count == 0) {
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

    public int [] GetTeamScore () {
        int [] teamScore = new int [5];
        for (int x = 1; x <= numberOfPlayers; x++) {
            PlayerClass player = Player [x];
            if (player == null) {
                continue;
            }
            PlayerPropertiesClass properties = player.properties;
            if (properties == null) {
                continue;
            }
            teamScore [properties.team] += player.score;
        }
        return teamScore;
    }

    public void FinishGame (int winCondition, int limit) {
        this.winCondition = winCondition;
        finished = true;
        int [] teamScore = GetTeamScore ();

        // Counting number of player tokens
        int [] tokenCount = new int [5];
        foreach (TileClass tile in Board.tileList) {
            if (tile.IsFilledTile ()) {
                tokenCount [tile.token.owner]++;
            }
        }
        bool strongestTokenRequired = false;
        for (int x = 1; x <= numberOfPlayers; x++) {
            PlayerPropertiesClass properties = GetPlayerProperties (x);
            if (properties == null) {
                continue;
            }
            switch (properties.specialStatus) {
                case 5:
                    strongestTokenRequired = true;
                    break;
                case 6:
                    break;
            }
        }

        int bestScore = -99999;
        for (int x = 1; x <= numberOfPlayers; x++) {
            PlayerClass player = GetPlayer (x);
            if (player == null) {
                continue;
            }
            PlayerPropertiesClass properties = player.properties;
            if (properties == null || player.lost) {
                continue;
            }
            if (winCondition == 6) {
                if (player.enabled) {
                    winner.Add (player);
                }
                continue;
            }
            if (winCondition == 5) {
                if (properties.specialStatus == 6) {
                    winner.Add (player);
                }
                continue;
            }
            if (properties.specialStatus == 7 && tokenCount [x] == 0) {
                continue;
            }
            if (winner.Count == 0) {
                winner.Add (player);
                bestScore = teamScore [properties.team];
            } else if (player != null) {
                if (bestScore < teamScore [properties.team]) {
                    winner.Clear ();
                    winner.Add (player);
                    bestScore = teamScore [properties.team];
                } else if (bestScore == teamScore [properties.team]) {
                    int anotherWinnerCount = winner.Count;
                    bool skip = false;
                    for (int y = 0; y < anotherWinnerCount; y++) {
                        if (winner [0].properties.team == properties.team) {
                            winner.Add (player);
                            skip = true;
                            break;
                        }
                    }
                    if (skip) {
                        continue;
                    }
                    winner.Clear ();
                }
            }
        }
        if (winner.Count > 0 && winner [0].properties != null && strongestTokenRequired) {
            int strongestValue = 0;
            bool conditionMeet = false;
            foreach (TileClass tile in Board.tile) {
                TokenClass token = tile.token;
                if (token == null) {
                    continue;
                }
                if (strongestValue < token.value) {
                    if (token.owner == winner [0].properties.playerNumber) {
                        conditionMeet = true;
                    }
                }
                if (strongestValue <= token.value) {
                    strongestValue = token.value;
                    if (token.owner != winner [0].properties.playerNumber) {
                        conditionMeet = false;
                    }
                }
            }
            if (conditionMeet) {
                winner = null;
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
                            if (winner.Contains (player)) {
                                ServerData.IncrementThisGameModeWon (accountName, this.properties.gameMode);
                            } else if (winner != null) {
                                ServerData.IncrementThisGameModeLost (accountName, this.properties.gameMode);
                            } else {
                                ServerData.IncrementThisGameModeDrawn (accountName, this.properties.gameMode);
                            }
                            ServerData.DecrementThisGameModeUnfinished (accountName, this.properties.gameMode);
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
                limit = properties.scoreLimit;
                break;
            case 2:
                limit = properties.turnLimit;
                break;
        }
        int experienceGain = 0;
        if (player != null) {
            PlayerPropertiesClass properties = player.properties;
            if (InputController.autoRunAI) {
                //Debug.Log ("wat");
                RatingClass.AnalyzeStatistics (this);
            }
            if (properties != null) {

                ClientInterface client = properties.client;
                if (client != null) {
                    string accountName = client.AccountName;
                    int matchType = this.properties.specialType;
                    switch (matchType) {
                        case 1:
                            if (winner.Contains (Player [1]) && !ServerData.GetUserFinishedPuzzle (accountName, this.properties.gameMode)) {
                                client.SavePuzzleResult (this.properties.gameMode);
                                experienceGain += 40;
                            }
                            break;
                        case 2:
                            if (winner.Contains (Player [1])){// && !ServerData.GetUserFinishedBoss (accountName, this.properties.gameMode)) {
                                client.SaveBossResult (this.properties.gameMode, Player);
                                experienceGain += 40;
                            }
                            break;
                        case 3:
                            if (winner.Contains (Player [1]) && !ServerData.GetUserFinishedTutorial (accountName, this.properties.gameMode)) {
                                client.SaveTutorialResult (this.properties.gameMode);
                                experienceGain += 40;
                            }
                            break;
                    }

                    if (winner.Contains (player)) {
                        experienceGain += turn * 2;
                    } else if (!properties.conceded) {
                        experienceGain += turn;
                    }
                    //

                    ServerLogic.AddExperience (client, accountName, experienceGain);
                    string [] winnersNames = new string [winner.Count];
                    for (int x = 0; x < winner.Count; x++) {
                        winnersNames [x] = winner [x].properties.displayName;
                    }
                    int level = ServerData.GetUserLevel (accountName);
                    int currentExperience = ServerData.GetUserExperience (accountName);
                    int maxExperience = ServerLogic.ExperienceNeededToLevelUp (level);
                    client.TargetShowMatchResult (client.connectionToClient, matchType, winnersNames, winCondition, limit, level, currentExperience, maxExperience, experienceGain);
                }
            }
        }
    }

    public void NewMatch (int gameMode, int matchType, int numberOfPlayers) {
        this.numberOfPlayers = numberOfPlayers;
        properties = new MatchPropertiesClass (gameMode);
        SetPlayers ();

        Board = new BoardClass (this);
        Board.LoadRandomFromGameMode (gameMode, matchType);
    }

    public void RotateAbilityArea (int playerNumber, int stackNumber) {
        Player [playerNumber].RotateTopCard (stackNumber);
    }

    public void MakeRandomMove (int playerNumber) {
        List<TileClass> tiles = Board.GetPlayableTiles (playerNumber);
        if (tiles.Count == 0) {
            return;
        }
        TileClass tile = tiles [Random.Range (0, tiles.Count)];
        PlayCard (tile.x, tile.y, turnOfPlayer, Random.Range (0, 4));
    }


    public void RunAI () {
        //Debug.Log (turnOfPlayer);
        ServerManagement.RunAIOnSeperateThread (this);
        //new Thread (RunAIOnThread).Start();
    }

    public void RunAIOnThread () {
        Vector3Int output = GetPlayer (turnOfPlayer).properties.AI.FindBestMove (this);
        int x = output.x;
        int y = output.y;
        //Debug.Log (x + " " + y + " " + Board.tile [x, y].enabled + " " + Board.tile [x, y].IsEmptyTile () + " ... " + Player [turnOfPlayer].GetTopCard (output.z).cardNumber);
        ServerManagement.PlayCardOnMainThread (this, output.x, output.y, turnOfPlayer, output.z);
    }

    public IEnumerator IEPlayCard (int x, int y, int playerNumber, int stackNumber) {

        PlayCard (x, y, playerNumber, stackNumber);
        yield return 0;
    }

    public void PlayCard (int x, int y, int playerNumber, int stackNumber) {
        if (InputController.debuggingEnabled) {
            Debug.Log ("Play card order recieved");
        }
        PlayerClass player = Player [playerNumber];
        CardClass card = player.GetTopCard (stackNumber);
        //Debug.Log ("Attempting to play card: " + card.tokenType + " , top card number " + player.GetStack (stackNumber).topCardNumber);
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
        if (!properties.usedCardsArePutOnBottomOfStack && stack != null && !stack.atLeast1Enabled) {
            return;
        }
        PlayCard (lastMoveId + 1, x, y, playerNumber, stackNumber, card.abilityType, card.abilityArea, card.tokenType, card.tokenValue);
        //UpdateBoard ();
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
            Debug.Log ("Checking conditions." + System.Environment.NewLine + 
                "Finished: " + finished +
                "Turn of player: " + turnOfPlayer + ", your number:" + playerNumber +
                "Tile enabled: " + tile.enabled +
                "Tile is empty: " + (tile.token == null).ToString ());
            if (tile.token != null) {
                Debug.Log ("Token value: " + tile.token.value);
                Debug.Log ("Token temp value: " + tile.token.tempValue);
                Debug.Log ("Token destoryed: " + tile.token.destroyed);
                Debug.Log ("Token owner: " + tile.token.owner);
                Debug.Log ("Token type: " + tile.token.type);
            }
        }
        if (!finished && turnOfPlayer == playerNumber && tile.IsPlayable (playerNumber)) {
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
                    //Debug.Log ("Test11");
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
        if (real) {
            for (int px = 0; px < Player.Length; px++) {
                if (GetPlayerProperties (px) != null && GetPlayerProperties (px) != null) {
                    //Debug.Log (px + " " + GetPlayerProperties (px).AI);
                }
            }
           // Debug.Log (finished + ", " + real + ", " + AI + ", " + turn + ",  " + turnOfPlayer);
        }
        if (!finished && real && AI != null) {
            //Debug.Log ("Test8");
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
        HandClass hand = player.hand;
        int topCardNumber = 0;
        if (hand != null) {
            StackClass stack = player.GetStack (stackNumber);
            topCardNumber = player.GetTopCardNumber (stackNumber);
        }
        SaveThisTurnMove (tile.x, tile.y, playerNumber, stackNumber, topCardNumber, card, token);
        player.MoveTopCard (stackNumber, topCardNumber, false, !properties.usedCardsArePutOnBottomOfStack);
        UseAbility (tile, playerNumber, stackNumber, topCardNumber, card.abilityArea, abilityType);
        SaveLastMove (tile.x, tile.y, playerNumber, stackNumber, topCardNumber, card, token);
        AILearning (abilityType);
        UpdateVisuals ();
        player.VisualMoveTopCard (stackNumber, topCardNumber, !properties.usedCardsArePutOnBottomOfStack);
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
            PlayerClass player = Player [move.playerNumber];
            if (player.hand == null) {
                return LastMove.usedCard;
            }
            return Player [move.playerNumber].GetCard (move.stackNumber, move.usedCardNumber);
        }
    }

    public void UpdateBoard () {
        if (!updateBoard) {
            return;
        }
        if (visualMatch != null) {
            VisualMatch.GlobalTimer += AppSettings.GetAnimationsDuration ();
        }
        updateBoard = false;
        foreach (TileClass tile in Board.tileList) {
            tile.UpdateTempValue ();
            tile.Update ();
        }
        if (visualMatch) {
            SoundManager.PlayQueuedAudioClips ();
        }
        UpdateBoard ();
    }

    public VectorInfo GetVectorInfo (int x, int y, int playerNumber, int abilityArea, int abilityType, TokenClass token) {
        return GetVectorInfo (Board.tile [x, y], playerNumber, abilityArea, abilityType, token);
    }

    public VectorInfo GetVectorInfo (TileClass tile, int playerNumber, int abilityArea, int abilityType, TokenClass token) {
        List <AbilityVector> vectors = new List <AbilityVector> (Board.GetAbilityVectors (tile.x, tile.y, abilityArea));
        if (tile != null) {
            //Debug.Log (Board.BeforeAbilityTriggers.Count);
        }
        foreach (TokenClass triggeredToken in Board.BeforeAbilityTriggers) {
            switch (triggeredToken.type) {
                case 18:
                    bool exists = false;
                    foreach (AbilityVector vector in vectors) {
                        if (vector.x == triggeredToken.x && vector.y == triggeredToken.y) {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists) {
                        vectors.Add (new AbilityVector (triggeredToken.tile));
                    }
                    break;
            }
        }
        VectorInfo info = new VectorInfo (vectors.ToArray (), token, GetTeam (token.owner));
        info.CheckAbilityTriggers (this, playerNumber, tile, abilityType, token);
        return info;
    }

    public void TokenLeavesTile (TokenClass token) {
        switch (token.type) {
        }

    }

    public void DestroyToken (TokenClass token, int x, int y) {
        /*if (!real) {
            Debug.Log (real + " Destroy token");
        }*/
        TokenLeavesTile (token);
        TileClass tile = Board.GetTile (x, y);
        VectorInfo VI = GetTokenVectorInfo (tile, token);
        int tokenType = token.type;
        int tokenOwner = token.owner;
        PlayerPropertiesClass hisProperties = GetPlayerProperties (tokenOwner);
        int tokenTeam = 0;
        if (hisProperties != null) {
            tokenOwner = hisProperties.team;
        }
        switch (tokenType) {
            case 5:
                if (visualMatch != null) {
                    visualMatch.CreateRealTokenEffect (tile, tokenType);
                    if (visualMatch) {
                        SoundManager.AddAudioClipToQueue (MyAudioClip.TokenNegativeExplosion);
                    }
                }
                break;
            case 12:
            case 21:
                if (visualMatch != null) {
                    visualMatch.CreateRealTokenEffect (tile, tokenType);
                    if (visualMatch) {
                        SoundManager.AddAudioClipToQueue (MyAudioClip.TokenNegativeAbility);
                    }
                }
                Player [token.owner].score -= 20;
                break;
        }
        token.RemoveType ();
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
        for (int p = 1; p < Player.Length; p++) {
            PlayerPropertiesClass properties = GetPlayerProperties (p);
            if (properties == null || properties.specialStatus != 9 || properties.team == tokenTeam) {
                continue;
            }
            PlayerClass player = GetPlayer (p);
            if (player.hand == null) {
                continue;
            }
            foreach (StackClass stack in player.hand.stack) {
                foreach (CardClass card in stack.card) {
                    ModifyCardValue (card, 1);
                }
            }
        }
    }

    public VectorInfo GetTokenVectorInfo (TileClass tile, TokenClass token) {
        AbilityVector [] vectors = Board.GetAbilityVectors (tile.x, tile.y, 4);
        VectorInfo info = new VectorInfo (vectors, token, GetTeam (token.owner));
        info.CheckTokenTriggers (this, tile, token);
        return info;
    }

    public void UseAbility (TileClass tile, int playerNumber, int stackNumber, int cardNumber, int abilityArea, int abilityType) {
        Vector2Int vector = VerifyAbilityType (tile, playerNumber, abilityType);
        abilityType = vector.x;
        int castCount = vector.y;
        for (int x = 0; x < castCount; x++) {
            if (tile.token == null) {
                return;
            }
            VectorInfo info = GetVectorInfo (tile, playerNumber, abilityArea, abilityType, tile.token);
            if (visualMatch != null) {
                VisualEffectInterface.DelayedCreateRealEffects (info, abilityType);
            }
            int tokenType = tile.token.type;
            UseAbilityTrigger1 (info, tile, playerNumber, abilityType, tokenType);
            UseAbilityTrigger2 (info, tile, playerNumber, abilityType, tokenType);
            UseAbilityVector (info, tile, playerNumber, abilityType, tokenType);
            UseAbilityConstant (info, tile, playerNumber, stackNumber, cardNumber, abilityType, tokenType);
            if (visualMatch) {
                if (real) {
                    SoundManager.PlayAbilityAudioClips (abilityType);
                }
            }
            updateBoard = true;
            UpdateBoard ();
        }
        if (visualMatch) {
            VisualMatch.GlobalTimer += AppSettings.GetAnimationsDuration ();
        }
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

    public Vector2Int VerifyAbilityType (TileClass tile, int playerNumber, int abilityType) {
        int newAbilityType = abilityType;
        int castCount = 1;
        if (Player [playerNumber].properties.specialStatus == 2) {
            castCount ++;
        }
        List<TokenClass> tempList = new List<TokenClass> ();
        foreach (TokenClass triggeredToken in Board.BeforeAbilityTriggers) {
            tempList.Add (triggeredToken);
        }
        foreach (TokenClass triggeredToken in tempList) {
                if (triggeredToken != null) {
                    int triggeredType = triggeredToken.type;
                    switch (triggeredType) {
                        case 9:
                            if (abilityType != 0 && tile != triggeredToken.tile) {
                                castCount--;
                                ChangeType (triggeredToken.tile, 0);
                                updateBoard = true;
                            if (visualMatch) {
                                SoundManager.AddAudioClipToQueue (MyAudioClip.TokenTrigger);
                            }
                        }
                            break;
                        case 15:
                            if (abilityType != 0) {
                                castCount++;
                                ChangeType (triggeredToken.tile, 0);
                                updateBoard = true;
                            if (visualMatch) {
                                SoundManager.AddAudioClipToQueue (MyAudioClip.TokenTrigger);
                            }
                        }
                            break;

                    }
                    switch (triggeredType) {
                        case 9:
                        case 15:
                        if (abilityType != 0 && visualMatch != null && (triggeredType != 9 || tile != triggeredToken.tile)) {
                                visualMatch.CreateRealTokenEffect (triggeredToken.tile, triggeredType);
                            }
                            break;
                    }
                } else {
                    //Debug.Log ("Wut");
                }
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
        }
        return new Vector2Int (abilityType, castCount);
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
                case 66:
                    SwapToken (tile, target);
                    break;
                case 21:
                case 29:
                case 30:
                case 35:
                case 63:
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
                case 32:
                    ModifyTempValue (tile, 1);
                    break;
                case 39:
                    CreateToken (target, info.Triggered2 [0].token.type, 1, playerNumber);
                    break;
                case 40:
                    ChangeType (tile, 0);
                    break;
                case 41:
                    ModifyTempValue (target, -3);
                    break;
                case 43:
                    ModifyTempValue (target, 1);
                    break;
                case 44:
                    ModifyTempValue (target, -2);
                    break;
                case 47:
                    SetDestroy (target);
                    Player [playerNumber].AddScore (10);
                    break;
                case 48:
                    Player [playerNumber].AddScore (-20);
                    CreateToken (target, 0, 1, playerNumber);
                    break;
                case 50:
                    SetOwner (tile, target.token.owner);
                    break;
                case 52:
                    ModifyTempValue (target, 2);
                    break;
                case 53:
                    CreateToken (target, tile.token.type, 1, playerNumber);
                    break;
                case 54:
                    ModifyTempValue (target, 1);
                    ModifyTempValue (tile, 1);
                    break;
                case 55:
                    ModifyTempValue (tile, target.token.value - 1);
                    ModifyTempValue (target, 1 - target.token.value);
                    break;
                case 57:
                    ModifyTempValue (target, info.Triggered2 [0].token.value - target.token.value);
                    break;
                case 58:
                    ModifyTempValue (target, - (target.token.value) / 2);
                    ChangeType (target, tile.token.type);
                    break;
                case 59:
                    SetDestroy (target);
                    DisableTile (target);
                    break;
                case 60:
                    DisableTile (target);
                    break;
                case 64:
                    SetOwner (target, playerNumber);
                    break;
                case 65:
                    ModifyTempValue (tile, target.token.value);
                    ModifyTempValue (target, - target.token.value);
                    break;
                case 67:
                    SetDestroy (target);
                    break;
                case 68:
                    ModifyTempValue (target, target.token.value);
                    break;
                case 69:
                    SetDestroy (target);
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
                    ModifyTempValue (target, -1);
                    break;
                case 56:
                    ModifyTempValue (info.Triggered1 [0], target.token.value);
                    SetDestroy (target);
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
                if (info.triggeredPlayers != null) {
                    foreach (int pNumber in info.triggeredPlayers) {
                        PlayerClass player = GetPlayer (pNumber);
                        if (player == null) {
                            continue;
                        }
                        HandClass hand = player.hand;
                        //Debug.Log ("Player: " + pNumber);
                        if (hand != null) {
                            for (int y = 0; y < hand.stack.Length; y++) {
                                //Debug.Log (hand.stack [y].topCardNumber);
                                player.MoveTopCard (y);
                                //Debug.Log (hand.stack [y].topCardNumber);
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
            case 42: {
                PlayerClass player = Player [playerNumber];
                if (player.hand == null) {
                    break;
                }
                CardClass card = player.GetCard (stackNumber, cardNumber);
                SetCardValue (card, 1);
            }
            break;
            case 46:
                if (!properties.turnWinCondition) {
                    break;
                }
                if (info.triggeredPlayers != null) {
                    foreach (int pNumber in info.triggeredPlayers) {
                        if (Player.Length <= pNumber) {
                            continue;
                        }
                        PlayerClass player = Player [pNumber];
                        if (player == null) {
                            continue;
                        }
                        player.AddScore (-TurnsLeft ());
                    }
                }
            break;
            case 49: {
                if (LastMove == null) {
                    break;
                }
                int player = LastMove.playerNumber;
                if (Player [player].hand == null) {
                    break;
                }
                CardClass card = GetLastPlayedCard ();
                if (card == null) {
                    break;
                }
                Player [player].AIValue += AIClass.AproxTokenValue (card.tokenType, card.tokenValue - 2) - AIClass.AproxTokenValue (card.tokenType, card.tokenValue);
                ModifyCardValue (card, -2);

            }
            break;
            case 51:
                if (info.triggeredPlayers != null) {
                    AIClass AI = Player [playerNumber].properties.AI;
                    foreach (int pNumber in info.triggeredPlayers) {
                        if (Player.Length <= pNumber) {
                            continue;
                        }
                        PlayerClass player = Player [pNumber];
                        if (player == null) {
                            continue;
                        }
                        if (AI != null) {
                            player.AIValue -= 4.1f * AI.turnsLeft / properties.turnLimit;
                        }
                        HandClass hand = player.hand;
                        if (hand != null) {
                            for (int y = 0; y < hand.stack.Length; y++) {
                                ModifyCardValue (player.GetTopCard(y), -1);
                            }
                        }
                    }
                }
                break;
            case 53:
                SetDestroy (tile);
                break;
            case 62:
                if (LastMove != null) {
                    Player [LastMove.playerNumber].MoveCardToTheTop (LastMove.stackNumber, LastMove.usedCardNumber);
                }
                break;
            case 70:
                if (info.triggeredPlayers != null) {
                    foreach (int pNumber in info.triggeredPlayers) {
                        PlayerClass targetPlayer = GetPlayer (pNumber);
                        PlayerClass player = GetPlayer (playerNumber);
                        if (targetPlayer != null && targetPlayer.score >= player.score + 20) {
                            targetPlayer.AddScore (-20);
                        }
                    }
                }
                break;
            case 71: 
                {
                PlayerClass player = GetPlayer (playerNumber);
                HandClass hand = player.hand;
                if (hand != null) {
                    for (int y = 0; y < hand.stack.Length; y++) {
                        player.RotateTopCard (y, info.tokenCount);
                    }
                }
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
        if (visualMatch != null) {
            switch (abilityType) {
                case 38:
                case 42:
                case 62:
                    VisualEffectInterface.DelayedRealEffect (tile.x, tile.y, abilityType, true);
                    break;
            }
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
        if (tile == null) {
            return;
        }
        TokenClass token = tile.token;
        PlayerClass player = Player [token.owner];
        if (token != null && (player == null || player.properties == null || player.properties.specialStatus != 3)) {
            updateBoard = true;
            tile.token.ChangeType (newType);
        }
    }

    public void SetType (TileClass tile, int newType) {
        updateBoard = true;
        tile.token.SetType (newType);
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
            PlayerClass player = GetPlayer (target.owner);
            if (target != null && (player == null || player.properties == null || player.properties.specialStatus != 3)) {
                updateBoard = true;
                target.ModifyTempValue (value);
            }
        }
    }

    public void SetOwner (TileClass tile, int owner) {
        if (tile != null) {
            TokenClass target = tile.token;
            if (target != null) {
                updateBoard = true;
                target.SetOwner (owner);
            }
        }
    }

    public void SetDestroy (TileClass tile) {
        if (tile != null) {
            TokenClass target = tile.token;
            if (target != null) {
                updateBoard = true;
                target.destroyed = true;
            }
        }
    }

    public TokenClass PlayToken (TileClass tile, CardClass card, int playerNumber) {
        if (tile != null) {
            //Debug.Log ("Played card: " + card.tokenType);
            //Debug.Log (Board.NumberOfTypes [7]);
            int sumOfValues = card.tokenValue + Board.NumberOfTypes [7] - Board.NumberOfTypes [11];
            TokenClass token = CreateToken (tile, card.tokenType, sumOfValues, playerNumber);
            //TokenClass token = tile.CreateToken (card, playerNumber);
            if (tile.visualTile != null) {
                tile.token.visualToken.DelayedAddPlayAnimation ();
            }
            foreach (TokenClass triggeredToken in Board.BeforeTokenPlayedTriggers) {
                if (triggeredToken != null) {
                    int triggeredType = triggeredToken.type;
                    switch (triggeredType) {
                        case 14:
                            if (triggeredToken.value < token.value && RelationLogic.IsEnemyTeam (GetTeam (triggeredToken.tile), GetTeam (token.owner))) {
                                ModifyTempValue (triggeredToken.tile, 1);

                                SoundManager.AddAudioClipToQueue (MyAudioClip.TokenPositiveRegularAbility);
                            }
                            break;

                    }
                }
            }
            for (int x = 1; x < numberOfPlayers; x++) {
                PlayerClass player = Player [x];
                if (player != null) {
                    HandClass hand = player.hand;
                    if (hand != null) {
                        foreach (StackClass stack in hand.stack) {
                            CardClass tempCard = stack.getTopCard ();
                            switch (tempCard.abilityType) {
                                case 45:
                                    if (tempCard.tokenValue < token.value && RelationLogic.IsEnemyTeam (GetTeam (x), GetTeam (token.owner))) {
                                        ModifyCardValue (tempCard, 1);
                                    }
                                    break;
                            }
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
            updateBoard = true;
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

    public PlayerClass GetPlayer (int playerNumber) {
        if (playerNumber < 0 || playerNumber >= Player.Length) {
            return null;
        }
        return Player [playerNumber];
    }

    public PlayerPropertiesClass GetPlayerProperties (int playerNumber) {
        PlayerClass player = GetPlayer (playerNumber);
        if (player == null) {
            return null;
        }
        return player.properties;
    }

    void SetPlayers () {
        Player = new PlayerClass [numberOfPlayers + 1];
    }

    public void SetPlayer (int PlayerNumber, PlayerClass player) {
        if (player == null) {
            return;
        }
        MatchClass currentlyPlayedMatch = MatchMakingClass.FindMatch (player.properties.accountName);
        if (currentlyPlayedMatch != null && !InputController.autoRunAI) {
            currentlyPlayedMatch.Concede (player.properties.accountName);
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
