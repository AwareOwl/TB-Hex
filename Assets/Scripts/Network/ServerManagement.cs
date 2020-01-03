using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class ServerManagement : MonoBehaviour {

    static public bool TestServer = true;

    static ServerManagement Instance;

    static List<Task <AIResult>> listOfTasksToDoOnSeperateThread = new List<Task<AIResult>> ();
    static List<Action> listOfActionsToDoOnMainThread = new List<Action> ();
    

    bool AutoDeleteAllFinished = true;

    private void Awake () {
        Instance = this;
        if (TestServer) {
            ServerData.SaveBackUp ();
        }
        //ServerData.DeleteBackUps ();
        ServerData.SetInitVector ();
        ServerVersionManager.CheckServerVersion ();
        ServerVersionManager.FinalizeServerVersion ();
    }

    private void OnEnable () {
        Debug.Log ("Enabling server managment");
    }

    private void OnDisable () {
        Debug.Log ("Disabling server managment");
    }

    // Update is called once per frame
    void Update () {
        MatchMakingClass.matches.RemoveAll (match => match.finished);
        
        while (listOfActionsToDoOnMainThread.Count > 0) {
            Action tempAction = listOfActionsToDoOnMainThread [0];
            listOfActionsToDoOnMainThread.RemoveAt (0);

            tempAction ();
        }
        while (listOfTasksToDoOnSeperateThread.Count > 0) {
            Task <AIResult> task = listOfTasksToDoOnSeperateThread [0];
            if (!task.IsCompleted) {
                break;
            }
            listOfTasksToDoOnSeperateThread.RemoveAt (0);
            AIResult result = task.Result;
            result.match.PlayCard (result.x, result.y, result.match.turnOfPlayer, result.stackNumber);
        }
    }

    class AIResult {
        public MatchClass match;
        public int x;
        public int y;
        public int stackNumber;
        public AIResult (MatchClass match, int x, int y, int stackNumber) {
            this.match = match;
            this.x = x;
            this.y = y;
            this.stackNumber = stackNumber;
        }
    }

    static public void RunAIOnSeperateThread (MatchClass match) {
        Task <AIResult> task = Task <AIResult>.Run (() => {
            Vector3Int results = match.GetPlayer (match.turnOfPlayer).properties.AI.FindBestMove (match);
            return new AIResult (match, results.x, results.y, results.z);
        });
        listOfTasksToDoOnSeperateThread.Add (task);
    }

    static public void PlayCardOnMainThread (MatchClass match, int x, int y, int playerNumber, int stackNumber) {
        Action aFunction = () => {
            match.PlayCard (x, y, playerNumber, stackNumber);
        };
        listOfActionsToDoOnMainThread.Add (aFunction);
        //Instance.StartCoroutine (match.IEPlayCard (x, y, playerNumber, stackNumber));
    }
}
