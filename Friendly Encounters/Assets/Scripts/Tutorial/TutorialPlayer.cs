using UnityEngine;

public class TutorialPlayer : MonoBehaviour
{
    private int waypointIndex = 0;
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        TutorialTurnSystem.players.Add(this);
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
}
