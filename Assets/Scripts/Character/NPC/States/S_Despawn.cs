using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Despawn : BaseState
{
    private StateMachine _cont;
    IEventAndDataHandler _handler;
    public S_Despawn(NPC_Controller stateMachine) : base("Spawn", stateMachine)
    {
        _cont = stateMachine;
    }

    #region State methods
    public override void Enter()
    {
        _cont.TryGetComponent<IEventAndDataHandler>(out _handler);
        base.Enter();
        _handler.TriggerEvent("Disable_C");
    }

    public override void UpdateState()
    {
        _cont.ChangeState(_handler.GetValue<BaseState>("State_Spawn"));
    }

    public override void Exit()
    {
        base.Exit();
    }
    #endregion
}
