using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloWaterBalloonSpawner : MonoBehaviour
{
    static public List<SoloPlayerController> players = new List<SoloPlayerController>();
    public static SoloWaterBalloonSpawner instance = null;

    public GameObject block;
    public Transform topLeft;
    public Transform topRight;

    private bool spawning = true;

    void Start()
    {
        StartCoroutine(SpawnWaterBalloon());
    }

    IEnumerator SpawnWaterBalloon()
    {
        const float MIN_TIME = 1.0f;
        const float MAX_TIME = 1.5f;

        while (spawning)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(MIN_TIME, MAX_TIME));

            Vector2 spawnPosition = new Vector2(UnityEngine.Random.Range(topLeft.position.x, topRight.position.x), topLeft.position.y);
            Instantiate(block, spawnPosition, Quaternion.identity);
        }
    }
}
