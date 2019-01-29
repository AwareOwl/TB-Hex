using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAnimation : MonoBehaviour {

    int stack;
    int numberOfStacks;
    public int position;
    VisualCard visual;

    public const float shuffleTime = 0.5f;
    public float shuffleTimer = 0;

    public void Init (VisualCard visual, int stack, int numberOfStacks, int position) {
        this.visual = visual;
        this.stack = stack;
        this.numberOfStacks = numberOfStacks;
        this.position = position;
    }
	
	// Update is called once per frame
	void Update () {
        int x = stack;
        int y = position;
        float shuffleLength = 2f;
        transform.localPosition -= new Vector3 (0, 0, - Mathf.Sin (shuffleTimer / shuffleTime * Mathf.PI) * shuffleLength);
        shuffleTimer -= Time.deltaTime;
        shuffleTimer = Mathf.Max (shuffleTimer, 0);
        Debug.Log (x.ToString () + " " + numberOfStacks.ToString ());
        Vector3 dest = Vector3.Lerp (transform.localPosition, new Vector3 ((x + 0.5f - numberOfStacks / 2f) * 1.3f, 2 - 0.15f * y, -5.55f - 0.025f * y), Time.deltaTime * 4);
        transform.localPosition = dest + new Vector3 (0, 0, - Mathf.Sin (shuffleTimer / shuffleTime * Mathf.PI) * shuffleLength);
        if (stack == InGameUI.SelectedStack && position == 0) {
            visual.EnableHighlight ();
        } else {
            visual.DisableHighlight ();
        }
    }
}
