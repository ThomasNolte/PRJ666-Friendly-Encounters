using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;

[RequireComponent(typeof(NetworkIdentity))]
public class ChatManager : NetworkBehaviour
{
    private NetworkClient client;
    private ChatUI chatUI = null;

    [TextArea(3, 77)] public string blackList;
    public string replaceString = "*";

    private string clientName = string.Empty;
    private bool isSetup = false;
    private bool isClientInitializated = false;

    void Awake()
    {
        clientName = MyGameManager.GetUser().Name;
        chatUI = FindObjectOfType<ChatUI>();
    }

    void Start()
    {
        if (!isSetup)
        {
            SetupClient();
        }
        if (isClientInitializated && !isServer)
        {
            OnConnected(null);
        }
    }

    public override void OnStartClient()
    {
        isClientInitializated = true;
    }

    private void SendChatText(InputField field)
    {
        string text = field.text;
        if (string.IsNullOrEmpty(text) || text.Contains("\n"))
        {
            field.text = string.Empty;
            return;
        }
        SendMessageChat(text);
        field.text = string.Empty;
    }

    private void SendMessageChat(string msg)
    {
        MessageInfo info = new MessageInfo();
        info.text = msg;
        info.sender = clientName;
        if (client.isConnected)
        {
            client.Send(MyMsgType.ChatMsg, info);
        }
    }

    public void SetupClient()
    {
        NetworkManager net = FindObjectOfType<NetworkManager>();
        client = net.client;
        client.RegisterHandler(MsgType.Connect, OnConnected);

        if (isServer)
        {
            NetworkServer.RegisterHandler(MyMsgType.ChatMsg, OnChatMessage);
        }
        isSetup = true;
    }

    public void OnChatMessage(NetworkMessage netMsg)
    {
        MessageInfo info = netMsg.ReadMessage<MessageInfo>();
        if (isServer && !info.PassForServer)
        {
            info.PassForServer = true;
            //Send to all clients
            NetworkServer.SendToAll(MyMsgType.ChatMsg, info);
        }
        else if (client.isConnected)
        {
            string text = GetMessageFormat(info.text, info.sender);
            chatUI.AddNewLine(text);
        }
    }

    public void OnConnected(NetworkMessage netMsg)
    {
        if (client.isConnected)
        {
            client.RegisterHandler(MyMsgType.ChatMsg, OnChatMessage);
        }
    }

    public string GetMessageFormat(string text, string sender)
    {
        string hex = ColorUtility.ToHtmlStringRGBA(Color.cyan);
        string msg = text;
        string formatedText = "";
        if (GetBlackListArray.Length > 0)
        {
            msg = ChatUtils.FilterWord(GetBlackListArray, text, replaceString);
        }
        formatedText = string.Format("<color=#{0}> [{2}]</color>{1}", hex, msg, sender);
        return formatedText;
    }

    private string[] GetBlackListArray
    {
        get
        {
            List<string> list = new List<string>();
            string[] token = blackList.Split(',');
            foreach (string str in token)
            {
                string text = str.Trim();
                if (!string.IsNullOrEmpty(text))
                {
                    list.Add(text);
                }
            }
            return list.ToArray();
        }
    }

    public NetworkClient Client
    {
        get
        {
            return client;
        }
        set
        {
            client = value;
        }
    }
}
