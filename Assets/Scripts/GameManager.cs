using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Component.Spawning;
using FishNet.Object;
using TheKingdomMetaverse.WorldStreaming;
using Cinemachine;


public class GameManager : Singleton<GameManager>
{
    
    public PlayerSpawner playerSpawner;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Application.targetFrameRate = 60;
        playerSpawner.OnSpawned += (NetworkObject networkObject) =>
        {
            GameObject.FindObjectOfType<WorldStreamer>().player = networkObject.transform.GetChild(0);
            GameObject.FindObjectOfType<CinemachineVirtualCamera>().Follow = networkObject.transform.GetChild(0);
        };


    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
