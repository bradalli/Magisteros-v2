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

        // How many threats needed to make this npc flee (If set to 0, the npc will never enter combat).
        int maxFearLevel = 5;
        // The distance at which this npc will disable itself.
        float maxDistToPlayer = 50;

        #endregion

        #region Monobehaviour

        private void Start()
        {
            if (TryGetComponent(out _handler))
            {
                #region Data initialisation "SET"

                _handler.AddEvent("Set_T_ClosestThreatInProx", Set_ClosestThreatInProx);
                _handler.AddEvent("Set_T_ClosestThreatInView", Set_ClosestThreatInView);
                _handler.AddEvent("Set_B_ProxContainsThreat", Set_ProxContainsThreat);
                _handler.AddEvent("Set_B_ViewContainsThreat", Set_ViewContainsThreat);
                _handler.AddEvent("Set_V_ProxThreatsAvgPosition", Set_ProxThreatsAvgPosition);
                _handler.AddEvent("Set_V_ViewThreatsAvgPosition", Set_ViewThreatsAvgPosition);
                _handler.AddEvent("Set_B_InRangeOfPlayer", Set_InRangeOfPlayer);
                _handler.AddEvent("Set_B_IsFearful", Set_IsFearful);

                #endregion

                #region Data initialisation "GET"

                _handler.AddEvent("Get_I_MaxFearLevel", Get_MaxFearLevel);
                _handler.AddEvent("Get_F_MaxDistToPlayer", Get_MaxDistToPlayer);

                #endregion
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

        #region Set data methods

        void Set_ClosestThreatInProx() => 
            _handler.SetValue("T_ClosestThreatInProx", ClosestTypeInList(targetType.Threat, targetsInProximity));
        void Set_ClosestThreatInView() => 
            _handler.SetValue("T_ClosestThreatInView", ClosestTypeInList(targetType.Threat, targetsInView));
        void Set_ProxContainsThreat() => _handler.SetValue("B_ProxContainsThreat", 
            NumberOfTypeInList(targetType.Threat, targetsInProximity) > 0);
        void Set_ViewContainsThreat() => _handler.SetValue("B_ViewContainsThreat",
            NumberOfTypeInList(targetType.Threat, targetsInView) > 0);
        void Set_ProxThreatsAvgPosition() => _handler.SetValue("V_ProxThreatsAvgPosition",
            AveragePositionOfTypeInList(targetType.Threat, targetsInProximity));
        void Set_ViewThreatsAvgPosition() => _handler.SetValue("V_ViewThreatsAvgPosition",
            AveragePositionOfTypeInList(targetType.Threat, targetsInView));
        void Set_InRangeOfPlayer() => _handler.SetValue("B_InRangeOfPlayer", InRangeOfPlayer());
        void Set_IsFearful() => _handler.SetValue("B_IsFearful", IsNpcFearful());

        #endregion

        #region Get data methods

        void Get_MaxFearLevel() => _handler.GetValue<int>("I_MaxFearLevel");
        void Get_MaxDistToPlayer() => _handler.GetValue<float>("F_MaxDistToPlayer");

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
        Vector3 AveragePositionOfTypeInList(targetType type, List<Collider> list)
        {
            Vector3 averagePosition = Vector3.zero;

            foreach (Collider c in list)
            {
                switch (type)
                {
                    case targetType.Ally:
                        averagePosition += c.transform.position;
                        break;

                    case targetType.Threat:
                        averagePosition += c.transform.position;
                        break;
                }
            }

            averagePosition /= list.Count;

            return averagePosition;
        }
        #endregion
    }
}

