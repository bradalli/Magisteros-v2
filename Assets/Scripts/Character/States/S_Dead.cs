using Brad.FSM;

public class S_Dead : BaseState
{
    #region Private Variables

    private StateMachine _cont;
    private IEventAndDataHandler _handler;

    #endregion

    #region State Methods

    public override void Enter()
    {
        _cont = stateMachine;
        _cont.TryGetComponent(out _handler);

        base.Enter();

        _handler.TriggerEvent("Die");
        _handler.TriggerEvent("Disable_C");
    }

    #endregion
}

