using Brad.FSM;

public class S_Idle : BaseState
{
    #region Private Variables

    private StateMachine _cont;
    private IEventAndDataHandler _handler;

    #endregion

    #region State Methods

    public override void Enter()
    {
        // Cache components
        _cont = stateMachine;
        _cont.TryGetComponent(out _handler);
        
        // Trigger events
        _handler.TriggerEvent("Stop_Move");
        _handler.TriggerEvent("Stop_LookAt");

        base.Enter();
    }

    public override void UpdateState()
    {
        #region Transitions
        // -> Despawn
        if (!_handler.GetValue<bool>("B_InRangeOfPlayer"))
        {
            _cont.ChangeState(_handler.GetValue<BaseState>("State_Despawn"));
            return;
        }

        // -> Alert
        if(_handler.GetValue<bool>("B_ProxContainsThreat"))
        {
            _cont.ChangeState(_handler.GetValue<BaseState>("State_Alert"));
            return;
        } // !!REMINDER!! - Need to add function for other allies in proximity to warn this npc.

        // -> Move
        if (_handler.GetValue<Waypoint>("W_CurrWp") != null)
        {
            _cont.ChangeState(_handler.GetValue<BaseState>("State_Move"));
            return;
        }

        #endregion
    }
    public override void Exit()
    {
        base.Exit();
    }

    #endregion
}

