using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Idle : BaseState
{
    private StateMachine _cont;
    private IEventAndDataHandler _handler;
    public S_Idle(NPC_Controller stateMachine) : base("Idle", stateMachine)
    {
        _cont = stateMachine;
    }

    public override void Enter()
    {
        _cont.TryGetComponent(out _handler);
        base.Enter();

        _handler.TriggerEvent("Stop_Move");
        _handler.TriggerEvent("Stop_LookAtTarget");
    }

    public override void UpdateState()
    {
        #region Transitions
        // -> Despawn
        if (!_handler.GetValue<bool>("B_InRangeOfPlayer"))
        {
            _cont.ChangeState(_handler.GetValue<BaseState>("State_Despawn"));
            return;
        }

        // -> Alert
        if(_handler.GetValue<bool>("B_ProxContainsThreat"))
        {
            _cont.ChangeState(_handler.GetValue<BaseState>("State_Alert"));
            return;
        } // Need to add function for other allies in proximity to warn this npc.

        // -> Perform
        /* NOT PLANNING ON IMPLEMENTING AT THIS TIME (12.06.2023)
        if(_cont.Get_CurrAction() != null)
        {
            _cont.ChangeState(_cont.performState);
            return;
        }*/

        // -> Move
        if (_handler.GetValue<Waypoint>("W_CurrWp") != null)
        {
            _cont.ChangeState(_handler.GetValue<BaseState>("State_Move"));
            return;
        }

        #endregion
    }
    public override void Exit()
    {
        base.Exit();
    }
}

