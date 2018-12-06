using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRowClass : GOUI {

    GameObject RowBackground;
    GameObject Text;
    GameObject Edit;
    GameObject Option;

    string setName;
    int setNumber;


    public SetRowClass () {

    }

    public SetRowClass (int x) {
        GameObject Clone;
        RowBackground = CreateSprite ("UI/Panel_Slot_01_Sliced", 720, 330 + x * 90, 11, 540, 90, true);

        Text = CreateText ("", 720, 330 + x * 90, 13, 0.03f);

        Edit = CreateSprite ("UI/Butt_S_Name", 945 - 60, 330 + x * 90, 12, 60, 60, true);
        Edit.GetComponent<Renderer> ().enabled = false;
        Edit.GetComponent<Collider> ().enabled = false;

        Option = CreateSprite ("UI/Butt_S_Delete", 945, 330 + x * 90, 12, 60, 60, true);
        Option.GetComponent<Renderer> ().enabled = false;
        Option.GetComponent<Collider> ().enabled = false;
    }

    public void SetState (string setName, int setId, int mode) {
        SpriteRenderer editRenderer = Edit.GetComponent<SpriteRenderer> ();
        Text.GetComponent<TextMesh> ().text = setName;
        SetState (mode);
    }

    public void SetState (int mode) {
        switch (mode) {
            case 0:
                Edit.GetComponent<Renderer> ().enabled = true;
                Edit.GetComponent<Collider> ().enabled = true;
                Option.GetComponent<Renderer> ().enabled = true;
                Option.GetComponent<Collider> ().enabled = true;
                SetSprite (Option, "UI/Butt_S_Delete");
                break;
            case 1:
                Edit.GetComponent<Renderer> ().enabled = false;
                Edit.GetComponent<Collider> ().enabled = false;
                Option.GetComponent<Renderer> ().enabled = true;
                Option.GetComponent<Collider> ().enabled = true;
                SetSprite (Option, "UI/Butt_S_Add");
                Text.GetComponent<TextMesh> ().text = Language.EmptySlot;
                break;
            case 2:
                Edit.GetComponent<Renderer> ().enabled = false;
                Edit.GetComponent<Collider> ().enabled = false;
                Option.GetComponent<Renderer> ().enabled = false;
                Option.GetComponent<Collider> ().enabled = false;
                break;

        }
    }

    }
