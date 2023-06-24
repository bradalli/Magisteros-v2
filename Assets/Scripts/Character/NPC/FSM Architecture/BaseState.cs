using UnityEngine;

namespace Brad.FSM
{
    [System.Serializable]
    public abstract class BaseState : ScriptableObject
    {
        public new string name;
        protected StateMachine stateMachine;

        public void SetValues(string stateName, StateMachine machine)
        {
            name = stateName;
            stateMachine = machine;
        }

        public virtual void Enter() { }
        public virtual void UpdateState() { }
        public virtual void Exit() { }
    }
}
