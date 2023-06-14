using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Flee : BaseState
{
    private StateMachine _cont;
    private IEventAndDataHandler _handler;
    private Vector3 fleeDirection;
    IDamagable myDmg;
    /*
    public S_Flee(NPC_Controller stateMachine) : base("Flee", stateMachine)
    {
        _cont = stateMachine;
    }*/

    public override void Enter()
    {
        _cont = stateMachine;
        _cont.TryGetComponent(out _handler);
        base.Enter();

        // Work out target flee direction
        fleeDirection = (_cont.transform.position - _handler.GetValue<Vector3>("V_ProxThreatsAvgPosition")).normalized;
        _handler.TriggerEvent("Start_Move");

        _cont.transform.TryGetComponent(out myDmg);
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

        // -> Dead
        if (myDmg != null)
        {
            if (myDmg.Health == 0)
            {
                _cont.ChangeState(_handler.GetValue<BaseState>("State_Dead"));
            }
        }

        // -> Idle
        if (!_handler.GetValue<bool>("B_ProxContainsThreat"))
        {
            _cont.ChangeState(_handler.GetValue<BaseState>("State_Idle"));
            return;
        }

        // -> Combat
        if (!_handler.GetValue<bool>("B_IsFearful"))
        {
            _cont.ChangeState(_handler.GetValue<BaseState>("State_Idle"));
            return;
        }
        #endregion

        fleeDirection = (_cont.transform.position - _handler.GetValue<Vector3>("V_ProxThreatsAvgPosition")).normalized;
        Vector3 fleePosition = _cont.transform.position + (fleeDirection * _handler.GetValue<float>("F_FleeDistance"));
        _handler.SetValue("V_Destination", fleePosition);
    }

    public override void Exit()
    {
        _handler.TriggerEvent("Stop_Move");
        base.Exit();
    }
}

