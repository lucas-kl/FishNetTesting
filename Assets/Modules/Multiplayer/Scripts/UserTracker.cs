using FishNet;
using FishNet.Managing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserTracker : MonoBehaviour
{

    private NetworkManager _networkManager;
    public TextMeshProUGUI textUI;

    // Start is called before the first frame update
    void Start()
    {
        _networkManager = InstanceFinder.NetworkManager;
        if (_networkManager == null)
        {
            Debug.LogWarning($"PlayerSpawner on {gameObject.name} cannot work as NetworkManager wasn't found on this object or within parent objects.");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        textUI.text = "(N): " + _networkManager.ClientManager.Clients.Count.ToString();
    }
}
