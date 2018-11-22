using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    public const int LOCAL = 0;
    public const int ONLINE = 1;

    private int currentLobby = -1;
    private int hostType = -1;

    public GameObject gameLobby;
    public GameObject lobbyCreation;
    public GameObject publicLobby;
    public GameObject warningMessage;

    public Button hostButton;
    public Button createLanButton;
    public Button joinLanButton;
    public Button dodgeWaterBalloonButton;
    public Button startButton;
    public Button readyButton;
    public Button quitButton;

    MyGameManager manager;
    MyNetworkManager networkManager;
    LobbyUI lobbyUI;

    public enum LobbyIndex
    {
        GAMELOBBY,
        CREATIONLOBBY,
        PUBLICLOBBY
    }

    void Awake()
    {
        manager = FindObjectOfType<MyGameManager>();
        networkManager = FindObjectOfType<MyNetworkManager>();
        lobbyUI = FindObjectOfType<LobbyUI>();
        hostButton.onClick.AddListener(StartHostingCreation);
        createLanButton.onClick.AddListener(StartLANCreation);
        joinLanButton.onClick.AddListener(StartClient);
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(LastState);
        dodgeWaterBalloonButton.onClick.AddListener(StartBalloonGame);
        ActiveGameLobby();
    }

    void StartLANCreation()
    {
        hostType = LOCAL;
        StartLAN();
        ActivePublicLobby();
    }
    void StartHostingCreation()
    {
        hostType = ONLINE;
        ActiveLobbyCreation();
    }

    public void StartClient()
    {
        networkManager.StartClient();
    }

    public void StartLAN()
    {
        networkManager.StopMatchMaker();
        networkManager.StartHost();

        if (networkManager.IsClientConnected() && !ClientScene.ready)
        {
            ClientScene.Ready(networkManager.client.connection);
            if (ClientScene.localPlayers.Count == 0)
                ClientScene.AddPlayer((short)0);
        }
    }
    public void StartHosting(LobbyInfo info)
    {
        lobbyUI.SetLobbyInfo(info);
        ActivePublicLobby();
        networkManager.StartHosting(info.lobbyName, (uint)info.amountOfPlayers, info.lobbyPassword);
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
            message.GetComponentInChildren<Text>().text = "You need at least two players to start multiplayer";
        }
    }

    private void StartGame()
    {
        if (LobbyManager.players.Count >= 2)
        {
            MyNetworkManager.singleton.ServerChangeScene("PlayState");
        }
        else
        {
            GameObject message = Instantiate(warningMessage, publicLobby.transform);
            message.GetComponentInChildren<Text>().text = "You need at least two players to start multiplayer";
        }
    }

    private void LastState()
    {
        switch (currentLobby)
        {
            case (int)LobbyIndex.GAMELOBBY:
                manager.MyLoadScene((int)MyGameManager.STATES.PROFILESTATE);
                break;
            case (int)LobbyIndex.CREATIONLOBBY:
                ActiveGameLobby();
                break;
            case (int)LobbyIndex.PUBLICLOBBY:
                networkManager.StopHost();
                if (hostType == LOCAL)
                {
                    networkManager.StopClient();
                }
                ActiveGameLobby();
                break;
        }
    }

    public void SetHostButtons()
    {
        startButton.gameObject.SetActive(true);
        dodgeWaterBalloonButton.gameObject.SetActive(true);
    }

    public void ActiveGameLobby()
    {
        currentLobby = (int)LobbyIndex.GAMELOBBY;
        quitButton.GetComponentInChildren<Text>().text = "BACK TO PROFILE";
        gameLobby.SetActive(true);
        lobbyCreation.SetActive(false);
        publicLobby.SetActive(false);
    }

    public void ActivePublicLobby()
    {
        currentLobby = (int)LobbyIndex.PUBLICLOBBY;
        quitButton.GetComponentInChildren<Text>().text = "QUIT";
        gameLobby.SetActive(false);
        lobbyCreation.SetActive(false);
        publicLobby.SetActive(true);
    }

    public void ActiveLobbyCreation()
    {
        currentLobby = (int)LobbyIndex.CREATIONLOBBY;
        quitButton.GetComponentInChildren<Text>().text = "BACK";
        gameLobby.SetActive(false);
        lobbyCreation.SetActive(true);
        publicLobby.SetActive(false);
    }

    public int CurrentLobby
    {
        get
        {
            return currentLobby;
        }
        set
        {
            currentLobby = value;
        }
    }

    public int HostType
    {
        get
        {
            return hostType;
        }
        set
        {
            hostType = value;
        }
    }
}
