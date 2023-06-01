using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Move : BaseState
{
    private NPC_Controller _cont;
    public S_Move(NPC_Controller stateMachine) : base("Move", stateMachine)
    {
        _cont = stateMachine;
    }
}
