using UnityEngine;
using UnityEngine.UI;

public class MazeTimer : MonoBehaviour {

    public Text timer;
    private float startTime;
    private bool finished = false;

	// Use this for initialization
	void Start () {
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (!MyGameManager.pause)
        {
            if (finished)
            {
                return;
            }
            else
            {
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
    public void notFinished()
    {
        finished = false;
    }
}
