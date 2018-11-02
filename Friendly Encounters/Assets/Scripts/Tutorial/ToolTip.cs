using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    public string[] tips = {
        "Tip1",
        "Tip2",
        "Tip3",
        "Tip4",
        ""
    };

    private int tipIndex = 0;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(NextTip);
        GetComponentInChildren<Text>().text = tips[tipIndex];
    }

    private void NextTip()
    {
        if (tipIndex == tips.Length - 1)
        {
            Destroy(gameObject);
        }
        else {
            tipIndex++;
            GetComponentInChildren<Text>().text = tips[tipIndex];
        }
    }
}
