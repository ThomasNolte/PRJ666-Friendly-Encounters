using UnityEngine;

public class TutorialCamera : MonoBehaviour
{
    public static TutorialCamera instance = null;
    private GameObject player;
    private Vector3 offset;
    private Vector3 prevPosition;

    private TutorialTurnSystem manager;
    private TutorialMiniGameManager tutorialManager;
    private Transform playerTransform;

    void Awake()
    {
        tutorialManager = FindObjectOfType<TutorialMiniGameManager>();
        manager = FindObjectOfType<TutorialTurnSystem>();
        //Setting the initial position of the camera
        playerTransform = TutorialTurnSystem.players[0].transform;
    }

    void LateUpdate()
    {
        if (manager.IsMiniGameRunning)
        {
            Vector3 pos = Vector3.zero;
            switch (tutorialManager.MiniGameSelected)
            {
                case (int)TutorialMiniGameManager.MiniGameState.SIMONSAYS:
                    GetComponent<Camera>().orthographicSize = 5f;
                    pos = new Vector3(0, 0, -10);

                    transform.position = pos;
                    break;
                case (int)TutorialMiniGameManager.MiniGameState.COINCOLLECTOR:
                    pos = playerTransform.position + new Vector3(0, 0, -10);
                    pos.z = -10;

                    transform.position = pos;
                    break;
                case (int)TutorialMiniGameManager.MiniGameState.DODGEWATERBALLOON:
                    GetComponent<Camera>().orthographicSize = 5f;
                    pos = new Vector3(0, 0, -10);

                    transform.position = pos;
                    break;
                case (int)TutorialMiniGameManager.MiniGameState.MATCHINGCARDS:
                    GetComponent<Camera>().orthographicSize = 5f;
                    pos = new Vector3(0, 0, -10);

                    transform.position = pos;
                    break;
                case (int)TutorialMiniGameManager.MiniGameState.MAZE:
                    GetComponent<Camera>().orthographicSize = 5f;
                    pos = new Vector3(0, 0, -10);

                    transform.position = pos;
                    break;
            }
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
            prevPosition = transform.position;
        }

    }

    public void setTarget(Transform target)
    {
        playerTransform = target;
    }
}
