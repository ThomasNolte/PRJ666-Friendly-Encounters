using UnityEngine;
using UnityEngine.UI;

public class LobbyCreationManager : MonoBehaviour {

    private MyGameManager manager;

    public InputField lobbyName;
    public InputField passwordField;

    public Dropdown maps;
    public Dropdown rounds;
    public Dropdown amountOfPlayers;

    public Button backButton;
    public Button finishButton;
    public Button profileButton;

    void Awake()
    {
        manager = FindObjectOfType<MyGameManager>();
        profileButton.gameObject.SetActive(false);
        backButton.onClick.AddListener(BackToGameLobby);
        finishButton.onClick.AddListener(CheckForm);
    }

    private void BackToGameLobby()
    {

    }

    private void CheckForm()
    {

        profileButton.gameObject.SetActive(true);
    }

}
