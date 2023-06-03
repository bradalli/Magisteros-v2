using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Flee : BaseState
{
    private NPC_Controller _cont;
    private Vector3 fleeDirection;
    public S_Flee(NPC_Controller stateMachine) : base("Flee", stateMachine)
    {
        _cont = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        // Work out the average position of all the threats in proximity
        Vector3 averageThreatPosition = Vector3.zero;
        foreach(Collider col in _cont.Get_ThreatsInProx())
        {
            averageThreatPosition += col.transform.position;
        }
        averageThreatPosition /= _cont.Get_ThreatsInProxNum();

        // Work out target flee direction
        fleeDirection = (_cont.transform.position - averageThreatPosition).normalized;
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

        // -> Idle
        if (_cont.Get_ThreatsInProxNum() == 0)
        {
            _cont.ChangeState(_cont.idleState);
            return;
        }

        // -> Combat
        if (!_cont.Get_IsFearOverMax())
        {
            _cont.ChangeState(_cont.combatState);
            return;
        }
        #endregion
        Vector3 fleePosition = _cont.transform.position + (fleeDirection * _cont.fleeDistance);
        _cont.Set_NavDestination(fleePosition);
    }

    public override void Exit()
    {
        base.Exit();
    }
}

