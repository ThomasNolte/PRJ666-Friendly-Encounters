using UnityEngine;
using UnityEngine.UI;

public class SoloTimer : MonoBehaviour {

    public Text timer;
    private float startTime;
    private bool finished = false;
    
    void Start()
    {
        startTime = Time.time;
    }
    
    void Update()
    {
        if (!MyGameManager.pause)
        {
            if (!finished){ 
                float t = Time.time - startTime;

                string minutes = ((int)t / 60).ToString();
                string seconds = (t % 60).ToString("f2");
                timer.text = minutes + ":" + seconds;
            }
        }

    }

    public void Finish()
    {
        finished = true;
        timer.color = Color.yellow;
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
}
