using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
 
    [SerializeField]
    private List<GameObject> _cardsToSpawn;
    [SerializeField]
    private List<int> _cardsToSpawnCount;

    [SerializeField]
    private Transform _startPoint;

    [SerializeField]
    int _rows;

    [SerializeField]
    int _columns;

    [SerializeField]
    float _xDistance;

    [SerializeField]
    float _yDistance;

    private MatchingCardManager _gameManager;

    // Use this for initialization
    void Start()
    {
        _gameManager = FindObjectOfType<MatchingCardManager>();
    }

    private void Update() //Objects are spawned in Update, so script execution order won't affect this scrip
    {
        Spawn();
        gameObject.SetActive(false); //After spawning objects deactivate this gameObject
    }

    void Spawn()
    {
        int cardNumber;
        int maxCards = _rows * _columns;

        if (maxCards % 2 == 0)
        {
            _cardsToSpawnCount = new List<int>();

            _gameManager.cardsLeft = maxCards;

            for (int i = 0; i < _cardsToSpawn.Count; i++)
            {
                _cardsToSpawnCount.Add(0);
            }

            for (int i = 0; i < _cardsToSpawnCount.Count; i++)
            {
                _cardsToSpawnCount[i] = Random.Range(0, (int)(maxCards / (_cardsToSpawn.Count - 1)) + 1);
                if (_cardsToSpawnCount[i] % 2 != 0)
                {
                    if (Random.Range(0, 100) < 50)
                    {
                        _cardsToSpawnCount[i]--;
                    }
                    else
                    {
                        _cardsToSpawnCount[i]++;
                    }
                }
            }

            int sum = 0;

            for (int i = 0; i < _cardsToSpawnCount.Count; i++)
            {
                sum += _cardsToSpawnCount[i];
            }
            if (sum < maxCards)
            {
                for (int i = 0; i < (int)((maxCards - sum) / 2); i++)
                {
                    _cardsToSpawnCount[Random.Range(0, _cardsToSpawn.Count - 1)] += 2; //Because it must be even
                }
            }
            bool forward = true;
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    cardNumber = Random.Range(0, _cardsToSpawnCount.Count - 1);
                    if (_cardsToSpawnCount[cardNumber] > 0)
                    {
                        Instantiate(_cardsToSpawn[cardNumber], new Vector3(_startPoint.position.x + _xDistance * j, _startPoint.position.y - _yDistance * i, _startPoint.position.z), Quaternion.Euler(0.0f, 0.0f, 0.0f));
                        _cardsToSpawnCount[cardNumber]--;
                    }
                    else
                    {
                        if (forward)
                        {
                            for (int k = 0; k < _cardsToSpawnCount.Count; k++)
                            {
                                if (_cardsToSpawnCount[k] > 0)
                                {
                                    Instantiate(_cardsToSpawn[k], new Vector3(_startPoint.position.x + _xDistance * j, _startPoint.position.y - _yDistance * i, _startPoint.position.z), Quaternion.Euler(0.0f, 0.0f, 0.0f));
                                    _cardsToSpawnCount[k]--;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int k = _cardsToSpawnCount.Count - 1; k >= 0; k--)
                            {
                                if (_cardsToSpawnCount[k] > 0)
                                {
                                    Instantiate(_cardsToSpawn[k], new Vector3(_startPoint.position.x + _xDistance * j, _startPoint.position.y - _yDistance * i, _startPoint.position.z), Quaternion.Euler(0.0f, 0.0f, 0.0f));
                                    _cardsToSpawnCount[k]--;
                                    break;
                                }
                            }
                        }
                        forward = !forward;
                    }
                }
            }
        }
        else
        {
            Debug.Log("Number of cards must be even!");
        }
    }
}