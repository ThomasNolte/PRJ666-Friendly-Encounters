using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ResetPassword : MonoBehaviour
{
    public InputField Password1;
    public InputField Password2;

    public Text InvalidPassword;
    public Text InvalidMatch;

    private Regex passwordvalidator = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");

    public void GetInputs()
    {
        if (!passwordvalidator.Match(Password1.text.ToString()).Success)
        {
            InvalidPassword.text = "Invalid User Password, Must contain 1 low case, 1 uppercase, 1 number, 1 special character";
            Password1.text = "";
            Password2.text = "";
        }
        else
        {
            InvalidPassword.text = ""; //reset password validation message

            if (Password1.text.ToString() != Password2.text.ToString())
            {
                InvalidMatch.text = "Passwords do not match";
                Password2.text = "";
            }
            else
            {
                ChangePassword();
                InvalidMatch.text = ""; //reset match validation message
                MyGameManager.instance.MyLoadScene((int)MyGameManager.STATES.LOGINSTATE);
            }
        }
    }

    public void ChangePassword()
    {
        SSH ssh = new SSH();
        ssh.Initialize("myvmlab.senecacollege.ca", 6265, "student", "frndly02", 3306);
        ssh.OpenSSHConnection();
        ssh.OpenPort();

        ssh.mysql.Initialize("127.0.0.1", Convert.ToString(ssh.boundport), "FriendlyEncounters", "student", "frndly02");

        ssh.mysql.SQLChangePassword(MyGameManager.user.Name, Password1.text.ToString(), MyGameManager.user.Preset);

        ssh.CloseSSHConnection();

    }
}
