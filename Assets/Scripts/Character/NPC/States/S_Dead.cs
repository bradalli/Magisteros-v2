using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Dead : BaseState
{
    private StateMachine _cont;
    private IEventAndDataHandler _handler;
    public S_Dead(StateMachine stateMachine) : base("Dead", stateMachine)
    {
        _cont = stateMachine;
    }

    public override void Enter()
    {
        _cont.TryGetComponent(out _handler);

        base.Enter();

        _handler.TriggerEvent("Die");
        _handler.TriggerEvent("Disable_C");
    }
}

