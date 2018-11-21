using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerLogic : MonoBehaviour {
    
    static public void LogIn (ClientInterface client, string userName, string password) {
        if (userName != null && userName != "") {
            if (ServerData.UserExists (userName)) {
                if (VerifyUserPassword (userName, password)) {
                    //JoinGameAgainstAI (client);
                    CompleteLogIn (client, userName);

                } else {
                    client.TargetShowMessage (client.connectionToClient, Language.PasswordIsIncorrectKey);
                }
            } else {
                client.TargetShowMessage (client.connectionToClient, Language.AccountDoesntExistKey);
            }
        } else {
            client.TargetShowMessage (client.connectionToClient, Language.PleaseEnterAccountNameKey);
        }
    }

    static public void Register (ClientInterface client, string accountName, string userName, string password, string email) {
        if (accountName != null && accountName != "") {
            if (!ServerData.UserExists (accountName)) {
                if (password.Length >= 8) {
                    ServerData.CreateAccount (accountName, userName, password, email);
                    client.TargetShowMessage (client.connectionToClient, Language.AccountCreatedKey);
                } else {
                    client.TargetShowMessage (client.connectionToClient, Language.PasswordHasToHaveAtLeast8CharactersKey);
                }
            } else {
                client.TargetShowMessage (client.connectionToClient, Language.AccountWithThisNameAlreadyExistsKey);
            }
        } else {
            client.TargetShowMessage (client.connectionToClient, Language.AccountNameCantBeNullKey);
        }
    }

    static public bool VerifyUserPassword (string userName, string password) {
        string userPassword = ServerData.GetUserKeyData (userName, "Password");
        if (password.CompareTo (userPassword) == 0) {
            return true;
        }
        return false;
    }

    static public void CompleteLogIn (ClientInterface client, string accountName) {
        client.AccountName = accountName;
        string userName = ServerData.GetUserKeyData (accountName, ServerData.UserNameKey);
        if (userName == "null" || userName == "") {
            userName = accountName;
        }
        client.UserName = userName;
        client.TargetLogIn (client.connectionToClient, accountName, userName);
    }

    static public void JoinGameAgainstAI (ClientInterface client) {
        HandClass hand1 = new HandClass ();
        HandClass hand2 = new HandClass ();
        hand1.GenerateRandomHand ();
        hand2.GenerateRandomHand ();
        MatchMakingClass.CreateGame (new PlayerPropertiesClass [] {
            new PlayerPropertiesClass (1, false, client.AccountName, client.UserName, hand1, client),
            new PlayerPropertiesClass (2, true, "Doge2", "Doge2", hand2, null) });
    }

    static public void ShowMatchResult (ClientInterface client, string winnerName, int winCondition, int limit) {
        client.TargetShowMatchResult (client.connectionToClient, winnerName, winCondition, limit);
    }
}
