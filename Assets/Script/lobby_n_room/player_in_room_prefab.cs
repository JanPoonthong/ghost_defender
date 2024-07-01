using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;

public class player_in_room_prefab : MonoBehaviour
{
    public TextMeshProUGUI player_name_text;
    void Start(){
        player_in_room_spawner script = GameObject.FindObjectOfType<player_in_room_spawner>();
        script.RPCset_parent_of_player_in_room_prefab(GetComponent<NetworkObject>());
    }
    [Rpc(RpcSources.All,RpcTargets.All,HostMode =RpcHostMode.SourceIsServer)]
    public void RPCdestroy_self(){
        Destroy(gameObject);
    }
    [Rpc(RpcSources.All,RpcTargets.All,HostMode =RpcHostMode.SourceIsServer)]
    public void RPCset_name(string _name){
        player_name_text.text = _name;
    }
}
