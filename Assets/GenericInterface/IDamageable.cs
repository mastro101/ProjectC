using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    GameObject gameObject { get; }
    Transform transform { get; }
    Rigidbody myRigidbody { get; }

    int currentHealth { get; set; }
    int maxHealth { get; set; }
    bool isDead { get; set; }
    bool invulnerable { get; set; }

    System.Action<int> OnDamage { get; set; }
    System.Action<IDamageable> OnDeath { get; set; }

    void TakeDamage(int _damage);
    void Stun(float _duration);
    void KnockBack(float _force);
    void TempInvulnerability(float _duration);
}