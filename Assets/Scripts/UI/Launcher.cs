using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : GOUI {

	// Use this for initialization
	void Start () {

        UICanvas = GameObject.Find ("Canvas");
        EnvironmentScript.CreateNewBackground (1);
        //Debug.Log (Language.test);
        //EnvironmentScript.CreateRandomBoard ();
        //EnvironmentScript.CreateNormalBoard ();

        //VisualCard.Make4Cards ();
        //InGameUI.CreatePlayersUI ();
        CreateLauncherGUI ();
    }

    public void Test () {
    }

    public void CreateLauncherGUI () {
        GameObject Clone;
        GameObject Parent;


        Clone = CreateSprite ("UI/Butt_M_EmptySquare", true);
        SetInPixScale (Clone, 60, 60);
        SetInPixPosition (Clone, 1440 - 165, 45, 11);
        Parent = Clone;
        Clone.name = "SelectENG";

        Clone = CreateText ("ENG", 1440 - 165, 45, 12, 0.02f);
        Clone.GetComponent<TextMesh> ().color = Color.black;
        AddTextToGameObject (Parent, Clone);


        Clone = CreateSprite ("UI/Butt_M_EmptySquare", true);
        SetInPixScale (Clone, 60, 60);
        SetInPixPosition (Clone, 1440 - 105, 45, 11);
        Parent = Clone;
        Clone.name = "SelectPL";

        Clone = CreateText ("PL", 1440 - 105, 45, 12, 0.02f);
        Clone.GetComponent<TextMesh> ().color = Color.black;
        AddTextToGameObject (Parent, Clone);

        //Clone = CreateSprite ("UI/White", 720, 540, 13, 1440, 1080, false);

        Clone = CreateSprite ("UI/Panel_Window_01_Sliced", 300, 450, 10, 510, 300, false);


        Clone = CreateSprite ("UI/Butt_M_EmptySquare", true);
        SetInPixScale (Clone, 400, 90);
        SetInPixPosition (Clone, 300, 400, 11);
        Parent = Clone;
        Clone.name = "StartHost";

        Clone = CreateText (Language.CreateLocalNetwork, 300, 400, 12, 0.03f);
        Clone.GetComponent<TextMesh> ().color = Color.black;
        AddTextToGameObject (Parent, Clone);

        Clone = CreateSprite ("UI/Butt_M_EmptySquare", true);
        SetInPixScale (Clone, 400, 90);
        SetInPixPosition (Clone, 300, 500, 11);
        Parent = Clone;
        Clone.name = "StartClient";

        Clone = CreateText (Language.JoinLocalNetwork, 300, 500, 12, 0.03f);
        Clone.GetComponent<TextMesh> ().color = Color.black;
        AddTextToGameObject (Parent, Clone);

        /*
        Clone = CreateSprite ("UI/Butt_M_EmptySquare", true);
        SetInPixScale (Clone, 400, 90);
        SetInPixPosition (Clone, 300, 600, 11);
        Parent = Clone;
        Clone.name = "StartServer";*/
    }
}
