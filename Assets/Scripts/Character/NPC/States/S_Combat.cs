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
        base.Enter();

        fsMachine.transform.TryGetComponent<IDamagable>(out myDmg);
        target = _handler.GetValue<Transform>("CombatTarget");
    }

    public override void UpdateState()
    {
        #region Transitions
        // -> Despawn
        if (_handler.GetValue<bool>("b_OutOfRangeToPlayer"))
        {
            fsMachine.ChangeState(fsMachine.despawnState);
            return;
        }

        // -> Dead
        if (myDmg != null)
        {
            if (myDmg.Health == 0)
            {
                fsMachine.ChangeState(fsMachine.deadState);
            }
        }
        
        // -> Idle
        if (_handler.GetValue<int>("i_ThreatsInProxNum") == 0) // Need to change this to when all threats are dead
        {
            fsMachine.ChangeState(fsMachine.idleState);
            return;
        }

        // -> Flee
        if(_handler.GetValue<bool>("b_FearLevelMax"))
        {
            fsMachine.ChangeState(fsMachine.fleeState);
            return;
        }

        // -> Search
        if(_handler.GetValue<int>("i_ThreatsInViewNum") == 0)
        {
            fsMachine.ChangeState(fsMachine.searchState);
            return;
        }

        #endregion

        // Change target if closest threat has changed.
        target = _handler.GetValue<Transform>("CombatTarget");

        if (target != null)
        {
            if (_handler.GetValue<string>("s_CurrentAnimState") != "Attack")
            {
                _handler.TriggerEvent("StopMoving");

                if(!attacking && Vector3.Distance(fsMachine.transform.position, target.transform.position) < _attackDistance)
                {
                    if((Time.time - lastAttackTime) > _attackDelay && !attacking)
                    {
                        _handler.TriggerEvent("Attack");
                        lastAttackTime = Time.time;
                        attacking = true;
                        return;
                    }
                }
            }

            else
            {
                _handler.TriggerEvent("FollowCombatTarget");
                attacking = false;
            }
        }
    }

    public override void Exit()
    {
        _handler.TriggerEvent("StopMoving");
        _handler.TriggerEvent("StopLooking");
        base.Exit();
    }
}
