using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Alert : BaseState
{
    private NPC_Controller _cont;
    bool _damageReceived;
    public S_Alert(NPC_Controller stateMachine) : base("Alert", stateMachine)
    {
        _cont = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        _cont.Set_AnimBool("InCombat", true);
        _cont.Set_NavDestination(_cont.transform.position);
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
        if(_cont.Get_ThreatsInProxNum() == 0)
        {
            _cont.ChangeState(_cont.idleState);
            return;
        }

        // -> Combat
        Debug.Log(_cont.Get_ThreatsInViewNum());
        if(_cont.Get_ThreatsInViewNum() > 0)
        {
            _cont.ChangeState(_cont.combatState);
            return;
        }
        #endregion

        _cont.Set_LookAtPosition(_cont.Get_ClosestThreatInProx().transform.position);
    }

    public override void Exit()
    {
        base.Exit();
        _cont.Set_AnimBool("InCombat", false);
    }
}
