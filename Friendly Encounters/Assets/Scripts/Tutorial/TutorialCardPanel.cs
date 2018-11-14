using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCardPanel : MonoBehaviour
{
    public const int MAXCARDS = 4;

    public NetworkCard card;
    
    private int cardSelectedIndex = -1;
    private bool finishSelectingCards = false;

    private List<GameObject[]> movementHand = new List<GameObject[]>();
    private List<GameObject[]> interactionHand = new List<GameObject[]>();
    private GameObject[] cardList;
    private TutorialTurnSystem playManager;

    void Awake()
    {
        playManager = FindObjectOfType<TutorialTurnSystem>();
    }

    void Start()
    {
        for (int i = 0; i < TutorialTurnSystem.players.Count; i++)
        {
            cardList = new GameObject[MAXCARDS];
            for (int j = 0; j < MAXCARDS; j++)
            {
                cardList[j] = Instantiate(card.gameObject, transform);
                cardList[j].GetComponent<NetworkCard>().SetRandomMovementCard();
            }
            movementHand.Add(cardList);
            for (int j = 0; j < MAXCARDS; j++)
            {
                cardList[j] = Instantiate(card.gameObject, transform);
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
                        //for (int j = MAXCARDS - 1; j >= 0; j--)
                        //{
                        //    interactionHand[playManager.PlayerTurnIndex][j].SetActive(true);
                        //    if (interactionHand[playManager.PlayerTurnIndex][j].GetComponent<NetworkCard>().Selected)
                        //    {
                        //        if (playManager.PlayerMoving || playManager.IsMiniGameRunning)
                        //        {
                        //            interactionHand[playManager.PlayerTurnIndex][j].GetComponent<NetworkCard>().Selected = false;
                        //        }
                        //        else
                        //        {
                        //            cardSelectedIndex = interactionHand[playManager.PlayerTurnIndex][j].GetComponent<NetworkCard>().Index;
                        //            GameObject rmv = interactionHand[playManager.PlayerTurnIndex][j];
                        //            Destroy(rmv);
                        //            interactionHand[playManager.PlayerTurnIndex][j] = Instantiate(card.gameObject, transform);
                        //            playManager.InteractPlayer(cardSelectedIndex);
                        //        }
                        //    }
                        //}


                        for (int j = MAXCARDS - 1; j >= 0; j--)
                        {
                            movementHand[playManager.PlayerTurnIndex][j].SetActive(true);
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
                                    movementHand[playManager.PlayerTurnIndex][j] = Instantiate(card.gameObject, transform);
                                    playManager.MovePlayer(cardSelectedIndex);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int k = MAXCARDS - 1; k >= 0; k--)
                        {
                            movementHand[i][k].SetActive(false);
                        }
                    }

                }
            }
        }
    }

    public void PlayInteractionCard(int cardIndex)
    {
        switch (cardIndex)
        {
            case (int)NetworkCard.CardIndex.DISCARDCARD:

                break;
            case (int)NetworkCard.CardIndex.MOVEBACK:

                break;
            case (int)NetworkCard.CardIndex.MOVEFORWARD:

                break;
            case (int)NetworkCard.CardIndex.DRAWCARD:

                break;
            case (int)NetworkCard.CardIndex.SWITCHPOSITION:

                break;
            case (int)NetworkCard.CardIndex.SWITCHCARD:

                break;
            case (int)NetworkCard.CardIndex.DUELCARD:

                break;
            case (int)NetworkCard.CardIndex.STEALCARD:

                break;
            case (int)NetworkCard.CardIndex.SKIPTURN:

                break;
            case (int)NetworkCard.CardIndex.UPGRADETILE:

                break;
        }
    }


}
