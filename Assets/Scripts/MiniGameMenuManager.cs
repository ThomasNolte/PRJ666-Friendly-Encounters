using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class MiniGameMenuManager : MonoBehaviour
{
    public const int NEXTSCORE = 5;

    public static int previousPage = 0;

    public GameObject horizontalScrollSnap;
    public GameObject scoreBoardCanvas;

    public Button showScoreBoardButton;

    void Start()
    {
        horizontalScrollSnap.GetComponent<HorizontalScrollSnap>().StartingScreen = previousPage;
        horizontalScrollSnap.GetComponent<HorizontalScrollSnap>().UpdateLayout();

        showScoreBoardButton.onClick.AddListener(ShowScoreBoard);
    }

    private void ShowScoreBoard()
    {
        if (scoreBoardCanvas.activeInHierarchy)
        {
            scoreBoardCanvas.gameObject.SetActive(false);
        }
        else
        {
            scoreBoardCanvas.gameObject.SetActive(true);
        }
    }

    //Keeping the page consistent
    public void GetSelectedPage(int index)
    {
        previousPage = (index - 1);
    }
}
