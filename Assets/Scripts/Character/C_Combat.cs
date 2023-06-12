using Brad.Character;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class C_Combat : MonoBehaviour
{
    #region Private variables

    IEventAndDataHandler _handler;
    
    int damage = 5;

    Transform meshT;

    #endregion

    #region MonoBehaviour

    private void OnEnable()
    {
        // Cache components
        TryGetComponent(out _handler);

        // Only continue if component(s) are found
        if (_handler != null)
        {
            // Refresh data initialisation GET
            _handler.AddEvent("Get_I_Damage", Get_Damage);
            _handler.AddEvent("Get_T_Mesh", Get_Mesh);

            // Event initialisation
            _handler.AddEvent("AttackHit", AttackCast);
        }
    }

    private void OnDrawGizmos()
    {
        if (_handler != null && meshT != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + (meshT.forward.normalized * 1.5f), 1);
        }
    }

    #endregion

    #region Custom methods

    #region Refresh data methods

    void Get_Damage() => damage = _handler.GetValue<int>("I_Damage");
    void Get_Mesh() => meshT = _handler.GetValue<Transform>("T_Mesh");

    #endregion

    #region Event methods

    void AttackCast()
    {
        // Create a list of targets that are found within an overlap sphere
        List<Collider> hitColliders = 
            Physics.OverlapSphere(transform.position + (meshT.forward.normalized * 1.5f), 1).ToList();
        TryGetComponent(out Collider myCol);

        // Exclude this character's collider if found within the list
        if (hitColliders.Contains(myCol))
            hitColliders.Remove(myCol);

        // For each target found, apply damage to it using the IDamagable interface
        foreach(Collider target in hitColliders)
        {
            // Cache the target's IDamagable interface
            target.TryGetComponent(out IDamagable targetDmg);

            // Only apply damage if a IDamagable component is found
            if (targetDmg != null)
                targetDmg.Damage(damage);
        }
    }

    #endregion

    #endregion
}
