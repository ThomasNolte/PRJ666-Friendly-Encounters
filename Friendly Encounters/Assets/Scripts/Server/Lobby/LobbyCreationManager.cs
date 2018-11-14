using UnityEngine;
using UnityEngine.UI;

public class LobbyCreationManager : MonoBehaviour
{
    private LobbyController controller;

    public InputField lobbyName;
    public InputField passwordField;

    public Toggle passwordToggle;

    public Dropdown maps;
    public Dropdown rounds;
    public Dropdown amountOfPlayers;

    public Button backButton;
    public Button finishButton;
    
    void Awake()
    {
        controller = FindObjectOfType<LobbyController>();
        backButton.onClick.AddListener(BackToGameLobby);
        finishButton.onClick.AddListener(CheckForm);
    }

    void Update()
    {
        passwordField.gameObject.SetActive(passwordToggle.isOn);
    }

    private void BackToGameLobby()
    {
    }

    private void CheckForm()
    {
    }

}
