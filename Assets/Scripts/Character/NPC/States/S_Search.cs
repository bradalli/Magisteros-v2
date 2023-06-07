using Brad.Character;
using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class S_Search : BaseState
{
    private NPC_Controller _cont;

    Vector3 startPosition;
    float searchTimeLength = 20f;
    float enterTime;

    float searchRange = 20f;
    Vector3 searchPosition;

    int sampleAttempts = 0;

    public S_Search(NPC_Controller stateMachine) : base("Search", stateMachine)
    {
        _cont = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        enterTime = Time.time;
        startPosition = _cont.transform.position;

        // Find a random position within the searchRange and sample it from the nav mesh.
        Vector3 randomPosition = startPosition + Random.insideUnitSphere * searchRange;
        SetNavPosition(randomPosition);
    }

    public override void UpdateState()
    {
        float timePassed = Time.time - enterTime;

        #region Transitions
        // -> Despawn
        if (_cont.Get_IsNpcOutOfRange())
        {
            _cont.ChangeState(_cont.despawnState);
            return;
        }

        // -> Idle
        if (timePassed > searchTimeLength)
        {
            _cont.ChangeState(_cont.idleState);
            return;
        }

        // -> Combat
        if (_cont.Get_ThreatsInViewNum() > 0)
        {
            _cont.ChangeState(_cont.combatState);
            return;
        }
        #endregion

        //Debug.Log(_cont.Get_RemainingNavDistance());

        if(_cont.Get_NavReachedDestination())
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
        _cont.Set_NavDestination(desiredPosition);
    }
}
