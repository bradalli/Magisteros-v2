using Brad.Character;
using Brad.FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Combat : BaseState
{
    StateMachine fsMachine;
    private IEventAndDataHandler _handler;
    IDamagable myDmg, targetDmg;
    Transform target;
    float _attackDistance = 2.5f;
    float _attackDelay = 1f;

    bool attacking;
    float lastAttackTime;
    float startTargetLost;
    /*
    public S_Combat(StateMachine stateMachine) : base("Combat", stateMachine)
    {
        fsMachine = stateMachine;
    }*/

    public override void Enter()
    {
        fsMachine = stateMachine;
        fsMachine.TryGetComponent(out _handler);
        base.Enter();

        fsMachine.transform.TryGetComponent(out myDmg);
        target = _handler.GetValue<Transform>("T_ClosestThreatInView");

        //_handler.SetValue("T_LookTarget", target);
        _handler.SetValue("T_FollowTarget", target);

        _handler.TriggerEvent("Start_Move");
        //_handler.TriggerEvent("Start_LookAt");
        _handler.TriggerEvent("Start_Combat");

        _handler.AddEvent("Stop_Attack", SetLastAttackTime);
    }

    public override void UpdateState()
    {
        #region Transitions
        // -> Despawn
        if (!_handler.GetValue<bool>("B_InRangeOfPlayer"))
        {
            fsMachine.ChangeState(_handler.GetValue<BaseState>("State_Despawn"));
            return;
        }

        // -> Dead
        if (myDmg != null)
        {
            if (myDmg.Health == 0)
            {
                fsMachine.ChangeState(_handler.GetValue<BaseState>("State_Dead"));
            }
        }
        
        // -> Idle
        if (!_handler.GetValue<bool>("B_ProxContainsThreat")) // Need to change this to when all threats are dead
        {
            fsMachine.ChangeState(_handler.GetValue<BaseState>("State_Idle"));
            return;
        }

        // -> Flee
        if(_handler.GetValue<bool>("B_IsFearful"))
        {
            fsMachine.ChangeState(_handler.GetValue<BaseState>("State_Flee"));
            return;
        }

        // -> Search
        if(!_handler.GetValue<bool>("B_ViewContainsThreat"))
        {
            fsMachine.ChangeState(_handler.GetValue<BaseState>("State_Search"));
            return;
        }

        #endregion

        // Change target if closest threat has changed.
        Transform tmpTarget = _handler.GetValue<Transform>("T_ClosestThreatInView");
        if(target != tmpTarget)
        {
            _handler.SetValue("T_LookTarget", target);
            _handler.SetValue("T_FollowTarget", target);
            target = tmpTarget;
        }

        if (target != null)
        {
            if (Vector3.Distance(fsMachine.transform.position, target.transform.position) < _attackDistance)
            {
                if ((Time.time - lastAttackTime) > _attackDelay && !_handler.GetValue<bool>("B_Attacking"))
                {
                    _handler.TriggerEvent("Stop_Move");
                    _handler.TriggerEvent("Start_Attack");
                }
            }

            else if (!_handler.GetValue<bool>("B_Moving") && !_handler.GetValue<bool>("B_Attacking"))
            {
                _handler.TriggerEvent("Start_Move");
                //_handler.TriggerEvent("Start_LookAt");
            }
        }
    }

    void SetLastAttackTime()
    {
        lastAttackTime = Time.time;
    }

    public override void Exit()
    {
        _handler.RemoveEvent("Stop_Attack", SetLastAttackTime);
        _handler.TriggerEvent("Stop_Move");
        //_handler.TriggerEvent("Stop_LookAt");
        _handler.TriggerEvent("Stop_Combat");
        base.Exit();
    }
}
