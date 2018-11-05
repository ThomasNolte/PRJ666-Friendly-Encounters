using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayManager : NetworkBehaviour
{
    public static PlayManager instance = null;
    static public List<NetworkPlayer> players = new List<NetworkPlayer>();

    public static bool IsRunning = false;

    //[SyncVar]
    //private int turnIndex = 0;
    private int maxTurns = 0;
    private int cardIndex = 0;
    private int currentSpace = 0;

    //private bool turnFinished = false;

    public Transform[] waypoints;
    public float playerMoveSpeed = 5f;
    [HideInInspector]
    public bool movePlayer;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        enabled = false;
        IsRunning = true;
        DontDestroyOnLoad(this);
    }

    [ServerCallback]
    void Update()
    {
        Init();

        if (movePlayer && players[0].GetComponent<NetworkPlayer>().WaypointIndex < waypoints.Length)
        {
            players[0].transform.position = Vector2.MoveTowards(players[0].transform.position, waypoints[currentSpace].position, playerMoveSpeed * Time.deltaTime);
            if (players[0].transform.position == waypoints[currentSpace].position)
            {
                if (players[0].GetComponent<NetworkPlayer>().WaypointIndex == currentSpace)
                {
                    //if (turnIndex == maxTurns)
                    //{
                    //    turnIndex = 0;
                    //}
                    //else {
                    //    turnIndex++;
                    //}
                    movePlayer = false;
                }
                currentSpace++;
            }
        }

        if (PlayScoreBoard.IsPlayState)
        {
            foreach (NetworkPlayer player in players)
            {
                player.gameObject.transform.position = waypoints[0].position;
                maxTurns++;
            }
            PlayScoreBoard.IsPlayState = false;
        }
    }

    void Init()
    {
        if (PlayScoreBoard.IsPlayState)
        {
            Array.Copy(GameObject.Find("Waypoints").GetComponentsInChildren<Transform>(), 1, waypoints, 0, GameObject.Find("Waypoints").GetComponentsInChildren<Transform>().Length - 1);

        }
    }

    IEnumerator SpendTurn()
    {
        yield return new WaitForSeconds(0.5f);
    }

    public void MovePlayer(int index)
    {
        cardIndex = index + 1;
        currentSpace = players[0].GetComponent<NetworkPlayer>().WaypointIndex;
        players[0].GetComponent<NetworkPlayer>().WaypointIndex += cardIndex;
        if (players[0].GetComponent<NetworkPlayer>().WaypointIndex > waypoints.Length)
        {
            players[0].GetComponent<NetworkPlayer>().WaypointIndex = waypoints.Length - 1;
        }
        movePlayer = true;
    }
}
