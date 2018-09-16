using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour {

    public const int MENUSTATE = 0;
    public const int LOGINSTATE = 1;
    public const int REGISTERSTATE = 2;
    public const int PLAYSTATE = 3;
    public const int LEVELSTATE = 4;
    public const int MINIGAMESTATE = 5;

    public void PlayButton() {
        SceneManager.LoadScene(PLAYSTATE);
    }

}
