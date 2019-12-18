using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CardPanel : NetworkBehaviour
{
    public const int MAXCARDS = 4;

    public NetworkCard card;

    public GameObject interactionCardPanel;
    public GameObject movementCardPanel;
    public GameObject deck;
    public Transform parentCardPanels;

    public Text typeCardText;

    [SyncVar] private int cardSelectedPosition = -1;
    [SyncVar] private bool finishInteraction = false;
    [SyncVar] private bool interacting = false;
    [SyncVar] private bool drawnCard = false;

    private GameObject interactionPanel;
    private GameObject movementPanel;
    private GameObject[] movementHand;
    private GameObject[] interactionHand;
    private PlayManager playManager;

    [ServerCallback]
    void Awake()
    {
        playManager = FindObjectOfType<PlayManager>();
        deck.GetComponentInChildren<Button>().onClick.AddListener(CmdDrawCard);
        deck.SetActive(true);
    }

    [ServerCallback]
    void Start()
    {
        if (localPlayerAuthority)
        {
            movementPanel = Instantiate(movementCardPanel, parentCardPanels);
            NetworkServer.Spawn(movementPanel);
            movementHand = new GameObject[MAXCARDS];
            for (int j = 0; j < MAXCARDS - 1; j++)
            {
                movementHand[j] = Instantiate(card.gameObject, movementPanel.transform);
                movementHand[j].GetComponent<NetworkCard>().SetRandomMovementCard();
                NetworkServer.Spawn(movementHand[j]);
            }
            movementHand[MAXCARDS - 1] = Instantiate(card.gameObject, movementPanel.transform);
            movementHand[MAXCARDS - 1].GetComponent<NetworkCard>().SetOriginalImage();
            NetworkServer.Spawn(movementHand[MAXCARDS - 1]);

            interactionPanel = Instantiate(interactionCardPanel, parentCardPanels);
            interactionHand = new GameObject[MAXCARDS];
            NetworkServer.Spawn(interactionPanel);
            for (int j = 0; j < MAXCARDS - 1; j++)
            {
                interactionHand[j] = Instantiate(card.gameObject, interactionPanel.transform);
                interactionHand[j].GetComponent<NetworkCard>().SetRandomInteractionCard();
                NetworkServer.Spawn(interactionHand[j]);
            }
            interactionHand[MAXCARDS - 1] = Instantiate(card.gameObject, interactionPanel.transform);
            interactionHand[MAXCARDS - 1].GetComponent<NetworkCard>().SetOriginalImage();
            NetworkServer.Spawn(interactionHand[MAXCARDS - 1]);
        }
    }

    [ServerCallback]
    void Update()
    {
        if (!playManager.IsMiniGameRunning)
        {
            for (int i = 0; i < TutorialTurnSystem.players.Count; i++)
            {
                if (playManager.PlayerTurnIndex == i)
                {
                    if (!drawnCard)
                    {
                        interactionPanel.SetActive(true);
                        typeCardText.text = "Interaction Cards";
                    }
                    else
                    {
                        if (!finishInteraction)
                        {
                            interactionPanel.SetActive(true);
                            movementPanel.SetActive(false);
                            for (int j = 0; j < MAXCARDS && !interacting; j++)
                            {
                                if (interactionHand[j].GetComponent<NetworkCard>().Selected)
                                {
                                    if (playManager.PlayerMoving ||
                                        playManager.IsMiniGameRunning ||
                                        interactionHand[j].GetComponent<NetworkCard>().Empty)
                                    {
                                        interactionHand[j].GetComponent<NetworkCard>().Selected = false;
                                    }
                                    else
                                    {
                                        //Disable interacting first (Order matters here)
                                        interacting = true;
                                        cardSelectedPosition = j;
                                        playManager.InteractPlayer(interactionHand[j].GetComponent<NetworkCard>().Index, j);
                                    }
                                }
                            }
                        }
                        else
                        {
                            interactionPanel.SetActive(false);
                            movementPanel.SetActive(true);
                            typeCardText.text = "Movement Cards";
                            for (int j = MAXCARDS - 1; j >= 0; j--)
                            {
                                if (movementHand[j].GetComponent<NetworkCard>().Selected)
                                {
                                    if (playManager.PlayerMoving ||
                                        playManager.IsMiniGameRunning ||
                                        movementHand[j].GetComponent<NetworkCard>().Empty)
                                    {
                                        movementHand[j].GetComponent<NetworkCard>().Selected = false;
                                    }
                                    else
                                    {
                                        playManager.MovePlayer(movementHand[j].GetComponent<NetworkCard>().Index);
                                        movementHand[j].GetComponent<NetworkCard>().SetOriginalImage();
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
                    interactionPanel.SetActive(false);
                    movementPanel.SetActive(false);
                }
            }
        }
    }

    public void DeselectCard()
    {
        interactionHand[cardSelectedPosition].GetComponent<NetworkCard>().Selected = false;
    }

    public void RemoveCard()
    {
        interactionHand[cardSelectedPosition].GetComponent<NetworkCard>().SetOriginalImage();
    }

    public void ResetCard(int originalIndex)
    {
        interactionHand[cardSelectedPosition].GetComponent<NetworkCard>().SetCard(originalIndex);
    }

    [Command]
    public void CmdDrawCard()
    {
        for (int i = 0; i < MAXCARDS; i++)
        {
            if (interactionHand[i].GetComponent<NetworkCard>().Empty)
            {
                interactionHand[i].GetComponent<NetworkCard>().SetRandomInteractionCard();
                i = MAXCARDS;
            }
        }
        for (int i = 0; i < MAXCARDS; i++)
        {
            if (movementHand[i].GetComponent<NetworkCard>().Empty)
            {
                movementHand[i].GetComponent<NetworkCard>().SetRandomMovementCard();
                i = MAXCARDS;
            }
        }
        drawnCard = true;
        deck.SetActive(false);
    }

    [Command]
    public void CmdActionDrawCard()
    {
        int emptyCount = 0;
        for (int i = 0; i < MAXCARDS && emptyCount != 2; i++)
        {
            if (interactionHand[i].GetComponent<NetworkCard>().Empty)
            {
                interactionHand[i].GetComponent<NetworkCard>().SetRandomInteractionCard();
                emptyCount++;
            }
        }
    }

    public void DoAction(int cardType, int cardIndex, int playerIndex, int originalCardIndex, int swapCardType, int swapPosition)
    {
        switch (cardType)
        {
            case (int)NetworkCard.CardIndex.DISCARDCARD:
                interactionHand[cardIndex].GetComponent<NetworkCard>().SetOriginalImage();
                break;
            case (int)NetworkCard.CardIndex.SWITCHCARD:
                interactionHand[originalCardIndex].GetComponent<NetworkCard>().SetOriginalImage();
                interactionHand[swapPosition].GetComponent<NetworkCard>().SetCard(interactionHand[cardIndex].GetComponent<NetworkCard>().Index);
                interactionHand[cardIndex].GetComponent<NetworkCard>().SetCard(swapCardType);
                break;
            case (int)NetworkCard.CardIndex.STEALCARD:
                interactionHand[originalCardIndex].GetComponent<NetworkCard>().SetCard(interactionHand[cardIndex].GetComponent<NetworkCard>().Index);
                interactionHand[cardIndex].GetComponent<NetworkCard>().SetOriginalImage();
                break;
        }
        playManager.InteractingWithPlayer = false;
    }

    //Returns the card positions of playerIndex hand
    //excludeCard allows you to exclude a card from selection
    public int[] GetCardPositionIndex(int playerIndex, int excludeCard = -1)
    {
        int size = GetNumberCards(playerIndex);
        if (excludeCard != -1) size -= 1;
        int[] indexes = new int[size];
        int count = 0;
        int i = 0;

        //foreach (GameObject obj in interactionHand[playerIndex])
        //{
        //    if (!obj.GetComponent<NetworkCard>().Empty &&
        //        obj.GetComponent<NetworkCard>().Index != excludeCard)
        //    {
        //        indexes[i] = count;
        //        i++;
        //    }
        //    count++;
        //}

        return indexes;
    }

    //Return the amount of cards playerIndex has
    public int GetNumberCards(int playerIndex)
    {
        int count = 0;
        //foreach (GameObject obj in interactionHand[playerIndex])
        //{
        //    if (!obj.GetComponent<NetworkCard>().Empty)
        //    {
        //        count++;
        //    }
        //}
        return count;
    }

    //Getting the indexes of a playerIndex hand
    //excludeCard allows you to exclude a card from selection
    public int[] GetPlayersHand(int playerIndex, int excludeCard = -1)
    {
        int size = GetNumberCards(playerIndex);
        if (excludeCard != -1) size -= 1;
        int[] indexes = new int[size];
        int i = 0;

        //foreach (GameObject obj in interactionHand[playerIndex])
        //{
        //    if (!obj.GetComponent<NetworkCard>().Empty &&
        //        obj.GetComponent<NetworkCard>().Index != excludeCard)
        //    {
        //        indexes[i] = obj.GetComponent<NetworkCard>().Index;
        //        i++;
        //    }
        //}
        return indexes;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        ClientScene.RegisterPrefab(movementCardPanel);
        ClientScene.RegisterPrefab(interactionCardPanel);
        ClientScene.RegisterPrefab(card.gameObject);
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
