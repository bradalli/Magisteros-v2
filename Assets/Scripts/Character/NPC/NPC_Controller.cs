using Brad.FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brad.Character
{
    public class NPC_Controller : StateMachine, IDataManager, IDamagable
    {
        #region Public Variables
        // Data handler
        public Dictionary<string, object> data;

        // Attributes
        public float fleeDistance = 15f; // NPC

        // Stats
        public int maxHealth = 100; // Both
        public int health; // Both

        // Both
        #region Interface instance properties
        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }
        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        public Dictionary<string, object> dataDictionary { get => data; }
        #endregion

        #region Events
        public event Action<Vector3> E_SetNavDestination; // Both
        public event Action<Vector3> E_LookAtPosition; // NPC
        public event Action<Waypoint> E_SetCurrWaypoint; // NPC
        public event Action<string, bool> E_SetAnimBool; // Both
        public event Action<string> E_SetAnimTrigger; // Both
        public event Action E_ActionEnd; // Both
        public event Action<CharacterAction> E_NewAction; // Both
        public event Action<string> E_ResetTrigger; // Both
        #endregion

        #endregion

        // NPC
        #region States
        [HideInInspector]
        public S_Spawn spawnState;
        [HideInInspector]
        public S_Despawn despawnState;
        [HideInInspector]
        public S_Idle idleState;
        [HideInInspector]
        public S_Perform performState;
        [HideInInspector]
        public S_Move moveState;
        [HideInInspector]
        public S_Alert alertState;
        [HideInInspector]
        public S_Search searchState;
        [HideInInspector]
        public S_Flee fleeState;
        [HideInInspector]
        public S_Combat combatState;
        [HideInInspector]
        public S_Dead deadState;
        #endregion

        #region Private Variables

        // Components

        #endregion

        #region MonoBehaviour
        private void OnEnable()
        {
            // NPC
            #region State initialisation
            spawnState = new S_Spawn(this);
            despawnState = new S_Despawn(this);
            idleState = new S_Idle(this);
            performState = new S_Perform(this);
            moveState = new S_Move(this);
            alertState = new S_Alert(this);
            searchState = new S_Search(this);
            fleeState = new S_Flee(this);
            combatState = new S_Combat(this);
            deadState = new S_Dead(this);
            #endregion
            #region Data initialisation
            dataDictionary.Add("Event", EnableNPC());
            #endregion
        }
        #endregion

        #region Custom Methods

        public void EnableNPC(bool value) // Both
        {
            foreach (Behaviour comp in gameObject.GetComponents<Behaviour>())
            {
                if (comp != this)
                {
                    comp.enabled = value;
                }
            }
        }

        #endregion

        #region Event invoke
        public void Set_NavDestination(Vector3 position) => E_SetNavDestination.Invoke(position);
        public void Set_LookAtPosition(Vector3 position) => E_LookAtPosition.Invoke(position);
        public void Set_CurrentWaypoint(Waypoint waypoint) => E_SetCurrWaypoint.Invoke(waypoint);
        public void Set_AnimBool(string boolName, bool value) => E_SetAnimBool.Invoke(boolName, value);
        public void Set_AnimTrigger(string triggerName) => E_SetAnimTrigger.Invoke(triggerName);
        public void Do_ActionEnd() => E_ActionEnd.Invoke();
        public void Set_NewAction(CharacterAction newAction) => E_NewAction.Invoke(newAction);
        public void Set_ResetTrigger(string name) => E_ResetTrigger.Invoke(name);
        #endregion 

        #region FSM Methods
        protected override BaseState GetInitialState()
        {
            return spawnState;
        }
        #endregion

        #region Miscellaneous

        #endregion
    }
}

