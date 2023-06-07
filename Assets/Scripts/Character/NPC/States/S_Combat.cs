using Brad.Character;
using Brad.FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Combat : BaseState
{
    NPC_Controller fsMachine;
    private EventAndDataManager mang;
    IDamagable myDmg, targetDmg;
    Transform target;
    float _attackDistance = 2.5f;
    float _attackDelay = 1f;

    bool attacking;
    float lastAttackTime;
    float startTargetLost;

    public S_Combat(NPC_Controller stateMachine) : base("Combat", stateMachine)
    {
        fsMachine = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        fsMachine.transform.TryGetComponent<IDamagable>(out myDmg);
        target = mang.GetValue<Transform>("CombatTarget");
    }

    public override void UpdateState()
    {
        #region Transitions
        // -> Despawn
        if (mang.GetValue<bool>("b_OutOfRangeToPlayer"))
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
        if (mang.GetValue<int>("i_ThreatsInProxNum") == 0) // Need to change this to when all threats are dead
        {
            fsMachine.ChangeState(fsMachine.idleState);
            return;
        }

        // -> Flee
        if(mang.GetValue<bool>("b_FearLevelMax"))
        {
            fsMachine.ChangeState(fsMachine.fleeState);
            return;
        }

        // -> Search
        if(mang.GetValue<int>("i_ThreatsInViewNum") == 0)
        {
            fsMachine.ChangeState(fsMachine.searchState);
            return;
        }

        #endregion

        // Change target if closest threat has changed.
        target = mang.GetValue<Transform>("CombatTarget");

        if (target != null)
        {
            if (mang.GetValue<string>("s_CurrentAnimState") != "Attack")
            {
                mang.TriggerEvent("StopMoving");

                if(!attacking && Vector3.Distance(fsMachine.transform.position, target.transform.position) < _attackDistance)
                {
                    if((Time.time - lastAttackTime) > _attackDelay && !attacking)
                    {
                        mang.TriggerEvent("Attack");
                        lastAttackTime = Time.time;
                        attacking = true;
                        return;
                    }
                }
            }

            else
            {
                mang.TriggerEvent("FollowCombatTarget");
                attacking = false;
            }
        }
    }

    public override void Exit()
    {
        mang.TriggerEvent("StopMoving");
        mang.TriggerEvent("StopLooking");
        base.Exit();
    }
}
