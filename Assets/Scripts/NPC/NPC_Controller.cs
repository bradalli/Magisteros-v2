using Brad.FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brad.Character
{
    public class NPC_Controller : StateMachine
    {
        #region Public Variables
        // Attributes
        [SerializeField] string npcName = "NPC";

        // Events

        // State information

        #region Delegates
        public delegate bool BoolCheck();
        public BoolCheck d_IsNpcOutOfRange;
        public BoolCheck d_IsAThreatInView;
        public BoolCheck d_IsFearOverMax;

        public delegate C_Action ActionCheck();
        public ActionCheck d_CurrentAction;

        public delegate Waypoint WaypointCheck();
        public WaypointCheck d_CurrentWaypoint;

        public delegate int IntCheck();
        public IntCheck d_ThreatsInProxNum;
        public IntCheck d_AlliesInProxNum;

        #endregion

        #region Events
        public event Action<Vector3> E_SetNavDestination;
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
        private void Awake()
        {
            #region State initialisation
            spawnState = new S_Spawn(this);
            despawnState = new S_Despawn(this);
            idleState = new S_Idle(this);
            performState = new S_Perform(this);
            moveState = new S_Move(this);
            alertState = new S_Alert(this);
            fleeState = new S_Flee(this);
            combatState = new S_Combat(this);
            deadState = new S_Dead(this);
            #endregion
            
        }
        #endregion

        #region Custom Methods


        #endregion

        #region Event invoke
        public void SetNavDestination(Vector3 position)
        {
            E_SetNavDestination.Invoke(position);
        }
        #endregion

        #region Delegate invoke events
        public bool Get_IsNpcOutOfRange() => d_IsNpcOutOfRange.Invoke();
        public int Get_ThreatsInProxNum() => d_ThreatsInProxNum.Invoke();
        public int Get_AlliesInProxNum() => d_AlliesInProxNum.Invoke();
        public C_Action Get_CurrAction() => d_CurrentAction.Invoke();
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

