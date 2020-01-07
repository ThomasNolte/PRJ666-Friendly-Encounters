using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class ResetPassword : MonoBehaviour
{
    private MyMongoDB db = new MyMongoDB();

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
                User resetUser = MyGameManager.user;
                resetUser.password = Password1.text;
                resetUser.presetFlag = 0;
                db.ChangePassword(resetUser);
                InvalidMatch.text = ""; //reset match validation message
                MyGameManager.instance.MyLoadScene((int)MyGameManager.STATES.LOGINSTATE);
            }
        }
    }
}
