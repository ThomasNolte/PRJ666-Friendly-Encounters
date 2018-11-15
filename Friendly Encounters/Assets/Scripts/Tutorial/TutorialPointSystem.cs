using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPointSystem : MonoBehaviour
{
    public GameObject hudPrefab;
    public Transform hudPanel;

    public GameObject playerSelectionCanvas;
    public GameObject playerSelectionPanel;

    private GameObject[] playerHuds;
    private List<GameObject> allHuds = new List<GameObject>();
    private TutorialTurnSystem playManager;
    private int[] points;
    private bool selectSelf = false;
    private bool playerIsSelected = false;
    private int selectedPlayerIndex = -1;

    // Start instead of Awake because the players use the awake function first
    // to be added into the turn system's player list
    void Start()
    {
        playManager = GetComponent<TutorialTurnSystem>();
        points = new int[TutorialTurnSystem.players.Count];
        playerHuds = new GameObject[TutorialTurnSystem.players.Count];
        for (int i = 0; i < playerHuds.Length; i++)
        {
            points[i] = 0;
            GameObject hud = Instantiate(hudPrefab, playerSelectionPanel.transform);
            hud.GetComponent<PlayerHUD>().GetComponentsInChildren<Text>()[1].text = string.Empty;
            hud.GetComponent<PlayerHUD>().ChangeColor(i);
            hud.GetComponentsInChildren<Text>()[0].text = "PLAYER " + (i + 1);
            allHuds.Add(hud);
            playerHuds[i] = Instantiate(hudPrefab, hudPanel);
            playerHuds[i].GetComponent<PlayerHUD>().ChangeColor(i);
            playerHuds[i].GetComponentsInChildren<Text>()[0].text = "PLAYER " + (i + 1); 
            allHuds.Add(playerHuds[i]);
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

            if (playManager.PlayerSelectionEnabled && !playerIsSelected)
            {
                foreach (GameObject hud in allHuds)
                {
                    if (hud.GetComponent<PlayerHUD>().Selected && playManager.PlayerSelectionEnabled)
                    {
                        playerIsSelected = true;
                        hud.GetComponent<PlayerHUD>().Selected = false;
                        selectedPlayerIndex = hud.GetComponent<PlayerHUD>().ImgIndex;
                        if (selectSelf)
                        {
                            if (playManager.PlayerTurnIndex == selectedPlayerIndex)
                            {
                                playerIsSelected = false;
                            }
                        }

                        if (playerIsSelected)
                        {
                            playerIsSelected = false;
                            playManager.PlayerSelectionEnabled = false;
                            playerSelectionCanvas.SetActive(false);
                            playManager.DoAction();
                        }
                    }
                }
            }
        }
    }

    public void DisplayPlayerSelection(bool canSelectSelf = false)
    {
        selectSelf = canSelectSelf;
        playerSelectionCanvas.SetActive(true);
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

    public bool PlayerIsSelected
    {
        get
        {
            return playerIsSelected;

        }
        set
        {
            playerIsSelected = value;
        }
    }

    public int SelectedPlayerIndex
    {
        get
        {
            return selectedPlayerIndex;
        }
        set
        {
            selectedPlayerIndex = value;
        }
    }
}
