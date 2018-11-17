using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCardPanel : MonoBehaviour
{
    public const int MAXCARDS = 4;

    public NetworkCard card;

    public GameObject interactionCardPanel;
    public GameObject movementCardPanel;

    public Text typeCardText;
    public Button doneInteractionButton;

    private int cardSelectedIndex = -1;
    private bool finishInteraction = false;
    private bool interacting = false;

    private GameObject[] interactionPanels;
    private GameObject[] movementPanels;
    private List<GameObject[]> movementHand = new List<GameObject[]>();
    private List<GameObject[]> interactionHand = new List<GameObject[]>();
    private GameObject[] cardList;
    private TutorialTurnSystem playManager;

    void Awake()
    {
        playManager = FindObjectOfType<TutorialTurnSystem>();
        doneInteractionButton.onClick.AddListener(FinishInteractionTurn);
    }

    void Start()
    {
        movementPanels = new GameObject[TutorialTurnSystem.players.Count];
        interactionPanels = new GameObject[TutorialTurnSystem.players.Count];
        for (int i = 0; i < TutorialTurnSystem.players.Count; i++)
        {
            movementPanels[i] = Instantiate(movementCardPanel, transform);
            cardList = new GameObject[MAXCARDS];
            for (int j = 0; j < MAXCARDS; j++)
            {
                cardList[j] = Instantiate(card.gameObject, movementPanels[i].transform);
                cardList[j].GetComponent<NetworkCard>().SetRandomMovementCard();
            }
            movementHand.Add(cardList);

            interactionPanels[i] = Instantiate(interactionCardPanel, transform);
            cardList = new GameObject[MAXCARDS];
            for (int j = 0; j < MAXCARDS; j++)
            {
                cardList[j] = Instantiate(card.gameObject, interactionPanels[i].transform);
                cardList[j].GetComponent<NetworkCard>().SetRandomInteractionCard();
            }
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
                        if (!finishInteraction)
                        {
                            doneInteractionButton.gameObject.SetActive(true);
                            interactionPanels[i].SetActive(true);
                            movementPanels[i].SetActive(false);
                            typeCardText.text = "Interaction Cards";
                            for (int j = MAXCARDS - 1; j >= 0 && !interacting; j--)
                            {
                                if (interactionHand[playManager.PlayerTurnIndex][j].GetComponent<NetworkCard>().Selected)
                                {
                                    if (playManager.PlayerMoving || playManager.IsMiniGameRunning)
                                    {
                                        interactionHand[playManager.PlayerTurnIndex][j].GetComponent<NetworkCard>().Selected = false;
                                    }
                                    else
                                    {
                                        cardSelectedIndex = interactionHand[playManager.PlayerTurnIndex][j].GetComponent<NetworkCard>().Index;
                                        GameObject rmv = interactionHand[playManager.PlayerTurnIndex][j];
                                        Destroy(rmv);
                                        interactionHand[playManager.PlayerTurnIndex][j] = Instantiate(card.gameObject, interactionPanels[i].transform);
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
                                    if (playManager.PlayerMoving || playManager.IsMiniGameRunning)
                                    {
                                        movementHand[playManager.PlayerTurnIndex][j].GetComponent<NetworkCard>().Selected = false;
                                    }
                                    else
                                    {
                                        cardSelectedIndex = movementHand[playManager.PlayerTurnIndex][j].GetComponent<NetworkCard>().Index;
                                        GameObject rmv = movementHand[playManager.PlayerTurnIndex][j];
                                        Destroy(rmv);
                                        movementHand[playManager.PlayerTurnIndex][j] = Instantiate(card.gameObject, movementPanels[i].transform);
                                        playManager.MovePlayer(cardSelectedIndex);
                                        finishInteraction = false;
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

    private void FinishInteractionTurn()
    {
        finishInteraction = true;
        doneInteractionButton.gameObject.SetActive(false);
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
}
