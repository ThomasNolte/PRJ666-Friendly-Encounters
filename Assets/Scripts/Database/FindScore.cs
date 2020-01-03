using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindScore : MonoBehaviour {

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
/*        SSH ssh = new SSH();
        ssh.Initialize("myvmlab.senecacollege.ca", 6265, "student", "frndly02", 3306);
        ssh.OpenSSHConnection();
        ssh.OpenPort();

        ssh.mysql.Initialize("127.0.0.1", Convert.ToString(ssh.boundport), "FriendlyEncounters", "student", "frndly02");

        scores = ssh.mysql.SQLSelectAllScores();

        ssh.CloseSSHConnection();*/

        int rank = 1;
        foreach (Score s in scores)
        {
            GameObject tempScore = Instantiate(scorePrefab, scorePanel.transform);
            tempScore.GetComponent<ScoreHandler>().SetRank(rank);
            tempScore.GetComponent<ScoreHandler>().SetPlayerName(s.PlayerName);
            tempScore.GetComponent<ScoreHandler>().SetMiniGameName(s.MiniGameName);
            tempScore.GetComponent<ScoreHandler>().SetTime(s.Minutes, s.Seconds);
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
/*        SSH ssh = new SSH();
        ssh.Initialize("myvmlab.senecacollege.ca", 6265, "student", "frndly02", 3306);
        ssh.OpenSSHConnection();
        ssh.OpenPort();

        ssh.mysql.Initialize("127.0.0.1", Convert.ToString(ssh.boundport), "FriendlyEncounters", "student", "frndly02");

        scores = ssh.mysql.SQLSelectScore(minigameName);

        ssh.CloseSSHConnection();*/

        int rank = 1;
        foreach (Score s in scores)
        {
            GameObject tempScore = Instantiate(scorePrefab, scorePanel.transform);
            tempScore.GetComponent<ScoreHandler>().SetRank(rank);
            tempScore.GetComponent<ScoreHandler>().SetPlayerName(s.PlayerName);
            tempScore.GetComponent<ScoreHandler>().SetMiniGameName(s.MiniGameName);
            tempScore.GetComponent<ScoreHandler>().SetTime(s.Minutes, s.Seconds);
            scorePrefabs.Add(tempScore);
            rank++;
        }
    }
}
