using FishNet.Managing;
using FishNet.Transporting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour
{

    private NetworkManager _networkManager;
    private LocalConnectionStates _serverState = LocalConnectionStates.Stopped;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene(1);
        if(!_networkManager)
        {
            _networkManager = FindObjectOfType<NetworkManager>();
#if UNITY_WEBGL
            _networkManager.ClientManager.StartConnection();
#endif
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
