using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDamagable : MonoBehaviour, IDamagable
{
    public int maxHealth = 100;
    public int health;
    public Action damageReceived;
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public int Health { get => health; set => health = value; }
    public Action DamageReceived { get => damageReceived; set => damageReceived = value; }

    void IDamagable.DamageReceived()
    {
        throw new NotImplementedException();
    }
}
