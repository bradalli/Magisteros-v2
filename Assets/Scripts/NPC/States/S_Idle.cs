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
        _cont.SetNavDestination(_cont.transform.position);
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
        }

        // -> Perform
        if(_cont.Get_CurrAction() != null)
        {
            _cont.ChangeState(_cont.performState);
        }

        // -> Move
        if (_cont.d_CurrentWaypoint() != null)
        {
            _cont.ChangeState(_cont.moveState);
        }

        #endregion
    }
    public override void Exit()
    {
        base.Exit();
    }
}

