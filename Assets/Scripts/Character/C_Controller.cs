using Brad.FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Controller : StateMachine
{
    public string characterName = "NPC";
    public Transform meshT;
    public event Action E_AttackSwing;

    public void OnValidate()
    {
        #region Initialised components
        meshT = transform.Find($"{characterName}_Mesh");
        #endregion
    }

    public void Do_AttackSwing() => E_AttackSwing.Invoke();
}
