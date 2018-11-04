using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCardPanel : MonoBehaviour
{
    public const int MAXCARDS = 4;

    public NetworkCard card;
    
    private int cardSelectedIndex = -1;

    private List<GameObject[]> playersHand = new List<GameObject[]>();
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
            }
            playersHand.Add(cardList);
        }
    }

    void Update()
    {
        if (!playManager.IsMiniGameRunning)
        {
            for (int i = 0; i < TutorialTurnSystem.players.Count; i++)
            {
                if (playManager.PlayerTurnIndex == i)
                {
                    for (int j = MAXCARDS - 1; j >= 0; j--)
                    {
                        playersHand[playManager.PlayerTurnIndex][j].SetActive(true);
                        if (playersHand[playManager.PlayerTurnIndex][j].GetComponent<NetworkCard>().Selected)
                        {
                            if (playManager.movePlayer || playManager.IsMiniGameRunning)
                            {
                                playersHand[playManager.PlayerTurnIndex][j].GetComponent<NetworkCard>().Selected = false;
                            }
                            else
                            {
                                cardSelectedIndex = playersHand[playManager.PlayerTurnIndex][j].GetComponent<NetworkCard>().Index;
                                GameObject rmv = playersHand[playManager.PlayerTurnIndex][j];
                                Destroy(rmv);
                                playersHand[playManager.PlayerTurnIndex][j] = Instantiate(card.gameObject, transform);
                                playManager.MovePlayer(cardSelectedIndex);
                            }
                        }
                    }
                }
                else
                {
                    for (int k = MAXCARDS - 1; k >= 0; k--)
                    {
                        playersHand[i][k].SetActive(false);
                    }
                }

            }
        }
    }


}
