using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffectScript : MonoBehaviour {

    public bool [] Invisible = new bool [2];
    public bool [] BlendAppear = new bool [2];
	public bool [] BlendDisappear = new bool [2];
    public bool [] GrowAppear = new bool [2];
    public bool [] GrowDisappear = new bool [2];

    public bool [] Uprising = new bool [2];
	public bool [] Rotating = new bool [2];
	public bool [] Growing = new bool [2];
	public bool [] Moving = new bool [2];
	public bool [] Fluctuate = new bool [2];
	public bool [] Droping = new bool [2];
	public bool [] Twist = new bool [2];
    public bool [] Floating = new bool [2];

    public float FloatingHeight = 0f;
    public float FloatingDestination = 0f; 


	int Size;
	int Height;

	public bool Pushed;
	public bool Triggered;
	public bool AutoDestroy;
	public bool TurnedToCamera;
    public bool Colored;
	public Color Color;
	bool Extended;

	float alpha;
	float timer;
	float continuousTimer;
    float timerScale = AppSettings.AnimationDuration;

	Vector3 SSize;

    public bool PresetPosition;
	public Vector3 SPos;
	Vector3 EPos;

	public void PushIt (Vector3 destination) {
		Pushed = true;
		EPos = destination / transform.parent.lossyScale.x;
	}

	public void Init (Color color, bool autoDestroy, bool triggered) {
        if (color != null) {
            Colored = true;
        }
		Color = color;
		AutoDestroy = autoDestroy;
		Triggered = triggered;
	}

	Renderer [] renderers;

	private void Awake () {
	}

    public void SetStartingPosition (Vector3 startingPosition) {
        PresetPosition = true;
        SPos = startingPosition;
    }

	void Start () {
        if (!PresetPosition) {
            SPos = transform.localPosition;
        }
		SSize = transform.localScale;
		renderers = GetComponentsInChildren<Renderer> ();
		if (!AutoDestroy) {
			timerScale *= 0.5f;
		}
        if (Colored) {
            foreach (Renderer renderer in renderers) {
                if (Invisible [0] || BlendAppear [0]) {
                    alpha = 0;
                } else {
                    alpha = Color.a;
                }
                renderer.material.color = new Color (Color.r, Color.g, Color.b, Color.a);
                if (Droping [phase]) {
                    transform.localPosition = new Vector3 (SPos.x, SPos.y - AppDefaults.TokenSpawnHeight, SPos.z);
                } else {
                    transform.localPosition = new Vector3 (SPos.x, SPos.y, SPos.z);
                }
                if (Triggered) {
                    renderer.material.color = new Color (Color.r, Color.g, Color.b, alpha);
                } else {
                    renderer.material.color = new Color (0.75f, 0.75f, 0.75f, alpha);
                }
            }
        }
	}

	public bool CheckPhase () {
		if (Uprising [phase - 1]){
			Height++;
		}
		if (Growing [phase - 1]) {
			Size++;
		}
		return 
			BlendAppear [phase] ||
			BlendDisappear [phase] ||
			Uprising [phase] ||
			Rotating [phase] ||
			Growing [phase] ||
			Moving [phase] ||
			Invisible [phase] ||
			Twist [phase];
	}

	int phase = 0;
	bool uphold = true;

	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		continuousTimer += Time.deltaTime;
		if (AutoDestroy) {
			if (timer > timerScale) {
				timer -= timerScale;
				phase++;
				if (phase < 2){
					uphold = CheckPhase ();
				}
			}
			if (phase > 1) {
				Destroy (gameObject);
				return;
			} 
		} else {
			if (timer > timerScale){
				timer = timerScale;
			}
		}
		if (uphold) {
			if (Rotating [phase]) {
				transform.localEulerAngles = new Vector3 (0, -90f * continuousTimer / timerScale, 0);
			} else if (Fluctuate [phase]) {
				transform.localEulerAngles = new Vector3 (0, 20f * Mathf.Sin (continuousTimer / timerScale), 0);
			} else if (Twist [phase]) {
				transform.localEulerAngles = new Vector3 (0, -30f * (1 - Mathf.Min (continuousTimer / timerScale, 1)), 0);
			}
			if (TurnedToCamera) {
				transform.localEulerAngles = new Vector3 (270, 0, 0);
				//transform.LookAt (CameraScript.Camera.transform);
			}
            if (Colored) {
                foreach (Renderer renderer in renderers) {
                    if (timer < timerScale) {
                        if (Invisible [phase]) {
                            alpha = 0;
                        } else if (BlendAppear [phase]) {
                            alpha = Color.a * timer / timerScale;
                        } else if (BlendDisappear [phase] && AutoDestroy) {
                            alpha = Color.a - Color.a * timer / timerScale;
                        } else {
                            alpha = Color.a;
                        }
                    } else {
                        alpha = Color.a;
                    }
                    if (Triggered) {
                        renderer.material.color = new Color (Color.r, Color.g, Color.b, alpha);
                    } else {
                        renderer.material.color = new Color (0.75f, 0.75f, 0.75f, alpha);
                    }
                }
            }
            if (GrowAppear [phase]) {
                float x = SSize.x * (timer / timerScale);
                transform.localScale = new Vector3 (x, x, x);
            }
            if (Growing [phase]) {
                float x = SSize.x + (Size + timer / timerScale) / 2;
                transform.localScale = new Vector3 (x, x, x);
            }
            float PosX = EPos.x * Mathf.Sin (Mathf.Min (continuousTimer / timerScale, 1) * Mathf.PI / 2) + SPos.x;
            float PosY = EPos.y * Mathf.Sin (Mathf.Min (continuousTimer / timerScale, 1) * Mathf.PI / 2) + SPos.y;
            if (Uprising [phase]) {
                transform.localPosition = new Vector3 (PosX, PosY + 0.75f * (Height + timer / timerScale), SPos.z);
            } else if (Droping [phase]) {
                transform.localPosition = new Vector3 (PosX, PosY + 0.75f * Height - AppDefaults.TokenSpawnHeight * (1 - timer / timerScale), SPos.z);
            } else {
                transform.localPosition = new Vector3 (PosX, PosY + 0.75f * Height, SPos.z);
            }
            if (Floating [phase]) {
                float HeighModifier = Mathf.Abs (FloatingDestination - FloatingHeight) * 6;
                float floatingSpeed = Mathf.Min ((1f + HeighModifier) * Time.deltaTime, 1);
                FloatingDestination = Random.Range (-0.4f, 0.4f) * floatingSpeed + FloatingDestination * (1f - floatingSpeed);
                FloatingHeight = FloatingDestination * floatingSpeed + FloatingHeight * (1f - floatingSpeed);
                transform.Translate (Vector3.up * FloatingHeight);
            }

        } else if (AutoDestroy) {
			Destroy (gameObject);
		}
	}

    public void PushItToHeight (float value) {
        float delta = value - SPos.y;
        //SPos = new Vector3 (SPos.x, value, SPos.z);
        SPos += new Vector3 (0, delta, 0);
        //PresetPosition = true;
        FloatingHeight -= delta;
    }
/*
    public void PushItDown (float value) {
        SPos += new Vector3 (0, -1, 0);
        FloatingHeight++;
    }*/
}
