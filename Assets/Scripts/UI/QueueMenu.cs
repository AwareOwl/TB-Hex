using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueMenu : GOUI {
    
    void Start () {
        CurrentGUI = this;
        CreateQueueMenu ();
    }

    static public void ShowQueueMenu () {
        DestroyMenu ();
        CurrentCanvas.AddComponent<QueueMenu> ();
    }

    public void CreateQueueMenu () {
        ShowMessage (Language.WaitingInQueue, UIString.Cancel);
    }

}
