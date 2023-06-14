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
    float moveSpeed = 5;

    Vector3 moveDestination;

    private IEventAndDataHandler _handler;
    private NavMeshAgent navAgent;
    public Transform mesh;

    private Waypoint currentWaypoint;

    [SerializeField] bool moving = false;
    [SerializeField] bool looking = false;
    private Vector3 lookDirection;
    [SerializeField] private Transform followTarget;
    [SerializeField] private Transform lookTarget;
    [SerializeField] private Vector3 lookPos;
    #endregion

    #region MonoBehaviour

    private void Start()
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
            _handler.AddEvent("Get_T_FollowTarget", Get_FollowTarget);

            // Update data initialisation SET
            _handler.AddEvent("Set_B_DestinationReach", Set_DestinationReach);
            _handler.AddEvent("Set_V_Velocity", Set_Velocity);
            _handler.AddEvent("Set_W_CurrWp", Set_CurrWp);
            _handler.AddEvent("Set_B_Moving", Set_Moving);

            // Event initialisation
            _handler.AddEvent("Start_Move", Start_Move);
            _handler.AddEvent("Stop_Move", Stop_Move);
            _handler.AddEvent("GoToWp", GotoWp);
            _handler.AddEvent("Start_LookAt", Start_LookAt);
            _handler.AddEvent("Stop_LookAt", Stop_LookAt);
        }

        else
            Debug.LogError(_handler.GetType().ToString() + " could not be found");

        mesh = _handler.GetValue<Transform>("T_Mesh");
    }
    private void Update()
    {
        // Make the mesh of the character look at a direction every frame
        Look();
        Move();
    }

    #endregion

    #region Custom methods

    #region Refresh data methods

    void Get_LookSpeed() => lookSpeed = _handler.GetValue<float>("F_LookSpeed");
    void Get_MoveSpeed() => moveSpeed = _handler.GetValue<float>("F_MoveSpeed");
    void Get_Mesh() => mesh = _handler.GetValue<Transform>("T_Mesh");
    void Get_Destination() => moveDestination = _handler.GetValue<Vector3>("V_Destination");
    void Get_LookTarget() => lookTarget = _handler.GetValue<Transform>("T_LookTarget");
    void Get_LookPosition() => lookPos = _handler.GetValue<Vector3>("V_LookPosition");
    void Get_FollowTarget() => followTarget = _handler.GetValue<Transform>("T_FollowTarget");

    void Set_CurrWp() => _handler.SetValue("W_CurrWp", currentWaypoint);
    void Set_DestinationReach() => _handler.SetValue("B_DestinationReach",
        navAgent.remainingDistance <= navAgent.stoppingDistance);
    void Set_Velocity() => _handler.SetValue("V_Velocity", navAgent.velocity);
    void Set_Moving() => _handler.SetValue("B_Moving", !navAgent.isStopped);

    #endregion

    #region Event methods

    void Start_Move() 
    {
        navAgent.isStopped = false;
        moving = true;
    }
    void Stop_Move()
    {
        navAgent.isStopped = true;
        moving = false;
    }
    void GotoWp()
    {
        if (currentWaypoint != null)
            navAgent.destination = currentWaypoint.transform.position;

        else
            Debug.LogError("Unable to go to CurrWp because it is null.");
    }
    void Start_LookAt() => looking = true;
    void Stop_LookAt() => looking = false;

    #endregion

    void Look()
    {
        if (looking)
        {
            if (lookTarget != null)
            {
                lookDirection = navAgent.transform.position - lookTarget.position;
            }

            else
            {
                lookDirection = navAgent.transform.position - lookPos;
            }
        }

        else if(navAgent.velocity.magnitude > 0.5f)
        {
            lookDirection = navAgent.velocity.normalized;
        }

        // Rotate mesh to look at lookAtDir at the speed of lookSpeed
        lookDirection = Vector3.RotateTowards(mesh.forward, lookDirection, lookSpeed * Time.deltaTime, 1);
        lookDirection = new Vector3(lookDirection.x, 0, lookDirection.z); // Ensure targetLookDir.y is 0

        // Convert the vector to a quaternion, using mesh's up vector as up
        mesh.rotation = Quaternion.LookRotation(lookDirection, mesh.up);
    }

    void Move()
    {
        if(!navAgent.isStopped && navAgent.destination != GetNavDestination())
            navAgent.destination = GetNavDestination();
    }

    Vector3 GetNavDestination()
    {
        if (followTarget != null)
            return followTarget.position;

        if (currentWaypoint != null)
            return currentWaypoint.transform.position;

        return moveDestination;
    }

    #endregion
}
