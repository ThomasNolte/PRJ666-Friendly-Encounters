using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayManager : MonoBehaviour
{
    static public List<BasePlayer> players = new List<BasePlayer>();
    static public PlayManager instance = null;

    //[SyncVar]
    private int turnIndex = 0;
    private int cardIndex = 0;
    private int currentSpace = 0;
    private bool turnFinished = false;

    public GameObject[] waypoints;
    public float playerMoveSpeed = 5f;
    public bool movePlayer;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (BasePlayer player in players)
        {
            player.gameObject.transform.position = waypoints[0].transform.position;
        }
    }

    void Start()
    {
        //StartCoroutine(SpendTurn());
    }

    void Update()
    {
        if (movePlayer && players[0].GetComponent<BasePlayer>().WaypointIndex < waypoints.Length)
        {
            Debug.Log("Current space: " + currentSpace);
            players[0].transform.position = Vector2.MoveTowards(players[0].transform.position, waypoints[currentSpace].transform.position, playerMoveSpeed * Time.deltaTime);
            if (players[0].transform.position == waypoints[currentSpace].transform.position)
            {
                if (players[0].GetComponent<BasePlayer>().WaypointIndex == currentSpace)
                {
                    movePlayer = false;
                }
                currentSpace++;
            }

        }
    }

    IEnumerator SpendTurn()
    {
        yield return new WaitForSeconds(0.5f);
    }

    public void MovePlayer(int index)
    {
        Debug.Log(index);
        cardIndex = index + 1;
        currentSpace = players[0].GetComponent<BasePlayer>().WaypointIndex;
        players[0].GetComponent<BasePlayer>().WaypointIndex += cardIndex;
        if (players[0].GetComponent<BasePlayer>().WaypointIndex > waypoints.Length)
        {
            players[0].GetComponent<BasePlayer>().WaypointIndex = waypoints.Length - 1;
        }
        //else
        //{
            movePlayer = true;
        //}
    }

}
