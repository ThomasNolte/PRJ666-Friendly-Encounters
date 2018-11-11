using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMiniGameManager : MonoBehaviour
{
    public static bool IsMiniGameFinished = false;
    public const int MAXSTATES = 5;

    public GameObject[] images;

    public GameObject simonsaysContainer;
    public GameObject coinCollectorContainer;
    public GameObject dodgeWaterBalloonContainer;
    public GameObject matchingCardsContainer;
    public GameObject mazeContainer;

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
            dodgeWaterBalloonContainer.SetActive(true);
        }
        //if (miniGameSelected != -1)
        //{
        //    switch (miniGameSelected)
        //    {
        //        case (int)MiniGameState.SIMONSAYS:
        //            manager.MyLoadScene((int)MyGameManager.STATES.MENUSTATE);
        //            break;
        //        case (int)MiniGameState.COINCOLLECTOR:
        //            manager.MyLoadScene((int)MyGameManager.STATES.COINCOLLECTORSTATE);
        //            break;
        //        case (int)MiniGameState.DODGEWATERBALLOON:
        //            manager.MyLoadScene((int)MyGameManager.STATES.SOLODODGEWATERBALLOONSTATE);
        //            break;
        //        case (int)MiniGameState.MATCHINGCARDS:
        //            manager.MyLoadScene((int)MyGameManager.STATES.MATCHINGCARDSTATE);
        //            break;
        //        case (int)MiniGameState.MAZE:
        //            manager.MyLoadScene((int)MyGameManager.STATES.MAZESTATE);
        //            break;
        //    }
        //    miniGameSelected = -1;
        //}

        if (IsMiniGameFinished)
        {
            IsMiniGameFinished = false;

        }
    }

    public void RollGame()
    {
        StartCoroutine("RollMiniGame");
    }

    public IEnumerator RollMiniGame()
    {
        int index = -1;
        Color prevColor = Color.white;
        for (int i = 0; i < 20; i++)
        {
            index = Random.Range(0, MAXSTATES);
            prevColor = images[index].GetComponentInChildren<Image>().color;
            images[index].GetComponentInChildren<Image>().color = new Color(255, 0, 0, 50);
            yield return new WaitForSeconds(0.10f);
            images[index].GetComponentInChildren<Image>().color = prevColor;
        }
        images[index].GetComponentInChildren<Image>().color = new Color(255, 0, 0, 50);

        //Wait and let the player see what was selected
        yield return new WaitForSeconds(2f);
        miniGameSelected = index;
        playManager.miniGamePanel.SetActive(false);
        images[index].GetComponentInChildren<Image>().color = prevColor;
    }
}
