using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerMiniGame : MonoBehaviour
{

    private GameObject player;
    private Vector3 offset;

    private float cameraSizeOffsetX = 3.3f;
    private float cameraSizeOffsetY = 2f;

    public Transform topLeft;
    public Transform bottomRight;

    void Awake()
    {
        player = GameObject.Find("Player");
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        Vector3 pos = player.transform.position + offset;
        //Set the camera's transform to players
        //but plus the offset between camera and player

        pos.x = Mathf.Clamp(pos.x, topLeft.position.x + cameraSizeOffsetX, bottomRight.position.x - cameraSizeOffsetX);
        pos.y = Mathf.Clamp(pos.y, bottomRight.position.y + cameraSizeOffsetY, topLeft.position.y - cameraSizeOffsetY);
        pos.z = -10;

        transform.position = pos;
    }
}
