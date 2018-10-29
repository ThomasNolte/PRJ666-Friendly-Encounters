using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    public static bool connectedLobby;

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
        ActivePublicLobby();
        networkManager.StartClient();
    }

    void StartLAN()
    {
        ActivePublicLobby();
        networkManager.StopMatchMaker();
        networkManager.StartHost();

        if (networkManager.IsClientConnected() && !ClientScene.ready)
        {
            ClientScene.Ready(networkManager.client.connection);
            if (ClientScene.localPlayers.Count == 0)
                ClientScene.AddPlayer((short)0);
        }
    }
    void StartHosting()
    {
        ActivePublicLobby();
        networkManager.StartHosting();
    }

    public void ActivePublicLobby()
    {
        gameLobby.SetActive(false);
        lobbyCreation.SetActive(false);
        publicLobby.SetActive(true);
        connectedLobby = true;
    }

}
