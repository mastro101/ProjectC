using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBullet : BulletBase
{
    [SerializeField] float speed;
    [SerializeField] float knockbackForce;

    public override void Shoot(Vector3 shootPosition, Vector3 direction, GameObject _shootable)
    {
        base.Shoot(shootPosition, direction, _shootable);
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    protected override void Tick()
    {
        base.Tick();
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }

    public override void OnDamageableCollide(IDamageable damageable)
    {
        base.OnDamageableCollide(damageable);
        Vector3 knockbackDirection = (transform.position - damageable.transform.position) * -1;
        knockbackDirection.Normalize();
        damageable.myRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode.VelocityChange);
        Return();
    }
}