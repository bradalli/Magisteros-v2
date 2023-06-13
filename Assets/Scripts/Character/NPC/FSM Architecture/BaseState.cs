using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Brad.FSM
{
    [System.Serializable]
    public abstract class BaseState : ScriptableObject
    {
        public new string name;
        protected StateMachine stateMachine;

        public BaseState(string name, StateMachine stateMachine)
        {
            this.name = name;
            this.stateMachine = stateMachine;
        }

        public virtual void Enter() { }
        public virtual void UpdateState() { }
        public virtual void Exit() { }
    }
}
