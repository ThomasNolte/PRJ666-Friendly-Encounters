public class Score
{
    private string playName;
    private string miniGameName;
    private int minutes;
    private int seconds;

    public Score()
    {
        playName = "";
        miniGameName = "";
        minutes = -1;
        seconds = -1;
    }

    public int Seconds
    {
        get
        {
            return seconds;
        }

        set
        {
            seconds = value;
        }
    }

    public int Minutes
    {
        get
        {
            return minutes;
        }

        set
        {
            minutes = value;
        }
    }

    public string MiniGameName
    {
        get
        {
            return miniGameName;
        }

        set
        {
            miniGameName = value;
        }
    }

    public string PlayerName
    {
        get
        {
            return playName;
        }

        set
        {
            playName = value;
        }
    }
}
