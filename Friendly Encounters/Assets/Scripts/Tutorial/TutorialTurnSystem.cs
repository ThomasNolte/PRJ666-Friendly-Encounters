using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTurnSystem : MonoBehaviour
{
    public static List<TutorialPlayer> players = new List<TutorialPlayer>();

    public GameObject miniGamePanel;

    private TutorialPointSystem pointSystem;

    private int playerTurnIndex = 0;
    private int currentRound = 0;
    private int maxTurns = 15;
    private int cardIndex = 0;
    private int currentSpace = 0;

    private bool turnFinished;
    private bool roundFinished;
    private bool isMiniGameRunning;

    public Transform[] waypoints;
    public float playerMoveSpeed = 5f;
    [HideInInspector]
    public bool movePlayer;

    void Awake()
    {
        pointSystem = FindObjectOfType<TutorialPointSystem>();
        Array.Copy(GameObject.Find("Waypoints").GetComponentsInChildren<Transform>(), 1, waypoints, 0, GameObject.Find("Waypoints").GetComponentsInChildren<Transform>().Length - 1);
    }

    void Update()
    {
        if (roundFinished) {
            IsMiniGameRunning = true;
            miniGamePanel.SetActive(true);
            roundFinished = false;
        }
        else {
        //Check if player is allowed to move
        if (movePlayer && players[playerTurnIndex].GetComponent<TutorialPlayer>().WaypointIndex < waypoints.Length && !turnFinished && !IsMiniGameRunning)
            {
                //Smooth player moving transition
                players[playerTurnIndex].transform.position = Vector2.MoveTowards(players[playerTurnIndex].transform.position, waypoints[currentSpace].position, playerMoveSpeed * Time.deltaTime);
                players[playerTurnIndex].GetComponent<TutorialPlayer>().WalkAnimation(true);
                //This makes the player move from one waypoint to the next
                if (players[playerTurnIndex].transform.position == waypoints[currentSpace].position)
                {
                    players[playerTurnIndex].GetComponent<TutorialPlayer>().WalkAnimation(false);
                    //Once the player has reach the waypoint
                    if (players[playerTurnIndex].GetComponent<TutorialPlayer>().WaypointIndex == currentSpace)
                    {
                        //Check if the waypoint is owned by a player
                        if (waypoints[currentSpace].GetComponent<Waypoint>().OwnByPlayer && playerTurnIndex != waypoints[currentSpace].GetComponent<Waypoint>().PlayerIndex)
                        {
                            pointSystem.MinusPoints(playerTurnIndex, 10);
                            pointSystem.AddPoints(waypoints[currentSpace].GetComponent<Waypoint>().PlayerIndex, 10);
                        }
                        else
                        {
                            waypoints[currentSpace].GetComponent<Waypoint>().SetPlayer(playerTurnIndex);
                        }
                        playerTurnIndex++;
                        turnFinished = true;
                    }

                    currentSpace++;
                    //Once the player makes one full rotation around the map
                    if (currentSpace == waypoints.Length)
                    {
                        currentSpace = 0;
                    }

                    //Switch players
                    if (playerTurnIndex == players.Count)
                    {
                        currentRound++;
                        playerTurnIndex = 0;
                        roundFinished = true;
                    }

                    //Check if the game is finish
                    if (currentRound == maxTurns)
                    {
                        //TODO:
                        //Game is finish
                    }
                }
            }
        }
    }

    public void MovePlayer(int index)
    {
        cardIndex = index + 1;
        currentSpace = players[playerTurnIndex].GetComponent<TutorialPlayer>().WaypointIndex;
        players[playerTurnIndex].GetComponent<TutorialPlayer>().WaypointIndex += cardIndex;
        if (players[playerTurnIndex].GetComponent<TutorialPlayer>().WaypointIndex > waypoints.Length - 1)
        {
            players[playerTurnIndex].GetComponent<TutorialPlayer>().WaypointIndex = players[playerTurnIndex].GetComponent<TutorialPlayer>().WaypointIndex - waypoints.Length;
            pointSystem.AddPoints(playerTurnIndex, 20);
        }
        movePlayer = true;
    }

    public void MiniGamePicked(int index)
    {

    }

    public bool TurnFinished
    {
        get
        {
            return turnFinished;
        }
        set
        {
            turnFinished = value;
        }
    }
    public bool RoundFinished
    {
        get
        {
            return roundFinished;
        }
        set
        {
            roundFinished = value;
        }
    }
    public bool IsMiniGameRunning
    {
        get {
            return isMiniGameRunning;
        }
        set {
            isMiniGameRunning = value;
        }
    }

    public int PlayerTurnIndex
    {
        get
        {
            return playerTurnIndex;
        }
        set
        {
            playerTurnIndex = value;
        }
    }
    public float PlayerMoveSpeed
    {
        get
        {
            return playerMoveSpeed;
        }
        set
        {
            playerMoveSpeed = value;
        }
    }
}
