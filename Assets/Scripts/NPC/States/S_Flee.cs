using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Flee : BaseState
{
    private NPC_Controller _cont;
    public S_Flee(NPC_Controller stateMachine) : base("Flee", stateMachine)
    {
        _cont = stateMachine;
    }
}

