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
    public AudioClip [] GetAbilitySound (AbilityType abilityType) {
        List <AudioClip> output = new List<AudioClip>();
        switch (abilityType) {
            case AbilityType.T47:
                output.Add (AbilityTokenSacrifice);
                break;
            case AbilityType.T2:
                output.Add (AbilityTokenCreation);
                break;
        }

        switch (abilityType) {
            case AbilityType.T20:
            case AbilityType.T62:
                output.Add (ShufflingCard);
                break;

            case AbilityType.T6:
            case AbilityType.T40:
                    output.Add (AbilityOther1);
                break;
            //       output.Add (AbilityOther2);
            //       output.Add (AbilityOther3);
            case AbilityType.T7:
                output.Add (AbilityEmptyEffect);
                break;

            case AbilityType.T3:
            case AbilityType.T59:
            case AbilityType.T60:
                output.Add (AbilityTileDisable);
                break;
                

            case AbilityType.T38:
            case AbilityType.T42:
            case AbilityType.T49:
            case AbilityType.T51:
            case AbilityType.T71:
                output.Add (AbilityCardChange);
                break;
            case AbilityType.T45:
            case AbilityType.T61:
            case AbilityType.T72:
                output.Add (AbilityCardChangeRepeated);
                break;
                output.Add (AbilityGettingACard);
            
                output.Add (AbilityOwnScoreLose);
            case AbilityType.T4:
            case AbilityType.T28:
            case AbilityType.T47:
                output.Add (AbilityOwnScoreGain);
                break;
            case AbilityType.T46:
            case AbilityType.T70:
                output.Add (AbilityEnemyScoreChange);
                break;

            case AbilityType.T1:
            case AbilityType.T8:
            case AbilityType.T12:
            case AbilityType.T21:
            case AbilityType.T24:
            case AbilityType.T41:
            case AbilityType.T44:
            case AbilityType.T57:
                output.Add (AbilityTokenValueReduction);
                break;
            case AbilityType.T14:
            case AbilityType.T29:
            case AbilityType.T30:
            case AbilityType.T35:
            case AbilityType.T63:
            case AbilityType.T67:
            case AbilityType.T69:
                output.Add (AbilityTokenDestruction);
                break;
            case AbilityType.T2:
            case AbilityType.T9:
            case AbilityType.T27:
            case AbilityType.T32:
            case AbilityType.T52:
            case AbilityType.T54:
            case AbilityType.T68:
            case AbilityType.T74:
                output.Add (AbilityTokenValueGain);
                break;
            case AbilityType.T13:
            case AbilityType.T26:
            case AbilityType.T43:
                output.Add (AbilityTokenValueChange);
                break;
            case AbilityType.T25:
            case AbilityType.T31:
            case AbilityType.T48:
                output.Add (AbilityTokenCreation);
                break;
            case AbilityType.T15:
            case AbilityType.T23:
            case AbilityType.T55:
            case AbilityType.T56:
            case AbilityType.T65:
                output.Add (AbilityTokenValueSteal);
                break;
            case AbilityType.T18:
            case AbilityType.T39:
            case AbilityType.T53:
            case AbilityType.T73:
                output.Add (AbilityTokenCloning);
                break;
            case AbilityType.T10:
            case AbilityType.T16:
            case AbilityType.T17:
            case AbilityType.T22:
            case AbilityType.T33:
            case AbilityType.T37:
            case AbilityType.T58:
                output.Add (AbilityTokenTypeChange);
                break;
            case AbilityType.T50:
            case AbilityType.T64:
                output.Add (AbilityTokenOwnerChange);
                break;
            case AbilityType.T5:
            case AbilityType.T34:
                output.Add (AbilityTokenPush);
                break;
            case AbilityType.T11:
            case AbilityType.T19:
            case AbilityType.T36:
            case AbilityType.T66:
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
