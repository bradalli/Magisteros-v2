using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Alert : BaseState
{
    private NPC_Controller _cont;
    bool _damageReceived;

    Collider _target;

    float alertTimeLength = 2f;
    float enterTime;
    public S_Alert(NPC_Controller stateMachine) : base("Alert", stateMachine)
    {
        _cont = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        _cont.Set_AnimBool("InCombat", true);
        _cont.Set_NavDestination(_cont.transform.position);

        enterTime = Time.time;
    }

    public override void UpdateState()
    {
        float timePassed = Time.time - enterTime;

        #region Transitions
        // -> Despawn
        if (_cont.Get_IsNpcOutOfRange())
        {
            _cont.ChangeState(_cont.despawnState);
            return;
        }

        // -> Idle
        if(timePassed > alertTimeLength && _cont.Get_ThreatsInProxNum() == 0)
        {
            _cont.ChangeState(_cont.idleState);
            return;
        }

        // -> Combat
        if(_cont.Get_ThreatsInViewNum() > 0)
        {
            _cont.ChangeState(_cont.combatState);
            return;
        }

        // -> Search
        if(timePassed > alertTimeLength && _cont.Get_ThreatsInProxNum() > 0)
        {
            _cont.ChangeState(_cont.searchState);
            return;
        }
        #endregion

        // Change target if closest threat has changed.
        if (_cont.Get_ClosestThreatInView() != null)
        {
            if (_cont.Get_ClosestThreatInView() != _target)
                _target = _cont.Get_ClosestThreatInView();
        }

        if (_target != null)
        {
            _cont.Set_LookAtPosition(_target.transform.position);
        }
    }

    public override void Exit()
    {
        base.Exit();
        _cont.Set_AnimBool("InCombat", false);
    }
}
