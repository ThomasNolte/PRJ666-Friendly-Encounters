using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCamera : MonoBehaviour
{
    private GameObject player;
    private Vector3 offset;

    Transform playerTransform;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void LateUpdate()
    {
        //If the player is gone no need to move the camera
        if (playerTransform != null)
        {
            Vector3 pos = playerTransform.position + new Vector3(0, 0, -10);
            //Set the camera's transform to players
            //but plus the offset between camera and player

            //pos.x = Mathf.Clamp(pos.x, topLeft.position.x + cameraSizeOffsetX, bottomRight.position.x - cameraSizeOffsetX);
            //pos.y = Mathf.Clamp(pos.y, bottomRight.position.y + cameraSizeOffsetY, topLeft.position.y - cameraSizeOffsetY);
            pos.z = -10;

            transform.position = pos;
        }

    }

    public void setTarget(Transform target)
    {
        playerTransform = target;
    }
}
