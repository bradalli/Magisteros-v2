using Brad.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_AnimEvents : MonoBehaviour
{
    EventAndDataManager mang;

    private void OnEnable()
    {
        transform.parent.TryGetComponent<EventAndDataManager>(out mang);
    }

    public void AttackSwing()
    {
        if (mang)
            mang.TriggerEvent("Attack");
    }
}
