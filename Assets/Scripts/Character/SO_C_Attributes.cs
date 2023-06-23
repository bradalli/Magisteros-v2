using Brad.FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[System.Serializable]
[CreateAssetMenu(fileName = "Character Attributes", 
    menuName = "Character/Character Attributes", order = 1)]
public class SO_C_Attributes : ScriptableObject
{
    [System.Serializable]
    public struct Attribute
    {
        public string name;
        public enum variableType { Type_String, Type_Float, Type_Int }
        public variableType type;
        public string value;
        [TextAreaAttribute]
        public string description;
    }

    [System.Serializable]
    public struct State
    {
        public string name;
        public Object stateClass;
    }

    public Attribute[] attributes;
    public State[] states;
    public Object meshPrefab;
}
