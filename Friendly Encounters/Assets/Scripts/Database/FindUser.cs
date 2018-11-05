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

    public void Login()
    {
        LookupUser(UserName.text.ToString(), UserPassword.text.ToString());
        MyGameManager.SetUser(user);
    }

    public void RecoverPassword()
    {
        LookupUser(UserName.text.ToString());
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
            if(user.Preset == 0)
            {
                states.MyLoadScene((int)MyGameManager.STATES.PROFILESTATE);
            }
            else
            {
                states.MyLoadScene((int)MyGameManager.STATES.RESETPASSWORD);
            }
        }
    }

    public void LookupUser(string uid)
    {

        SSH ssh = new SSH();
        ssh.Initialize("myvmlab.senecacollege.ca", 6265, "student", "frndly02", 3306);
        ssh.OpenSSHConnection();
        ssh.OpenPort();

        ssh.mysql.Initialize("127.0.0.1", Convert.ToString(ssh.boundport), "FriendlyEncounters", "student", "frndly02");

        user = ssh.mysql.SQLSelectUser(uid);

        ssh.CloseSSHConnection();

        if (user.Name == null || user.Name == "Guest")
        {
            //invalid login
            InvalidInput.text = "Username not found, please enter your account username";
        }
        else
        {
            states.MyLoadScene((int)MyGameManager.STATES.FORGOTPASSWORD);
        }
    }
}
