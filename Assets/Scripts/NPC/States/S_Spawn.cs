using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Spawn : BaseState
{
    private NPC_Controller _cont;
    public S_Spawn(NPC_Controller stateMachine) : base("Spawn", stateMachine) 
    {
        _cont = stateMachine;
    }
}
