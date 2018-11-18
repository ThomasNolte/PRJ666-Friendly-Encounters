using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public List<string> lobbyPlayers = new List<string>();
    
    public GameObject lobbyPlayer = null;
    public Transform playerPanel = null;

    public void AddPlayer()
    {
        foreach (Transform child in playerPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        for (int i= 0; i < lobbyPlayers.Count; i++) {
            GameObject player = Instantiate(lobbyPlayer) as GameObject;
            player.GetComponentInChildren<Text>().text = lobbyPlayers[i];
            player.GetComponent<LayoutElement>().CalculateLayoutInputVertical();
            player.GetComponent<LayoutElement>().CalculateLayoutInputHorizontal();
            player.transform.SetParent(playerPanel, false);
        }
    }
}
