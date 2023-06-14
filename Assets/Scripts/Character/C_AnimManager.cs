using Brad.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_AnimManager : MonoBehaviour
{
    #region Private variables

    IEventAndDataHandler _handler;
    Animator anim;
    float animSpeed;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        // Cache components
        TryGetComponent(out _handler);
        anim = GetComponentInChildren<Animator>();

        // Only continue if component(s) are found
        if (_handler != null)
        {
            // Refresh data initialisation
            _handler.AddEvent("Set_AS_CurrAnimState", Set_CurrAnimState);

            // Event initialisation
            _handler.AddEvent("Start_Attack", Trigger_AnimAttack);
            _handler.AddEvent("Respawn", Trigger_AnimRespawn);
            _handler.AddEvent("Die", Trigger_AnimDead);
        }
    }

    private void Update()
    {
        if(_handler != null)
        {
            // Set the anim float to drive the blend tree (to make the character run)
            anim.SetFloat("Forward", _handler.GetValue<Vector3>("V_Velocity").magnitude);

            // #CONSIDER# - Allowing strafing, not just moving forward!
        }
    }

    #endregion

    #region Custom methods

    #region Refresh data methods

    void Set_CurrAnimState() => _handler.SetValue("AS_CurrAnimState", anim.GetCurrentAnimatorStateInfo(0));

    #endregion

    #region Event methods

    void Trigger_AnimAttack()
    {
        _handler.SetValue("B_Attacking", true);
        anim.SetTrigger("Attack");
    }
    void Trigger_AnimRespawn() => anim.SetTrigger("Respawn");
    void Trigger_AnimDead() => anim.SetTrigger("Die");

    #endregion

    #endregion
}
