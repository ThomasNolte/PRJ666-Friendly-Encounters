using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRunner : MonoBehaviour
{
    public float walkSpeed, rotationSpeed;
    private bool playerMoving = false;
    public Transform rotationTransform;
    Vector2 direction = Vector2.zero;

    private Animator animator;

    int targetX = 1;
    int targetY = 1;

    int currentX = 1;
    int currentY = 1;

    float currentAngle;
    float lastAngle;

    void Awake()
    {
        FindObjectOfType<MazeCamera>().setTarget(gameObject.transform);
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!MyGameManager.pause)
        {
            bool targetReached = transform.position.x == targetX && transform.position.y == targetY;
            currentX = Mathf.FloorToInt(transform.position.x);
            currentY = Mathf.FloorToInt(transform.position.y);
            direction.x = Input.GetAxisRaw("Horizontal");
            direction.y = Input.GetAxisRaw("Vertical");

            playerMoving = (direction == Vector2.zero) ? false : true;

            float angle = 0;

            if (direction.x > 0)
            {
                angle = 270;

                if (MazeGenerator.instance.GetMazeGridCell(currentX + 1, currentY) && targetReached)
                {
                    targetX = currentX + 1;
                    targetY = currentY;
                }
            }
            else if (direction.x < 0)
            {
                angle = 90;

                if (MazeGenerator.instance.GetMazeGridCell(currentX - 1, currentY) && targetReached)
                {
                    targetX = currentX - 1;
                    targetY = currentY;
                }
            }
            else if (direction.y > 0)
            {
                angle = 0;

                if (MazeGenerator.instance.GetMazeGridCell(currentX, currentY + 1) && targetReached)
                {
                    targetX = currentX;
                    targetY = currentY + 1;
                }
            }
            else if (direction.y < 0)
            {
                angle = 180;

                if (MazeGenerator.instance.GetMazeGridCell(currentX, currentY - 1) && targetReached)
                {
                    targetX = currentX;
                    targetY = currentY - 1;
                }
            }
            else
            {
                angle = lastAngle;
            }

            currentAngle = Mathf.LerpAngle(currentAngle, angle, rotationSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetX, targetY), walkSpeed * Time.deltaTime);
            rotationTransform.eulerAngles = new Vector3(0, 0, currentAngle);
            lastAngle = angle;
        }
    }

    void LateUpdate()
    {
        animator.SetBool("playerMove", playerMoving);
    }
}
