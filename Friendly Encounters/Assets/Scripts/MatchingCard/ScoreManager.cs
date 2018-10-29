using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    private int _score = 0;

    [SerializeField]
    private int _initialTimeScoreBonus = 100000;
    [SerializeField]
    private int _bonusScorePerSecondLost = 80;
    [SerializeField]
    private int _scorePerCard = 50;

    private TimeCounter _timeCounter;

    public int score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
        }
    }

    void Start()
    {
        _timeCounter = FindObjectOfType<TimeCounter>();
    }

    public void AddScore()
    {
        _score += _scorePerCard;
    }

    public void CalculateEndScore()
    {
        _score += Mathf.Clamp(_initialTimeScoreBonus - _bonusScorePerSecondLost* _timeCounter.timeCounted, 0, _initialTimeScoreBonus);
    }
}
