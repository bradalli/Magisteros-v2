using Brad.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Attributes", menuName = "Character/Character Attributes", order = 1)]
public class SO_C_Attributes : ScriptableObject
{
    public struct Attribute
    {
        public string name;
        public enum variableType { Type_String, Type_Float, Type_Int }
        public variableType type;
        public string value;
        public string description;
    }

    public struct State
    {
        public string name;
        public BaseState stateClass;
    }

    public Attribute[] attributes;
    public State[] states;
    public object meshPrefab;
}
