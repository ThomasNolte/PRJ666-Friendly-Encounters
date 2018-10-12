using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour{

    [SerializeField]
    private float moveSpeed = 5f;

    void Update(){
        if (isLocalPlayer){
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(horizontal, 0f, vertical);
            transform.position += movement * Time.deltaTime * moveSpeed;

        }
    }
}
