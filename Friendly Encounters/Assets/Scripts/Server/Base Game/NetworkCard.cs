using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkCard : MonoBehaviour
{
    public enum CardIndex
    {
        DISCARDCARD,
        MOVEBACK,
        MOVEFORWARD,
        DRAWCARD,
        SWITCHPOSITION,
        SWITCHCARD,
        DUELCARD,
        STEALCARD,
        SKIPTURN,
        UPGRADETILE,
        MOVE1,
        MOVE2,
        MOVE3,
        MOVE4,
        MOVE5
    }
    public const int MAXIMAGES = 15;
    public const int MOVEMENTCARDCUTOFF = 10;

    public Sprite[] cardImages;

    private int index;
    private bool selected = false;

    public void SetRandomMovementCard()
    {
        index = Random.Range(MOVEMENTCARDCUTOFF, MAXIMAGES);
        GetComponent<Image>().sprite = cardImages[index];
        GetComponent<Button>().onClick.AddListener(OnSelected);
        index -= MOVEMENTCARDCUTOFF;
    }

    public void SetRandomInteractionCard()
    {
        index = Random.Range(0, 9);
        GetComponent<Image>().sprite = cardImages[index];
        GetComponent<Button>().onClick.AddListener(OnSelected);
    }

    public int Index
    {
        get
        {
            return index;
        }
        set
        {
            index = value;
        }
    }

    public bool Selected
    {
        get
        {
            return selected;
        }
        set
        {
            selected = value;
        }
    }

    private void OnSelected()
    {
        Selected = true;
    }
}
