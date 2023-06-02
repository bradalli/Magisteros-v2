using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brad.Character
{
    public class NPC_Controller : StateMachine
    {
        #region Public Variables
        // Attributes
        [SerializeField] string name = "NPC";
        [SerializeField] float maxDistToPlayer = 50;

        // Events

        // State information

        #region Delegates
        public delegate bool BoolCheck();
        public BoolCheck HasACurrentAction;

        public BoolCheck HasACurrentWaypoint;

        public BoolCheck IsAThreatInView;

        public BoolCheck IsFearOverMax;

        public delegate int IntCheck();
        public IntCheck GetThreatsInProxNum;
        public IntCheck GetAlliesInProxNum;

        #endregion

        #region Events

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
        public bool IsNpcOutOfRangeToPlayer()
        {
            float distToPlayer = Vector3.Distance(transform.position, Player_Controller.Instance.transform.position);
            return distToPlayer > maxDistToPlayer;
        }
        #endregion

        #region FSM Methods
        protected override BaseState GetInitialState()
        {
            return spawnState;
        }
        #endregion

        #region Coroutines

        #endregion

        #region Miscellaneous

        #endregion
    }
}

