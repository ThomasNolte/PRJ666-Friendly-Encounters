using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MazeDirectives : MonoBehaviour {

    public int keys;
    public static float timer;

    public Text keysValueText;

    public Text Time;

    public MazeGoal mazeGoalPrefab;
    public MazeKey mazeKeyPrefab;

    MazeGoal mazeGoal;

    int foundKeys;

    List<Vector3> mazeKeyPositions;

    void Awake() {
        MazeGenerator.OnMazeReady += StartDirectives;
    }

    void Start() {
        SetKeyValueText();
    }

   /* void Update()
    {
        timer = Time.time;
    }
    */

    void StartDirectives() {
        mazeGoal = Instantiate(mazeGoalPrefab, MazeGenerator.instance.mazeGoalPosition, Quaternion.identity) as MazeGoal;
        mazeGoal.transform.SetParent(transform);

        mazeKeyPositions = MazeGenerator.instance.GetRandomFloorPositions(keys);

        for (int i = 0; i < mazeKeyPositions.Count; i++) {
            MazeKey mazeKey = Instantiate(mazeKeyPrefab, mazeKeyPositions[i], Quaternion.identity) as MazeKey;
            mazeKey.transform.SetParent(transform);
        }
    }

    public void OnKeyFound() {
        foundKeys++;

        SetKeyValueText();

        if (foundKeys == keys)
        {
            GetComponentInChildren<MazeGoal>().OpenGoal();
        }
    }

    void SetKeyValueText() {
        keysValueText.text = foundKeys.ToString() + " of " + keys.ToString();
    }


}