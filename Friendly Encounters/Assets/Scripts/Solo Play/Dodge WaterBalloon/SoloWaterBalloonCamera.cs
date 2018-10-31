using UnityEngine;

public class SoloWaterBalloonCamera : MonoBehaviour
{
    private GameObject player;
    private Vector3 offset;
    void Start()
    {
        player = GameObject.Find("Player");
        offset = transform.position - player.transform.position;
    }
    
    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.transform.position + offset;
        }
    }
}
