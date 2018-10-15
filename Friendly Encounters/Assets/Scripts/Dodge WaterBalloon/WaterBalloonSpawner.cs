using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBalloonSpawner : MonoBehaviour {

    public GameObject block;
    public Vector2 secondsBetweenSpawnsMinMax;
    float nextSpawnTime;

    public Transform topLeft;
    public Transform topRight;

    public Vector2 spawnSizeMinMax;
	
	void Update () {
        if (Time.time > nextSpawnTime)
        {
            float secondsBetweenSpawns = Mathf.Lerp(secondsBetweenSpawnsMinMax.y, secondsBetweenSpawnsMinMax.x, Difficulty.GetDifficultyPercent());
            nextSpawnTime = Time.time + secondsBetweenSpawns;
            
            float spawnSize = Random.Range(spawnSizeMinMax.x, spawnSizeMinMax.y);
            Vector2 spawnPosition = new Vector2(Random.Range(topLeft.position.x, topRight.position.x), topLeft.position.y + spawnSize);
            Instantiate(block, spawnPosition, Quaternion.identity);
            //newBlock.transform.localScale = Vector2.one * spawnSize;
        }
    }
}
