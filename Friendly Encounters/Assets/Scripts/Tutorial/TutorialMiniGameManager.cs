using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMiniGameManager : MonoBehaviour
{
    public Sprite[] images;

    public const int MAXSTATES = 5;

    public enum MiniGameState
    {
        SIMONSAYS,
        COINCOLLECTOR,
        DODGEWATERBALLOON,
        MATCHINGCARDS,
        MAZE
    }

    public IEnumerator RollMiniGame()
    {
        int randomDiceSide = 0;
        float seconds = 5f;
        while(seconds > 0)
        {
            randomDiceSide = Random.Range(0, MAXSTATES);
            GetComponent<Image>().sprite = images[0];
            yield return new WaitForSeconds(0.05f + seconds);
        }
    }
}
