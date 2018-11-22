using UnityEngine;
using UnityEngine.Networking;

public class WaterBalloon : NetworkBehaviour {

    private WaterBalloonSpawner spawner;
    private Vector2 movePosition;
    private Rigidbody2D rb;
    NetworkTransform networkTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        networkTransform = GetComponent<NetworkTransform>();
        spawner = FindObjectOfType<WaterBalloonSpawner>();
    }

    [ServerCallback]
    void FixedUpdate()
    {
            rb.AddForce(movePosition);
            DeleteOnBounds(spawner.topLeft, spawner.bottomRight);
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
        get
        {

            return movePosition;
        }
        set
        {
            movePosition = value;
        }
    }

    public Rigidbody2D Body
    {
        get
        {
            return rb;
        }
        set
        {
            rb = value;
        }
    }


    [ServerCallback]
    void OnTriggerEnter2D(Collider2D collision)
    {
        //we collide so we dirty the NetworkTransform to sync it on clients.
        networkTransform.SetDirtyBit(1);

        if (collision.gameObject.tag == "Player")
        {
            NetworkPlayer p = collision.gameObject.GetComponent<NetworkPlayer>();
            p.Kill();
        }
    }
}
