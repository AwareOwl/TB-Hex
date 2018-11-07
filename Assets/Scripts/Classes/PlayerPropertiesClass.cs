using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPropertiesClass {

    public string displayName;
    public string accountName;

    public AIClass AI;

    public HandClass hand;

    public int team;


    public PlayerPropertiesClass () {

    }

    /*public PlayerPropertiesClass (int team, string accountName, HandClass hand) {
        this.team = team;
        this.accountName = accountName;
        this.hand = hand;
    }*/

    public PlayerPropertiesClass (int team, bool AI, string accountName, HandClass hand) {
        this.team = team;
        this.accountName = accountName;
        this.hand = hand;
        if (AI) {
            this.AI = new AIClass ();
        }
    }
}
