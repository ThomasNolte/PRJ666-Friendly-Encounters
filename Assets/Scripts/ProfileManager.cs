using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    public Text userText;
    public Text emailText;
    public Text gamesPlayedText;
    public Text minigamesPlayedText;
    public Text winsText;
    public Text loseText;
    public Text rankText;

    public Button logoutButton;

    void Awake()
    {
/*        userText.text = "Username: " + MyGameManager.user.Name;
        emailText.text = "Email: " + MyGameManager.user.Email;
        if (MyGameManager.user.Name == "Guest" && MyGameManager.user.Email == "Guest")
        {
            logoutButton.GetComponentInChildren<Text>().text = "MAIN MENU";
            gamesPlayedText.gameObject.SetActive(false);
            minigamesPlayedText.gameObject.SetActive(false);
            winsText.gameObject.SetActive(false);
            loseText.gameObject.SetActive(false);
            rankText.gameObject.SetActive(false);
        }*/
    }

}
