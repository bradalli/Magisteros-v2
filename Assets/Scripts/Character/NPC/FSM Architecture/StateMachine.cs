using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brad.FSM
{
    public class StateMachine : MonoBehaviour
    {
        public BaseState currentState;

        /*
        void Start()
        {
            currentState = GetInitialState();
            if (currentState != null)
                currentState.Enter();
        }*/

        public void Update()
        {
            if (currentState != null)
                currentState.UpdateState();
        }

        public void ChangeState(BaseState newState)
        {
            currentState.Exit();

            currentState = newState;
            currentState.Enter();
        }

        protected virtual BaseState GetInitialState()
        {
            return null;
        }

        private void OnGUI()
        {
            string content = currentState != null ? currentState.name : "(no current state)";
            GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
        }
    }
}
