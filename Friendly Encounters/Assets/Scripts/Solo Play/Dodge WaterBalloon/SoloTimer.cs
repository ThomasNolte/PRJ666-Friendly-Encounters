using UnityEngine;
using UnityEngine.UI;

public class SoloTimer : MonoBehaviour {

    public Text timer;
    private float startTime;
    private string minutes;
    private string seconds;
    private bool finished = false;
    
    void Start()
    {
        startTime = 0;
    }
    
    void Update()
    {
        if (!MyGameManager.pause)
        {
            if (!finished){
                startTime += Time.deltaTime;

                minutes = Mathf.Floor(startTime / 60).ToString("00");
                seconds = (startTime % 60).ToString("00");

                timer.text = string.Format("{0}:{1}", minutes, seconds);
            }
        }

    }

    public string GetFormatedTime()
    {
        return string.Format("{0}:{1}", minutes, seconds);
    }

    public void Finish()
    {
        finished = true;
        timer.color = Color.yellow;
        startTime = 0;
    }
    

    public bool Finished
    {
        get
        {
            return finished;
        }
        set
        {
            finished = value;
        }
    }

    public string Minutes
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

    public string Seconds
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
}
