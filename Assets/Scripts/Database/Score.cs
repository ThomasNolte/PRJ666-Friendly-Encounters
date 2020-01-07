using MongoDB.Bson;
public class Score
{
    public ObjectId _id { set; get;  }
    public string playName { set; get; }
    public string miniGameName { set; get;  }
    public int minutes { set; get; }
    public int seconds { set; get; }

    public override int GetHashCode()
    {
        int result = _id.GetHashCode();
        result = 31 * playName.GetHashCode();
        result = 31 * miniGameName.GetHashCode();
        result = 31 * minutes.GetHashCode();
        result = 31 * seconds.GetHashCode();
        return result;
    }

    public override bool Equals(object obj)
    {
        if (this == obj)
        {
            return true;
        }
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        Score that = (Score)obj;
        bool idEquals = Equals(_id, that._id);
        bool playNameEquals = Equals(playName, that.playName);
        bool miniGameNameEquals = Equals(miniGameName, that.miniGameName);
        bool minutesEquals = Equals(minutes, that.minutes);
        bool secondsEquals = Equals(seconds, that.seconds);
        return idEquals && playNameEquals && miniGameNameEquals && minutesEquals && secondsEquals;
    }
}
