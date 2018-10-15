using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{

    MyGameManager states;

    public Text userText;
    public Text emailText;

    void Awake()
    {
        states = GameObject.Find("SceneManager").GetComponent<MyGameManager>();
        userText.text = "Username: " + states.GetUser().Name;
        emailText.text = "Email: " + states.GetUser().Email;
    }

}
