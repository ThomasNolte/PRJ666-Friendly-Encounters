using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float speed = 0.2f;

    void Update()
    {
        Vector2 offset = new Vector2(Time.time * speed, 0);
        GetComponent<Renderer>().material.mainTextureOffset = offset;
    }
}
