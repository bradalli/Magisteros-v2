using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Despawn : BaseState
{
    private NPC_Controller _cont;
    public S_Despawn(NPC_Controller stateMachine) : base("Spawn", stateMachine)
    {
        _cont = stateMachine;
    }

    #region State methods
    public override void Enter()
    {
        base.Enter();
        EnableNPC(false);
    }

    public override void UpdateState()
    {
        _cont.ChangeState(_cont.spawnState);
    }

    public override void Exit()
    {
        base.Exit();
    }
    #endregion

    #region Custom methods
    void EnableNPC(bool value)
    {
        foreach (MonoBehaviour comp in _cont.gameObject.GetComponents<MonoBehaviour>())
        {
            if (comp != _cont)
            {
                comp.enabled = value;
            }
        }
    }
    #endregion
}
