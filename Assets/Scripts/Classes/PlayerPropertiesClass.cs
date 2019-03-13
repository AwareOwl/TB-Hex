using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPropertiesClass {

    public string displayName;
    public string accountName;
    public int avatar;
    public int playerNumber;

    public bool conceded = false;
    public bool enabled = true;

    public int team;

    public AIClass AI;

    public HandClass startingHand;

    public ClientInterface client;


    public string [] PlayerPropertiesToString () {
        List<string> s = new List<string> ();
        s.Add (displayName);
        s.Add (avatar.ToString());
        s.Add (team.ToString());
        s.Add (playerNumber.ToString ());
        s.Add (conceded.ToString ());
        s.Add (enabled.ToString ());
        return s.ToArray ();
    }

    public void LoadFromString (string [] lines) {
        displayName = lines [0];
        avatar = int.Parse (lines [1]);
        team = int.Parse (lines [2]);
        playerNumber = int.Parse (lines [3]);
        conceded = Convert.ToBoolean (lines [4]);
        enabled = Convert.ToBoolean (lines [5]);
    }

    public PlayerPropertiesClass () {

    }

    /*public PlayerPropertiesClass (int team, string accountName, HandClass hand) {
        this.team = team;
        this.accountName = accountName;
        this.hand = hand;
    }*/

    public PlayerPropertiesClass (int team, ClientInterface client) {
        NewPlayerProperties (team, null, client.AccountName, client.UserName, new HandClass (client), client);
    }

    public PlayerPropertiesClass (int team, AIClass AI, string accountName, string userName, HandClass hand, ClientInterface client) {
        NewPlayerProperties (team, AI, accountName, userName, hand, client);
    }

    public void NewPlayerProperties (int team, AIClass AI, string accountName, string userName, HandClass hand, ClientInterface client) {
        this.team = team;
        this.accountName = accountName;
        this.displayName = userName;
        if (AI != null) {
            this.avatar = 2;
        } else {
            this.avatar = ServerData.GetUserAvatar (accountName);
        }
        this.startingHand = hand;
        this.client = client;
        this.AI = AI;
    }
}
