using Brad.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class C_Move : MonoBehaviour
{
    [SerializeField] float lookSpeed = 5f;

    private NPC_Controller npcCont;
    private NavMeshAgent navAgent;
    public Waypoint currentWaypoint;

    private Vector3 targetLookDir = Vector3.forward;
    public Vector3 lookAtPosition;

    private void OnEnable()
    {
        TryGetComponent<NPC_Controller>(out npcCont);
        TryGetComponent<NavMeshAgent>(out navAgent);
        if (npcCont != null)
        {
            npcCont.d_CurrentWaypoint += ReturnCurrentWaypoint;

            if (navAgent != null)
            {
                npcCont.d_RemainingNavDistance += ReturnRemainingNavDistance;
                npcCont.d_NavVelocity += ReturnVelocity;
                npcCont.d_NavReachedDestination += ReturnReachedDestination;

                npcCont.E_SetNavDestination += SetAgentDestination;
                npcCont.E_LookAtPosition += SetLookAtPosition;
            }
        }
    }

    private void Update()
    {
        Look();
    }

    void Look()
    {
        if (lookAtPosition == Vector3.zero && navAgent.velocity.normalized.magnitude > 0)
            targetLookDir = Vector3.RotateTowards(npcCont.meshT.forward, navAgent.velocity.normalized, lookSpeed * Time.deltaTime, 0);

        if(lookAtPosition != Vector3.zero)
            targetLookDir = Vector3.RotateTowards(npcCont.meshT.forward, lookAtPosition, lookSpeed * Time.deltaTime, 1);

        // Ensure targetLookDir.y is 0
        targetLookDir = new Vector3(targetLookDir.x, 0, targetLookDir.z);
        
        npcCont.meshT.rotation = Quaternion.LookRotation(targetLookDir, npcCont.meshT.up);
    }

    public void SetLookAtPosition(Vector3 position)
    {
        lookAtPosition = position;
    }

    public void SetAgentDestination(Vector3 position)
    {
        navAgent.SetDestination(position);
    }

    public float ReturnRemainingNavDistance()
    {
        return navAgent.remainingDistance;
    }

    public Waypoint ReturnCurrentWaypoint()
    {
        return currentWaypoint;
    }

    public Vector3 ReturnVelocity()
    {
        return navAgent.velocity;
    }

    public bool ReturnReachedDestination()
    {
        return navAgent.remainingDistance <= navAgent.stoppingDistance;
    }
}
