using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBalloon : MonoBehaviour {

    public float lifeTime = 5f;

    void Start() {
        Destroy(gameObject, lifeTime);
    }
}
