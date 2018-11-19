using UnityEngine;

public class MazeCamera : MonoBehaviour
{
    [HideInInspector]
    public Transform playerTransform;
    private Vector3 offset;         //Private variable to store the offset distance between the player and camera

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
