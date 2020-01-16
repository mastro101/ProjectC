using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBullet : BulletBase
{
    [SerializeField] float speed;
    [SerializeField] float knockbackForce;
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionKnockbackForce;
    [SerializeField] int explosionDamage;

    public override void Shoot(Vector3 shootPosition, Vector3 direction, IShooter _shooter)
    {
        base.Shoot(shootPosition, direction, _shooter);
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z), Vector3.up);
    }

    protected override void Tick()
    {
        base.Tick();
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }

    public override void OnDamageableCollide(IDamageable damageable)
    {
        base.OnDamageableCollide(damageable);
        damageable.KnockBack(knockbackForce);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        for (int i = 0; i < colliders.Length; i++)
        {
            IDamageable _damageable = colliders[i].GetComponentInParent<IDamageable>();
            if (_damageable != null)
            {
                if (_damageable.gameObject == shooter.gameObject)
                    continue;

                _damageable.TakeDamage(explosionDamage);
                _damageable.myRigidbody.AddExplosionForce(explosionKnockbackForce, transform.position, explosionRadius, 0f, ForceMode.Impulse);
            }
        }
        Return();
    }
}