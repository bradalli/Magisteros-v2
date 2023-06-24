using Brad.FSM;

public class S_Spawn : BaseState
{
    #region Private variables

    private StateMachine fsMachine;
    private IEventAndDataHandler _handler;
    private IDamagable _dmg;

    #endregion

    #region State methods
    public override void Enter()
    {
        // Cache components
        fsMachine = stateMachine;
        fsMachine.TryGetComponent(out _handler);
        fsMachine.TryGetComponent(out _dmg);

        // Trigger events
        _handler.TriggerEvent("Disable_C");
        if (_dmg.Health <= 0) 
            _handler.TriggerEvent("Respawn");

        base.Enter();
    }

    public override void UpdateState()
    {
        // Only advance if npc is within range of player
        // -> Idle
        if (_handler.GetValue<bool>("B_InRangeOfPlayer"))
        {
            _handler.TriggerEvent("Enable_C");
            
            if (_dmg != null)
            {
                _dmg.ResetHealth();
            }

            fsMachine.ChangeState(_handler.GetValue<BaseState>("State_Idle"));
        }
    }

    public override void Exit()
    {
        base.Exit();  
    }
    #endregion
}
