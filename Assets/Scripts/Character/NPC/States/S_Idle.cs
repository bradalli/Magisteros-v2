using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Idle : BaseState
{
    private NPC_Controller _cont;
    public S_Idle(NPC_Controller stateMachine) : base("Idle", stateMachine)
    {
        _cont = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        _cont.Set_NavDestination(_cont.transform.position);
        _cont.Set_LookAtPosition(Vector3.zero);
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
        if(_cont.Get_ThreatsInProxNum() > 0)
        {
            _cont.ChangeState(_cont.alertState);
            return;
        } // Need to add function for other allies in proximity to warn this npc.

        // -> Perform
        if(_cont.Get_CurrAction() != null)
        {
            _cont.ChangeState(_cont.performState);
            return;
        }

        // -> Move
        if (_cont.Get_CurrWaypoint() != null)
        {
            _cont.ChangeState(_cont.moveState);
            return;
        }

        #endregion
    }
    public override void Exit()
    {
        base.Exit();
    }
}

