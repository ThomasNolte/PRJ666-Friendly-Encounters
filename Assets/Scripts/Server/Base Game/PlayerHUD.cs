using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public enum ColorIndex
    {
        RED,
        BLUE,
        GREEN,
        YELLOW,
        CYAN,
        PURPLE
    }

    public const int MAXCOLOURS = 6;
    public Sprite[] imgs;

    private bool selected = false;
    private int imgIndex = 0;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(HUDSelected);
    }

    private void HUDSelected()
    {
        selected = true;
    }

    public void ChangeColor(int index)
    {
        GetComponent<Image>().sprite = imgs[index];
        imgIndex = index;
    }

    public int ImgIndex
    {
        get
        {
            return imgIndex;
        }
        set
        {
            imgIndex = value;
        }
    }

    public bool Selected
    {
        get
        {
            return selected;
        }
        set
        {
            selected = value;
        }
    }

}
