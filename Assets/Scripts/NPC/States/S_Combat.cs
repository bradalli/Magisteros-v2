using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Combat : BaseState
{
    private NPC_Controller _cont;
    public S_Combat(NPC_Controller stateMachine) : base("Combat", stateMachine)
    {
        _cont = stateMachine;
    }
}
