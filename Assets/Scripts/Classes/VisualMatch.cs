using System;
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
}
