using Brad.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_AnimEvents : MonoBehaviour
{
    IEventAndDataHandler _handler;

    private void Start()
    {
        transform.parent.TryGetComponent(out _handler);
    }

    public void AttackSwing()
    {
        if (_handler != null)
            _handler.TriggerEvent("AttackHit");
    }

    public void Set_IsAttackingTrue()
    {
        if (_handler != null)
        {
            _handler.TriggerEvent("Start_Attack");
        }
    }

    public void Set_IsAttackingFalse()
    {
        if (_handler != null)
        {
            _handler.TriggerEvent("Stop_Attack");
        }
    }
}
