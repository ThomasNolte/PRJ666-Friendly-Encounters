using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMiniGameManager : MonoBehaviour
{
    public const int MAXSTATES = 5;

    public GameObject[] images;

    public GameObject simonsaysContainer;
    public GameObject coinCollectorContainer;
    public GameObject dodgeWaterBalloonContainer;
    public GameObject matchingCardsContainer;
    public GameObject mazeContainer;

    private TutorialTurnSystem playManager;
    private int miniGameSelected = -1;
    private bool isMiniGameFinished = false;
    private bool isBaseGame = false;

    void Awake()
    {
        playManager = FindObjectOfType<TutorialTurnSystem>();
    }

    public enum MiniGameState
    {
        SIMONSAYS,
        COINCOLLECTOR,
        DODGEWATERBALLOON,
        MATCHINGCARDS,
        MAZE
    }

    private void Update()
    {
        if (!MyGameManager.pause)
        {
            if (miniGameSelected != -1)
            {
                isBaseGame = true;
                switch (miniGameSelected)
                {
                    case (int)MiniGameState.SIMONSAYS:
                        dodgeWaterBalloonContainer.SetActive(true);
                        break;
                    case (int)MiniGameState.COINCOLLECTOR:
                        coinCollectorContainer.SetActive(true);
                        break;
                    case (int)MiniGameState.DODGEWATERBALLOON:
                        dodgeWaterBalloonContainer.SetActive(true);
                        break;
                    case (int)MiniGameState.MATCHINGCARDS:
                        dodgeWaterBalloonContainer.SetActive(true);
                        break;
                    case (int)MiniGameState.MAZE:
                        dodgeWaterBalloonContainer.SetActive(true);
                        break;
                }
            }


            if (IsMiniGameFinished)
            {
                TurnOffMiniGames();
                playManager.ShowGame(true);
                Camera.main.orthographicSize = 2;
                Camera.main.gameObject.GetComponent<TutorialCamera>().ResetPositionToFirstPlayer();
                miniGameSelected = -1;
                playManager.IsMiniGameRunning = false;
                IsMiniGameFinished = false;
            }
        }
    }

    public void TurnOffMiniGames()
    {
        simonsaysContainer.SetActive(false);
        coinCollectorContainer.SetActive(false);
        dodgeWaterBalloonContainer.SetActive(false);
        matchingCardsContainer.SetActive(false);
        mazeContainer.SetActive(false);
    }

    public void RollGame()
    {
        StartCoroutine("RollMiniGame");
    }

    public IEnumerator RollMiniGame()
    {

        int index = -1;
        Color prevColor = Color.white;
        for (int i = 0; i < 20; i++)
        {
            index = Random.Range(0, MAXSTATES);
            prevColor = images[index].GetComponentInChildren<Image>().color;
            images[index].GetComponentInChildren<Image>().color = new Color(255, 0, 0, 50);
            yield return new WaitForSeconds(0.30f);
            images[index].GetComponentInChildren<Image>().color = prevColor;
        }
        images[index].GetComponentInChildren<Image>().color = new Color(255, 0, 0, 50);

        //Wait and let the player see what was selected
        yield return new WaitForSeconds(2f);
        miniGameSelected = index;
        playManager.miniGamePanel.SetActive(false);
        images[index].GetComponentInChildren<Image>().color = prevColor;
    }

    public int MiniGameSelected
    {
        get
        {
            return miniGameSelected;
        }
        set
        {
            miniGameSelected = value;
        }
    }

    public bool IsMiniGameFinished
    {
        get
        {
            return isMiniGameFinished;
        }
        set
        {
            isMiniGameFinished = value;
        }
    }
    public bool IsBaseGame
    {
        get
        {
            return isBaseGame;
        }
        set
        {
            isBaseGame = value;
        }
    }
}
