using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAnimation : MonoBehaviour {

    static public bool [] stackZoomed = new bool [15];

    int stackSize;
    int stack;
    int numberOfStacks;
    public int position;
    VisualCard visual;

    public const float shuffleTime = 0.5f;
    public float shuffleTimer = 0;

    static public void RemoveAllZooms () {
        for (int x = 0; x < stackZoomed.Length; x++) {
            stackZoomed [x] = false;
        }
    }

    public void Init (VisualCard visual, int stack, int stackSize, int numberOfStacks, int position) {
        this.visual = visual;
        this.stack = stack;
        this.stackSize = stackSize;
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
        if (!stackZoomed [stack]) {
            transform.localEulerAngles = new Vector3 (Mathf.LerpAngle (transform.localEulerAngles.x, 0, Time.deltaTime * 4), 0, 0);
            Vector3 dest = Vector3.Lerp (transform.localPosition, new Vector3 ((x + 0.5f - numberOfStacks / 2f) * 1.3f, 2 - 0.15f * y, -5.55f - 0.025f * y), Time.deltaTime * 4);
            transform.localPosition = dest + new Vector3 (0, 0, -Mathf.Sin (shuffleTimer / shuffleTime * Mathf.PI) * shuffleLength);
        } else {
            transform.localEulerAngles = new Vector3 (Mathf.LerpAngle (transform.localEulerAngles.x, -5, Time.deltaTime * 4), 0, 0);
            Vector3 dest = Vector3.Lerp (transform.localPosition, new Vector3 ((x + 0.5f - numberOfStacks / 2f) * 1.3f, 2 - 0.2f * y + 0.2f * stackSize, -5.55f - 1.3f * y + 1.3f * (stackSize - 1)), Time.deltaTime * 6);
            transform.localPosition = dest + new Vector3 (0, 0, -Mathf.Sin (shuffleTimer / shuffleTime * Mathf.PI) * shuffleLength);
        }
        if (stack == InGameUI.SelectedStack && position == 0) {
            visual.EnableHighlight ();
        } else {
            visual.DisableHighlight ();
        }
    }
}
