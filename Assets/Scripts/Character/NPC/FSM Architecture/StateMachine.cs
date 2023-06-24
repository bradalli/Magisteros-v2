using UnityEngine;

namespace Brad.FSM
{
    public class StateMachine : MonoBehaviour
    {
        public BaseState currentState;

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

#if UNITY_EDITOR

        private void OnGUI()
        {
            string content = currentState != null ? currentState.name : "(no current state)";
            GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
        }

#endif
    }
}
