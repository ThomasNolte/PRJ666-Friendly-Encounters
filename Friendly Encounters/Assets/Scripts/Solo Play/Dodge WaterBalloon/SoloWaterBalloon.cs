using UnityEngine;

public class SoloWaterBalloon : MonoBehaviour {

    public float lifeTime = 5f;
    public float moveSpeed = 0.1f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
