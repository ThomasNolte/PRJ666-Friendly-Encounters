using UnityEngine;

public class SoloPlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10f;
    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        rb2d.AddForce(movement * moveSpeed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("FallingBlock"))
        {
            Destroy(gameObject);
        }
    }
}
