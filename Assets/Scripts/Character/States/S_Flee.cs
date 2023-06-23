using Brad.FSM;
using UnityEngine;

public class S_Flee : BaseState
{
    #region Private Variables

    private StateMachine _cont;
    private IEventAndDataHandler _handler;
    private Vector3 fleeDirection;
    IDamagable myDmg;

    #endregion

    #region State Methods

    public override void Enter()
    {
        // Cache components
        _cont = stateMachine;
        _cont.TryGetComponent(out _handler);
        _cont.transform.TryGetComponent(out myDmg);

        // Calculate target flee direction
        fleeDirection = (_cont.transform.position - _handler.GetValue<Vector3>("V_ProxThreatsAvgPosition")).normalized;
        _handler.TriggerEvent("Start_Move");

        base.Enter();
    }

    public override void UpdateState()
    {
        #region Transitions
        // -> Despawn (Despawn when out of range to the player)
        if (!_handler.GetValue<bool>("B_InRangeOfPlayer"))
        {
            _cont.ChangeState(_handler.GetValue<BaseState>("State_Despawn"));
            return;
        }

        // -> Dead (If health is depleted)
        if (myDmg != null)
        {
            if (myDmg.IsHealthDepleted())
            {
                _cont.ChangeState(_handler.GetValue<BaseState>("State_Dead"));
            }
        }

        // -> Idle (If proximity no longer contains a threat)
        if (!_handler.GetValue<bool>("B_ProxContainsThreat"))
        {
            _cont.ChangeState(_handler.GetValue<BaseState>("State_Idle"));
            return;
        }

        // -> Combat (If fear level exceeds max)
        if (!_handler.GetValue<bool>("B_IsFearful"))
        {
            _cont.ChangeState(_handler.GetValue<BaseState>("State_Idle"));
            return;
        }
        #endregion

        // Update flee direction
        fleeDirection = (_cont.transform.position - _handler.GetValue<Vector3>("V_ProxThreatsAvgPosition")).normalized;
        Vector3 fleePosition = _cont.transform.position + (fleeDirection * _handler.GetValue<float>("F_FleeDistance"));
        _handler.SetValue("V_Destination", fleePosition);
    }

    public override void Exit()
    {
        _handler.TriggerEvent("Stop_Move");
        base.Exit();
    }

    #endregion
}

