using Brad.FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;
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

        [Serializable]
        public struct DictionaryValue { public string key; public string value; }
        public List<DictionaryValue> dataList;
        public List<DictionaryValue> eventsList;

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

        private void OnEnable()
        {
            TryGetComponent(out handler);
            TryGetComponent(out damage);

            handler.EventDictionary = new Dictionary<string, Action>();
            handler.DataDictionary = new Dictionary<string, object>();

            damage.ResetHealth();

            // NPC
            #region State initialisation

            if (handler != null)
            {;
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

                handler.SetValue("T_Mesh", transform.Find($"{gameObject.name}_Mesh"));
                //handler.AddEvent("Next_State", NextState);
                handler.AddEvent("Enable_C", EnableNPC);
                handler.AddEvent("Disable_C", DisableNPC);
                handler.AddEvent("Set_B_InRangeOfPlayer", Set_InRangeOfPlayer);

                EnableNPC();

                #endregion
            }
            /*
            // Trigger all events that call to "set" a data variable
            for(int i = 0; i < events.Count; i++)
            {
                if (events.ElementAt(i).Key.Contains("Set_") ^ events.ElementAt(i).Key.Contains("Get_"))
                {
                    events.ElementAt(i).Value.Invoke();
                }
            }*/
        }

        void UpdateLists()
        {
            dataList = new List<DictionaryValue>();
            eventsList = new List<DictionaryValue>();

            for (int i = 0; i < data.Count; i++)
            {
                DictionaryValue val = new DictionaryValue { key = handler.DataDictionary.ElementAt(i).Key, value = handler.DataDictionary.ElementAt(i).Value.ToString() };
                dataList.Add(val);
            }

            for (int i = 0; i < events.Count; i++)
            {
                DictionaryValue val = new DictionaryValue { key = handler.EventDictionary.ElementAt(i).Key, value = handler.EventDictionary.ElementAt(i).Value.ToString() };
                eventsList.Add(val);
            }
        }

        new void Update()
        {
            base.Update();
            
            if (Input.GetKeyDown(KeyCode.P))
                StartFSM();
            /*
            if (Input.GetKeyDown(KeyCode.U))
                UpdateLists();*/
        }

        #endregion

        #region Custom Methods

        void StartFSM()
        {
            currentState = GetInitialState();
            if (currentState != null)
                currentState.Enter();
        }

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

        void Set_InRangeOfPlayer() => handler.SetValue("B_InRangeOfPlayer", InRangeOfPlayer());

        bool InRangeOfPlayer()
        {
            float distToPlayer = Vector3.Distance(transform.position, Player_Controller.Instance.transform.position);
            return distToPlayer < handler.GetValue<float>("F_MaxDistToPlayer");
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

