using MongoDB.Bson;
public class Score
{
    public ObjectId _id { set; get;  }
    public string playName { set; get; }
    public string miniGameName { set; get;  }
    public int minutes { set; get; }
    public int seconds { set; get; }
}
