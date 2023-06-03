using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Search : BaseState
{
    private NPC_Controller _cont;

    float searchTimeLength = 2f;
    float enterTime;

    public S_Search(NPC_Controller stateMachine) : base("Search", stateMachine)
    {
        _cont = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
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
        if (timePassed > searchTimeLength)
        {
            _cont.ChangeState(_cont.idleState);
            return;
        }

        // -> Combat
        if (_cont.Get_ThreatsInViewNum() > 0)
        {
            _cont.ChangeState(_cont.combatState);
            return;
        }
        #endregion
    }

    public override void Exit()
    {
        base.Exit();
    }
}
