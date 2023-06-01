using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Perform : BaseState
{
    private NPC_Controller _cont;
    public S_Perform(NPC_Controller stateMachine) : base("Peform", stateMachine)
    {
        _cont = stateMachine;
    }
}
