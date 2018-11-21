using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyGameManager : MonoBehaviour
{
    public static MyGameManager instance = null;
    public static int lastSceneIndex;
    public static int currentSceneIndex;
    public static bool pause = false;
    private static User user;
    public GameObject loadingCanvas;
    public GameObject pauseMenu;
    
    public Button resumeButton;
    public Button quitButton;

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
        MAZESTATE,
        COINCOLLECTORSTATE,
        SOLODODGEWATERBALLOONSTATE,
        FORGOTPASSWORD,
        RESETPASSWORD,
        SIMONSAYSSTATE //This state doesn't exist yet
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
        SceneManager.activeSceneChanged += OnChangeScene;
        SceneManager.sceneUnloaded += SceneUnLoadedMethod;
        
        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitToMenu);
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && 
            currentSceneIndex != (int)STATES.GAMELOBBYSTATE &&
            currentSceneIndex != (int)STATES.PLAYSTATE &&
            currentSceneIndex != (int)STATES.LOGINSTATE &&
            currentSceneIndex != (int)STATES.FORGOTPASSWORD &&
            currentSceneIndex != (int)STATES.REGISTERSTATE &&
            currentSceneIndex != (int)STATES.RESETPASSWORD &&
            currentSceneIndex != (int)STATES.SETTINGSTATE &&
            currentSceneIndex != (int)STATES.CREDITSTATE &&
            currentSceneIndex != (int)STATES.MINIGAMESTATE &&
            currentSceneIndex != (int)STATES.MENUSTATE &&
            currentSceneIndex != (int)STATES.PROFILESTATE)
        {
            if (lastSceneIndex == (int)STATES.MINIGAMESTATE)
            {
                quitButton.GetComponentInChildren<Text>().text = "MINIGAME MENU";
            }
            else
            {
                quitButton.GetComponentInChildren<Text>().text = "MAIN MENU";
            }
            if (!pause)
            {
                pauseMenu.SetActive(true);
                pause = true;
            }
            else
            {
                pauseMenu.SetActive(false);
                pause = false;
            }
        }
    }

    private void SceneUnLoadedMethod(Scene sceneNumber)
    {
        int sceneIndex = sceneNumber.buildIndex;
        if (lastSceneIndex != sceneIndex)
        {
            lastSceneIndex = sceneIndex;
        }
    }

    private void OnChangeScene(Scene current, Scene next)
    {
        if (loadingCanvas != null)
        {
            loadingCanvas.SetActive(false);
        }

        switch (next.buildIndex)
        {
            case (int)STATES.MENUSTATE:
                LoginButton();
                RegisterButton();
                GuestButton();
                TutorialButton();
                SettingButton();
                CreditButton();
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
                ProfileButton();
                break;
            case (int)STATES.MINIGAMESTATE:
                SimonSaysButton();
                MazeButton();
                CoinCollectorButton();
                SoloDodgeWaterBalloonButton();
                MatchingCardButton();
                ProfileButton();
                break;
            case (int)STATES.SETTINGSTATE:
                MenuButton();
                break;
            case (int)STATES.TUTORIALSTATE:
                break;
            case (int)STATES.GAMELOBBYSTATE:
                break;
            case (int)STATES.CREDITSTATE:
                MenuButton();
                break;
            case (int)STATES.PROFILESTATE:
                MiniGameButton();
                GameLobbyButton();
                SoloPlayButton();
                MenuButton();
                break;
            case (int)STATES.SIMONSAYSSTATE:
                break;
            case (int)STATES.DODGEWATERBALLOONSTATE:
                GameLobbyButton();
                break;
            case (int)STATES.MATCHINGCARDSTATE:
                break;
            case (int)STATES.MAZESTATE:
                break;
            case (int)STATES.COINCOLLECTORSTATE:
                break;
            case (int)STATES.SOLODODGEWATERBALLOONSTATE:
                break;
            case (int)STATES.FORGOTPASSWORD:
                MenuButton();
                break;
            case (int)STATES.RESETPASSWORD:
                MenuButton();
                break;
        }
        currentSceneIndex = next.buildIndex;
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
    public void SoloPlayButton()
    {
        Button btn = GameObject.Find("SoloPlay Button").GetComponent<Button>();
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
    public void SimonSaysButton()
    {
        Button btn = GameObject.Find("MiniGame1 Button").GetComponent<Button>();
        btn.onClick.AddListener(delegate { MyLoadScene((int)STATES.SIMONSAYSSTATE); });
    }
    public void CoinCollectorButton()
    {
        Button btn = GameObject.Find("MiniGame2 Button").GetComponent<Button>();
        btn.onClick.AddListener(delegate { MyLoadScene((int)STATES.COINCOLLECTORSTATE); });
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
    public void SoloDodgeWaterBalloonButton()
    {
        Button btn = GameObject.Find("SoloWaterBalloon Button").GetComponent<Button>();
        btn.onClick.AddListener(delegate { MyLoadScene((int)STATES.SOLODODGEWATERBALLOONSTATE); });
    }
    public void ResetPasswordButton()
    {
        Button btn = GameObject.Find("ResetPassword Button").GetComponent<Button>();
        btn.onClick.AddListener(delegate { MyLoadScene((int)STATES.RESETPASSWORD); });
    }

    public void MyLoadScene(int index)
    {
        loadingCanvas.SetActive(true);

        StartCoroutine("LoadNewScene", index);
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        pause = false;
    }

    public void QuitToMenu()
    {
        pauseMenu.SetActive(false);
        pause = false;
        if (lastSceneIndex == (int)STATES.MINIGAMESTATE)
        {
            MyLoadScene((int)STATES.MINIGAMESTATE);
        }
        else
        {
            quitButton.GetComponentInChildren<Text>().text = "MAIN MENU";
            MyLoadScene((int)STATES.MENUSTATE);
            if (FindObjectOfType<TutorialTurnSystem>() != null)
            {
                FindObjectOfType<TutorialTurnSystem>().Clear();
            }
        }
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
        //yield return new WaitForSeconds(1.0f);
        AsyncOperation async = SceneManager.LoadSceneAsync(index);

        while (!async.isDone)
        {
            yield return null;
        }
    }

}
