using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Alert : BaseState
{
    private NPC_Controller _cont;
    public S_Alert(NPC_Controller stateMachine) : base("Alert", stateMachine)
    {
        _cont = stateMachine;
    }
}
