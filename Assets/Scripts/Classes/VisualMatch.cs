﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualMatch : MonoBehaviour {

    static public float GlobalTimer = 0;
    static public VisualMatch instance;
    MatchClass match;

    private void Awake () {
        instance = this;
        GlobalTimer = 0;
    }

    private void Update () {
        GlobalTimer = Mathf.Max (0, GlobalTimer - Time.deltaTime);
    }

    public void EnableVisual () {
        //usedCardPreview = GOUI.CurrentCanvas.AddComponent<UsedCardPreview> ();
    }

    public void PlayCard (int playerNumber, CardClass card) {
        StartCoroutine (IEPlayCard (playerNumber, card));
        GlobalTimer += 0.5f;
    }

    public IEnumerator IEPlayCard (int playerNumber, CardClass card) {
        yield return new WaitForSeconds (GlobalTimer);
        new GameObject ().AddComponent<UsedCardPreview> ().Init (playerNumber, card);
    }



    public void EnableTile (VisualTile tile, bool enable) {
        StartCoroutine (IEEnableTile (tile, enable));
    }

    public IEnumerator IEEnableTile (VisualTile tile, bool enable) {
        yield return new WaitForSeconds (GlobalTimer);
        tile.EnableTile (enable);
    }


    public void Init (VisualToken token, GameObject parent, int owner, int type, int value) {
        StartCoroutine (IEInit (token, parent, owner, type, value));
    }
    public IEnumerator IEInit (VisualToken token, GameObject parent, int owner, int type, int value) {
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
        GlobalTimer += 0.5f;
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

    public void SetState (VisualToken token, int owner, int type, int value) {
        StartCoroutine (IESetState (token, owner, type, value));
    }

    public IEnumerator IESetState (VisualToken token, int owner, int type, int value) {
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


    public void SetPlayerHealthBar (VisualPlayer player, int score, int scoreIncome, int scoreLimit) {
        StartCoroutine (IESetPlayerHealthBar (player, score, scoreIncome, scoreLimit));
    }

    public IEnumerator IESetPlayerHealthBar (VisualPlayer player, int score, int scoreIncome, int scoreLimit) {
        yield return new WaitForSeconds (GlobalTimer);
        player.SetPlayerHealthBar (score, scoreIncome, scoreLimit);
    }
}