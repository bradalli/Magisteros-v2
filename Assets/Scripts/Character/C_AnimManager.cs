using Brad.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_AnimManager : MonoBehaviour
{
    NPC_Controller npcCont;
    Animator anim;
    private void OnEnable()
    {
        TryGetComponent<NPC_Controller>(out npcCont);
        anim = GetComponentInChildren<Animator>();
        if(npcCont != null)
        {
            if (anim != null)
            {
                npcCont.E_SetAnimBool += SetBool;
                npcCont.E_SetAnimTrigger += SetTrigger;
                npcCont.d_IsCurrAnimStateThis += ReturnIsCurrentStateThis;
                npcCont.d_IsCurrAnimTransToThis += ReturnIsTransToThis;
                npcCont.d_IsCurrAnimTransFromThis += ReturnIsTransFromThis;
                npcCont.E_ResetTrigger += ResetTrigger;
            }
        }
    }

    private void Update()
    {
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
