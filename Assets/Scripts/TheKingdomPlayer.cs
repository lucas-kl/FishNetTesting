using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TheKingdomMetaverse.WorldStreaming;
using UnityEngine;
using FishNet.Managing;
using FishNet.Transporting;
using FishNet;
using FishNet.Object;
using StarterAssets;
using UnityEngine.InputSystem;

public class TheKingdomPlayer : MonoBehaviour
{

    public ThirdPersonController playerController;
    public PlayerInput playerInput;

    [SerializeField]
    private NetworkObject networkObject;

    private void Awake()
    {
        playerController.enabled = false;
        playerInput.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        networkObject = this.GetComponent<NetworkObject>();
        if (networkObject.IsOwner)
        {
            GameObject.FindObjectOfType<WorldStreamer>().player = this.transform.GetChild(1);
            GameObject.FindObjectOfType<CinemachineVirtualCamera>().Follow = this.transform.GetChild(0);
            playerController.enabled = true;
            playerInput.enabled = true;
            playerInput.ActivateInput();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
