using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : GOUI {

    static public GameObject [,] VisualEffectAnchor;
    static public GameObject [,] RealVisualEffectAnchor;

    static public InGameUI instance;

    static public MatchClass PlayedMatch;

    static public int MyPlayerNumber = 1;

    static public int SelectedStack;

    static public int NumberOfPlayers = 2;

    static public int CurrentlyOverX = -1;
    static public int CurrentlyOverY = -1;

    override public void DestroyThis () {
        DestroyVisuals ();
    }

    public void DestroyVisuals () {
        if (VisualEffectAnchor != null) {
            foreach (GameObject obj in VisualEffectAnchor) {
                if (obj != null) {
                    DestroyImmediate (obj);
                }
            }
            foreach (GameObject obj in RealVisualEffectAnchor) {
                if (obj != null) {
                    DestroyImmediate (obj);
                }
            }
        }
        if (PlayedMatch != null) {
            PlayedMatch.DestroyVisuals ();
        }
    }

    static public PlayerClass GetPlayer (int number) {
        return PlayedMatch.Player [number];
    }

    private void Start () {
        instance = this;
        CurrentGUI = this;

        MyPlayerNumber = ClientLogic.MyInterface.playerNumber;
        NumberOfPlayers = PlayedMatch.numberOfPlayers;
        EnvironmentScript.CreateNewBackground ();
        CreatePlayersUI ();
        GetPlayer (PlayedMatch.turnOfPlayer).visualPlayer.SetPlayerActive (true);
        PlayedMatch.EnableVisuals ();
        SelectStack (0);

        ExitButton.name = UIString.ShowInGameMenu;
        GOUI.SetSprite (ExitButton, "UI/Butt_S_Settings", true);
    }


    static public void RotateAbilityAreaOnServer (int stackNumber) {
        ClientLogic.MyInterface.CmdCurrentGameRotateAbilityArea (stackNumber);
    }

    static public void RotateAbilityArea (int playerNumber, int stackNumber) {
        PlayedMatch.RotateAbilityArea (playerNumber, stackNumber);
    }

    static public void SelectStack (int x) {
        /*HandClass hand = GetPlayer ().properties.hand;
        StackClass stack = hand.stack [SelectedStack];
        foreach (CardClass card in stack.card) {
            card.visualCard.DisableHighlight ();
        }*/
        SelectedStack = x;
        //GetSelectedCard ().visualCard.EnableHighlight ();
        RefreshHovers ();
    }

    float fetchMissingPacketsTimer = -2;

    bool stacksZoomed;

    public void Update () {
        fetchMissingPacketsTimer += Time.deltaTime;
        if (fetchMissingPacketsTimer >= 0) {
            fetchMissingPacketsTimer -= 2;
            ClientLogic.MyInterface.CmdCurrentGameFetchMissingMoves (PlayedMatch.lastMoveId);
        }
        for (int x = 1; x <= 4; x++) {
            if (Input.GetKeyDown (x.ToString ())) {
                SelectStack (x - 1);
                //PlayedMatch.MoveTopCard (MyPlayerNumber, x - 1);
            }
        }
        if (Input.GetKeyDown ("f4")) {
            ShowInGameUI ();
        }
        if (Input.GetKeyDown ("f5")) {
            ClientLogic.MyInterface.CmdDownloadCurrentGame ();
            //ShowInGameUI ();
        }
        if (Input.GetKeyDown ("i")) {
            //Debug.Log (PlayedMatch.Mov)
        }
        if (Input.GetKeyDown ("p")) {
            //ClientLogic.MyInterface.CmdCurrentGameFetchMissingMoves (PlayedMatch.lastMoveId);
            //Debug.Log (PlayedMatch.Mov)
        }
        if (Input.GetKeyDown ("h")){
            stacksZoomed = !stacksZoomed;
            int count = CardAnimation.stackZoomed.Length;
            for (int x = 0; x < count; x++) {
                CardAnimation.stackZoomed [x] = stacksZoomed;
            }
        }
        /*if (Input.GetKeyDown ("r")) {
            ClientLogic.MyInterface.CmdJoinGameAgainstAI ();
        }
        if (Input.GetKeyDown ("p")) {
            PlayedMatch.MakeRandomMove ();
        }
        if (Input.GetKeyDown ("a")) {
            PlayedMatch.RunAI ();
        }
        if (Input.GetKeyDown ("h")) {
            MatchClass match = PlayedMatch;
            Debug.Log ("Test");
            while (match != null) {
                Debug.Log (match.turn);
                match = match.prevMatch;
            }
        }
        if (Input.GetKeyDown ("t")) {
            Debug.Log (PlayedMatch.turn);
        }*/
        /*if (Input.GetKeyDown ("z")) {
            if (PlayedMatch.prevMatch != null) {
                DestroyVisuals ();
                PlayedMatch = PlayedMatch.prevMatch;
                ShowInGameUI (PlayedMatch);
            }
        }*/
    }

    static public void TileAction (int x, int y) {
        if (InputController.debuggingEnabled) {
            Debug.Log ("Tile action performed");
        }
        ClientLogic.MyInterface.CmdCurrentGameMakeAMove (x, y, SelectedStack);
        //PlayedMatch.PlayCard (x, y, MyPlayerNumber, SelectedStack);
        RefreshHovers ();
    }

    static public void ShowInGameUI () {
        DestroyMenu ();
       // PlayedMatch = MatchMakingClass.FindMatch (ClientLogic.MyInterface.AccountName);
        CurrentCanvas.AddComponent<InGameUI> ();
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
        return GetPlayer ().GetTopCard (SelectedStack);
    }
    
    static public void CreatePlayersUI () {
        int numberOfPlayers = 0;
        int playerPosition = 0;
        for (int x = 1; x <= NumberOfPlayers; x++) {
            PlayerClass player = GetPlayer (x);
            if (player != null) {
                numberOfPlayers++;
            }
        }
        for (int x = 1; x <= NumberOfPlayers; x++) {
            PlayerClass player = GetPlayer (x);
            if (player != null) {
                bool ally = player.properties.team == GetPlayer (MyPlayerNumber).properties.team;
                player.EnableVisuals ();
                player.visualPlayer.CreatePlayerUI (player, ally, numberOfPlayers, playerPosition);
                player.visualPlayer.SetPlayerHealthBar (PlayedMatch, player);
                playerPosition++;
            }
        }
        int sx = PlayedMatch.Board.tile.GetLength (0);
        int sy = PlayedMatch.Board.tile.GetLength (1);
        VisualEffectAnchor = new GameObject [sx + 2, sy + 2];
        RealVisualEffectAnchor = new GameObject [sx + 2, sy + 2];
        for (int x = 0; x < sx + 2; x++) {
            for (int y = 0; y < sy + 2; y++) {
                VisualEffectAnchor [x, y] = new GameObject ();
                VisualEffectAnchor [x, y].transform.localPosition = VisualTile.TilePosition (x - 1, 0.2f, y - 1);
                VisualEffectAnchor [x, y].name = "VisualEffectAnchor";
                RealVisualEffectAnchor [x, y] = new GameObject ();
                RealVisualEffectAnchor [x, y].transform.localPosition = VisualTile.TilePosition (x - 1, 0.2f, y - 1);
            }
        }
    }

    static public GameObject GetAnchor (int x, int y) {
        return VisualEffectAnchor [x + 1, y + 1];
    }

    static public GameObject GetRealAnchor (int x, int y) {
        return RealVisualEffectAnchor [x + 1, y + 1];
    }

    static public void HideAreaHovers () {
        CurrentlyOverX = -1;
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

    static public void RefreshHovers () {
        SetAreaHovers (CurrentlyOverX, CurrentlyOverY);
    }


    static public void SetAreaHovers (int x, int y) {
        if (x  < 0) {
            return;
        }
        HideAreaHovers ();
        CurrentlyOverX = x;
        CurrentlyOverY = y;

        if (PlayedMatch.Board.tile [x, y].IsEmptyTile ()) {
            CardClass card = GetSelectedCard ();
            int abilityType = card.abilityType;
            int abilityArea = card.abilityArea;
            int tokenType = card.tokenType;
            int tokenValue = card.value;
            int tokenModifier = PlayedMatch.Board.NumberOfTypes [7] - PlayedMatch.Board.NumberOfTypes [11];
            tokenValue += tokenModifier;

            VisualToken token = new VisualToken ();
            if (tokenModifier > 0) {
                token.Text.GetComponent<Renderer> ().material.color = new Color (0, 0.5f, 0);
            } else if (tokenModifier < 0) {
                token.Text.GetComponent<Renderer> ().material.color = new Color (0.5f, 0, 0);
            }
            token.AddCreateAnimation ();
            token.SetState (MyPlayerNumber, tokenType, tokenValue);
            token.SetParent (GetAnchor (x, y));
            TileClass tokenTile = PlayedMatch.Board.GetTile (x, y);
            VectorInfo tokenInfo = PlayedMatch.GetTokenVectorInfo (tokenTile, new TokenClass (null, tokenType, tokenValue, MyPlayerNumber));
            switch (tokenType) {
                case 3:
                case 4:
                    if (tokenInfo.Triggered1 == null || tokenInfo.Triggered1.Count == 0) {
                        break;
                    }
                    TileClass trigger = tokenInfo.Triggered1 [0];
                    VisualEffectInterface.SetRotateTo (token.BorderAccent [0], GetAnchor (x, y), GetAnchor (trigger.x, trigger.y));
                    break;
            }

            switch (abilityType) {
                case 7:
                    GameObject Clone = VisualEffectInterface.CreateEffect1 (GetAnchor (x, y), abilityType, false, false);
                    Clone.transform.localPosition = new Vector3 (0, 0.5f, 0);
                    if (PlayedMatch.LastMove != null) {
                        abilityType = PlayedMatch.LastMove.usedCard.abilityType;
                    }
                    break;
            }
            VectorInfo info = PlayedMatch.GetVectorInfo (x, y, MyPlayerNumber, abilityArea, abilityType, new TokenClass (null, tokenType, tokenValue, MyPlayerNumber));
            foreach (AbilityVector vector in info.TriggeredVector) {
                VisualEffectInterface.CreateEffectPointingAt (
                    GetAnchor (vector.x, vector.y), GetAnchor (vector.pushX, vector.pushY).transform.position, abilityType, true, false);
                VisualEffectInterface.CreateEffect2 (GetAnchor (vector.pushX, vector.pushY), abilityType, true, false);
            }
            foreach (AbilityVector vector in info.NotTriggeredVector) {
                VisualEffectInterface.CreateEffectPointingAt (
                    GetAnchor (vector.x, vector.y), GetAnchor (vector.pushX, vector.pushY).transform.position, abilityType, false, false);
                VisualEffectInterface.CreateEffect2 (GetAnchor (vector.pushX, vector.pushY), abilityType, false, false);
            }
            foreach (TileClass tile in info.Triggered1) {
                VisualEffectInterface.CreateEffect1 (GetAnchor (tile.x, tile.y), abilityType, true, false);
            }
            foreach (TileClass tile in info.Triggered2) {
                VisualEffectInterface.CreateEffect2 (GetAnchor (tile.x, tile.y), abilityType, true, false);
            }
            foreach (TileClass tile in info.NotTriggered) {
                if (tile.enabled) {
                    VisualEffectInterface.CreateEffect1 (GetAnchor (tile.x, tile.y), abilityType, false, false);
                }
            }
        }
    }
}
