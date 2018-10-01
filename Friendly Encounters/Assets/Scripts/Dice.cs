using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour {

    public static int whosTurn = 1;
    public Sprite[] diceSides;
    
    private Image img;
    private Button btn;
    private bool coroutineAllowed = true;

	// Use this for initialization
	void Start () {
        img = GetComponent<Image>();
        btn = GetComponent<Button>();
        btn.onClick.AddListener(Roll);
        img.sprite = diceSides[5];
    }

    private void Roll() {
        if (coroutineAllowed)
            StartCoroutine("RollTheDice");
    }

    private IEnumerator RollTheDice() {
        coroutineAllowed = false;
        int randomDiceSide = 0;
        for (int i = 0; i <= 20; i++) {
            randomDiceSide = Random.Range(0, 6);
            img.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.05f);
        }

        TurnSystem.diceSide = randomDiceSide + 1;

        if (whosTurn == 1){
            TurnSystem.MovePlayer(1);
        }
        else if (whosTurn == -1){
            TurnSystem.MovePlayer(2);
        }

        whosTurn *= -1;

        coroutineAllowed = true;
    }



}
