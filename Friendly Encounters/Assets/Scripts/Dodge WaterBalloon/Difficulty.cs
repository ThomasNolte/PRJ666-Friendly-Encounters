using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Difficulty
{
    //60
    static float secondsToMaxDifficulty = 1000;

    public static float GetDifficultyPercent()
    {
        return Mathf.Clamp01(Time.timeSinceLevelLoad / secondsToMaxDifficulty);
    }

}
