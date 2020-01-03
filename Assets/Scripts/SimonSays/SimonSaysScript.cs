using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SimonSaysScript : MonoBehaviour
{
    public GameObject[] colors;
    public GameObject arrowRight, arrowLeft;
    public Button[] buttons;
    //public Button test;
    public Text playerscore;
    public int[] seq;
    public int[] userseq;
    public int counter = 0, repeat = 3, round; //repeat = how many colors in each round, round = how many rounds in total
    public int score;

    public GameObject gameOverCanvas;
    public GameObject scoreCanvas;

    private TutorialMiniGameManager manager;
    private SoloTimer timer;
    private Score data;
    private bool gameOver;
    private bool reset;

    void Awake()
    {
        manager = FindObjectOfType<TutorialMiniGameManager>();
        if (manager == null) {
            timer = FindObjectOfType<SoloTimer>();
        }
        data = new Score();
    }

    void Start()
    {
        Init();
        buttons[0].onClick.AddListener(blueOnClick);
        buttons[1].onClick.AddListener(redOnClick);
        buttons[2].onClick.AddListener(greenOnClick);
    }

    void Update()
    {
        if (gameOver)
        {
            gameOver = false;
            gameOverCanvas.SetActive(true);
            if (manager != null)
            {
                StartCoroutine(BackToMainGame());
            }
            else
            {
                timer.Finish();
                StartCoroutine(ScoreScreen());
            }
        }

        if (reset)
        {
            Init();
        }
    }

    private void Init()
    {
        round = 4; //4 rounds
        score = 0;
        playerscore.text = score.ToString();
        seq = new int[repeat];
        userseq = new int[repeat];
        float time = 1, time2 = 5;

        buttons[0].interactable = false;
        buttons[1].interactable = false;
        buttons[2].interactable = false;
        gameOver = false;

        StartCoroutine(delay(time, time2));

    }

    IEnumerator ScoreScreen()
    {
/*        if (MyGameManager.user.Name != "Guest")
        {
            data.PlayerName = MyGameManager.user.Name;
            data.MiniGameName = "Simon Says";
            data.Minutes = System.Convert.ToInt32(timer.Minutes);
            data.Seconds = System.Convert.ToInt32(timer.Seconds);
            scoreCanvas.GetComponent<AddScore>().Add(data);
        }*/
        scoreCanvas.GetComponent<FindScore>().LookUpScores("Simon Says");
        yield return new WaitForSeconds(1.5f);
        gameOverCanvas.SetActive(false);
        scoreCanvas.SetActive(true);
    }

    IEnumerator BackToMainGame()
    {
        yield return new WaitForSeconds(2.5f);
        gameOverCanvas.SetActive(false);
        manager.IsMiniGameFinished = true;
        reset = true;
    }

    public void blueOnClick()
    {

        if (counter == repeat - 1)
        {
            buttons[0].interactable = false;
            buttons[1].interactable = false;
            buttons[2].interactable = false;

        }
        if (counter < repeat)
        {
            userseq[counter] = 0;
            counter++;
        }

    }

    public void redOnClick()
    {

        if (counter == repeat - 1)
        {
            buttons[0].interactable = false;
            buttons[1].interactable = false;
            buttons[2].interactable = false;

        }
        if (counter < repeat)
        {
            userseq[counter] = 1;
            counter++;
        }

    }

    public void greenOnClick()
    {

        if (counter == repeat - 1)
        {
            buttons[0].interactable = false;
            buttons[1].interactable = false;
            buttons[2].interactable = false;

        }
        if (counter < repeat)
        {
            userseq[counter] = 2;
            counter++;
        }


    }

    public void testOnClick()
    {
        for (int i = 0; i < repeat; i++)
        {
            Debug.Log("clicked:" + userseq[i]);

        }

    }

    IEnumerator delay(float time, float time2)
    {
        for (int x = 0; x < round && !gameOver; x++) //each round
        {
            Debug.Log("round:" + x);
            yield return new WaitForSeconds(time * 2);
            for (int i = 0; i < repeat; i++)
            {
                int pick = Random.Range(0, 3);
                seq[i] = pick;
                yield return new WaitForSeconds(time / 3);

                colors[pick].GetComponent<Renderer>().enabled = true;
                yield return new WaitForSeconds(time / 2);
                colors[pick].GetComponent<Renderer>().enabled = false;

            }
            buttons[0].interactable = true;
            buttons[1].interactable = true;
            buttons[2].interactable = true;
            arrowLeft.GetComponent<Renderer>().enabled = true;
            arrowRight.GetComponent<Renderer>().enabled = true;
            yield return new WaitForSeconds(time2);
            buttons[0].interactable = false;
            buttons[1].interactable = false;
            buttons[2].interactable = false;
            arrowLeft.GetComponent<Renderer>().enabled = false;
            arrowRight.GetComponent<Renderer>().enabled = false;

            for (int i = 0; i < repeat; i++)
            {
                Debug.Log("game:" + seq[i]);

            }

            if (seq.SequenceEqual(userseq))
            {
                score = score + 20;
                playerscore.text = score.ToString();
            }
            else
            {
                gameOver = true;
                Debug.Log("you lose");
            }
            counter = 0;
            repeat++;
            seq = new int[repeat];
            userseq = new int[repeat];
        }

        Debug.Log("you winn!");
    }
}



