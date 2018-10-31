using UnityEngine;

public class SoloWaterBalloon : MonoBehaviour {

    public float lifeTime = 5f;
    public float moveSpeed = 0.1f;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector3 pos = transform.position;

        rb.AddForce(pos);
        Destroy(gameObject, lifeTime);
    }
}
