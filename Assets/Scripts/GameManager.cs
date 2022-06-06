using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Component.Spawning;
using FishNet.Object;
using TheKingdomMetaverse.WorldStreaming;
using Cinemachine;
using FishNet;
using FishNet.Managing;
using FishNet.Transporting;

public class GameManager : Singleton<GameManager>
{

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Application.targetFrameRate = 60;

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
