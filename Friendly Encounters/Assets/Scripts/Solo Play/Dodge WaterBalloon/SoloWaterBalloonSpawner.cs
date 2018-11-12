using System.Collections;
using UnityEngine;

public class SoloWaterBalloonSpawner : MonoBehaviour
{
    public static bool gameOver = false;
    
    public GameObject gameOverText;
    public GameObject block;
    public Transform topLeft;
    public Transform topRight;
    public Transform bottomLeft;
    public Transform bottomRight;

    private TutorialMiniGameManager manager;
    private bool spawning;

    void Awake()
    {
        manager = FindObjectOfType<TutorialMiniGameManager>();
        spawning = true;
        gameOverText.SetActive(false);
        gameOver = false;
    }

    void Start()
    {
        StartCoroutine(SpawnWaterBalloon());
    }

    void FixedUpdate()
    {
        if (gameOver)
        {
            spawning = false;
            gameOverText.SetActive(true);
            if (manager != null)
            {
                StartCoroutine(BackToMainGame());
            }
        }
    }

    IEnumerator BackToMainGame()
    {
        gameOver = false;
        yield return new WaitForSeconds(2f);
        gameOverText.SetActive(false);
        manager.IsMiniGameFinished = true;
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
                    obj = Instantiate(block, spawnPosition, Quaternion.identity);
                    obj.GetComponent<SoloWaterBalloon>().MovePosition = Vector2.down * 2;
                    break;
                case 1:
                    spawnPosition = new Vector2(Random.Range(bottomLeft.position.x, bottomRight.position.x), bottomLeft.position.y);
                    obj = Instantiate(block, spawnPosition, Quaternion.Euler(0, 0, 180));
                    obj.GetComponent<SoloWaterBalloon>().MovePosition = Vector2.up * 2;
                    break;
                case 2:
                    spawnPosition = new Vector2(topLeft.position.x, Random.Range(topLeft.position.y, bottomLeft.position.y));
                    obj = Instantiate(block, spawnPosition, Quaternion.Euler(0, 0, 90));
                    obj.GetComponent<SoloWaterBalloon>().MovePosition = Vector2.right * 2;
                    break;
                case 3:
                    spawnPosition = new Vector2(bottomRight.position.x, Random.Range(topRight.position.y, bottomRight.position.y));
                    obj = Instantiate(block, spawnPosition, Quaternion.Euler(0, 0, -90));
                    obj.GetComponent<SoloWaterBalloon>().MovePosition = Vector2.left * 2;
                    break;
            }
        }
    }
}
