using Brad.FSM;
using UnityEngine;

public class S_Search : BaseState
{
    private StateMachine _cont;
    private IEventAndDataHandler _handler;

    Vector3 startPosition;
    float searchTimeLength = 20f;
    float enterTime;

    float searchRange = 20f;
    Vector3 searchPosition;


    public override void Enter()
    {
        _cont = stateMachine;
        _cont.TryGetComponent(out _handler);
        base.Enter();
        enterTime = Time.time;
        startPosition = _cont.transform.position;

        _cont.TryGetComponent(out _handler);

        _handler.TriggerEvent("Start_Move");

        // Find a random position within the searchRange and sample it from the nav mesh.
        Vector3 randomPosition = startPosition + Random.insideUnitSphere * searchRange;
        SetNavPosition(randomPosition);
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
            // Find a random position within the searchRange and sample it from the nav mesh.
            Vector3 randomPosition = startPosition + Random.insideUnitSphere * searchRange;
            SetNavPosition(randomPosition);
        }
    }

    public override void Exit()
    {
        base.Exit();
        SetNavPosition(startPosition);
    }

    void SetNavPosition(Vector3 desiredPosition)
    {
        _handler.SetValue("V_Destination", desiredPosition);
    }
}
