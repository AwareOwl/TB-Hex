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
                    client.TargetShowMessage (client.connectionToClient, Language.PasswordIsIncorrectKey);
                }
            } else {
                client.TargetShowMessage (client.connectionToClient, Language.AccountDoesntExistKey);
            }
        } else {
            client.TargetShowMessage (client.connectionToClient, Language.PleaseEnterAccountNameKey);
        }
    }

    static public void Register (ClientInterface client, string userName, string password, string email) {
        if (userName != null && userName != "") {
            if (!ServerData.UserExists (userName)) {
                if (password.Length >= 8) {
                    ServerData.CreateAccount (userName, password, email);
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

    static public void CompleteLogIn (ClientInterface client, string userName) {
        client.UserName = userName;
        client.TargetLogIn (client.connectionToClient);
    }
}
