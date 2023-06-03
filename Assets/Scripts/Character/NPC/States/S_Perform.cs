using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Perform : BaseState
{
    private NPC_Controller _cont;
    private CharacterAction _action;
    public S_Perform(NPC_Controller stateMachine) : base("Peform", stateMachine)
    {
        _cont = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        _action = _cont.Get_CurrAction();

        if (_action.position != Vector3.zero)
            _cont.Set_NavDestination(_cont.Get_CurrAction().position);

        else
            _cont.Set_NavDestination(_cont.transform.position);

        if (_action != null)
            _action.Start();
    }

    public override void UpdateState()
    {
        #region Transitions
        // -> Despawn
        if (_cont.Get_IsNpcOutOfRange())
        {
            _cont.ChangeState(_cont.despawnState);
            return;
        }

        // -> Alert
        if (_cont.Get_ThreatsInProxNum() > 0)
        {
            _cont.ChangeState(_cont.alertState);
            return;
        }

        // -> Idle
        if (_cont.Get_CurrAction() == null)
        {
            _cont.ChangeState(_cont.idleState);
            return;
        }
        #endregion

        if (_action != null)
            _action.UpdateAction();

        if(_action.actionState == CharacterAction.state.Complete)
        {
            _cont.Set_ActionEnd();
        }
    }

    public override void Exit()
    {
        base.Exit();

        if (_action != null)
            _action.Stop();
    }
}
