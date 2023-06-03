using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction
{
    public string name;
    public Vector3 position;
    public enum state { Starting, Running, Complete }
    public state actionState = state.Starting;

    public virtual void Start() 
    {
        actionState = state.Starting;
    } 

    public virtual void UpdateAction() 
    {
        actionState = state.Running;
    }

    public virtual void Stop() 
    {
        actionState = state.Complete;
    }
}
