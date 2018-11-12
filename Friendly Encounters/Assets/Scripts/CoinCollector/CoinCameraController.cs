using UnityEngine;
using UnityEngine.UI;

public class CoinCameraController : MonoBehaviour
{
    public GameObject player;       //Public variable to store a reference to the player game object
    private Vector3 offset;         //Private variable to store the offset distance between the player and camera

    public Text countText;          //Store a reference to the UI Text component which will display the number of pickups collected.
    public Text winText;            //Store a reference to the UI Text component which will display the 'You win' message.

    public static int count;              //Integer to store the number of pickups collected so far.

    private bool gameOver = false;
    private TutorialMiniGameManager manager;

    // Use this for initialization
    void Start()
    {
        manager = FindObjectOfType<TutorialMiniGameManager>();
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.transform.position;

        //Initialize count to zero.
        count = 0;

        //Initialze winText to a blank string since we haven't won yet at beginning.
        winText.text = "";
        
        //Call our SetCountText function which will update the text with the current value for count.
        SetCountText();

    }
    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        transform.position = player.transform.position + offset;
        SetCountText();
    }


    //This function updates the text displaying the number of objects we've collected and displays our victory message if we've collected all of them.
    void SetCountText()
    {
        //Set the text property of our our countText object to "Count: " followed by the number stored in our count variable.
        countText.text = "Count: " + count.ToString();

        //Check if we've collected all 12 pickups. If we have...
        if (count >= 12 && !gameOver)
        {
            //... then set the text property of our winText object to "You win!"
            winText.text = "You win!";
            if (manager != null)
            {
                manager.IsMiniGameFinished = true;
            }
            gameOver = true;
        }
    }
}
