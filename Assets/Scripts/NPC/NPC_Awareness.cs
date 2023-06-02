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
        private void OnEnable()
        {
            #region Delegate subscriptions
            if (TryGetComponent(out npcCont))
            {
                npcCont.GetThreatsInProxNum += FindNumberOfThreatsInProximity;
                npcCont.IsAThreatInView += DoesViewContainThreat;

                npcCont.GetAlliesInProxNum += FindNumberOfAlliesInProximity;
            }

            else
                Debug.LogError($"{this.name}... is missing a NPC_Controller component");
            #endregion
        }


        #region Custom Methods 
        public int FindNumberOfAlliesInProximity()
        {
            // Finds how many "allies" are within the proximity.
            int allyNum = 0;

            foreach (Collider target in targetsInProximity)
            {
                if (target.CompareTag(this.tag))
                    allyNum++;
            }

            return allyNum;
        }
        public int FindNumberOfThreatsInProximity()
        {
            // Finds how many "allies" are within the proximity.
            int threatNum = 0;

            foreach (Collider target in targetsInProximity)
            {
                if (!target.CompareTag(this.tag))
                    threatNum++;
            }

            return threatNum;
        }
        public bool DoesViewContainThreat()
        {
            foreach(Collider c in targetsInView)
            {
                if (c.CompareTag(this.tag))
                    return true;
            }

            return false;
        }
        #endregion
    }
}

