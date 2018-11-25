using UnityEngine;

public class TutorialCardSelection : MonoBehaviour
{
    public GameObject cardPrefab;
    public GameObject cardPanel;

    private TutorialCardPanel mainCardPanel;
    private GameObject[] cards;
    private int[] cardIndexes;
    private int actionType = -1;
    private int originalCardIndex = -1;
    private int playerIndex = -1;
    private int swapCardType = -1;
    private int swapPosition = -1;
    private bool pickedOwnCard = false;

    void Awake()
    {
        mainCardPanel = FindObjectOfType<TutorialCardPanel>();
    }

    void Update()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].GetComponent<NetworkCard>().Selected)
            {
                if (actionType == (int)NetworkCard.CardIndex.SWITCHCARD && !pickedOwnCard)
                {
                    //Don't need to deselect the card since
                    //the card is destroyed
                    swapCardType = cards[i].GetComponent<NetworkCard>().Index;
                    swapPosition = cardIndexes[i];
                    for (int j = cards.Length - 1; j >= 0; j--)
                    {
                        Destroy(cards[j]);
                    }
                    cardIndexes = mainCardPanel.GetCardPositionIndex(playerIndex);
                    cards = new GameObject[cardIndexes.Length];
                    for (int j = 0; j < cards.Length; j++)
                    {
                        cards[j] = Instantiate(cardPrefab, cardPanel.transform);
                    }
                    pickedOwnCard = true;
                }
                else
                {
                    cards[i].GetComponent<NetworkCard>().Selected = false;
                    FinishAction(cardIndexes[i]);
                }
            }
        }
    }

    public void StartCardSelection(int action, int player, int originalCard)
    {
        cardIndexes = mainCardPanel.GetCardPositionIndex(player);
        cards = new GameObject[cardIndexes.Length];
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i] = Instantiate(cardPrefab, cardPanel.transform);
        }
        actionType = action;
        playerIndex = player;
        originalCardIndex = originalCard;
    }

    public void SwitchCards(int action, int player, int originalCard, int currentPlayerIndex)
    {
        int[] currentPlayerCards = mainCardPanel.GetPlayersHand(currentPlayerIndex, (int)NetworkCard.CardIndex.SWITCHCARD);
        cards = new GameObject[currentPlayerCards.Length];
        for (int i = 0; i < currentPlayerCards.Length; i++)
        {
            cards[i] = Instantiate(cardPrefab, cardPanel.transform);
            cards[i].GetComponent<NetworkCard>().SetCard(currentPlayerCards[i]);
        }
        cardIndexes = mainCardPanel.GetCardPositionIndex(currentPlayerIndex, (int)NetworkCard.CardIndex.SWITCHCARD);
        actionType = action;
        playerIndex = player;
        originalCardIndex = originalCard;
    }

    public void FinishAction(int selectedCardPosition)
    {
        mainCardPanel.GetComponent<TutorialCardPanel>().DoAction(actionType, selectedCardPosition, playerIndex, originalCardIndex, swapCardType, swapPosition);
        Destroy(gameObject);
    }
}
