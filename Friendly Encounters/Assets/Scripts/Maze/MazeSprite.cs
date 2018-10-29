using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSprite : MonoBehaviour {

    SpriteRenderer sr;

    void Awake() {
        sr = GetComponent<SpriteRenderer>();

    }

    public void setSprite(Sprite sprite, int sortingOrder) {
        sr.sprite = sprite;
        sr.sortingOrder = sortingOrder;
    }

    public void setSprite(Sprite sprite) {
        setSprite(sprite, 0);
    }
    

    
}
