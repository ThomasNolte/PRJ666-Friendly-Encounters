using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetScoreText : MonoBehaviour {

    [SerializeField]
    private Text _scoreText;

    private ScoreManager _scoreManager;

    // Use this for initialization
    void Start()
    {
        _scoreManager = FindObjectOfType<ScoreManager>();
        _scoreText.text = "Score: " + _scoreManager.score;
    }
}
