using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TheKingdomMetaverse.WorldStreaming;
using UnityEngine;
using FishNet.Managing;
using FishNet.Transporting;
using FishNet;
using FishNet.Object;

public class TheKingdomPlayer : MonoBehaviour
{
    [SerializeField]
    private NetworkObject networkObject;
    // Start is called before the first frame update
    void Start()
    {
        networkObject = this.GetComponent<NetworkObject>();
        if (networkObject.IsOwner)
        {
            GameObject.FindObjectOfType<WorldStreamer>().player = this.transform.GetChild(1);
            GameObject.FindObjectOfType<CinemachineVirtualCamera>().Follow = this.transform.GetChild(0);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
