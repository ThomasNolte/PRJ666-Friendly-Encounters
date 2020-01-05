using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AddUser : MonoBehaviour {

    private MyMongoDB db = new MyMongoDB();

    //input fields
    public InputField UserName;
    public InputField UserEmail;
    public InputField UserPassword;

    //invalid textboxes messages
    public Text InvalidUserName;
    public Text InvalidEmail;
    public Text InvalidPassword;

    //regular expressions
    private Regex emailvalidator = new Regex(@"^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$");
    private Regex passwordvalidator = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
    private Regex usernamevalidator = new Regex(@"^[a-zA-Z_]([a-zA-Z0-9_-]{0,31}|[a-zA-Z0-9_-]{0,30}\$)$");

    public void GetInputs()
    {
        //flag for any invalid inputs
        bool isvalid = true;
        //validate inputs
        if (!usernamevalidator.Match(UserName.text.ToString()).Success)
        {
            InvalidUserName.text = "Invalid User Name, must be between 2-30, connot contain 2 consecutive spaces, must begin and end with a character";
            UserName.text = "";
            isvalid = false;
        }
        else { InvalidUserName.text = ""; } //reset username validation message
        if (!passwordvalidator.Match(UserPassword.text.ToString()).Success)
        {
            InvalidPassword.text = "Invalid User Password, Must contain 1 low case, 1 uppercase, 1 number, 1 special character";
            UserPassword.text = "";
            isvalid = false;
        }
        else { InvalidPassword.text = ""; } //reset password validation message
        if (!emailvalidator.Match(UserEmail.text.ToString()).Success)
        {
            InvalidEmail.text = "Invalid email address";
            UserEmail.text = "";
            isvalid = false;
        }
        else { InvalidEmail.text = ""; } //reset email validation message

        if (isvalid)
        {
            User newUser = new User();
            newUser.username = UserName.text.ToString();
            newUser.email = UserEmail.text.ToString();
            newUser.password = UserPassword.text.ToString();
            newUser.presetFlag = 0;
            Add(newUser);
        }
    }

    public void Add(User newUser)
    {
        bool success = true;
        db.Initialize();
        success = db.FindUserName(newUser.username) != null? false:true;
        if (success)
        {
            db.InsertUser(newUser);
            MyGameManager.instance.MyLoadScene((int)MyGameManager.STATES.LOGINSTATE);
        }
        else
        {
            InvalidUserName.text = "Username already exists. you will have to use a different one.";
        }
    }
}
