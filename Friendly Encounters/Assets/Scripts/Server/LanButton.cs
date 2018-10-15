using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanButton : MonoBehaviour {

    MyNetworkManager networkManager;
    MyGameManager states;

    void Awake(){
        states = GameObject.Find("SceneManager").GetComponent<MyGameManager>();
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<MyNetworkManager>();
        GetComponent<Button>().onClick.AddListener(StartHosting);
    }

    void StartHosting(){
        networkManager.StopMatchMaker();
        networkManager.StartHost();
        states.PublicLobbyButton();
    }
}
