using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brad.Character
{
    public class Player_Controller : MonoBehaviour, IDamagable
    {
        public static Player_Controller Instance;

        int maxHealth = 100;
        int health;

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
        }
    }
}

