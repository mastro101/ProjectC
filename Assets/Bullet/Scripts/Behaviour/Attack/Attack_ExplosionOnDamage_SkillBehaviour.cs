using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_ExplosionOnDamage_SkillBehaviour : BaseSkillBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float radius;
    [SerializeField] float knockbackForce;
    [SerializeField] bool friendlyFire;

    protected override void OnDamage(IDamageable _damageable)
    {
        base.OnDamage(_damageable);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < colliders.Length; i++)
        {
            IDamageable damageable = colliders[i].GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                if (!friendlyFire && damageable == skill.shooter)
                    continue;

                damageable.TakeDamage(damage);
                damageable.KnockBack(knockbackForce, transform.position);
            }
        }
    }
}
