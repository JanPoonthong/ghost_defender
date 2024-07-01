using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class player_movement : NetworkBehaviour
{
    NetworkCharacterController char_control;
    public float speed;
    void Start(){
        char_control = GetComponent<NetworkCharacterController>();
    }
    public override void FixedUpdateNetwork()
    {
        if(GetInput(out networkInputData data)){
            data.direction.Normalize();
            char_control.Move(speed*data.direction*Runner.DeltaTime);
        }
    }
}
