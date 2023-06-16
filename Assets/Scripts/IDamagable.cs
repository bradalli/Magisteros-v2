using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public int MaxHealth { get; set; }
    public int Health { get; set; }

    public void Damage(int damage)
    {
        if (!IsHealthDepleted())
        {
            Health -= damage;
            DamageReceived();
        }
    }

    void DamageReceived();

    public void Heal(int heal)
    {
        if (!IsHealthDepleted())
            Health += heal;
    }
    public void ResetHealth()
    {
        Health = MaxHealth;
    }
    public bool IsHealthDepleted()
    {
        return Health <= 0;
    }
}
