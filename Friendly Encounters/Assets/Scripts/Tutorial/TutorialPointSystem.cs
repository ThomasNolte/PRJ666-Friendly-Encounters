using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPointSystem : MonoBehaviour
{
    public GameObject hudPrefab;
    public GameObject warningPrefab;
    public Transform hudPanel;

    public GameObject playerSelectionCanvas;
    public GameObject playerSelectionPanel;
    public GameObject blockPanel;

    public Button cancelPlayerSelection;
    public Button doneInteractionButton;

    private GameObject[] playerHuds;
    private List<GameObject> allHuds = new List<GameObject>();
    private TutorialTurnSystem playManager;
    private TutorialCardPanel cardPanel;

    private int[] points;

    private bool selectSelf = false;
    private bool playerIsSelected = false;
    private bool playerSelectionIsActive = false;
    private int selectedPlayerIndex = -1;

    // Start instead of Awake because the players use the awake function first
    // to be added into the turn system's player list
    void Start()
    {
        playManager = GetComponent<TutorialTurnSystem>();
        cardPanel = FindObjectOfType<TutorialCardPanel>();
        points = new int[TutorialTurnSystem.players.Count];
        playerHuds = new GameObject[TutorialTurnSystem.players.Count];
        cancelPlayerSelection.onClick.AddListener(CancelInteraction);
        doneInteractionButton.onClick.AddListener(FinishInteractionTurn);
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

                        if (!selectSelf)
                        {
                            if (playManager.PlayerTurnIndex == selectedPlayerIndex)
                            {
                                GameObject message = Instantiate(warningPrefab, playerSelectionCanvas.gameObject.transform);
                                message.GetComponent<WarningMessage>().SetWarningText("You cannot select yourself for this card!");
                                playerIsSelected = false;
                            }
                        }

                        if (playerIsSelected)
                        {
                            playerIsSelected = false;
                            playManager.PlayerSelectionEnabled = false;
                            TurnOnPlayerSelection(false);
                            playManager.DoAction();
                        }
                    }
                }
            }


            if (cardPanel.isActiveAndEnabled)
            {
                if (!cardPanel.FinishInteraction &&
                    cardPanel.DrawnCard &&
                    !playerSelectionIsActive &&
                    !doneInteractionButton.isActiveAndEnabled)
                {
                    doneInteractionButton.gameObject.SetActive(true);
                }
            }
        }
    }

    public void DisplayPlayerSelection(bool canSelectSelf)
    {
        selectSelf = canSelectSelf;
        TurnOnPlayerSelection(true);
        doneInteractionButton.gameObject.SetActive(false);
    }

    private void FinishInteractionTurn()
    {
        FindObjectOfType<TutorialCardPanel>().FinishInteraction = true;
        doneInteractionButton.gameObject.SetActive(false);
    }

    public void CancelInteraction()
    {
        TurnOnPlayerSelection(false);
        //Reset the interaction flags
        playManager.InteractingWithPlayer = false;
        FindObjectOfType<TutorialCardPanel>().Interacting = false;
        playManager.InteractionIndex = -1;
        
    }

    public void TurnOnPlayerSelection(bool value)
    {
        playerSelectionIsActive = value;
        playerSelectionCanvas.SetActive(value);
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
