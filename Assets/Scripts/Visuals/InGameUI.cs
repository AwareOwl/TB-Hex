using System.Collections.Generic;
using UnityEngine;

public class InGameUI : GOUI {

    static public GameObject [,] VisualEffectAnchor;
    static public GameObject [,] RealVisualEffectAnchor;

    static public InGameUI instance;

    static public MatchClass PlayedMatch;

    static public int myPlayerNumber = 1;

    static public int selectedStack;

    static public int numberOfPlayers = 2;

    static public int currentlyOverX = -1;
    static public int currentlyOverY = -1;

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
        CurrentGOUI = this;
        RemoveAllZooms ();

        myPlayerNumber = ClientLogic.MyInterface.playerNumber;
        numberOfPlayers = PlayedMatch.numberOfPlayers;
        EnvironmentScript.CreateNewBackground ();
        CreatePlayersUI ();
        CreateTurnUI ();
        CreateSpecialUI ();
        GetPlayer (PlayedMatch.turnOfPlayer).visualTeam.SetPlayerActive (true);
        PlayedMatch.EnableVisuals ();
        if (PlayedMatch.properties.specialType != 3) {
            SelectStack (0);
        } else {
            SelectStack (-1);
        }

        OptionsButton.name = UIString.ShowInGameOptionsMenu;

        SoundManager.PlayAudioClip (MyAudioClip.MatchStart);

        if (PlayedMatch.properties.specialType == 3) {
            TutorialManager.SetTutorialNumber (PlayedMatch.properties.specialId);
        }
        TutorialManager.NewState (TutorialManager.beginMatch);
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
        if (PlayedMatch.Player [myPlayerNumber].hand.GetNumberOfStacks () <= x || !GetPlayer ().GetStack (x).atLeast1Enabled) {
            return;
        }
        selectedStack = x;
        TutorialManager.NewState (TutorialManager.selectCardState);
        //GetSelectedCard ().visualCard.EnableHighlight ();
        RefreshHovers ();
    }

    float fetchMissingPacketsTimer = -2;

    bool stacksZoomed;

    public void RemoveAllZooms () {
        stacksZoomed = false;
        CardAnimation.RemoveAllZooms ();
    }

    public void Update () {
        fetchMissingPacketsTimer += Time.deltaTime;


        //Tooltip.permanent = false;
        //Tooltip.NewTooltip (720, (int) (Time.time * 5 % 1080), Language.TutorialTooltip [0]);

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
                CardAnimation.SetStackZoomed (x, stacksZoomed);
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
        if (selectedStack < 0 || selectedStack >= GetPlayer ().hand.stack.Length) {
            return;
        }
        if (TutorialManager.blockActions) {
            return;
        }
        ClientLogic.MyInterface.CmdCurrentGameMakeAMove (x, y, selectedStack);
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
        return PlayedMatch.Player [myPlayerNumber];
    }

    static public CardClass GetSelectedCard () {
        return GetPlayer ().GetTopCard (selectedStack);
    }
    
    static public void CreatePlayersUI () {
        int numberOfPlayers = 0;
        int numberOfTeams = 0;
        int globalPosition = 0;
        int teamPosition = 0;
        bool numerate = false;
        int myTeam = GetPlayer (myPlayerNumber).properties.team;
        for (int x = 1; x <= InGameUI.numberOfPlayers; x++) {
            PlayerClass player = GetPlayer (x);
            if (player != null) {
                numberOfPlayers++;
            }
        }
        List<PlayerClass> [] teams = new List<PlayerClass> [5];
        for (int x = 0; x < teams.Length; x++) {
            teams [x] = new List<PlayerClass> ();
        }
        for (int x = 1; x <= InGameUI.numberOfPlayers; x++) {
            PlayerClass player = GetPlayer (x);
            if (player == null) {
                continue;
            }
            PlayerPropertiesClass properties = player.properties;
            if (player.properties == null) {
                continue;
            }
            if (properties.team != x) {
                numerate = true;
            }
            int team = properties.team;
            if (teams [team].Count == 0) {
                numberOfTeams++;
            }
            teams [properties.team].Add (player);
            player.EnableVisuals ();
        }
        for (int x = 1; x < teams.Length; x++) {
            int count = teams [x].Count;
            if (count > 0) {
                PlayerClass player = teams [x] [0];
                bool ally = player.properties.team == myTeam;
                VisualTeam vTeam = player.visualTeam;
                vTeam.CreatePlayerUI (teams [x], numerate, ally, numberOfTeams, numberOfPlayers, teamPosition++, globalPosition);
                for (int y = 0; y < count; y++) {
                    player = teams [x] [y];
                    player.visualTeam = vTeam;
                    //vTeam.SetPlayerHealthBar (y, player.score, player.scoreIncome, PlayedMatch.properties.scoreLimit);
                }
                vTeam.UpdateVisuals (PlayedMatch);
                globalPosition += count;
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

    static TextMesh TurnText;

    static public void CreateTurnUI () {
        if (!PlayedMatch.properties.turnWinCondition) {
            return;
        }
        GameObject Clone;
        Clone = CreateSprite ("UI/White", 75, 45, 10, 120, 70, true);
        Clone.name = UIString.TurnCounter;
        //Clone = CreateSprite ("UI/Panel_PopUp_01_Sliced", 75, 45, 10, 120, 75, true);
        Clone.GetComponent<Renderer> ().material.color = Color.black;
        Clone = CreateSprite ("Textures/Other/Turn", 45, 45, 11, 45, 45, true);
        Clone.GetComponent<Collider> ().enabled = false;
        if (PlayedMatch.properties.turnWinCondition) {
            Clone = CreateText (PlayedMatch.TurnsLeft ().ToString (), 100, 45, 11, 0.025f);
        } else {
            Clone = CreateText ("-", 100, 45, 11, 0.025f);
        }
        Clone.GetComponent<Renderer> ().material.color = Color.white;
        TurnText = Clone.GetComponent<TextMesh> ();
        //Clone.GetComponent<Text> ().material.color = Color.white;
    }

    static public void CreateSpecialUI () {
        if (!PlayedMatch.properties.special) {
            return;
        }
        GameObject Clone;
        Clone = CreateSprite ("UI/Butt_S_Revert", 1380, 1020, 11, 90, 90, true);
        switch (PlayedMatch.properties.specialType) {
            case 1:
                Clone.name = UIString.RestartPuzzle;
                break;
            case 2:
                Clone.name = UIString.RestartBoss;
                break;
            case 3:
                Clone.name = UIString.RestartTutorial;
                break;
        }
    }

    static public void RestartPuzzle () {
        ClientInterface client = ClientLogic.MyInterface;
        client.CmdCurrentGameConcede ();
        client.CmdStartPuzzle (PlayedMatch.properties.gameMode);
    }

    static public void RestartBoss () {
        ClientInterface client = ClientLogic.MyInterface;
        client.CmdCurrentGameConcede ();
        string bossName2;
        string bossName3 = "";
        string bossName4 = "";
        bossName2 = PlayedMatch.Player [2].properties.displayName;
        if (PlayedMatch.GetPlayer (3) != null) {
            bossName3 = PlayedMatch.Player [3].properties.displayName;
        }
        if (PlayedMatch.GetPlayer (4) != null) {
            bossName4 = PlayedMatch.Player [4].properties.displayName;
        }
        client.CmdStartBoss (PlayedMatch.properties.gameMode, bossName2, bossName3, bossName4);
    }

    static public void RestartTutorial () {
        ClientInterface client = ClientLogic.MyInterface;
        client.CmdCurrentGameConcede ();
        string tutorialName = "";
        if (numberOfPlayers > 1) {
            tutorialName = PlayedMatch.Player [2].properties.displayName;
        }
        client.CmdStartTutorial (PlayedMatch.properties.gameMode, tutorialName);
    }

    static public void SetTurn (int turnsLeft) {
        TurnText.text = turnsLeft.ToString();
    }

    static public GameObject GetAnchor (int x, int y) {
        return VisualEffectAnchor [x + 1, y + 1];
    }

    static public GameObject GetRealAnchor (int x, int y) {
        return RealVisualEffectAnchor [x + 1, y + 1];
    }

    static public void HideAreaHovers () {
        //currentlyOverX = -1;
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
        //Debug.Log (currentlyOverX + " " + currentlyOverY);
        SetAreaHovers (currentlyOverX, currentlyOverY);
    }

    static public void CheckHideAreaCovers (int x, int y) {
        StackClass stack = GetPlayer ().GetStack (selectedStack);
        if (stack == null) {
            return;
        }
        if (!PlayedMatch.Board.tile [x, y].IsPlayable (myPlayerNumber) || !stack.atLeast1Enabled) {
            HideAreaHovers ();
        }
    }

    static public void SetAreaHovers (int x, int y) {
        if (x  < 0) {
            return;
        }
        HideAreaHovers ();
        currentlyOverX = x;
        currentlyOverY = y;

        StackClass stack = GetPlayer ().GetStack (selectedStack);
        if (stack == null) {
            return;
        }

        if (PlayedMatch.Board.tile [x, y].IsPlayable (myPlayerNumber) && stack.atLeast1Enabled) {
            CardClass card = GetSelectedCard ();
            AbilityType abilityType = card.abilityType;
            int abilityArea = card.abilityArea;
            TokenType tokenType = card.tokenType;
            int tokenValue = card.tokenValue;
            int tokenModifier = PlayedMatch.Board.NumberOfTypes [7] - PlayedMatch.Board.NumberOfTypes [11];
            tokenValue += tokenModifier;

            VisualToken token = new VisualToken ();
            if (tokenModifier > 0) {
                token.Text.GetComponent<Renderer> ().material.color = new Color (0, 0.5f, 0);
            } else if (tokenModifier < 0) {
                token.Text.GetComponent<Renderer> ().material.color = new Color (0.5f, 0, 0);
            }
            token.AddCreateAnimation ();
            token.SetState (myPlayerNumber, tokenType, tokenValue);
            token.SetParent (GetAnchor (x, y));
            TileClass tokenTile = PlayedMatch.Board.GetTile (x, y);
            VectorInfo tokenInfo = PlayedMatch.GetTokenVectorInfo (tokenTile, new TokenClass (null, tokenType, tokenValue, myPlayerNumber));
            switch (tokenType) {
                case TokenType.T3:
                case TokenType.T4:
                    if (tokenInfo.Triggered1 == null || tokenInfo.Triggered1.Count == 0) {
                        break;
                    }
                    TileClass trigger = tokenInfo.Triggered1 [0];
                    VisualEffectInterface.SetRotateTo (token.BorderAccent [0], GetAnchor (x, y), GetAnchor (trigger.x, trigger.y));
                    break;
            }

            switch (abilityType) {
                case AbilityType.T7:
                    GameObject Clone = VisualEffectInterface.CreateEffect1 (GetAnchor (x, y), abilityType, true, false);
                    Clone.transform.localPosition = new Vector3 (0, 0.5f, 0);
                    if (PlayedMatch.LastMove != null) {
                        abilityType = PlayedMatch.GetLastPlayedCard ().abilityType;
                    }
                    break;
                case AbilityType.T38:
                case AbilityType.T42:
                case AbilityType.T49:
                    Clone = VisualEffectInterface.CreateEffect1 (GetAnchor (x, y), abilityType, true, false);
                    Clone.transform.localPosition = new Vector3 (0, 0.5f, 0);
                    break;
                case AbilityType.T40:
                    Clone = VisualEffectInterface.CreateEffect1 (GetAnchor (x, y), abilityType, tokenInfo.remainsCount < 2, false);
                    Clone.transform.localPosition = new Vector3 (0, 0.5f, 0);
                    break;
            }
            VectorInfo info = PlayedMatch.GetVectorInfo (x, y, myPlayerNumber, abilityArea, abilityType, new TokenClass (null, tokenType, tokenValue, myPlayerNumber));
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
