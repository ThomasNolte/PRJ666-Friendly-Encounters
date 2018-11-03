using UnityEngine;

public class DisconnectButton : MonoBehaviour
{

    MyNetworkManager networkManager;

    public void DisconnectMe()
    {
        //if(iserver)
        networkManager = FindObjectOfType<MyNetworkManager>();
        networkManager.StopClient();
        // networkManager.StopHost();
    }
}
