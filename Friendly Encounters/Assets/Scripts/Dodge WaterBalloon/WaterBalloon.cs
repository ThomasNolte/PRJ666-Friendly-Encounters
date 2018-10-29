using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WaterBalloon : NetworkBehaviour {

    public float lifeTime = 5f;
    public float moveSpeed = 0.1f;

    Rigidbody2D rb;
    NetworkTransform networkTransform;

    void Start() {
        networkTransform = GetComponent<NetworkTransform>();
        rb = GetComponent<Rigidbody2D>();
        Vector3 pos = transform.position;
        pos.y -= moveSpeed;

        rb.AddForce(pos);
        Destroy(gameObject, lifeTime);
    }

    [ServerCallback]
    void OnTriggerEnter2D(Collider2D collision)
    {
        //we collide so we dirty the NetworkTrasnform to sync it on clients.
        networkTransform.SetDirtyBit(1);

        if (collision.gameObject.tag == "Player")
        {
            NetworkPlayer p = collision.gameObject.GetComponent<NetworkPlayer>();
            p.Kill();
        }
    }
}
