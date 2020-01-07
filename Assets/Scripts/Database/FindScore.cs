using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindScore : MonoBehaviour {

    MyMongoDB db = new MyMongoDB();
    public GameObject scorePrefab;
    public GameObject scorePanel;
    
    public Button closeButton;
    
    private List<Score> scores = new List<Score>();
    private List<GameObject> scorePrefabs = new List<GameObject>();

    void Start()
    {
        closeButton.onClick.AddListener(StartClose);
        if (MyGameManager.currentSceneIndex == (int)MyGameManager.STATES.MINIGAMESTATE)
        {
            closeButton.GetComponentInChildren<Text>().text = "CLOSE";
        }
        else
        {
            closeButton.GetComponentInChildren<Text>().text = "MINIGAME MENU";
        }
    }

    private void StartClose()
    {
        if (MyGameManager.currentSceneIndex == (int)MyGameManager.STATES.MINIGAMESTATE)
        {
            gameObject.SetActive(false);
        }
        else
        {
            MyGameManager.instance.MyLoadScene((int)MyGameManager.STATES.MINIGAMESTATE);
        }
    }

    public void GetAllScores()
    {
        scores.Clear();
        for (int i = scorePrefabs.Count - 1; i >= 0; i--)
        {
            Destroy(scorePrefabs[i]);
        }
        scorePrefabs.Clear();

        scores = db.AllScores();

        int rank = 1;
        foreach (Score s in scores)
        {
            GameObject tempScore = Instantiate(scorePrefab, scorePanel.transform);
            tempScore.GetComponent<ScoreHandler>().SetRank(rank);
            tempScore.GetComponent<ScoreHandler>().SetPlayerName(s.playName);
            tempScore.GetComponent<ScoreHandler>().SetMiniGameName(s.miniGameName);
            tempScore.GetComponent<ScoreHandler>().SetTime(s.minutes, s.seconds);
            scorePrefabs.Add(tempScore);
            rank++;
        }
    }

    public void LookUpScores(string minigameName)
    {
        scores.Clear();
        for (int i = scorePrefabs.Count - 1; i >= 0; i--)
        {
            Destroy(scorePrefabs[i]);
        }
        scorePrefabs.Clear();

        scores = db.FindMiniGamesScores(minigameName);

        int rank = 1;
        foreach (Score s in scores)
        {
            GameObject tempScore = Instantiate(scorePrefab, scorePanel.transform);
            tempScore.GetComponent<ScoreHandler>().SetRank(rank);
            tempScore.GetComponent<ScoreHandler>().SetPlayerName(s.playName);
            tempScore.GetComponent<ScoreHandler>().SetMiniGameName(s.miniGameName);
            tempScore.GetComponent<ScoreHandler>().SetTime(s.minutes, s.seconds);
            scorePrefabs.Add(tempScore);
            rank++;
        }
    }
}
