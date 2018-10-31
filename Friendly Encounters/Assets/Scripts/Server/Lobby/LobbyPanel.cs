using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(NetworkIdentity))]
public class LobbyPanel : NetworkBehaviour
{
    public static LobbyPanel instance = null;

    private NetworkClient client;
    private LobbyUI lobbyUI = null;
    
    private bool isSetup = false;
    private bool isClientInitializated = false;

    private MessageInfo packet;

    void Awake()
    {
        packet = new MessageInfo();
        packet.sender = MyGameManager.GetUser().Name;
        lobbyUI = FindObjectOfType<LobbyUI>();
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

    private void OnConnected(NetworkMessage netMsg)
    {
        if (client.isConnected)
        {
            //client.RegisterHandler(MyMsgType.JoinMsg, OnJoinMessage);
            //client.Send(MyMsgType.JoinMsg, packet);
        }
    }

    public void SetupClient()
    {
        NetworkManager net = FindObjectOfType<NetworkManager>();
        client = net.client;
        //client.RegisterHandler(MsgType.Connect, OnConnected);

        if (isServer)
        {
            //NetworkServer.RegisterHandler(MyMsgType.JoinMsg, OnJoinMessage);
        }
        isSetup = true;
    }

    public void OnJoinMessage(NetworkMessage netMsg)
    {
        MessageInfo info = netMsg.ReadMessage<MessageInfo>();
        if (isServer && !info.PassForServer)
        {
            info.PassForServer = true;
            //Send to all clients
            NetworkServer.SendToAll(MyMsgType.JoinMsg, info);
        }
        else if (client.isConnected)
        {
            lobbyUI.AddPlayer(info.sender);
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