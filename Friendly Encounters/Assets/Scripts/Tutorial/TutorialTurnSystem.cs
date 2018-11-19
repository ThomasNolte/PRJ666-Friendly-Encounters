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

    public GameObject turnText;
    public GameObject roundText;

    public GameObject cardPanel;
    public Button zoomOutButton;
    public Button lookAtBoardButton;
    public GameObject roundEndText;
    
    public GameObject cardSelection;

    private TutorialPointSystem pointSystem;
    private TutorialMiniGameManager miniManager;

    private int playerTurnIndex = 0;
    private int currentRound = 1;
    private int maxTurns = 15;
    private int cardIndex = 0;
    private int nextSpace = 0;
    private int currentMapIndex = -1;
    private int interactionIndex = -1;

    private bool turnFinished;
    private bool isMiniGameRunning;
    private bool zoomedOut;
    private bool isLookingAtBoard;

    private Transform[] waypoints;
    public float playerMoveSpeed = 5f;
    private bool movePlayer;
    private bool isInteracting;
    private bool moveInteracting;
    private bool playerSelectionEnabled = false;

    void Awake()
    {
        pointSystem = GetComponent<TutorialPointSystem>();
        miniManager = GetComponent<TutorialMiniGameManager>();
        zoomOutButton.onClick.AddListener(ZoomOut);
        lookAtBoardButton.onClick.AddListener(PanCamera);
    }

    void Start()
    {
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
            if (moveInteracting)
            {
                MovePlayerAction(pointSystem.SelectedPlayerIndex);
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
                            pointSystem.MinusPoints(playerTurnIndex, 10);
                            pointSystem.AddPoints(waypoints[nextSpace].GetComponent<Waypoint>().PlayerIndex, 10);
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

    public void ShowGame(bool value)
    {
        gameCanvas.SetActive(value);
        maps[currentMapIndex].SetActive(value);
    }

    void LateUpdate()
    {
        turnText.GetComponentInChildren<Text>().text = "PLAYER'S " + (playerTurnIndex + 1) + " TURN";
        roundText.GetComponentInChildren<Text>().text = "Round: " + currentRound + "/" + maxTurns;
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
            Clear();
        }
        movePlayer = true;
    }

    public void MovePlayerAction(int playerIndex)
    {
        //Focus camera on the moving player
        FindObjectOfType<TutorialCamera>().setTarget(players[playerIndex].gameObject.transform);
        Debug.Log(playerIndex);
        Debug.Log("Starting position: " + nextSpace);
        Debug.Log(players[playerIndex].GetComponent<TutorialPlayer>().WaypointIndex);
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
                    pointSystem.MinusPoints(playerIndex, 10);
                    pointSystem.AddPoints(waypoints[nextSpace].GetComponent<Waypoint>().PlayerIndex, 10);
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
        cardSelection.GetComponent<TutorialCardSelection>().OriginalCardIndex = selectedCardIndex;
        switch (index)
        {
            case (int)NetworkCard.CardIndex.DISCARDCARD:
                playerSelectionEnabled = true;
                pointSystem.DisplayPlayerSelection(false);
                break;
            case (int)NetworkCard.CardIndex.MOVEFORWARD:
                playerSelectionEnabled = true;
                pointSystem.DisplayPlayerSelection(true);
                break;
            case (int)NetworkCard.CardIndex.DRAWCARD:
                playerSelectionEnabled = true;
                DoAction();
                break;
            case (int)NetworkCard.CardIndex.SWITCHPOSITION:
                playerSelectionEnabled = true;
                pointSystem.DisplayPlayerSelection(false);
                break;
            case (int)NetworkCard.CardIndex.SWITCHCARD:
                playerSelectionEnabled = true;
                pointSystem.DisplayPlayerSelection(false);
                break;
            case (int)NetworkCard.CardIndex.DUELCARD:
                playerSelectionEnabled = true;
                pointSystem.DisplayPlayerSelection(false);
                break;
            case (int)NetworkCard.CardIndex.STEALCARD:
                playerSelectionEnabled = true;
                pointSystem.DisplayPlayerSelection(false);
                break;
            case (int)NetworkCard.CardIndex.SKIPTURN:
                playerSelectionEnabled = true;
                pointSystem.DisplayPlayerSelection(true);
                break;
            case (int)NetworkCard.CardIndex.UPGRADETILE:
                DoAction();
                break;
        }
    }

    public void DoAction()
    {
        switch (interactionIndex)
        {
            case (int)NetworkCard.CardIndex.DISCARDCARD:
                cardSelection.GetComponent<TutorialCardSelection>().StartCardSelection(interactionIndex, pointSystem.SelectedPlayerIndex);
                break;
            case (int)NetworkCard.CardIndex.MOVEFORWARD:
                nextSpace = players[pointSystem.SelectedPlayerIndex].GetComponent<TutorialPlayer>().WaypointIndex;
                players[pointSystem.SelectedPlayerIndex].GetComponent<TutorialPlayer>().WaypointIndex += 2;
                moveInteracting = true;
                break;
            case (int)NetworkCard.CardIndex.DRAWCARD:

                break;
            case (int)NetworkCard.CardIndex.SWITCHPOSITION:

                break;
            case (int)NetworkCard.CardIndex.SWITCHCARD:
                cardSelection.GetComponent<TutorialCardSelection>().StartCardSelection(interactionIndex, pointSystem.SelectedPlayerIndex);
                break;
            case (int)NetworkCard.CardIndex.DUELCARD:

                break;
            case (int)NetworkCard.CardIndex.STEALCARD:
                cardSelection.GetComponent<TutorialCardSelection>().StartCardSelection(interactionIndex, pointSystem.SelectedPlayerIndex);
                break;
            case (int)NetworkCard.CardIndex.SKIPTURN:

                break;
            case (int)NetworkCard.CardIndex.UPGRADETILE:

                break;
        }
        cardPanel.GetComponent<TutorialCardPanel>().Interacting = false;
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


    public void ZoomOut()
    {
        if (zoomedOut)
        {
            Camera.main.orthographicSize = 2;
            zoomOutButton.GetComponentInChildren<Text>().text = "ZOOM OUT";
            zoomedOut = false;
            lookAtBoardButton.gameObject.SetActive(true);
            SetGameHUD(true);
        }
        else
        {
            Camera.main.orthographicSize = 5;
            zoomOutButton.GetComponentInChildren<Text>().text = "BACK";
            zoomedOut = true;
            lookAtBoardButton.gameObject.SetActive(false);
            SetGameHUD(false);
        }

    }

    public void PanCamera()
    {
        if (IsLookingAtBoard)
        {
            Camera.main.orthographicSize = 2;
            lookAtBoardButton.GetComponentInChildren<Text>().text = "LOOK AT BOARD";
            isLookingAtBoard = false;
            zoomOutButton.gameObject.SetActive(true);
            SetGameHUD(true);
        }
        else
        {
            Camera.main.orthographicSize = 5;
            lookAtBoardButton.GetComponentInChildren<Text>().text = "BACK";
            isLookingAtBoard = true;
            zoomOutButton.gameObject.SetActive(false);
            SetGameHUD(false);
        }
    }

    public void SetPlayersPositions()
    {
        foreach (TutorialPlayer player in players)
        {
            player.transform.position = waypoints[0].transform.position;
        }
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
    public bool ZoomedOut
    {
        get
        {
            return zoomedOut;
        }
        set
        {
            zoomedOut = value;
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

    public void Clear()
    {
        players.Clear();
    }
}
