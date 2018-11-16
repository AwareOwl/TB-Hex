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
    public bool real = true;

    public MoveHistoryClass LastMove;

    public MatchPropertiesClass Properties;

    public MatchClass prevMatch;


    public MatchClass () {

    }

    public MatchClass (MatchClass match) {
        this.Board = new BoardClass (match.Board);
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

    public void EndTurn () {
        int [] playerIncome = new int [Player.Length];

        foreach (TileClass tile in Board.tileList) {
            TokenClass token = tile.token;
            if (token != null) {
                int value = token.value;
                if (token.type == 1) {
                    value *= 2;
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

        turnOfPlayer = Mathf.Max (1, (turnOfPlayer + 1) % (numberOfPlayers + 1));
        turn++;
        CheckFinishCondition ();
        PlayerClass nextPlayer = Player [turnOfPlayer];
        AIClass AI = nextPlayer.properties.AI;
        if (!finished && real && AI != null) {
            RunAI ();
        }

        //AIClass AI = Player [1].properties.AI;
       // Debug.Log (AI.TurnToWinPredict (this, 1) + " " + AI.TurnToWinPredict (this, 2) + " " + AI.CalculateMatchValue (this, 1) + " " + AI.CalculateMatchValue (this, 2));
    }

    public void CheckFinishCondition () {
        for (int x = 1; x <= numberOfPlayers; x++) {
            if (Player [x].score >= Properties.scoreLimit) {
                FinishGame ();
                return;
            }
        }
        if (turn >= Properties.turnLimit) {
            FinishGame ();
            return;
        }
        if (Board.GetEmptyTiles ().Count == 0) {
            FinishGame ();
            return;
        }
    }

    public void FinishGame () {
        finished = true;
        for (int x = 1; x <= numberOfPlayers; x++) {
            if (winner == null || winner.score < Player [x].score) {
                winner = Player [x];
            }
        }
        if (real) {
            RatingClass.AnalyzeStatistics (this);
            //Debug.Log ("GameFinished: " + winner.playerNumber + " " + Player [1].score + " " + Player [2].score + " " + turn);
        }
    }

    public void NewMatch (int numberOfPlayers) {
        this.numberOfPlayers = numberOfPlayers;
        Properties = new MatchPropertiesClass ();
        SetPlayers ();

        Board = new BoardClass ();
        Board.LoadFromFile (Random.Range (1, 5));
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
        TileClass tile = Board.tile [x, y];
        if (!finished && turnOfPlayer == playerNumber && tile.enabled && tile.token == null) {
            PlayerClass player = Player [playerNumber];
            CardClass card = player.GetTopCard (stackNumber);
            PlayCard (x, y, playerNumber, stackNumber, card);
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
        SaveLastMove (tile.x, tile.y, card, token, playerNumber);
        UpdateBoard ();
        player.MoveTopCard (stackNumber);
        EndTurn ();
    }

    public void SaveLastMove (int x, int y, CardClass playedCard, TokenClass token, int playerNumber) {
        LastMove = new MoveHistoryClass (x, y, playedCard, token, LastMove);
        Player [playerNumber].LastMove = new MoveHistoryClass (x, y, playedCard, token, Player [playerNumber].LastMove);
    }

    public void UpdateBoard () {
        foreach (TileClass tile in Board.tileList) {
            tile.Update ();
        }
    }

    public VectorInfo GetVectorInfo (int x, int y, int playerNumber, int abilityArea, int abilityType) {
        return GetVectorInfo (Board.tile [x, y], abilityArea, abilityType);
    }

    public VectorInfo GetVectorInfo (TileClass tile, int abilityArea, int abilityType) {
        AbilityVector [] vectors = Board.GetAbilityVectors (tile.x, tile.y, abilityArea).ToArray ();
        VectorInfo info = new VectorInfo (vectors);
        info.CheckTriggers (this, tile, abilityType);
        return info;
    }

    public void UseAbility (TileClass tile, int playerNumber, int abilityArea, int abilityType) {
        if (abilityType == 7) {
            if (LastMove != null && LastMove.usedCard.abilityType != abilityType) {
                abilityType = LastMove.usedCard.abilityType;
            }
        }
        AbilityVector [] vectors = Board.GetAbilityVectors (tile.x, tile.y, abilityArea).ToArray ();
        VectorInfo info = new VectorInfo (vectors);
        info.CheckTriggers (this, tile, abilityType);
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
                    target.token.SetType (0);
                    break;
                case 11:
                    SwapToken (tile, target);
                    break;
                case 12:
                    ModifyTempValue (target, -4);
                    break;
                case 13:
                    ModifyTempValue (target, tile.token.value - target.token.value);
                    break;
            }
        }
        foreach (TileClass target in info.Triggered2) {
            switch (abilityType) {
                case 2:
                    CreateToken (target, 0, 1, playerNumber);
                    break;
            }
        }
        foreach (AbilityVector vector in info.TriggeredVector) {
            switch (abilityType) {
                case 5:
                    MoveToken (vector.target, vector.pushTarget);
                    break;
            }
        }

        /*foreach (AbilityVector vector in vectors) {
            switch (abilityType) {
                case 1:
                    ModifyTempValue (vector.target, -1);
                    break;
                case 2:
                    ModifyTempValue (vector.target, 1);
                    CreateToken (vector.target, 0, 1, playerNumber);
                    break;
                case 3:
                    DisableEmptyTile (vector.target);
                    break;
                case 4:
                    Player [playerNumber].AddScore (GetTokenValue (vector.target));
                    break;
                case 5:
                    MoveToken (vector.target, vector.pushTarget);
                    break;
                case 6:
                    if (vector.target != null && vector.target.IsEmptyTile ()) {
                        ModifyTempValue (tile, -1);
                    }
                    break;
                case 8:
                    if (vector.target != null && vector.target.IsFilledTile () && vector.target.token.owner != playerNumber && LastPlayedToken () != null) {
                        ModifyTempValue (LastPlayedToken ().tile, -1);
                    }
                    break;
                case 10:
                    if (vector.target != null && vector.target.IsFilledTile ()) {
                        vector.target.token.SetType (0);
                    }
                    break;
                case 13:
                    if (vector.target != null && vector.target.IsFilledTile ()) {
                        ModifyTempValue (vector.target, tile.token.value - vector.target.token.value);
                    }
                    break;
            }
        }
        switch (abilityType) {
            case 9:
                if (info.WeakestTargets.Count == 1) {
                    ModifyTempValue (info.WeakestTargets [0].target, 2);
                }
                break;
            case 11:
                SwapToken (tile, LastPlayedTile ());
                break;
            case 12:
                if (info.StrongestTargets.Count == 1) {
                    ModifyTempValue (info.StrongestTargets [0].target, -4);
                }
                break;

        }*/
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
                target.ModifyTempValue (value);
            }
        }
    }

    public TokenClass PlayToken (TileClass tile, CardClass card, int playerNumber) {
        if (tile != null) {
            TokenClass token = CreateToken (tile, card, playerNumber);
            if (tile.visualTile != null) {
                tile.token.visualToken.AddPlayAnimation ();
            }
            return token;
        }
        return null;
    }

    public TokenClass CreateToken (TileClass tile, CardClass card, int playerNumber) {
        if (tile != null) {
            return tile.CreateToken (card, playerNumber);
        }
        return null;
    }

    public TokenClass CreateToken (TileClass tile, int type, int value, int playerNumber) {
        if (tile != null) {
            return tile.CreateToken (type, value, playerNumber);
        }
        return null;
    }

    public TokenClass CreateToken (TileClass tile, TokenClass token) {
        if (tile != null) {
            return tile.CreateToken (token);
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

    public void MoveToken (TileClass sourceTile, TileClass destinationTile) {
        if (sourceTile != null && sourceTile.token != null) {
            TokenClass sourceToken = sourceTile.token;
            if (destinationTile != null && destinationTile.enabled) {
                if (destinationTile.token == null) {
                    sourceTile.AttachToken (null);
                    destinationTile.AttachToken (sourceToken);
                }
            } else {
                DestroyToken (sourceTile);
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

    void LoadBoard () {
        Board = new BoardClass ();
        Board.LoadFromFile (1);
    }

    void SetPlayers () {
        Player = new PlayerClass [numberOfPlayers + 1];
        Player [0] = new PlayerClass ();
    }

    public void SetPlayer (int PlayerNumber, PlayerClass player) {
        Player [PlayerNumber] = player;
        player.playerNumber = PlayerNumber;
        AIClass AI = player.properties.AI;
        if (AI != null) {
            AI.MaxEmptyTileCount = Board.GetEmptyTilesCount ();
        }
    }

    public void EnableVisuals () {
        for (int x = 1; x < Player.Length; x++) {
            Player [x].EnableVisuals ();
        }
    }
    
}
