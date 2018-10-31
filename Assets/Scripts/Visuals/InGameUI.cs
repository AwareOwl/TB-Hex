using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : GOUI {

    static public InGameUI instance;

    static public MatchClass PlayedMatch;

    static public int MyPlayerNumber = 1;

    static public int SelectedStack;

    static public int NumberOfPlayers = 2;

    static public PlayerClass GetPlayer (int number) {
        return PlayedMatch.Player [number];
    }

    private void Start () {
        instance = this;
        CurrentGUI = this;

        CreatePlayersUI ();
        PlayedMatch.Board.EnableVisualisation ();
    }

    public void Update () {
        for (int x = 1; x <= 4; x++) {
            if (Input.GetKeyDown (x.ToString())) {
                SelectedStack = x - 1;
                //PlayedMatch.MoveTopCard (MyPlayerNumber, x - 1);
            }
        }
    }

    static public void TileAction (int x, int y) {
        PlayedMatch.PlayCard (MyPlayerNumber, SelectedStack, x, y);
    }

    static public void ShowInGameUI (MatchClass playedMatch) {
        DestroyMenu ();
        PlayedMatch = playedMatch;
        CurrentCanvas.AddComponent<InGameUI> ();
    }
    
    static public void CreatePlayersUI () {
        for (int x = 0; x < NumberOfPlayers; x++) {
            PlayerClass player = GetPlayer (x + 1);
            bool ally = player.playerNumber == GetPlayer (MyPlayerNumber).playerNumber;
            player.EnableVisuals ();
            player.visualPlayer.CreatePlayerUI (player, ally, NumberOfPlayers);
            player.visualPlayer.SetPlayerHealthBar (PlayedMatch, player);
        }
    }

    public void SetPlayerHealthBar (int playerNumber) {
        PlayerClass player = GetPlayer (playerNumber);

        //SetPlayerHealthBar (playerNumber, player.score, player.scoreIncome, PlayedMatch.Properties.scoreLimit);
    }
}
