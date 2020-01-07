using System;
using UnityEngine;

public class AddScore : MonoBehaviour
{
    MyMongoDB db = new MyMongoDB();
    public void Add(Score score)
    {
        db.InsertScore(score);
    }
}
