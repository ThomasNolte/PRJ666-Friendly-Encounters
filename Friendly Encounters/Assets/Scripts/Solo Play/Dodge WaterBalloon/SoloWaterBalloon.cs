using UnityEngine;

public class SoloWaterBalloon : MonoBehaviour {

    public float lifeTime = 5f;

    private Vector2 movePosition;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        if (!MyGameManager.pause)
        {
            rb.AddForce(movePosition);
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
