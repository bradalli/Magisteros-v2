using Brad.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class C_Move : MonoBehaviour
{
    [SerializeField] float lookSpeed = 5f;

    private IEventAndDataHandler _handler;
    private NavMeshAgent navAgent;
    private Transform meshTransform;

    public Waypoint currentWaypoint;

    bool lookAtTarget = false;
    private Vector3 targetLookDir = Vector3.forward;
    public Vector3 lookAtPosition;
    private Transform lookTarget;
    

    private void OnEnable()
    {
        TryGetComponent<IEventAndDataHandler>(out _handler);
        TryGetComponent<NavMeshAgent>(out navAgent);
        if (_handler != null)
        {
            //npcCont.d_CurrentWaypoint += ReturnCurrentWaypoint;
            _handler.AddEvent("Update_CurrWp", Update_CurrentWaypoint);

            if (navAgent != null)
            {
                // Update data initialisation
                _handler.AddEvent("Update_MeshTransform", Update_MeshTransform);
                _handler.AddEvent("Update_NavAgentInfo", Update_NavAgentDestInfo);
                _handler.AddEvent("Update_NavVelocity", Update_NavAgentVelocity);
                _handler.AddEvent("Update_NavDestination", Update_NavAgentDestination);
                _handler.AddEvent("Update_LookTarget", Update_LookTarget);

                // Event initialisation
                _handler.AddEvent("Start_NavMove", Start_NavMove);
                _handler.AddEvent("Stop_NavMove", Stop_NavMove);
                _handler.AddEvent("Start_LookAtTarget", Start_LookingAtTarget);
                _handler.AddEvent("Stop_LookAtTarget", Stop_LookingAtTarget);
            }
        }

        else
            Debug.LogError(_handler.GetType().ToString() + " could not be found");
    }

    private void Update()
    {
        Look();
    }

    void Look()
    {
        if (lookAtPosition == Vector3.zero && navAgent.velocity.normalized.magnitude > 0)
            targetLookDir = Vector3.RotateTowards(meshTransform.forward, navAgent.velocity.normalized, lookSpeed * Time.deltaTime, 0);

        if(lookAtPosition != Vector3.zero)
            targetLookDir = Vector3.RotateTowards(meshTransform.forward, lookAtPosition, lookSpeed * Time.deltaTime, 1);

        // Ensure targetLookDir.y is 0
        targetLookDir = new Vector3(targetLookDir.x, 0, targetLookDir.z);

        meshTransform.rotation = Quaternion.LookRotation(targetLookDir, meshTransform.up);
    }

    #region Update data methods

    void Update_MeshTransform() => meshTransform = _handler.GetValue<Transform>("MeshTransform");
    void Update_CurrentWaypoint() => _handler.SetValue("Waypoint_Current", currentWaypoint);
    void Update_NavAgentDestInfo()
    {
        _handler.SetValue("Nav_ReachedDest", navAgent.remainingDistance <= navAgent.stoppingDistance);
        _handler.SetValue("Nav_RemainingDist", navAgent.remainingDistance);
    }
    void Update_NavAgentVelocity() => _handler.SetValue("Nav_Velocity", navAgent.velocity);
    void Update_NavAgentDestination() => navAgent.destination = _handler.GetValue<Vector3>("Nav_Destination");
    void Update_LookTarget() => lookTarget = _handler.GetValue<Transform>("TargetTransform");

    #endregion

    #region Event methods

    void Start_NavMove() => navAgent.isStopped = false;
    void Stop_NavMove() => navAgent.isStopped = true;
    void Start_LookingAtTarget() => lookAtTarget = true;
    void Stop_LookingAtTarget() => lookAtTarget = false;

    #endregion
}
