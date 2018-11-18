using UnityEngine;
using UnityEngine.UI;

public class ToolTipToggle : MonoBehaviour
{
    void Start()
    {
        GetComponent<Toggle>().isOn = SettingManager.toolTipOn;
    }
}