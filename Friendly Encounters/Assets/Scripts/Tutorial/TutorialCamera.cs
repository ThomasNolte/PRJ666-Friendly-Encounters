using UnityEngine;

public class TutorialCamera : MonoBehaviour
{
    public static TutorialCamera instance = null;
    
    public Transform[] topLefts;
    public Transform[] bottomRights;
    
    private Vector3 offset;

    private float mouseSensitivity = 0.011f;
    private Vector3 lastPosition;
    private Vector3 extents;
    private bool reachDestination;

    private float cameraSizeOffsetX;
    private float cameraSizeOffsetY;

    private TutorialTurnSystem manager;
    private TutorialMiniGameManager tutorialManager;
    private Transform playerTransform;

    void Awake()
    {
        tutorialManager = FindObjectOfType<TutorialMiniGameManager>();
        manager = FindObjectOfType<TutorialTurnSystem>();

        //Getting the fixed bounds for the panning camera
        extents = CameraExtension.OrthographicBounds(Camera.main).extents;

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
                    Camera.main.transform.Translate(-(delta.x * mouseSensitivity), -(delta.y * mouseSensitivity), 0);
                    FixedBounds(topLefts[manager.CurrentMapIndex], bottomRights[manager.CurrentMapIndex]);
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
                    reachDestination = true;
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

    public void FixedBounds(Transform topLeft, Transform bottomRight)
    {
        //Fixed bounds for the translate function
        //If we weren't using the translate function we could clamp the coordinates
        //Too far right
        if ((Camera.main.transform.position.x + extents.x) > bottomRight.position.x)
        {
            Camera.main.transform.position = new Vector3(bottomRight.position.x - extents.x, Camera.main.transform.position.y, -10);
        }
        //Too far Left
        if ((Camera.main.transform.position.x - extents.x) < topLeft.position.x)
        {
            Camera.main.transform.position = new Vector3(topLeft.position.x + extents.x, Camera.main.transform.position.y, -10);
        }
        //Too far Up
        if ((Camera.main.transform.position.y + extents.y) > topLeft.position.y)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, topLeft.position.y - extents.y, -10);
        }
        //Too far Down
        if ((Camera.main.transform.position.y - extents.y) < bottomRight.position.y)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, bottomRight.position.y + extents.y, -10);
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

    public bool ReachDestination
    {
        get
        {
            return reachDestination;
        }

        set
        {
            reachDestination = value;
        }
    }
}
