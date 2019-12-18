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
    
    public Button finishButton;
    
    void Awake()
    {
        controller = FindObjectOfType<LobbyController>();
        finishButton.onClick.AddListener(CheckForm);
    }

    void Update()
    {
        passwordField.gameObject.SetActive(passwordToggle.isOn);
    }

    private void CheckForm()
    {
        bool pass = true;
        LobbyInfo info = new LobbyInfo();

        if (!string.IsNullOrEmpty(lobbyName.text))
        {
            info.lobbyName = lobbyName.text;
            if (passwordToggle.isOn)
            {
                if (!string.IsNullOrEmpty(passwordField.text))
                {
                    info.lobbyPassword = passwordField.text;
                }
                else
                {
                    GameObject message = Instantiate(controller.warningMessage, controller.lobbyCreation.transform);
                    message.GetComponentInChildren<Text>().text = "Please enter a password for the lobby";
                    pass = false;
                }

            }
            info.amountOfPlayers = int.Parse(amountOfPlayers.options[amountOfPlayers.value].text);
            info.amountOfRounds = int.Parse(rounds.options[rounds.value].text);
            info.mapName = maps.options[maps.value].text;
        }
        else
        {
            GameObject message = Instantiate(controller.warningMessage, controller.lobbyCreation.transform);
            message.GetComponentInChildren<Text>().text = "Please enter a lobby name";
            pass = false;
        }


        if (pass)
        {
            controller.StartHosting(info);
        }
    }

}
