using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPropertiesClass {

    public string displayName;
    public string accountName;
    public int playerNumber;

    public int team;

    public AIClass AI;

    public HandClass hand;

    public ClientInterface client;


    public string [] PlayerPropertiesToString () {
        List<string> s = new List<string> ();
        s.Add (displayName);
        s.Add (team.ToString());
        s.Add (playerNumber.ToString ());
        return s.ToArray ();
    }

    public void LoadFromString (string [] lines) {
        displayName = lines [0];
        team = int.Parse (lines [1]);
        playerNumber = int.Parse (lines [2]);
    }

    public PlayerPropertiesClass () {

    }

    /*public PlayerPropertiesClass (int team, string accountName, HandClass hand) {
        this.team = team;
        this.accountName = accountName;
        this.hand = hand;
    }*/

    public PlayerPropertiesClass (int team, ClientInterface client) {
        NewPlayerProperties (team, false, client.AccountName, client.UserName, new HandClass (client), client);
    }

    public PlayerPropertiesClass (int team, bool AI, string accountName, string userName, HandClass hand, ClientInterface client) {
        NewPlayerProperties (team, AI, accountName, userName, hand, client);
    }

    public void NewPlayerProperties (int team, bool AI, string accountName, string userName, HandClass hand, ClientInterface client) {
        this.team = team;
        this.accountName = accountName;
        this.displayName = userName;
        this.hand = hand;
        this.client = client;
        if (AI) {
            this.AI = new AIClass ();
        }
    }
}
