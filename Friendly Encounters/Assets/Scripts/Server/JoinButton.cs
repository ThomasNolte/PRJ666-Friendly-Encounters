using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class JoinButton : MonoBehaviour
{
    private GameObject gameLobby;
    private GameObject lobbyCreation;
    private GameObject publicLobby;

    private Text buttonText;
    private MatchInfoSnapshot match;

    private void Awake()
    {
        buttonText = GetComponentInChildren<Text>();
        GetComponent<Button>().onClick.AddListener(JoinMatch);

        gameLobby = GameObject.Find("GameLobby");
        lobbyCreation = GameObject.Find("CreationLobby");
        publicLobby = GameObject.Find("PublicLobby");
    }

    public void Initialize(MatchInfoSnapshot match, Transform panelTransform)
    {
        this.match = match;
        buttonText.text = match.name;
        transform.SetParent(panelTransform);
        transform.localScale = Vector2.one;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector2.zero;
    }

    private void JoinMatch()
    {
        FindObjectOfType<MyNetworkManager>().JoinMatch(match);
        gameLobby.SetActive(false);
        lobbyCreation.SetActive(false);
        publicLobby.SetActive(true);
    }
}
