using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float speed = 0.2f;

    void Update()
    {
        Vector2 offset = new Vector2(0, -(Time.time * speed));
        GetComponent<Renderer>().material.mainTextureOffset = offset;
    }
}
