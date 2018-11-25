using UnityEngine.Networking;

public class LobbyInfo : MessageBase
{
    public int currentActivePlayers;
    public string sender;
    public string mapName;
    public int amountOfRounds;
    public int amountOfPlayers;
    public string lobbyPassword;
    public string lobbyName;
    public bool PassForServer;
}
