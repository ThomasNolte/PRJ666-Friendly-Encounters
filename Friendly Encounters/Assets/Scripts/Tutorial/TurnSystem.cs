using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour {

    private static GameObject[] players;
    public static int amountOfPlayers = 2;
    public static int amountOfRounds = 0;
    public static int player1StartWaypoint = 0;
    public static int player2StartWaypoint = 0;
    public static int diceSide = 0;
    public static int whosTurn = 1;

	// Use this for initialization
	void Start () {
        players = GameObject.FindGameObjectsWithTag("Character");

        players[0].GetComponent<Player>().moveAllowed = false;
        players[1].GetComponent<Player>().moveAllowed = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (players[0].GetComponent<Player>().waypointIndex > player1StartWaypoint + diceSide) {
            players[0].GetComponent<Player>().moveAllowed = false;
            player1StartWaypoint = players[0].GetComponent<Player>().waypointIndex - 1;
        }
        if (players[1].GetComponent<Player>().waypointIndex > player2StartWaypoint + diceSide) {
            players[1].GetComponent<Player>().moveAllowed = false;
            player2StartWaypoint = players[1].GetComponent<Player>().waypointIndex - 1;
        }
    }

    public static void MovePlayer(int turn) {
        switch (turn) {
            case 1:
                players[0].GetComponent<Player>().moveAllowed = true;
                break;
            case 2:
                players[1].GetComponent<Player>().moveAllowed = true;
                break;
        }
    }
}
