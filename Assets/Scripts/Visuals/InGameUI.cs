using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : GOUI {

    static public GameObject [,] VisualEffectAnchor;

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
        PlayedMatch.PlayCard (x, y, MyPlayerNumber, SelectedStack);
    }

    static public void ShowInGameUI (MatchClass playedMatch) {
        DestroyMenu ();
        PlayedMatch = playedMatch;
        CurrentCanvas.AddComponent<InGameUI> ();
    }

    static public PlayerClass GetPlayer () {
        return PlayedMatch.Player [MyPlayerNumber];
    }

    static public CardClass GetSelectedCard () {
        return GetPlayer ().Hand.Stack [SelectedStack].TopCard ();
    }
    
    static public void CreatePlayersUI () {
        for (int x = 0; x < NumberOfPlayers; x++) {
            PlayerClass player = GetPlayer (x + 1);
            bool ally = player.playerNumber == GetPlayer (MyPlayerNumber).playerNumber;
            player.EnableVisuals ();
            player.visualPlayer.CreatePlayerUI (player, ally, NumberOfPlayers);
            player.visualPlayer.SetPlayerHealthBar (PlayedMatch, player);
        }
        int sx = PlayedMatch.Board.tile.GetLength (0);
        int sy = PlayedMatch.Board.tile.GetLength (1);
        VisualEffectAnchor = new GameObject [sx, sy];
        for (int x = 0; x < sx; x++) {
            for (int y = 0; y < sy; y++) {
                VisualEffectAnchor [x, y] = new GameObject ();
                VisualEffectAnchor [x, y].transform.localPosition = VisualTile.TilePosition (x, 0.2f, y);
            }
        }
    }

    public void SetPlayerHealthBar (int playerNumber) {
        PlayerClass player = GetPlayer (playerNumber);

        //SetPlayerHealthBar (playerNumber, player.score, player.scoreIncome, PlayedMatch.Properties.scoreLimit);
    }

    static public void SetAreaHovers (int x, int y) {
        foreach (GameObject anchor in VisualEffectAnchor) {
            Transform [] childs = new Transform [anchor.transform.childCount];
            for (int c = 0; c < childs.Length; c++) {
                childs [c] = anchor.transform.GetChild (c);
            }
            for (int c = 0; c < childs.Length; c++) {
                DestroyImmediate (childs [c].gameObject);
            }
        }
        List<AbilityVector> list = PlayedMatch.Board.GetAbilityVectors (x, y, GetSelectedCard ().abilityArea);
        foreach (AbilityVector vector in list) {
            if (PlayedMatch.Board.IsTileEnabled (vector.x, vector.y)) {
                VisualEffectInterface.CreateEffect (VisualEffectAnchor [vector.x, vector.y], GetSelectedCard ().abilityType, true, false);
            }
        }
    }
}
