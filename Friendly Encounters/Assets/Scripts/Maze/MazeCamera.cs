using UnityEngine;
using System.Collections;

public class MazeCamera : MonoBehaviour
{
    [HideInInspector]
    public Transform playerTransform;
    private Vector3 offset;         //Private variable to store the offset distance between the player and camera

    private bool cutscene = true;

    float timeLeft = 2f;
    void Awake()
    {
        Camera.main.orthographicSize = 12;
        Camera.main.transform.position = new Vector3(15f, 7.5f, -10);
        RenderSettings.ambientLight = Color.white;
        cutscene = true;
    }
    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        if (!MyGameManager.pause)
        {
            if (cutscene)
            {
                timeLeft -= Time.deltaTime;
                if (timeLeft < 0)
                {
                    Camera.main.orthographicSize = 6;
                    RenderSettings.ambientLight = Color.gray;
                    cutscene = false;
                }
            }
            else if (playerTransform)
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
