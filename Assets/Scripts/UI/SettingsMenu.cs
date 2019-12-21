using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : GOUI {

    static public SettingsMenu Instance;

    static PageUI pageUI;

    static List<GameObject> garbage = new List<GameObject> ();

    static public Text bossNameText;
    static public Text bossDescriptionText;

    // Use this for initialization
    void Start () {
        Instance = this;
        AppSettings.RememberCurrentSettings ();
        CreateSettingsMenu ();
        CurrentSubGOUI = this;
        //DestroyTemplateButtons ();
    }

    override public void DestroyThis () {
        base.DestroyThis ();
        foreach (GameObject obj in garbage) {
            if (obj != null) {
                DestroyImmediate (obj);
            }
        }
    }

    static public void ShowSettingsMenu () {
        LoadSettingsMenu ();
    }

    static public void LoadSettingsMenu () {
        DestroySubMenu ();
        CurrentCanvas.AddComponent<SettingsMenu> ();
    }

    static public void CreateSettingsMenu () {

        GameObject BackgroundObject;
        GameObject Clone;
        UIController UIC;

        Clone = CreateUIImage ("UI/Transparent", 720, 540, 10000, 10000, false);
        garbage.Add (Clone);

        Clone = CreateUIImage ("UI/Panel_Window_01_Sliced", 720, 540, 900, 690,  false);
        Clone.GetComponent<Image> ().type = Image.Type.Sliced;
        DestroyImmediate (Clone.GetComponent<Collider> ());
        DestroyImmediate (Clone.GetComponent<UIController> ());
        BackgroundObject = Clone;
        garbage.Add (Clone);


        Clone = CreateUIText (Language.Settings, 600, 300, 500, 36);
        Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;
        garbage.Add (Clone);

        Clone = CreateUIText ("", 1015, 120, 500, 36);
        Text text = Clone.GetComponent<Text> ();
        text.alignment = TextAnchor.MiddleLeft;
        bossNameText = text;
        garbage.Add (Clone);


        Clone = CreateUIText ("", 1030, 955, 570, 24);
        text = Clone.GetComponent<Text> ();
        text.alignment = TextAnchor.UpperLeft;
        bossDescriptionText = text;
        garbage.Add (Clone);

        GameObject pageUIObject = new GameObject ();
        pageUI = pageUIObject.AddComponent<PageUI> ();

        for (int x = 0; x < 5; x++) {
            int py = 405 + x * 60;
            Clone = CreateUISlider (945, py, 120, 20);
            string s = "";
            Slider slider = Clone.GetComponent<Slider> ();
            switch (x) {
                case 0:
                    slider.onValueChanged.AddListener (Instance.ChangeMusicVolume);
                    slider.value = AppSettings.MusicVolume;
                    s = Language.MusicVolume;
                    break;
                case 1:
                    slider.onValueChanged.AddListener (Instance.ChangeSFXVolume);
                    slider.value = AppSettings.SFXVolume;
                    s = Language.SFXVolume;
                    break;
                case 2:
                    slider.onValueChanged.AddListener (Instance.ChangeAnimationsDurations);
                    slider.value = AppSettings.AnimationsDuration;
                    s = Language.AnimationDuration;
                    break;
                case 3:
                    slider.onValueChanged.AddListener (Instance.ChangeTimeBetweenTurns);
                    slider.value = AppSettings.TimeBetweenTurns;
                    s = Language.TimeBetweenTurns;
                    break;
                case 4:
                    slider.onValueChanged.AddListener (Instance.ChangePlayedCardDisplayDuration);
                    slider.value = AppSettings.PlayedCardPreviewDuration;
                    s = Language.PlayedCardDisplayTime;
                    break;
            }
            garbage.Add (Clone);
            Clone = CreateUIText (s, 555, py, 390, 24);
            Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;
            garbage.Add (Clone);
        }

        Clone = CreateUIButton ("UI/Butt_M_Apply", 720 - 330, 765, 90, 90, true);
        Clone.name = UIString.SettingsMenuApply;
        garbage.Add (Clone);

        Clone = CreateUIButton ("UI/Butt_M_Discard", 720 + 330, 765, 90, 90, true);
        Clone.name = UIString.SettingsMenuExit;
        garbage.Add (Clone);
    }

    public void ChangeMusicVolume (float value) {
        AppSettings.SetMusicVolume (value);
    }

    public void ChangeSFXVolume (float value) {
        AppSettings.SetSFXVolume (value);
    }

    public void ChangeAnimationsDurations (float value) {
        AppSettings.SetAnimationsDuration (value);
    }

    public void ChangeTimeBetweenTurns (float value) {
        AppSettings.SetTimeBetweenTurns (value);
    }

    public void ChangePlayedCardDisplayDuration (float value) {
        AppSettings.SetPlayedCardPreviewDuration (value);
    }

    public void Apply () {
        AppSettings.SaveSettings ();
        DestroySubMenu ();
    }

    public void Exit () {
        AppSettings.RestoreSettings ();
        DestroySubMenu ();
    }
}
