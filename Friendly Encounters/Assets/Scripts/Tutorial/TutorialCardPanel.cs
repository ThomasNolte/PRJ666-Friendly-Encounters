using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCardPanel : MonoBehaviour
{
    public const int MAXCARDS = 4;

    public NetworkCard card;
    public Button deck;

    private int cardCount = 0;
    private int cardSelectedIndex = -1;

    List<GameObject> playersHand = new List<GameObject>();
    TutorialTurnSystem playManager;

    void Awake()
    {
        playManager = FindObjectOfType<TutorialTurnSystem>();
        deck.onClick.AddListener(DrawCard);
    }

    private void DrawCard()
    {
        if (cardCount < MAXCARDS)
        {
            cardCount++;
            playersHand.Add(Instantiate(card.gameObject, transform));
        }
    }

    void Update()
    {
        if (playersHand.Count > 0)
        {
            for (int i = playersHand.Count - 1; i >= 0; i--)
            {
                if (playersHand[i].GetComponent<NetworkCard>().Selected)
                {
                    if (playManager.movePlayer)
                    {
                        playersHand[i].GetComponent<NetworkCard>().Selected = false;
                    }
                    else
                    {
                        cardSelectedIndex = playersHand[i].GetComponent<NetworkCard>().Index;
                        cardCount--;
                        GameObject rmv = playersHand[i];
                        playersHand.Remove(playersHand[i]);
                        Destroy(rmv);
                        playManager.MovePlayer(cardSelectedIndex);
                    }
                }
            }
        }
    }


}
