using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMiniGameManager : MonoBehaviour
{
    public GameObject[] images;

    public const int MAXSTATES = 5;
    private TutorialTurnSystem playManager;
    private MyGameManager manager;
    private int miniGameSelected = -1;

    void Awake()
    {
        playManager = FindObjectOfType<TutorialTurnSystem>();
        manager = FindObjectOfType<MyGameManager>();
    }

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
        if (miniGameSelected != -1)
        {
            switch (miniGameSelected)
            {
                case (int)MiniGameState.SIMONSAYS:
                    manager.MyLoadScene((int)MyGameManager.STATES.MENUSTATE);
                    break;
                case (int)MiniGameState.COINCOLLECTOR:
                    manager.MyLoadScene((int)MyGameManager.STATES.COINCOLLECTORSTATE);
                    break;
                case (int)MiniGameState.DODGEWATERBALLOON:
                    manager.MyLoadScene((int)MyGameManager.STATES.SOLODODGEWATERBALLOONSTATE);
                    break;
                case (int)MiniGameState.MATCHINGCARDS:
                    manager.MyLoadScene((int)MyGameManager.STATES.MATCHINGCARDSTATE);
                    break;
                case (int)MiniGameState.MAZE:
                    manager.MyLoadScene((int)MyGameManager.STATES.MAZESTATE);
                    break;
            }
            miniGameSelected = -1;
        }
    }

    public void RollGame()
    {
        StartCoroutine("RollMiniGame");
    }

    public IEnumerator RollMiniGame()
    {
        int index = -1;
        for (int i = 0; i < 20; i++)
        {
            index = Random.Range(0, MAXSTATES);
            Color prevColor = images[index].GetComponentInChildren<Image>().color;
            images[index].GetComponentInChildren<Image>().color = new Color(255, 0, 0, 50);
            yield return new WaitForSeconds(0.05f);
            images[index].GetComponentInChildren<Image>().color = prevColor;
        }
        images[index].GetComponentInChildren<Image>().color = new Color(255, 0, 0, 50);

        //Wait and let the player see what was selected
        yield return new WaitForSeconds(1.5f);
        miniGameSelected = index;
        playManager.miniGamePanel.SetActive(false);
    }
}
