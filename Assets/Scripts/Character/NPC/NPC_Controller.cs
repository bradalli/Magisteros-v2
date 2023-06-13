using Brad.FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using StateMachine = Brad.FSM.StateMachine;

namespace Brad.Character
{
    public class NPC_Controller : StateMachine, IEventAndDataHandler, IDamagable
    {
        #region Public Variables
        //Event and data handler
        public SO_C_Attributes so_Attributes;
        public IEventAndDataHandler handler;
        IDamagable damage;
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
        public Dictionary<string, object> DataDictionary { get => data; set => data = value; }
        public Dictionary<string, Action> EventDictionary { get => events; set => events = value; }
        #endregion

        #endregion

        #region Private Variables

        // Components

        #endregion

        #region MonoBehaviour

        private void Start()
        {
            TryGetComponent(out handler);
            TryGetComponent(out damage);

            handler.EventDictionary = new Dictionary<string, Action>();
            handler.DataDictionary = new Dictionary<string, object>();

            damage.ResetHealth();

            // NPC
            #region State initialisation

            if (handler != null)
            {
                for(int i = 0; i < so_Attributes.attributes.Length; i++)
                {
                    SO_C_Attributes.Attribute tmpAttribute = so_Attributes.attributes[i];
                    switch (tmpAttribute.type)
                    {
                        case SO_C_Attributes.Attribute.variableType.Type_Float:
                            handler.SetValue(tmpAttribute.name, float.Parse(tmpAttribute.value));
                            break;

                        case SO_C_Attributes.Attribute.variableType.Type_Int:
                            handler.SetValue(tmpAttribute.name, int.Parse(tmpAttribute.value));
                            break;

                        case SO_C_Attributes.Attribute.variableType.Type_String:
                            handler.SetValue(tmpAttribute.name, tmpAttribute.value);
                            break;
                    }
                }

                foreach (SO_C_Attributes.State state in so_Attributes.states)
                {
                    BaseState stateInstance = (BaseState)ScriptableObject.CreateInstance(state.stateClass.GetClass());
                    stateInstance.SetValues(state.name, this);

                    handler.SetValue($"State_{state.name}", stateInstance);
                }
                #endregion

                #region Data initialisation

                #endregion

                #region Event initialisation

                EnableNPC();

                //handler.AddEvent("Next_State", NextState);
                handler.AddEvent("Enable_C", EnableNPC);
                handler.AddEvent("Disable_C", DisableNPC);

                #endregion

                /*
                for (int i = 0; i < data.Count; i++)
                {
                    Debug.Log($"DataDictionary({i}) = {data.ElementAt(i).Key}, {data.ElementAt(i).Value}");
                }

                for (int i = 0; i < events.Count; i++)
                {
                    Debug.Log($"EventDictionary({i}) = {events.ElementAt(i).Key}, {events.ElementAt(i).Value}");
                }*/
            }
            
            currentState = GetInitialState();
            if (currentState != null)
                currentState.Enter();
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

