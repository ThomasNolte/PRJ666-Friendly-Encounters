﻿using System.Collections;
using UnityEngine;

public class SoloWaterBalloonSpawner : MonoBehaviour
{
    public static bool gameOver = false;

    public GameObject playerPrefab;
    public GameObject gameOverCanvas;
    public GameObject scoreCanvas;
    public GameObject block;
    public Transform[] spawnPoints;
    public Transform topLeft;
    public Transform topRight;
    public Transform bottomLeft;
    public Transform bottomRight;

    private TutorialMiniGameManager manager;
    private SoloTimer timer;
    private Score score;
    private bool spawning;
    private bool reset;

    void Awake()
    {
        manager = FindObjectOfType<TutorialMiniGameManager>();
        timer = FindObjectOfType<SoloTimer>();
        score = new Score();
        Init();
    }

    void Init()
    {
        spawning = true;
        gameOverCanvas.SetActive(false);
        gameOver = false;
        Instantiate(playerPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity, null);
        StartCoroutine(SpawnWaterBalloon());
    }

    void FixedUpdate()
    {
        if (!MyGameManager.pause)
        {
            if (gameOver)
            {
                spawning = false;
                gameOver = false;
                gameOverCanvas.SetActive(true);
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
            //This is only for the main game
            //For solo play changing screens will reset
            if (reset)
            {
                Init();
                reset = false;
            }
        }
    }

    IEnumerator ScoreScreen()
    {
        if (MyGameManager.GetUser().Name != "Guest") {
            score.PlayerName = MyGameManager.GetUser().Name;
            score.MiniGameName = "Water Balloon";
            score.Minutes = System.Convert.ToInt32(timer.Minutes);
            score.Seconds = System.Convert.ToInt32(timer.Seconds);
            scoreCanvas.GetComponent<AddScore>().Add(score);
        }
        yield return new WaitForSeconds(1.5f);
        gameOverCanvas.SetActive(false);
        scoreCanvas.SetActive(true);
    }

    IEnumerator BackToMainGame()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        yield return new WaitForSeconds(2.5f);
        gameOverCanvas.SetActive(false);
        manager.IsMiniGameFinished = true;
        reset = true;
    }

    IEnumerator SpawnWaterBalloon()
    {
        const float MIN_TIME = 1.0f;
        const float MAX_TIME = 1.5f;

        while (spawning)
        {
            int spawnSide = Random.Range(0, 4);
            yield return new WaitForSeconds(Random.Range(MIN_TIME, MAX_TIME));
            
            Vector2 spawnPosition;
            GameObject obj;

            switch (spawnSide)
            {
                case 0:
                    spawnPosition = new Vector2(Random.Range(topLeft.position.x, topRight.position.x), topLeft.position.y);
                    obj = Instantiate(block, spawnPosition, Quaternion.identity, transform);
                    obj.GetComponent<SoloWaterBalloon>().MovePosition = Vector2.down * 2;
                    break;
                case 1:
                    spawnPosition = new Vector2(Random.Range(bottomLeft.position.x, bottomRight.position.x), bottomLeft.position.y);
                    obj = Instantiate(block, spawnPosition, Quaternion.Euler(0, 0, 180), transform);
                    obj.GetComponent<SoloWaterBalloon>().MovePosition = Vector2.up * 2;
                    break;
                case 2:
                    spawnPosition = new Vector2(topLeft.position.x, Random.Range(topLeft.position.y, bottomLeft.position.y));
                    obj = Instantiate(block, spawnPosition, Quaternion.Euler(0, 0, 90), transform);
                    obj.GetComponent<SoloWaterBalloon>().MovePosition = Vector2.right * 2;
                    break;
                case 3:
                    spawnPosition = new Vector2(bottomRight.position.x, Random.Range(topRight.position.y, bottomRight.position.y));
                    obj = Instantiate(block, spawnPosition, Quaternion.Euler(0, 0, -90), transform);
                    obj.GetComponent<SoloWaterBalloon>().MovePosition = Vector2.left * 2;
                    break;
            }
        }
    }
}
