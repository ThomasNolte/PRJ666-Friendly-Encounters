using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    public Text userText;
    public Text emailText;

    public Button logoutButton;

    void Awake()
    {
        userText.text = "Username: " + MyGameManager.GetUser().Name;
        emailText.text = "Email: " + MyGameManager.GetUser().Email;
        if (MyGameManager.GetUser().Name == "Guest" && MyGameManager.GetUser().Email == "Guest")
        {
            logoutButton.GetComponentInChildren<Text>().text = "MAIN MENU";
        }
    }

}
