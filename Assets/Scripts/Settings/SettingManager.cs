using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour {
    
    public static SettingManager instance = null;
    public static bool toolTipOn = true;
    
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

    void Update()
    {
        if (MyGameManager.currentSceneIndex == (int)MyGameManager.STATES.SETTINGSTATE)
        {
            toolTipOn = GameObject.Find("ToolTip Toggle").GetComponent<Toggle>().isOn;
        }
    }
}
