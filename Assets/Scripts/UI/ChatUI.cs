using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatUI : GOUI {

    static List<GameObject> garbage = new List<GameObject> ();
    static public ChatUI instance;

    static GameObject Content;
    static GameObject textObject;
    static InputField inputField;
    static Text text;

    static string wholeString = "";
    static List<string> Nickname = new List<string> ();
    static List<string> Message = new List<string> ();

	// Use this for initialization
	void Awake () {
        instance = this;
        CreateChatUI ();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    override public void DestroyThis () {
        foreach (GameObject obj in garbage) {
            if (obj != null) {
                DestroyImmediate (obj);
            }
        }
        DestroyImmediate (instance);
    }

    static public void RecieveMessage (string userName, string message) {
        Nickname.Add (userName);
        Message.Add (message);
        wholeString += userName + ": " + message + System.Environment.NewLine;
        RefreshText ();
    }

    static public void SendMessage () {
        ClientLogic.MyInterface.CmdChatSendMessage (inputField.text);
        inputField.text = "";
    }

    static public void ShowChatUI () {
        CurrentCanvas.AddComponent<ChatUI> ();
    }

    static public void RefreshText () {

        text.text = wholeString;
        float height = text.preferredHeight;
        text.alignment = TextAnchor.MiddleLeft;
        Content.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, height + 30);

        textObject.transform.SetParent (Content.transform);
        textObject.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, 0);
        textObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (420, height + 15);
    }

    static public void ToggleChatUI () {
        if (instance == null) {
            ShowChatUI ();
        } else {
            instance.DestroyThis ();
        }
    }

    static public void CreateChatUI () {

        GameObject Clone;

        Clone = CreateUIScrollView (1170, 540, 480, 900);
        Content = Clone.transform.Find ("Viewport").Find ("Content").gameObject;
        garbage.Add (Clone);

        Clone = CreateInputField (Language.EnterText, 1110, 1020, 360, 60);
        inputField = Clone.GetComponent<InputField> ();
        garbage.Add (Clone);

        Clone = CreateSpriteWithText ("UI/Butt_M_EmptySquare", Language.Send, 1350, 1020, 20, 120, 60);
        Clone.name = UIString.ChatSendButton;
        garbage.Add (Clone);

        Clone = CreateUIText ("", 0, 0);
        text = Clone.GetComponent<Text> ();
        textObject = Clone;
        Clone.GetComponent<RectTransform> ().sizeDelta = new Vector2 (420, 100);
        RefreshText ();
    }
}
