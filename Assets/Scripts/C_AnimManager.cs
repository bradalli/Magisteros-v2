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
            }
        }
    }

    void SetBool(string name, bool value)
    {
        anim.SetBool(name, value);
    }

    void SetTrigger(string name)
    {
        anim.SetTrigger(name);
    }
}
