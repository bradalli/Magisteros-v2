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

    Vector3 lastLookDirection;
    Vector3 lookDirection = Vector3.forward;

    int lookAroundNum = 3;
    
    float alertTimeLength = 5f;
    float enterTime;

    float lookAtPosLength;
    float lastLookTime;
    public S_Alert(NPC_Controller stateMachine) : base("Alert", stateMachine)
    {
        _cont = stateMachine;
    }

    public override void Enter()
    {
        _cont.Set_AnimBool("InCombat", true);
        _cont.Set_NavDestination(_cont.transform.position);

        lookAtPosLength = alertTimeLength / lookAroundNum;

        lastLookDirection = lookDirection;
        enterTime = Time.time;
        lastLookTime = Time.time - lookAtPosLength;

        //_target = _cont.Get_ClosestThreatInProx();

        base.Enter();
    }

    public override void UpdateState()
    {
        float timePassedAlert = Time.time - enterTime;
        float timePassedLook = Time.time - lastLookTime;

        #region Transitions
        // -> Despawn
        if (_cont.Get_IsNpcOutOfRange())
        {
            _cont.ChangeState(_cont.despawnState);
            return;
        }

        // -> Idle
        if(timePassedAlert > alertTimeLength && _cont.Get_ThreatsInProxNum() == 0)
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
        if(timePassedAlert > alertTimeLength && _cont.Get_ThreatsInProxNum() > 0)
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
            // Every time lookAtPosLength has elapsed, pick a new direction to look
            if(timePassedLook > lookAtPosLength)
            {
                // Set the lookDirection to a random direction
                lookDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1, 1));

                // If new direction is too similar to the last one, look in the opposite direction.
                if (Vector3.Distance(lastLookDirection, lookDirection) < 0.5f)
                    lookDirection *= -1;

                lookDirection += _cont.meshT.position;
                lastLookTime = Time.time;
            }
                

            if(lookDirection != lastLookDirection)
                _cont.Set_LookAtPosition(lookDirection);
        }
    }

    public override void Exit()
    {
        _cont.Set_AnimBool("InCombat", false);
        _cont.Set_LookAtPosition(Vector3.zero);
        base.Exit();
    }
}
