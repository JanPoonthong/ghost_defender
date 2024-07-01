using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;

public class timer_script : NetworkBehaviour
{
    public int minute_start = 10;
    public int second = 0;
    public TextMeshProUGUI timer_text;

    void Start()
    {
        // network_manager.runnerInstance.StartCoroutine(wait_for_each_sec());
    }

    IEnumerator wait_for_each_sec()
    {
        print("are you doing this?");
        print(minute_start >= 0);
        while (minute_start >= 0)
        {
            // print("doing timer");
            if (second == 0)
            {
                minute_start -= 1;
                second = 60;
            }
            second -= 1;

            // update_timer_rpc(minute_start,second);
            timer_text.text = minute_start.ToString("D2") + ":" + second.ToString("D2");

            yield return new WaitForSeconds(1f);
        }
    }
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void update_timer_rpc(int _minute, int _second)
    {
        timer_text.text = _minute.ToString("D2") + ":" + _second.ToString("D2");
    }
}
