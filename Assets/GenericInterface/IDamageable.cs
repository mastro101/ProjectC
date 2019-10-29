using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    GameObject gameObject { get; }

    int currentHealth { get; set; }
    int maxHealth { get; set; }
    bool isDead { get; set; }

    System.Action<int> OnDamage { get; set; }

    void TakeDamage(int _damage);
}
