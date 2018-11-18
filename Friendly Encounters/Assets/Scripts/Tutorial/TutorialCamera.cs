using UnityEngine;

public class TutorialCamera : MonoBehaviour
{
    public static TutorialCamera instance = null;
    
    public Transform topLeft;
    public Transform bottomRight;

    private GameObject player;
    private Vector3 offset;

    private float mouseSensitivity = 0.011f;
    private Vector3 lastPosition;

    private float cameraSizeOffsetX;
    private float cameraSizeOffsetY;

    private TutorialTurnSystem manager;
    private TutorialMiniGameManager tutorialManager;
    private Transform playerTransform;

    void Awake()
    {
        tutorialManager = FindObjectOfType<TutorialMiniGameManager>();
        manager = FindObjectOfType<TutorialTurnSystem>();
        if (playerTransform == null && TutorialTurnSystem.players.Count > 0)
        {
            playerTransform = TutorialTurnSystem.players[0].transform;
        }
    }

    void LateUpdate()
    {
        if (!MyGameManager.pause)
        {
            if (manager.IsLookingAtBoard)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    lastPosition = Input.mousePosition;
                }
                if (Input.GetMouseButton(0))
                {
                    Vector3 delta = Input.mousePosition - lastPosition;
                    Debug.Log(Camera.main.transform.position);
                    Debug.Log(CameraExtension.OrthographicBounds(Camera.main.GetComponent<Camera>()));
                    if (Camera.main.transform.position.y > bottomRight.position.y + CameraExtension.OrthographicBounds(Camera.main.GetComponent<Camera>()).extents.y)
                    {
                        Camera.main.transform.Translate(-(delta.x * mouseSensitivity), -(delta.y * mouseSensitivity), 0);
                    }
                    lastPosition = Input.mousePosition;
                }
            }
            else if (manager.IsMiniGameRunning)
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
            else if (manager.TurnFinished)
            {
                playerTransform = TutorialTurnSystem.players[manager.PlayerTurnIndex].transform;
                Vector3 playerPos = new Vector3(playerTransform.position.x, playerTransform.position.y, -10);
                transform.position = Vector3.MoveTowards(transform.position, playerPos, (manager.PlayerMoveSpeed * 1.5f) * Time.deltaTime);
                if (transform.position == playerPos)
                {
                    manager.TurnFinished = false;
                    manager.PlayerMoving = false;
                }
            }

            //If the player is gone no need to move the camera
            if (playerTransform != null && !manager.TurnFinished && !manager.IsMiniGameRunning && !manager.IsLookingAtBoard)
            {
                Vector3 pos = playerTransform.position + new Vector3(0, 0, -10);
                pos.z = -10;

                transform.position = pos;
            }

            if (playerTransform == null && TutorialTurnSystem.players.Count > 0)
            {
                playerTransform = TutorialTurnSystem.players[0].transform;
            }
        }
    }

    public void ResetPositionToFirstPlayer()
    {
        transform.position = TutorialTurnSystem.players[0].transform.position;
    }

    public void setTarget(Transform target)
    {
        playerTransform = target;
    }
}
