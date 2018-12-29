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
            this.Player [x] = new PlayerClass (match.Player [x]);
        }
        this.LastMove = match.LastMove;
        this.Properties = match.Properties;
        this.prevMatch = match.prevMatch;
    }

    public void Concede (int playerNumber) {
        Player [playerNumber].properties.conceded = true;
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
        winCondition = 4;
        if (visualMatch != null) {
            visualMatch.ShowMatchResult (winner.displayName, winCondition, 0);
        }
    }

    public void EndTurn () {
        int [] playerIncome = new int [Player.Length];

        foreach (TileClass tile in Board.tileList) {
            TokenClass token = tile.token;
            if (token != null) {
                int value = token.value;
                switch (token.type) {
                    case 1:
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
            }
        }
        for (int x = 0; x < Player.Length; x++) {
            PlayerClass player = Player [x];
            player.SetScoreIncome (playerIncome [x]);
            player.AddScore (playerIncome [x]);
            player.UpdateVisuals (this);
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

        turnOfPlayer = Mathf.Max (1, (turnOfPlayer + 1) % (numberOfPlayers + 1));
        turn++;
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
        for (int x = 1; x <= numberOfPlayers; x++) {
            if (Player [x].score >= Properties.scoreLimit) {
                FinishGame (1, Properties.scoreLimit);
                return;
            }
        }
        if (turn >= Properties.turnLimit) {
            FinishGame (2, Properties.turnLimit);
            return;
        }
        if (Board.GetEmptyTiles ().Count == 0) {
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
            } else {
                if (winner.score < Player [x].score) {
                    winner = Player [x];
                } else if (winner.score == Player [x].score) {
                    winner = null;
                }
            }
        }
        if (real) {
            if (visualMatch != null) {
                visualMatch.ShowMatchResult (winner.properties.displayName, winCondition, limit);
            }
            /*for (int x = 1; x <= numberOfPlayers; x++) {
                ClientInterface client = Player [x].properties.client;
                if (client != null) {
                }
            }*/
            if (InputController.autoRunAI) {
                RatingClass.AnalyzeStatistics (this);
            }
            //Debug.Log ("GameFinished: " + winner.playerNumber + " " + Player [1].score + " " + Player [2].score + " " + turn);
        }
    }

    public void NewMatch (int gameMode, int numberOfPlayers) {
        this.numberOfPlayers = numberOfPlayers;
        Properties = new MatchPropertiesClass ();
        SetPlayers ();

        Board = new BoardClass (this);
        Board.LoadRandomFromGameMode (gameMode);
    }

    public void MoveTopCard (int playerNumber, int stackNumber) {
        Player [playerNumber].MoveTopCard (stackNumber);
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
        PlayCard (lastMoveId + 1, x, y, playerNumber, stackNumber, card.abilityType, card.abilityArea, card.tokenType, card.value);
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
            Debug.Log ("Tile is empty: " + (tile.token == null).ToString());
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
        UseAbility (tile, playerNumber, card.abilityArea, card.abilityType);
        SaveLastMove (tile.x, tile.y, playerNumber, stackNumber, card, token);
        updateBoard = true;
        UpdateBoard ();
        UpdateVisuals ();
        player.MoveTopCard (stackNumber);
        EndTurn ();
    }

    public void VisualPlayCard (int playerNumber, CardClass card) {
        if (visualMatch != null) {
            visualMatch.PlayCard (playerNumber, card);
        }
    }

    public void SaveLastMove (int x, int y, int playerNumber, int stackNumber, CardClass playedCard, TokenClass token) {
        LastMove = new MoveHistoryClass (lastMoveId + 1, x, y, playerNumber, stackNumber, playedCard, token, LastMove);
        Player [playerNumber].LastMove = new MoveHistoryClass (lastMoveId + 1, x, y, playerNumber, stackNumber, playedCard, token, Player [playerNumber].LastMove);
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

    public void DestroyToken (TokenClass token) {
        VectorInfo VI = GetTokenVectorInfo (token.tile, token);
        switch (token.type) {
            case 5:
                if (visualMatch != null) {
                    visualMatch.CreateRealTokenEffect (token.tile, token.type);
                }
                break;
        }
        foreach (TileClass target in VI.Triggered1) {
            switch (token.type) {
                case 5:
                    if (visualMatch != null) {
                        visualMatch.CreateRealTokenVectorEffect (token.tile, target, token.type);
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

    public void UseAbility (TileClass tile, int playerNumber, int abilityArea, int abilityType) {
        if (abilityType == 7) {
            if (visualMatch != null) {
                bool triggered = LastMove != null && LastMove.usedCard.abilityType != abilityType;
                VisualEffectInterface.DelayedRealEffect (tile.x, tile.y, abilityType, triggered);
                if (triggered) {
                    abilityType = LastMove.usedCard.abilityType;
                }
            }
        }
        VectorInfo info = GetVectorInfo (tile, abilityArea, abilityType, tile.token);
        if (visualMatch != null) {
            VisualEffectInterface.DelayedCreateRealEffects (info, abilityType);
            VisualMatch.GlobalTimer += 0.5f;
        }
        int tokenType = tile.token.type;
        foreach (TileClass target in info.Triggered1) {
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
                    target.token.ChangeType (0);
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
                    target.token.ChangeType (tokenType);
                    break;
                case 17:
                    tile.token.ChangeType (target.token.type);
                    target.token.ChangeType (tokenType);
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
                    ModifyTempValue (info.Triggered2[0], -1);
                    break;
            }
        }
        foreach (TileClass target in info.Triggered2) {
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
                    target.token.ChangeType (tokenType);
                    break;
            }
        }
        foreach (AbilityVector vector in info.TriggeredVector) {
            switch (abilityType) {
                case 5:
                    MoveToken (vector.target, vector.pushX, vector.pushY);
                    break;
            }
        }
        switch (abilityType) {
            case 20:
                foreach (int pNumber in info.TargetPlayers) {
                    PlayerClass player = Player [pNumber];
                    HandClass hand = player.GetHand ();
                    if (hand != null) {
                        for (int y = 0; y < hand.stack.Length; y++) {
                            player.MoveTopCard (y);
                        }
                    }
                }
                break;
        }
    }

    public TileClass LastPlayedTile () {
        if (LastMove == null) {
            return null;
        }
        TokenClass token = LastMove.playedToken;
        if (token != null) {
            return Board.tile [token.x, token.y];
        }
        return null;
    }

    public TokenClass LastPlayedToken () {
        if (LastMove == null) {
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
            TokenClass token = CreateToken (tile, card.tokenType, card.value + Board.NumberOfTypes [7], playerNumber);
            //TokenClass token = tile.CreateToken (card, playerNumber);
            if (tile.visualTile != null) {
                tile.token.visualToken.DelayedAddPlayAnimation ();
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
                    sourceTile.AttachToken (destinationToken);
                    destinationTile.AttachToken (sourceToken);
                }
            }
        }
    }

    public void DestroyToken (TileClass tile) {
        if (tile != null && tile.token != null) {
            tile.token.DestroyToken ();
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
            Player [x].EnableVisuals ();
        }
    }

    public void DestroyVisuals () {
        foreach (TileClass tile in Board.tile) {
            if (tile.visualTile != null) {
                tile.DestroyVisual ();
            }
        }
        foreach (PlayerClass player in Player) {
            player.DestroyVisuals ();
        }
        if (visualMatch != null) {
            visualMatch.DestroyVisuals ();
        }
    }

}
