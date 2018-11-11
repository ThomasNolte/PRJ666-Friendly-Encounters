using UnityEngine;

public class TutorialCamera : MonoBehaviour
{
    public static TutorialCamera instance = null;
    private GameObject player;
    private Vector3 offset;

    TutorialTurnSystem manager;
    Transform playerTransform;

    void Awake()
    {
        manager = FindObjectOfType<TutorialTurnSystem>();
        playerTransform = TutorialTurnSystem.players[0].transform;
    }

    void LateUpdate()
    {
        if (manager.IsMiniGameRunning)
        {
            GetComponent<Camera>().orthographicSize = 5f;
            Vector3 pos = new Vector3(0, 0, -10);

            transform.position = pos;
        }
        else if (manager.TurnFinished) {
            playerTransform = TutorialTurnSystem.players[manager.PlayerTurnIndex].transform;
            Vector3 playerPos = new Vector3(playerTransform.position.x, playerTransform.position.y, -10);
            transform.position = Vector3.MoveTowards(transform.position, playerPos, (manager.PlayerMoveSpeed * 1.5f) * Time.deltaTime);
            if (transform.position == playerPos) {
                manager.TurnFinished = false;
                manager.movePlayer = false;
            }
        }
        //If the player is gone no need to move the camera
        if (playerTransform != null && !manager.TurnFinished)
        {
            Vector3 pos = playerTransform.position + new Vector3(0, 0, -10);
            pos.z = -10;

            transform.position = pos;
        }

    }

    public void setTarget(Transform target)
    {
        playerTransform = target;
    }
}
