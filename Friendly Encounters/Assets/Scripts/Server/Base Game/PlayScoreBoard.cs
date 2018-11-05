using UnityEngine;

public class PlayScoreBoard : MonoBehaviour
{
    public static bool IsPlayState = false;
    void Awake()
    {
        if (!FindObjectOfType<PlayManager>().enabled)
        {
            FindObjectOfType<PlayManager>().enabled = true;
        }
        IsPlayState = true;
    }
}
