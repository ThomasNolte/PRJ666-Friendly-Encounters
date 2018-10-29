using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class MyNetworkManager : NetworkManager {

    private float nextRefreshTime;

    List<NetworkClient> playerList;

    public void OnServerAddPlayer()
    {

    }

    void Awake()
    {
        playerList = new List<NetworkClient>();
    }

    private void Update() {
        if (Time.time >= nextRefreshTime) {
            RefreshMatches();
        }
        Debug.Log(NetworkManager.singleton.client.connection.playerControllers.Count);
    }
    public void StartHosting()
    {
        StartMatchMaker();
        matchMaker.CreateMatch("Testing Game Lobby", 4, true, "", "", "", 0, 0, OnMatchCreated);
    }

    private void OnMatchCreated(bool success, string extendedInfo, MatchInfo responseData)
    {
        base.StartHost(responseData);
        if (IsClientConnected() && !ClientScene.ready)
        {
            ClientScene.Ready(client.connection);
            if (ClientScene.localPlayers.Count == 0)
                ClientScene.AddPlayer((short)0);
        }
        RefreshMatches();
    }

    public void JoinMatch(MatchInfoSnapshot match){
        if (matchMaker == null) {
            StartMatchMaker();
        }
        matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, HandleJoinedMatch);
    }

    private void HandleJoinedMatch(bool success, string extendedInfo, MatchInfo responseData){
        StartClient(responseData);
    }

    public void RefreshMatches(){
        nextRefreshTime = Time.time + 5f;
        if (matchMaker == null) {
            StartMatchMaker();
        }

        matchMaker.ListMatches(0, 10, "", true, 0, 0, HandleListMatchesComplete);
    }

    private void HandleListMatchesComplete(bool success, string extendedInfo, List<MatchInfoSnapshot> responseData){
        AvailableMatchesList.HandleNewMatchList(responseData);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        if (IsClientConnected() && !ClientScene.ready)
        {
            ClientScene.Ready(client.connection);
            if (ClientScene.localPlayers.Count == 0)
                ClientScene.AddPlayer((short)0);
        }
    }
}
