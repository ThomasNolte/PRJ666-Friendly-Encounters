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

    public const int DEFAULTPOINTS = 10;
    public const int MAXCOLOURS = 6;

    public Sprite[] imgs;

    private int playerIndex = -1;
    private bool ownByPlayer = false;
    private int points = DEFAULTPOINTS;

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

    public void RankUp()
    {
        points += DEFAULTPOINTS;
    }

    //This function is called each time player clicks on GameObject (Need boxcollider for this to work)
    private void OnMouseDown()
    {
        if (ownByPlayer &&
            FindObjectOfType<TutorialTurnSystem>().UpgradeTile &&
            FindObjectOfType<TutorialTurnSystem>().PlayerTurnIndex == playerIndex)
        {
            FindObjectOfType<TutorialTurnSystem>().UpgradeTile = false;
            FindObjectOfType<TutorialTurnSystem>().IsLookingAtBoard = false;
            RankUp();
        }
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

    public int Points
    {
        get
        {
            return points;
        }

        set
        {
            points = value;
        }
    }
}
