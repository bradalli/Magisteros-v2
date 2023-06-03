using Brad.Character;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class C_Combat : MonoBehaviour
{
    int damage = 5;
    NPC_Controller npcCont;

    private void OnEnable()
    {
        TryGetComponent(out npcCont);

        if (npcCont != null)
        {
            npcCont.E_AttackSwing += AttackCast;
        }
    }

    void AttackCast()
    {
        List<Collider> hitColliders = Physics.OverlapSphere(transform.position + (transform.forward.normalized * 1.5f), 1).ToList<Collider>();
        TryGetComponent<Collider>(out Collider myCol);
        if (hitColliders.Contains(myCol))
            hitColliders.Remove(myCol);

        foreach(Collider target in hitColliders)
        {
            target.TryGetComponent<IDamagable>(out IDamagable targetDmg);
            if (targetDmg != null)
                targetDmg.Damage(damage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (transform.forward.normalized * 1.5f), 1);
    }
}
