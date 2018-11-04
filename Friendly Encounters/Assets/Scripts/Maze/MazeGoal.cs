using UnityEngine;
using System.Collections;

public class MazeGoal : MonoBehaviour
{

    public Sprite closedGoalSprite;
    public Sprite openedGoalSprite;
    bool openDoor = false;

    void Start() {
        GetComponentInChildren<SpriteRenderer>().sprite = closedGoalSprite;
        openDoor = false;

    }

    public void OpenGoal() {
        GetComponentInChildren<SpriteRenderer>().sprite = openedGoalSprite;
        openDoor = true;
    }

    void OnTriggerEnter2D() {
        if (openDoor == true)
        {
            GameObject.Find("Player").SendMessage("Finish");
            
        }
        else
        {
            GameObject.Find("Player").SendMessage("notFinished");
        }
          
    }
}