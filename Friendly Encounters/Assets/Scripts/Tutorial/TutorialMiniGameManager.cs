using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMiniGameManager : MonoBehaviour
{
    public GameObject[] images;

    public const int MAXSTATES = 5;
    private int miniGameSelected = -1;

    public enum MiniGameState
    {
        SIMONSAYS,
        COINCOLLECTOR,
        DODGEWATERBALLOON,
        MATCHINGCARDS,
        MAZE
    }

    private void Update()
    {
        
    }

    public void RollGame()
    {
        StartCoroutine("RollMiniGame");
    }

    public IEnumerator RollMiniGame()
    {
        int index = -1;
        for (int i = 0; i < 15; i++)
        {
            index = Random.Range(0, MAXSTATES);
            Color prevColor = images[index].GetComponent<Image>().color;
            images[index].GetComponent<Image>().color = Color.black;
            yield return new WaitForSeconds(0.25f);
            images[index].GetComponent<Image>().color = prevColor;
        }
        images[index].GetComponent<Image>().color = Color.black;
        miniGameSelected = index;
    }
}
