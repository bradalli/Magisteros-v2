using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class S_Alert : BaseState
{
    #region Private Variables

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

    #endregion

    #region State Methods

    public override void Enter()
    {
        _cont = stateMachine;
        _cont.TryGetComponent(out _handler);

        // Trigger events
        _handler.TriggerEvent("Stop_Move");
        _handler.TriggerEvent("Start_LookAt");

        // Cache variables
        lookAtPosLength = alertTimeLength / lookAroundNum;
        enterTime = Time.time;
        lastLookTime = Time.time - lookAtPosLength;
        lastLookDirection = lookDirection;

        // Implement base
        base.Enter();
    }

    public override void UpdateState()
    {
        // Cache time since given times
        float timeSinceAlertEnter = Time.time - enterTime;
        float timeSinceLastLook = Time.time - lastLookTime;

        #region Transitions
        // -> Despawn (Despawn when out of range to the player)
        if (!_handler.GetValue<bool>("B_InRangeOfPlayer"))
        {
            _cont.ChangeState(_handler.GetValue<BaseState>("State_Despawn"));
            return;
        }

        // -> Idle (Go idle when alertTimeLength has elapsed since state was entered AND the proximity contains no threat)
        if(timeSinceAlertEnter > alertTimeLength && !_handler.GetValue<bool>("B_ProxContainsThreat"))
        {
            _cont.ChangeState(_handler.GetValue<BaseState>("State_Idle"));
            return;
        }

        // -> Combat (Go into combat when a threat has been detected in the view cone of the character)
        if(_handler.GetValue<bool>("B_ViewContainsThreat"))
        {
            _cont.ChangeState(_handler.GetValue<BaseState>("State_Combat"));
            return;
        }

        // -> Search (Go search when alertTimeLength has elapsed since state was entered AND the proximity still contains a threat)
        if (timeSinceAlertEnter > alertTimeLength && _handler.GetValue<bool>("B_ProxContainsThreat"))
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

        if (_target == null)
        {
            // Every time lookAtPosLength has elapsed, pick a new direction to look
            if(timeSinceLastLook > lookAtPosLength)
            {
                // Set the lookDirection to a random direction
                lookDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1, 1));

                // If new direction is too similar to the last one, look in the opposite direction.
                if (Vector3.Distance(lastLookDirection, lookDirection) < 0.5f)
                    lookDirection *= -1;

                lookDirection += _cont.transform.position;
                lastLookTime = Time.time;
            }

            // Update V_LookPosition if lookDirection has changed since last frame
            if (lookDirection != lastLookDirection)
                _handler.SetValue("V_LookPosition", lookDirection);

            // Update lastLookDirection
            lastLookDirection = lookDirection;
        }
    }

    public override void Exit()
    {
        // Revert events triggered in Enter()
        _handler.TriggerEvent("Stop_LookAt");
        base.Exit();
    }

    #endregion
}
