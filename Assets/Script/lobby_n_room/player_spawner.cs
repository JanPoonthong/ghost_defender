using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class player_spawner : NetworkBehaviour,IPlayerJoined
{
    public GameObject player_prefab;

    public void PlayerJoined(PlayerRef player)
    {
        if(Runner.IsServer)
            Runner.Spawn(player_prefab,new Vector3(0,.2f,0),Quaternion.identity);
    }
}
