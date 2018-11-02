using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPointSystem : MonoBehaviour {

    [SerializeField]
    private GameObject hudPrefab;
    [SerializeField]
    private Transform hudPanel;
    private GameObject[] playerHuds;

    private int[] points;

    //Start since the players use the awake function first
    void Start()
    {
        points = new int[TutorialTurnSystem.players.Count];
        playerHuds = new GameObject[TutorialTurnSystem.players.Count];
        for (int i = 0; i < playerHuds.Length; i++) {
            points[i] = 0;
            playerHuds[i] = Instantiate(hudPrefab, hudPanel);
            playerHuds[i].GetComponent<PlayerHUD>().ChangeColor(i);
        }
    }

    void Update()
    {
        for (int i = 0; i < playerHuds.Length; i++)
        {
            Text[] pointsText = playerHuds[i].GetComponentsInChildren<Text>();
            pointsText[0].text = "PLAYER " + (i + 1);
            pointsText[1].text = "POINTS: " + points[i];
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
