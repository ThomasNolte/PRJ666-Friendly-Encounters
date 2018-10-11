using UnityEngine;
using UnityEngine.UI;
using System;

public class FindUser : MonoBehaviour {

    StateManager State = new StateManager();

    //input fields
    public InputField UserName;
    public InputField UserPassword;

    public Text InvalidInput;

    private string uid;
    private string upwd;

    public User user;

    public void GetInputs()
    {
        uid = UserName.text.ToString();
        upwd = UserPassword.text.ToString();

        LookupUser();
    }

    public void LookupUser()
    {
        SSH ssh = new SSH();
        ssh.Initialize("myvmlab.senecacollege.ca", 6265, "student", "frndly02", 3306);
        ssh.OpenSSHConnection();
        ssh.OpenPort();

        ssh.mysql.Initialize("127.0.0.1", Convert.ToString(ssh.boundport), "FriendlyEncounters", "student", "frndly02");
        user = ssh.mysql.SQLSelectUser(uid, upwd);
        if (user.UserName == null || user.UserName == "")
        {
            InvalidInput.text = "invalid Username or Password.";
        }
        else
        {
            InvalidInput.text = "successful login wait for further implementation";
            Debug.Log(user.UserName);
        }
    }
}
