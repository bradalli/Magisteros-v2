using Brad.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_AnimManager : MonoBehaviour
{
    EventAndDataManager mang;
    Animator anim;
    private void OnEnable()
    {
        TryGetComponent<EventAndDataManager>(out mang);
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if(npcCont)
            anim.SetFloat("Forward", npcCont.Get_NavVelocity().magnitude);
    }

    bool ReturnIsCurrentStateThis(string name, int layer)
    {
        return anim.GetNextAnimatorStateInfo(layer).IsName(name);
    }

    bool ReturnIsTransToThis(string name, int layer)
    {
        return anim.GetAnimatorTransitionInfo(layer).IsName($"Move -> {name}");
    }

    bool ReturnIsTransFromThis(string name, int layer)
    {
        return anim.GetAnimatorTransitionInfo(layer).IsName($"{name} -> Move");
    }

    void SetBool(string name, bool value)
    {
        anim.SetBool(name, value);
    }

    void SetTrigger(string name)
    {
        anim.SetTrigger(name);
    }

    void ResetTrigger(string name)
    {
        anim.ResetTrigger(name);
    }
}
