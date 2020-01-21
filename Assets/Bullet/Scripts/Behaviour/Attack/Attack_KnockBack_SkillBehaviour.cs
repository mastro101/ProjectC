using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_KnockBack_SkillBehaviour : BaseSkillBehaviour
{
    [SerializeField] float knockbackForce;

    protected override void OnDamage(IDamageable _damageable)
    {
        base.OnDamage(_damageable);
        _damageable.KnockBack(knockbackForce, skill.transform.position);
    }
}