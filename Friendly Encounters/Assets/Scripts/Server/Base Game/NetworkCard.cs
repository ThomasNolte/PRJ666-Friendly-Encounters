using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkCard : NetworkBehaviour
{
    public enum CardIndex
    {
        DISCARDCARD,
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
    public const int MAXIMAGES = 14;
    public const int MOVEMENTCARDCUTOFF = 9;

    public Sprite[] cardImages;
    public Sprite originalImage;
    
    [SyncVar] private int index;
    [SyncVar] private bool selected = false;
    [SyncVar] private bool empty = false;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(CmdOnSelected);
    }

    public void SetRandomMovementCard()
    {
        index = Random.Range(MOVEMENTCARDCUTOFF, MAXIMAGES);
        GetComponent<Image>().sprite = cardImages[index];
        GetComponent<Button>().onClick.AddListener(CmdOnSelected);
        index -= MOVEMENTCARDCUTOFF;
        selected = false;
        empty = false;
    }

    public void SetRandomInteractionCard()
    {
        index = Random.Range(0, 9);
        GetComponent<Image>().sprite = cardImages[index];
        GetComponent<Button>().onClick.AddListener(CmdOnSelected);
        selected = false;
        empty = false;
    }

    public void SetOriginalImage()
    {
        index = -1;
        GetComponent<Image>().sprite = originalImage;
        selected = false;
        empty = true;
    }

    public void SetCard(int selectedIndex)
    {
        index = selectedIndex;
        GetComponent<Image>().sprite = cardImages[index];
        selected = false;
        empty = false;
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

    public bool Empty
    {
        get
        {
            return empty;
        }
        set
        {
            empty = value;
        }
    }

    [Command]
    private void CmdOnSelected()
    {
        Selected = true;
    }
}
