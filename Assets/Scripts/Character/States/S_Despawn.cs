using Brad.FSM;

public class S_Despawn : BaseState
{
    #region Private Variables

    private StateMachine _cont;
    IEventAndDataHandler _handler;

    #endregion

    #region State methods
    public override void Enter()
    {
        _cont = stateMachine;
        _cont.TryGetComponent(out _handler);
        base.Enter();
        _handler.TriggerEvent("Disable_C");
    }

    public override void UpdateState()
    {
        _cont.ChangeState(_handler.GetValue<BaseState>("State_Spawn"));
    }

    public override void Exit()
    {
        base.Exit();
    }
    #endregion
}
