using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Spawn : BaseState
{
    #region Private variables

    private StateMachine fsMachine;
    private IEventAndDataHandler _handler;
    private IDamagable _dmg;

    #endregion

    #region State methods
    /*
    public S_Spawn(StateMachine stateMachine) : base("Spawn", stateMachine)
    {
        fsMachine = stateMachine;
        fsMachine.TryGetComponent(out _dmg);
    }*/
    public override void Enter()
    {
        fsMachine = stateMachine;
        fsMachine.TryGetComponent(out _handler);

        base.Enter();
        _handler.TriggerEvent("Disable_C");
        fsMachine.TryGetComponent(out _dmg);
        if (_dmg.Health <= 0) 
            _handler.TriggerEvent("Respawn");
    }

    public override void UpdateState()
    {
        // Refresh value in case it has changed
        //_handler.TriggerEvent("Get_B_InRangeOfPlayer");
        
        // Retrieve value from handler and only advance if npc is within range of player
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
