using UnityEngine;

public class TutorialCardSelection : MonoBehaviour
{
    public GameObject cardPrefab;
    public GameObject cardPanel;

    private GameObject[] cards;
    private int selectedCardIndex = -1;
    private bool cardHasBeenSelected = false;
    private bool canvasActive = false;

    void Awake()
    {
        cards = new GameObject[TutorialCardPanel.MAXCARDS];
        for (int i = 0; i < TutorialCardPanel.MAXCARDS; i++)
        {
            cards[i] = Instantiate(cardPrefab, cardPanel.transform);
        }
    }

    void Update()
    {
        for (int i = 0; i < TutorialCardPanel.MAXCARDS && !cardHasBeenSelected; i++)
        {
            if (cards[i].GetComponent<NetworkCard>().Selected)
            {
                selectedCardIndex = i;
                cardHasBeenSelected = true;
                TurnCanvasOff();
            }
        }
    }

    public void DoAction(int cardType, int playerIndex)
    {
        gameObject.SetActive(true);
        switch (cardType)
        {
            case (int)NetworkCard.CardIndex.DISCARDCARD:

                break;
            case (int)NetworkCard.CardIndex.SWITCHCARD:

                break;
            case (int)NetworkCard.CardIndex.STEALCARD:

                break;
        }
    }

    public void TurnCanvasOff()
    {
        gameObject.SetActive(false);
        cardHasBeenSelected = false;
    }

    public int SelectedCardIndex
    {
        get
        {
            return selectedCardIndex;
        }
        set
        {
            selectedCardIndex = value;
        }
    }

    public bool CanvasActive
    {
        get
        {
            return canvasActive;
        }
        set
        {
            canvasActive = value;
        }
    }
}
