using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffectScript : MonoBehaviour {
    
	public bool triggered = true;
    public bool destroyOnEnd = false;
    public bool destroyThisOnEnd = false;
    public int endPhase = 1;
    
	float timer;
    float percentageTimer;
    float timerScale = AppSettings.AnimationDuration;
    int currentPhase = 0;
    public List <float> phaseTimer;
    
    public List <Color> basicColor = null;
    public List <Vector3> basicScale;
    public List <Vector3> basicPosition;
    public List <Vector3> deltaPosition;
    
    public List <bool> drift;

    public List<bool> lerpPosition;
    public List<bool> jumpAnimation;
    float driftHeight = 0f;
    float driftDestination = 0f;

    public bool rotateToCamera;
    public bool rotateToCameraVertical;

    Renderer [] renderers;

    public void Init (Color color, int endPhase, bool triggered, bool destroyOnEnd) {
        this.endPhase = endPhase;
        this.triggered = triggered;
        this.destroyOnEnd = destroyOnEnd;
        SetColor (color);
	}

    public void Init () {

    }
    
    public void SetColor (Color color) {
        basicColor = new List<Color> ();
        for (int x = 0; x <= endPhase; x++) {
            basicColor.Add (color);
        }
    }

    public void SetLastColor (Color color) {
        basicColor [endPhase] = color;
    }

    public void AutoSetPosition () {
        if (basicPosition == null) {
            SetPosition (transform.localPosition);
        }
    }

    public void SetPosition (Vector3 position) {
        basicPosition = new List<Vector3> ();
        for (int x = 0; x <= endPhase; x++) {
            basicPosition.Add (position);
        }
    }

    public void SetLastPosition (Vector3 position) {
        if (basicPosition == null) {
            SetPosition (position);
            return;
        }
        basicPosition [endPhase] = position;
    }

    public void SetDeltaPosition (Vector3 position) {
        deltaPosition = new List<Vector3> ();
        for (int x = 0; x <= endPhase; x++) {
            deltaPosition.Add (position);
        }
    }

    public void SetScale (Vector3 scale) {
        basicScale = new List<Vector3> ();
        for (int x = 0; x <= endPhase; x++) {
            basicScale.Add (scale);
        }
    }

    public void SetLastScale (Vector3 scale) {
        if (basicScale == null) {
            SetScale (scale);
        }
        basicScale [endPhase] = scale;
    }

    public void SetDrift (bool driftValue) {
        drift = new List<bool> ();
        for (int x = 0; x <= endPhase; x++) {
            drift.Add (driftValue);
        }
    }

    public void SetPosition (Vector3 [] position) {
        basicPosition = new List<Vector3> ();
        for (int x = 0; x <= endPhase; x++) {
            basicPosition.Add (position [Mathf.Min (position.Length - 1, x)]);
        }
    }

    public void SetDeltaPosition (Vector3 [] position) {
        deltaPosition = new List<Vector3> ();
        for (int x = 0; x <= endPhase; x++) {
            deltaPosition.Add (position [Mathf.Min (position.Length - 1, x)]);
        }
    }

    public void SetScale (Vector3 [] scale) {
        basicScale = new List<Vector3> ();
        for (int x = 0; x <= endPhase; x++) {
            basicScale.Add (scale [Mathf.Min (scale.Length - 1, x)]);
        }
    }

    public void SetLerpPosition (bool lerpPosition) {
        this.lerpPosition = new List<bool> ();
        for (int x = 0; x <= endPhase; x++) {
            this.lerpPosition.Add (lerpPosition);
        }
    }

    public void SetJumpAnimation (bool jumpAnimation) {
        this.jumpAnimation = new List<bool> ();
        for (int x = 0; x <= endPhase; x++) {
            this.jumpAnimation.Add (jumpAnimation);
        }
    }

    public void AddPhase () {
        if (phaseTimer != null) {
            phaseTimer.Add (phaseTimer [endPhase] * 2 - phaseTimer [endPhase - 1]);
        }
        if (basicColor != null) {
            basicColor.Add (basicColor [endPhase]);
        }
        if (basicPosition != null) {
            basicPosition.Add (basicPosition [endPhase]);
        }
        if (deltaPosition != null) {
            deltaPosition.Add (deltaPosition [endPhase]);
        }
        if (lerpPosition != null) {
            lerpPosition.Add (lerpPosition [endPhase]);
        }
        if (jumpAnimation != null) {
            jumpAnimation.Add (jumpAnimation [endPhase]);
        }
        if (basicScale != null) {
            basicScale.Add (basicScale [endPhase]);
        }
        if (drift != null) {
            drift.Add (drift [endPhase]);
        }
        endPhase++;
    }

    public void SetPhaseTimer (float duration) {
        phaseTimer = new List<float> ();
        phaseTimer.Add (0);
        for (int x = 1; x <= endPhase; x++) {
            phaseTimer.Add (duration + phaseTimer [x - 1]);
        }
    }

    public void SetPhaseTimer (int index, float duration) {
        phaseTimer [index] = phaseTimer [index - 1] + duration;
    }

    public void SetLastPhaseTimer (float duration) {
        SetPhaseTimer (endPhase, duration);
    }

    void Start () {
        AutoSetPosition ();
        if (basicScale == null) {
            SetScale (transform.localScale);
        }
        if (phaseTimer == null) {
            SetPhaseTimer (1);
        }
		renderers = GetComponentsInChildren<Renderer> ();
        UpdateEverything ();
	}

    public void UpdateColor () {
        if (basicColor != null && basicColor.Count > 0) {
            Color newColor;
            newColor = basicColor [currentPhase] * (1 - percentageTimer)
            + basicColor [Mathf.Min (currentPhase + 1, endPhase)] * percentageTimer;
            foreach (Renderer renderer in renderers) {
                if (triggered) {
                    renderer.material.color = newColor;
                } else {
                    renderer.material.color = new Color (0.25f, 0.25f, 0.25f, newColor.a) * 0.7f + newColor * 0.3f;
                }
            }
        }
    }

    public void UpdatePosition () {
        Vector3 newPosition;
        Vector3 Position1 = basicPosition [currentPhase];
        Vector3 Position2 = basicPosition [Mathf.Min (currentPhase + 1, endPhase)];
        if (deltaPosition != null) {
            Position1 += deltaPosition [currentPhase];
            Position2 += deltaPosition [Mathf.Min (currentPhase + 1, endPhase)];
        }

        float magnitude = (Position2 - Position1).magnitude;

        if (lerpPosition != null && lerpPosition [currentPhase]) {

            transform.localPosition = Vector3.Lerp (transform.localPosition, Position2, 0.15f);

        } else {

            newPosition = Position1 * (1 - percentageTimer) + Position2 * (percentageTimer);

            if (jumpAnimation != null && jumpAnimation [currentPhase]) {
                newPosition += new Vector3 (0, Mathf.Sin (percentageTimer * Mathf.PI) * magnitude, 0);
            }


            if (drift != null && drift [currentPhase]) {
                //Debug.Log ("Test");
                float heightModifier = Mathf.Abs (driftDestination - driftHeight) * 6;
                float driftSpeed = Mathf.Min ((1f + heightModifier) * Time.deltaTime, 1);
                driftDestination = Random.Range (-0.4f, 0.4f) * driftSpeed + driftDestination * (1f - driftSpeed);
                driftHeight = driftDestination * driftSpeed + driftHeight * (1f - driftSpeed);
                newPosition += Vector3.up * driftHeight;
            }
            transform.localPosition = newPosition;
        }
    }

    public void UpdateScale () {
        Vector3 newScale;
        Vector3 Scale1 = basicScale [currentPhase];
        Vector3 Scale2 = basicScale [Mathf.Min (currentPhase + 1, endPhase)];

        newScale = Scale1 * (1 - percentageTimer) + Scale2 * percentageTimer;

        transform.localScale = newScale;
    }

    public void UpdatePhase () {
        timer += Time.deltaTime;
        timer = Mathf.Min (timer, phaseTimer [endPhase] * timerScale);
        for (int x = currentPhase; x <= endPhase; x++) {
            if (timer / timerScale >= phaseTimer [x]) {
                currentPhase = x;
            }
        }
        if (currentPhase < endPhase) {
            percentageTimer = timer / timerScale - phaseTimer [currentPhase];
            percentageTimer /= phaseTimer [currentPhase + 1] - phaseTimer [currentPhase];
        } else {
            percentageTimer = 1;
        }
    }

    public void UpdateRotation () {
        if (rotateToCameraVertical) {
            Vector3 dPos = transform.position - Camera.main.transform.position;
            float atan = Mathf.Atan2 (dPos.y, dPos.x);
            transform.localEulerAngles = new Vector3 (0, atan * 180 / Mathf.PI + 90, 0);
        }
        if (rotateToCamera) {
            Vector3 dPos = transform.position - Camera.main.transform.position;
            Quaternion rotation = Quaternion.LookRotation (dPos, Vector3.up);
            transform.rotation = rotation;
        }
    }

	// Update is called once per frame
	void Update () {
        UpdateEverything ();
	}

    void UpdateEverything () {
        UpdatePhase ();
        UpdateColor ();
        UpdatePosition ();
        UpdateScale ();
        UpdateRotation ();
        if (currentPhase == endPhase) {
            if (destroyThisOnEnd) {
                GameObject.DestroyImmediate (this);
            }
            if (destroyOnEnd) {
                GameObject.DestroyImmediate (gameObject);
            }
        }
    }

    public void PushToHeight (float height) {
        AddPhase ();
        AutoSetPosition ();
        Vector3 pos = basicPosition [endPhase];
        pos = new Vector3 (pos.x, height, pos.z);
        basicPosition [endPhase] = pos;
    }
/*
    public void PushItDown (float value) {
        SPos += new Vector3 (0, -1, 0);
        FloatingHeight++;
    }*/
}
