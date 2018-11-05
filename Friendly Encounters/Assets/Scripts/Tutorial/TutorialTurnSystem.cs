using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTurnSystem : MonoBehaviour
{
    public static List<TutorialPlayer> players = new List<TutorialPlayer>();

    public enum MapNames
    {
        TUTORIAL,
        MAP1,
        MAP2,
        MAP3
    }
    
    public GameObject miniGamePanel;
    public GameObject turnText; 

    private TutorialPointSystem pointSystem;
    private TutorialMiniGameManager miniManager;

    private int mapIndex = 2;
    private int playerTurnIndex = 0;
    private int currentRound = 0;
    private int maxTurns = 15;
    private int cardIndex = 0;
    private int currentSpace = 0;

    private bool turnFinished;
    private bool roundFinished;
    private bool isMiniGameRunning;

    private Transform[] waypoints;
    public float playerMoveSpeed = 5f;
    [HideInInspector]
    public bool movePlayer;

    void Awake()
    {
        pointSystem = GetComponent<TutorialPointSystem>();
        miniManager = GetComponent<TutorialMiniGameManager>();
        string mapName = string.Empty;
        switch (mapIndex)
        {
            case (int)MapNames.TUTORIAL:
                mapName = MapNames.TUTORIAL.ToString().Substring(0, 1) + MapNames.TUTORIAL.ToString().Substring(1).ToLower();
                break;
            case (int)MapNames.MAP1:
                mapName = MapNames.MAP1.ToString().Substring(0, 1) + MapNames.MAP1.ToString().Substring(1).ToLower();
                break;
            case (int)MapNames.MAP2:
                mapName = MapNames.MAP2.ToString().Substring(0, 1) + MapNames.MAP2.ToString().Substring(1).ToLower();
                break;
            case (int)MapNames.MAP3:
                mapName = MapNames.MAP3.ToString().Substring(0, 1) + MapNames.MAP3.ToString().Substring(1).ToLower();
                break;
        }
        waypoints = new Transform[GameObject.Find(mapName + " Waypoints").GetComponentsInChildren<Transform>().Length - 1];
        Array.Copy(GameObject.Find(mapName + " Waypoints").GetComponentsInChildren<Transform>(), 1, waypoints, 0, GameObject.Find(mapName + " Waypoints").GetComponentsInChildren<Transform>().Length - 1);
    }

    void Start()
    {
        foreach (TutorialPlayer player in players)
        {
            player.transform.position = waypoints[0].transform.position;
        }
    }

    void Update()
    {
        if (roundFinished)
        {
            IsMiniGameRunning = true;
            miniGamePanel.SetActive(true);
            miniManager.RollGame();
            roundFinished = false;
        }
        else
        {
            //Check if player is allowed to move
            if (players.Count > 0)
            {
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
    }

    void LateUpdate()
    {
        turnText.GetComponentInChildren<Text>().text = "PLAYER'S " + (playerTurnIndex + 1) + " TURN";
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
            roundFinished = true;
            Clean();
        }
        movePlayer = true;
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
        get
        {
            return isMiniGameRunning;
        }
        set
        {
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

    public void Clean()
    {
        players.Clear();
    }
}
