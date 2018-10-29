using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//List of players in the lobby
public class LobbyPlayerList : MonoBehaviour
{
    public static LobbyPlayerList instance = null; 

    public RectTransform playerListContentTransform;
    public GameObject warningDirectPlayServer;
    public Transform addButtonRow;

    protected VerticalLayoutGroup layout;
    protected List<LobbyPlayer> players = new List<LobbyPlayer>();

    public void OnEnable()
    {
        instance = this;
        layout = playerListContentTransform.GetComponent<VerticalLayoutGroup>();
    }

    public void DisplayDirectServerWarning(bool enabled)
    {
        if (warningDirectPlayServer != null)
            warningDirectPlayServer.SetActive(enabled);
    }

    void Update()
    {
        //this dirty the layout to force it to recompute evryframe (a sync problem between client/server
        //sometime to child being assigned before layout was enabled/init, leading to broken layouting)

        if (layout)
            layout.childAlignment = Time.frameCount % 2 == 0 ? TextAnchor.UpperCenter : TextAnchor.UpperLeft;
    }

    public void AddPlayer(LobbyPlayer player)
    {
        if (players.Contains(player))
            return;

        players.Add(player);

        player.transform.SetParent(playerListContentTransform, false);
        addButtonRow.transform.SetAsLastSibling();

        PlayerListModified();
    }

    public void RemovePlayer(LobbyPlayer player)
    {
        players.Remove(player);
        PlayerListModified();
    }

    public void PlayerListModified()
    {
        int i = 0;
        foreach (LobbyPlayer p in players)
        {
            p.OnPlayerListChanged(i);
            ++i;
        }
    }
}