using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WaterBalloonSpawner : NetworkBehaviour
{
    static public List<NetworkPlayer> players = new List<NetworkPlayer>();
    static public WaterBalloonSpawner instance = null;

    public GameObject block;
    public Transform topLeft;
    public Transform topRight;

    private bool spawning = true;

    void Awake()
    {
        instance = this;
        enabled = false;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (isServer)
        {
            StartCoroutine(SpawnWaterBalloon());
        }
        foreach (NetworkPlayer player in players)
        {
            player.Col.enabled = true;
        }
    }

    IEnumerator SpawnWaterBalloon()
    {
        const float MIN_TIME = 1.0f;
        const float MAX_TIME = 1.5f;

        while (spawning)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(MIN_TIME, MAX_TIME));

            Vector2 spawnPosition = new Vector2(UnityEngine.Random.Range(topLeft.position.x, topRight.position.x), topLeft.position.y);
            GameObject obj = Instantiate(block, spawnPosition, Quaternion.identity) as GameObject;
            NetworkServer.Spawn(obj);
        }
    }

    /*
    [ServerCallback]
    void Update()
    {
    }
    */

    public override void OnStartClient()
    {
        base.OnStartClient();
        ClientScene.RegisterPrefab(block);
    }
}
