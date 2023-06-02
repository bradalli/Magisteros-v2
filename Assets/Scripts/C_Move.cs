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

    private Vector3 targetLookDir, currLookDir;
    private Vector3 lookAtPosition;

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
        if (lookAtPosition != Vector3.zero)
            targetLookDir = new Vector3(lookAtPosition.x, transform.position.y, lookAtPosition.z) - transform.position;

        else if (navAgent.hasPath)
            targetLookDir = new Vector3(navAgent.nextPosition.x, transform.position.y, navAgent.nextPosition.z)
                - transform.position;

        else
            targetLookDir = new Vector3(Vector3.forward.x, transform.position.y, Vector3.forward.z);


        targetLookDir = new Vector3(targetLookDir.x, 0, targetLookDir.z);
        transform.rotation = Quaternion.LookRotation(targetLookDir, transform.up);
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
}
