﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : GOUI {

    static public GameObject [,] VisualEffectAnchor;

    static public InGameUI instance;

    static public MatchClass PlayedMatch;

    static public int MyPlayerNumber = 1;

    static public int SelectedStack;

    static public int NumberOfPlayers = 2;

    static public void DestroyVisuals () {
        if (VisualEffectAnchor != null) {
            for (int x = 0; x < VisualEffectAnchor.GetLength (0); x++) {

            }
            foreach (GameObject obj in VisualEffectAnchor) {
                if (obj != null) {
                    Debug.Log ("Test");
                        DestroyImmediate (obj);
                    }
            }
        }

        if (PlayedMatch != null) {
            foreach (TileClass tile in PlayedMatch.Board.tile) {
                if (tile.visualTile != null) {
                    tile.visualTile.DestroyVisual ();
                }
            }
            foreach (PlayerClass player in PlayedMatch.Player) {
                //DestroyImmediate (player.visualPlayer);
                if (player.properties != null) {
                    foreach (StackClass stack in player.GetHand ().stack) {
                        foreach (CardClass card in stack.card) {
                            if (card.visualCard != null) {
                                DestroyImmediate (card.visualCard.Anchor);
                            }

                        }
                    }
                }
            }
        }
    }

    static public PlayerClass GetPlayer (int number) {
        return PlayedMatch.Player [number];
    }

    private void Start () {
        instance = this;
        CurrentGUI = this;
        Debug.Log (name);

        CreatePlayersUI ();
        PlayedMatch.Board.EnableVisualisation ();
    }


    public void Update () {
        for (int x = 1; x <= 4; x++) {
            if (Input.GetKeyDown (x.ToString ())) {
                SelectedStack = x - 1;
                //PlayedMatch.MoveTopCard (MyPlayerNumber, x - 1);
            }
        }
        if (Input.GetKeyDown ("r")) {
            ClientLogic.MyInterface.CmdJoinGameAgainstAI ();
        }
    }

    static public void TileAction (int x, int y) {
        PlayedMatch.PlayCard (x, y, MyPlayerNumber, SelectedStack);
    }

    static public void ShowInGameUI (MatchClass playedMatch) {
        DestroyVisuals ();
        DestroyMenu ();
        PlayedMatch = playedMatch;
        CurrentCanvas.AddComponent<InGameUI> ();
    }

    static public PlayerClass GetPlayer () {
        return PlayedMatch.Player [MyPlayerNumber];
    }

    static public CardClass GetSelectedCard () {
        return GetPlayer ().GetTopCard (SelectedStack);
    }
    
    static public void CreatePlayersUI () {
        Debug.Log ("Test2");
        for (int x = 1; x <= NumberOfPlayers; x++) {
            PlayerClass player = GetPlayer (x);
            bool ally = player.properties.team == GetPlayer (MyPlayerNumber).properties.team;
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
                VisualEffectAnchor [x, y].name = "VisualEffectAnchor";
            }
        }
    }

    static public void HideAreaHovers () {
        foreach (GameObject anchor in VisualEffectAnchor) {
            Transform [] childs = new Transform [anchor.transform.childCount];
            for (int c = 0; c < childs.Length; c++) {
                childs [c] = anchor.transform.GetChild (c);
            }
            for (int c = 0; c < childs.Length; c++) {
                DestroyImmediate (childs [c].gameObject);
            }
        }
    }


    static public void SetAreaHovers (int x, int y) {
        HideAreaHovers ();
        if (PlayedMatch.Board.tile [x, y].enabled) {
            List<AbilityVector> list = PlayedMatch.Board.GetAbilityVectors (x, y, GetSelectedCard ().abilityArea);
            foreach (AbilityVector vector in list) {
                if (PlayedMatch.Board.IsTileEnabled (vector.x, vector.y)) {
                    VisualEffectInterface.CreateEffect (VisualEffectAnchor [vector.x, vector.y], GetSelectedCard ().abilityType, true, false);
                    /*if (PlayedMatch.Board.IsTileInBounds (vector.pushX, vector.pushY)) {
                        VisualEffectInterface.CreateEffect (VisualEffectAnchor [vector.pushX, vector.pushY], GetSelectedCard ().abilityType, true, false);
                    }*/
                }
            }
        }
    }
}
