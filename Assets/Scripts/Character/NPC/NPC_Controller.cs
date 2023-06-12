using Brad.FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Brad.Character
{
    public class NPC_Controller : StateMachine, IEventAndDataHandler, IDamagable
    {
        #region Public Variables
        //Event and data handler
        public SO_C_Attributes attributes;
        IEventAndDataHandler handler;
        [HideInInspector]
        public Dictionary<string, object> data;
        [HideInInspector]
        public Dictionary<string, Action> events;

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

        #region Private Variables

        // Components

        #endregion

        #region MonoBehaviour

        private void OnEnable()
        {
            // NPC
            #region State initialisation

            foreach(SO_C_Attributes.State state in attributes.states)
            {
                BaseState tmpState = new BaseState(state.stateClass);
                handler.SetValue($"State_{state.name}", state.stateClass as BaseState);
            }

            foreach(SO_C_Attributes.Attribute attribute in attributes.attributes)
            {
                switch (attribute.type)
                {
                    case SO_C_Attributes.Attribute.variableType.Type_Float:
                        handler.SetValue(attribute.name, float.Parse(attribute.value));
                        break;

                    case SO_C_Attributes.Attribute.variableType.Type_Int:
                        handler.SetValue(attribute.name, int.Parse(attribute.value));
                        break;

                    case SO_C_Attributes.Attribute.variableType.Type_String:
                        handler.SetValue(attribute.name, attribute.value);
                        break;
                }
            }
            /*
            spawnState = new S_Spawn(this);
            despawnState = new S_Despawn(this);
            idleState = new S_Idle(this);
            performState = new S_Perform(this);
            moveState = new S_Move(this);
            alertState = new S_Alert(this);
            searchState = new S_Search(this);
            fleeState = new S_Flee(this);
            combatState = new S_Combat(this);
            deadState = new S_Dead(this);*/
            #endregion

            handler = GetComponent<IEventAndDataHandler>();

            #region Data initialisation

            #endregion

            #region Event initialisation

            //handler.AddEvent("Next_State", NextState);
            handler.AddEvent("Enable_C", EnableNPC);
            handler.AddEvent("Disable_C", DisableNPC);

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
        void DisableNPC()
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
            return handler.GetValue<BaseState>("State_Spawn");
        }
        #endregion
    }
}

