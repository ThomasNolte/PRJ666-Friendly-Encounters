using UnityEngine;

public class MiniGameMenuManager : MonoBehaviour
{
    public static int previousPage = 0;

    void Start()
    {
        GetComponent<UnityEngine.UI.Extensions.HorizontalScrollSnap>().CurrentPage = previousPage;
        GetComponent<UnityEngine.UI.Extensions.HorizontalScrollSnap>().UpdateLayout();
    }

    public void GetSelectedPage(int index)
    {
        previousPage = (index - 1);
    }
}
