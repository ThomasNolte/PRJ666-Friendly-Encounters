using MongoDB.Bson;

public class User{

/*    private int UserId;
    private string UserName;
    private string UserPassword;
    private string UserEmail;
    private int PResetFlag;*/

    public ObjectId _id { set; get; }

    //public int ActiveConnection { set; get; }
    public string username { private set; get; }
    public string email { private set; get; }
    public string password { private set; get; }
}
