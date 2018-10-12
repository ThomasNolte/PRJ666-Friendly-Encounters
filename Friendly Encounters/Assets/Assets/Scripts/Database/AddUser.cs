using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AddUser : MonoBehaviour {

    StateManager State = new StateManager();

    //input fields
    public InputField UserName;
    public InputField UserEmail;
    public InputField UserPassword;

    //invalid textboxes messages
    public Text InvalidUserName;
    public Text InvalidEmail;
    public Text InvalidPassword;

    private string uid;
    private string uemail;
    private string upwd;

    //regular expressions
    private Regex emailvalidator = new Regex(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");
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
            uid = UserName.text.ToString();
            uemail = UserEmail.text.ToString();
            upwd = UserPassword.text.ToString();
            Add();
        }
    }

    public void Add()
    {
        SSH ssh = new SSH();
        ssh.Initialize("myvmlab.senecacollege.ca", 6265, "student", "frndly02", 3306);
        ssh.OpenSSHConnection();
        ssh.OpenPort();

        ssh.mysql.Initialize("127.0.0.1", Convert.ToString(ssh.boundport), "FriendlyEncounters", "student", "frndly02");
        ssh.mysql.SQLInsertUser(uid, upwd, uemail);

        State.LoginButton();
    }
}
