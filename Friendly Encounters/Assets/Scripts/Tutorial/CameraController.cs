using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private GameObject[] players;
    private GameObject localPlayer;
    private Vector3[] offsets;

    void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Character");
        offsets = new Vector3[TurnSystem.amountOfPlayers];

        for (int i = 0; i < TurnSystem.amountOfPlayers; i++)
        {
            offsets[i] = transform.position - players[i].transform.position;
        }
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        if (Dice.whosTurn == 1)
        {
            transform.position = players[1].transform.position + offsets[1];
        }
        else
        {
            transform.position = players[0].transform.position + offsets[0];
        }
    }
}
