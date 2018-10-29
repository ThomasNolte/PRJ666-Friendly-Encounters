using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyGameManager : MonoBehaviour
{
    public static MyGameManager instance = null;

    private static User user;

    private Text loadingText;

    private bool loadScene = false;

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
        DODGEWATERBALLOONSTATE,
        MATCHINGCARDSTATE,
        MAZESTATE
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

        //Initialize an empty user
        user = new User();
        loadingText = GameObject.Find("Loading text").GetComponent<Text>();
        SceneManager.activeSceneChanged += OnChangeScene;

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (loadScene && loadingText != null)
        {
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));
        }

        //Wire the play button if lobby is connected
        if (LobbyController.connectedLobby)
        {
            PlayButton();
            DodgeWaterBalloonButton();
            LobbyController.connectedLobby = false;
        }
    }

    private void OnChangeScene(Scene current, Scene next)
    {
        loadScene = false;
        loadingText = GameObject.Find("Loading text").GetComponent<Text>();

        switch (next.buildIndex)
        {
            case (int)STATES.MENUSTATE:
                LoginButton();
                RegisterButton();
                GuestButton();
                TutorialButton();
                SettingButton();
                ExitButton();
                break;
            case (int)STATES.LOGINSTATE:
                //Profile button not needed, hard coded in Find User
                RegisterButton();
                MenuButton();
                break;
            case (int)STATES.REGISTERSTATE:
                //Register button not needed, hard coded in Find User
                MenuButton();
                break;
            case (int)STATES.PLAYSTATE:
                MenuButton();
                break;
            case (int)STATES.MINIGAMESTATE:
                MazeButton();
                //DodgeWaterBalloonButton();
                MatchingCardButton();
                MenuButton();
                break;
            case (int)STATES.SETTINGSTATE:
                MenuButton();
                break;
            case (int)STATES.TUTORIALSTATE:
                MenuButton();
                break;
            case (int)STATES.GAMELOBBYSTATE:
                MenuButton();
                break;
            case (int)STATES.CREDITSTATE:
                MenuButton();
                break;
            case (int)STATES.PROFILESTATE:
                MiniGameButton();
                GameLobbyButton();
                MenuButton();
                break;
            case (int)STATES.DODGEWATERBALLOONSTATE:
                MenuButton();
                break;
            case (int)STATES.MATCHINGCARDSTATE:
                MenuButton();
                break;
            case (int)STATES.MAZESTATE:
                MenuButton();
                break;
        }
    }

    public void MenuButton()
    {
        Button btn = GameObject.Find("Main Menu Button").GetComponent<Button>();
        btn.onClick.AddListener(delegate { MyLoadScene((int)STATES.MENUSTATE); });
    }
    public void LoginButton()
    {
        Button btn = GameObject.Find("Login Button").GetComponent<Button>();
        btn.onClick.AddListener(delegate { MyLoadScene((int)STATES.LOGINSTATE); });
    }
    public void RegisterButton()
    {
        Button btn = GameObject.Find("Register Button").GetComponent<Button>();
        btn.onClick.AddListener(delegate { MyLoadScene((int)STATES.REGISTERSTATE); });
    }
    public void PlayButton()
    {
        Button btn = GameObject.Find("Play Button").GetComponent<Button>();
        btn.onClick.AddListener(delegate { MyLoadScene((int)STATES.PLAYSTATE); });
    }
    public void GuestButton()
    {
        Button btn = GameObject.Find("Guest Button").GetComponent<Button>();
        btn.onClick.AddListener(delegate { MyLoadScene((int)STATES.PROFILESTATE); });
    }
    public void MiniGameButton()
    {
        Button btn = GameObject.Find("MiniGame Button").GetComponent<Button>();
        btn.onClick.AddListener(delegate { MyLoadScene((int)STATES.MINIGAMESTATE); });
    }
    public void SettingButton()
    {
        Button btn = GameObject.Find("Setting Button").GetComponent<Button>();
        btn.onClick.AddListener(delegate { MyLoadScene((int)STATES.SETTINGSTATE); });
    }
    public void TutorialButton()
    {
        Button btn = GameObject.Find("Tutorial Button").GetComponent<Button>();
        btn.onClick.AddListener(delegate { MyLoadScene((int)STATES.TUTORIALSTATE); });
    }
    public void GameLobbyButton()
    {
        Button btn = GameObject.Find("GameLobby Button").GetComponent<Button>();
        btn.onClick.AddListener(delegate { MyLoadScene((int)STATES.GAMELOBBYSTATE); });
    }
    public void CreditButton()
    {
        Button btn = GameObject.Find("Credit Button").GetComponent<Button>();
        btn.onClick.AddListener(delegate { MyLoadScene((int)STATES.CREDITSTATE); });
    }
    public void ProfileButton()
    {
        Button btn = GameObject.Find("Profile Button").GetComponent<Button>();
        btn.onClick.AddListener(delegate { MyLoadScene((int)STATES.PROFILESTATE); });
    }
    public void DodgeWaterBalloonButton()
    {
        Button btn = GameObject.Find("MiniGame3 Button").GetComponent<Button>();
        btn.onClick.AddListener(delegate { MyLoadScene((int)STATES.DODGEWATERBALLOONSTATE); });
    }
    public void MatchingCardButton()
    {
        Button btn = GameObject.Find("MiniGame4 Button").GetComponent<Button>();
        btn.onClick.AddListener(delegate { MyLoadScene((int)STATES.MATCHINGCARDSTATE); });
    }
    public void MazeButton()
    {
        Button btn = GameObject.Find("MiniGame5 Button").GetComponent<Button>();
        btn.onClick.AddListener(delegate { MyLoadScene((int)STATES.MAZESTATE); });
    }


    public void MyLoadScene(int index)
    {
        loadScene = true;
        loadingText.text = "Loading...";
        StartCoroutine("LoadNewScene", index);
    }

    public void ExitButton()
    {
        Button btn = GameObject.Find("Exit Button").GetComponent<Button>();
        btn.onClick.AddListener(delegate { Application.Quit(); });
    }

    public static User GetUser() { return user; }
    public static void SetUser(User u) { user = u; }

    IEnumerator LoadNewScene(int index)
    {
        //yield return new WaitForSeconds(0.1f);
        AsyncOperation async = SceneManager.LoadSceneAsync(index);

        while (!async.isDone)
        {
            yield return null;
        }
    }

}
