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
}
