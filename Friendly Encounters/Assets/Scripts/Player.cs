using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//NetworkBehaviour

public class Player: MonoBehaviour{

    public Transform[] waypoints;

    [SerializeField]
    private float moveSpeed = 5f;

    [HideInInspector]
    public int waypointIndex = 0;

    public bool moveAllowed = false;

    void Start() {
        //Setting the player's position to the beginning tile
        transform.position = waypoints[waypointIndex].transform.position;
    }

    void Update(){
            if (moveAllowed)
                Move();
    }

    private void Move() {
        if (waypointIndex <= waypoints.Length - 1) {
            transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, moveSpeed * Time.deltaTime);
            if (transform.position == waypoints[waypointIndex].transform.position) {
                waypointIndex += 1;
            }
        }
    }

}
