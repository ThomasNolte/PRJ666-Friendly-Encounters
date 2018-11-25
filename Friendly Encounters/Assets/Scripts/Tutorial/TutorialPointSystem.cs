using UnityEngine;
using UnityEngine.UI;

public class TutorialPointSystem : MonoBehaviour
{
    public GameObject hudPrefab;
    public Transform hudPanel;

    private GameObject[] playerHuds;

    private int[] points;
    
    // Start instead of Awake because the players use the awake function first
    // to be added into the turn system's player list
    void Start()
    {
        points = new int[TutorialTurnSystem.players.Count];
        playerHuds = new GameObject[TutorialTurnSystem.players.Count];
        for (int i = 0; i < playerHuds.Length; i++)
        {
            points[i] = 0;
            playerHuds[i] = Instantiate(hudPrefab, hudPanel);
            playerHuds[i].GetComponent<PlayerHUD>().ChangeColor(i);
            playerHuds[i].GetComponentsInChildren<Text>()[0].text = "PLAYER " + (i + 1);
        }
    }

    void Update()
    {
        if (!MyGameManager.pause)
        {
            for (int i = 0; i < playerHuds.Length; i++)
            {
                Text[] pointsText = playerHuds[i].GetComponentsInChildren<Text>();
                pointsText[1].text = "POINTS: " + points[i];
                playerHuds[i].GetComponent<PlayerHUD>().Selected = false;
            }
        }
    }


    public void AddPoints(int playerIndex, int point)
    {
        SetPoints(playerIndex, (points[playerIndex] + point));
    }

    public void MinusPoints(int playerIndex, int point)
    {
        SetPoints(playerIndex, (points[playerIndex] - point));
    }

    public void SetPoints(int playerIndex, int point)
    {
        points[playerIndex] = point;
    }
}
