using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour {

    public static StateManager instance = null;

    public const int MENUSTATE = 0;
    public const int LOGINSTATE = 1;
    public const int REGISTERSTATE = 2;
    public const int PLAYSTATE = 3;
    public const int MINIGAMESTATE = 4;
    public const int LOBBYCREATIONSTATE = 5;
    public const int PUBLICLOBBYSTATE = 6;
    public const int SETTINGSTATE = 7;
    public const int TUTORIALSTATE = 8;
    public const int GAMELOBBYSTATE = 9;

    void Awake() {
        if (instance == null){
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    
    public void MenuButton() { SceneManager.LoadScene(MENUSTATE); }
    public void LoginButton() { SceneManager.LoadScene(LOGINSTATE); }
    public void RegisterButton() { SceneManager.LoadScene(REGISTERSTATE); }
    public void PlayButton() { SceneManager.LoadScene(PLAYSTATE); }
    public void MiniGameButton() { SceneManager.LoadScene(MINIGAMESTATE); }
    public void LobbyCreationButton() { SceneManager.LoadScene(LOBBYCREATIONSTATE); }
    public void PublicLobbyButton() { SceneManager.LoadScene(PUBLICLOBBYSTATE); }
    public void SettingButton() { SceneManager.LoadScene(SETTINGSTATE); }
    public void TutorialButton() { SceneManager.LoadScene(TUTORIALSTATE); }
    public void GameLobbyButton() { SceneManager.LoadScene(GAMELOBBYSTATE); }

    public void ExitApplication() { Application.Quit();}

}
