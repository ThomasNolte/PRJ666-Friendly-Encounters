using System.Net;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Text;
using System;

public class RecoverPassword : MonoBehaviour
{
    private MyMongoDB db = new MyMongoDB();

    public InputField UserEmail;

    public Text InvalidEmail;
    
    public void GetInputs()
    {
        InvalidEmail.text = "";
        if (UserEmail.text.ToString() == "")
        {
            InvalidEmail.text = "Please Enter the Email address for account with username: " + MyGameManager.user.username;
        }
        else
        {
            if (UserEmail.text.ToString() == MyGameManager.user.email)
            {
                Debug.Log(MyGameManager.user.email);
                SendEmail(MyGameManager.user.email);
                InvalidEmail.text = "An email has been sent with your new password";
                MyGameManager.instance.MyLoadScene((int)MyGameManager.STATES.LOGINSTATE);
            }
            else
            {
                Debug.Log(MyGameManager.user.email);
                InvalidEmail.text = "Email does not match the account's email";
            }
        }
    }

    private void SendEmail(string email)
    {
        string newpassword = GeneratePassword();

        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("friendlyencounters02@gmail.com");
        mail.To.Add(email);
        mail.Subject = "Friendly Encounters Password Reset";
        mail.Body = "Greeting!\n\nYour new password is:\n" + newpassword + "\n\nUpon your next login you will be asked to reset your password.";

        SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
        smtp.EnableSsl = true;
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = (ICredentialsByHost)new NetworkCredential("friendlyencounters02@gmail.com", "frndly02");
        ServicePointManager.ServerCertificateValidationCallback =
                 delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                 { return true; };
        smtp.Send(mail);

        User resetUser = new User();
        resetUser = MyGameManager.user;
        resetUser.password = newpassword;
        resetUser.presetFlag = 1;
        db.ChangePassword(resetUser);
    }

    private string GeneratePassword()
    {
        System.Random ran = new System.Random();
        StringBuilder builder = new StringBuilder();
        char c;
        for (int i = 0; i < 10; i++)
        {
            c = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * ran.NextDouble() + 65)));
            builder.Append(c);
        }
        return builder.ToString();
    }
}
