using Brad.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_AnimEvents : MonoBehaviour
{
    NPC_Controller npcCont;

    private void OnEnable()
    {
        transform.parent.TryGetComponent<NPC_Controller>(out npcCont);
    }

    public void AttackSwing()
    {
        if (npcCont)
            npcCont.Do_AttackSwing();
    }
}
