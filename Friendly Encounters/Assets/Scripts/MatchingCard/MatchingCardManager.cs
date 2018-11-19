using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchingCardManager : MonoBehaviour
{

    private GameObject _firstCard = null;
    private GameObject _secondCard = null;

    private int _cardsLeft;

    private bool _canFlip = true;

    [SerializeField]
    private float _timeBetweenFlips = 0.75f;

    //Handling the scoring and gameover screen
    public GameObject winCanvas;
    public GameObject scoreCanvas;

    private SoloTimer timer;
    private Score score;

    public Text winText;

    void Start()
    {
        timer = FindObjectOfType<SoloTimer>();
        score = new Score();
    }


    public bool canFlip
    {
        get
        {
            return _canFlip;
        }
        set
        {
            _canFlip = value;
        }
    }

    public int cardsLeft
    {
        get
        {
            return _cardsLeft;
        }
        set
        {
            _cardsLeft = value;
        }
    }

    public void AddCard(GameObject card) //This function will be called from CardController class
    {
        if (_firstCard == null) //Adds first card
        {
            _firstCard = card;
        }
        else //Adds second card and checks if both cards match
        {
            _canFlip = false;
            _secondCard = card;

            if (CheckIfMatch())
            {
                DecreaseCardCount();

                StartCoroutine(DeactivateCards());
            }
            else
            {
                StartCoroutine(FlipCards());
            }
        }
    }

    IEnumerator ScoreScreen()
    {
        if (MyGameManager.GetUser().Name != "Guest")
        {
            score.PlayerName = MyGameManager.GetUser().Name;
            score.MiniGameName = "Matching Cards";
            score.Minutes = System.Convert.ToInt32(timer.Minutes);
            score.Seconds = System.Convert.ToInt32(timer.Seconds);
            scoreCanvas.GetComponent<AddScore>().Add(score);
        }
        yield return new WaitForSeconds(1.5f);
        winCanvas.SetActive(false);
        scoreCanvas.SetActive(true);
    }

    IEnumerator DeactivateCards()
    {
        yield return new WaitForSeconds(_timeBetweenFlips); //Wait so player can see flipped cards and not click to fast
        _firstCard.SetActive(false);
        _secondCard.SetActive(false);
        Reset();
    }

    IEnumerator FlipCards()
    {
        yield return new WaitForSeconds(_timeBetweenFlips); //Wait so player can see flipped cards and not click too fast
        _firstCard.GetComponent<CardController>().ChangeSide();
        _secondCard.GetComponent<CardController>().ChangeSide();
        Reset();
    }

    public void Reset()
    {
        _canFlip = true;
        _firstCard = null;
        _secondCard = null;
    }

    public void DecreaseCardCount()
    {
        _cardsLeft -= 2;
        if (_cardsLeft <= 0)
        {
            winCanvas.SetActive(true);
            timer.Finish();
            winText.text = "Finished Time: " + timer.GetFormatedTime();
            StartCoroutine(ScoreScreen());
        }
    }

    bool CheckIfMatch()
    {
        if (_firstCard.GetComponent<CardController>().cardName == _secondCard.GetComponent<CardController>().cardName)
        {
            return true;
        }

        return false;
    }

}
