using UnityEngine;
using System.Collections;

public class MazeGoal : MonoBehaviour
{
    public Sprite closedGoalSprite;
    public Sprite openedGoalSprite;

    public SoloTimer timer;

    private GameObject scoreCanvas;
    private Score score;

    bool openDoor = false;

    void Start() {
        GetComponentInChildren<SpriteRenderer>().sprite = closedGoalSprite;
        timer = FindObjectOfType<SoloTimer>();
        scoreCanvas = FindObjectOfType<AddScore>().gameObject;
        scoreCanvas.SetActive(false);
        score = new Score();
        openDoor = false;
    }

    public void OpenGoal() {
        GetComponentInChildren<SpriteRenderer>().sprite = openedGoalSprite;
        openDoor = true;
    }

    void OnTriggerEnter2D() {
        if (openDoor == true)
        {
            timer.Finish();
            Destroy(FindObjectOfType<MazeRunner>().gameObject);
            StartCoroutine(ScoreScreen());
        }
    }

    IEnumerator ScoreScreen()
    {
        if (MyGameManager.user.Name != "Guest")
        {
            score.PlayerName = MyGameManager.user.Name;
            score.MiniGameName = "Maze";
            score.Minutes = System.Convert.ToInt32(timer.Minutes);
            score.Seconds = System.Convert.ToInt32(timer.Seconds);
            scoreCanvas.GetComponent<AddScore>().Add(score);
        }
        scoreCanvas.GetComponent<FindScore>().LookUpScores("Maze");
        yield return new WaitForSeconds(1.5f);
        scoreCanvas.SetActive(true);
    }
}