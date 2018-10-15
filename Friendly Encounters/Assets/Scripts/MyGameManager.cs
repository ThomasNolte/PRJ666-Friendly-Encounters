using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyGameManager : MonoBehaviour
{

    public static MyGameManager instance = null;

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
    public const int CREDITSTATE = 10;
    public const int PROFILESTATE = 11;
    public const int DODGEWATERBALLOONSTATE = 12;

    private User user;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
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
    public void CreditButton() { SceneManager.LoadScene(CREDITSTATE); }
    public void ProfileButton() { SceneManager.LoadScene(PROFILESTATE); }
    public void DodgeWaterBalloonButton() { SceneManager.LoadScene(DODGEWATERBALLOONSTATE); }

    public void ExitApplication() { Application.Quit(); }

    public User GetUser() { return user; }
    public void SetUser(User u) { user = u; }

}
