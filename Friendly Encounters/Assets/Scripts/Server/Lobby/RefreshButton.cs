using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefreshButton : MonoBehaviour {

    MyNetworkManager networkManager;

    void Awake(){
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<MyNetworkManager>();
        GetComponent<Button>().onClick.AddListener(RefreshMatches);
    }

    void RefreshMatches(){
        networkManager.RefreshMatches();
    }
}
