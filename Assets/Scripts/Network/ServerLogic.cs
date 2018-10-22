using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerLogic : MonoBehaviour {
    
    static public void LogIn (ClientInterface client, string userName, string password) {
        if (userName != null && userName != "") {
            if (ServerData.UserExists (userName)) {
                if (VerifyUserPassword (userName, password)) {
                    CompleteLogIn (client, userName);
                } else {
                    client.TargetShowMessage (client.connectionToClient, "Password is incorrect.");
                }
            } else {
                client.TargetShowMessage (client.connectionToClient, "Account doesn't exists.");
            }
        } else {
            client.TargetShowMessage (client.connectionToClient, "Please eneter user name.");
        }
    }

    static public void Register (ClientInterface client, string userName, string password, string email) {
        if (userName != null && userName != "") {
            if (password.Length >= 8) {
                ServerData.CreateAccount (userName, password, email);
            } else {
                client.TargetShowMessage (client.connectionToClient, "Password has to have at least 8 characters.");
            }
        } else {
            client.TargetShowMessage (client.connectionToClient, "User name can't be null.");
        }
    }

    static public bool VerifyUserPassword (string userName, string password) {
        string userPassword = ServerData.GetUserKeyData (userName, "Password");
        if (password.CompareTo (userPassword) == 0) {
            return true;
        }
        return false;
    }

    static public void CompleteLogIn (ClientInterface client, string userName) {
        client.UserName = userName;
        client.TargetLogIn (client.connectionToClient);
    }
}
