using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField]
    private GameObject lobbyPlayer = null;
    [SerializeField]
    private Transform playerPanel;

    public void AddPlayer(string playername)
    {
        GameObject player = Instantiate(lobbyPlayer) as GameObject;
        player.GetComponentInChildren<Text>().text = playername;
        player.GetComponent<LayoutElement>().CalculateLayoutInputVertical();
        player.GetComponent<LayoutElement>().CalculateLayoutInputHorizontal();
        player.transform.SetParent(playerPanel, false);
    }
}
