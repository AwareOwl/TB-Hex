﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualMatch : MonoBehaviour {

    static public float GlobalTimer = 0;
    static public VisualMatch instance;

    static public List<GameObject> garbage = new List<GameObject> ();

    private void Awake () {
        instance = this;
        GlobalTimer = 0.5f;
    }

    private void Update () {
        GlobalTimer = Mathf.Max (0, GlobalTimer - Time.deltaTime);
    }

    public void EnableVisual () {
        //usedCardPreview = GOUI.CurrentCanvas.AddComponent<UsedCardPreview> ();
    }

    public void DestroyVisuals () {
        foreach (GameObject obj in garbage) {
            DestroyImmediate (obj);
        }
        DestroyImmediate (this);
    }

    public void PlayCard (int playerNumber, bool player, int playerPosition, CardClass card) {
        StartCoroutine (IEPlayCard (playerNumber, player, playerPosition, card));
    }

    public IEnumerator IEPlayCard (int playerNumber, bool player, int playerPosition, CardClass card) {
        yield return new WaitForSeconds (GlobalTimer);
        GameObject Clone = new GameObject ();
        Clone.AddComponent<UsedCardPreview> ().Init (playerNumber, player, playerPosition, card);
        //Clone.AddComponent<UIController> ();
        //Clone.name = UIString.UsedCardPreview;
    }

    public void CreateRemains (VisualTile tile) {
        StartCoroutine (IECreateRemains (tile));
    }

    public IEnumerator IECreateRemains (VisualTile tile) {
        yield return new WaitForSeconds (GlobalTimer);
        tile.CreateRemains ();
    }


    public void EnableTile (VisualTile tile, bool enable) {
        StartCoroutine (IEEnableTile (tile, enable));
    }

    public IEnumerator IEEnableTile (VisualTile tile, bool enable) {
        yield return new WaitForSeconds (GlobalTimer);
        tile.EnableTile (enable);
    }


    public void Init (VisualToken token, GameObject parent, int owner, TokenType type, int value) {
        StartCoroutine (IEInit (token, parent, owner, type, value));
    }
    public IEnumerator IEInit (VisualToken token, GameObject parent, int owner, TokenType type, int value) {
        yield return new WaitForSeconds (GlobalTimer);
        token.Init (parent, owner, type, value);
    }

    public void CreateToken (VisualToken token) {
        StartCoroutine (IECreateToken (token));
    }
    public IEnumerator IECreateToken (VisualToken token) {
        yield return new WaitForSeconds (GlobalTimer);
        token.CreateToken ();
    }

    public void AddPlayAnimation (VisualToken token) {
        StartCoroutine (IEAddPlayAnimation (token));
    }

    public IEnumerator IEAddPlayAnimation (VisualToken token) {
        yield return new WaitForSeconds (GlobalTimer);
        token.AddPlayAnimation ();
    }

    public void AddCreateAnimation (VisualToken token) {
        StartCoroutine (IEAddCreateAnimation (token));
    }

    public IEnumerator IEAddCreateAnimation (VisualToken token) {
        yield return new WaitForSeconds (GlobalTimer);
        token.AddCreateAnimation ();
    }

    public void SetState (VisualToken token, int owner, TokenType type, int value) {
        StartCoroutine (IESetState (token, owner, type, value));
    }

    public IEnumerator IESetState (VisualToken token, int owner, TokenType type, int value) {
        yield return new WaitForSeconds (GlobalTimer);
        token.SetState (owner, type, value);
    }

    public void DestroyToken (VisualToken token, GameObject anchor) {
        StartCoroutine (IEDestroyToken (token, anchor));
    }

    public IEnumerator IEDestroyToken (VisualToken token, GameObject anchor) {
        yield return new WaitForSeconds (GlobalTimer);
        token.DestroyToken (anchor);
    }

    public void SetTile (VisualToken token, GameObject tile) {
        StartCoroutine (IESetTile (token, tile));
    }

    public IEnumerator IESetTile (VisualToken token, GameObject tile) {
        yield return new WaitForSeconds (GlobalTimer);
        token.SetTile (tile);
    }

    public void MoveToDisabledTile (VisualToken token, int x, int y) {
        StartCoroutine (IEMoveToDisabledTile (token, x, y));
    }

    public IEnumerator IEMoveToDisabledTile (VisualToken token, int x, int y) {
        yield return new WaitForSeconds (GlobalTimer);
        token.MoveToDisabledTile (x, y);
    }

    public void RotateTo (VisualToken token, TileClass source, TileClass dest) {
        StartCoroutine (IERotateTo (token, source, dest));
    }

    public IEnumerator IERotateTo (VisualToken token, TileClass source, TileClass dest) {
        yield return new WaitForSeconds (GlobalTimer);
        VisualEffectInterface.SetRotateTo (token.BorderAccent [0], source.visualTile.Anchor, dest.visualTile.Anchor);
    }

    public void NullRotateTo (VisualToken token) {
        StartCoroutine (IENullRotateTo (token));
    }

    public IEnumerator IENullRotateTo (VisualToken token) {
        yield return new WaitForSeconds (GlobalTimer);
        token.BorderAccent [0].GetComponent<VisualEffectScript> ().SetRotateTo ();
    }


    public void SetPlayerHealthBar (VisualTeam player, int [] score, int [] scoreIncome, int scoreLimit) {
        StartCoroutine (IESetPlayerHealthBar (player, score, scoreIncome, scoreLimit));
    }

    public IEnumerator IESetPlayerHealthBar (VisualTeam player, int [] score, int [] scoreIncome, int scoreLimit) {
        yield return new WaitForSeconds (GlobalTimer);
        player.SetPlayerHealthBar (score, scoreIncome, scoreLimit);
    }

    public void SetPlayerActive (VisualTeam player, bool active) {
        StartCoroutine (IESetPlayerActive (player, active));
    }

    public IEnumerator IESetPlayerActive (VisualTeam player, bool active) {
        yield return new WaitForSeconds (GlobalTimer);
        player.SetPlayerActive (active);
    }






    public void CreateRealEffects (VectorInfo info, AbilityType abilityType) {
        StartCoroutine (IECreateRealEffects (info, abilityType));
    }

    public IEnumerator IECreateRealEffects (VectorInfo info, AbilityType abilityType) {
        yield return new WaitForSeconds (GlobalTimer);
        VisualEffectInterface.CreateRealEffects (info, abilityType);
    }

    public void RealEffect (int x, int y, AbilityType abilityType, bool triggered) {
        StartCoroutine (IERealEffect (x, y, abilityType, triggered));
    }

    public IEnumerator IERealEffect (int x, int y, AbilityType abilityType, bool triggered) {
        yield return new WaitForSeconds (GlobalTimer);
        VisualEffectInterface.RealEffect (x, y, abilityType, triggered);
    }

    public void CreateRealTokenEffect (TileClass token, TokenType effectType) {
        StartCoroutine (IECreateRealTokenEffect (token, effectType));
    }

    public IEnumerator IECreateRealTokenEffect (TileClass token, TokenType effectType) {
        yield return new WaitForSeconds (GlobalTimer);
        VisualEffectInterface.CreateRealTokenEffect (token, effectType);
    }

    public void CreateRealTokenVectorEffect (TileClass token, TileClass destination, TokenType effectType) {
        StartCoroutine (IECreateRealTokenVectorEffect (token, destination, effectType));
    }

    public IEnumerator IECreateRealTokenVectorEffect (TileClass token, TileClass destination, TokenType effectType) {
        yield return new WaitForSeconds (GlobalTimer);
        VisualEffectInterface.CreateRealTokenVectorEffect (token, destination, effectType);
    }




    public void ShowMatchResult (int matchType, string [] winnersNames, int winCondition, int limit, int level, int currentExperience, int maxExperience, int experienceGain) {
        StartCoroutine (IEShowMatchResult (matchType, winnersNames, winCondition, limit, level, currentExperience, maxExperience, experienceGain));
    }

    public IEnumerator IEShowMatchResult (int matchType, string [] winnersNames, int winCondition, int limit, int level, int currentExperience, int maxExperience, int experienceGain) {
        yield return new WaitForSeconds (GlobalTimer);
        string type;
        switch (matchType) {
            case 3:
                type = "TutorialResults";
                break;
            case 2:
                type = "BossResults";
                break;
            case 1:
                type = "PuzzleResults";
                break;
            default:
                type = "MatchResults";
                break;
        }
        GOUI.ShowMessageWithProgressionBar (Language.GetMatchResult (winnersNames, winCondition, limit), type, level, currentExperience, maxExperience, experienceGain);
    }




    public void ShuffleCardVisual (PlayerClass player, CardClass card) {
        StartCoroutine (IEShuffleCardVisual (player, card));
    }

    public IEnumerator IEShuffleCardVisual (PlayerClass client, CardClass card) {
        yield return new WaitForSeconds (GlobalTimer);
        client.ShuffleCardVisual (card);
    }

    public void DestroyCardVisual (VisualCard vCard) {
        StartCoroutine (IEDestroyCardVisual (vCard));
    }

    public IEnumerator IEDestroyCardVisual (VisualCard vCard) {
        yield return new WaitForSeconds (GlobalTimer);
        vCard.DestroyCardVisual ();
    }

    public void SetState (VisualCard vCard, int tokenValue, TokenType tokenType, int abilityArea, AbilityType abilityType) {
        StartCoroutine (IESetState (vCard, tokenValue, tokenType, abilityArea, abilityType));
    }

    public IEnumerator IESetState (VisualCard vCard, int tokenValue, TokenType tokenType, int abilityArea, AbilityType abilityType) {
        yield return new WaitForSeconds (GlobalTimer);
        vCard.SetState (tokenValue, tokenType, abilityArea, abilityType);
    }

    public void UpdateCardVisuals (PlayerClass player, int stackNumber, int stackSize, int cardNumber, int position) {
        StartCoroutine (IEUpdateCardVisuals (player, stackNumber, stackSize, cardNumber, position));
    }

    public IEnumerator IEUpdateCardVisuals (PlayerClass client, int stackNumber, int stackSize, int cardNumber, int position) {
        yield return new WaitForSeconds (GlobalTimer);
        client.UpdateCardVisuals (stackNumber, stackSize, cardNumber, position);
    }

    public void UpdateTurnsLeft (int turnsLeft) {
        StartCoroutine (IEUpdateTurnsLeft (turnsLeft));
    }

    public IEnumerator IEUpdateTurnsLeft (int turnsLeft) {
        yield return new WaitForSeconds (GlobalTimer);
        InGameUI.SetTurn (turnsLeft);
    }

    public void DelayedTooltip (int px, int py, int side, string s) {
        StartCoroutine (IEDelayedTooltip (px, py, side, s));
    }

    public IEnumerator IEDelayedTooltip (int px, int py, int side, string s) {
        yield return new WaitForSeconds (GlobalTimer);
        Tooltip.NewTooltip (px, py, side, s);
    }


}
