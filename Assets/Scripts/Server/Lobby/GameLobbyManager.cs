using UnityEngine;
using UnityEngine.UI;

public class GameLobbyManager : MonoBehaviour
{
    [SerializeField]
    public Button joinLanButton;

    MyNetworkManager networkManager;

    void Awake()
    {
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<MyNetworkManager>();
        joinLanButton.onClick.AddListener(StartClient);
    }

    void StartClient()
    {
        networkManager.StartClient();
    }
}
