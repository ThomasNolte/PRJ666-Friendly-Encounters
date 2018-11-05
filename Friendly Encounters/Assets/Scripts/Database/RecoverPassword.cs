using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Text;
using System;

public class RecoverPassword : MonoBehaviour {

    MyGameManager states;

    public InputField UserEmail;

    public Text InvalidEmail;

    void Awake()
    {
        states = GameObject.Find("MyGameManager").GetComponent<MyGameManager>();
    }

    public void GetInputs()
    {
        InvalidEmail.text = "";
        if (UserEmail.text.ToString() == MyGameManager.GetUser().Email)
        {
            Debug.Log(MyGameManager.GetUser().Email);
            SendEmail(MyGameManager.GetUser().Email);
            InvalidEmail.text = "An email has been sent with your new password";
            states.MyLoadScene((int)MyGameManager.STATES.LOGINSTATE);
        }
        else
        {
            Debug.Log(MyGameManager.GetUser().Email);
            InvalidEmail.text = "Email does not match accounts email";
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
        smtp.Credentials = new NetworkCredential("friendlyencounters02@gmail.com", "frndly02");
        ServicePointManager.ServerCertificateValidationCallback =
                 delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                 { return true; };
        smtp.Send(mail);

        ChangePassword(newpassword);
    }

    private string GeneratePassword()
    {
        System.Random ran = new System.Random();
        StringBuilder builder = new StringBuilder();
        char c;
        for(int i = 0; i < 10; i++)
        {
            c = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * ran.NextDouble() + 65)));
            builder.Append(c);
        }
        return builder.ToString();
    }

    private void ChangePassword(string password)
    {
        SSH ssh = new SSH();
        ssh.Initialize("myvmlab.senecacollege.ca", 6265, "student", "frndly02", 3306);
        ssh.OpenSSHConnection();
        ssh.OpenPort();

        ssh.mysql.Initialize("127.0.0.1", Convert.ToString(ssh.boundport), "FriendlyEncounters", "student", "frndly02");

        ssh.mysql.SQLChangePassword(MyGameManager.GetUser().Name, password, MyGameManager.GetUser().Preset);

        ssh.CloseSSHConnection();
    }
}
