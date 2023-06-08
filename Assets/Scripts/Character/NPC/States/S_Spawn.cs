using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Spawn : BaseState
{
    #region Private variables

    private NPC_Controller fsMachine;
    private IEventAndDataHandler _handler;
    private IDamagable _dmg;

    #endregion

    #region State methods

    public S_Spawn(NPC_Controller stateMachine) : base("Spawn", stateMachine)
    {
        fsMachine = stateMachine;
        fsMachine.TryGetComponent(out _dmg);
    }
    public override void Enter()
    {
        base.Enter();
        _handler.TriggerEvent("Disable");
        _handler.TriggerEvent("Trigger_AnimRespawn");
    }

    public override void UpdateState()
    {
        // Refresh value in case it has changed
        _handler.TriggerEvent("Refresh_InRangeOfPlayerB");
        
        // Retrieve value from handler and only advance if npc is within range of player
        if (!_handler.GetValue<bool>("InRangeOfPlayerB"))
        {
            _handler.TriggerEvent("Enable");
            
            if (_dmg != null)
            {
                _dmg.ResetHealth();
            }

            fsMachine.ChangeState(fsMachine.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();  
    }
    #endregion
}
