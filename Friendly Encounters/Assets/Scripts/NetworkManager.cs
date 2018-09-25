using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

[AddComponentMenu("Network/NetworkManager")]
public class NetworkManager : MonoBehaviour {

    public static string currentSceneName = string.Empty;

    private static List<Transform> StartPositions = new List<Transform>();
    private static AddPlayerMessage APM = new AddPlayerMessage();
    private static RemovePlayerMessage RPM = new RemovePlayerMessage();
    private static ErrorMessage EM = new ErrorMessage();

    [SerializeField]
    private int networkPort = 7777;
    [SerializeField]
    private string serverBindAddress = string.Empty;
    [SerializeField]
    private string networkAddress = "localhost";
    [SerializeField]
    private bool dontDestroyOnLoad = true;
    [SerializeField]
    private bool runInBackground = true;
    [SerializeField]
    private bool scriptCRCCheck = true;
    [SerializeField]
    private float maxDelay = 0.01f;
    [SerializeField]
    private LogFilter.FilterLevel logLevel = LogFilter.FilterLevel.Info;
    [SerializeField]
    private bool autoCreatePlayer = true;
    [SerializeField]
    private string offlineScene = string.Empty;
    [SerializeField]
    private string onlineScene = string.Empty;
    [SerializeField]
    private List<GameObject> spawnPrefabs = new List<GameObject>();
    [SerializeField]
    private int maxConnections = 4;
    [SerializeField]
    private List<QosType> channels = new List<QosType>();
    [SerializeField]
    private int simulatedLatency = 1;
    [SerializeField]
    private string matchHost = "mm.unet.unity3d.com";
    [SerializeField]
    private int matchPort = 443;
    [SerializeField]
    public string matchName = "default";
    [SerializeField]
    public uint matchSize = 4;
    [SerializeField]
    private bool serverBindToIP;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private PlayerSpawnMethod playerSpawnMethod;


    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
