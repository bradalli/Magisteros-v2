using Brad.FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brad.Character
{
    public class NPC_Controller : StateMachine, IEventAndDataHandler, IDamagable
    {
        #region Public Variables
        //Event and data handler
        IEventAndDataHandler handler;
        [HideInInspector]
        public Dictionary<string, object> data;
        [HideInInspector]
        public Dictionary<string, Action> events;

        // Attributes
        public float fleeDistance = 15f; // NPC

        // Stats
        public int maxHealth = 100;
        public int health;

        #region Interface instance properties
        public int MaxHealth { get => maxHealth; set => maxHealth = value; }
        public int Health { get => health; set => health = value; }
        public Dictionary<string, object> dataDictionary { get => data; }
        public Dictionary<string, Action> eventDictionary { get => events; }
        #endregion

        #endregion

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

            handler = GetComponent<IEventAndDataHandler>();

            #region Data initialisation

            #endregion

            #region Event initialisation
            handler.AddEvent("Enable", EnableNPC);
            handler.AddEvent("Disable", DisableNPC);
            #endregion
        }

        #endregion

        #region Custom Methods

        #region Event methods

        void EnableNPC()
        {
            foreach (Behaviour comp in gameObject.GetComponents<Behaviour>())
            {
                if (comp != this)
                {
                    comp.enabled = true;
                }
            }
        }
        public void DisableNPC()
        {
            foreach (Behaviour comp in gameObject.GetComponents<Behaviour>())
            {
                if (comp != this)
                {
                    comp.enabled = false;
                }
            }
        }

        #endregion

        #endregion

        #region FSM Methods
        protected override BaseState GetInitialState()
        {
            return spawnState;
        }
        #endregion
    }
}

