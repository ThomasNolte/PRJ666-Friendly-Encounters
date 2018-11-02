using UnityEngine;

public class Waypoint : MonoBehaviour
{

    public enum ColorIndex
    {
        RED,
        BLUE,
        GREEN,
        YELLOW,
        CYAN,
        PURPLE
    }

    public const int MAXCOLOURS = 6;

    public Sprite[] imgs;

    private int playerIndex = -1;
    private bool ownByPlayer = false;
    private int rank = 0;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetPlayer(int index)
    {
        sr.sprite = imgs[index];
        ownByPlayer = true;
        playerIndex = index;
    }

    public bool OwnByPlayer
    {
        get
        {
            return ownByPlayer;
        }
        set
        {
            ownByPlayer = value;
        }
    }

    public int PlayerIndex
    {
        get
        {
            return playerIndex;
        }
        set
        {
            playerIndex = value;
        }
    }

    public int Rank
    {
        get
        {
            return rank;
        }
        set
        {
            rank = value;
        }
    }
}
