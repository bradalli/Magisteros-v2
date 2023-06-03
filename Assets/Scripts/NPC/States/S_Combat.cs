using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Combat : BaseState
{
    private NPC_Controller _cont;
    private Collider _target;

    float startTargetLost;

    public S_Combat(NPC_Controller stateMachine) : base("Combat", stateMachine)
    {
        _cont = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        _target = _cont.Get_ClosestThreatInProx();
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

        // -> Dead
        if(_cont.currHealthValue == 0)
        {
            _cont.ChangeState(_cont.deadState);
        }

        // -> Idle
        if (_cont.Get_ThreatsInProxNum() == 0)
        {
            _cont.ChangeState(_cont.idleState);
            return;
        }

        // -> Flee
        if(_cont.Get_IsFearOverMax())
        {
            _cont.ChangeState(_cont.fleeState);
        }

        // -> Search
        //if()

            #endregion

        // Change target if closest threat has changed.
        if(_cont.Get_ClosestThreatInProx() != null)
        {
            if (_cont.Get_ClosestThreatInProx() != _target)
                _target = _cont.Get_ClosestThreatInProx();
        }

        if(_target != null)
        {
            _cont.Set_NavDestination(_target.transform.position);
            _cont.Set_LookAtPosition(_target.transform.position);
        }
    }

    public override void Exit()
    {
        base.Exit();
        _cont.Set_NavDestination(_cont.transform.position);
        _cont.Set_LookAtPosition(_cont.transform.forward);
    }
}
