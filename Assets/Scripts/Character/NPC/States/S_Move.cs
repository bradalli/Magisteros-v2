using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Move : BaseState
{
    private IEventAndDataHandler _handler;
    private StateMachine _cont;
    private Waypoint _wayp;
    public S_Move(StateMachine stateMachine) : base("Move", stateMachine)
    {
        _cont = stateMachine;
    }

    public override void Enter()
    {
        _cont.TryGetComponent(out _handler);

        base.Enter();
        _wayp = _handler.GetValue<Waypoint>("W_CurrWp");

        if (_wayp.transform.position != Vector3.zero)
        {
            _handler.SetValue("V_Destination", _wayp.transform.position);
            _handler.TriggerEvent("Get_V_Destination");
            _handler.TriggerEvent("Start_Move");
        }
            
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
        if (_handler.GetValue<bool>("B_ProxContainsThreat"))
        {
            _cont.ChangeState(_handler.GetValue<BaseState>("State_Alert"));
            return;
        }

        // -> Perform
        /*if (_cont.Get_CurrAction() != null)
        {
            _cont.ChangeState(_cont.performState);
            return;
        }*/

        // -> Idle
        if (_handler.GetValue<Waypoint>("W_CurrWp") == null)
        {
            _cont.ChangeState(_handler.GetValue<BaseState>("State_Idle"));
            return;
        }
        #endregion

        if (_handler.GetValue<bool>("B_DestinationReach"))
        {
            if(_wayp.nextWp != null)
            {
                _handler.SetValue("W_CurrWp", _wayp.nextWp);
                _wayp = _wayp.nextWp;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        _handler.TriggerEvent("Stop_Move");
    }
}
