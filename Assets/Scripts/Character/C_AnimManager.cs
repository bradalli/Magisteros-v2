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
            _handler.AddEvent("Refresh_BaseAnimState", Refresh_BaseAnimState);

            // Event initialisation
            _handler.AddEvent("Trigger_AnimAttack", Trigger_AnimAttack);
            _handler.AddEvent("Trigger_AnimRespawn", Trigger_AnimRespawn);
        }
    }

    private void OnDisable()
    {
        // Only continue if component(s) are found
        if (_handler != null)
        {
            // Refresh data removal
            _handler.RemoveEvent("Refresh_BaseAnimState", Refresh_BaseAnimState);

            // Event removal
            _handler.RemoveEvent("Do_AttackSwing", Trigger_AnimAttack);
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

    void Refresh_BaseAnimState() => _handler.SetValue("CurrAnimState", anim.GetCurrentAnimatorStateInfo(0));

    #endregion

    #region Event methods

    void Trigger_AnimAttack() => anim.SetTrigger("tAttack");
    void Trigger_AnimRespawn() => anim.SetTrigger("tRespawn");

    #endregion

    #endregion
}
