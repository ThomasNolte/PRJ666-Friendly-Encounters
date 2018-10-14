using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour {

    public int UserId;
    public string UserName;
    public string UserPassword;
    public string UserEmail;



    public User()
    {
        this.UserId = 0;
        this.UserName = "";
        this.UserPassword = "";
        this.UserEmail = "";
    }

}
