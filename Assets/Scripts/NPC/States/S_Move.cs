using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Move : BaseState
{
    private NPC_Controller _cont;
    private Waypoint _wayp;
    public S_Move(NPC_Controller stateMachine) : base("Move", stateMachine)
    {
        _cont = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        _wayp = _cont.Get_CurrWaypoint();

        if (_wayp.transform.position != Vector3.zero)
            _cont.Set_NavDestination(_cont.Get_CurrWaypoint().transform.position);
    }

    public override void UpdateState()
    {
        #region Transitions
        // -> Despawn
        if (_cont.Get_IsNpcOutOfRange())
        {
            _cont.ChangeState(_cont.despawnState);
            return;
        }

        // -> Alert
        if (_cont.Get_ThreatsInProxNum() > 0)
        {
            _cont.ChangeState(_cont.alertState);
            return;
        }

        // -> Perform
        if (_cont.Get_CurrAction() != null)
        {
            _cont.ChangeState(_cont.performState);
            return;
        }
            
        // -> Idle
        if (_cont.Get_CurrWaypoint() == null)
        {
            _cont.ChangeState(_cont.idleState);
            return;
        }
        #endregion

        if (_cont.Get_RemainingNavDistance() <= 0.05f)
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
