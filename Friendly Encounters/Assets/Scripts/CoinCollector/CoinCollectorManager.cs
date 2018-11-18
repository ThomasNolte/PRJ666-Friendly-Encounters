using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CoinCollectorManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform[] spawnPoints;
    public Text countText;          //Store a reference to the UI Text component which will display the number of pickups collected.
    public Text winText;            //Store a reference to the UI Text component which will display the 'You win' message.

    public static int count;              //Integer to store the number of pickups collected so far.

    private bool gameOver = false;
    private TutorialMiniGameManager manager;

    // Use this for initialization
    void Start()
    {
        manager = FindObjectOfType<TutorialMiniGameManager>();
        //Initialize count to zero.
        count = 0;
        //Initialze winText to a blank string since we haven't won yet at beginning.
        winText.text = "";
        
        Instantiate(playerPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity, null);

        //Call our SetCountText function which will update the text with the current value for count.
        SetCountText();
    }

    void Update()
    {
        if (!MyGameManager.pause)
        {
            SetCountText();

            if (gameOver)
            {
                if (manager != null)
                {
                    StartCoroutine(BackToMainGame());
                }
            }
        }
    }

    //This function updates the text displaying the number of objects we've collected and displays our victory message if we've collected all of them.
    void SetCountText()
    {
        //Set the text property of our our countText object to "Count: " followed by the number stored in our count variable.
        countText.text = "Count: " + count.ToString() + "/12";

        //Check if we've collected all 12 pickups. If we have...
        if (count >= 12 && !gameOver)
        {
            //... then set the text property of our winText object to "You win!"
            winText.text = "You win!";
            gameOver = true;
        }
    }

    IEnumerator BackToMainGame()
    { 
        gameOver = false;
        yield return new WaitForSeconds(2.5f);
        manager.IsMiniGameFinished = true;
    }
}
