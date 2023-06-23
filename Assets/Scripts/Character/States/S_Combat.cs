using Brad.FSM;
using UnityEngine;
using Random = UnityEngine.Random;

public class S_Combat : BaseState
{
    #region Private Variables

    StateMachine fsMachine;
    private IEventAndDataHandler _handler;
    IDamagable myDmg;
    Transform target;
    float _attackDistance = 2f;
    float _attackDelay = 0.5f;
    float lastAttackTime;

    #endregion

    #region State methods

    public override void Enter()
    {
        fsMachine = stateMachine;
        fsMachine.TryGetComponent(out _handler);
        fsMachine.transform.TryGetComponent(out myDmg);
        
        target = _handler.GetValue<Transform>("T_ClosestThreatInView");

        _handler.SetValue("T_LookTarget", target);
        _handler.SetValue("T_FollowTarget", target);

        _handler.TriggerEvent("Start_Move");
        _handler.TriggerEvent("Start_LookAt");
        _handler.TriggerEvent("Start_Combat");

        _handler.AddEvent("Stop_Attack", SetLastAttackTime);

        base.Enter();
    }

    public override void UpdateState()
    {
        #region Transitions
        // -> Despawn (Despawn when out of range to the player)
        if (!_handler.GetValue<bool>("B_InRangeOfPlayer"))
        {
            fsMachine.ChangeState(_handler.GetValue<BaseState>("State_Despawn"));
            return;
        }

        // -> Dead (Die when health reaches zero or less than zero)
        if (myDmg != null)
        {
            if (myDmg.IsHealthDepleted())
            {
                fsMachine.ChangeState(_handler.GetValue<BaseState>("State_Dead"));
            }
        }
        
        // -> Idle (Go idle when proximity no longer contains a threat)
        if (!_handler.GetValue<bool>("B_ProxContainsThreat"))
        {
            fsMachine.ChangeState(_handler.GetValue<BaseState>("State_Idle"));
            return;
        }

        // -> Flee (Go flee if fear level reaches or exceeds max fear)
        if(_handler.GetValue<bool>("B_IsFearful"))
        {
            fsMachine.ChangeState(_handler.GetValue<BaseState>("State_Flee"));
            return;
        }

        // -> Search (Go search if view no longer contains threat)
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
            // Only attack if target is within attackDistance AND attackDelay has elapsed since Last Attack AND character isn't currently attacking
            if (Vector3.Distance(fsMachine.transform.position, target.transform.position) < _attackDistance)
            {
                if ((Time.time - lastAttackTime) > _attackDelay && !_handler.GetValue<bool>("B_Attacking"))
                {
                    _handler.TriggerEvent("Stop_Move");
                    _handler.TriggerEvent("Start_Attack");
                }
            }

            // If previous is false AND character is not moving AND character is not attacking
            else if (!_handler.GetValue<bool>("B_Moving") && !_handler.GetValue<bool>("B_Attacking"))
            {
                _handler.TriggerEvent("Start_Move");
            }
        }
    }

    public override void Exit()
    {
        // Reset events and variables
        _handler.RemoveEvent("Stop_Attack", SetLastAttackTime);
        _handler.TriggerEvent("Stop_Move");
        _handler.TriggerEvent("Stop_LookAt");
        _handler.TriggerEvent("Stop_Combat");
        _handler.SetValue<Transform>("T_FollowTarget", null);
        _handler.SetValue<Transform>("T_LookTarget", null);

        base.Exit();
    }

    #endregion

    #region Custom methods

    void SetLastAttackTime()
    {
        // Set last attack time to current time with an added random variation
        lastAttackTime = Time.time + Random.Range(0, .25f);
    }

    #endregion
}
