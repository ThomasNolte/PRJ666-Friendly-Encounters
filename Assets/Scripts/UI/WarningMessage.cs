using UnityEngine;
using UnityEngine.UI;

public class WarningMessage : MonoBehaviour {

    private Text warningText;

    void Awake() {
        GetComponent<Button>().onClick.AddListener(ButtonClicked);
        warningText = GetComponentsInChildren<Text>()[0];
        warningText.text = "Please enter your warning message here!";
        foreach (Text t in gameObject.GetComponentsInChildren<Text>())
        {
            t.CrossFadeAlpha(0, 1.8f, false);
        }
        Destroy(gameObject, 1.8f);
    }

    void Update()
    {
        GetComponent<Image>().CrossFadeAlpha(0, 1.8f, false);
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
