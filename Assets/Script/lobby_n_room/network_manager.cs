using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class network_manager : SimulationBehaviour,INetworkRunnerCallbacks
{
    //to create an instance of network runner. makes easy for other script for usage
    public static NetworkRunner runnerInstance;
    //lobby's info
    public string lobby_name = "default";

    //part of room_content_prefab
    public GameObject room_content_prefab;
    public Transform room_content_transform;

    //part of creating the room
    public TMP_InputField room_name_input;
    public TMP_InputField player_name_input;
    //player part
    public GameObject player_prefab;
    public GameObject player_in_room_prefab;
    //local player detail part
    public string player_nick_name;
    void Awake(){
        runnerInstance = gameObject.GetComponent<NetworkRunner>();
        if(runnerInstance == null){
            runnerInstance = gameObject.AddComponent<NetworkRunner>();
        }
    }
    void Start(){
        runnerInstance.JoinSessionLobby(SessionLobby.ClientServer,lobby_name);
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        //destroy all room first, before create new list of room
        destroy_all_room_content_prefab();

        //create new one
        create_all_room_content_prefab(sessionList);
    }
    void destroy_all_room_content_prefab(){
        for(int i =0;i<room_content_transform.childCount;i++){
            room_content_transform.GetChild(i).GetComponent<room_content_prefab_script>().destroy_self();
        }
    }
    void create_all_room_content_prefab(List<SessionInfo> session){
        foreach(SessionInfo each in session){
            //create method
            GameObject room_clone = Instantiate(room_content_prefab,room_content_transform);

            //apply room's info
            room_content_prefab_script room_info = room_clone.GetComponent<room_content_prefab_script>();
            room_info.room_name.text = each.Name;
            room_info.room_count.text = each.PlayerCount + "/" + each.MaxPlayers;
        }
    }

    //creating room method
    public void create_room(){
        runnerInstance.StartGame(new StartGameArgs(){
            SessionName = room_name_input.text,
            GameMode = GameMode.AutoHostOrClient,
            Scene = SceneRef.FromIndex(1),
        });
        player_nick_name = player_name_input.text;

        player_name_input.text = "";
        room_name_input.text = "";
    }


    public void OnConnectedToServer(NetworkRunner runner)
    {
        
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        SceneManager.LoadScene(0);
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new networkInputData();

        if (Input.GetKey(KeyCode.W))
            data.direction += Vector3.forward;

        if (Input.GetKey(KeyCode.S))
            data.direction += Vector3.back;

        if (Input.GetKey(KeyCode.A))
            data.direction += Vector3.left;

        if (Input.GetKey(KeyCode.D))
            data.direction += Vector3.right;

        input.Set(data);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        // if(runner.IsServer){
        //     Debug.LogWarning("finished loading scene");

        //     player_in_room_spawner script_spawner = GameObject.FindObjectOfType<player_in_room_spawner>();
        //     script_spawner.RPCadding_player_list(player);

        //     Debug.LogWarning("this is player nick name : " + player_nick_name);

        //     // runnerInstance.Spawn(player_prefab,new Vector3(0,.2f,0),Quaternion.identity,player);
        // }
        Debug.LogWarning("finished loading scene");

        player_in_room_spawner script_spawner = GameObject.FindObjectOfType<player_in_room_spawner>();
        script_spawner.RPCadding_player_list(player,player_nick_name);

        Debug.LogWarning("this is player nick name : " + player_nick_name);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        SceneManager.LoadScene(0);
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        SceneManager.LoadScene(0);
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }
}
