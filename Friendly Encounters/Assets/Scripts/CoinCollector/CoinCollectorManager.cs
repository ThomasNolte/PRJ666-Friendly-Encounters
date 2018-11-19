using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CoinCollectorManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform[] spawnPoints;
    public Text countText;          //Store a reference to the UI Text component which will display the number of pickups collected.
    public Text winText;            //Store a reference to the UI Text component which will display the 'You win' message.

    public GameObject scoreCanvas;

    public static int count;              //Integer to store the number of pickups collected so far.

    private bool gameOver;

    private TutorialMiniGameManager manager;
    private SoloTimer timer;
    private Score score;
    private bool reset;

    // Use this for initialization
    void Start()
    {
        manager = FindObjectOfType<TutorialMiniGameManager>();
        timer = FindObjectOfType<SoloTimer>();
        score = new Score();
        Init();
        //Call our SetCountText function which will update the text with the current value for count.
        SetCountText();
    }

    void Init()
    {
        //Initialize count to zero.
        count = 0;
        //Initialze winText to a blank string since we haven't won yet at beginning.
        winText.text = "";
        gameOver = false;
        reset = false;
        Instantiate(playerPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity, null);
    }

    void Update()
    {
        if (!MyGameManager.pause && !gameOver)
        {
            SetCountText();

            if (gameOver)
            {
                Destroy(FindObjectOfType<SoloPlayerController>().gameObject);
                timer.Finish();
                if (manager != null)
                {
                    StartCoroutine(BackToMainGame());
                }
                else
                {
                    StartCoroutine(ScoreScreen());
                }
            }

            if (reset)
            {
                Init();
                reset = false;
            }
        }
    }

    IEnumerator ScoreScreen()
    {
        if (MyGameManager.GetUser().Name != "Guest")
        {
            score.PlayerName = MyGameManager.GetUser().Name;
            score.MiniGameName = "Coin Collector";
            score.Minutes = System.Convert.ToInt32(timer.Minutes);
            score.Seconds = System.Convert.ToInt32(timer.Seconds);
            scoreCanvas.GetComponent<AddScore>().Add(score);
        }
        yield return new WaitForSeconds(1.5f);
        winText.text = "";
        scoreCanvas.SetActive(true);
    }

    //This function updates the text displaying the number of objects we've collected and displays our victory message if we've collected all of them.
    void SetCountText()
    {
        //Set the text property of our our countText object to "Count: " followed by the number stored in our count variable.
        countText.text = "Count: " + count.ToString() + "/12";

        //Check if we've collected all 12 pickups. If we have...
        if (count >= 12 && !gameOver)
        {
            //... then set the text property of our winText object to "You win!"
            winText.text = "You win!";
            gameOver = true;
        }
    }

    IEnumerator BackToMainGame()
    { 
        yield return new WaitForSeconds(2.5f);
        manager.IsMiniGameFinished = true;
        reset = true;
    }

    public bool GameOver
    {
        get
        {
            return gameOver;
        }

        set
        {
            gameOver = value;
        }
    }

}
