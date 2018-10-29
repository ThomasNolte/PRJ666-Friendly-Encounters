using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField]
    private float moveSpeed = 10f;

    private Rigidbody2D rb;
    private Collider2D col;

    private int waypointIndex = 0;
    private bool canControl = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        //We don't want to handle collision on client, so disable collider there
        //col.enabled = isServer;
        col.enabled = false;

        DontDestroyOnLoad(gameObject);

        PlayManager.players.Add(this);
        WaterBalloonSpawner.players.Add(this);
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    [ClientCallback]
    void FixedUpdate()
    {
        if (!isLocalPlayer || !canControl || !PlayManager.IsRunning)
            return;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        rb.AddForce(movement * moveSpeed);

    }

    [ClientCallback]
    void OnTriggerEnter2D(Collider2D c)
    {
        if (isServer)
            return; // Hosting client, Server path will handle collision
        
        NetworkPlayer obj = c.gameObject.GetComponent<NetworkPlayer>();

        if (obj != null)
        {
            LocalDestroy();
        }
    }

    //We can't disable the whole object, as it would impair synchronisation/communication
    //So disabling mean disabling collider & renderer only
    public void EnablePlayer(bool enable)
    {
        GetComponent<Renderer>().enabled = enable;
        col.enabled = isServer && enable;
        canControl = enable;
    }


    public override void OnStartLocalPlayer()
    {
        Camera.main.GetComponent<NetworkCamera>().setTarget(gameObject.transform);
    }

    [Server]
    public void Kill()
    {
        RpcDestroy();
        EnablePlayer(false);
    }

    [ClientRpc]
    void RpcDestroy()
    {
        LocalDestroy();
    }


    //Locally destroy the player on client's machine
    [Client]
    public void LocalDestroy()
    {
        if (!canControl)
            return;
        EnablePlayer(false);
    }


    public int WaypointIndex
    {
        get
        {
            return waypointIndex;
        }
        set
        {
            waypointIndex = value;
        }
    }

    public Collider2D Col
    {
        get {
            return col;
        }
        set {
            col = value;
        }
    }
}
