using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetScoreText : MonoBehaviour {

    [SerializeField]
    private Text _scoreText;

    private TimeCounter timeManager;

    // Use this for initialization
    void Start()
    {
        timeManager = FindObjectOfType<TimeCounter>();
        _scoreText.text = timeManager.DisplayEndTime();
    }
}
