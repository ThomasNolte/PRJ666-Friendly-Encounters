using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour
{
    public Text rankText;
    public Text playerNameText;
    public Text miniGameNameText;
    public Text timeText;

    public void SetRank(int rank)
    {
        rankText.text = "RANK #" + rank;
    }
    public void SetPlayerName(string name)
    {
        playerNameText.text = "NAME: " + name;
    }
    public void SetMiniGameName(string name)
    {
        miniGameNameText.text = "MINIGAME: " + name;
    }
    public void SetTime(int min, int sec)
    {
        timeText.text = "TIME: " + string.Format("{0}:{1}", min.ToString("00"), sec.ToString("00"));
    }
}
