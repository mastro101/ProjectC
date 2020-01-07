using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMPCollideDetection : MonoBehaviour
{
    BulletBase bullet;

    private void Awake()
    {
        bullet = GetComponentInParent<BulletBase>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bullet.state == BulletBase.State.Shooted)
        {
            IDamageable damageable = other.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                if (damageable.gameObject != bullet.shooter.gameObject)
                    bullet.OnDamageableCollide(damageable);
            }
        }
    }
}