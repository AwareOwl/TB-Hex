using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageUI : GOUI {

    public int currentPage;
    public int numberOfButtons;
    public int pageLimit;

    public GameObject [] ButtonBackground;
    public TextMesh [] ButtonText;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init (int numberOfButtons, int pageLimit, Vector2Int startingPosition, string name) {
        numberOfButtons = Mathf.Min (numberOfButtons, pageLimit);
        this.numberOfButtons = numberOfButtons;
        this.pageLimit = pageLimit;
        GameObject Clone;
        ButtonBackground = new GameObject [numberOfButtons];
        ButtonText = new TextMesh [numberOfButtons];
        if (pageLimit > 1) {
            for (int x = 0; x < numberOfButtons; x++) {
                Clone = CreateSpriteWithText ("UI/Butt_M_EmptyRect_Sliced", (x + 1).ToString (), startingPosition.x + 60 * x, startingPosition.y, 11, 60, 60);
                UIController UIC = Clone.GetComponent<UIController> ();
                UIC.number = x;
                UIC.pageUI = this;
                Clone.name = name;
                ButtonBackground [x] = Clone;
                ButtonText [x] = Clone.transform.Find ("Text").GetComponent<TextMesh> ();
            }
        }
    }

    public void ClickOnButton (int buttonNumber) {

    }

    public int ButtonNumberToPageNumber (int number) {
        if (number == 0) {
            return 0;
        } else if (number == numberOfButtons - 1) {
            return Mathf.Max (pageLimit - 1, numberOfButtons - 1);
        } else {
            return Mathf.Max (number, Mathf.Min (pageLimit - 1, Mathf.Min (number + currentPage - (numberOfButtons - 1) / 2, pageLimit + number - numberOfButtons)));
        }
    }

    public int SelectPage (int page) {
        page = ButtonNumberToPageNumber (page);
        currentPage = page;
        for (int x = 0; x < numberOfButtons; x++) {
            GameObject button = ButtonBackground [x];
            if (button == null) {
                continue;
            }
            button.GetComponent<UIController> ().FreeAndUnlcok ();

            int number = ButtonNumberToPageNumber (x);
            ButtonText [x].text = (number + 1).ToString ();
            if (number == page && button != null) {
                button.GetComponent<UIController> ().PressAndLock ();
            }
        }
        for (int x = 0; x < numberOfButtons; x++) {
        }
        return currentPage;
    }
}
