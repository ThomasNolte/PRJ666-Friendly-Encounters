using UnityEngine;
using UnityEngine.UI;
using System;

public class FindUser : MonoBehaviour {
    
    //input fields
    public InputField UserName;
    public InputField UserPassword;

    public Text InvalidInput;

    public User player;
    
    public void Login()
    {
        InvalidInput.text = "";
        if (UserName.text.ToString() == "" || UserPassword.text.ToString() == "")
        {
            InvalidInput.text = "Please enter a username and password";
        }
        else
        {
            LookupUser(UserName.text.ToString(), UserPassword.text.ToString());
            MyGameManager.user = player;
        }
    }

    public void RecoverPassword()
    {
        InvalidInput.text = "";
        if (UserName.text.ToString() == "")
        {
            InvalidInput.text = "Please enter your account's username to recover the password";
        }
        else
        {
            LookupUser(UserName.text.ToString());
            MyGameManager.user = player;
        }
    }

    public void LookupUser(string uid, string upwd)
    {
        /*        SSH ssh = new SSH();
                ssh.Initialize("myvmlab.senecacollege.ca", 6265, "student", "frndly02", 3306);
                ssh.OpenSSHConnection();
                ssh.OpenPort();

                ssh.mysql.Initialize("127.0.0.1", Convert.ToString(ssh.boundport), "FriendlyEncounters", "student", "frndly02");

                player = ssh.mysql.SQLSelectUser(uid, upwd);

                ssh.CloseSSHConnection();*/

        MyMongoDB mb = new MyMongoDB();
        mb.Initialize();

/*        if (player.Name == null || player.Name == "Guest")
        {
            //invalid login
            InvalidInput.text = "Invalid Username or Password.";
        }
        else
        {
            if(player.Preset == 0)
            {
                MyGameManager.instance.MyLoadScene((int)MyGameManager.STATES.PROFILESTATE);
            }
            else
            {
                MyGameManager.instance.MyLoadScene((int)MyGameManager.STATES.RESETPASSWORD);
            }
        }*/
    }

    public void LookupUser(string uid)
    {

/*        SSH ssh = new SSH();
        ssh.Initialize("myvmlab.senecacollege.ca", 6265, "student", "frndly02", 3306);
        ssh.OpenSSHConnection();
        ssh.OpenPort();

        ssh.mysql.Initialize("127.0.0.1", Convert.ToString(ssh.boundport), "FriendlyEncounters", "student", "frndly02");

        player = ssh.mysql.SQLSelectUser(uid);

        ssh.CloseSSHConnection();*/

/*        if (player.Name == null || player.Name == "Guest")
        {
            //invalid login
            InvalidInput.text = "Username not found, please enter your account username";
        }
        else
        {
            MyGameManager.instance.MyLoadScene((int)MyGameManager.STATES.FORGOTPASSWORD);
        }*/
    }
}
