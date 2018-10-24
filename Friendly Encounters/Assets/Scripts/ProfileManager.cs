using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    public Text userText;
    public Text emailText;

    void Awake()
    {
        userText.text = "Username: " + MyGameManager.GetUser().Name;
        emailText.text = "Email: " + MyGameManager.GetUser().Email;
    }

}
