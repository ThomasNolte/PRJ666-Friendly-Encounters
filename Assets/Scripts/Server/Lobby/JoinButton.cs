using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class JoinButton : MonoBehaviour
{
    private LobbyController manager;

    public Text lobbyNameText;
    public Text amountOfPlayersText;
    public Text passwordText;
    private MatchInfoSnapshot match;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(JoinMatch);

        manager = FindObjectOfType<LobbyController>();
    }

    public void Initialize(MatchInfoSnapshot match, Transform panelTransform)
    {
        this.match = match;
        lobbyNameText.text = match.name;
        amountOfPlayersText.text = match.currentSize + "/" + match.maxSize;
        passwordText.text = (match.isPrivate)?"YES":"NO";
        transform.SetParent(panelTransform);
        transform.localScale = Vector2.one;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector2.zero;
    }

    private void JoinMatch()
    {
        MyNetworkManager networkManager = FindObjectOfType<MyNetworkManager>();
        networkManager.JoinMatch(match);
        if (networkManager.IsClientConnected() && !ClientScene.ready)
        {
            ClientScene.Ready(networkManager.client.connection);
            if (ClientScene.localPlayers.Count == 0)
                ClientScene.AddPlayer((short)0);
        }
        manager.ActivePublicLobby();
    }
}
