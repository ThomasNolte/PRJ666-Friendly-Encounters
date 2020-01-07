using MongoDB.Bson;

public class User{

    public User() {
        _id = new ObjectId();
        username = "Guest";
        email = "Guest";
        password = "password";
        presetFlag = 0;
    }
    public ObjectId _id { set; get; }

    //public int ActiveConnection { set; get; }
    public string username { set; get; }
    public string email { set; get; }
    public string password { set; get; }
    public int presetFlag { set; get; }

    public override int GetHashCode()
    {
        int result = _id.GetHashCode();
        result = 31 * username.GetHashCode();
        result = 31 * email.GetHashCode();
        result = 31 * password.GetHashCode();
        result = 31 * presetFlag.GetHashCode();
        return result;
    }

    public override bool Equals(object obj)
    {
        if (this == obj) 
        {
            return true;
        }
        if (obj == null || GetType() != obj.GetType()) {
            return false;
        }
        User that = (User)obj;
        bool idEquals = Equals(_id, that._id);
        bool usernameEquals = Equals(username, that.username);
        bool emailEquals = Equals(email, that.email);
        bool passwordEquals = Equals(password, that.password);
        bool presetFlagEquals = Equals(presetFlag, that.presetFlag);
        return idEquals && usernameEquals && emailEquals && passwordEquals && presetFlagEquals;
    }
}
