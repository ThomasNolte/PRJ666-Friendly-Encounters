using UnityEngine;
using UnityEngine.UI;
using System;

public class FindUser : MonoBehaviour {

    MyGameManager states;

    //input fields
    public InputField UserName;
    public InputField UserPassword;

    public Text InvalidInput;

    public User user;

    void Awake() {
        states = GameObject.Find("MyGameManager").GetComponent<MyGameManager>();
    }

    public void GetInputs()
    {
        LookupUser(UserName.text.ToString(), UserPassword.text.ToString());
        MyGameManager.SetUser(user);
    }

    public void LookupUser(string uid, string upwd)
    {
        SSH ssh = new SSH();
        ssh.Initialize("myvmlab.senecacollege.ca", 6265, "student", "frndly02", 3306);
        ssh.OpenSSHConnection();
        ssh.OpenPort();

        ssh.mysql.Initialize("127.0.0.1", Convert.ToString(ssh.boundport), "FriendlyEncounters", "student", "frndly02");

        user = ssh.mysql.SQLSelectUser(uid, upwd);

        ssh.CloseSSHConnection();

        if (user.Name == null || user.Name == "Guest")
        {
            //invalid login
            InvalidInput.text = "Invalid Username or Password.";
        }
        else
        {
            states.MyLoadScene((int)MyGameManager.STATES.PROFILESTATE);

        }
    }
}
