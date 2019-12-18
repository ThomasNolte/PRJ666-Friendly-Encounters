using UnityEngine;

public class SoloWaterBalloon : MonoBehaviour {

    private SoloWaterBalloonSpawner spawner;
    private Vector2 movePosition;
    private Vector2 prevVelocity;
    private Rigidbody2D rb;

    private bool hasStop = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spawner = FindObjectOfType<SoloWaterBalloonSpawner>();
    }

    void FixedUpdate()
    {
        if (!MyGameManager.pause)
        {
            if (hasStop)
            {
                rb.velocity = prevVelocity;
                hasStop = false;
            }
            rb.AddForce(movePosition);
            prevVelocity = rb.velocity;
            DeleteOnBounds(spawner.topLeft, spawner.bottomRight);
        }
        else
        {
            hasStop = true;
            rb.velocity = Vector2.zero;
        }
    }

    public void DeleteOnBounds(Transform topLeft, Transform bottomRight)
    {
        if (transform.position.x > bottomRight.position.x ||
            transform.position.x < topLeft.position.x ||
            transform.position.y > topLeft.position.y ||
            transform.position.y < bottomRight.position.y)
        {
            Destroy(gameObject);
        }
    }


    public Vector2 MovePosition
    {
        get {

            return movePosition;
        }
        set {
            movePosition = value;
        }
    }

    public Rigidbody2D Body {
        get {
            return rb;
        }
        set {
            rb = value;
        }
    }
    
}
