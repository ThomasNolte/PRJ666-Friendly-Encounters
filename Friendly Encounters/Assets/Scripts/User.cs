public class User{

    private int UserId;
    private string UserName;
    private string UserPassword;
    private string UserEmail;

    public User()
    {
        this.UserId = 0;
        this.UserName = "";
        this.UserPassword = "";
        this.UserEmail = "";
    }

    public int ID
    {
        get
        {
            return UserId;
        }
        set
        {
            UserId = value;
        }
    }

    public string Name
    {
        get
        {
            return UserName;
        }
        set
        {
            UserName = value;
        }
    }

    public string Password
    {
        get
        {
            return UserPassword;
        }
        set
        {
            UserPassword = value;
        }
    }

    public string Email
    {
        get
        {
            return UserEmail;
        }
        set
        {
            UserEmail = value;
        }
    }

}
