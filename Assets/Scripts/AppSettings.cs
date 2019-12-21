using UnityEngine;

public class AppSettings :MonoBehaviour {

    static string MusicVolumeKey = "MusicVolume";
    static string SFXVolumeKey = "SFXVolume";
    static string TimeBetweenTurnsKey = "TimeBetweenTurns";
    static string AnimationsDurationKey = "AnimationsDuration";
    static string PlayedCardPreviewDurationKey = "PlayedCardPreviewDuration";

    static public float MusicVolume = 0.5f;
    static public float SFXVolume = 0.5f;
    static public float TimeBetweenTurns = 0.5f;
    static public float AnimationsDuration = 0.5f;
    static public float PlayedCardPreviewDuration = 0.5f;

    static public float PrevMusicVolume = 0.5f;
    static public float PrevSFXVolume = 0.5f;
    static public float PrevTimeBetweenTurns = 0.5f;
    static public float PrevAnimationsDuration = 0.5f;
    static public float PrevPlayedCardPreviewDuration = 0.5f;
    

    private void Awake () {
        LoadSettings ();
    }

    static void LoadSettings () {
        Debug.Log ("Loading settings");
        if (PlayerPrefs.HasKey (MusicVolumeKey)) {
            SetMusicVolume (PlayerPrefs.GetFloat (MusicVolumeKey));
        }
        if (PlayerPrefs.HasKey (SFXVolumeKey)) {
            SetSFXVolume (PlayerPrefs.GetFloat (SFXVolumeKey));
        }
        if (PlayerPrefs.HasKey (TimeBetweenTurnsKey)) {
            SetTimeBetweenTurns (PlayerPrefs.GetFloat (TimeBetweenTurnsKey));
        }
        if (PlayerPrefs.HasKey (AnimationsDurationKey)) {
            SetAnimationsDuration (PlayerPrefs.GetFloat (AnimationsDurationKey));
        }
        if (PlayerPrefs.HasKey (PlayedCardPreviewDurationKey)) {
            SetPlayedCardPreviewDuration (PlayerPrefs.GetFloat (PlayedCardPreviewDurationKey));
        }
    }

    static public float GetAnimationsDuration () {
        return 0.25f + AnimationsDuration * 0.5f;
    }

    static public void RememberCurrentSettings () {
        PrevMusicVolume = MusicVolume;
        PrevSFXVolume = SFXVolume;
        PrevTimeBetweenTurns = TimeBetweenTurns;
        PrevAnimationsDuration = AnimationsDuration;
        PrevPlayedCardPreviewDuration = PlayedCardPreviewDuration;
    }

    static public void RestoreSettings () {
        SetMusicVolume (PrevMusicVolume);
        SetSFXVolume (PrevSFXVolume);
        SetTimeBetweenTurns (PrevTimeBetweenTurns);
        SetAnimationsDuration (PrevAnimationsDuration);
        SetPlayedCardPreviewDuration (PrevPlayedCardPreviewDuration);
    }

    static public void SaveSettings () {
        PlayerPrefs.SetFloat (MusicVolumeKey, MusicVolume);
        PlayerPrefs.SetFloat (SFXVolumeKey, SFXVolume);
        PlayerPrefs.SetFloat (TimeBetweenTurnsKey, TimeBetweenTurns);
        PlayerPrefs.SetFloat (AnimationsDurationKey, AnimationsDuration);
        PlayerPrefs.SetFloat (PlayedCardPreviewDurationKey, PlayedCardPreviewDuration);
    }

    static public void SetMusicVolume (float value) {
        MusicVolume = value;
        SoundManager.SetMusicVolume (value);
    }

    static public void SetSFXVolume (float value) {
        SFXVolume = value;
        SoundManager.SetSFXVolume (value);
    }

    static public void SetTimeBetweenTurns (float value) {
        TimeBetweenTurns = value;
    }

    static public void SetAnimationsDuration (float value) {
        AnimationsDuration = value;
    }

    static public void SetPlayedCardPreviewDuration (float value) {
        PlayedCardPreviewDuration = value;
    }
}
