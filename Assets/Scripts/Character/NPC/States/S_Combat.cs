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

    public S_Combat(StateMachine stateMachine) : base("Combat", stateMachine)
    {
        fsMachine = stateMachine;
    }

    public override void Enter()
    {
        fsMachine.TryGetComponent<IEventAndDataHandler>(out _handler);
        base.Enter();

        fsMachine.transform.TryGetComponent(out myDmg);
        target = _handler.GetValue<Transform>("T_ClosestThreatInView");


        _handler.TriggerEvent("Start_Move");
        _handler.TriggerEvent("Start_LookAt");
        _handler.TriggerEvent("Start_Combat");
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
        target = _handler.GetValue<Transform>("T_ClosestThreatInView");

        if (target != null)
        {
            if (!_handler.GetValue<bool>("B_Attacking"))
            {
                _handler.TriggerEvent("Stop_Move");

                if(!attacking && Vector3.Distance(fsMachine.transform.position, target.transform.position) < _attackDistance)
                {
                    if((Time.time - lastAttackTime) > _attackDelay && !attacking)
                    {
                        _handler.TriggerEvent("Start_Attack");
                        lastAttackTime = Time.time;
                        return;
                    }
                }
            }

            else
            {
                _handler.TriggerEvent("Start_Move");
                attacking = false;
            }
        }
    }

    public override void Exit()
    {
        _handler.TriggerEvent("Stop_Move");
        _handler.TriggerEvent("Stop_LookAt");
        _handler.TriggerEvent("Stop_Combat");
        base.Exit();
    }
}
