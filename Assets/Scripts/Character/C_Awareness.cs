using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Brad.Character
{
    public class C_Awareness : MonoBehaviour
    {
        #region Public and Serialized variables

        [Header("View Cone Attributes")]
        public float viewConeFov = 60f;
        public float viewConeMaxDistance = 5f;
        public List<Collider> targetsInView;

        [Header("Proximity Attributes")]
        [SerializeField] LayerMask proximityMask;
        [SerializeField] float proximityRange = 10f;
        public List<Collider> targetsInProximity;
        #endregion

        #region Private variables
        Collider myCollider;
        C_Controller cont;
        #endregion

        #region Monobehaviour

        private void Awake()
        {
            TryGetComponent(out myCollider);
            TryGetComponent(out cont);
        }

        private void Update()
        {
            targetsInProximity = FindTargetsInProximity(transform.position, proximityRange, proximityMask);

            if (targetsInProximity.Count > 0)
            {
                targetsInView = FindTargetsInView();
            }
        }

        public void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                // Proximity gizmos
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(transform.position, proximityRange);

                Gizmos.color = Color.blue;
                if (targetsInProximity.Count > 0)
                {
                    Gizmos.DrawLine(transform.position, FindClosestTargetInProximity().transform.position);
                }
                    
                // View cone gizmos
                Gizmos.color = Color.yellow;
                #region Draw view cone
                float totalFOV = viewConeFov;
                float rayRange = viewConeMaxDistance;
                Quaternion leftRayRotation = Quaternion.AngleAxis(-totalFOV, Vector3.up);
                Quaternion rightRayRotation = Quaternion.AngleAxis(totalFOV, Vector3.up);
                Vector3 leftRayDirection = leftRayRotation * cont.meshT.forward;
                Vector3 rightRayDirection = rightRayRotation * cont.meshT.forward;
                Gizmos.DrawRay(transform.position, leftRayDirection * rayRange);
                Gizmos.DrawRay(transform.position, rightRayDirection * rayRange);
                Handles.color = Color.yellow;
                UnityEditor.Handles.DrawWireArc(transform.position, transform.up, leftRayDirection, viewConeFov * 2, viewConeMaxDistance);
                #endregion
                Gizmos.color = Color.red;
                if (FindClosestTargetInView() != null && targetsInProximity.Count > 0)
                    Gizmos.DrawLine(transform.position, FindClosestTargetInView().transform.position);
            }
        }

        #endregion

        #region Custom methods

        List<Collider> FindTargetsInProximity(Vector3 origin, float range, LayerMask mask)
        {
            // Finds all objects in an overlap sphere with a specific layer ("Character").
            List<Collider> tmpFoundTargetsInProx = Physics.OverlapSphere(origin, range, mask).ToList<Collider>();

            if (tmpFoundTargetsInProx.Contains(myCollider))
            {
                tmpFoundTargetsInProx.Remove(myCollider);
            }

            // Only add characters if they are unobstructed via a raycast.
            List<Collider> tmpUnobstructedTsInProx = new List<Collider>();
            foreach(Collider c in tmpFoundTargetsInProx)
            {
                tmpUnobstructedTsInProx.Add(c);
            }

            return tmpUnobstructedTsInProx;
        }
        List<Collider> FindTargetsInView()
        {
            List<Collider> targetsInView = new List<Collider>();

            // Returns targets in proximity that are in also in range of the view cone using vector3 angle,
            // it also checks if the view is obstructed using a raycast.
            if (targetsInProximity.Count > 0)
            {
                foreach (Collider c in targetsInProximity)
                {
                    Vector3 targetDir = (c.transform.position - transform.position).normalized;
                    float angle = Vector3.Angle(targetDir, cont.meshT.forward);
                    float distance = Vector3.Distance(transform.position, c.transform.position);

                    if (angle < viewConeFov && distance < viewConeMaxDistance)
                    {
                        Physics.Raycast(transform.position + targetDir * .51f, targetDir, out RaycastHit hit, distance);
                        if (hit.collider == c)
                            targetsInView.Add(c);
                    }
                }
            }

            return targetsInView;
        }

        public Collider FindClosestTargetInProximity()
        {
            // Returns the closest target that is in the proximity.
            Collider closest = null;

            if (targetsInProximity.Count > 0)
            {
                float minDist = Mathf.Infinity;
                Vector3 currentPos = transform.position;
                foreach (Collider c in targetsInProximity)
                {
                    float dist = Vector3.Distance(c.transform.position, currentPos);
                    if (dist < minDist)
                    {
                        closest = c;
                        minDist = dist;
                    }
                }
            }

            return closest;
        }
        public Collider FindClosestTargetInView()
        {
            // Returns the closest target that is in the view cone.
            Collider closest = null;

            if (targetsInView.Count > 0)
            {
                float minDist = Mathf.Infinity;
                Vector3 currentPos = transform.position;
                foreach (Collider c in targetsInView)
                {
                    float dist = Vector3.Distance(c.transform.position, currentPos);
                    if (dist < minDist)
                    {
                        closest = c;
                        minDist = dist;
                    }
                }
            }

            return closest;
        }
        #endregion
    }
}

