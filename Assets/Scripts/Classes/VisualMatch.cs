using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualMatch : MonoBehaviour {

    static public float GlobalTimer = 0;
    static public VisualMatch instance;

    static public List<GameObject> garbage = new List<GameObject> ();

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

    public void DestroyVisuals () {
        foreach (GameObject obj in garbage) {
            DestroyImmediate (obj);
        }
        DestroyImmediate (this);
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


    public void SetPlayerHealthBar (VisualPlayer player, int score, int scoreIncome, int scoreLimit) {
        StartCoroutine (IESetPlayerHealthBar (player, score, scoreIncome, scoreLimit));
    }

    public IEnumerator IESetPlayerHealthBar (VisualPlayer player, int score, int scoreIncome, int scoreLimit) {
        yield return new WaitForSeconds (GlobalTimer);
        player.SetPlayerHealthBar (score, scoreIncome, scoreLimit);
    }





    public void CreateRealEffects (VectorInfo info, int abilityType) {
        StartCoroutine (IECreateRealEffects (info, abilityType));
    }

    public IEnumerator IECreateRealEffects (VectorInfo info, int abilityType) {
        yield return new WaitForSeconds (GlobalTimer);
        VisualEffectInterface.CreateRealEffects (info, abilityType);
    }
    public void RealEffect (int x, int y, int abilityType, bool triggered) {
        StartCoroutine (IERealEffect (x, y, abilityType, triggered));
    }

    public IEnumerator IERealEffect (int x, int y, int abilityType, bool triggered) {
        yield return new WaitForSeconds (GlobalTimer);
        VisualEffectInterface.RealEffect (x, y, abilityType, triggered);
    }

    public void CreateRealTokenVectorEffect (TileClass token, TileClass destination, int effectType) {
        StartCoroutine (IECreateRealTokenVectorEffect (token, destination, effectType));
    }

    public IEnumerator IECreateRealTokenVectorEffect (TileClass token, TileClass destination, int effectType) {
        yield return new WaitForSeconds (GlobalTimer);
        VisualEffectInterface.CreateRealTokenVectorEffect (token, destination, effectType);
    }




    public void ShowMatchResult (string winnerName, int winCondition, int limit) {
        StartCoroutine (IEShowMatchResult (winnerName, winCondition, limit));
    }

    public IEnumerator IEShowMatchResult (string winnerName, int winCondition, int limit) {
        yield return new WaitForSeconds (GlobalTimer);
        GOUI.ShowMessage (Language.GetMatchResult (winnerName, winCondition, limit), "MainMenu");
    }




    public void ShuffleCardVisual (PlayerClass player, CardClass card) {
        StartCoroutine (IEShuffleCardVisual (player, card));
    }

    public IEnumerator IEShuffleCardVisual (PlayerClass client, CardClass card) {
        yield return new WaitForSeconds (GlobalTimer);
        client.ShuffleCardVisual (card);
    }

    public void UpdateCardVisuals (PlayerClass player, int stackNumber, int cardNumber, int position) {
        StartCoroutine (IEUpdateCardVisuals (player, stackNumber, cardNumber, position));
    }

    public IEnumerator IEUpdateCardVisuals (PlayerClass client, int stackNumber, int cardNumber, int position) {
        yield return new WaitForSeconds (GlobalTimer);
        client.UpdateCardVisuals (stackNumber, cardNumber, position);
    }


}
