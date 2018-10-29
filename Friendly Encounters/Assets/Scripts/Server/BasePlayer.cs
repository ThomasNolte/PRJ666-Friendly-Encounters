using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour {

    private int waypointIndex = 0;

	void Awake() {
        Camera.main.GetComponent<PlayCamera>().setTarget(gameObject.transform);
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
