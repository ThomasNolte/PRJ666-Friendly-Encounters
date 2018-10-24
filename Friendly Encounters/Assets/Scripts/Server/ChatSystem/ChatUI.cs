using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatUI : MonoBehaviour
{
    [SerializeField]
    private GameObject MessagePrefab = null;
    [SerializeField]
    private Transform ChatPanel = null;

    private List<GameObject> cacheMessages = new List<GameObject>();

    private InputField inputField;

    private ChatManager chat;

    void Awake()
    {
        chat = FindObjectOfType<ChatManager>();
        inputField = GameObject.Find("InputField").GetComponent<InputField>();
        inputField.onValueChanged.AddListener(ClearInputField);
    }

    public void AddNewLine(string text)
    {
        GameObject newline = Instantiate(MessagePrefab) as GameObject;
        newline.GetComponent<Text>().text = text;
        newline.GetComponent<LayoutElement>().CalculateLayoutInputVertical();
        newline.GetComponent<LayoutElement>().CalculateLayoutInputHorizontal();
        newline.transform.SetParent(ChatPanel, false);
        cacheMessages.Add(newline);
    }

    public void ClearInputField(string text)
    {
        if (text.Contains("\n")) {
            //Remove the newline character
            text.Remove(text.Length - 1);
            chat.SendChatText(inputField);
        }
    }

    public void Clean()
    {
        foreach (GameObject g in cacheMessages)
        {
            Destroy(g);
        }
        cacheMessages.Clear();
    }
}
