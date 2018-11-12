using UnityEngine;
using UnityEngine.UI;

public class WarningMessage : MonoBehaviour {

    private Text warningText;

    void Awake() {
        GetComponent<Button>().onClick.AddListener(ButtonClicked);
        warningText = GetComponentInChildren<Text>();
        warningText.text = "test one two three";

        Destroy(gameObject, 2f);
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
