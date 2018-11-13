using UnityEngine;
using UnityEngine.UI;

public class WarningMessage : MonoBehaviour {

    private Text warningText;

    void Awake() {
        GetComponent<Button>().onClick.AddListener(ButtonClicked);
        warningText = GetComponentInChildren<Text>();
        warningText.text = "Please enter your warning message here!";
    }

    void Start()
    {
        GetComponent<Button>().image.CrossFadeAlpha(0, 2.5f, false);
        foreach (Text t in gameObject.GetComponentsInChildren<Text>())
        {
            t.CrossFadeAlpha(0, 2.5f, false);
        }
        Destroy(gameObject, 2.5f);
    }

    public void ButtonClicked()
    {
        Destroy(gameObject);
    }

    public void SetWarningText(string message)
    {
        warningText.text = message;
    }
}
