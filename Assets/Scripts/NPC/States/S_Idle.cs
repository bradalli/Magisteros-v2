using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Idle : BaseState
{
    private NPC_Controller _cont;
    public S_Idle(NPC_Controller stateMachine) : base("Idle", stateMachine)
    {
        _cont = stateMachine;
    }
}

