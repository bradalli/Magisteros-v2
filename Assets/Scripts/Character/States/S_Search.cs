using Brad.FSM;
using UnityEngine;

public class S_Search : BaseState
{
    #region Private Variables

    private StateMachine _cont;
    private IEventAndDataHandler _handler;

    Vector3 startPosition;
    float searchRange = 20f;
    float searchTimeLength = 20f;
    float enterTime;

    #endregion

    #region State Methods

    public override void Enter()
    {
        _cont = stateMachine;
        _cont.TryGetComponent(out _handler);
        base.Enter();
        enterTime = Time.time;
        startPosition = _cont.transform.position;

        _cont.TryGetComponent(out _handler);

        _handler.TriggerEvent("Start_Move");

        // Find a random position within the searchRange
        SetNavDestination(FindRandomSearchPosition(startPosition, searchRange));
    }

    public override void UpdateState()
    {
        float timePassed = Time.time - enterTime;

        #region Transitions
        // -> Despawn
        if (!_handler.GetValue<bool>("B_InRangeOfPlayer"))
        {
            _cont.ChangeState(_handler.GetValue<BaseState>("State_Despawn"));
            return;
        }

        // -> Idle
        if (timePassed > searchTimeLength)
        {
            _cont.ChangeState(_handler.GetValue<BaseState>("State_Idle"));
            return;
        }

        // -> Combat
        if (_handler.GetValue<bool>("B_ViewContainsThreat"))
        {
            _cont.ChangeState(_handler.GetValue<BaseState>("State_Combat"));
            return;
        }
        #endregion

        if(!_handler.GetValue<bool>("B_ViewContainsThreat") && _handler.GetValue<bool>("B_DestinationReach"))
        {
            // Find a random position within the searchRange
            SetNavDestination(FindRandomSearchPosition(startPosition, searchRange));
        }
    }

    public override void Exit()
    {
        base.Exit();
        SetNavDestination(startPosition);
    }

    #endregion

    #region Custom Methods

    // Find a random position within the searchRange
    Vector3 FindRandomSearchPosition(Vector3 origin, float range)
    {
        return origin + Random.insideUnitSphere * range;
    }

    // Sets a new destination for the character to reach
    void SetNavDestination(Vector3 desiredPosition)
    {
        _handler.SetValue("V_Destination", desiredPosition);
    }

    #endregion
}
