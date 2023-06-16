using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Brad.Character
{
    public class Player_Controller : MonoBehaviour, IDamagable, IEventAndDataHandler
    {
        public static Player_Controller Instance;

        bool dead = false;

        [SerializeField] int maxHealth = 100;
        [SerializeField] int health;

        [SerializeField] float lookSpeed;
        [SerializeField] int damage = 25;

        private Transform meshT;
        private NavMeshAgent navAgent;
        private Animator anim;
        public IEventAndDataHandler handler;

        #region interface instance properties
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

        public Dictionary<string, object> data;
        public Dictionary<string, Action> events;
        public Action damageReceived;
        public Dictionary<string, object> DataDictionary { get => data; set => data = value; }
        public Dictionary<string, Action> EventDictionary { get => events; set => events = value; }
        #endregion

        private void OnEnable()
        {
            TryGetComponent(out handler);
            handler.EventDictionary = new Dictionary<string, Action>();
            handler.DataDictionary = new Dictionary<string, object>();

            Instance = this;
            health = maxHealth;
            TryGetComponent(out navAgent);
            
            anim = GetComponentInChildren<Animator>();
            meshT = transform.Find("Player_Mesh");

            handler.SetValue("I_MaxHealth", maxHealth);
            handler.SetValue("T_Mesh", meshT);
            handler.SetValue("I_Damage", damage);
            handler.AddEvent("DamageReceived", damageReceived);
            handler.AddEvent("DamageReceived", UpdateHealthValue);

            UpdateHealthValue();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (!dead)
            {
                Move();
                Attack();
            }

            if(!dead && health <= 0)
            {
                DisableC();
                handler.TriggerEvent("Die");
                dead = true;
            }
        }

        void UpdateHealthValue()
        {
            handler.SetValue("I_Health", health);
        }

        void Attack()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if(!handler.GetValue<bool>("B_Attacking"))
                    handler.TriggerEvent("Start_Attack");
            }
        }

        void Move()
        {
            Vector3 forwardVector = Camera.main.transform.forward * Input.GetAxis("Vertical");
            Vector3 rightVector = Camera.main.transform.right * Input.GetAxis("Horizontal");

            Vector3 moveVector = forwardVector + rightVector;
            moveVector = new Vector3(moveVector.x, 0, moveVector.z);

            if (navAgent)
                navAgent.SetDestination(transform.position + moveVector);

            handler.SetValue("V_Velocity", moveVector * navAgent.speed);

            Vector3 targetLookDir = Vector3.RotateTowards(meshT.forward, moveVector,
                lookSpeed * Time.deltaTime, 0);
            meshT.rotation = Quaternion.LookRotation(targetLookDir, meshT.up);
        }

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

        void DisableC()
        {
            foreach (Behaviour comp in gameObject.GetComponents<Behaviour>())
            {
                if (comp != this)
                {
                    comp.enabled = false;
                }
            }
        }

        void IDamagable.DamageReceived()
        {
            handler.TriggerEvent("DamageReceived");
        }
    }
}

