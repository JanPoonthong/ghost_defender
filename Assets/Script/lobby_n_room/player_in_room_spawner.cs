using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player_in_room_spawner : NetworkBehaviour
{
    public GameObject player_in_room_prefab;
    public Transform player_in_room_transform;
    public Dictionary<PlayerRef, string> player_list = new Dictionary<PlayerRef, string>();

    // void Start(){
    //     // Invoke("post_start",.5f);
    // }
    // void post_start(){
    //     if(Runner.IsServer || Runner.IsClient){
    //         RPCadding_player_list();
    //         RPCupdate_player_in_room_content();
    //         Debug.LogWarning("Hello!");
    //     }
    // }

    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPCupdate_player_in_room_content()
    {
        if (!Runner.IsServer) return;

        for (int i = 0; i < player_in_room_transform.childCount; i++)
        {
            player_in_room_transform.GetChild(i).GetComponent<player_in_room_prefab>().RPCdestroy_self();
        }

        print("####### this is player count : " + player_list.Count);
        foreach (var each in player_list.Keys)
        {
            print("############### this is player name : " + player_list[each]);
            RPCcreate_player_in_room_prefab(each);
        }
    }
    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPCadding_player_list(PlayerRef player_id, string player_name)
    {
        player_list.Add(player_id, player_name);

        RPCupdate_player_in_room_content();
    }
    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPCremove_player_list(PlayerRef target)
    {
        player_list.Remove(target);

        RPCupdate_player_in_room_content();
    }
    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPCcreate_player_in_room_prefab(PlayerRef player)
    {
        NetworkObject clone = Runner.Spawn(player_in_room_prefab, Vector3.zero, Quaternion.identity, player);
        RPCset_name_of_player_in_room_prefab(clone, player_list[player]);
    }
    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPCset_name_of_player_in_room_prefab(NetworkObject target, string name)
    {
        target.GetComponent<player_in_room_prefab>().RPCset_name(name);
    }
    // [Rpc(RpcSources.All,RpcTargets.All,HostMode =RpcHostMode.SourceIsServer)]
    // public void RPCremove_player_in_room_prefab(NetworkObject target){
    //     target.GetComponent<player_in_room_prefab>().RPCdestroy_self();
    // }
    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPCset_parent_of_player_in_room_prefab(NetworkObject target)
    {
        target.transform.SetParent(player_in_room_transform);
    }
    public void start_game()
    {
        network_manager.runnerInstance.LoadScene(SceneRef.FromIndex(3));
    }
    public void leave_game()
    {
        if (Runner.IsClient)
        {
            RPCremove_player_list(network_manager.runnerInstance.LocalPlayer);
            Runner.Disconnect(network_manager.runnerInstance.LocalPlayer);
            // Runner.Disconnect(network_manager.runnerInstance.LocalPlayer);
        }

    }
}