using UnityEngine;

public class TutorialPlayer : MonoBehaviour
{
    public Sprite[] playerTags;
    public SpriteRenderer playerTag;

    private int waypointIndex = 0;
    private bool skip = false;

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        TutorialTurnSystem.players.Add(this);
        playerTag.sprite = playerTags[TutorialTurnSystem.players.Count - 1];
        playerTag.enabled = false;
    }

    public void WalkAnimation(bool value)
    {
        animator.SetBool("playerMove", value);
    }
    

    public int WaypointIndex
    {
        get
        {
            return waypointIndex;
        }
        set
        {
            waypointIndex = value;
        }
    }

    public bool Skip
    {
        get
        {
            return skip;
        }

        set
        {
            skip = value;
        }
    }

    public bool TextVisible 
    {
        get
        {
            return playerTag.enabled;
        }   
        set
        {
            playerTag.enabled = value;
        }
    }

}
