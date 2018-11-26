using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WaterBalloonSpawner : NetworkBehaviour
{
    static public List<NetworkPlayer> players = new List<NetworkPlayer>();

    public const float MIN_TIME = 0.9f;
    public const float MAX_TIME = 0.3f;
    public const float SPEED = 500.0f;

    public GameObject block;
    public GameObject gameOverCanvas;
    public Transform[] spawnPoints;
    public Transform topLeft;
    public Transform topRight;
    public Transform bottomLeft;
    public Transform bottomRight;

    private float nextSpawnTime;

    private bool gameOver = false;
    private bool spawning = true;

    void Awake()
    {
        //enabled = false;
    }

    [ServerCallback]
    void Start()
    {
        int i = 0;
        foreach (NetworkPlayer player in players)
        {
            player.Col.enabled = true;
            player.transform.position = spawnPoints[i++].position;
        }
    }

    [ServerCallback]
    void FixedUpdate()
    {
        if (spawning)
        {
            if (Time.time > nextSpawnTime)
            {
                int spawnSide = UnityEngine.Random.Range(0, 4);
                float secondsBetweenSpawns = Mathf.Lerp(MIN_TIME, MAX_TIME, Difficulty.GetDifficultyPercent());
                nextSpawnTime = Time.time + secondsBetweenSpawns;

                Vector2 spawnPosition;
                GameObject obj;

                switch (spawnSide)
                {
                    case 0:
                        spawnPosition = new Vector2(UnityEngine.Random.Range(topLeft.position.x, topRight.position.x), topLeft.position.y);
                        obj = Instantiate(block, spawnPosition, Quaternion.identity, transform);
                        obj.GetComponent<WaterBalloon>().Init(Vector2.down * SPEED);
                        NetworkServer.Spawn(obj);
                        break;
                    case 1:
                        spawnPosition = new Vector2(UnityEngine.Random.Range(bottomLeft.position.x, bottomRight.position.x), bottomLeft.position.y);
                        obj = Instantiate(block, spawnPosition, Quaternion.Euler(0, 0, 180), transform);
                        obj.GetComponent<WaterBalloon>().Init(Vector2.up * SPEED);
                        NetworkServer.Spawn(obj);
                        break;
                    case 2:
                        spawnPosition = new Vector2(topLeft.position.x, UnityEngine.Random.Range(topLeft.position.y, bottomLeft.position.y));
                        obj = Instantiate(block, spawnPosition, Quaternion.Euler(0, 0, 90), transform);
                        obj.GetComponent<WaterBalloon>().Init(Vector2.right * SPEED);
                        NetworkServer.Spawn(obj);
                        break;
                    case 3:
                        spawnPosition = new Vector2(bottomRight.position.x, UnityEngine.Random.Range(topRight.position.y, bottomRight.position.y));
                        obj = Instantiate(block, spawnPosition, Quaternion.Euler(0, 0, -90), transform);
                        obj.GetComponent<WaterBalloon>().Init(Vector2.left * SPEED);
                        NetworkServer.Spawn(obj);
                        break;
                }
            }
        }

        if (gameOver)
        {
            spawning = false;
            gameOver = false;
            gameOverCanvas.SetActive(true);
            //timer.Finish();
            StartCoroutine(BackToMainGame());
        }
    }

    IEnumerator BackToMainGame()
    {
        foreach (Transform child in transform)
        {
            NetworkServer.Destroy(child.gameObject);
        }
        yield return new WaitForSeconds(2.5f);
        gameOverCanvas.SetActive(false);
        //manager.IsMiniGameFinished = true;
        //reset = true;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        ClientScene.RegisterPrefab(block);
    }
}
