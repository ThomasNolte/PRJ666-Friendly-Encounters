using UnityEngine;
using System.Collections;

public class MazeCamera : MonoBehaviour
{
    [HideInInspector]
    public Transform playerTransform;
    private Vector3 offset;         //Private variable to store the offset distance between the player and camera
    float timeLeft = 5;
    void Awake()
    {
        Camera.main.orthographicSize = 12;
        RenderSettings.ambientLight = Color.white;
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0) {
            Camera.main.orthographicSize = 6;
            RenderSettings.ambientLight = Color.gray;
        }
     
    }
    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        if (!MyGameManager.pause)
        {
            if (playerTransform)
            {
                Vector3 pos = playerTransform.position + new Vector3(0, 0, -10);
                //Set the camera's transform to players
                //but plus the offset between camera and player

                transform.position = pos;
            }
        }

    }

    public void setTarget(Transform target)
    {
        playerTransform = target;
    }
}
