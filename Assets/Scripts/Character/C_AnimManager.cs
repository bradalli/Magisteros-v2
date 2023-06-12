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

    private void OnEnable()
    {
        // Cache components
        TryGetComponent(out _handler);
        TryGetComponent(out anim);

        // Only continue if component(s) are found
        if (_handler != null)
        {
            // Refresh data initialisation
            _handler.AddEvent("Set_AS_CurrAnimState", Set_CurrAnimState);

            // Event initialisation
            _handler.AddEvent("AttackHit", Trigger_AnimAttack);
            _handler.AddEvent("Respawn", Trigger_AnimRespawn);
            _handler.AddEvent("Die", Trigger_AnimDead);
        }
    }

    private void Update()
    {
        if(_handler != null)
        {
            // Set the anim float to drive the blend tree (to make the character run)
            _handler.TriggerEvent("Refresh_VelocityV");
            anim.SetFloat("Forward", _handler.GetValue<Vector3>("VelocityV").magnitude);

            // #CONSIDER# - Allowing strafing, not just moving forward!
        }
    }

    #endregion

    #region Custom methods

    #region Refresh data methods

    void Set_CurrAnimState() => _handler.SetValue("AS_CurrAnimState", anim.GetCurrentAnimatorStateInfo(0));

    #endregion

    #region Event methods

    void Trigger_AnimAttack() => anim.SetTrigger("Attack");
    void Trigger_AnimRespawn() => anim.SetBool("Dead", false);
    void Trigger_AnimDead() => anim.SetBool("Dead", true);

    #endregion

    #endregion
}
