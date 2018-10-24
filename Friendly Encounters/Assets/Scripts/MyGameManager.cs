using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyGameManager : MonoBehaviour
{
    public static MyGameManager instance = null;

    private static User user;

    public enum STATES
    {
        MENUSTATE,
        LOGINSTATE,
        REGISTERSTATE,
        PLAYSTATE,
        MINIGAMESTATE,
        SETTINGSTATE,
        TUTORIALSTATE,
        GAMELOBBYSTATE,
        CREDITSTATE,
        PROFILESTATE,
        DODGEWATERBALLOONSTATE
    }

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

        //Initialize an empty user
        user = new User();
    }

    public void MenuButton() { SceneManager.LoadScene((int)STATES.MENUSTATE); }
    public void LoginButton() { SceneManager.LoadScene((int)STATES.LOGINSTATE); }
    public void RegisterButton() { SceneManager.LoadScene((int)STATES.REGISTERSTATE); }
    public void PlayButton() { SceneManager.LoadScene((int)STATES.PLAYSTATE); }
    public void GuestButton(){ SceneManager.LoadScene((int)STATES.PROFILESTATE); }
    public void MiniGameButton() { SceneManager.LoadScene((int)STATES.MINIGAMESTATE); }
    public void SettingButton() { SceneManager.LoadScene((int)STATES.SETTINGSTATE); }
    public void TutorialButton() { SceneManager.LoadScene((int)STATES.TUTORIALSTATE); }
    public void GameLobbyButton() { SceneManager.LoadScene((int)STATES.GAMELOBBYSTATE); }
    public void CreditButton() { SceneManager.LoadScene((int)STATES.CREDITSTATE); }
    public void ProfileButton() { SceneManager.LoadScene((int)STATES.PROFILESTATE); }
    public void DodgeWaterBalloonButton() { SceneManager.LoadScene((int)STATES.DODGEWATERBALLOONSTATE); }

    public void ExitApplication() { Application.Quit(); }

    public static User GetUser() { return user; }
    public static void SetUser(User u) { user = u; }

}
