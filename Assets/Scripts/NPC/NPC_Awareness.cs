using Brad.Character;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Brad.Character
{
    public class NPC_Awareness : C_Awareness
    {
        NPC_Controller npcCont;

        [Tooltip("How many threats needed to make this npc flee (If set to 0, the npc will never enter combat).")]
        [SerializeField] int maxFearLevel = 5;
        [Tooltip("The distance at which this npc will disable itself.")]
        [SerializeField] float maxDistToPlayer = 50;

        #region Monobehaviour
        private void OnEnable()
        {
            #region Delegate subscriptions

            if (TryGetComponent(out npcCont))
            {
                npcCont.d_IsNpcOutOfRange += IsNpcOutOfRangeToPlayer;
                npcCont.d_IsFearOverMax += IsNpcFearful;

                npcCont.d_ThreatsInProxNum += FindNumberOfThreatsInProximity;
                npcCont.d_ThreatsInViewNum += FindNumberOfThreatsInView;

                npcCont.d_AlliesInProxNum += FindNumberOfAlliesInProximity;

                npcCont.d_ThreatsInProx += ReturnThreatsInProxColliders;

                npcCont.d_ClosestThreatInProx += FindClosestTargetInProximity;
                npcCont.d_ClosestThreatInView += FindClosestTargetInView;
            }

            else
                Debug.LogError($"{this.name}... is missing a NPC_Controller component");
            #endregion
        }

        private new void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            // NPC Bounds
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(transform.position, maxDistToPlayer);
        }
        #endregion

        #region Custom Methods 
        public bool IsNpcOutOfRangeToPlayer()
        {
            float distToPlayer = Vector3.Distance(transform.position, Player_Controller.Instance.transform.position);
            return distToPlayer > maxDistToPlayer;
        }
        public bool IsNpcFearful()
        {
            // Determine how many threats outnumber this npc.
            int fearLevel = FindNumberOfThreatsInProximity() - (1 + FindNumberOfAlliesInProximity());
            return fearLevel >= maxFearLevel;
        }
        public int FindNumberOfAlliesInProximity()
        {
            // Finds how many "allies" are within the proximity.
            int allyNum = 0;

            if(targetsInProximity.Count > 0)
            {
                foreach (Collider target in targetsInProximity)
                {
                    if (target.CompareTag(transform.tag))
                        allyNum++;
                }
            }

            return allyNum;
        }
        public int FindNumberOfThreatsInProximity()
        {
            // Finds how many "threats" are within the proximity.
            int threatNum = 0;

            if (targetsInProximity.Count > 0)
            {
                foreach (Collider target in targetsInProximity)
                {
                    if (!target.CompareTag(transform.tag))
                        threatNum++;
                }
            }

            return threatNum;
        }
        public int FindNumberOfThreatsInView()
        {
            // Finds how many "threats" are within the view cone.

            int threatNum = 0;
            foreach(Collider c in targetsInView)
            {
                if (!c.CompareTag(transform.tag))
                    threatNum++;
            }

            return threatNum;
        }
        public Collider[] ReturnThreatsInProxColliders()
        {
            List<Collider> tmpThreatsInProx = new List<Collider>();

            if (targetsInProximity.Count > 0)
            {
                foreach (Collider target in targetsInProximity)
                {
                    if (!target.CompareTag(transform.tag))
                        tmpThreatsInProx.Add(target);
                }
            }

            return tmpThreatsInProx.ToArray();
        }
        public Collider[] ReturnThreatsInViewColliders()
        {
            List<Collider> tmpThreatsInView = new List<Collider>();

            if (targetsInView.Count > 0)
            {
                foreach (Collider target in targetsInView)
                {
                    if (!target.CompareTag(transform.tag))
                        tmpThreatsInView.Add(target);
                }
            }

            return tmpThreatsInView.ToArray();
        }
        #endregion
    }
}

