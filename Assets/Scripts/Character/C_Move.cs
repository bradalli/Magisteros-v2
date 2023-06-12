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
    float moveSpeed;

    private IEventAndDataHandler _handler;
    private NavMeshAgent navAgent;
    private Transform mesh;

    private Waypoint currentWaypoint;

    bool lookAtTargetB = false;
    private Vector3 lookAtDir = Vector3.forward;
    private Vector3 targetLookDir;
    private Transform lookTargetT;
    private Vector3 lookPos;
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
            // Update data initialisation GET
            _handler.AddEvent("Get_F_LookSpeed", Get_LookSpeed);
            _handler.AddEvent("Get_F_MoveSpeed", Get_MoveSpeed);
            _handler.AddEvent("Get_T_Mesh", Get_Mesh);
            _handler.AddEvent("Get_V_Destination", Get_Destination);
            _handler.AddEvent("Get_T_LookTarget", Get_LookTarget);
            _handler.AddEvent("Get_V_LookPosition", Get_LookPosition);

            // Update data initialisation SET
            _handler.AddEvent("Set_B_DestinationReach", Set_DestinationReach);
            _handler.AddEvent("Set_V_Velocity", Set_Velocity);
            _handler.AddEvent("Set_W_CurrWp", Set_CurrWp);

            // Event initialisation
            _handler.AddEvent("Start_Move", Start_Move);
            _handler.AddEvent("Stop_Move", Stop_Move);
            _handler.AddEvent("GoToWp", GotoWp);
            _handler.AddEvent("Start_LookAt", Start_LookAt);
            _handler.AddEvent("Stop_LookAt", Stop_LookAt);
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

    void Get_LookSpeed() => lookSpeed = _handler.GetValue<float>("F_LookSpeed");
    void Get_MoveSpeed() => moveSpeed = _handler.GetValue<float>("F_MoveSpeed");
    void Get_Mesh() => mesh = _handler.GetValue<Transform>("T_Mesh");
    void Get_Destination() => navAgent.destination = _handler.GetValue<Vector3>("V_Destination");
    void Get_LookTarget() => lookTargetT = _handler.GetValue<Transform>("T_LookTarget");
    void Get_LookPosition() => lookPos = _handler.GetValue<Vector3>("V_LookPosition");

    void Set_CurrWp() => _handler.SetValue("W_CurrWp", currentWaypoint);
    void Set_DestinationReach() => _handler.SetValue("B_DestinationReach",
        navAgent.remainingDistance <= navAgent.stoppingDistance);
    void Set_Velocity() => _handler.SetValue("V_Velocity", navAgent.velocity);

    #endregion

    #region Event methods

    void Start_Move() => navAgent.isStopped = false;
    void Stop_Move() => navAgent.isStopped = true;
    void GotoWp()
    {
        if (currentWaypoint != null)
            navAgent.destination = currentWaypoint.transform.position;

        else
            Debug.LogError("Unable to go to CurrWp because it is null.");
    }
    void Start_LookAt() => lookAtTargetB = true;
    void Stop_LookAt() => lookAtTargetB = false;

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
