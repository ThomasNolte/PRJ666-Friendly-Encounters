using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    public static bool connectedLobby;

    public GameObject gameLobby;
    public GameObject lobbyCreation;
    public GameObject publicLobby;
    public GameObject warningMessage;

    public Button hostButton;
    public Button createButton;
    public Button joinLanButton;
    public Button startButton;
    public Button readyButton;

    MyNetworkManager networkManager;

    void Awake()
    {
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<MyNetworkManager>();
        hostButton.onClick.AddListener(StartHosting);
        createButton.onClick.AddListener(StartLAN);
        joinLanButton.onClick.AddListener(StartClient);
        startButton.onClick.AddListener(StartGame);
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

    void StartBalloonGame()
    {
        if (LobbyManager.players.Count >= 2)
        {
            MyNetworkManager.singleton.ServerChangeScene("DodgeWaterBalloonState");
        }
        else
        {
            GameObject message = Instantiate(warningMessage, publicLobby.transform);
            message.GetComponent<Text>().text = "You need at least two players to start multiplayer";
        }
    }

    void StartGame()
    {
        if (LobbyManager.players.Count >= 2)
        {
            MyNetworkManager.singleton.ServerChangeScene("PlayState");
        }
        else
        {
            GameObject message = Instantiate(warningMessage, publicLobby.transform);
            message.GetComponent<Text>().text = "You need at least two players to start multiplayer";
        }
    }

    public void SetStartButtonActive()
    {
        startButton.gameObject.SetActive(true);
    }

    public void ActivePublicLobby()
    {
        gameLobby.SetActive(false);
        lobbyCreation.SetActive(false);
        publicLobby.SetActive(true);
        connectedLobby = true;
    }
}
