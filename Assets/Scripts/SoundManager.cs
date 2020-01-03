using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    static SoundManager Instance;
    public AudioSource SoundtrackSource;
    public AudioSource SoundsSource;

    private void OnEnable () {
        DontDestroyOnLoad (transform.root);
        Instance = this;
        SetMusicVolume (AppSettings.MusicVolume);
        SetSFXVolume (AppSettings.SFXVolume);
    }

    private void Start () {
        SoundtrackSource.clip = SoundResourcesManager.Instance.Soundtrack;
        SoundtrackSource.Play ();
    }

    public void Update () {
        if (Random.Range (0, 10) == 0) {
            //SoundtrackSource.PlayOneShot (SoundResourcesManager.Instance.AbilityTokenValueReduction);
        }
    }

    static public void SetMusicVolume (float value) {
        if (!Instance) {
            return;
        }
        Instance.SoundtrackSource.volume = value;
    }
    static public void SetSFXVolume (float value) {
        if (!Instance) {
            return;
        }
        Instance.SoundsSource.volume = value;
    }

    static public void PlayAudioClip (MyAudioClip soundClip) {
        float volume = SoundResourcesManager.Instance.GetSoundVolume (soundClip);
        Instance.SoundsSource.PlayOneShot (SoundResourcesManager.Instance.GetSound (soundClip), volume);
    }

    static List<MyAudioClip> queuedClips = new List<MyAudioClip> ();

    static public void AddAudioClipToQueue (MyAudioClip soundClip) {
        if (queuedClips.Contains (soundClip)){
            return;
        }
        queuedClips.Add (soundClip);
    }

    static public void PlayQueuedAudioClips () {
        foreach (MyAudioClip soundClip in queuedClips) {
            Instance.StartCoroutine (Instance.PlayAudioClipWithDelay (soundClip));
        }
        queuedClips.Clear ();
    }

    static public void PlayAbilityAudioClips (AbilityType abilityType) {
        PlayAudioClips (SoundResourcesManager.Instance.GetAbilitySound (abilityType));
    }

    static public void PlayAudioClips (AudioClip [] audioClips) {
        foreach (AudioClip audioClip in audioClips) {
            Instance.StartCoroutine (Instance.PlayAudioClipWithDelay (audioClip));
        }
    }

    public IEnumerator PlayAudioClipWithDelay (MyAudioClip soundClip) {
        yield return new WaitForSeconds (VisualMatch.GlobalTimer);
        float volume = SoundResourcesManager.Instance.GetSoundVolume (soundClip);
        SoundsSource.PlayOneShot (SoundResourcesManager.Instance.GetSound (soundClip), volume);
    }

    public IEnumerator PlayAudioClipWithDelay (AudioClip audioClip) {
        yield return new WaitForSeconds (VisualMatch.GlobalTimer);
        SoundsSource.PlayOneShot (audioClip);
    }


}
