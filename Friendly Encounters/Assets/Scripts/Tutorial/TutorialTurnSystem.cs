using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTurnSystem : MonoBehaviour
{
    public static List<TutorialPlayer> players = new List<TutorialPlayer>();

    public static bool IsRunning = false;

    private int cardIndex = 0;
    private int currentSpace = 0;

    public Transform[] waypoints;
    public float playerMoveSpeed = 5f;
    [HideInInspector]
    public bool movePlayer;

    void Awake()
    {
        Array.Copy(GameObject.Find("Waypoints").GetComponentsInChildren<Transform>(), 1, waypoints, 0, GameObject.Find("Waypoints").GetComponentsInChildren<Transform>().Length - 1);
        IsRunning = true;
    }

    void Update()
    {

        if (movePlayer && players[0].GetComponent<TutorialPlayer>().WaypointIndex < waypoints.Length)
        {
            Debug.Log("Current space: " + currentSpace);
            players[0].transform.position = Vector2.MoveTowards(players[0].transform.position, waypoints[currentSpace].position, playerMoveSpeed * Time.deltaTime);
            if (players[0].transform.position == waypoints[currentSpace].position)
            {
                if (players[0].GetComponent<TutorialPlayer>().WaypointIndex == currentSpace)
                {
                    movePlayer = false;
                }
                currentSpace++;
            }
        }
    }

    public void MovePlayer(int index)
    {
        cardIndex = index + 1;
        currentSpace = players[0].GetComponent<TutorialPlayer>().WaypointIndex;
        players[0].GetComponent<TutorialPlayer>().WaypointIndex += cardIndex;
        if (players[0].GetComponent<TutorialPlayer>().WaypointIndex > waypoints.Length)
        {
            players[0].GetComponent<TutorialPlayer>().WaypointIndex = waypoints.Length - 1;
        }
        movePlayer = true;
    }
}
