using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeSettingsEditor : GOUI {

    static Toggle hasScoreLimitToggle;
    static Toggle hasTurnLimitToggle;
    static Toggle isAllowedToRotateCardsToggle;

    static InputField scoreInputField;
    static InputField turnInputField;
    static InputField numberOfStacksInputField;
    static InputField minimumNumberOfCardsPerStackInputField;

    static bool editMode;

    static bool hasScoreWinCondition;
    static int scoreWinConditionValue;
    static bool hasTurnWinCondition;
    static int turnWinConditionValue;
    static bool isAllowedToRotateCardsDuringMatch;
    static int numberOfStacks;
    static int minimumNumberOfCardsInStack;

    static List<GameObject> Garbage = new List<GameObject> ();

    public override void DestroyThis () {
        if (Garbage != null) {
            foreach (GameObject obj in Garbage) {
                if (obj != null) {
                    DestroyImmediate (obj);
                }
            }
        }
    }

    // Use this for initialization
    void Awake () {
        CreateGameModeSettingsEditor ();
        CurrentGUI = this;
    }

    static public void ShowGameModeSettingsEditor () {
        ClientLogic.MyInterface.CmdDownloadGameModeSettings (GameModeEditor.gameModeId);
    }

    static public void ShowGameModeSettingsEditor (bool isClientOwner, bool hasScoreWinCondition, int scoreWinConditionValue, bool hasTurnWinCondition, int turnWinConditionValue,
        bool isAllowedToRotateCardsDuringMatch, int numberOfStacks, int minimumNumberOfCardsInStack) {

        editMode = isClientOwner;
        GameModeSettingsEditor.hasScoreWinCondition = hasScoreWinCondition;
        GameModeSettingsEditor.scoreWinConditionValue = scoreWinConditionValue;
        GameModeSettingsEditor.hasTurnWinCondition = hasTurnWinCondition;
        GameModeSettingsEditor.turnWinConditionValue = turnWinConditionValue;
        GameModeSettingsEditor.isAllowedToRotateCardsDuringMatch = isAllowedToRotateCardsDuringMatch;
        GameModeSettingsEditor.numberOfStacks = numberOfStacks;
        GameModeSettingsEditor.minimumNumberOfCardsInStack = minimumNumberOfCardsInStack;

        DestroyMenu ();
        CurrentCanvas.AddComponent<GameModeSettingsEditor> ();
    }

    static public void SaveGameModeSettings () {

        hasScoreWinCondition = hasScoreLimitToggle.isOn;
        scoreWinConditionValue = int.Parse (scoreInputField.text);
        hasTurnWinCondition = hasTurnLimitToggle.isOn;
        turnWinConditionValue = int.Parse (turnInputField.text);
        isAllowedToRotateCardsDuringMatch = isAllowedToRotateCardsToggle.isOn;
        numberOfStacks = int.Parse (numberOfStacksInputField.text);
        minimumNumberOfCardsInStack = int.Parse (minimumNumberOfCardsPerStackInputField.text);

        ClientLogic.MyInterface.CmdSaveGameModeSettings (GameModeEditor.gameModeId,
            hasScoreWinCondition, scoreWinConditionValue, hasTurnWinCondition, turnWinConditionValue,
            isAllowedToRotateCardsDuringMatch, numberOfStacks, minimumNumberOfCardsInStack);
        GameModeEditor.ShowGameModeEditor ();
    }

    static public void CreateGameModeSettingsEditor () {
        GameObject Clone;
        InputField inputField;

        Clone = CreateSprite ("UI/Panel_Window_01_Sliced", 720, 540, 10, 1200, 660, false);

        Clone = CreateUIText (Language.GameModeSettings + ":", 720, 300, 520, 36);

        Clone = CreateUIToggle (Language.EndMatchAfterReachingScoreLimit, 690, 390, 900, 36);
        hasScoreLimitToggle = Clone.GetComponent<Toggle> ();
        hasScoreLimitToggle.isOn = hasScoreWinCondition;
        if (!editMode) {
            hasScoreLimitToggle.interactable = false;
        }
        //Garbage.Add (Clone);

        Clone = CreateInputField ("", 1200, 390, 90, 60);
        inputField = Clone.GetComponent<InputField> ();
        inputField.text = scoreWinConditionValue.ToString ();
        inputField.contentType = InputField.ContentType.IntegerNumber;
        inputField.characterLimit = 4;
        scoreInputField = inputField;
        scoreInputField.onValueChanged.AddListener (delegate {
            int value;
            int.TryParse (scoreInputField.text, out value);
            if (value < 1) {
                scoreInputField.text = "1";
            }
        });
        if (!editMode) {
            scoreInputField.interactable = false;
        }
        Garbage.Add (Clone);

        Clone = CreateUIToggle (Language.EndMatchAfterReachingTurnLimit, 690, 450, 900, 36);
        hasTurnLimitToggle = Clone.GetComponent<Toggle> ();
        hasTurnLimitToggle.isOn = hasTurnWinCondition;
        if (!editMode) {
            hasTurnLimitToggle.interactable = false;
        }
        //Garbage.Add (Clone);

        Clone = CreateInputField ("", 1200, 450, 90, 60);
        inputField = Clone.GetComponent<InputField> ();
        inputField.text = turnWinConditionValue.ToString ();
        inputField.contentType = InputField.ContentType.IntegerNumber;
        inputField.characterLimit = 3;
        turnInputField = inputField;
        turnInputField.onValueChanged.AddListener (delegate {
            int value;
            int.TryParse (turnInputField.text, out value);
            if (value < 1) {
                turnInputField.text = "1";
            }
        });
        if (!editMode) {
            turnInputField.interactable = false;
        }
        Garbage.Add (Clone);

        Clone = CreateUIToggle (Language.AllowToRotateAbilityAreaDuringMatch, 690, 510, 900, 36);
        isAllowedToRotateCardsToggle = Clone.GetComponent<Toggle> ();
        isAllowedToRotateCardsToggle.isOn = isAllowedToRotateCardsDuringMatch;
        if (!editMode) {
            isAllowedToRotateCardsToggle.interactable = false;
        }

        /*
        Clone = CreateInputField ("40", 1200, 570, 90, 60);
        inputField = Clone.GetComponent<InputField> ();
        inputField.contentType = InputField.ContentType.IntegerNumber;
        inputField.characterLimit = 3;
        TurnInputField = inputField;
        TurnInputField.onValueChanged.AddListener (delegate {
            int value;
            int.TryParse (TurnInputField.text, out value);
            if (value < 1) {
                TurnInputField.text = "1";
            }
        });*/

        Clone = CreateUIText (Language.NumberOfStacks, 720, 570, 900, 30);
        Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;
        Garbage.Add (Clone);

        Clone = CreateInputField ("", 1200, 570, 90, 60);
        inputField = Clone.GetComponent<InputField> ();
        inputField.text = numberOfStacks.ToString ();
        inputField.contentType = InputField.ContentType.IntegerNumber;
        inputField.characterLimit = 3;
        numberOfStacksInputField = inputField;
        numberOfStacksInputField.onValueChanged.AddListener (delegate {
            int value;
            int.TryParse (numberOfStacksInputField.text, out value);
            if (value < 1) {
                numberOfStacksInputField.text = "1";
            }
            if (value > 4) {
                numberOfStacksInputField.text = "4";
            }
        });
        if (!editMode) {
            numberOfStacksInputField.interactable = false;
        }
        Garbage.Add (Clone);


        Clone = CreateUIText (Language.MinimumNumberOfCardsInStack, 720, 630, 900, 30);
        Clone.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;
        Garbage.Add (Clone);

        Clone = CreateInputField ("", 1200, 630, 90, 60);
        inputField = Clone.GetComponent<InputField> ();
        inputField.text = minimumNumberOfCardsInStack.ToString ();
        inputField.contentType = InputField.ContentType.IntegerNumber;
        inputField.characterLimit = 3;
        minimumNumberOfCardsPerStackInputField = inputField;
        minimumNumberOfCardsPerStackInputField.onValueChanged.AddListener (delegate {
            int value;
            int.TryParse (minimumNumberOfCardsPerStackInputField.text, out value);
            if (value < 1) {
                minimumNumberOfCardsPerStackInputField.text = "1";
            }
            if (value > 5) {
                minimumNumberOfCardsPerStackInputField.text = "5";
            }
        });
        if (!editMode) {
            minimumNumberOfCardsPerStackInputField.interactable = false;
        }

        if (editMode) {
            Clone = CreateSprite ("UI/Butt_M_Apply", 240, 750, 11, 90, 90, true);
            Clone.name = UIString.SaveGameModeSettings;
        }

        Clone = CreateSprite ("UI/Butt_M_Discard", 1200, 750, 11, 90, 90, true);
        Clone.name = UIString.GoBackToGameModeEditor;

    }
}
