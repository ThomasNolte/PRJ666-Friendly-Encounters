using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSpawner : MonoBehaviour
{
    public Button Easy;
    public Button Medium;
    public Button Hard;

    public GameObject DifficultyMenu;

    [SerializeField]
    private List<GameObject> _cardsToSpawn;

    [SerializeField]
    private List<int> _cardsToSpawnCount;

    [SerializeField]
    private Transform _startPoint;

    [SerializeField]
    float _xDistance;

    [SerializeField]
    float _yDistance;

    private MatchingCardManager _gameManager;
    private TutorialMiniGameManager miniGameManager;

    // Use this for initialization
    void Start()
    {
        _gameManager = FindObjectOfType<MatchingCardManager>();
        MyGameManager.pause = true;
        miniGameManager = FindObjectOfType<TutorialMiniGameManager>();
        if (miniGameManager != null)
        {
            Spawn(2);
        }
        else
        {
            Easy.onClick.AddListener(delegate { Spawn(1); });
            Medium.onClick.AddListener(delegate { Spawn(2); });
            Hard.onClick.AddListener(delegate { Spawn(3); });
        }
    }

    private void Update() //Objects are spawned in Update, so script execution order won't affect this script
    {
        gameObject.SetActive(false); //After spawning objects deactivate this gameObject
    }

    private List<int> RandomList(int size)
    {
        List<int> listNumbers = new List<int>();
        int number;
        for (int i = 0; i < size; i++)
        {
            do
            {
                number = Random.Range(0, size);
            } while (listNumbers.Contains(number));
            listNumbers.Add(number);
        }

        return listNumbers;
    }

    public void Spawn(int difficulty)
    {
        if (DifficultyMenu != null)
        {
            DifficultyMenu.SetActive(false);
        }
        int rows, columns, uniqueCards, cardNumber, maxCards;
        //set values for the difficulty. size of grid, unique cards to match.
        switch (difficulty)
        {
            case 1:
                rows = 4;
                columns = 4;
                uniqueCards = 7;
                break;
            case 2:
                rows = 4;
                columns = 5;
                uniqueCards = 10;
                break;
            case 3:
                rows = 4;
                columns = 7;
                uniqueCards = 14;
                break;
            default:
                rows = 4;
                columns = 4;
                uniqueCards = 7;
                break;
        }

        maxCards = rows * columns;

        if (maxCards % 2 == 0)
        {
            _cardsToSpawnCount = new List<int>();

            _gameManager.cardsLeft = maxCards;

            for (int i = 0; i < uniqueCards; i++)
            {
                Debug.Log(i + " " + rows * columns);

                _cardsToSpawnCount.Add(0);
            }

            //create list of random number. make it big enough to fill the grid size
            var list = new List<int>(rows * columns);
            //random value between 0 and number of unique cards wanted
            //numbers will not repeat an tell it reaches the number of unique cards.
            list = RandomList(uniqueCards - 1).Concat(RandomList(uniqueCards - 1)).ToList();

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
                    _cardsToSpawnCount[list[i]] += 2; //Because it must be even
                }
            }

            bool forward = true;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
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
        MyGameManager.pause = false;
    }
}
