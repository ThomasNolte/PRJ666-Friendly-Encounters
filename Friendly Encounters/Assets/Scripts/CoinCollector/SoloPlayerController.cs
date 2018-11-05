using UnityEngine;

public class SoloPlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10f;
    private bool playerMoving = false;
    private Rigidbody2D rb2d;
    private Animator animator;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        playerMoving = (movement == Vector2.zero) ? false : true;

        rb2d.AddForce(movement * moveSpeed);
    }

    void LateUpdate()
    {
        animator.SetBool("playerMove", playerMoving);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("FallingBlock"))
        {
            SoloWaterBalloonSpawner.gameOver = true;
            Destroy(gameObject);
        }
    }
}
