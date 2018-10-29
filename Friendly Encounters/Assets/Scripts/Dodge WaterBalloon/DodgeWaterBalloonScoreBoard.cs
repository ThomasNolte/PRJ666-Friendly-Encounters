using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeWaterBalloonScoreBoard : MonoBehaviour {

	void Start () {
        if (!FindObjectOfType<WaterBalloonSpawner>().enabled)
        {
            FindObjectOfType<WaterBalloonSpawner>().enabled = true;
        }
    }
	
}
