using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Move : BaseState
{
    private IEventAndDataHandler _handler;
    private NPC_Controller _cont;
    private Waypoint _wayp;
    public S_Move(NPC_Controller stateMachine) : base("Move", stateMachine)
    {
        _cont = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        _handler.TriggerEvent("Refresh_CurrWp");
        _wayp = _handler.GetValue<Waypoint>("CurrWp");

        if (_wayp.transform.position != Vector3.zero)
        {
            _handler.SetValue("DestinationV", _wayp.transform.position);
            _handler.TriggerEvent("Refresh_DestinationV");
            _handler.TriggerEvent("Start_Move");
        }
            
    }

    public override void UpdateState()
    {
        #region Transitions
        // -> Despawn
        _handler.TriggerEvent("Refresh_InRangeToPlayerB");
        if (!_handler.GetValue<bool>("InRangeToPlayerB"))
        {
            _cont.ChangeState(_cont.despawnState);
            return;
        }

        // -> Alert
        _handler.TriggerEvent("Refresh_ProxContainsThreatB");
        if (_handler.GetValue<bool>("ProxContainsThreatB"))
        {
            _cont.ChangeState(_cont.alertState);
            return;
        }

        // -> Perform
        /*if (_cont.Get_CurrAction() != null)
        {
            _cont.ChangeState(_cont.performState);
            return;
        }*/

        // -> Idle
        _handler.TriggerEvent("Refresh_CurrWp");
        if (_handler.GetValue<Waypoint>("CurrWp") != null)
        {
            _cont.ChangeState(_cont.idleState);
            return;
        }
        #endregion

        _handler.TriggerEvent("Refresh_DestinationReachB");
        if (_handler.GetValue<bool>("DestinationReachB"))
        {
            if(_wayp.nextWp != null)
            {
                _cont.Set_CurrentWaypoint(_wayp.nextWp);
                _wayp = _wayp.nextWp;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        _cont.Set_NavDestination(_cont.transform.position);
    }
}
