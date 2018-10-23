using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkIdentity))]
public class ChatManager : NetworkBehaviour
{
    public string clientName = string.Empty;

    private NetworkClient client;
    private ChatUI chatUI = null;

    private bool isSetup = false;
    private bool isClientInitializated = false;

    public class MyMsgType
    {
        public static short ChatMsg = MsgType.Highest + 1;
    }

    void Awake()
    {
        clientName = string.Empty;
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

    public void SendChatText(InputField field)
    {
        string text = field.text;
        if (string.IsNullOrEmpty(text))
            return;
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
        if (client.isConnected) {
            client.RegisterHandler(MyMsgType.ChatMsg, OnChatMessage);
        }
    }

    public string GetMessageFormat(string text, string sender)
    {
        string hex = ColorUtility.ToHtmlStringRGBA(Color.cyan);
        string msg = string.Format("{2}<color=#{0}>[{1}]</color>", hex, text, sender);
        return msg;
    }
}
