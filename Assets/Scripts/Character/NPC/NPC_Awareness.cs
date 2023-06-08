using Brad.Character;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Brad.Character
{
    public class NPC_Awareness : C_Awareness
    {
        #region Private variables
        IEventAndDataHandler _handler;

        [Tooltip("How many threats needed to make this npc flee (If set to 0, the npc will never enter combat).")]
        [SerializeField] int maxFearLevel = 5;
        [Tooltip("The distance at which this npc will disable itself.")]
        [SerializeField] float maxDistToPlayer = 50;

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            if (TryGetComponent(out _handler))
            {
                _handler.AddEvent("Refresh_ClosestThreatInProxT", Refresh_ClosestThreatInProxT);
                _handler.AddEvent("Refresh_ClosestThreatInViewT", Refresh_ClosestThreatInViewT);
                _handler.AddEvent("Refresh_ProxContainsThreatB", Refresh_ProxContainsThreatB);
                _handler.AddEvent("Refresh_ViewContainsThreatB", Refresh_ViewContainsThreatB);
                _handler.AddEvent("Refresh_InRangeToPlayerB", Refresh_InRangeOfPlayerB);
                _handler.AddEvent("Refresh_IsFearfulB", Refresh_IsFearfulB);
            }

            else
                Debug.LogError($"{this.name}... is missing components");
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

        #region Refresh data methods

        void Refresh_ClosestThreatInProxT() => 
            _handler.SetValue("ClosestThreatInProxT", ClosestTypeInList(targetType.Threat, targetsInProximity));
        void Refresh_ClosestThreatInViewT() => 
            _handler.SetValue("ClosestThreatInViewT", ClosestTypeInList(targetType.Threat, targetsInView));
        void Refresh_ProxContainsThreatB() => _handler.SetValue("ProxContainsThreatB", 
            NumberOfTypeInList(targetType.Threat, targetsInProximity) > 0);
        void Refresh_ViewContainsThreatB() => _handler.SetValue("ViewContainsThreatB",
            NumberOfTypeInList(targetType.Threat, targetsInView) > 0);
        void Refresh_InRangeOfPlayerB() => _handler.SetValue("InRangeOfPlayerB", InRangeOfPlayer());
        void Refresh_IsFearfulB() => _handler.SetValue("IsFearfulB", IsNpcFearful());

        #endregion
        public enum targetType { Ally, Threat }
        bool InRangeOfPlayer()
        {
            float distToPlayer = Vector3.Distance(transform.position, Player_Controller.Instance.transform.position);
            return distToPlayer < maxDistToPlayer;
        }
        bool IsNpcFearful()
        {
            // Determine how many threats outnumber this npc.
            int fearLevel = NumberOfTypeInList(targetType.Threat, targetsInProximity)
                - (1 + NumberOfTypeInList(targetType.Ally, targetsInProximity));
            return fearLevel >= maxFearLevel;
        }
        int NumberOfTypeInList(targetType type, List<Collider> list)
        {
            // Finds how many are within the proximity/view.
            int typeNum = 0;

            if(list.Count > 0)
            {
                foreach (Collider target in list)
                {
                    switch (type)
                    {
                        case targetType.Ally:
                            if (target.CompareTag(transform.tag))
                                typeNum++;
                            break;

                        case targetType.Threat:
                            if (!target.CompareTag(transform.tag))
                                typeNum++;
                            break;
                    }
                }
            }

            return typeNum;
        }
        Transform ClosestTypeInList(targetType type, List<Collider> list)
        {
            // Returns the closest target that is in the proximity.
            Transform closest = null;

            if (list.Count > 0)
            {
                float minDist = Mathf.Infinity;
                Vector3 currentPos = transform.position;
                foreach (Collider c in list)
                {
                    float dist = Vector3.Distance(c.transform.position, currentPos);
                    if (dist < minDist)
                    {
                        switch (type)
                        {
                            case targetType.Ally:
                                if (c.CompareTag(transform.tag))
                                {
                                    closest = c.transform;
                                    minDist = dist;
                                }
                                    
                                break;

                            case targetType.Threat:
                                if (!c.CompareTag(transform.tag))
                                {
                                    closest = c.transform;
                                    minDist = dist;
                                }
                                break;
                        }
                    }
                }
            }

            return closest;
        }
        #endregion
    }
}

