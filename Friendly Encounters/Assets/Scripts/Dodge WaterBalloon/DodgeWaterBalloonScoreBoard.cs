using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeWaterBalloonScoreBoard : MonoBehaviour {

    public static bool IsDodgeWaterBalloon = false;

	void Start () {
        IsDodgeWaterBalloon = true;
        if (!FindObjectOfType<WaterBalloonSpawner>().enabled)
        {
            FindObjectOfType<WaterBalloonSpawner>().enabled = true;
        }
    }
	
}
