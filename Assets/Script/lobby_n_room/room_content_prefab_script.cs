using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class room_content_prefab_script : MonoBehaviour
{
    public TextMeshProUGUI room_name, room_count;
    public Button join_button;
    public void join_button_function()
    {
        network_manager.runnerInstance.StartGame(new StartGameArgs()
        {
            SessionName = room_name.text,
            GameMode = GameMode.AutoHostOrClient,
        });
    }
    public void destroy_self()
    {
        Destroy(this);
    }
}
