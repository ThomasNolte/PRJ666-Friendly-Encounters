using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostButton : MonoBehaviour {

    GameObject networkContainer;
    MyNetworkManager networkManager;

    void Awake(){
        networkContainer = GameObject.FindGameObjectWithTag("NetworkManager");
        networkManager = networkContainer.GetComponent<MyNetworkManager>();
        GetComponent<Button>().onClick.AddListener(StartHosting);
    }

    void StartHosting() {
        networkManager.StartHosting();
    }
}
