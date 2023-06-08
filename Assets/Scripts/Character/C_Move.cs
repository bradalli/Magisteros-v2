using Brad.Character;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class C_Move : MonoBehaviour
{
    #region Private variables
    float lookSpeed = 5f;

    private IEventAndDataHandler _handler;
    private NavMeshAgent navAgent;
    private Transform mesh;

    private Waypoint currentWaypoint;

    bool lookAtTargetB = false;
    private Vector3 lookAtDir = Vector3.forward;
    private Vector3 targetLookDir;
    private Transform lookTargetT;
    #endregion

    #region MonoBehaviour

    private void OnEnable()
    {
        // Cache components
        TryGetComponent(out _handler);
        TryGetComponent(out navAgent);

        // Only continue if components are found
        if (_handler != null && navAgent != null)
        {
            // Update data initialisation
            _handler.AddEvent("Refresh_LookSpeedF", Refresh_LookSpeedF);
            _handler.AddEvent("Refresh_MeshT", Refresh_MeshT);
            _handler.AddEvent("Refresh_DestinationReachB", Refresh_DestinationReachB);
            _handler.AddEvent("Refresh_VelocityV", Refresh_VelocityV);
            _handler.AddEvent("Refresh_Destination", Refresh_Destination);
            _handler.AddEvent("Refresh_LookTargetT", Refresh_LookTargetT);
            _handler.AddEvent("Refresh_CurrWp", Refresh_CurrWp);

            // Event initialisation
            _handler.AddEvent("Start_Move", Start_Move);
            _handler.AddEvent("Stop_Move", Stop_Move);
            _handler.AddEvent("Do_GoToWp", Do_GotoWp);
            _handler.AddEvent("Start_LookAtTarget", Start_LookAtTarget);
            _handler.AddEvent("Stop_LookAtTarget", Stop_LookAtTarget);
        }

        else
            Debug.LogError(_handler.GetType().ToString() + " could not be found");
    }
    private void OnDisable()
    {
        // Only continue if components are found
        if (_handler != null && navAgent != null)
        {
            // Update data removal
            _handler.RemoveEvent("Refresh_LookSpeedF", Refresh_LookSpeedF);
            _handler.RemoveEvent("Refresh_MeshT", Refresh_MeshT);
            _handler.RemoveEvent("Refresh_NavAgentInfo", Refresh_DestinationReachB);
            _handler.RemoveEvent("Refresh_NavAgentVel", Refresh_VelocityV);
            _handler.RemoveEvent("Refresh_NavAgentDestination", Refresh_Destination);
            _handler.RemoveEvent("Refresh_LookTargetT", Refresh_LookTargetT);
            _handler.RemoveEvent("Update_CurrWp", Refresh_CurrWp);

            // Event removal
            _handler.RemoveEvent("Start_Move", Start_Move);
            _handler.RemoveEvent("Stop_Move", Stop_Move);
            _handler.RemoveEvent("Do_GoToWp", Do_GotoWp);
            _handler.RemoveEvent("Start_LookAtTarget", Start_LookAtTarget);
            _handler.RemoveEvent("Stop_LookAtTarget", Stop_LookAtTarget);
        }

        else
            Debug.LogError(_handler.GetType().ToString() + " could not be found");
    }
    private void Update()
    {
        // Make the mesh of the character look at a direction every frame
        Look();
    }

    #endregion

    #region Custom methods

    #region Refresh data methods

    void Refresh_LookSpeedF() => lookSpeed = _handler.GetValue<float>("LookSpeedF");
    void Refresh_MeshT() => mesh = _handler.GetValue<Transform>("MeshT");
    void Refresh_CurrWp() => _handler.SetValue("CurrWp", currentWaypoint);
    void Refresh_DestinationReachB() =>_handler.SetValue("DestinationReachB", 
        navAgent.remainingDistance <= navAgent.stoppingDistance);
    void Refresh_VelocityV() => _handler.SetValue("VelocityV", navAgent.velocity);
    void Refresh_Destination() => navAgent.destination = _handler.GetValue<Vector3>("DestinationV");
    void Refresh_LookTargetT() => lookTargetT = _handler.GetValue<Transform>("TargetT");

    #endregion

    #region Event methods

    void Start_Move() => navAgent.isStopped = false;
    void Stop_Move() => navAgent.isStopped = true;
    void Do_GotoWp()
    {
        if (currentWaypoint != null)
            navAgent.destination = currentWaypoint.transform.position;

        else
            Debug.LogError("Unable to go to CurrWp because it is null.");
    }
    void Start_LookAtTarget() => lookAtTargetB = true;
    void Stop_LookAtTarget() => lookAtTargetB = false;

    #endregion

    void Look()
    {
        // Only look at target if bool is true and target is also not null
        targetLookDir = lookAtTargetB && lookTargetT ? 
            navAgent.transform.position - lookTargetT.position : navAgent.velocity.normalized;
        
        // Rotate mesh to look at lookAtDir at the speed of lookSpeed
        lookAtDir = Vector3.RotateTowards(mesh.forward, targetLookDir, lookSpeed * Time.deltaTime, 1);
        lookAtDir = new Vector3(lookAtDir.x, 0, lookAtDir.z); // Ensure targetLookDir.y is 0

        // Convert the vector to a quaternion, using mesh's up vector as up
        mesh.rotation = Quaternion.LookRotation(lookAtDir, mesh.up);
    }

    #endregion
}
