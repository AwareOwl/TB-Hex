using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MyAudioClip {
    OnButtonOver,
    OnButtonClick,
    WinWindow,
    LoseWindow,
    ContentUnlocked,
    Error,
    SetEditorCardAdd,
    SetEditorCardRemove,


    MatchStart,
    TurnNumberLimit,
    TurnTimeLimit,
    QuestFinished,
    MatchEvent,

    OnTileOver,
    TokenPlayAlly,
    TokenPlayEnemy,
    EmptyTileInvalidMove,
    TriggerTokenTile,
    TriggerTokenInvalidMove,
    TriggerObject,
    TriggerObjectInvalid,
    TokenDestruction,
    TokenPositiveAbility,
    TokenNegativeAbility,
    TokenOtherAbility,
    TokenPositiveRegularAbility,
    TokenNegativeRegularAbility,
    TokenPositiveExplosion,
    TokenNegativeExplosion,
    TokenTrigger,

    Mulligan,
    SetClick,
    ShufflingCard,
    UsedCardAppear,
    UsedCardDisappear,
    SetPreview,

    AbilityOther1,
    AbilityOther2,
    AbilityOther3,
    AbilityEmptyEffect,
    AbilityTileDisable,
    AbilityTokenConditionMeet,

    AbilityCardChange,
    AbilityCardChangeRepeated,
    AbilityGettingACard,

    AbilityOwnScoreLose,
    AbilityOwnScoreGain,
    AbilityEnemyScoreChange,

    AbilityTokenValueReduction,
    AbilityTokenDestruction,
    AbilityTokenSacrifice,
    AbilityTokenValueGain,
    AbilityTokenValueChange,
    AbilityTokenCreation,
    AbilityTokenValueSteal,
    AbilityTokenCloning,
    AbilityTokenTypeChange,
    AbilityTokenOwnerChange,
    AbilityTokenPush,
    AbilityTokenPositionSwap,
    AbilityTokenGrab
}

public class SoundResourcesManager : MonoBehaviour {

    static public SoundResourcesManager Instance;

    public AudioClip Soundtrack;

    [Header ("Application")]
    public AudioClip OnButtonOver1;
    public AudioClip OnButtonOver2;
    public AudioClip OnButtonClick;
    public AudioClip WinWindow;
    public AudioClip LoseWindow;
    public AudioClip ContentUnlocked;
    public AudioClip Error;
    public AudioClip SetEditorCardAdd;
    public AudioClip SetEditorCardRemove;


    [Header ("In-Game")]
    public AudioClip MatchStart;
    public AudioClip TurnNumberLimit;
    public AudioClip TurnTimeLimit;
    public AudioClip QuestFinished;
    public AudioClip MatchEvent;

    [Header ("Tile/Token")]
    public AudioClip [] OnTileOver = new AudioClip [0];
    public AudioClip [] TokenPlayAlly = new AudioClip [0];
    public AudioClip [] TokenPlayEnemy = new AudioClip [0];
    public AudioClip EmptyTileInvalidMove;
    public AudioClip TriggerTokenTile;
    public AudioClip TriggerTokenInvalidMove;
    public AudioClip TriggerObject;
    public AudioClip TriggerObjectInvalid;
    public AudioClip TokenDestruction;
    public AudioClip TokenPositiveAbility;
    public AudioClip TokenNegativeAbility;
    public AudioClip TokenOtherAbility;
    public AudioClip TokenPositiveRegularAbility;
    public AudioClip TokenNegativeRegularAbility;
    public AudioClip TokenPositiveExplosion;
    public AudioClip TokenNegativeExplosion;
    public AudioClip TokenTrigger;


    [Header ("Cards")]
    public AudioClip Mulligan;
    public AudioClip SetClick;
    public AudioClip ShufflingCard;
    public AudioClip UsedCardAppear;
    public AudioClip UsedCardDisappear;
    public AudioClip SetPreview;

    [Header ("Ability")]
    public AudioClip AbilityOther1;
    public AudioClip AbilityOther2;
    public AudioClip AbilityOther3;
    public AudioClip AbilityEmptyEffect;
    public AudioClip AbilityTileDisable;
    public AudioClip AbilityTokenConditionMeet;

    [Header ("Ability -> Card")]
    public AudioClip AbilityCardChange;
    public AudioClip AbilityCardChangeRepeated;
    public AudioClip AbilityGettingACard;

    [Header ("Ability -> Player")]
    public AudioClip AbilityOwnScoreLose;
    public AudioClip AbilityOwnScoreGain;
    public AudioClip AbilityEnemyScoreChange;
    
    [Header ("Ability -> Token")]
    public AudioClip AbilityTokenValueReduction;
    public AudioClip AbilityTokenDestruction;
    public AudioClip AbilityTokenSacrifice;
    public AudioClip AbilityTokenValueGain;
    public AudioClip AbilityTokenValueChange;
    public AudioClip AbilityTokenCreation;
    public AudioClip AbilityTokenValueSteal;
    public AudioClip AbilityTokenCloning;
    public AudioClip AbilityTokenTypeChange;
    public AudioClip AbilityTokenOwnerChange;
    public AudioClip AbilityTokenPush;
    public AudioClip AbilityTokenPositionSwap;
    public AudioClip AbilityTokenGrab;

    // Use this for initialization
    void OnEnable () {
        Instance = this;
        //Debug.Log (SoundResourcesManager.Instance.Soundtrack);
    }

    int OnButtonOverCounter = 0;
    int OnTileOverCounter = 0;

    public float GetSoundVolume (MyAudioClip soundClip) {
        switch (soundClip) {
            case MyAudioClip.ShufflingCard:
                return 0.05f;
            case MyAudioClip.SetClick:
                return 0.8f;
            case MyAudioClip.OnTileOver:
                return 0.05f;
            case MyAudioClip.OnButtonOver:
                return 0.4f;
            case MyAudioClip.OnButtonClick:
                return 0.8f;
            case MyAudioClip.MatchStart:
                return 0.25f;
            case MyAudioClip.TokenTrigger:
                return 0.6f;
            case MyAudioClip.TokenNegativeAbility:
                return 0.8f;
            case MyAudioClip.TokenPositiveAbility:
                return 0.8f;
            case MyAudioClip.TokenNegativeRegularAbility:
                return 0.1f;
            case MyAudioClip.TokenPositiveRegularAbility:
                return 0.1f;
            default:
                return 1;
        }
    }

    public AudioClip GetSound (MyAudioClip soundClip) {
        switch (soundClip) {

            case MyAudioClip.OnButtonOver:
                if (OnButtonOverCounter == 0) {
                    OnButtonOverCounter = 1;
                    return OnButtonOver1;
                } else {
                    OnButtonOverCounter = 0;
                    return OnButtonOver2;
                }
            case MyAudioClip.OnButtonClick:
                return OnButtonClick;
            case MyAudioClip.WinWindow:
                return WinWindow;
            case MyAudioClip.LoseWindow:
                return LoseWindow;
            case MyAudioClip.ContentUnlocked:
                return ContentUnlocked;
            case MyAudioClip.Error:
                return Error;
            case MyAudioClip.SetEditorCardAdd:
                return SetEditorCardAdd;
            case MyAudioClip.SetEditorCardRemove:
                return SetEditorCardRemove;

            case MyAudioClip.MatchStart:
                return MatchStart;
            case MyAudioClip.TurnNumberLimit:
                return TurnNumberLimit;
            case MyAudioClip.TurnTimeLimit:
                return TurnTimeLimit;
            case MyAudioClip.QuestFinished:
                return QuestFinished;
            case MyAudioClip.MatchEvent:
                return MatchEvent;

            case MyAudioClip.OnTileOver:
                OnTileOverCounter++;
                OnTileOverCounter %= OnTileOver.Length;
                return OnTileOver [OnTileOverCounter];
            case MyAudioClip.TokenPlayAlly:
                return TokenPlayAlly [Random.Range (0, TokenPlayAlly.Length)];
            case MyAudioClip.TokenPlayEnemy:
                return TokenPlayEnemy [Random.Range (0, TokenPlayEnemy.Length)];
            case MyAudioClip.EmptyTileInvalidMove:
                return EmptyTileInvalidMove;
            case MyAudioClip.TriggerTokenTile:
                return TriggerTokenTile;
            case MyAudioClip.TriggerTokenInvalidMove:
                return TriggerTokenInvalidMove;
            case MyAudioClip.TriggerObject:
                return TriggerObject;
            case MyAudioClip.TriggerObjectInvalid:
                return TriggerObjectInvalid;
            case MyAudioClip.TokenDestruction:
                return TokenDestruction;
            case MyAudioClip.TokenPositiveAbility:
                return TokenPositiveAbility;
            case MyAudioClip.TokenNegativeAbility:
                return TokenNegativeAbility;
            case MyAudioClip.TokenOtherAbility:
                return TokenOtherAbility;
            case MyAudioClip.TokenPositiveRegularAbility:
                return TokenPositiveRegularAbility;
            case MyAudioClip.TokenNegativeRegularAbility:
                return TokenNegativeRegularAbility;
            case MyAudioClip.TokenPositiveExplosion:
                return TokenPositiveExplosion;
            case MyAudioClip.TokenNegativeExplosion:
                return TokenNegativeExplosion;
            case MyAudioClip.TokenTrigger:
                return TokenTrigger;

            case MyAudioClip.Mulligan:
                return Mulligan;
            case MyAudioClip.SetClick:
                return SetClick;
            case MyAudioClip.ShufflingCard:
                return ShufflingCard;
            case MyAudioClip.UsedCardAppear:
                return UsedCardAppear;
            case MyAudioClip.UsedCardDisappear:
                return UsedCardDisappear;
            case MyAudioClip.SetPreview:
                return SetPreview;

            case MyAudioClip.AbilityOther1:
                return AbilityOther1;
            case MyAudioClip.AbilityOther2:
                return AbilityOther2;
            case MyAudioClip.AbilityOther3:
                return AbilityOther3;
            case MyAudioClip.AbilityEmptyEffect:
                return AbilityEmptyEffect;
            case MyAudioClip.AbilityTileDisable:
                return AbilityTileDisable;
            case MyAudioClip.AbilityTokenConditionMeet:
                return AbilityTokenConditionMeet;

            case MyAudioClip.AbilityCardChange:
                return AbilityCardChange;
            case MyAudioClip.AbilityCardChangeRepeated:
                return AbilityCardChangeRepeated;
            case MyAudioClip.AbilityGettingACard:
                return AbilityGettingACard;

            case MyAudioClip.AbilityOwnScoreLose:
                return AbilityOwnScoreLose;
            case MyAudioClip.AbilityOwnScoreGain:
                return AbilityOwnScoreGain;
            case MyAudioClip.AbilityEnemyScoreChange:
                return AbilityEnemyScoreChange;

            case MyAudioClip.AbilityTokenValueReduction:
                return AbilityTokenValueReduction;
            case MyAudioClip.AbilityTokenDestruction:
                return AbilityTokenDestruction;
            case MyAudioClip.AbilityTokenSacrifice:
                return AbilityTokenSacrifice;
            case MyAudioClip.AbilityTokenValueGain:
                return AbilityTokenValueGain;
            case MyAudioClip.AbilityTokenValueChange:
                return AbilityTokenValueChange;
            case MyAudioClip.AbilityTokenCreation:
                return AbilityTokenCreation;
            case MyAudioClip.AbilityTokenValueSteal:
                return AbilityTokenValueSteal;
            case MyAudioClip.AbilityTokenCloning:
                return AbilityTokenCloning;
            case MyAudioClip.AbilityTokenTypeChange:
                return AbilityTokenTypeChange;
            case MyAudioClip.AbilityTokenOwnerChange:
                return AbilityTokenOwnerChange;
            case MyAudioClip.AbilityTokenPush:
                return AbilityTokenPush;
            case MyAudioClip.AbilityTokenPositionSwap:
                return AbilityTokenPositionSwap;
            case MyAudioClip.AbilityTokenGrab:
                return AbilityTokenGrab;

            default:
                return null;
        }
    }
    public AudioClip [] GetAbilitySound (int abilityType) {
        List <AudioClip> output = new List<AudioClip>();
        switch (abilityType) {
            case 47:
                output.Add (AbilityTokenSacrifice);
                break;
            case 2:
                output.Add (AbilityTokenCreation);
                break;
        }

        switch (abilityType) {
            case 20:
            case 62:
                output.Add (ShufflingCard);
                break;

            case 6:
            case 40:
                    output.Add (AbilityOther1);
                break;
            //       output.Add (AbilityOther2);
            //       output.Add (AbilityOther3);
            case 7:
                output.Add (AbilityEmptyEffect);
                break;

            case 3:
            case 59:
            case 60:
                output.Add (AbilityTileDisable);
                break;
                

            case 38:
            case 42:
            case 49:
            case 51:
            case 71:
                output.Add (AbilityCardChange);
                break;
            case 45:
            case 61:
                output.Add (AbilityCardChangeRepeated);
                break;
                output.Add (AbilityGettingACard);
            
                output.Add (AbilityOwnScoreLose);
            case 4:
            case 28:
            case 47:
                output.Add (AbilityOwnScoreGain);
                break;
            case 46:
            case 70:
                output.Add (AbilityEnemyScoreChange);
                break;

            case 1:
            case 8:
            case 12:
            case 21:
            case 24:
            case 41:
            case 44:
            case 57:
                output.Add (AbilityTokenValueReduction);
                break;
            case 14:
            case 29:
            case 30:
            case 35:
            case 63:
            case 67:
            case 69:
                output.Add (AbilityTokenDestruction);
                break;
            case 2:
            case 9:
            case 27:
            case 32:
            case 52:
            case 54:
            case 68:
                output.Add (AbilityTokenValueGain);
                break;
            case 13:
            case 26:
            case 43:
                output.Add (AbilityTokenValueChange);
                break;
            case 25:
            case 31:
            case 48:
                output.Add (AbilityTokenCreation);
                break;
            case 15:
            case 23:
            case 55:
            case 56:
            case 65:
                output.Add (AbilityTokenValueSteal);
                break;
            case 18:
            case 39:
            case 53:
                output.Add (AbilityTokenCloning);
                break;
            case 10:
            case 16:
            case 17:
            case 22:
            case 33:
            case 37:
            case 58:
                output.Add (AbilityTokenTypeChange);
                break;
            case 50:
            case 64:
                output.Add (AbilityTokenOwnerChange);
                break;
            case 5:
            case 34:
                output.Add (AbilityTokenPush);
                break;
            case 11:
            case 19:
            case 36:
            case 66:
                output.Add (AbilityTokenPositionSwap);
                break;
            //case 50:
                //output.Add (AbilityTokenGrab);
                break;
        }

        return output.ToArray ();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
