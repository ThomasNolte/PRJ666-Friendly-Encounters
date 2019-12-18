using UnityEngine;

public class CoinCameraController : MonoBehaviour
{
    [HideInInspector]
    public Transform playerTransform;
    private Vector3 offset;         //Private variable to store the offset distance between the player and camera

    private float cameraSizeOffsetX;
    private float cameraSizeOffsetY;

    public Transform topLeft;
    public Transform bottomRight;

    void Awake()
    {
        cameraSizeOffsetX = CameraExtension.OrthographicBounds(Camera.main.GetComponent<Camera>()).extents.x;
        cameraSizeOffsetY = CameraExtension.OrthographicBounds(Camera.main.GetComponent<Camera>()).extents.y;
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
                pos.x = Mathf.Clamp(pos.x, topLeft.position.x + cameraSizeOffsetX, bottomRight.position.x - cameraSizeOffsetX);
                pos.y = Mathf.Clamp(pos.y, bottomRight.position.y + cameraSizeOffsetY, topLeft.position.y - cameraSizeOffsetY);
                pos.z = -10;

                transform.position = pos;
            }
        }

    }

    public void setTarget(Transform target)
    {
        playerTransform = target;
    }
}
