using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Combat : BaseState
{
    private NPC_Controller _cont;
    IDamagable _contDmg;
    private Collider _target;
    IDamagable _targetDmg;
    float _attackDistance = 2.5f;
    float _attackDelay = 1f;

    bool attacking;
    float lastAttackTime;
    float startTargetLost;

    public S_Combat(NPC_Controller stateMachine) : base("Combat", stateMachine)
    {
        _cont = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        _target = ReturnTarget();

        _cont.TryGetComponent<IDamagable>(out _contDmg);
        if (_target != null)
            _target.TryGetComponent<IDamagable>(out _targetDmg);
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

        // -> Dead
        if (_contDmg != null)
        {
            if (_contDmg.Health == 0)
            {
                _cont.ChangeState(_cont.deadState);
            }
        }
        
        // -> Idle
        if (_cont.Get_ThreatsInProxNum() == 0) // Need to change this to when all threats are dead
        {
            _cont.ChangeState(_cont.idleState);
            return;
        }

        // -> Flee
        if(_cont.Get_IsFearOverMax())
        {
            _cont.ChangeState(_cont.fleeState);
            return;
        }

        // -> Search
        if(_cont.Get_ThreatsInViewNum() == 0)
        {
            _cont.ChangeState(_cont.searchState);
            return;
        }

            #endregion

        // Change target if closest threat has changed.
        if(ReturnTarget() != null)
        {
            if (ReturnTarget() != _target)
                _target = ReturnTarget();
        }

        if(_target != null)
        {
            
            if (!_cont.Get_IsCurrAnimStateThis("Attack", 0) && !(_cont.Get_IsCurrAnimTransToThis("Attack", 0) ^ _cont.Get_IsCurrAnimTransFromThis("Attack", 0)))
            {
                _cont.Set_NavDestination(_target.transform.position);

                if(!attacking && Vector3.Distance(_cont.transform.position, _target.transform.position) < _attackDistance)
                {
                    if((Time.time - lastAttackTime) > _attackDelay && !attacking)
                    {
                        _cont.Set_AnimTrigger("tAttack");
                        Debug.Log($"LAT({lastAttackTime}s)... CAT({Time.time}s)... Diff({Time.time - lastAttackTime}s)... Delay({_attackDelay}s)");
                        lastAttackTime = Time.time;
                        attacking = true;
                        return;
                    }
                }
            }

            else
            {
                _cont.Set_ResetTrigger("tAttack");
                _cont.Set_NavDestination(_cont.transform.position);
                attacking = false;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        _cont.Set_NavDestination(_cont.transform.position);
        _cont.Set_LookAtPosition(_cont.transform.forward);
    }

    public Collider ReturnTarget()
    {
        Collider _pickTarget = null;

        if (_cont.Get_ThreatsInViewNum() > 0)
            _pickTarget = _cont.Get_ClosestThreatInView();

        else
            _pickTarget = _cont.Get_ClosestThreatInProx();

        return _pickTarget;
    }
}
