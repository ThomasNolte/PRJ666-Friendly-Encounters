using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour {

    private int waypointIndex = 0;

    Rigidbody2D rb;

	void Awake() {
        Camera.main.GetComponent<PlayCamera>().setTarget(gameObject.transform);
        rb = GetComponent<Rigidbody2D>();

        PlayManager.players.Add(this);
    }

    public int WaypointIndex
    {
        get {
            return waypointIndex;
        }
        set {
            waypointIndex = value;
        }
    }
}
