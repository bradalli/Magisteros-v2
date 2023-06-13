using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class S_Alert : BaseState
{
    private StateMachine _cont;
    private IEventAndDataHandler _handler;
    bool _damageReceived;

    Transform _target;
    Transform _mesh;

    Vector3 lastLookDirection;
    Vector3 lookDirection = Vector3.forward;

    int lookAroundNum = 3;
    
    float alertTimeLength = 5f;
    float enterTime;

    float lookAtPosLength;
    float lastLookTime;
    /*
    public S_Alert(StateMachine stateMachine) : base("Alert", stateMachine)
    {
        _cont = stateMachine;
    }*/

    public override void Enter()
    {
        _cont = stateMachine;
        _cont.TryGetComponent(out _handler);

        _handler.TriggerEvent("Start_Combat");
        _handler.TriggerEvent("Stop_Move");

        lookAtPosLength = alertTimeLength / lookAroundNum;

        lastLookDirection = lookDirection;
        enterTime = Time.time;
        lastLookTime = Time.time - lookAtPosLength;

        base.Enter();
    }

    public override void UpdateState()
    {
        float timePassedAlert = Time.time - enterTime;
        float timePassedLook = Time.time - lastLookTime;

        #region Transitions
        // -> Despawn
        if (!_handler.GetValue<bool>("B_InRangeOfPlayer"))
        {
            _cont.ChangeState(_handler.GetValue<BaseState>("State_Despawn"));
            return;
        }

        // -> Idle
        if(timePassedAlert > alertTimeLength && !_handler.GetValue<bool>("B_ProxContainsThreat"))
        {
            _cont.ChangeState(_handler.GetValue<BaseState>("State_Idle"));
            return;
        }

        // -> Combat
        if(_handler.GetValue<bool>("B_ViewContainsThreat"))
        {
            _cont.ChangeState(_handler.GetValue<BaseState>("State_Combat"));
            return;
        }

        // -> Search
        if(timePassedAlert > alertTimeLength && _handler.GetValue<bool>("B_ProxContainsThreat"))
        {
            _cont.ChangeState(_handler.GetValue<BaseState>("State_Search"));
            return;
        }
        #endregion

        // Change target if closest threat has changed.
        Transform tmpTarget = _handler.GetValue<Transform>("T_ClosestThreatInView");
        if (tmpTarget != null)
        {
            if (tmpTarget != _target)
                _target = tmpTarget;
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

                lookDirection += _cont.transform.position;
                lastLookTime = Time.time;
            }


            if (lookDirection != lastLookDirection)
                _handler.SetValue("V_LookPosition", lookDirection);
        }
    }

    public override void Exit()
    {
        _handler.TriggerEvent("Stop_Combat");
        _handler.TriggerEvent("Stop_LookAt");
        base.Exit();
    }
}
