using Brad.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_AnimEvents : MonoBehaviour
{
    IEventAndDataHandler _handler;

    private void OnEnable()
    {
        transform.parent.TryGetComponent(out _handler);

        if(_handler != null)
        {
            _handler.SetValue("IsAttackingB", false);
        }
    }

    public void AttackSwing()
    {
        if (_handler != null)
            _handler.TriggerEvent("Do_AttackSwing");
    }

    public void Set_IsAttackingTrue()
    {
        if (_handler != null)
        {
            _handler.SetValue("IsAttackingB", true);
        }
    }

    public void Set_IsAttackingFalse()
    {
        if (_handler != null)
        {
            _handler.SetValue("IsAttackingB", false);
        }
    }
}
