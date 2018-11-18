using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCardPanel : MonoBehaviour
{
    public const int MAXCARDS = 4;

    public NetworkCard card;

    public GameObject interactionCardPanel;
    public GameObject movementCardPanel;
    public GameObject deck;

    public Text typeCardText;

    private int cardSelectedIndex = -1;
    private bool finishInteraction = false;
    private bool interacting = false;
    private bool drawnCard = false;

    private GameObject[] interactionPanels;
    private GameObject[] movementPanels;
    private List<GameObject[]> movementHand = new List<GameObject[]>();
    private List<GameObject[]> interactionHand = new List<GameObject[]>();
    private GameObject[] cardList;
    private TutorialTurnSystem playManager;

    void Awake()
    {
        playManager = FindObjectOfType<TutorialTurnSystem>();
        deck.GetComponentInChildren<Button>().onClick.AddListener(DrawCard);
    }

    void Start()
    {
        movementPanels = new GameObject[TutorialTurnSystem.players.Count];
        interactionPanels = new GameObject[TutorialTurnSystem.players.Count];
        for (int i = 0; i < TutorialTurnSystem.players.Count; i++)
        {
            movementPanels[i] = Instantiate(movementCardPanel, transform);
            cardList = new GameObject[MAXCARDS];
            for (int j = 0; j < MAXCARDS - 1; j++)
            {
                cardList[j] = Instantiate(card.gameObject, movementPanels[i].transform);
                cardList[j].GetComponent<NetworkCard>().SetRandomMovementCard();
            }
            cardList[MAXCARDS - 1] = Instantiate(card.gameObject, movementPanels[i].transform);
            cardList[MAXCARDS - 1].GetComponent<NetworkCard>().SetOriginalImage();
            movementHand.Add(cardList);

            interactionPanels[i] = Instantiate(interactionCardPanel, transform);
            cardList = new GameObject[MAXCARDS];
            for (int j = 0; j < MAXCARDS - 1; j++)
            {
                cardList[j] = Instantiate(card.gameObject, interactionPanels[i].transform);
                cardList[j].GetComponent<NetworkCard>().SetRandomInteractionCard();
            }
            cardList[MAXCARDS - 1] = Instantiate(card.gameObject, interactionPanels[i].transform);
            cardList[MAXCARDS - 1].GetComponent<NetworkCard>().SetOriginalImage();
            interactionHand.Add(cardList);
        }
    }

    void Update()
    {
        if (!MyGameManager.pause)
        {
            if (!playManager.IsMiniGameRunning)
            {
                for (int i = 0; i < TutorialTurnSystem.players.Count; i++)
                {
                    if (playManager.PlayerTurnIndex == i)
                    {
                        if (!drawnCard)
                        {
                            interactionPanels[i].SetActive(true);
                            typeCardText.text = "Interaction Cards";
                            deck.SetActive(true);
                        }
                        else
                        {
                            if (!finishInteraction)
                            {
                                interactionPanels[i].SetActive(true);
                                movementPanels[i].SetActive(false);
                                typeCardText.text = "Interaction Cards";
                                for (int j = MAXCARDS - 1; j >= 0 && !interacting; j--)
                                {
                                    if (interactionHand[playManager.PlayerTurnIndex][j].GetComponent<NetworkCard>().Selected)
                                    {
                                        if (playManager.PlayerMoving ||
                                            playManager.IsMiniGameRunning ||
                                            interactionHand[playManager.PlayerTurnIndex][j].GetComponent<NetworkCard>().Empty)
                                        {
                                            interactionHand[playManager.PlayerTurnIndex][j].GetComponent<NetworkCard>().Selected = false;
                                        }
                                        else
                                        {
                                            cardSelectedIndex = interactionHand[playManager.PlayerTurnIndex][j].GetComponent<NetworkCard>().Index;
                                            interactionHand[playManager.PlayerTurnIndex][j].GetComponent<NetworkCard>().SetOriginalImage();
                                            playManager.InteractPlayer(cardSelectedIndex);
                                            interacting = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                interactionPanels[i].SetActive(false);
                                movementPanels[i].SetActive(true);
                                typeCardText.text = "Movement Cards";
                                for (int j = MAXCARDS - 1; j >= 0; j--)
                                {
                                    if (movementHand[playManager.PlayerTurnIndex][j].GetComponent<NetworkCard>().Selected)
                                    {
                                        if (playManager.PlayerMoving ||
                                            playManager.IsMiniGameRunning ||
                                            movementHand[playManager.PlayerTurnIndex][j].GetComponent<NetworkCard>().Empty)
                                        {
                                            movementHand[playManager.PlayerTurnIndex][j].GetComponent<NetworkCard>().Selected = false;
                                        }
                                        else
                                        {
                                            cardSelectedIndex = movementHand[playManager.PlayerTurnIndex][j].GetComponent<NetworkCard>().Index;
                                            movementHand[playManager.PlayerTurnIndex][j].GetComponent<NetworkCard>().SetOriginalImage();
                                            playManager.MovePlayer(cardSelectedIndex);
                                            finishInteraction = false;
                                            drawnCard = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //Disable the the display of the other players cards
                        interactionPanels[i].SetActive(false);
                        movementPanels[i].SetActive(false);
                    }
                }
            }
        }
    }
    

    public void DrawCard()
    {
        for (int i = 0; i < MAXCARDS; i++)
        {
            if (interactionHand[playManager.PlayerTurnIndex][i].GetComponent<NetworkCard>().Empty)
            {
                interactionHand[playManager.PlayerTurnIndex][i].GetComponent<NetworkCard>().SetRandomInteractionCard();
            }
        }
        for (int i = 0; i < MAXCARDS; i++)
        {
            if (movementHand[playManager.PlayerTurnIndex][i].GetComponent<NetworkCard>().Empty)
            {
                movementHand[playManager.PlayerTurnIndex][i].GetComponent<NetworkCard>().SetRandomMovementCard();
            }
        }
        drawnCard = true;
        deck.SetActive(false);
    }

    public void DoAction(int cardType, int cardIndex, int playerIndex)
    {
        switch (cardType)
        {
            case (int)NetworkCard.CardIndex.DISCARDCARD:
                interactionHand[playerIndex][cardIndex].GetComponent<NetworkCard>().SetOriginalImage();
                break;
            case (int)NetworkCard.CardIndex.SWITCHCARD:

                interactionHand[playerIndex][cardIndex].GetComponent<NetworkCard>().SetOriginalImage();
                break;
            case (int)NetworkCard.CardIndex.STEALCARD:

                interactionHand[playerIndex][cardIndex].GetComponent<NetworkCard>().SetOriginalImage();
                break;
        }
    }

    public bool FinishInteraction
    {
        get
        {
            return finishInteraction;
        }
        set
        {
            finishInteraction = value;
        }
    }


    public bool Interacting
    {
        get
        {
            return interacting;
        }
        set
        {
            interacting = value;
        }
    }

    public bool DrawnCard
    {
        get
        {
            return drawnCard;
        }
        set
        {
            drawnCard = value;
        }
    }
}
