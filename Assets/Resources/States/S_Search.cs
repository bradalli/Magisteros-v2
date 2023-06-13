using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class S_Search : BaseState
{
    private StateMachine _cont;
    private IEventAndDataHandler _handler;

    Vector3 startPosition;
    float searchTimeLength = 20f;
    float enterTime;

    float searchRange = 20f;
    Vector3 searchPosition;

    //int sampleAttempts = 0;
    /*
    public S_Search(StateMachine stateMachine) : base("Search", stateMachine)
    {
        _cont = stateMachine;
    }*/

    public override void Enter()
    {
        _cont = stateMachine;
        _cont.TryGetComponent(out _handler);
        base.Enter();
        enterTime = Time.time;
        startPosition = _cont.transform.position;

        _cont.TryGetComponent(out _handler);

        _handler.TriggerEvent("Start_Move");

        // Find a random position within the searchRange and sample it from the nav mesh.
        Vector3 randomPosition = startPosition + Random.insideUnitSphere * searchRange;
        SetNavPosition(randomPosition);
    }

    public override void UpdateState()
    {
        float timePassed = Time.time - enterTime;

        #region Transitions
        // -> Despawn
        _handler.TriggerEvent("Refresh_InRangeToPlayerB");
        if (!_handler.GetValue<bool>("InRangeToPlayerB"))
        {
            _cont.ChangeState(_handler.GetValue<BaseState>("State_Despawn"));
            return;
        }

        // -> Idle
        if (timePassed > searchTimeLength)
        {
            _cont.ChangeState(_handler.GetValue<BaseState>("State_Idle"));
            return;
        }

        // -> Combat
        if (_handler.GetValue<bool>("B_ViewContainsThreat"))
        {
            _cont.ChangeState(_handler.GetValue<BaseState>("State_Combat"));
            return;
        }
        #endregion

        //Debug.Log(_cont.Get_RemainingNavDistance());
        if(_handler.GetValue<bool>("B_ViewContainsThreat"))
        {
            // Find a random position within the searchRange and sample it from the nav mesh.
            Vector3 randomPosition = startPosition + Random.insideUnitSphere * searchRange;
            SetNavPosition(randomPosition);
        }
    }

    public override void Exit()
    {
        base.Exit();
        SetNavPosition(startPosition);
    }

    void SetNavPosition(Vector3 desiredPosition)
    {
        /*
        NavMeshHit hit;
        if (NavMesh.SamplePosition(desiredPosition, out hit, 10f, NavMesh.AllAreas))
        {
            searchPosition = hit.position;
            _cont.Set_NavDestination(searchPosition);
            sampleAttempts = 0;
        }

        else
        {
            Debug.LogError("Navmesh not found in sample attempt");
            sampleAttempts++;

            if (sampleAttempts < 100)
                SetNavPosition(desiredPosition);

            else
                Debug.LogError("NavMesh sample attempts limit reached");
        }*/
        //_cont.Set_NavDestination(desiredPosition);
        _handler.SetValue("V_Destination", desiredPosition);
    }
}
