﻿using UnityEngine;
using UnityEngine.UI;

public class FindUser : MonoBehaviour {

    private MyMongoDB db = new MyMongoDB();

    //input fields
    public InputField UserName;
    public InputField UserPassword;

    public Text InvalidInput;
    
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
        }
    }

    public void LookupUser(string uid, string upwd)
    {
        User player = db.FindUser(uid, upwd);
        if (player == null)
        {
            //invalid login
            InvalidInput.text = "Invalid Username or Password.";
        }
        else
        {
            MyGameManager.user = player;
            if (player.presetFlag == 0)
            {
                MyGameManager.instance.MyLoadScene((int)MyGameManager.STATES.PROFILESTATE);
            }
            else
            {
                MyGameManager.instance.MyLoadScene((int)MyGameManager.STATES.RESETPASSWORD);
            }
        }
    }

    public void LookupUser(string uid)
    {
        User player = db.FindUserName(uid);
        if (player == null)
        {
            //invalid login
            InvalidInput.text = "Username not found, please enter your account username";
        }
        else
        {
            MyGameManager.user = player;
            MyGameManager.instance.MyLoadScene((int)MyGameManager.STATES.FORGOTPASSWORD);
        }
    }
}
