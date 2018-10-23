using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour {

    public GameObject gameLobby;
    public GameObject lobbyCreation;
    public GameObject publicLobby;

    public Button hostButton;
    public Button createButton;
    public Button joinLanButton;

    MyNetworkManager networkManager;

    void Awake()
    {
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<MyNetworkManager>();
        hostButton.onClick.AddListener(StartHosting);
        createButton.onClick.AddListener(StartLAN);
        joinLanButton.onClick.AddListener(StartClient);
        gameLobby.SetActive(true);
        lobbyCreation.SetActive(false);
        publicLobby.SetActive(false);
    }

    void StartClient()
    {
        gameLobby.SetActive(false);
        lobbyCreation.SetActive(false);
        publicLobby.SetActive(true);
        networkManager.StartClient();
        ClientScene.Ready(networkManager.client.connection);
        if (ClientScene.localPlayers.Count == 0)
            ClientScene.AddPlayer((short)0);
    }

    void StartLAN()
    {
        gameLobby.SetActive(false);
        lobbyCreation.SetActive(false);
        publicLobby.SetActive(true);
        networkManager.StopMatchMaker();
        networkManager.StartHost();
    }

    void StartHosting()
    {
        gameLobby.SetActive(false);
        lobbyCreation.SetActive(false);
        publicLobby.SetActive(true);
        networkManager.StartHosting();
    }

}
