using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Brad.Character
{
    public class Player_Controller : MonoBehaviour, IDamagable
    {
        public static Player_Controller Instance;

        [SerializeField] int maxHealth = 100;
        [SerializeField] int health;

        [SerializeField] float lookSpeed;

        private Transform meshT;
        private NavMeshAgent navAgent;
        private Animator anim;

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
        #endregion

        private void Awake()
        {
            Instance = this;
            health = maxHealth;
            TryGetComponent(out navAgent);
            anim = GetComponentInChildren<Animator>();
            meshT = transform.Find("Player_Mesh");
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            Vector3 forwardVector = Camera.main.transform.forward * Input.GetAxis("Vertical");
            Vector3 rightVector = Camera.main.transform.right * Input.GetAxis("Horizontal");

            Vector3 moveVector = forwardVector + rightVector;
            moveVector = new Vector3(moveVector.x, 0, moveVector.z);

            if (navAgent)
                navAgent.Move(moveVector * navAgent.speed * Time.deltaTime);

            anim.SetFloat("Forward", moveVector.magnitude * navAgent.speed);

            Vector3 targetLookDir = Vector3.RotateTowards(meshT.forward, moveVector, 
                lookSpeed * Time.deltaTime, 0);
            meshT.rotation = Quaternion.LookRotation(targetLookDir, meshT.up);
        }
    }
}

