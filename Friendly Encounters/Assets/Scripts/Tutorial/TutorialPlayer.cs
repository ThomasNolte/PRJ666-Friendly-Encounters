using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TutorialPlayer : MonoBehaviour
{
    private int waypointIndex = 0;

    void Awake()
    {
        TutorialTurnSystem.players.Add(this);
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
