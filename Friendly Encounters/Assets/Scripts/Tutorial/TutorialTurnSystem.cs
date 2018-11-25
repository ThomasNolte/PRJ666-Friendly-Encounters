using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTurnSystem : MonoBehaviour
{
    public static List<TutorialPlayer> players = new List<TutorialPlayer>();

    public const int ONECYCLEPOINTS = 20;

    public enum MapNames
    {
        TUTORIALMAP,
        MAP1,
        MAP2,
        MAP3
    }

    public GameObject gameCanvas;
    public GameObject mapCanvas;
    public GameObject miniGamePanel;
    public GameObject[] maps;
    public GameObject playerPrefab;

    public GameObject turnText;
    public GameObject roundText;

    public Button lookAtBoardButton;
    public GameObject roundEndText;

    public GameObject playerSelectionCanvas;
    public GameObject playerSelectionPanel;
    public Button cancelPlayerSelection;
    public Button doneInteractionButton;
    public GameObject cardSelection;
    public GameObject warningPrefab;

    public TutorialCardPanel cardPanel;

    private TutorialPointSystem pointSystem;
    private TutorialMiniGameManager miniManager;

    private int amountOfPlayers = 2;
    //playerTurnIndex: The current players turn
    private int playerTurnIndex = 0;
    private int currentRound = 1;
    private int maxTurns = 15;
    private int cardIndex = 0;
    private int nextSpace = 0;
    private int currentMapIndex = -1;
    private int interactionIndex = -1;
    private int originalCardIndex = -1;

    private bool turnFinished;
    private bool isMiniGameRunning;
    private bool isLookingAtBoard;

    private Transform[] waypoints;
    public float playerMoveSpeed = 5f;
    private bool movePlayer;
    private bool isInteracting;
    private bool upgradeTile;
    private bool moveInteracting;

    //Player selection variables
    private GameObject[] selectionHUDS;
    private bool selectSelf = false;
    private bool playerIsSelected = false;
    private bool playerSelectionEnabled = false;
    private int selectedPlayerIndex = -1;

    void Awake()
    {
        pointSystem = GetComponent<TutorialPointSystem>();
        miniManager = GetComponent<TutorialMiniGameManager>();
        cancelPlayerSelection.onClick.AddListener(CancelInteraction);
        doneInteractionButton.onClick.AddListener(FinishInteractionTurn);
        lookAtBoardButton.onClick.AddListener(PanCamera);
    }

    void Start()
    {
        for (int i = 0; i < amountOfPlayers; i++)
        {
            Instantiate(playerPrefab);
        }

        //SelectionHUDS are initialize here because
        //Player count is initialized on Awake
        selectionHUDS = new GameObject[players.Count];
        for (int i = 0; i < selectionHUDS.Length; i++)
        {
            GameObject hud = Instantiate(pointSystem.hudPrefab, playerSelectionPanel.transform);
            hud.GetComponent<PlayerHUD>().GetComponentsInChildren<Text>()[1].text = string.Empty;
            hud.GetComponent<PlayerHUD>().ChangeColor(i);
            hud.GetComponentsInChildren<Text>()[0].text = "PLAYER " + (i + 1);
            selectionHUDS[i] = hud;
        }

        if ((int)MyGameManager.STATES.MENUSTATE == MyGameManager.lastSceneIndex)
        {
            currentMapIndex = 0;
        }
        //If a map was selected before game start, use that map
        if (currentMapIndex != -1)
        {
            ChooseMap(currentMapIndex);
        }
    }

    void Update()
    {
        //Check if player is allowed to move
        if (players.Count > 0 && !MyGameManager.pause)
        {
            if (players[playerTurnIndex].Skip)
            {
                players[playerTurnIndex].Skip = false;
                playerTurnIndex++;
                if (playerTurnIndex == players.Count)
                {
                    playerTurnIndex = 0;
                }
            }

            if (upgradeTile)
            {
                bool check = false;
                foreach (Transform waypoint in waypoints)
                {
                    if (waypoint.GetComponent<Waypoint>().OwnByPlayer &&
                        waypoint.GetComponent<Waypoint>().PlayerIndex == playerTurnIndex)
                    {
                        check = true;
                    }
                }

                if (!check)
                {
                    //Spawn a warning message
                    GameObject message = Instantiate(warningPrefab, gameCanvas.transform);
                    message.GetComponent<WarningMessage>().SetWarningText("You have no tiles to upgrade!");
                    upgradeTile = false;
                    isLookingAtBoard = false;
                    cardPanel.DeselectCard();
                }
                else
                {
                    isLookingAtBoard = true;
                    cardPanel.gameObject.SetActive(false);
                    lookAtBoardButton.gameObject.SetActive(false);
                    cardPanel.RemoveCard();
                }
            }
            if (playerSelectionEnabled && !playerIsSelected)
            {
                foreach (GameObject hud in selectionHUDS)
                {
                    if (hud.GetComponent<PlayerHUD>().Selected && playerSelectionEnabled)
                    {
                        playerIsSelected = true;
                        hud.GetComponent<PlayerHUD>().Selected = false;
                        selectedPlayerIndex = hud.GetComponent<PlayerHUD>().ImgIndex;

                        //If the player cannot pick themselves
                        if (!selectSelf)
                        {
                            if (playerTurnIndex == selectedPlayerIndex)
                            {
                                //Spawn a warning message
                                GameObject message = Instantiate(warningPrefab, playerSelectionCanvas.gameObject.transform);
                                message.GetComponent<WarningMessage>().SetWarningText("You cannot select yourself for this card!");
                                playerIsSelected = false;
                            }
                        }

                        if (playerIsSelected)
                        {
                            //Checking card selection cards
                            if (interactionIndex == (int)NetworkCard.CardIndex.DISCARDCARD ||
                               interactionIndex == (int)NetworkCard.CardIndex.STEALCARD ||
                               interactionIndex == (int)NetworkCard.CardIndex.SWITCHCARD ||
                               interactionIndex == (int)NetworkCard.CardIndex.SKIPTURN)
                            {
                                //Checking if the player already has a skipped turn
                                if (interactionIndex == (int)NetworkCard.CardIndex.SKIPTURN &&
                                    players[selectedPlayerIndex].Skip)
                                {
                                    GameObject message = Instantiate(warningPrefab, playerSelectionCanvas.gameObject.transform);
                                    message.GetComponent<WarningMessage>().SetWarningText("You cannot skip a players turn twice!");
                                    playerIsSelected = false;
                                }
                                //Checking if the player has any cards to begin with
                                else if (cardPanel.GetNumberCards(selectedPlayerIndex) == 0)
                                {
                                    //Spawn a warning message
                                    GameObject message = Instantiate(warningPrefab, playerSelectionCanvas.gameObject.transform);
                                    message.GetComponent<WarningMessage>().SetWarningText("Player selected has no cards to pick from!");
                                    playerIsSelected = false;
                                }
                                //Checking if the player has any card to swap (excluding the switch card itself)
                                else if (interactionIndex == (int)NetworkCard.CardIndex.SWITCHCARD &&
                                    cardPanel.GetNumberCards(playerTurnIndex) == 1)
                                {
                                    GameObject message = Instantiate(warningPrefab, playerSelectionCanvas.gameObject.transform);
                                    message.GetComponent<WarningMessage>().SetWarningText("You have no cards to switch!");
                                    playerIsSelected = false;
                                }
                            }
                            //Double check because of the condition above
                            if (playerIsSelected)
                            {
                                playerIsSelected = false;
                                TurnOnPlayerSelection(false);
                                DoAction();
                            }
                        }
                    }
                }
            }

            if (moveInteracting)
            {
                MovePlayerAction(selectedPlayerIndex);
            }
            else if (movePlayer && players[playerTurnIndex].GetComponent<TutorialPlayer>().WaypointIndex < waypoints.Length && !turnFinished && !IsMiniGameRunning)
            {
                //Smooth player moving transition
                players[playerTurnIndex].transform.position = Vector2.MoveTowards(players[playerTurnIndex].transform.position, waypoints[nextSpace].position, playerMoveSpeed * Time.deltaTime);
                players[playerTurnIndex].GetComponent<TutorialPlayer>().WalkAnimation(true);
                //This makes the player move from one waypoint to the next
                if (players[playerTurnIndex].transform.position == waypoints[nextSpace].position)
                {
                    players[playerTurnIndex].GetComponent<TutorialPlayer>().WalkAnimation(false);
                    //Once the player has reach the waypoint
                    if (players[playerTurnIndex].GetComponent<TutorialPlayer>().WaypointIndex == nextSpace)
                    {
                        //Check if the waypoint is owned by a player
                        if (waypoints[nextSpace].GetComponent<Waypoint>().OwnByPlayer && playerTurnIndex != waypoints[nextSpace].GetComponent<Waypoint>().PlayerIndex)
                        {
                            pointSystem.MinusPoints(playerTurnIndex, waypoints[nextSpace].GetComponent<Waypoint>().Points);
                            pointSystem.AddPoints(waypoints[nextSpace].GetComponent<Waypoint>().PlayerIndex, waypoints[nextSpace].GetComponent<Waypoint>().Points);
                        }
                        else
                        {
                            waypoints[nextSpace].GetComponent<Waypoint>().SetPlayer(playerTurnIndex);
                        }
                        playerTurnIndex++;
                        turnFinished = true;
                    }

                    nextSpace++;
                    //Once the player makes one full rotation around the map
                    if (nextSpace == waypoints.Length)
                    {
                        nextSpace = 0;
                    }

                    //Switch players
                    if (playerTurnIndex == players.Count)
                    {
                        currentRound++;
                        playerTurnIndex = 0;
                        StartCoroutine(RoundFinished());
                    }

                    //Check if the game is finish
                    if (currentRound == (maxTurns + 1))
                    {
                        //TODO:
                        //Game is finish
                    }
                }
            }
        }

    }

    //Graphical updates
    void LateUpdate()
    {
        if (cardPanel != null && cardPanel.isActiveAndEnabled)
        {
            if (!cardPanel.FinishInteraction &&
                cardPanel.DrawnCard &&
                !playerSelectionEnabled &&
                !doneInteractionButton.isActiveAndEnabled)
            {
                doneInteractionButton.gameObject.SetActive(true);
            }
        }

        turnText.GetComponentInChildren<Text>().text = "PLAYER'S " + (playerTurnIndex + 1) + " TURN";
        roundText.GetComponentInChildren<Text>().text = "Round: " + currentRound + "/" + maxTurns;

        if (Camera.main.GetComponent<TutorialCamera>().ReachDestination)
        {
            cardPanel.deck.SetActive(true);
            Camera.main.GetComponent<TutorialCamera>().ReachDestination = false;
        }

        if (movePlayer || moveInteracting)
        {
            lookAtBoardButton.gameObject.SetActive(false);
        }
        else if (!upgradeTile && !isLookingAtBoard)
        {
            SetGameHUD(true);
            lookAtBoardButton.gameObject.SetActive(true);
        }
        else if (isLookingAtBoard)
        {
            SetGameHUD(false);
        }
    }

    public void ShowGame(bool value)
    {
        gameCanvas.SetActive(value);
        maps[currentMapIndex].SetActive(value);
    }


    public void MovePlayer(int index)
    {
        cardIndex = index + 1;
        nextSpace = players[playerTurnIndex].GetComponent<TutorialPlayer>().WaypointIndex;
        players[playerTurnIndex].GetComponent<TutorialPlayer>().WaypointIndex += cardIndex;
        if (players[playerTurnIndex].GetComponent<TutorialPlayer>().WaypointIndex > waypoints.Length - 1)
        {
            players[playerTurnIndex].GetComponent<TutorialPlayer>().WaypointIndex = players[playerTurnIndex].GetComponent<TutorialPlayer>().WaypointIndex - waypoints.Length;
            pointSystem.AddPoints(playerTurnIndex, ONECYCLEPOINTS);
        }
        movePlayer = true;
    }

    public void MovePlayerAction(int playerIndex)
    {
        //Focus camera on the moving player
        FindObjectOfType<TutorialCamera>().setTarget(players[playerIndex].gameObject.transform);
        players[playerIndex].transform.position = Vector2.MoveTowards(players[playerIndex].transform.position, waypoints[nextSpace].position, playerMoveSpeed * Time.deltaTime);
        players[playerIndex].GetComponent<TutorialPlayer>().WalkAnimation(true);
        if (players[playerIndex].transform.position == waypoints[nextSpace].position)
        {
            players[playerIndex].GetComponent<TutorialPlayer>().WalkAnimation(false);
            //Once the player has reach the waypoint
            if (players[playerIndex].GetComponent<TutorialPlayer>().WaypointIndex == nextSpace)
            {
                //Check if the waypoint is owned by a player
                if (waypoints[nextSpace].GetComponent<Waypoint>().OwnByPlayer && playerIndex != waypoints[nextSpace].GetComponent<Waypoint>().PlayerIndex)
                {
                    pointSystem.MinusPoints(playerIndex, waypoints[nextSpace].GetComponent<Waypoint>().Points);
                    pointSystem.AddPoints(waypoints[nextSpace].GetComponent<Waypoint>().PlayerIndex, waypoints[nextSpace].GetComponent<Waypoint>().Points);
                }
                else
                {
                    waypoints[nextSpace].GetComponent<Waypoint>().SetPlayer(playerIndex);
                }
                interactionIndex = -1;
                moveInteracting = false;
                FindObjectOfType<TutorialCamera>().setTarget(players[playerTurnIndex].gameObject.transform);
            }

            nextSpace++;
            //Once the player makes one full rotation around the map
            if (nextSpace == waypoints.Length)
            {
                nextSpace = 0;
            }
        }
    }

    public void InteractPlayer(int index, int selectedCardIndex)
    {
        isInteracting = true;
        interactionIndex = index;
        originalCardIndex = selectedCardIndex;
        switch (index)
        {
            case (int)NetworkCard.CardIndex.DISCARDCARD:
                playerSelectionEnabled = true;
                DisplayPlayerSelection(false);
                break;
            case (int)NetworkCard.CardIndex.MOVEFORWARD:
                playerSelectionEnabled = true;
                DisplayPlayerSelection(true);
                break;
            case (int)NetworkCard.CardIndex.DRAWCARD:
                DoAction();
                break;
            case (int)NetworkCard.CardIndex.SWITCHPOSITION:
                playerSelectionEnabled = true;
                DisplayPlayerSelection(false);
                break;
            case (int)NetworkCard.CardIndex.SWITCHCARD:
                playerSelectionEnabled = true;
                DisplayPlayerSelection(false);
                break;
            case (int)NetworkCard.CardIndex.DUELCARD:
                playerSelectionEnabled = true;
                DisplayPlayerSelection(false);
                break;
            case (int)NetworkCard.CardIndex.STEALCARD:
                playerSelectionEnabled = true;
                DisplayPlayerSelection(false);
                break;
            case (int)NetworkCard.CardIndex.SKIPTURN:
                playerSelectionEnabled = true;
                DisplayPlayerSelection(false);
                break;
            case (int)NetworkCard.CardIndex.UPGRADETILE:
                DoAction();
                break;
        }
    }

    public void DoAction()
    {
        GameObject obj = null;
        switch (interactionIndex)
        {
            case (int)NetworkCard.CardIndex.DISCARDCARD:
                obj = Instantiate(cardSelection, null);
                obj.GetComponent<TutorialCardSelection>().StartCardSelection(interactionIndex, selectedPlayerIndex, originalCardIndex);
                cardPanel.RemoveCard();
                break;
            case (int)NetworkCard.CardIndex.MOVEFORWARD:
                nextSpace = players[selectedPlayerIndex].GetComponent<TutorialPlayer>().WaypointIndex;
                players[selectedPlayerIndex].GetComponent<TutorialPlayer>().WaypointIndex += 2;
                if (players[selectedPlayerIndex].GetComponent<TutorialPlayer>().WaypointIndex > waypoints.Length - 1)
                {
                    players[selectedPlayerIndex].GetComponent<TutorialPlayer>().WaypointIndex = players[selectedPlayerIndex].GetComponent<TutorialPlayer>().WaypointIndex - waypoints.Length;
                    pointSystem.AddPoints(selectedPlayerIndex, ONECYCLEPOINTS);
                }
                moveInteracting = true;
                cardPanel.RemoveCard();
                break;
            case (int)NetworkCard.CardIndex.DRAWCARD:
                cardPanel.RemoveCard();
                cardPanel.ActionDrawCard();
                break;
            case (int)NetworkCard.CardIndex.SWITCHPOSITION:
                Vector3 prevPos = players[playerTurnIndex].transform.position;
                int prevWaypoint = players[playerTurnIndex].WaypointIndex;
                players[playerTurnIndex].transform.position = players[selectedPlayerIndex].transform.position;
                players[playerTurnIndex].WaypointIndex = players[selectedPlayerIndex].WaypointIndex;
                players[selectedPlayerIndex].transform.position = prevPos;
                players[selectedPlayerIndex].WaypointIndex = prevWaypoint;
                cardPanel.RemoveCard();
                break;
            case (int)NetworkCard.CardIndex.SWITCHCARD:
                obj = Instantiate(cardSelection, null);
                obj.GetComponent<TutorialCardSelection>().SwitchCards(interactionIndex, selectedPlayerIndex, originalCardIndex, playerTurnIndex);
                cardPanel.RemoveCard();
                break;
            case (int)NetworkCard.CardIndex.DUELCARD:
                cardPanel.RemoveCard();
                break;
            case (int)NetworkCard.CardIndex.STEALCARD:
                obj = Instantiate(cardSelection, null);
                obj.GetComponent<TutorialCardSelection>().StartCardSelection(interactionIndex, selectedPlayerIndex, originalCardIndex);
                cardPanel.RemoveCard();
                break;
            case (int)NetworkCard.CardIndex.SKIPTURN:
                players[selectedPlayerIndex].Skip = true;
                cardPanel.RemoveCard();
                break;
            case (int)NetworkCard.CardIndex.UPGRADETILE:
                upgradeTile = true;
                break;
        }
        cardPanel.Interacting = false;
    }

    public void TutorialMap() { ChooseMap((int)MapNames.TUTORIALMAP); }
    public void Map1() { ChooseMap((int)MapNames.MAP1); }
    public void Map2() { ChooseMap((int)MapNames.MAP2); }
    public void Map3() { ChooseMap((int)MapNames.MAP3); }

    public void ChooseMap(int index)
    {
        currentMapIndex = index;
        maps[index].SetActive(true);
        string mapName = string.Empty;
        switch (index)
        {
            case (int)MapNames.TUTORIALMAP:
                mapName = MapNames.TUTORIALMAP.ToString().Substring(0, 1) + MapNames.TUTORIALMAP.ToString().Substring(1).ToLower();
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
        mapCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        SetPlayersPositions();
    }

    private void PanCamera()
    {
        if (IsLookingAtBoard)
        {
            lookAtBoardButton.GetComponentInChildren<Text>().text = "LOOK AT BOARD";
            isLookingAtBoard = false;
        }
        else
        {
            lookAtBoardButton.GetComponentInChildren<Text>().text = "BACK";
            isLookingAtBoard = true;
        }
    }

    public void SetPlayersPositions()
    {
        foreach (TutorialPlayer player in players)
        {
            player.transform.position = waypoints[0].transform.position;
        }
    }

    public void DisplayPlayerSelection(bool canSelectSelf)
    {
        selectSelf = canSelectSelf;
        TurnOnPlayerSelection(true);
    }

    private void FinishInteractionTurn()
    {
        cardPanel.FinishInteraction = true;
        doneInteractionButton.gameObject.SetActive(false);
    }

    public void CancelInteraction()
    {
        TurnOnPlayerSelection(false);
        //Reset the interaction flags
        playerSelectionEnabled = false;
        cardPanel.Interacting = false;
        cardPanel.ResetCard(interactionIndex);
        isInteracting = false;
        interactionIndex = -1;
    }

    public void TurnOnPlayerSelection(bool value)
    {
        playerSelectionEnabled = value;
        playerSelectionCanvas.SetActive(value);
    }

    public void SetGameHUD(bool value)
    {
        cardPanel.gameObject.SetActive(value);
        turnText.gameObject.SetActive(value);
    }

    IEnumerator RoundFinished()
    {
        //Once the last player's turn is over
        //Wait for a moment before going to the minigames
        IsMiniGameRunning = true;
        roundEndText.SetActive(true);
        cardPanel.deck.SetActive(false);
        yield return new WaitForSeconds(1.8f);
        roundEndText.SetActive(false);
        ShowGame(false);
        miniGamePanel.SetActive(true);
        miniManager.RollGame();
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
    public bool IsLookingAtBoard
    {
        get
        {
            return isLookingAtBoard;
        }
        set
        {
            isLookingAtBoard = value;
        }
    }

    public bool PlayerMoving
    {
        get
        {
            return movePlayer;
        }
        set
        {
            movePlayer = value;
        }

    }

    public bool InteractingWithPlayer
    {
        get
        {
            return isInteracting;
        }
        set
        {
            isInteracting = value;
        }
    }

    public int InteractionIndex
    {
        get
        {
            return interactionIndex;
        }
        set
        {
            interactionIndex = value;
        }
    }

    public bool PlayerSelectionEnabled
    {
        get
        {
            return playerSelectionEnabled;
        }
        set
        {
            playerSelectionEnabled = value;
        }
    }

    public int OriginalCardIndex
    {
        get
        {
            return originalCardIndex;
        }

        set
        {
            originalCardIndex = value;
        }
    }

    public int CurrentMapIndex
    {
        get
        {
            return currentMapIndex;
        }

        set
        {
            currentMapIndex = value;
        }
    }

    public bool UpgradeTile
    {
        get
        {
            return upgradeTile;
        }

        set
        {
            upgradeTile = value;
        }
    }

    public void Clear()
    {
        players.Clear();
    }
}
