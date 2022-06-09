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

    private Vector3 centerPoint;

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

            /*
            centerPoint = playerController.transform.position;
            playerController.transform.position += new Vector3(Random.Range(3f, 5f), 0, Random.Range(3f, 5f));
            */
        }

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (networkObject.IsOwner) {
            playerController.transform.RotateAround(centerPoint, Vector3.up, Time.deltaTime);
            playerController.transform.Rotate(new Vector3(1,1,0), Space.Self);
        }
        */
    }

}
