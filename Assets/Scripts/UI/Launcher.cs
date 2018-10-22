using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : GOUI {

	// Use this for initialization
	void Start () {
        UICanvas = GameObject.Find ("Canvas");
        EnvironmentScript.CreateNewBackground (1);
        Debug.Log (Language.test);
        //EnvironmentScript.CreateRandomBoard ();
        //EnvironmentScript.CreateNormalBoard ();

        //VisualCard.Make4Cards ();
        //InGameUI.CreatePlayersUI ();
        CreateLauncherGUI ();

        //ShowMessage ("test.");
        //ShowMessage ("Some error occured, please try again.");
        /*ShowMessage ("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin scelerisque sodales mi et accumsan. " +
            "Aliquam erat volutpat. Donec est turpis, interdum ut risus nec, congue viverra nulla. Morbi fermentum massa a lorem pulvinar ornare eu at neque. " +
            "Quisque id hendrerit lorem.");*/
    }

    public void Test () {
    }

    public void CreateLauncherGUI () {
        GameObject Clone;
        GameObject Parent;

        //Clone = CreateSprite ("UI/White", 720, 540, 13, 1440, 1080, false);

        Clone = CreateSprite ("UI/Panel_Window_01_Sliced", 300, 450, 10, 510, 300, false);


        Clone = CreateSprite ("UI/Butt_M_EmptySquare", true);
        SetInPixScale (Clone, 400, 90);
        SetInPixPosition (Clone, 300, 400, 11);
        Parent = Clone;
        Clone.name = "StartHost";

        Clone = CreateText ("Stwórz sieć lokalną", 300, 400, 12, 0.03f);
        Clone.GetComponent<TextMesh> ().color = Color.black;
        AddTextToGameObject (Parent, Clone);

        Clone = CreateSprite ("UI/Butt_M_EmptySquare", true);
        SetInPixScale (Clone, 400, 90);
        SetInPixPosition (Clone, 300, 500, 11);
        Parent = Clone;
        Clone.name = "StartClient";

        Clone = CreateText ("Dołącz do sieci lokalnej", 300, 500, 12, 0.03f);
        Clone.GetComponent<TextMesh> ().color = Color.black;
        AddTextToGameObject (Parent, Clone);


        /*for (int x = 0; x < 19; x++) {
            for (int y = 0; y < 13; y++) {
                Clone = CreateSprite ("UI/Butt_S_Delete");
                SetInPixScale (Clone, 90, 90);
                SetInPixPosition (Clone, 0 + 90 * x, 0 + 90 * y, -0.0001f);
            }
        }*/


    }
    
	// Update is called once per frame
	void Update () {
		
	}
}
