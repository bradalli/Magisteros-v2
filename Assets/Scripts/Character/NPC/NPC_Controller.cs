using Brad.FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brad.Character
{
    public class NPC_Controller : StateMachine, IDamagable
    {
        #region Public Variables
        // Attributes
        [SerializeField] string npcName = "NPC";
        public float fleeDistance = 15f;

        // Stats
        public int maxHealth = 100;
        public int health;

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
        #endregion

        #region Delegates
        public delegate bool BoolCheck();
        public BoolCheck d_IsNpcOutOfRange;
        public BoolCheck d_IsFearOverMax;

        public delegate bool AnimatorCheck(string name, int layer);
        public AnimatorCheck d_IsCurrAnimStateThis;
        public AnimatorCheck d_IsCurrAnimTransToThis;
        public AnimatorCheck d_IsCurrAnimTransFromThis;

        public delegate CharacterAction ActionCheck();
        public ActionCheck d_CurrentAction;

        public delegate Waypoint WaypointCheck();
        public WaypointCheck d_CurrentWaypoint;

        public delegate int IntCheck();
        public IntCheck d_ThreatsInProxNum;
        public IntCheck d_AlliesInProxNum;
        public IntCheck d_ThreatsInViewNum;

        public delegate float FloatCheck();
        public FloatCheck d_RemainingNavDistance;

        public delegate Vector3 Vector3Check();
        public Vector3Check d_NavVelocity;

        public delegate Collider ColliderCheck();
        public ColliderCheck d_ClosestThreatInProx;
        public ColliderCheck d_ClosestThreatInView;

        public delegate Collider[] ColliderArrayCheck();
        public ColliderArrayCheck d_ThreatsInProx;
        public ColliderArrayCheck d_ThreatsInView;

        #endregion

        #region Events
        public event Action<Vector3> E_SetNavDestination;
        public event Action<Vector3> E_LookAtPosition;
        public event Action<Waypoint> E_SetCurrWaypoint;
        public event Action<string, bool> E_SetAnimBool;
        public event Action<string> E_SetAnimTrigger;
        public event Action E_ActionEnd;
        public event Action<CharacterAction> E_NewAction;
        public event Action<string> E_ResetTrigger;
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
        private void Awake()
        {
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
            
        }
        #endregion

        #region Custom Methods


        #endregion

        #region Event invoke
        public void Set_NavDestination(Vector3 position) => E_SetNavDestination.Invoke(position);
        public void Set_LookAtPosition(Vector3 position) => E_LookAtPosition.Invoke(position);
        public void Set_CurrentWaypoint(Waypoint waypoint) => E_SetCurrWaypoint.Invoke(waypoint);
        public void Set_AnimBool(string boolName, bool value) => E_SetAnimBool.Invoke(boolName, value);
        public void Set_AnimTrigger(string triggerName) => E_SetAnimTrigger.Invoke(triggerName);
        public void Set_ActionEnd() => E_ActionEnd.Invoke();
        public void Set_NewAction(CharacterAction newAction) => E_NewAction.Invoke(newAction);
        public void Set_ResetTrigger(string name) => E_ResetTrigger.Invoke(name);
        #endregion 

        #region Delegate invoke
        public bool Get_IsNpcOutOfRange() => d_IsNpcOutOfRange.Invoke();
        public bool Get_IsFearOverMax() => d_IsFearOverMax.Invoke();
        public bool Get_IsCurrAnimStateThis(string name, int layer) => d_IsCurrAnimStateThis.Invoke(name, layer);
        public bool Get_IsCurrAnimTransToThis(string name, int layer) => d_IsCurrAnimTransToThis.Invoke(name, layer);
        public bool Get_IsCurrAnimTransFromThis(string name, int layer) => d_IsCurrAnimTransToThis.Invoke(name, layer);
        public int Get_ThreatsInProxNum() => d_ThreatsInProxNum.Invoke();
        public int Get_AlliesInProxNum() => d_AlliesInProxNum.Invoke();
        public int Get_ThreatsInViewNum() => d_ThreatsInViewNum.Invoke();
        public CharacterAction Get_CurrAction() => d_CurrentAction.Invoke();
        public Waypoint Get_CurrWaypoint() => d_CurrentWaypoint.Invoke();
        public float Get_RemainingNavDistance() => d_RemainingNavDistance.Invoke();
        public Vector3 Get_NavVelocity() => d_NavVelocity.Invoke();
        public Collider Get_ClosestThreatInProx() => d_ClosestThreatInProx.Invoke();
        public Collider Get_ClosestThreatInView() => d_ClosestThreatInView.Invoke();
        public Collider[] Get_ThreatsInProx() => d_ThreatsInProx.Invoke();
        public Collider[] Get_ThreatsInView() => d_ThreatsInView.Invoke();
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

