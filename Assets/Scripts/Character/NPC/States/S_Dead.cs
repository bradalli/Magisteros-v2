using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Dead : BaseState
{
    private NPC_Controller _cont;
    public S_Dead(NPC_Controller stateMachine) : base("Dead", stateMachine)
    {
        _cont = stateMachine;
    }
}

