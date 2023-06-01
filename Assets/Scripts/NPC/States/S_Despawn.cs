using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Despawn : BaseState
{
    private NPC_Controller _cont;
    public S_Despawn(NPC_Controller stateMachine) : base("Despawn", stateMachine)
    {
        _cont = stateMachine;
    }
}

